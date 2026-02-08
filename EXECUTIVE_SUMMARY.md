# Shaders-2 Repository - Executive Summary

**Generated:** 2026-02-08  
**Repository:** eyelanoesis/Shaders-2

---

## What Is This Repository?

A professional collection of **real-time GPU shaders** for **VJ (Video Jockey) software** and live visual performances, maintained by Copenhagen artist **Rune Abro** and the **Hackstage creative collective**.

---

## Contents at a Glance

### 🎨 **8 Visual Shaders**
1. **Mandelbrot.fs** - Classic fractal with zoom/pan controls
2. **JuliaSet.fs** - Animated Julia set fractals
3. **BurningShip.fs** - Asymmetrical fractal variant
4. **FractalNoise.fs** - Procedural noise generator (3 algorithms)
5. **KaleidoscopeFractal.fs** - Hypnotic symmetry patterns (4 modes)
6. **SierpinskiTriangle.fs** - Recursive triangle fractal
7. **fast-realistic-clouds-v1.3.0.fs** - Cloud generator with 6 presets
8. **SketchbookReveal** - Content-aware sketch transition (ISF + FFGL versions)

### 🔧 **FFGL Plugin System**
- Complete C++ build environment (CMake-based)
- 1 production plugin: **SketchbookReveal** (Windows .dll / macOS .bundle)
- Cross-platform compilation for 6+ VJ software packages

### 📚 **Documentation**
- ISF-to-FFGL conversion guides
- Build and installation instructions
- AI-assisted development session logs

---

## Technical Highlights

### Fractal Shaders
- **Algorithms:** Escape-time iteration, smooth coloring, fBM noise
- **Features:** Real-time zoom/pan, multiple colormaps, animations
- **Performance:** Optimized for 60 FPS at 1920×1080

### Transition Effects
- **SketchbookReveal:** Sobel edge detection + animated hatching
- **Parameters:** 9-11 adjustable controls per effect
- **Use Case:** Professional video transitions for live performances

### FFGL Plugins
- **Compatibility:** Resolume, VDMX, CoGe, Magic, Millumin, TouchDesigner
- **Architecture:** OpenGL-based with manual resource management
- **Distribution:** Compiled binaries for instant use

---

## Software Compatibility

**ISF Shaders Work In:**
- Resolume Wire (Arena 7.x+)
- VDMX (macOS)
- CoGe (macOS)
- Any ISF 2.0 compatible software

**FFGL Plugins Work In:**
- Resolume Arena/Avenue
- VDMX
- CoGe
- Magic Music Visuals
- Millumin
- TouchDesigner

---

## Repository Structure

```
Shaders-2/
├── shaders/               # 6 ISF fractal generators
├── mixers/                # Transition effects (ISF)
├── ffgl_agentic/          # FFGL plugin development
│   ├── source/plugins/    # C++ plugin code
│   └── docs/guides/       # Build & conversion guides
├── docs/                  # Repository documentation
└── fast-realistic-clouds-v1.3.0.fs  # Standalone cloud shader
```

---

## Key Features

✅ **Production-Ready:** Used in live VJ performances  
✅ **Cross-Platform:** Windows, macOS, Linux build support  
✅ **Well-Documented:** Comprehensive guides and session logs  
✅ **AI-Assisted:** Embraces modern coding assistant workflows  
✅ **Open Source:** CC BY 4.0 license (commercial use allowed)

---

## Use Cases

- 🎭 **Live Performances:** Real-time visuals for concerts and events
- 🎬 **Video Production:** Professional transitions and effects
- 📚 **Education:** GLSL shader programming examples
- 🔬 **Research:** Mathematical fractal visualization
- 🛠️ **Development:** ISF-to-FFGL conversion reference

---

## Development Workflow

### Prototyping (ISF)
1. Write shader in GLSL with ISF JSON metadata
2. Test in Resolume Wire (instant reload)
3. Iterate rapidly with live preview

### Production (FFGL)
1. Convert ISF to C++ plugin using conversion guide
2. Build with CMake for target platform
3. Distribute compiled .dll/.bundle files

---

## Technical Specifications

| Category | Details |
|----------|---------|
| **Languages** | GLSL (shaders), C++ (FFGL plugins) |
| **APIs** | ISF 2.0, FFGL 2.1, OpenGL |
| **Build System** | CMake 3.15+ |
| **Code Size** | ~1,500 lines GLSL, ~450 lines C++ |
| **Platforms** | Windows, macOS, Linux |
| **License** | CC BY 4.0 (open source) |

---

## Quick Start

### Using ISF Shaders
1. Copy `.isf` or `.fs` files to Resolume ISF folder
2. Restart Resolume Wire
3. Add shader as generator or effect in composition

### Building FFGL Plugins
```bash
cd ffgl_agentic
cmake -B build -S . -DCMAKE_BUILD_TYPE=Release
cmake --build build --config Release
# Copy resulting .dll/.bundle to VJ software plugin folder
```

---

## Who Is This For?

- **VJs & Live Artists:** Need real-time GPU effects for performances
- **Creative Coders:** Want to learn GLSL shader programming
- **Plugin Developers:** Porting shaders between ISF and FFGL formats
- **Resolume Users:** Seeking custom generators and transitions

---

## Standout Projects

### 🎨 SketchbookReveal
- **What:** Content-aware sketch reveal transition
- **How:** Sobel edge detection + animated hatching patterns
- **Versions:** ISF (interpreted) + FFGL (compiled)
- **Status:** Production-ready, used in live performances

### 🌀 Fractal Collection
- **What:** 6 interactive mathematical fractals
- **Algorithms:** Mandelbrot, Julia, Burning Ship, Kaleidoscope, Sierpinski, fBM noise
- **Features:** Real-time zoom, pan, animation, multiple color schemes

---

## Repository Health

✅ **Active Maintenance:** Recent updates (2026-02)  
✅ **Complete Documentation:** 7 markdown guides  
✅ **Version Control:** Git tags, changelogs, session logs  
✅ **AI Integration:** Documents agent-assisted development  
✅ **Professional Quality:** Industry-standard code and practices

---

## Links & Resources

- **Repository:** github.com/eyelanoesis/Shaders-2
- **License:** Creative Commons Attribution 4.0 (CC BY 4.0)
- **Maintainer:** Rune Abro (Hackstage Collective)
- **Location:** Copenhagen, Denmark

---

## Bottom Line

**Shaders-2** is a **complete toolkit** for creating and deploying real-time GPU visual effects in professional VJ software. It combines:
- Mathematical sophistication (advanced fractal algorithms)
- Production readiness (optimized, tested, documented)
- Developer experience (guides, examples, AI workflows)
- Creative vision (avant-garde digital art philosophy)

**Perfect for:** VJs, creative coders, and visual artists working with Resolume, VDMX, and similar platforms.

---

**For detailed technical analysis, see:** `REPOSITORY_ANALYSIS.md`
