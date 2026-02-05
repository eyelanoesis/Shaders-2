# ISF to FFGL Conversion Reference

Quick reference for converting ISF shaders (Shaders-2) to FFGL plugins (FFGL_agentic).

## Syntax Differences

### Texture Sampling
```glsl
// ISF
vec4 color = IMG_NORM_PIXEL(inputImage, uv);

// FFGL
uniform sampler2D inputTexture;
vec4 color = texture(inputTexture, uv);
```

### Resolution
```glsl
// ISF
vec2 res = RENDERSIZE;

// FFGL
uniform vec2 resolution;
vec2 res = resolution;
```

### Time
```glsl
// ISF
float t = TIME;

// FFGL
uniform float time;
float t = time;
```

### Normalized Coordinates
```glsl
// ISF
vec2 uv = isf_FragNormCoord;

// FFGL
vec2 uv = gl_FragCoord.xy / resolution;
```

### Fragment Output
```glsl
// ISF
gl_FragColor = vec4(color, 1.0);

// FFGL
gl_FragColor = vec4(color, 1.0);  // Same
// Or for newer GLSL versions:
out vec4 fragColor;
fragColor = vec4(color, 1.0);
```

## Parameter Mapping

### ISF JSON to FFGL C++

**ISF:**
```json
{
  "NAME": "hatchDensity",
  "TYPE": "float",
  "MIN": 5.0,
  "MAX": 90.0,
  "DEFAULT": 35.0,
  "LABEL": "Hatch Density"
}
```

**FFGL:**
```cpp
// In header file (.h)
enum ParamType : FFUInt32 {
    PARAM_HATCH_DENSITY = 0
};

// In plugin constructor
SetParamInfo(PARAM_HATCH_DENSITY, "Hatch Density", FF_TYPE_STANDARD, 0.5f);
SetParamRange(PARAM_HATCH_DENSITY, 5.0f, 90.0f);

// In shader
uniform float hatchDensity;  // Passed from C++ code
```

### Parameter Type Conversions

| ISF Type | FFGL Type | Notes |
|----------|-----------|-------|
| `float` | `FF_TYPE_STANDARD` | Normalized 0-1, scale in shader |
| `float` | `FF_TYPE_FLOAT` | Absolute float value |
| `bool` | `FF_TYPE_BOOLEAN` | True/false toggle |
| `long` (options) | `FF_TYPE_OPTION` | Dropdown selection |
| `point2D` | Two `FF_TYPE_STANDARD` params | Separate X and Y |
| `color` | `FF_TYPE_RED/GREEN/BLUE` | Three separate RGB params |
| `image` | Input texture | Use `SetInputInfo()` |

### Boolean Parameters

**ISF:**
```json
{
  "NAME": "enableEffect",
  "TYPE": "bool",
  "DEFAULT": 1
}
```

**FFGL:**
```cpp
SetParamInfo(PARAM_ENABLE_EFFECT, "Enable Effect", FF_TYPE_BOOLEAN, 1.0f);

// In shader
uniform bool enableEffect;
```

### Option/Enum Parameters

**ISF:**
```json
{
  "NAME": "blendMode",
  "TYPE": "long",
  "VALUES": ["Normal", "Add", "Multiply"],
  "DEFAULT": 0
}
```

**FFGL:**
```cpp
SetParamInfo(PARAM_BLEND_MODE, "Blend Mode", FF_TYPE_OPTION, 0.0f);
SetOptionParamInfo(PARAM_BLEND_MODE, 3, "Normal", "Add", "Multiply");

// In shader
uniform int blendMode;  // 0, 1, or 2
```

### Color Parameters

**ISF:**
```json
{
  "NAME": "tintColor",
  "TYPE": "color",
  "DEFAULT": [1.0, 1.0, 1.0, 1.0]
}
```

