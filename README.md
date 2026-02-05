# Shaders-2

A collection of ISF (Interactive Shader Format) shaders featuring fractal-based visuals for creative coding, VJing, and live visuals.

## What is ISF?

ISF (Interactive Shader Format) is an open-source standard for creating GPU-accelerated visual effects. It combines GLSL fragment shaders with JSON metadata that describes interactive parameters, making shaders portable across many software platforms including VDMX, Resolume, TouchDesigner, and more.

## Included Shaders

### Fractal Visualizations

| Shader | Description |
|--------|-------------|
| **Mandelbrot.fs** | Classic Mandelbrot set fractal with zoom, pan, and multiple color schemes |
| **JuliaSet.fs** | Julia set fractal with animated c parameter and interactive controls |
| **BurningShip.fs** | Burning Ship fractal variant with scientific colormaps (Inferno, Plasma, Viridis) |
| **SierpinskiTriangle.fs** | Classic Sierpinski Triangle fractal using iterative space folding |
| **KaleidoscopeFractal.fs** | Kaleidoscopic fractal patterns with multiple visual styles |
| **FractalNoise.fs** | Fractal Brownian Motion (fBM) noise generator with multiple noise types |

### Features

- **Interactive Controls**: All shaders include adjustable parameters for real-time manipulation
- **Color Schemes**: Multiple color palettes including Rainbow, Fire, Ocean, and scientific colormaps
- **Animation**: Built-in animation support with adjustable speed controls
- **Zoom & Pan**: Navigate deep into fractal structures
- **Cross-Platform**: Compatible with any ISF-supporting application

## Usage

### With ISF-Compatible Software

1. Copy the `.fs` files from the `shaders/` directory to your application's ISF folder:
   - **macOS**: `~/Library/Graphics/ISF/`
   - **Windows**: `%APPDATA%\ISF\`
2. Reload your application or rescan for new shaders
3. The shaders will appear in your shader browser with all controls available

### With Online ISF Editor

1. Visit [editor.isf.video](https://editor.isf.video)
2. Copy and paste the shader code into the editor
3. Adjust parameters in real-time and see the results

### Supported Applications

- [VDMX](https://vidvox.net/vdmx/)
- [Resolume](https://resolume.com/)
- [TouchDesigner](https://derivative.ca/)
- [Magic Music Visuals](https://magicmusicvisuals.com/)
- [CoGe](https://imimot.com/cogevj/)
- Any ISF-compatible software

## Shader Parameters

### Mandelbrot Set

- `zoom` - Magnification level (0.1 - 100.0)
- `centerX`, `centerY` - Pan position in the complex plane
- `maxIterations` - Detail level (10 - 500)
- `colorCycle` - Animate through color palette
- `colorIntensity` - Color repetition frequency
- `colorScheme` - Rainbow, Fire, Ocean, or Grayscale

### Julia Set

- All Mandelbrot parameters plus:
- `cReal`, `cImag` - Julia constant in the complex plane
- `animateC` - Toggle animated c parameter
- `animSpeed` - Speed of c animation

### Burning Ship

- Similar to Mandelbrot with:
- `invertY` - Flip vertically to show "ship" upright
- Scientific colormaps: Inferno, Plasma, Viridis, Grayscale

### Fractal Noise (fBM)

- `scale` - Noise scale
- `octaves` - Number of noise layers (1 - 8)
- `persistence` - Amplitude decay between octaves
- `lacunarity` - Frequency multiplier between octaves
- `noiseType` - Value Noise, Simplex-like, or Turbulence

### Sierpinski Triangle

- `iterations` - Recursion depth
- `thickness` - Line/edge thickness
- `fillColor`, `backgroundColor` - Custom colors
- `colorByDepth` - Color based on iteration depth

### Kaleidoscope Fractal

- `segments` - Number of kaleidoscope segments (3 - 16)
- `twist` - Radial twist amount
- `patternType` - Spiral, Flower, Crystal, or Hyperbolic

## Resources

- [ISF Documentation](https://docs.isf.video/)
- [ISF Specification](https://github.com/mrRay/ISF_Spec)
- [Online ISF Editor](https://editor.isf.video)
- [ISF Community Shaders](https://isf.video/shaders)

## License

These shaders are provided for creative and educational use. Feel free to modify and incorporate them into your projects.