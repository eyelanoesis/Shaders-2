# GitHub Copilot Instructions for Shaders-2

This repository contains ISF (Interactive Shader Format) shaders and FFGL (FreeFrame GL) plugins for VJ software like Resolume.

## Repository Structure

- **`shaders/`** - ISF fractal generator shaders (.fs files)
- **`mixers/`** - ISF transition/mixer shaders (.isf files)
- **`ffgl_agentic/`** - FFGL C++ plugin implementations
- **`docs/`** - Documentation and conversion guides
  - `docs/guides/ISF_TO_FFGL.md` - Comprehensive ISF to FFGL conversion reference
  - `docs/FFGL_AGENTIC_SETUP.md` - Repository setup guide

## ISF Shader Format Standards

### File Structure
ISF shaders use JSON metadata wrapped in `/*{ ... }*/` comments at the top of the file, followed by GLSL code.

### Required Metadata Fields
```json
{
  "ISFVSN": "2.0",
  "DESCRIPTION": "Brief description of the shader",
  "CREDIT": "Author or source",
  "CATEGORIES": ["Generator/Effect/Mixer/Fractal"],
  "INPUTS": [...]
}
```

**Important:** Use `"ISFVSN": "2.0"` (not `"2"`) as a string value.

### Input Types
- `float` - Numeric parameter with MIN, MAX, DEFAULT
- `bool` - Boolean toggle
- `long` - Dropdown selection with VALUES and LABELS
- `color` - RGB color picker
- `image` - Input texture (use TYPE: "image")
- `point2D` - X/Y coordinate pair

### ISF-Specific GLSL Built-ins
- `RENDERSIZE` - vec2 resolution (use this, NOT `RESOLUTION` or `tex_2D_size`)
- `TIME` - float elapsed time
- `isf_FragNormCoord` - vec2 normalized fragment coordinates (0-1)
- `IMG_NORM_PIXEL(texture, uv)` - Texture sampling with normalized coordinates
- `gl_FragColor` - Output color

### Coordinate System
Always use normalized coordinates:
```glsl
vec2 uv = isf_FragNormCoord;  // 0-1 range
// OR
vec2 uv = gl_FragCoord.xy / RENDERSIZE;
```

## Shader Categories and Organization

### Generators (shaders/)
- No input images required
- Generate patterns from parameters and time
- Examples: Mandelbrot, JuliaSet, FractalNoise

### Effects
- Require `inputImage` input
- Process/filter incoming video

### Mixers (mixers/)
Must have exactly these inputs:
- `startImage` (TYPE: "image") - First clip
- `endImage` (TYPE: "image") - Second clip  
- `progress` (TYPE: "float", MIN: 0.0, MAX: 1.0) - Transition progress

## Naming Conventions

- Use **PascalCase** for shader file names: `Mandelbrot.fs`, `SketchbookReveal.isf`
- Use **camelCase** for parameter names: `hatchDensity`, `colorScheme`, `maxIterations`
- Include version numbers when iterating: `SketchbookReveal_v4.1.isf`

## Code Style

### Utility Functions
Utility functions like `hsv2rgb()` are duplicated across shader files rather than shared. This is intentional for ISF compatibility (each shader must be self-contained).

### Comments
Add descriptive comments for complex algorithms (edge detection, fractal math, etc.) but keep simple parameter operations uncommented.

## FFGL Plugin Development

### Structure
FFGL plugins are C++ wrappers around GLSL shaders located in `ffgl_agentic/source/plugins/`.

Each plugin has:
- `.h` header - Class definition, parameter enums
- `.cpp` implementation - Plugin initialization, parameter handling, shader loading
- `.frag` shader file - GLSL fragment shader code
- `CMakeLists.txt` - Build configuration

### Key Conversions from ISF to FFGL

| ISF | FFGL |
|-----|------|
| `IMG_NORM_PIXEL(tex, uv)` | `texture(inputTexture, uv)` |
| `RENDERSIZE` | `uniform vec2 resolution;` |
| `TIME` | `uniform float time;` |
| `isf_FragNormCoord` | `gl_FragCoord.xy / resolution` |

### Parameter Handling
FFGL parameters are normalized 0-1 and mapped to shader-specific ranges in `ProcessOpenGL()`:
```cpp
// Example: Map 0-1 to 5.0-90.0 range
float actualValue = paramValue * 85.0 + 5.0;
```

### Build System
- **CMake 3.15+** required
- **OpenGL** dependency
- **FFGL SDK** submodule at `dependencies/ffgl`
- Outputs: `.dll` (Windows) or `.bundle` (macOS)

Build commands:
```bash
cmake -B build -S .
cmake --build build --config Release
cmake --install build
```

## Documentation Standards

- Keep READMEs for each shader explaining features, parameters, and usage
- Include recommended settings for different use cases
- Document version numbers and changes in CHANGELOG files
- Reference sources and inspirations

## Testing Workflow

ISF shaders:
1. Edit shader file
2. Save changes
3. Reload in Resolume (no build step needed)

FFGL plugins:
1. Build plugin with CMake
2. Copy to VJ software plugin folder
3. Restart software
4. Test effect in effects list

## Best Practices

1. **Self-contained shaders** - Each ISF file must include all required utility functions
2. **Parameter ranges** - Set sensible MIN/MAX/DEFAULT values for intuitive control
3. **Performance** - Test shader performance on target hardware, especially with multiple layers
4. **Documentation** - Document shader purpose, parameters, and best practices in README files
5. **Version tracking** - Use semantic versioning in filenames for significant iterations
6. **Conversion accuracy** - When porting ISF to FFGL, carefully follow the conversion guide in `docs/guides/ISF_TO_FFGL.md`

## AI-Assisted Development

This repository uses agentic AI workflows for code generation. When working with AI:
- Reference the comprehensive ISF_TO_FFGL.md guide for conversions
- Verify shader math algorithms (fractals, edge detection) are correctly implemented
- Test parameter ranges produce expected visual results
- Ensure ISF metadata is complete and accurate
