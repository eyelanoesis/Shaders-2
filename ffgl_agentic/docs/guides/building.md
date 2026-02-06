# Building FFGL Plugins

This guide covers building FFGL plugins from source.

## Prerequisites

### Windows

1. **Visual Studio 2019 or later**
   - Install "Desktop development with C++" workload
   - Includes MSVC compiler and Windows SDK

2. **CMake 3.15+**
   - Download from https://cmake.org/download/
   - Add to PATH during installation

3. **Git**
   - For cloning and managing submodules
   - Download from https://git-scm.com/

### macOS

1. **Xcode Command Line Tools**
   ```bash
   xcode-select --install
   ```

2. **CMake 3.15+**
   ```bash
   brew install cmake
   ```

3. **Git** (usually pre-installed)
   ```bash
   git --version
   ```

## Setup

### 1. Clone Repository

```bash
git clone https://github.com/eyelanoesis/Shaders-2.git
cd Shaders-2/ffgl_agentic
```

### 2. Initialize FFGL SDK Submodule

```bash
git submodule add https://github.com/resolume/ffgl.git dependencies/ffgl
git submodule update --init --recursive
```

This downloads the official FFGL SDK from Resolume.

## Building

### Windows (Visual Studio)

```bash
# Configure (generates .sln file)
cmake -B build -S . -G "Visual Studio 17 2022"

# Build Release
cmake --build build --config Release

# Build Debug (for development)
cmake --build build --config Debug
```

Output: `build/Release/SketchbookReveal.dll`

### Windows (MinGW)

```bash
cmake -B build -S . -G "MinGW Makefiles"
cmake --build build --config Release
```

### macOS

```bash
# Configure
cmake -B build -S .

# Build
cmake --build build --config Release
```

Output: `build/SketchbookReveal.bundle`

### Linux (if supported by host)

```bash
cmake -B build -S .
make -C build
```

## Building Single Plugin

To build only one plugin instead of all:

```bash
cmake --build build --target SketchbookReveal --config Release
```

## Build Options

### Debug vs Release

**Release** (default):
- Optimized for performance
- No debug symbols
- Use for distribution

**Debug**:
- Includes debug symbols
- Easier to debug crashes
- Slower performance

```bash
cmake --build build --config Debug
```

### Parallel Builds

Speed up compilation:

**Windows:**
```bash
cmake --build build --config Release -- /m
```

**macOS/Linux:**
```bash
cmake --build build --config Release -- -j8
```

## Troubleshooting

### CMake can't find OpenGL

**Windows:**
- Install/update Visual Studio with Windows SDK

**macOS:**
- OpenGL is built-in, check Xcode tools are installed

**Linux:**
```bash
sudo apt-get install libgl1-mesa-dev
```

### Submodule not initialized

```bash
git submodule update --init --recursive
```

### Build fails with "FFGL.h not found"

Check that submodule path is correct:
```bash
ls dependencies/ffgl/source/FFGL.h
```

If missing, re-initialize submodule.

### Wrong Visual Studio version

Change generator:
```bash
cmake -B build -S . -G "Visual Studio 16 2019"  # VS 2019
cmake -B build -S . -G "Visual Studio 17 2022"  # VS 2022
```

List available generators:
```bash
cmake --help
```

## Clean Build

Remove build artifacts and start fresh:

```bash
rm -rf build
cmake -B build -S .
cmake --build build --config Release
```

## Next Steps

- [Installation Guide](installing.md) - Install built plugins
- [Testing](#) - Test plugins in VJ software

## Advanced

### Custom Install Prefix

```bash
cmake -B build -S . -DCMAKE_INSTALL_PREFIX=/path/to/install
cmake --install build
```

### Cross-compilation

Requires cross-compilation toolchain setup (advanced users).

---

**Issues?** Open an issue on GitHub with:
- Your OS and version
- CMake version (`cmake --version`)
- Full error output
