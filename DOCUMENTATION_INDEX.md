# Repository Documentation Index

**Quick access to all documentation and analysis files**

---

## 📊 Analysis & Overview

### [EXECUTIVE_SUMMARY.md](EXECUTIVE_SUMMARY.md)
**Quick 5-minute read** covering:
- What is this repository?
- Contents at a glance (8 shaders, FFGL system)
- Technical highlights
- Software compatibility
- Use cases and target audience

👉 **Start here** for a high-level understanding.

---

### [REPOSITORY_ANALYSIS.md](REPOSITORY_ANALYSIS.md)
**Comprehensive 20-page analysis** covering:
- Complete repository structure
- Detailed shader-by-shader breakdown
- FFGL plugin architecture
- Technical patterns and best practices
- Development workflow documentation
- Software compatibility matrix

👉 **Read this** for technical deep-dive.

---

### [docs/AI_TOOL_COMPARISON.md](docs/AI_TOOL_COMPARISON.md)
**AI Tool Evaluation** covering:
- OpenClaw vs GitHub Copilot comparison
- Speed and performance analysis for shader development
- Use case recommendations
- Cost-benefit analysis
- Hybrid workflow suggestions

👉 **Read this** if considering different AI coding assistants.

---

## 📚 Technical Documentation

### Repository Root
- **[README.md](README.md)** - About Rune Abro & Hackstage

### FFGL Plugin Development
- **[ffgl_agentic/README.md](ffgl_agentic/README.md)** - FFGL overview
- **[ffgl_agentic/docs/guides/building.md](ffgl_agentic/docs/guides/building.md)** - Build instructions
- **[ffgl_agentic/docs/guides/installing.md](ffgl_agentic/docs/guides/installing.md)** - Installation guide

### Conversion Guides
- **[docs/guides/ISF_TO_FFGL.md](docs/guides/ISF_TO_FFGL.md)** - ISF-to-FFGL conversion reference
- **[docs/FFGL_AGENTIC_SETUP.md](docs/FFGL_AGENTIC_SETUP.md)** - FFGL repository setup guide

### Shader Documentation
- **[mixers/SketchbookReveal/README.md](mixers/SketchbookReveal/README.md)** - SketchbookReveal ISF usage
- **[mixers/SketchbookReveal/CHANGELOG.md](mixers/SketchbookReveal/CHANGELOG.md)** - Version history

### Session Logs
- **[docs/sessions/sketchbook_reveal_agent_session.md](docs/sessions/sketchbook_reveal_agent_session.md)** - ISF development
- **[ffgl_agentic/docs/sessions/sketchbook_reveal_ffgl_port.md](ffgl_agentic/docs/sessions/sketchbook_reveal_ffgl_port.md)** - FFGL porting

---

## 🎨 Shader Files

### Fractal Generators (shaders/)
- **[shaders/Mandelbrot.fs](shaders/Mandelbrot.fs)** - Classic Mandelbrot set
- **[shaders/JuliaSet.fs](shaders/JuliaSet.fs)** - Animated Julia sets
- **[shaders/BurningShip.fs](shaders/BurningShip.fs)** - Burning Ship fractal
- **[shaders/FractalNoise.fs](shaders/FractalNoise.fs)** - fBM noise generator
- **[shaders/KaleidoscopeFractal.fs](shaders/KaleidoscopeFractal.fs)** - Kaleidoscope patterns
- **[shaders/SierpinskiTriangle.fs](shaders/SierpinskiTriangle.fs)** - Sierpinski triangle

### Other Generators
- **[fast-realistic-clouds-v1.3.0.fs](fast-realistic-clouds-v1.3.0.fs)** - Cloud generator

### Transition Effects (mixers/)
- **[mixers/SketchbookReveal/SketchbookReveal_v4.1.isf](mixers/SketchbookReveal/SketchbookReveal_v4.1.isf)** - ISF mixer

---

## 🔧 FFGL Plugin Source

### SketchbookReveal Plugin
- **[ffgl_agentic/source/plugins/SketchbookReveal/SketchbookReveal.h](ffgl_agentic/source/plugins/SketchbookReveal/SketchbookReveal.h)** - Header
- **[ffgl_agentic/source/plugins/SketchbookReveal/SketchbookReveal.cpp](ffgl_agentic/source/plugins/SketchbookReveal/SketchbookReveal.cpp)** - Implementation
- **[ffgl_agentic/source/plugins/SketchbookReveal/CMakeLists.txt](ffgl_agentic/source/plugins/SketchbookReveal/CMakeLists.txt)** - Build config

### Build System
- **[ffgl_agentic/CMakeLists.txt](ffgl_agentic/CMakeLists.txt)** - Root CMake config

---

## 🚀 Quick Start Guides

### For VJs (Using Shaders)
1. Read **[EXECUTIVE_SUMMARY.md](EXECUTIVE_SUMMARY.md)** → "Quick Start" section
2. Copy `.isf` or `.fs` files to Resolume ISF folder
3. Use in Resolume Wire/Arena

### For Developers (Building FFGL)
1. Read **[ffgl_agentic/docs/guides/building.md](ffgl_agentic/docs/guides/building.md)**
2. Follow build instructions for your platform
3. Install plugin to VJ software

### For Shader Developers (Converting ISF to FFGL)
1. Read **[docs/guides/ISF_TO_FFGL.md](docs/guides/ISF_TO_FFGL.md)**
2. Follow conversion steps and checklist
3. Reference SketchbookReveal as example

---

## 📈 Repository Statistics

- **Total Shaders:** 8 (ISF format)
- **FFGL Plugins:** 1 (SketchbookReveal)
- **Documentation Files:** 9+ markdown files
- **Code Size:** ~1,500 lines GLSL, ~450 lines C++
- **Supported Platforms:** Windows, macOS, Linux
- **Compatible Software:** 6+ VJ applications

---

## 🔍 Finding What You Need

**I want to...**

- **Understand what this repo is about** → [EXECUTIVE_SUMMARY.md](EXECUTIVE_SUMMARY.md)
- **Get technical details on all shaders** → [REPOSITORY_ANALYSIS.md](REPOSITORY_ANALYSIS.md)
- **Use shaders in Resolume** → [mixers/SketchbookReveal/README.md](mixers/SketchbookReveal/README.md)
- **Build FFGL plugins** → [ffgl_agentic/docs/guides/building.md](ffgl_agentic/docs/guides/building.md)
- **Convert ISF to FFGL** → [docs/guides/ISF_TO_FFGL.md](docs/guides/ISF_TO_FFGL.md)
- **Learn about AI-assisted workflow** → [docs/FFGL_AGENTIC_SETUP.md](docs/FFGL_AGENTIC_SETUP.md)
- **Compare AI coding tools** → [docs/AI_TOOL_COMPARISON.md](docs/AI_TOOL_COMPARISON.md)
- **See development history** → [docs/sessions/](docs/sessions/)

---

## 📝 License

All code and documentation: **CC BY 4.0** (Creative Commons Attribution 4.0 International)

---

**Last Updated:** 2026-02-08  
**Maintained by:** Rune Abro / Hackstage Collective
