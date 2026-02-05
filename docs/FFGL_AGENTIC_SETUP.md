# FFGL_agentic Repository Setup Guide

## Overview
This guide explains how to set up `eyelanoesis/FFGL_agentic` as a dedicated repository for FFGL (FreeFrame GL) plugin development with agentic AI workflows.

## Why Separate Repository?

**Shaders-2**: ISF shaders for Resolume Wire (GLSL-based, JSON metadata)  
**FFGL_agentic**: FFGL plugins for broader VJ software (C++ wrappers, compiled binaries)

### Key Differences
| Aspect | ISF (Shaders-2) | FFGL (FFGL_agentic) |
|--------|----------------|---------------------|
| Format | `.isf` files | `.cpp` + `.h` + GLSL |
| Runtime | Interpreted | Compiled plugins |
| Software | Resolume Wire/Arena | Resolume, VDMX, Magic, etc. |
| Workflow | Direct edit → reload | Compile → install → test |
| AI Role | Shader code generation | Full plugin scaffolding |

## Setup Steps

### Step 1: Create Repository on GitHub

1. Go to https://github.com/new
2. Repository name: `FFGL_agentic`
3. Description: "FFGL plugins for VJ software, developed with AI-assisted agentic workflows"
4. Visibility: Public (or Private if preferred)
5. Initialize with:
   - ✅ README
   - ✅ .gitignore (C++)
   - ✅ License: CC BY 4.0 (to match Shaders-2)

### Step 2: Clone and Set Up Structure

```bash
git clone https://github.com/eyelanoesis/FFGL_agentic.git
cd FFGL_agentic

# Create directory structure
mkdir -p source/plugins/SketchbookReveal
mkdir -p binaries/{win64,macos}
mkdir -p docs/{sessions,guides}
mkdir -p resources/{shaders,textures}
```

### Step 3: Add FFGL SDK

```bash
# Clone FFGL SDK as submodule
git submodule add https://github.com/resolume/ffgl.git dependencies/ffgl
git submodule update --init --recursive
```

### Step 4: Port Sketchbook Reveal Shader

Copy from Shaders-2 and adapt:

**From:** `Shaders-2/mixers/SketchbookReveal/SketchbookReveal_v4.1.isf`  
**To:** `FFGL_agentic/source/plugins/SketchbookReveal/`

Files to create:
- `SketchbookReveal.cpp` - FFGL plugin wrapper
- `SketchbookReveal.h` - Header with parameter definitions
- `SketchbookReveal.frag` - Fragment shader (pure GLSL, no ISF wrapper)
- `SketchbookReveal.vert` - Vertex shader (passthrough)
- `CMakeLists.txt` - Build configuration

### Step 5: Initial Commit

```bash
git add .
git commit -m "Initial FFGL_agentic setup with Sketchbook Reveal plugin base"
git push origin main
```

## Repository Structure

```
FFGL_agentic/
├── source/
│   └── plugins/
│       └── SketchbookReveal/
│           ├── SketchbookReveal.cpp          # FFGL plugin implementation
│           ├── SketchbookReveal.h            # Plugin header
│           ├── SketchbookReveal.frag         # Fragment shader (GLSL)
│           ├── SketchbookReveal.vert         # Vertex shader
│           └── CMakeLists.txt                # Build config
├── binaries/
│   ├── win64/                                # Compiled .dll files
│   └── macos/                                # Compiled .bundle files
├── dependencies/
│   └── ffgl/                                 # FFGL SDK (submodule)
├── docs/
│   ├── sessions/
│   │   └── sketchbook_reveal_ffgl_port.md   # Porting session log
│   └── guides/
│       ├── building.md                       # Build instructions
│       └── installing.md                     # Installation guide
├── resources/
│   ├── shaders/                              # Shared GLSL utilities
│   └── textures/                             # Test images
├── .github/
│   └── workflows/
│       └── build.yml                         # CI/CD for auto-builds
├── .gitignore
├── CMakeLists.txt                            # Root build config
├── LICENSE                                   # CC BY 4.0
└── README.md
```

## Agentic Workflow Integration

### GitHub Copilot Agent Sessions

This repository is designed for AI-assisted development:

1. **Session-Based Development**
   - Each plugin gets a `docs/sessions/[plugin_name]_session.md`
   - Logs all AI interactions, design decisions, and iterations
   - Tracks version history and lessons learned

2. **Prompt-Driven Plugin Generation**
   - Start with shader concept (e.g., "sketchbook reveal")
   - AI generates FFGL wrapper boilerplate
   - Iterative refinement based on compile/runtime feedback

3. **Automated Testing**
   - CI/CD pipeline builds plugins for Windows/macOS
   - Generates test reports
   - Creates release artifacts automatically

### Example Agent Session Workflow

```bash
# Start new plugin session
copilot session start "Create kaleidoscope FFGL effect"

# Agent generates:
# - source/plugins/Kaleidoscope/
# - Boilerplate C++ wrapper
# - Initial GLSL shader
# - CMakeLists.txt

# Iterate with feedback
copilot iterate "Add rotation parameter, make it time-synced"

# Build and test
cmake --build build --target Kaleidoscope
# Test in Resolume...

# Document session
copilot session log > docs/sessions/kaleidoscope_session.md
```

## ISF to FFGL Conversion Guide

### Key Changes When Porting from ISF

| ISF (Shaders-2) | FFGL (FFGL_agentic) |
|-----------------|---------------------|
| `/*{ JSON }*/` metadata | C++ parameter definitions in `.h` |
| `IMG_NORM_PIXEL(tex, uv)` | `texture(tex, uv)` |
| `RENDERSIZE` | `uniform vec2 resolution` (passed by host) |
| `TIME` | `uniform float time` (passed by host) |
| `isf_FragNormCoord` | `gl_FragCoord.xy / resolution` |
| Auto parameter UI | Manual `SetParamInfo()` calls |

