# External Resources & References

**Curated collection of external resources relevant to the Shaders-2 repository**

This document catalogs all external specifications, tools, communities, learning materials, and related projects that are relevant to the shader development, VJ performance, fractal visualization, and AI-assisted creative coding workflows used in this repository.

---

## Table of Contents

- [ISF (Interactive Shader Format)](#isf-interactive-shader-format)
- [FFGL (FreeFrame GL)](#ffgl-freeframe-gl)
- [GLSL & OpenGL](#glsl--opengl)
- [Resolume Arena & Wire](#resolume-arena--wire)
- [VJ Software Ecosystem](#vj-software-ecosystem)
- [Fractal Mathematics & Visualization](#fractal-mathematics--visualization)
- [Shader Communities & Galleries](#shader-communities--galleries)
- [Learning Resources](#learning-resources)
- [AI-Assisted Creative Coding](#ai-assisted-creative-coding)
- [Video Sharing Frameworks](#video-sharing-frameworks)
- [Creative Coding Platforms](#creative-coding-platforms)
- [Video Art & Live Visuals Community](#video-art--live-visuals-community)

---

## ISF (Interactive Shader Format)

The shader format used by all `.fs` and `.isf` files in this repository.

| Resource | URL | Description |
|----------|-----|-------------|
| ISF Official Documentation | https://docs.isf.video/ | Quickstart, primer, and full reference for ISF 2.0 metadata, built-in variables, and multi-pass rendering |
| ISF Homepage | https://isf.video/ | Project overview, conceptual background, software integrations |
| ISF Online Editor | https://editor.isf.video/ | Browser-based ISF shader editor — create, test, and share shaders |
| ISF GitHub (Vidvox) | https://github.com/Vidvox/isf | Official repo with spec, ISF Editor links, example shaders, and frameworks |
| ISF Specification (mrRay) | https://github.com/mrRay/ISF_Spec | Alternative specification repo with sample files and integration details |
| ISF Developer Resources | https://isf.video/developers/ | Guidelines for adding ISF support to applications; libraries for Obj-C, C++, JavaScript |
| Resolume ISF Support | https://resolume.com/support/en/isf | How to use ISF shaders within Resolume Arena and Wire |

---

## FFGL (FreeFrame GL)

The compiled plugin format used in `ffgl_agentic/` for cross-platform VJ software compatibility.

| Resource | URL | Description |
|----------|-----|-------------|
| FFGL SDK (GitHub) | https://github.com/resolume/ffgl | Official SDK maintained by Resolume — source, examples, build setup |
| FFGL SDK README | https://github.com/resolume/ffgl/blob/master/README.md | Quickstart, stable releases, ARM-native macOS builds |
| FFGL Example Plugins | https://github.com/resolume/ffgl/tree/master/source/plugins | Reference plugin implementations for learning FFGL patterns |
| FFGL Plugin Writing Guide | https://metal-heart.org/writing-ffgl-plugins-for-resolume-arena-7/ | Third-party guide: live reloading, debugging, best practices |
| FFGL Plugins List (Forum) | https://resolume.com/forum/viewtopic.php?t=11828 | Community-maintained list of free and commercial FFGL plugins |
| PixelClone FFGL Plugins | https://pixelclone.com/software/ffgl-plugins/index.html | Commercial FFGL plugins with advanced controls and automation |

---

## GLSL & OpenGL

Core shader language and graphics API underlying all shaders and plugins in this repository.

| Resource | URL | Description |
|----------|-----|-------------|
| OpenGL Wiki | https://www.khronos.org/opengl/wiki | Official OpenGL documentation and reference |
| GLSL Core Language Reference | https://www.khronos.org/opengl/wiki/Core_Language_(GLSL) | GLSL language specification and built-in functions |
| GLSL Reference Card | https://www.khronos.org/files/opengl-quick-reference-card.pdf | Quick reference for GLSL types, functions, and qualifiers |
| CMake Tutorial | https://cmake.org/cmake/help/latest/guide/tutorial/ | Build system tutorial (used for FFGL plugin compilation) |

---

## Resolume Arena & Wire

Primary target VJ software for shaders in this repository.

| Resource | URL | Description |
|----------|-----|-------------|
| Resolume Official Site | https://resolume.com/ | Main product page for Arena, Avenue, and Wire |
| Wire Video Training | https://resolume.com/training/wire | Official tutorials — beginner to advanced Wire patching |
| Resolume Support | https://resolume.com/support/ | Documentation, FAQs, and troubleshooting |
| Resolume Directory List | https://resolume.com/support/en/directory-list | File paths for effects, plugins, and presets on each OS |
| Resolume Forum | https://resolume.com/forum/ | Community forum for VJs and plugin developers |
| Wire Tutorial Playlist (cliq) | https://www.youtube.com/playlist?list=PLQNjRSGsEz5k18s9bHBON3wpPf2EDPZ-g | Hands-on Wire patching tutorials |
| Resolume Plugins Playlist | https://www.youtube.com/playlist?list=PLsE4xEOZVLjQ8wt7kj3TmW35fdaU3Ld6e | Wire/FFGL plugin demos by Norimichi Tomita |
| Resolume VJ Playlist | https://www.youtube.com/playlist?list=PLwT5G8Y1jwnW2PPMvOt4c8uQK2P8iWU-U | Arena + Wire techniques by Dimitry Chamy |
| Creating Shaders with AI for Resolume | https://www.youtube.com/watch?v=c-Taj9hQ5o8 | Using Cursor AI to generate ISF/GLSL shaders for Wire |

### ISF Shader Installation Paths

| Platform | Path |
|----------|------|
| **Windows** | `C:\Users\[Username]\Documents\Resolume Arena\Extra Effects\` |
| **macOS** | `/Users/[Username]/Documents/Resolume Arena/Extra Effects/` |
| **FFGL Plugins (Windows)** | `C:\Program Files\Common Files\FreeFrame\` |
| **FFGL Plugins (macOS)** | `/Library/Graphics/FreeFrame Plug-Ins/` |
| **Resolume Plugins** | `[Install Dir]/plugins/vfx/` |

---

## VJ Software Ecosystem

Software compatible with the shaders and plugins in this repository.

| Software | Platform | ISF Support | FFGL Support | URL |
|----------|----------|-------------|--------------|-----|
| **Resolume Arena** | Win/Mac | ✅ (via Wire) | ✅ | https://resolume.com/ |
| **Resolume Avenue** | Win/Mac | ✅ (via Wire) | ✅ | https://resolume.com/ |
| **VDMX** | macOS | ✅ | ✅ | https://vidvox.net/ |
| **CoGe** | macOS | ✅ | ✅ | https://imimot.com/cogevj/ |
| **Magic Music Visuals** | Windows | — | ✅ | https://magicmusicvisuals.com/ |
| **Millumin** | macOS | — | ✅ | https://www.millumin.com/ |
| **TouchDesigner** | Win/Mac | — | ✅ (FFGL CHOP) | https://derivative.ca/ |
| **MadMapper** | Win/Mac | — | — | https://madmapper.com/ |
| **Notch** | Windows | — | — | https://www.notch.one/ |

### Free / Open-Source VJ Tools

| Tool | Description | URL |
|------|-------------|-----|
| GLMixer | Open-source GPU layer mixer | https://sourceforge.net/projects/glmixer/ |
| vimix | Open-source video mixer for live performances | https://brunelli.github.io/vimix/ |
| Visualz | Audio-reactive VJ software (free) | https://www.visualzstudio.com/ |
| NoiseDeck | Browser-based GPU visual generator | https://noisedeck.app/ |

---

## Fractal Mathematics & Visualization

Mathematical foundations behind the fractal shaders in `shaders/`.

### Fractal Algorithms Used in This Repository

| Fractal | Shader File | Algorithm | Formula |
|---------|------------|-----------|---------|
| Mandelbrot | `Mandelbrot.fs` | Escape-time iteration | z = z² + c |
| Julia Set | `JuliaSet.fs` | Julia iteration with variable c | z = z² + c (c fixed) |
| Burning Ship | `BurningShip.fs` | Absolute-value variant | z = (\|Re(z)\| + i\|Im(z)\|)² + c |
| fBM Noise | `FractalNoise.fs` | Multi-octave noise | Value / Simplex / Turbulence |
| Kaleidoscope | `KaleidoscopeFractal.fs` | Angular symmetry folding | mod(angle, 2π/n) |
| Sierpinski | `SierpinskiTriangle.fs` | Recursive space folding | Scale-reflect-fold |

### External Fractal Resources

| Resource | URL | Description |
|----------|-----|-------------|
| Fractals & Rendering Techniques (GLSL) | https://darkeclipz.github.io/fractals/ | Interactive GLSL fractal examples with rendering theory |
| GLSL Mandelbrot Tutorial (oZone3D) | https://ozone3d.net/tutorials/mandelbrot_set_p4.php | Step-by-step Mandelbrot in GLSL |
| GLSL Mandelbrot (elijah.mirecki) | https://elijah.mirecki.com/blog/glsl-mandelbrot/ | Coloring techniques, zoom, and palette tricks |
| Visualizing Mandelbrot with OpenGL | https://physicspython.wordpress.com/2020/02/16/visualizing-the-mandelbrot-set-using-opengl-part-1/ | Beginner-friendly Mandelbrot with modern OpenGL |
| Distance Estimated 3D Fractals | http://blog.hvidtfeldts.net/index.php/2011/06/distance-estimated-3d-fractals-part-i/ | Advanced 3D fractal rendering (Mandelbulb, Mandelbox) |
| Distance Estimator Compendium | https://jbaker.graphics/writings/DEC.html | Community compendium of fractal distance functions |
| GPU Fractal Generation Deep Dive | https://peerdh.com/blogs/programming-insights/a-deep-dive-into-fractal-generation-techniques-in-gpu-shaders | Mandelbrot, Julia, fBM noise in GPU shaders |
| Mandelbrot/Julia Fractal Explorer | https://panzi.github.io/webgl-mandelbrot/ | WebGL interactive explorer for multiple fractal types |
| Mandelbrot Set Explorer | https://mandel.gart.nz/ | Zoomable map-style Mandelbrot with Julia preview |
| shader-fractals (GitHub) | https://github.com/pedrotrschneider/shader-fractals | Curated GLSL fractal collection: Mandelbrot, Julia, Sierpinski, Menger |
| Mandelbrot OpenGL Project | https://github.com/Arukiap/Mandelbrot | Interactive Mandelbrot with orbit traps and coloring |
| GLSL Mandelbrot Explorer | https://github.com/sysrpl/Codebot.FractalsGL | Cross-platform real-time GPU fractal explorer |

### Fractal Mathematics

| Resource | URL | Description |
|----------|-----|-------------|
| Mandelbrot-Julia Sets (DynamicMath) | https://www.dynamicmath.xyz/mandelbrot-julia/ | Mathematical explanation with interactive visualizations |
| AnalyzeMath Fractal Visualizer | https://www.analyzemath.com/interactive-math/complex-numbers/Mandelbrot-Julia-sets-visualizer.html | Interactive exploration of Mandelbrot and Julia parameter space |
| Fractalis Interactive Explorer | https://realrobotix.github.io/fractalis/ | Browser-based fractal shader explorer with live controls |

---

## Shader Communities & Galleries

Platforms for discovering, sharing, and learning from other shader creators.

| Platform | URL | Description |
|----------|-----|-------------|
| **Shadertoy** | https://www.shadertoy.com/ | Largest GLSL shader community — thousands of open-source shaders |
| **ISF Shader Gallery** | https://editor.isf.video/ | Browse and share ISF-format shaders |
| **GLSL Sandbox** | https://glslsandbox.com/ | Real-time fragment shader editor and gallery |
| **OpenProcessing** | https://openprocessing.org/ | Creative coding sketches (p5.js/Processing) |
| **ShaderFrog** | https://shaderfrog.com/ | Visual shader editor with composable nodes |

---

## Learning Resources

### Essential Reading

| Resource | URL | Description |
|----------|-----|-------------|
| **The Book of Shaders** | https://thebookofshaders.com/ | Interactive step-by-step GLSL guide — the definitive beginner resource |
| Book of Shaders (GitHub) | https://github.com/patriciogonzalezvivo/thebookofshaders | Source code and contributions |
| **Inigo Quilez's Articles** | https://iquilezles.org/ | GLSL masterclass: distance functions, raymarching, fractals, noise |
| IQ: Distance Functions (3D) | https://iquilezles.org/articles/distfunctions/ | Signed distance functions for every primitive shape |
| IQ: Distance Functions (2D) | https://iquilezles.org/articles/distfunctions2d/ | 2D SDF library for shader art |
| GLSL Resources (Synesthesia) | https://app.synesthesia.live/docs/resources/glsl_resources.html | Curated list of GLSL tutorials, editors, and references |
| Favorite GLSL Resources (scry.art) | https://scry.art/blog/public/posts/favorite-glsl-resources/ | Community-recommended learning paths |

### Books

| Title | Description |
|-------|-------------|
| *The Book of Shaders* (Gonzalez Vivo & Lowe) | Interactive GLSL programming guide (free, online) |
| *The Nature of Code* (Daniel Shiffman) | Generative systems, noise, fractals in Processing/p5.js |
| *Graphics Shaders: Theory and Practice* | Academic reference for shader programming |

### Video Tutorials

| Resource | URL | Description |
|----------|-----|-------------|
| Resolume Wire Training | https://resolume.com/training/wire | Official video series for Wire visual patching |
| Resolume Wire Tutorials (cliq) | https://www.youtube.com/playlist?list=PLQNjRSGsEz5k18s9bHBON3wpPf2EDPZ-g | Community Wire patching walkthroughs |
| Creating Shaders with AI | https://www.youtube.com/watch?v=c-Taj9hQ5o8 | Using AI tools for Resolume shader development |
| Wire Line Overlay Tutorial | https://resolume.com/forum/viewtopic.php?t=23985 | Building custom effects in Wire |

---

## AI-Assisted Creative Coding

Workflows and tools for using AI assistants in shader and plugin development — a core practice in this repository.

| Resource | URL | Description |
|----------|-----|-------------|
| **GitHub Copilot** | https://github.blog/ai-and-ml/github-copilot/ | AI pair programmer for code completion and generation |
| Copilot Workspace | https://www.toolify.ai/ai-news/github-copilot-workspace-aiassisted-coding-a-deep-dive-3433033 | Deep dive into AI-assisted coding workflows |
| Copilot Agent Guidelines | https://gist.github.com/shashankanil/b1c65ec1aa32a11edcbdbe894ee1d7a0 | Creative coding guidelines for Copilot agent mode |
| AI Co-Artist (LLM + GLSL) | https://arxiv.org/html/2512.08951v1 | Research paper: LLM-powered framework for interactive shader creation |
| Copilot Real-World Use Cases | https://devblogs.microsoft.com/ise/accelerating-ai-development-with-github-copilot-real-world-use-cases/ | Enterprise-scale AI development workflows |
| Creating Shaders for Resolume with AI | https://www.youtube.com/watch?v=c-Taj9hQ5o8 | Practical tutorial on AI-generated ISF shaders |

### AI Workflow in This Repository

This repository embraces AI-assisted development as documented in `docs/sessions/`:

1. **Prototype** — Use AI to generate ISF shader code from natural language descriptions
2. **Test** — Load shader in Resolume Wire for instant visual feedback
3. **Iterate** — Refine with AI assistance based on visual results
4. **Port** — Convert ISF to FFGL using AI-generated C++ boilerplate
5. **Document** — Log sessions in `docs/sessions/` for reproducibility

---

## Video Sharing Frameworks

Tools for routing real-time video between applications in VJ setups.

| Framework | Platform | Type | URL | Description |
|-----------|----------|------|-----|-------------|
| **Syphon** | macOS | Local (GPU) | https://syphon.info/ | Zero-copy GPU texture sharing between macOS apps |
| **Spout** | Windows | Local (GPU) | https://spout.zeal.co/ | GPU texture sharing for Windows (open source) |
| Spout GitHub | Windows | Local (GPU) | https://github.com/leadedge/Spout2 | Source code and integration examples |
| **NDI** | Cross-platform | Network (IP) | https://ndi.video/ | High-quality video transport over IP networks |
| NDISyphon | macOS | Bridge | https://docs.vidvox.net/ndisyphon/ | Bridge between Syphon and NDI protocols |
| Blender Texture Sharing | Cross-platform | Multi-protocol | https://vjun.io/vdmo/blender-texture-sharing-spout-syphon-and-ndi-4o5g | Syphon/Spout/NDI integration for Blender |

---

## Creative Coding Platforms

Broader creative coding ecosystem tools and frameworks complementary to shader development.

| Platform | Language | URL | Description |
|----------|----------|-----|-------------|
| **Processing** | Java | https://processing.org/ | Foundational creative coding environment |
| **p5.js** | JavaScript | https://p5js.org/ | Browser-based Processing, ideal for web sketches |
| **openFrameworks** | C++ | https://openframeworks.cc/ | High-performance creative coding toolkit |
| **Three.js** | JavaScript | https://threejs.org/ | 3D graphics library for WebGL |
| **TouchDesigner** | Python/GLSL | https://derivative.ca/ | Node-based real-time multimedia platform |
| **Max/MSP/Jitter** | Visual | https://cycling74.com/products/max | Visual programming for audio/video/interactivity |

### Curated Resource Lists

| Resource | URL | Description |
|----------|-----|-------------|
| Awesome Creative Coding | https://github.com/terkelg/awesome-creative-coding | Comprehensive curated list: tools, tutorials, books, communities |
| GLSL Resources (Synesthesia) | https://app.synesthesia.live/docs/resources/glsl_resources.html | Editors, tutorials, and deep-dive articles |

---

## Video Art & Live Visuals Community

Communities, organizations, and hubs for VJs and visual artists.

| Resource | URL | Description |
|----------|-----|-------------|
| VJ Union | https://vjun.io/ | News, tools, artist showcases, and guides for VJs |
| VJ Galaxy | https://vjgalaxy.com/ | Real-time visual software reviews and resources |
| Resolume Forum | https://resolume.com/forum/ | Active community for Resolume users and developers |
| r/vjing (Reddit) | https://reddit.com/r/vjing | Reddit community for VJ discussion |
| r/creativecoding (Reddit) | https://reddit.com/r/creativecoding | Reddit community for creative coding |
| r/fractals (Reddit) | https://reddit.com/r/fractals | Reddit community for fractal art and math |
| Signal Culture | https://signalculture.org/ | Video art residencies and experimental media |

---

## Quick Reference: Technology Stack

Summary of all technologies used in this repository and their primary external resources.

| Technology | Role in Repo | Primary Resource |
|------------|-------------|------------------|
| **ISF 2.0** | Shader format | https://docs.isf.video/ |
| **GLSL** | Shader language | https://thebookofshaders.com/ |
| **OpenGL** | Graphics API | https://www.khronos.org/opengl/wiki |
| **FFGL 2.1** | Plugin API | https://github.com/resolume/ffgl |
| **C++17** | Plugin code | https://en.cppreference.com/ |
| **CMake 3.15+** | Build system | https://cmake.org/ |
| **Resolume Wire** | Primary target | https://resolume.com/training/wire |
| **GitHub Copilot** | AI assistant | https://github.blog/ai-and-ml/github-copilot/ |

---

**Last Updated:** 2026-02-08
**Maintained by:** Rune Abro / Hackstage Collective
