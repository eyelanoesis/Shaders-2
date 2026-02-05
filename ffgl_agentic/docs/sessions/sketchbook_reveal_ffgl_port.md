# FFGL Port Session: Sketchbook Reveal

**Date:** 2026-02-05  
**Source:** `mixers/SketchbookReveal/SketchbookReveal_v4.1.isf`  
**Target:** `ffgl_agentic/source/plugins/SketchbookReveal/`  
**Agent:** Grok (xAI) - Continuation of SketchReveal_AgentSession_001

## Port Overview

Converted ISF mixer shader to standalone FFGL plugin for broader VJ software compatibility.

### Changes Made

1. **Shader Code** (ISF → GLSL)
   - Removed ISF JSON wrapper
   - Replaced `IMG_NORM_PIXEL()` with `texture()`
   - Replaced `RENDERSIZE` with `uniform vec2 resolution`
   - Replaced `TIME` with `uniform float time`
   - Replaced `isf_FragNormCoord` with calculated UV

2. **C++ Wrapper** (New)
   - Created `SketchbookReveal` class extending `CFreeFrameGLPlugin`
   - Implemented parameter system (9 parameters)
   - Set up OpenGL resources (shaders, VAO, VBO)
   - Mapped 0-1 FFGL params to original ranges

3. **Build System** (New)
   - Root CMakeLists.txt
   - Plugin-specific CMakeLists.txt
   - Cross-platform configuration (Windows DLL, macOS bundle)

### Parameter Mapping

| ISF Parameter | Range | FFGL Parameter | Mapping |
|---------------|-------|----------------|---------|
| progress | 0.0-1.0 | Progress | Direct (0-1) |
| edgeGain | 0.0-3.0 | Edge Gain | `value * 3.0` |
| lumaHatch | 0.0-3.0 | Luma Hatch | `value * 3.0` |
| hatchIntensity | 0.0-1.0 | Hatch Intensity | Direct (0-1) |
| density | 5.0-90.0 | Density | `value * 85.0 + 5.0` |
| jitter | 0.0-1.0 | Jitter | Direct (0-1) |
| overlapDuration | 0.1-1.0 | Overlap Duration | `value * 0.9 + 0.1` |
| overlapSoftness | 0.0-0.5 | Overlap Softness | `value * 0.5` |
| animSpeed | 0.0-1.0 | Anim Speed | Direct (0-1) |

### Files Created

- `SketchbookReveal.h` - Plugin class declaration (142 lines)
- `SketchbookReveal.cpp` - Plugin implementation (287 lines)
- `CMakeLists.txt` (root) - Build configuration
- `CMakeLists.txt` (plugin) - Plugin-specific build
- `README.md` - Documentation
- `docs/sessions/sketchbook_reveal_ffgl_port.md` - This file

### Technical Notes

**Shader Differences:**
- FFGL uses standard GLSL 4.1 (no ISF extensions)
- Texture sampling is manual (no automatic binding)
- Resolution/time must be passed as uniforms

**Plugin Architecture:**
- Single input texture
- 9 adjustable parameters (all FF_TYPE_STANDARD 0-1)
- Fullscreen quad rendering
- Standard FFGL 2.1 API

**Performance:**
- Similar to ISF version (single-pass, no complex branching)
- Sobel edge detection: 9 texture reads per pixel
- Multi-layer hatching: 3 sin calculations
- Suitable for real-time use at HD resolutions

### Build Testing

**Status:** Not yet compiled (waiting for FFGL SDK submodule)

**Next Steps:**
1. Add FFGL SDK submodule
2. Test build on Windows (MSVC)
3. Test build on macOS (Clang)
4. Test in Resolume Arena
5. Test parameter ranges match ISF version
6. Verify edge detection accuracy

### Lessons Learned

1. **ISF→FFGL is Mechanical**: Most ISF shaders port easily with pattern substitution
2. **Parameter Ranges**: FFGL standardizes to 0-1; remap in shader
3. **Time Tracking**: FFGL doesn't provide time automatically; track in plugin
4. **Texture Binding**: FFGL requires manual GL setup vs ISF's automatic binding

### Known Issues

- Time counter assumes 60fps (should use actual frame delta)
- No error logging yet (shader compile failures silent)
- Missing Info.plist for macOS bundle
- No installer/packaging yet

### Future Enhancements

- Add second input for dual-texture transitions (true mixer)
- Expose noise type selection (currently fixed to Perlin-style)
- Add preset system for parameter combinations
- Create automated installer

---

**Status:** ✅ Code complete, awaiting build test  
**Session Continues:** Next port target TBD
