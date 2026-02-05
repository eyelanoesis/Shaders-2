# Getting Started with ISF Shaders

Welcome to the Shaders-2 repository! This guide will help you start creating and using ISF shaders for fractal visualizations.

## Quick Start

### Step 1: Choose Your Tool

#### Option A: ISF Editor (Recommended for Beginners)
1. Visit https://isf.video/
2. Download the ISF Editor for your platform (Mac/Windows/Linux)
3. Open any `.fs` file from the `shaders/` directory

#### Option B: Text Editor + Testing
1. Use any code editor (VS Code, Sublime Text, etc.)
2. Test in ISF-compatible software (VDMX, Resolume, etc.)

### Step 2: Try an Example

Open `shaders/JuliaSet.fs` in the ISF Editor:

1. Click "File" → "Open" 
2. Navigate to the shader file
3. The preview window shows the live output
4. Adjust sliders to change parameters

### Step 3: Experiment

Try modifying these values:

```glsl
"DEFAULT": -0.73,  // Change to -0.4 or -0.8
"MIN": -2.0,       // Expand the range
"MAX": 2.0,        // Or restrict it
```

Save and watch the changes in real-time!

## Understanding Shader Parameters

### View Controls

Most fractal shaders have navigation parameters:

- **Center Position** (`viewCenterX`, `focusX`, etc.)
  - Controls where you're looking
  - Negative values go left/down, positive go right/up

- **Zoom/Magnification** (`magnification`, `zoomLevel`, `viewScale`)
  - Higher values zoom in
  - Lower values zoom out
  - Start with default and experiment

### Quality Settings

- **Iteration Limits** (`maxSteps`, `iterationLimit`, `computeSteps`)
  - Higher = more detail, slower performance
  - Lower = faster, less detail
  - Recommended range: 100-300 for most uses

### Visual Controls

- **Color Parameters**
  - `hueRotation`, `colorCycle`, `paletteShift` - Change colors
  - `saturationLevel`, `brightnessBoost` - Adjust intensity
  - `colorFrequency`, `colorVariation` - Pattern frequency

## Working with Different Shaders

### 1. Mandelbrot Set (`Mandelbrot.fs`)

**Best For**: Classic fractal exploration

**Starting Point**:
- viewCenterX: -0.6
- viewCenterY: 0.0  
- magnification: 1.2

**Interesting Locations**:
- Center: (-0.75, 0.0) - Main bulb
- Spiral: (-0.7, 0.3) - Spiral patterns
- Deep zoom: (-0.761, 0.091) - Intricate details

### 2. Julia Set (`JuliaSet.fs`)

**Best For**: Exploring different constants

**Starting Point**:
- paramReal: -0.73
- paramImaginary: 0.19

**Try These Constants**:
- (-0.8, 0.156) - Dragon-like
- (-0.4, 0.6) - Dendrite
- (0.285, 0.01) - Spiral
- (-0.7, 0.27) - Classic

### 3. Burning Ship (`BurningShip.fs`)

**Best For**: Unique ship-like patterns

**Starting Point**:
- focusX: -0.5
- focusY: -0.6
- zoomLevel: 0.8

**Notable Features**:
- Ship shape in main view
- Lots of angular detail
- Different from Mandelbrot

### 4. Fractal Noise (`FractalNoise.fs`)

**Best For**: Organic textures and backgrounds

**Starting Point**:
- layerCount: 6
- baseFrequency: 2.5
- timeInfluence: 0.3 (for animation)

**Use Cases**:
- Background textures
- Cloud patterns
- Abstract animations
- Overlay effects

### 5. Sierpinski Triangle (`SierpinskiTriangle.fs`)

**Best For**: Geometric patterns

**Starting Point**:
- recursionDepth: 7
- rotationSpeed: 0.2

**Features**:
- Clean geometric lines
- Rotates over time
- Recursive structure

### 6. Kaleidoscope Fractal (`KaleidoscopeFractal.fs`)

**Best For**: Symmetrical patterns

**Starting Point**:
- segmentCount: 8 (like an octagon)
- reflectionDepth: 4

