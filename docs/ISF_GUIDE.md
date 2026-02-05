# ISF Shader Guide

## Introduction to ISF Format

ISF (Interactive Shader Format) is a standardized format for creating real-time visual effects shaders. This guide covers the fundamentals and provides examples from this repository.

## ISF File Structure

Every ISF shader consists of two parts:

### 1. JSON Metadata Block

The metadata appears at the top of the file as a comment block:

```glsl
/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "What your shader does",
    "CREDIT": "Your name or organization",
    "CATEGORIES": ["Category1", "Category2"],
    "INPUTS": [
        // Parameter definitions here
    ]
}*/
```

### 2. GLSL Shader Code

Standard OpenGL Shading Language code that implements the visual effect.

## Parameter Types

ISF supports various input types for user interaction:

### Float Parameters

```json
{
    "NAME": "magnification",
    "TYPE": "float",
    "DEFAULT": 1.2,
    "MIN": 0.01,
    "MAX": 100.0
}
```

### Color Parameters

```json
{
    "NAME": "tintColor",
    "TYPE": "color",
    "DEFAULT": [1.0, 0.5, 0.0, 1.0]
}
```

### Image Inputs

```json
{
    "NAME": "inputImage",
    "TYPE": "image"
}
```

### Point Parameters

```json
{
    "NAME": "centerPoint",
    "TYPE": "point2D",
    "DEFAULT": [0.5, 0.5]
}
```

## Built-in Variables

ISF provides several built-in variables:

- `isf_FragNormCoord` - Normalized fragment coordinates (0.0 to 1.0)
- `RENDERSIZE` - Output resolution as vec2
- `TIME` - Current time in seconds
- `TIMEDELTA` - Time since last frame
- `FRAMEINDEX` - Current frame number

## Shader Examples from This Repository

### Mandelbrot Fractal

Located at `shaders/Mandelbrot.fs`

This shader renders the classic Mandelbrot set with:
- Adjustable view center and magnification
- Dynamic color rotation
- Smooth iteration coloring
- Saturation controls

Key parameters:
- `viewCenterX`, `viewCenterY` - Navigate the fractal
- `magnification` - Zoom in/out
- `maxSteps` - Iteration limit (affects detail)
- `hueRotation` - Rotate the color palette

### Julia Set

Located at `shaders/JuliaSet.fs`

Interactive Julia set visualization with:
- Real-time parameter morphing
- Custom bailout threshold
- Time-based color cycling
- Interior shading

Key parameters:
- `paramReal`, `paramImaginary` - Define the Julia constant
- `viewScale` - Zoom level
- `colorCycle` - Color animation speed

### Burning Ship

Located at `shaders/BurningShip.fs`

The Burning Ship fractal with unique rendering:
- Absolute value transformation
- Custom color palettes
- Focus and zoom controls

This fractal differs from Mandelbrot by using absolute values before squaring.

### Fractal Noise

Located at `shaders/FractalNoise.fs`

Multi-octave noise generator featuring:
- Layered noise accumulation
- Frequency and amplitude control
- Time-based animation
- Smooth interpolation

Parameters:
- `layerCount` - Number of octaves
- `baseFrequency` - Initial frequency
- `amplitudeDecay` - How quickly layers fade
- `frequencyGrowth` - Frequency multiplier per layer

### Sierpinski Triangle

Located at `shaders/SierpinskiTriangle.fs`

Recursive geometric fractal with:
- Space folding techniques
- Rotation animation
- Edge detection
- Dynamic coloring

### Kaleidoscope Fractal

Located at `shaders/KaleidoscopeFractal.fs`

Symmetrical pattern generator with:
- Angular folding
- Spiral transformations
- Multiple reflection layers
- Pattern accumulation

## Color Space Conversion

All shaders in this repository use a consistent HSV to RGB conversion function:

```glsl
vec3 hsvColorTransform(vec3 hsv) {
    vec4 factors = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 adjusted = abs(fract(hsv.xxx + factors.xyz) * 6.0 - factors.www);
    return hsv.z * mix(factors.xxx, clamp(adjusted - factors.xxx, 0.0, 1.0), hsv.y);
}
```

This allows for dynamic color generation based on fractal properties.

## Smooth Iteration Coloring

For escape-time fractals, smooth coloring eliminates banding:

```glsl
float smoothValue = float(iteration) - log2(log2(magnitude));
```

This technique provides continuous color gradients across iteration boundaries.

## Performance Considerations

### Iteration Limits

Higher `maxSteps` or `iterationLimit` values:
- ✅ Increase detail
- ❌ Reduce performance

Start with lower values and increase as needed.

### Complexity vs Quality

Balance detail with frame rate:
- Use fewer octaves for real-time performance
- Reduce recursion depth on slower hardware
- Adjust iteration limits dynamically

### Aspect Ratio Correction

Always correct for aspect ratio to prevent distortion:

```glsl
vec2 aspectCorrected = coord * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0);
```

## Using These Shaders

### Supported Applications

ISF shaders work in:
- VDMX
- Resolume
- CoGe VJ
- MadMapper
- TouchDesigner (with ISF loader)
- Vuo
- Various video editing software

### Testing

Use the ISF Editor (https://isf.video/) to:
- Test shaders in real-time
- Adjust parameters interactively
- Export for use in production

### Modification Tips

1. **Change Parameters**: Adjust MIN/MAX ranges and DEFAULT values
2. **Color Schemes**: Modify the HSV values in color generation
3. **Animation**: Add TIME-based transformations
4. **Combine Effects**: Layer multiple transformation techniques

## Mathematical Background

### Complex Number Iteration

Fractals like Mandelbrot and Julia sets use complex number iteration:

```
z_{n+1} = z_n^2 + c
```

In GLSL:
```glsl
float newReal = z.x * z.x - z.y * z.y + c.x;
float newImag = 2.0 * z.x * z.y + c.y;
```

### Space Folding

Geometric fractals use space folding:

```glsl
point = abs(point);  // Mirror across axes
point = point.yx;     // Swap coordinates
```

### Noise Functions

Fractal noise combines multiple frequencies:

```glsl
for each octave:
    noise += perlin(coord * freq) * amplitude
    freq *= growth
    amplitude *= decay
```

## Troubleshooting

### Shader Won't Compile

- Check JSON syntax in metadata
- Ensure all braces match
- Verify semicolons after statements
- Check variable declarations

### Poor Performance

- Reduce iteration limits
- Lower octave/layer counts
- Simplify color calculations
- Use lower resolution

### Unexpected Colors

- Verify HSV values are in range [0,1]
- Check color transformation function
- Ensure proper normalization

### Distorted Output

- Apply aspect ratio correction
- Center coordinates properly
- Check coordinate transformations

## Further Resources

- ISF Specification: https://github.com/mrRay/ISF_Spec
- ISF Editor: https://isf.video/
- GLSL Reference: https://www.khronos.org/opengl/wiki/OpenGL_Shading_Language
- Fractal Mathematics: Books on complex dynamics and iterated function systems

## Contributing

When adding new shaders to this repository:

1. Follow the ISF 2.0 specification
2. Include comprehensive parameter descriptions
3. Use consistent naming conventions
4. Document any unique algorithms
5. Test across multiple applications
6. Provide usage examples

---

*This guide corresponds to the shaders in the `/shaders` directory of this repository.*