**FFGL:**
```cpp
SetParamInfo(PARAM_TINT_RED, "Tint Red", FF_TYPE_RED, 1.0f);
SetParamInfo(PARAM_TINT_GREEN, "Tint Green", FF_TYPE_GREEN, 1.0f);
SetParamInfo(PARAM_TINT_BLUE, "Tint Blue", FF_TYPE_BLUE, 1.0f);

// In shader
uniform vec3 tintColor;  // Combined in C++ before passing
```

## Multi-Texture Handling

### ISF Multiple Inputs

**ISF:**
```json
{
  "INPUTS": [
    {
      "NAME": "startImage",
      "TYPE": "image"
    },
    {
      "NAME": "endImage",
      "TYPE": "image"
    }
  ]
}
```

**FFGL:**
```cpp
// In plugin class
SetMinInputs(2);
SetMaxInputs(2);

// In ProcessOpenGL()
FFGLTextureStruct& input1 = *(inputTextures[0]);
FFGLTextureStruct& input2 = *(inputTextures[1]);

// Bind textures to shader
glActiveTexture(GL_TEXTURE0);
glBindTexture(GL_TEXTURE_2D, input1.Handle);
glUniform1i(shader.GetUniformLocation("startImage"), 0);

glActiveTexture(GL_TEXTURE1);
glBindTexture(GL_TEXTURE_2D, input2.Handle);
glUniform1i(shader.GetUniformLocation("endImage"), 1);
```

**GLSL Shader:**
```glsl
uniform sampler2D startImage;
uniform sampler2D endImage;

void main() {
    vec2 uv = gl_FragCoord.xy / resolution;
    vec4 col1 = texture(startImage, uv);
    vec4 col2 = texture(endImage, uv);
    gl_FragColor = mix(col1, col2, progress);
}
```

## Complete Example: Sketchbook Reveal

### ISF Version (Shaders-2)

**SketchbookReveal.isf:**
```glsl
/*{
  "ISFVSN": "2",
  "DESCRIPTION": "Sketchbook reveal transition",
  "CREDIT": "Your Name",
  "CATEGORIES": ["Transition"],
  "INPUTS": [
    {
      "NAME": "startImage",
      "TYPE": "image"
    },
    {
      "NAME": "endImage",
      "TYPE": "image"
    },
    {
      "NAME": "progress",
      "TYPE": "float",
      "MIN": 0.0,
      "MAX": 1.0,
      "DEFAULT": 0.0
    },
    {
      "NAME": "hatchDensity",
      "TYPE": "float",
      "MIN": 5.0,
      "MAX": 90.0,
      "DEFAULT": 35.0
    }
  ]
}*/

float sobel(vec2 uv, sampler2D tex) {
    vec2 px = 1.0 / RENDERSIZE.xy;
    
    float tl = length(IMG_NORM_PIXEL(tex, uv + vec2(-px.x, px.y)).rgb);
    float t  = length(IMG_NORM_PIXEL(tex, uv + vec2(0.0, px.y)).rgb);
    float tr = length(IMG_NORM_PIXEL(tex, uv + vec2(px.x, px.y)).rgb);
    
    float l  = length(IMG_NORM_PIXEL(tex, uv + vec2(-px.x, 0.0)).rgb);
    float r  = length(IMG_NORM_PIXEL(tex, uv + vec2(px.x, 0.0)).rgb);
    
    float bl = length(IMG_NORM_PIXEL(tex, uv + vec2(-px.x, -px.y)).rgb);
    float b  = length(IMG_NORM_PIXEL(tex, uv + vec2(0.0, -px.y)).rgb);
    float br = length(IMG_NORM_PIXEL(tex, uv + vec2(px.x, -px.y)).rgb);
    
    float gx = tl + 2.0*l + bl - tr - 2.0*r - br;
    float gy = tl + 2.0*t + tr - bl - 2.0*b - br;
    
    return sqrt(gx*gx + gy*gy);
}

void main() {
    vec2 uv = isf_FragNormCoord;
    
    float edge = sobel(uv, endImage);
    
    vec4 startCol = IMG_NORM_PIXEL(startImage, uv);
    vec4 endCol = IMG_NORM_PIXEL(endImage, uv);
    
    // Hatching effect
    vec2 hatchUV = uv * RENDERSIZE.xy / hatchDensity;
    float hatch = step(0.5, fract(hatchUV.x + hatchUV.y));
    
    float reveal = progress + edge * 0.3;
    reveal = smoothstep(reveal - 0.1, reveal + 0.1, hatch);
    
    gl_FragColor = mix(startCol, endCol, reveal);
}
```