**Experimentation**:
- Try segmentCount: 6 (hexagonal)
- Try segmentCount: 12 (dodecagonal)
- Adjust spiralTwist for swirling effects

## Creating Your First Shader

### Template

Create a new file `MyFirstShader.fs`:

```glsl
/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "My first ISF shader",
    "CREDIT": "Your Name",
    "CATEGORIES": ["Generator"],
    "INPUTS": [
        {
            "NAME": "intensity",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        }
    ]
}*/

void main() {
    vec2 coord = isf_FragNormCoord;
    
    // Simple gradient based on position
    vec3 color = vec3(coord.x, coord.y, intensity);
    
    gl_FragColor = vec4(color, 1.0);
}
```

### Building Up Complexity

1. **Start Simple**: Get something working first
2. **Add Parameters**: Make it interactive
3. **Add Animation**: Use `TIME` variable
4. **Refine Colors**: Implement HSV conversion
5. **Optimize**: Reduce calculations

## Common Modifications

### Change Color Schemes

Replace the hue calculation:

```glsl
// Original
float hue = fract(intensity * 3.0);

// Rainbow effect
float hue = fract(intensity * 5.0 + TIME * 0.1);

// Monochrome to color
float hue = mix(0.0, 0.7, intensity);
```

### Add Time Animation

```glsl
// Rotating view
float angle = TIME * rotationSpeed;
vec2 rotated = mat2(cos(angle), -sin(angle), 
                     sin(angle), cos(angle)) * coord;

// Pulsing effect
float pulse = 0.5 + 0.5 * sin(TIME * 2.0);
magnitude *= pulse;

// Moving center
vec2 center = vec2(sin(TIME * 0.3), cos(TIME * 0.2)) * 0.5;
```

### Adjust Performance

```glsl
// Dynamic iteration limit based on distance
float distFromCenter = length(screenPos);
int iterations = int(mix(50.0, 200.0, distFromCenter));

// Early exit for performance
if(stepCount > maxIterations * 0.5 && magnitude < 0.01) break;
```

## Troubleshooting

### "Shader won't compile"

**Check**:
1. Is the JSON block properly closed with `}*/`?
2. Are all GLSL statements ending with `;`?
3. Are variable types correct (float vs int)?
4. Are all variables declared before use?

### "Output is black"

**Check**:
1. Is `gl_FragColor` being set?
2. Are color values in range 0.0-1.0?
3. Is there a logic error preventing execution?
4. Try outputting a simple test color first

### "Output is distorted"

**Check**:
1. Aspect ratio correction applied?
2. Coordinates normalized properly?
3. Parameters in reasonable ranges?

### "Performance is slow"

**Solutions**:
1. Lower iteration limits
2. Reduce octave counts
3. Simplify calculations inside loops
4. Use lower resolution preview

## Next Steps

### Learn More

1. Read `docs/ISF_GUIDE.md` for detailed documentation
2. Study the shader code to understand techniques
3. Experiment with parameter values
4. Combine multiple effects

### Advanced Topics

- Multi-pass rendering
- Feedback effects
- Audio reactivity
- Custom image processing
- Vertex shaders
- Buffer persistence

### Resources

- **ISF Specification**: Full technical details
- **GLSL Reference**: OpenGL shading language
- **Fractal Mathematics**: Complex dynamics, IFS
- **Community Forums**: Share and get help

### Practice Projects

1. **Modify Colors**: Change all shaders to use your favorite palette
2. **Create Variations**: Combine elements from different shaders
3. **Add Features**: Implement new parameters or effects
4. **Optimize**: Make shaders faster without losing quality
5. **Original Creation**: Build something completely new

## Tips for Success

1. **Save Often**: Keep versions as you experiment
2. **Start Small**: Don't try to do everything at once
3. **Test Frequently**: See results as you make changes
4. **Read Examples**: Learn from existing shaders
5. **Experiment Freely**: There's no wrong way to explore

## Getting Help

If you get stuck:

1. Check the ISF specification for format details
2. Review the shader code comments
3. Test with simpler values
4. Look for similar examples online
5. Ask in ISF community forums

---

**Ready to create?** Open the ISF Editor and start experimenting with the shaders in this repository!
