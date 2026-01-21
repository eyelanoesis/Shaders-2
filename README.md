# Shaders-2

A collection of ISF (Interactive Shader Format) shaders for use in VJ software, live visuals, and creative coding environments.

## About ISF

ISF (Interactive Shader Format) is an open-source file format for GLSL fragment shaders that includes a JSON metadata header describing inputs and parameters. This makes shaders portable and easy to use across different host applications like VDMX, Resolume, Magic Music Visuals, and more.

## Shader Categories

### Generators

Generators create visual patterns from scratch without requiring an input image.

| Shader | Description |
|--------|-------------|
| **ColorGradient.fs** | Generates a smooth color gradient between two colors with adjustable angle and offset |
| **Plasma.fs** | Classic animated plasma effect with customizable speed, scale, and complexity |
| **Checkerboard.fs** | Animated checkerboard pattern with rotation and offset controls |
| **CircleWaves.fs** | Concentric animated circles emanating from a configurable center point |
| **Starfield.fs** | Animated space starfield with multiple layers and twinkling stars |
| **RadialBurst.fs** | Pulsing radial burst pattern with configurable rays and colors |

### Effects

Effects process an input image to create visual transformations.

| Shader | Description |
|--------|-------------|
| **BrightnessContrast.fs** | Adjusts brightness, contrast, and saturation of the input |
| **GaussianBlur.fs** | Applies a Gaussian blur with adjustable amount and quality |
| **ChromaticAberration.fs** | Creates RGB channel splitting for a glitch/lens effect |
| **EdgeDetection.fs** | Sobel edge detection with customizable colors and threshold |
| **Pixelate.fs** | Mosaic/pixelation effect with square, circle, or diamond shapes |
| **Vignette.fs** | Adds a vignette darkening effect around the edges |

## Usage

1. Copy the `.fs` files to your ISF-compatible application's shader folder
2. The shaders will appear in the application with their controls automatically generated from the JSON metadata
3. Adjust parameters in real-time to customize the visual output

### Common ISF Host Applications

- [VDMX](https://vidvox.net/vdmx/)
- [Resolume](https://resolume.com/)
- [Magic Music Visuals](https://magicmusicvisuals.com/)
- [Isadora](https://troikatronix.com/)
- [CoGe](https://imimot.com/cogevj/)

## File Structure

```
Shaders-2/
├── generators/          # Pattern generators (no input required)
│   ├── Checkerboard.fs
│   ├── CircleWaves.fs
│   ├── ColorGradient.fs
│   ├── Plasma.fs
│   ├── RadialBurst.fs
│   └── Starfield.fs
├── effects/             # Image effects (require input image)
│   ├── BrightnessContrast.fs
│   ├── ChromaticAberration.fs
│   ├── EdgeDetection.fs
│   ├── GaussianBlur.fs
│   ├── Pixelate.fs
│   └── Vignette.fs
└── README.md
```

## Testing Shaders

You can test these shaders using the online ISF Editor at [editor.isf.video](https://editor.isf.video/)

## Resources

- [ISF Specification](https://isf.video/)
- [ISF Documentation](https://vidvox.github.io/isf/)
- [ISF Shader Library](https://isf.vidvox.net/)

## License

These shaders are provided as open source for creative use