### FFGL Version (FFGL_agentic)

**SketchbookReveal.h:**
```cpp
#pragma once
#include <FFGL.h>
#include <FFGLLib.h>

class SketchbookReveal : public CFreeFrameGLPlugin
{
public:
    SketchbookReveal();
    ~SketchbookReveal();

    // Plugin info
    FFResult InitGL(const FFGLViewportStruct* vp) override;
    FFResult DeInitGL() override;
    FFResult ProcessOpenGL(ProcessOpenGLStruct* pGL) override;

    // Parameters
    FFResult SetFloatParameter(unsigned int index, float value) override;
    float GetFloatParameter(unsigned int index) override;

private:
    enum ParamType : FFUInt32 {
        PARAM_PROGRESS = 0,
        PARAM_HATCH_DENSITY = 1
    };

    float m_Progress;
    float m_HatchDensity;
    
    FFGLShader m_Shader;
    GLint m_StartImageLoc;
    GLint m_EndImageLoc;
    GLint m_ResolutionLoc;
    GLint m_ProgressLoc;
    GLint m_HatchDensityLoc;
};
```

**SketchbookReveal.cpp:**
```cpp
#include "SketchbookReveal.h"

static CFFGLPluginInfo PluginInfo(
    SketchbookReveal::CreateInstance,
    "SKBR",  // Plugin unique ID
    "Sketchbook Reveal",
    2,  // API major version
    1,  // API minor version
    1,  // Plugin major version
    0,  // Plugin minor version
    FF_EFFECT,
    "Sketchbook reveal transition effect",
    "Your Name"
);

SketchbookReveal::SketchbookReveal()
    : CFreeFrameGLPlugin()
    , m_Progress(0.0f)
    , m_HatchDensity(35.0f)
{
    SetMinInputs(2);
    SetMaxInputs(2);
    
    SetParamInfo(PARAM_PROGRESS, "Progress", FF_TYPE_STANDARD, 0.0f);
    SetParamInfo(PARAM_HATCH_DENSITY, "Hatch Density", FF_TYPE_STANDARD, 0.5f);
    SetParamRange(PARAM_HATCH_DENSITY, 5.0f, 90.0f);
}

SketchbookReveal::~SketchbookReveal()
{
}

FFResult SketchbookReveal::InitGL(const FFGLViewportStruct* vp)
{
    // Load and compile shaders
    if (!m_Shader.Compile(vertexShaderCode, fragmentShaderCode))
    {
        return FF_FAIL;
    }
    
    // Get uniform locations
    m_StartImageLoc = m_Shader.FindUniform("startImage");
    m_EndImageLoc = m_Shader.FindUniform("endImage");
    m_ResolutionLoc = m_Shader.FindUniform("resolution");
    m_ProgressLoc = m_Shader.FindUniform("progress");
    m_HatchDensityLoc = m_Shader.FindUniform("hatchDensity");
    
    return FF_SUCCESS;
}

FFResult SketchbookReveal::DeInitGL()
{
    m_Shader.FreeGLResources();
    return FF_SUCCESS;
}

FFResult SketchbookReveal::ProcessOpenGL(ProcessOpenGLStruct* pGL)
{
    if (pGL->numInputTextures < 2)
        return FF_FAIL;
    
    FFGLTextureStruct& input1 = *(pGL->inputTextures[0]);
    FFGLTextureStruct& input2 = *(pGL->inputTextures[1]);
    
    m_Shader.BindShader();
    
    // Set uniforms
    glUniform2f(m_ResolutionLoc, 
                static_cast<float>(vp->width), 
                static_cast<float>(vp->height));
    glUniform1f(m_ProgressLoc, m_Progress);
    glUniform1f(m_HatchDensityLoc, m_HatchDensity);
    
    // Bind textures
    glActiveTexture(GL_TEXTURE0);
    glBindTexture(GL_TEXTURE_2D, input1.Handle);
    glUniform1i(m_StartImageLoc, 0);
    
    glActiveTexture(GL_TEXTURE1);
    glBindTexture(GL_TEXTURE_2D, input2.Handle);
    glUniform1i(m_EndImageLoc, 1);
    
    // Draw quad
    glBegin(GL_QUADS);
    glTexCoord2f(0.0f, 0.0f); glVertex2f(-1.0f, -1.0f);
    glTexCoord2f(1.0f, 0.0f); glVertex2f( 1.0f, -1.0f);
    glTexCoord2f(1.0f, 1.0f); glVertex2f( 1.0f,  1.0f);
    glTexCoord2f(0.0f, 1.0f); glVertex2f(-1.0f,  1.0f);
    glEnd();
    
    m_Shader.UnbindShader();
    
    return FF_SUCCESS;
}

FFResult SketchbookReveal::SetFloatParameter(unsigned int index, float value)
{
    switch (index)
    {
        case PARAM_PROGRESS:
            m_Progress = value;
            return FF_SUCCESS;
        case PARAM_HATCH_DENSITY:
            m_HatchDensity = value;
            return FF_SUCCESS;
    }
    return FF_FAIL;
}

float SketchbookReveal::GetFloatParameter(unsigned int index)
{
    switch (index)
    {
        case PARAM_PROGRESS:
            return m_Progress;
        case PARAM_HATCH_DENSITY:
            return m_HatchDensity;
    }
    return 0.0f;
}
```

