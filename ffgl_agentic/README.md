# FFGL_agentic

FFGL plugins for VJ software, developed with AI-assisted agentic workflows.

## Overview

This directory contains compiled FFGL (FreeFrame GL) plugins ported from the ISF shaders in `../mixers/`.

**ISF (../mixers/)**: Fast prototyping, Resolume-only  
**FFGL (this directory)**: Production plugins, works in Resolume, VDMX, Magic, CoGe, etc.

## Current Plugins

- **SketchbookReveal** - Content-aware sketch reveal transition with edge detection

## Building

### Prerequisites

**Windows:**
- Visual Studio 2019+ with C++ tools
- CMake 3.15+
- FFGL SDK (submodule)

**macOS:**
- Xcode Command Line Tools
- CMake 3.15+
- FFGL SDK (submodule)

### Setup FFGL SDK

```bash
cd ffgl_agentic
git submodule add https://github.com/resolume/ffgl.git dependencies/ffgl
git submodule update --init --recursive
```

### Build Commands

```bash
cd ffgl_agentic

# Configure
cmake -B build -S .

# Build all plugins
cmake --build build --config Release

# Install
cmake --install build
```

### Build Single Plugin

```bash
cmake --build build --target SketchbookReveal --config Release
```

## Installing

**Windows:**
Copy `.dll` to `C:\Program Files\Resolume Arena\plugins\vfx\`

**macOS:**
Copy `.bundle` to `/Applications/Resolume Arena.app/Contents/MacOS/plugins/vfx/`

Or use your VJ software's custom plugin folder.

## Testing

1. Build plugin
2. Copy to VJ software plugin folder
3. Restart software
4. Effect should appear in effects list

## Documentation

- [Building Guide](docs/guides/building.md)
- [Installation Guide](docs/guides/installing.md)
- [Session Logs](docs/sessions/)

## Related

- **ISF Shaders**: See `../mixers/` for Resolume Wire versions
- **Conversion Guide**: See `../docs/guides/ISF_TO_FFGL.md`

## License

CC BY 4.0 (Creative Commons Attribution 4.0 International)
