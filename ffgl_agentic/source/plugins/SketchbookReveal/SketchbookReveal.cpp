#include "SketchbookReveal.h"
#include <cmath>
#include <ctime>

// Plugin metadata
static CFFGLPluginInfo PluginInfo(
    SketchbookReveal::CreateInstance,    // Create method
    "SKBR",                               // Plugin unique ID (4 chars)
    "Sketchbook Reveal",                  // Plugin name
    2,                                    // API major version
    1,                                    // API minor version  
    1,                                    // Plugin major version
    0,                                    // Plugin minor version
    FF_EFFECT,                            // Plugin type
    "Content-aware sketch reveal transition with edge detection and hatching",
    "by eyelanoesis (AI-assisted)"
);

// Vertex shader (passthrough)
static const char* vertexShaderSource = R"(
#version 410 core
layout(location = 0) in vec2 position;
out vec2 fragCoord;

void main() {
    fragCoord = position * 0.5 + 0.5;
    gl_Position = vec4(position, 0.0, 1.0);
}
)";

// Fragment shader (ported from ISF)
static const char* fragmentShaderSource = R"(
#version 410 core
uniform sampler2D inputTexture;
uniform vec2 resolution;
uniform float time;
uniform float progress;
uniform float edgeGain;
uniform float lumaHatch;
uniform float hatchIntensity;
uniform float density;
uniform float jitter;
uniform float overlapDuration;
uniform float overlapSoftness;
uniform float animSpeed;

in vec2 fragCoord;
out vec4 fragColor;

float hash(vec2 p) { 
    return fract(sin(dot(p, vec2(23.3, 91.7))) * 7241.155); 
}

float getLuma(vec4 c) { 
    return dot(c.rgb, vec3(0.299, 0.587, 0.114)); 
}

vec2 pixelSize() { 
    return 1.0 / resolution; 
}

float sobel(vec2 uv) {
    vec2 px = pixelSize();
    float tl = getLuma(texture(inputTexture, uv + px*vec2(-1,-1)));
    float  l = getLuma(texture(inputTexture, uv + px*vec2(-1, 0)));
    float bl = getLuma(texture(inputTexture, uv + px*vec2(-1, 1)));
    float  t = getLuma(texture(inputTexture, uv + px*vec2( 0,-1)));
    float  c = getLuma(texture(inputTexture, uv));
    float  b = getLuma(texture(inputTexture, uv + px*vec2( 0, 1)));
    float tr = getLuma(texture(inputTexture, uv + px*vec2( 1,-1)));
    float  r = getLuma(texture(inputTexture, uv + px*vec2( 1, 0)));
    float br = getLuma(texture(inputTexture, uv + px*vec2( 1, 1)));
    float gx = -tl - 2.0*l - bl + tr + 2.0*r + br;
    float gy = -tl - 2.0*t - tr + bl + 2.0*b + br;
    float edgeMag = sqrt(gx*gx + gy*gy);
    return clamp(edgeMag * 1.4, 0.0, 1.0);
}

float sketch(vec2 uv, float rot, float dens, float intensity, float jit, float prog, float modulator) {
    float angle = rot + 0.3*sin(prog*3.1416 + rot);
    float base = uv.x*cos(angle) + uv.y*sin(angle);
    float j = (hash(uv*47.9 + base) - 0.5) * jit;
    float hatch = sin(base * dens + j*8.0 + prog*3.1 + hash(uv*99.1)*4.0);
    float g = smoothstep(-0.35, 0.38, hatch);
    float pFill = smoothstep(0.29 - modulator*0.25, 0.91 + modulator*0.43, prog);
    return g * intensity * modulator * pFill;
}

void main() {
    vec2 uv = fragCoord;
    float t = time * animSpeed;

    float edge = sobel(uv);
    float luma = getLuma(texture(inputTexture, uv));
    float hatchMod = 1.0 - luma * lumaHatch;

    float edgeReveal = smoothstep(0.14, 0.72, progress - edgeGain*(1.0-edge));

    float hatchA = sketch(uv + t*0.021,  0.8, density, hatchIntensity, jitter, progress, hatchMod*edge);
    float hatchB = sketch(uv - t*0.013, -0.7, density, hatchIntensity, jitter*1.5, progress, hatchMod*edge*0.8);
    float hatchC = sketch(uv + t*0.017,  0.0, density*0.5, hatchIntensity*0.7, jitter*0.77, progress, hatchMod*0.4);

    float hatchSum = clamp(hatchA + hatchB + hatchC, 0.0, 1.0);
    float maskVal = clamp(edgeReveal + hatchSum, 0.0, 1.0);

    float remapped = smoothstep(0.0, overlapDuration, progress);
    float edge0 = remapped - overlapSoftness;
    float edge1 = remapped + overlapSoftness;
    float mask = smoothstep(edge0, edge1, maskVal);

    vec4 inputColor = texture(inputTexture, uv);
    fragColor = mix(vec4(0.0), inputColor, mask);
}
)";