### Conversion Steps

1. **Extract GLSL Code**
   - Remove ISF JSON wrapper
   - Replace ISF-specific functions with standard GLSL

2. **Create C++ Wrapper**
   ```cpp
   class SketchbookReveal : public CFFGLPlugin {
     // Parameter definitions
     // SetParamInfo() calls
     // ProcessOpenGL() implementation
   };
   ```

3. **Map Parameters**
   - ISF `"INPUTS"` → FFGL `SetParamInfo()`
   - ISF `progress` → FFGL `FF_TYPE_STANDARD` (0-1 range)
   - Custom params → `FF_TYPE_FLOAT`, `FF_TYPE_BOOLEAN`, etc.

4. **Handle Textures**
   - ISF multi-texture inputs → FFGL `SetInputInfo()`
   - Automatic texture binding in `ProcessOpenGL()`

5. **Build System**
   - CMake configuration for cross-platform builds
   - Link against OpenGL, FFGL SDK

## Building Plugins

### Prerequisites

**Windows:**
- Visual Studio 2019+ with C++ tools
- CMake 3.15+
- OpenGL development libraries

**macOS:**
- Xcode Command Line Tools
- CMake 3.15+
- OpenGL framework (included in macOS)

### Build Commands

```bash
# Configure
cmake -B build -S . -DCMAKE_BUILD_TYPE=Release

# Build all plugins
cmake --build build --config Release

# Build specific plugin
cmake --build build --target SketchbookReveal --config Release

# Install to system plugin folder
cmake --install build
```

### Installation Paths

**Windows:** `C:\Program Files\Common Files\FreeFrame\`  
**macOS:** `/Library/Graphics/FreeFrame Plug-Ins/`

Or copy manually to your VJ software's plugin folder:
- Resolume: `[Install Dir]/plugins/vfx/`
- VDMX: `~/Library/Application Support/VDMX/`

## Testing Workflow

1. **Build Plugin**
   ```bash
   cmake --build build --target SketchbookReveal
   ```

2. **Copy to Resolume**
   ```bash
   # Windows
   copy build\Release\SketchbookReveal.dll "C:\Program Files\Resolume Arena\plugins\vfx\"
   
   # macOS
   cp -R build/SketchbookReveal.bundle "/Applications/Resolume Arena.app/Contents/MacOS/plugins/vfx/"
   ```

3. **Test in Resolume**
   - Restart Resolume Arena
   - Add effect to layer
   - Verify parameters appear
   - Test with sample clips

4. **Debug Issues**
   - Check plugin loads: Resolume preferences → Plugins
   - Console output: Resolume menu → Show Log
   - Shader compile errors appear in log

## CI/CD Integration

Create `.github/workflows/build.yml`:

```yaml
name: Build FFGL Plugins

on: [push, pull_request]

jobs:
  build-windows:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
        with:
          submodules: recursive
      - name: Configure
        run: cmake -B build -S .
      - name: Build
        run: cmake --build build --config Release
      - name: Upload artifacts
        uses: actions/upload-artifact@v3
        with:
          name: ffgl-plugins-windows
          path: build/Release/*.dll

  build-macos:
    runs-on: macos-latest
    steps:
      - uses: actions/checkout@v3
        with:
          submodules: recursive
      - name: Configure
        run: cmake -B build -S .
      - name: Build
        run: cmake --build build --config Release
      - name: Upload artifacts
        uses: actions/upload-artifact@v3
        with:
          name: ffgl-plugins-macos
          path: build/*.bundle
```

## Agent Session Best Practices

When using AI (Grok, Copilot, etc.) for FFGL development:

1. **Start with ISF Prototype**
   - Develop shader logic in Shaders-2 first
   - Test visually in Resolume Wire
   - Port to FFGL once validated

2. **Document Everything**
   - Session logs for each plugin
   - Compilation errors and fixes
   - Parameter tuning notes

3. **Version Control**
   - Tag releases: `v1.0-SketchbookReveal`
   - Maintain CHANGELOG per plugin
   - Archive binaries with tags

4. **Iterative Refinement**
   - Build → Test → Feedback loop
   - Track regressions (like Shaders-2 v1.9 issue)
   - Keep working versions tagged

5. **Cross-Reference**
   - Link FFGL plugins to original ISF shaders
   - Note differences in implementation
   - Share learnings between repos

## Resources

- **FFGL SDK**: https://github.com/resolume/ffgl
- **FFGL Specification**: https://github.com/resolume/ffgl/wiki
- **OpenGL Documentation**: https://www.khronos.org/opengl/wiki
- **CMake Tutorial**: https://cmake.org/cmake/help/latest/guide/tutorial/
- **Resolume Plugin Development**: https://resolume.com/support/en/creating-effects-for-resolume

## Next Steps

1. ✅ Create `eyelanoesis/FFGL_agentic` repository
2. ✅ Clone and set up structure
3. ✅ Add FFGL SDK submodule
4. ⏳ Port Sketchbook Reveal to FFGL
5. ⏳ Set up CI/CD pipeline
6. ⏳ Document porting session
7. ⏳ Build and test in Resolume

## Support

For issues or questions:
- **ISF Shaders**: Open issue in `Shaders-2`
- **FFGL Plugins**: Open issue in `FFGL_agentic`
- **General**: Discuss in repo Discussions

---

**Created:** 2026-02-05  
**Agent:** Grok (xAI)  
**Session:** SketchReveal_AgentSession_001 (continuation)
