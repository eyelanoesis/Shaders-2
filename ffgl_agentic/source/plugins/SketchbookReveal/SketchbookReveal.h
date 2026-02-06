#pragma once
#include <FFGL.h>
#include <FFGLLib.h>
#include <string>

class SketchbookReveal : public CFreeFrameGLPlugin
{
public:
    SketchbookReveal();
    ~SketchbookReveal() override = default;

    // Plugin info
    static FFResult __stdcall CreateInstance(CFreeFrameGLPlugin **ppInstance) {
        *ppInstance = new SketchbookReveal();
        return (*ppInstance != nullptr) ? FF_SUCCESS : FF_FAIL;
    }
    
    FFResult InitGL(const FFGLViewportStruct *vp) override;
    FFResult DeInitGL() override;
    FFResult ProcessOpenGL(ProcessOpenGLStruct *pGL) override;

    // Parameters
    FFResult SetFloatParameter(unsigned int index, float value) override;
    float GetFloatParameter(unsigned int index) override;

private:
    // Parameter indices
    enum Parameters {
        PARAM_PROGRESS = 0,
        PARAM_EDGE_GAIN,
        PARAM_LUMA_HATCH,
        PARAM_HATCH_INTENSITY,
        PARAM_DENSITY,
        PARAM_JITTER,
        PARAM_OVERLAP_DURATION,
        PARAM_OVERLAP_SOFTNESS,
        PARAM_ANIM_SPEED,
        PARAM_COUNT
    };

    // OpenGL resources
    GLuint m_shaderProgram;
    GLuint m_vertexShader;
    GLuint m_fragmentShader;
    GLuint m_vao;
    GLuint m_vbo;

    // Uniform locations
    GLint m_inputTextureLoc;
    GLint m_resolutionLoc;
    GLint m_timeLoc;
    GLint m_progressLoc;
    GLint m_edgeGainLoc;
    GLint m_lumaHatchLoc;
    GLint m_hatchIntensityLoc;
    GLint m_densityLoc;
    GLint m_jitterLoc;
    GLint m_overlapDurationLoc;
    GLint m_overlapSoftnessLoc;
    GLint m_animSpeedLoc;

    // Parameter values
    float m_parameters[PARAM_COUNT];
    
    // Time tracking
    double m_timeCounter;

    // Helper methods
    bool CompileShader(GLuint shader, const char* source);
    bool LinkProgram();
    void SetupQuad();
};