SketchbookReveal::SketchbookReveal()
    : m_shaderProgram(0)
    , m_vertexShader(0)
    , m_fragmentShader(0)
    , m_vao(0)
    , m_vbo(0)
    , m_timeCounter(0.0)
{
    // Set parameter info
    SetMinInputs(1);
    SetMaxInputs(1);

    SetParamInfo(PARAM_PROGRESS, "Progress", FF_TYPE_STANDARD, 0.0f);
    SetParamInfo(PARAM_EDGE_GAIN, "Edge Gain", FF_TYPE_STANDARD, 0.47f); // 1.4/3.0
    SetParamInfo(PARAM_LUMA_HATCH, "Luma Hatch", FF_TYPE_STANDARD, 0.27f); // 0.8/3.0
    SetParamInfo(PARAM_HATCH_INTENSITY, "Hatch Intensity", FF_TYPE_STANDARD, 0.5f);
    SetParamInfo(PARAM_DENSITY, "Density", FF_TYPE_STANDARD, 0.35f); // (35-5)/(90-5)
    SetParamInfo(PARAM_JITTER, "Jitter", FF_TYPE_STANDARD, 0.25f);
    SetParamInfo(PARAM_OVERLAP_DURATION, "Overlap Duration", FF_TYPE_STANDARD, 0.45f);
    SetParamInfo(PARAM_OVERLAP_SOFTNESS, "Overlap Softness", FF_TYPE_STANDARD, 0.36f); // 0.18/0.5
    SetParamInfo(PARAM_ANIM_SPEED, "Anim Speed", FF_TYPE_STANDARD, 0.09f);

    // Initialize parameters with defaults
    for (int i = 0; i < PARAM_COUNT; i++) {
        m_parameters[i] = GetParamInfo(i)->m_defaultValue;
    }
}

FFResult SketchbookReveal::InitGL(const FFGLViewportStruct *vp) {
    // Compile shaders
    m_vertexShader = glCreateShader(GL_VERTEX_SHADER);
    m_fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);

    if (!CompileShader(m_vertexShader, vertexShaderSource) ||
        !CompileShader(m_fragmentShader, fragmentShaderSource)) {
        return FF_FAIL;
    }

    // Create and link program
    m_shaderProgram = glCreateProgram();
    glAttachShader(m_shaderProgram, m_vertexShader);
    glAttachShader(m_shaderProgram, m_fragmentShader);
    
    if (!LinkProgram()) {
        return FF_FAIL;
    }

    // Get uniform locations
    m_inputTextureLoc = glGetUniformLocation(m_shaderProgram, "inputTexture");
    m_resolutionLoc = glGetUniformLocation(m_shaderProgram, "resolution");
    m_timeLoc = glGetUniformLocation(m_shaderProgram, "time");
    m_progressLoc = glGetUniformLocation(m_shaderProgram, "progress");
    m_edgeGainLoc = glGetUniformLocation(m_shaderProgram, "edgeGain");
    m_lumaHatchLoc = glGetUniformLocation(m_shaderProgram, "lumaHatch");
    m_hatchIntensityLoc = glGetUniformLocation(m_shaderProgram, "hatchIntensity");
    m_densityLoc = glGetUniformLocation(m_shaderProgram, "density");
    m_jitterLoc = glGetUniformLocation(m_shaderProgram, "jitter");
    m_overlapDurationLoc = glGetUniformLocation(m_shaderProgram, "overlapDuration");
    m_overlapSoftnessLoc = glGetUniformLocation(m_shaderProgram, "overlapSoftness");
    m_animSpeedLoc = glGetUniformLocation(m_shaderProgram, "animSpeed");

    // Setup fullscreen quad
    SetupQuad();

    return FF_SUCCESS;
}