**SketchbookReveal.frag:**
```glsl
#version 410

uniform sampler2D startImage;
uniform sampler2D endImage;
uniform vec2 resolution;
uniform float progress;
uniform float hatchDensity;

out vec4 fragColor;

float sobel(vec2 uv) {
    vec2 px = 1.0 / resolution;
    
    float tl = length(texture(endImage, uv + vec2(-px.x, px.y)).rgb);
    float t  = length(texture(endImage, uv + vec2(0.0, px.y)).rgb);
    float tr = length(texture(endImage, uv + vec2(px.x, px.y)).rgb);
    
    float l  = length(texture(endImage, uv + vec2(-px.x, 0.0)).rgb);
    float r  = length(texture(endImage, uv + vec2(px.x, 0.0)).rgb);
    
    float bl = length(texture(endImage, uv + vec2(-px.x, -px.y)).rgb);
    float b  = length(texture(endImage, uv + vec2(0.0, -px.y)).rgb);
    float br = length(texture(endImage, uv + vec2(px.x, -px.y)).rgb);
    
    float gx = tl + 2.0*l + bl - tr - 2.0*r - br;
    float gy = tl + 2.0*t + tr - bl - 2.0*b - br;
    
    return sqrt(gx*gx + gy*gy);
}

void main() {
    vec2 uv = gl_FragCoord.xy / resolution;
    
    float edge = sobel(uv);
    
    vec4 startCol = texture(startImage, uv);
    vec4 endCol = texture(endImage, uv);
    
    // Hatching effect
    vec2 hatchUV = uv * resolution / hatchDensity;
    float hatch = step(0.5, fract(hatchUV.x + hatchUV.y));
    
    float reveal = progress + edge * 0.3;
    reveal = smoothstep(reveal - 0.1, reveal + 0.1, hatch);
    
    fragColor = mix(startCol, endCol, reveal);
}
```

