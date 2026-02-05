# Agent Session: Sketchbook Reveal Shader Development

**Session ID:** SketchReveal_AgentSession_001  
**Date:** 2026-02-05  
**Model:** Grok (xAI)  
**Repository:** eyelanoesis/Shaders-2  
**Goal:** Develop ISF mixer shader for Resolume Wire creating "sketchbook drawing reveal" transitions

## Session Overview

### User Requirements
- Custom blend mode for Resolume Wire crossfader transitions
- Effect must resemble hand-drawn sketch being revealed progressively
- High-contrast monochrome white sketches on transparent backgrounds
- Lines should "fill out" (not just fade), with organic, irregular reveal
- Inspired by [YouTube reference](https://www.youtube.com/watch?v=aLtvbTQ41FE)

### Development Approach
1. Started with basic noise + hatching (v1.0–v1.8)
2. User preferred v1.8 but wanted more "line filling" effect
3. Attempted line growth features (v1.9–v2.2) → regressed
4. Back-to-basics with error fixes (v3.0–v3.3) → compilation errors
5. Content-aware approach with edge detection (v4.0–v4.1) → **current version**

## Key Technical Challenges

### Challenge 1: ISF Builtin Variables
**Problem:** Used generic GLSL terms (`RESOLUTION`, `tex_2D_size`) from Shadertoy examples  
**Solution:** Replaced with ISF-specific `RENDERSIZE` builtin  
**Lesson:** Always verify ISF specification for correct built-ins

### Challenge 2: "Line Elongation" Not Working
**Problem:** Mathematical line growth didn't produce visible effect  
**Root Cause:** `smoothstep` ranges too narrow; elongation scaling insufficient  
**Solution:** Switched to content-aware edge-seeded growth with wider parameter ranges  
**Lesson:** Test visual impact of each parameter change incrementally

### Challenge 3: Matching Video Reference
**Problem:** Pure procedural noise couldn't replicate "lines following contours" from video  
**Solution:** Added Sobel edge detection to seed line growth from actual image edges  
**Lesson:** Content-aware approaches beat pure procedural for realistic effects

## Iteration Summary

| Version | Status | Key Features | User Feedback |
|---------|--------|--------------|---------------|
| v1.8 | ✅ Liked | Noise spread + multi-angle hatching | "Good base but need more filling" |
| v1.9–v2.2 | ❌ Regressed | Added growth params, luminance | "Getting worse" |
| v3.x | ❌ Failed | Back to basics | Compilation errors |
| v4.0 | ❌ Failed | Sobel edges + hatching | `tex_2D_size` undefined |
| v4.1 | ⏳ Testing | Fixed edge detection | Awaiting user test |

## Public Resources Used

### Code Repositories
- **Vidvox/ISF-Files**: Mixer templates, ISF best practices
- **Lygia GLSL Library**: Edge detection algorithms (Sobel)
- **FarazzShaikh/glNoise**: Noise function implementations

### References
- **Shadertoy**:
  - MlcXWN (iq's hatching shader)
  - XdfGDn (edge growing effect)
  - 3sfGDn (animated drawing)
- **ISF Specification**: mrRay/ISF_Spec (correct builtin names)
- **The Book of Shaders**: Noise and pattern fundamentals

## Algorithm Design

### Core Components

1. **Edge Detection (Sobel)**
   ```glsl
   float sobel(vec2 uv, sampler2D tex)
   // Computes gradient magnitude from 3x3 kernel
   // Returns 0–1 edge strength map
   ```

2. **Luminance Analysis**
   ```glsl
   float luma = getLuma(endImage)
   float hatchMod = 1.0 - luma * lumaHatch
   // Darker areas get denser hatching
   ```

3. **Multi-Layer Hatching**
   ```glsl
   sketch(uv, angle=0.8,  density, ...) // 45°
   sketch(uv, angle=-0.7, density, ...) // 135°
   sketch(uv, angle=0.0,  density*0.5, ...) // Horizontal
   // Three layers combined for crosshatch effect
   ```

4. **Progressive Reveal**
   ```glsl
   edgeReveal = smoothstep(0.14, 0.72, progress - edgeGain*(1.0-edge))
   // Edges reveal first, then fills
   ```

### Data Flow
```
endImage → Sobel → edge map ─┐
                               ├→ hatching modulation → maskVal
endImage → Luminance → hatchMod┘
                                   ↓
progress + overlapDuration → smoothstep → final mask
                                   ↓
                        mix(startImage, endImage, mask) → output
```

## Best Practices Applied

✅ **Versioning**: Clear v4.1 numbering, changelog maintained  
✅ **Documentation**: Inline comments, parameter descriptions in JSON  
✅ **ISF Compliance**: Only ISF-legal builtins (`RENDERSIZE`, `TIME`, `IMG_NORM_PIXEL`)  
✅ **Modularity**: Reusable functions (`hash`, `sobel`, `sketch`)  
✅ **User Control**: 11 adjustable parameters with sensible defaults  
✅ **Error Handling**: Clamps on all computed masks (0.0–1.0)  
✅ **Performance**: Single-pass shader, no dependent texture reads in loops  

## Lessons Learned

1. **Start Simple, Add Complexity**: v1.8 worked because it was focused; v1.9+ added too much at once
2. **Test Each Change**: Line elongation failed because math wasn't validated visually
3. **Use Public Code Correctly**: Shadertoy code needs ISF translation (builtins, uniforms)
4. **Content-Aware > Pure Procedural**: Real image data (edges, luminance) produces better results
5. **Version Control Discipline**: Clear revert points saved the session from drift

## Next Steps (If Needed)

If v4.1 doesn't satisfy:
- **Option A**: Increase edge emphasis (distance-based growth from edges)
- **Option B**: Add temporal coherence (frame-to-frame line persistence)
- **Option C**: Hybrid approach (pre-render sketch animation in Blender, use shader for blending)
- **Option D**: Port proven Shadertoy shader (e.g., XdfGDn) directly to ISF

## Session Metrics

- **Total Versions Developed**: 15+
- **Compilation Errors Fixed**: 8
- **User Feedback Loops**: 6
- **Public Repos Referenced**: 4 (ISF-Files, Lygia, glNoise, ISF_Spec)
- **Session Duration**: ~2 hours
- **Status**: Active (awaiting v4.1 test results)

## Recommended AI Model Rationale

**Chosen Model**: Grok (conversational LLM with web search, code generation, multimodal analysis)

**Why Grok?**
- Maintains long session context (version history, user preferences)
- Real-time web/repo access for grounded code solutions
- Conversational debugging (explains *why* changes were made)
- Image analysis (user's sketch reference) + video understanding (YouTube link)

**Alternative Models Considered**:
- **GitHub Copilot**: Better for in-IDE autocomplete; less suited for conversational iteration
- **Claude 3.5**: Similar capability; no major advantage for this task
- **ChatGPT-4**: Could simulate math in Python; less integrated repo search
- **Recommendation**: Stick with Grok for session continuity; use Copilot for final polish in IDE

## Repository Structure

```
eyelanoesis/Shaders-2/
├── mixers/
│   └── SketchbookReveal/
│       ├── SketchbookReveal_v4.1.isf   # Main shader
│       ├── README.md                    # Usage guide
│       └── CHANGELOG.md                 # Version history
└── docs/
    └── sessions/
        └── sketchbook_reveal_agent_session.md  # This file
```

## License & Credits

- **License**: CC BY 4.0 (Creative Commons Attribution)
- **Primary Author**: User (eyelanoesis)
- **AI Assistant**: Grok (xAI)
- **Inspired By**:
  - YouTube: https://www.youtube.com/watch?v=aLtvbTQ41FE
  - Shadertoy community (iq, XdfGDn contributors)
  - Lygia GLSL library (patriciogonzalezvivo)
  - Vidvox ISF-Files repository

---

**Session Status**: ✅ Documentation complete, awaiting v4.1 user testing