FFResult SketchbookReveal::DeInitGL() {
    if (m_shaderProgram) glDeleteProgram(m_shaderProgram);
    if (m_vertexShader) glDeleteShader(m_vertexShader);
    if (m_fragmentShader) glDeleteShader(m_fragmentShader);
    if (m_vao) glDeleteVertexArrays(1, &m_vao);
    if (m_vbo) glDeleteBuffers(1, &m_vbo);
    return FF_SUCCESS;
}

FFResult SketchbookReveal::ProcessOpenGL(ProcessOpenGLStruct *pGL) {
    if (!pGL || !pGL->numInputTextures) return FF_FAIL;

    glUseProgram(m_shaderProgram);

    // Bind input texture
    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, pGL->inputTextures[0]->Handle);
    glUniform1i(m_inputTextureLoc, 0);

    // Set uniforms
    glUniform2f(m_resolutionLoc, 
                (float)pGL->inputTextures[0]->Width, 
                (float)pGL->inputTextures[0]->Height);
    
    m_timeCounter += 1.0 / 60.0; // Assume 60fps, adjust as needed
    glUniform1f(m_timeLoc, (float)m_timeCounter);

    // Map 0-1 parameters to shader ranges
    glUniform1f(m_progressLoc, m_parameters[PARAM_PROGRESS]);
    glUniform1f(m_edgeGainLoc, m_parameters[PARAM_EDGE_GAIN] * 3.0f);
    glUniform1f(m_lumaHatchLoc, m_parameters[PARAM_LUMA_HATCH] * 3.0f);
    glUniform1f(m_hatchIntensityLoc, m_parameters[PARAM_HATCH_INTENSITY]);
    glUniform1f(m_densityLoc, m_parameters[PARAM_DENSITY] * 85.0f + 5.0f);
    glUniform1f(m_jitterLoc, m_parameters[PARAM_JITTER]);
    glUniform1f(m_overlapDurationLoc, m_parameters[PARAM_OVERLAP_DURATION] * 0.9f + 0.1f);
    glUniform1f(m_overlapSoftnessLoc, m_parameters[PARAM_OVERLAP_SOFTNESS] * 0.5f);
    glUniform1f(m_animSpeedLoc, m_parameters[PARAM_ANIM_SPEED]);

    // Render fullscreen quad
    glBindVertexArray(m_vao);
    glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);
    glBindVertexArray(0);

    glUseProgram(0);
    return FF_SUCCESS;
}

FFResult SketchbookReveal::SetFloatParameter(unsigned int index, float value) {
    if (index < PARAM_COUNT) {
        m_parameters[index] = value;
        return FF_SUCCESS;
    }
    return FF_FAIL;
}

float SketchbookReveal::GetFloatParameter(unsigned int index) {
    return (index < PARAM_COUNT) ? m_parameters[index] : 0.0f;
}

bool SketchbookReveal::CompileShader(GLuint shader, const char* source) {
    glShaderSource(shader, 1, &source, nullptr);
    glCompileShader(shader);

    GLint success;
    glGetShaderiv(shader, GL_COMPILE_STATUS, &success);
    if (!success) {
        char log[512];
        glGetShaderInfoLog(shader, 512, nullptr, log);
        // Log error (in production, use proper logging)
        return false;
    }
    return true;
}

bool SketchbookReveal::LinkProgram() {
    glLinkProgram(m_shaderProgram);

    GLint success;
    glGetProgramiv(m_shaderProgram, GL_LINK_STATUS, &success);
    if (!success) {
        char log[512];
        glGetProgramInfoLog(m_shaderProgram, 512, nullptr, log);
        return false;
    }
    return true;
}

void SketchbookReveal::SetupQuad() {
    float quadVertices[] = {
        -1.0f,  1.0f,
        -1.0f, -1.0f,
         1.0f,  1.0f,
         1.0f, -1.0f
    };

    glGenVertexArrays(1, &m_vao);
    glGenBuffers(1, &m_vbo);

    glBindVertexArray(m_vao);
    glBindBuffer(GL_ARRAY_BUFFER, m_vbo);
    glBufferData(GL_ARRAY_BUFFER, sizeof(quadVertices), quadVertices, GL_STATIC_DRAW);

    glVertexAttribPointer(0, 2, GL_FLOAT, GL_FALSE, 2 * sizeof(float), (void*)0);
    glEnableVertexAttribArray(0);

    glBindVertexArray(0);
}