**SketchbookReveal.vert:**
```glsl
#version 410

in vec4 position;
in vec2 texCoord;

out vec2 v_texCoord;

void main() {
    gl_Position = position;
    v_texCoord = texCoord;
}
```

## Common Conversion Issues

### Issue 1: Coordinate System Differences

**Problem:** ISF uses normalized coordinates (0-1), FFGL might use different systems.

**Solution:** Always normalize in FFGL shaders:
```glsl
vec2 uv = gl_FragCoord.xy / resolution;
```

### Issue 2: Parameter Scaling

**Problem:** FFGL `FF_TYPE_STANDARD` parameters are 0-1, but shader needs different range.

**Solution:** Scale in shader or use `FF_TYPE_FLOAT` for absolute values:
```glsl
// In shader, if using FF_TYPE_STANDARD
float actualValue = paramValue * (maxValue - minValue) + minValue;

// Or in C++ before passing to shader
float scaledValue = m_ParamValue * (90.0f - 5.0f) + 5.0f;
```

### Issue 3: Texture Wrapping

**Problem:** ISF handles texture wrapping automatically, FFGL requires explicit setup.

**Solution:** Set texture parameters in C++:
```cpp
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
```

### Issue 4: Multiple Passes

**Problem:** ISF supports render passes, FFGL requires manual FBO management.

**Solution:** Create framebuffer objects in `InitGL()`:
```cpp
glGenFramebuffers(1, &m_FBO);
glGenTextures(1, &m_IntermediateTexture);
// Configure and use for multi-pass rendering
```

## Migration Checklist

When converting an ISF shader to FFGL:

- [ ] Extract GLSL code from ISF JSON wrapper
- [ ] Replace `IMG_NORM_PIXEL()` with `texture()`
- [ ] Replace `RENDERSIZE` with `resolution` uniform
- [ ] Replace `TIME` with `time` uniform
- [ ] Replace `isf_FragNormCoord` with `gl_FragCoord.xy / resolution`
- [ ] Create C++ plugin class inheriting from `CFreeFrameGLPlugin`
- [ ] Define parameter enums in header
- [ ] Implement `SetParamInfo()` calls in constructor
- [ ] Map ISF parameter types to FFGL types
- [ ] Implement `ProcessOpenGL()` for rendering
- [ ] Handle multiple input textures if needed
- [ ] Compile and link shaders in `InitGL()`
- [ ] Create vertex shader (usually passthrough)
- [ ] Set up CMakeLists.txt for building
- [ ] Test in target VJ software (Resolume, VDMX, etc.)

## Quick Reference Table

| Operation | ISF | FFGL |
|-----------|-----|------|
| Get texture size | `RENDERSIZE` | `resolution` uniform |
| Sample texture | `IMG_NORM_PIXEL(tex, uv)` | `texture(tex, uv)` |
| Get normalized UV | `isf_FragNormCoord` | `gl_FragCoord.xy / resolution` |
| Get time | `TIME` | `time` uniform |
| Set output color | `gl_FragColor = color;` | `gl_FragColor = color;` or `fragColor = color;` |
| Define parameter | JSON in comment | `SetParamInfo()` in C++ |
| Multiple inputs | JSON INPUTS array | `SetMinInputs()`, `SetMaxInputs()` |
| Access input texture | Automatic | Manual `glBindTexture()` |

## Additional Resources

- **ISF Specification**: https://www.interactiveshaderformat.com/
- **FFGL SDK**: https://github.com/resolume/ffgl
- **FFGL Examples**: https://github.com/resolume/ffgl/tree/master/source/plugins
- **OpenGL Reference**: https://www.khronos.org/opengl/wiki/
- **GLSL Reference**: https://www.khronos.org/opengl/wiki/Core_Language_(GLSL)

---

**Last Updated:** 2026-02-05  
**For:** FFGL_agentic repository setup
