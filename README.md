# Shaders-2

A collection of ISF (Interactive Shader Format) shaders for fractal-based visual effects and generative art.

## 🎨 Shader Collection

### Available Shaders

| Shader | Description | File |
|--------|-------------|------|
| **Mandelbrot** | Classic Mandelbrot set with smooth coloring | `shaders/Mandelbrot.fs` |
| **Julia Set** | Interactive Julia set with morphing parameters | `shaders/JuliaSet.fs` |
| **Burning Ship** | Unique fractal with angular ship-like patterns | `shaders/BurningShip.fs` |
| **Fractal Noise** | Multi-octave procedural noise generator | `shaders/FractalNoise.fs` |
| **Sierpinski** | Animated geometric triangle fractal | `shaders/SierpinskiTriangle.fs` |
| **Kaleidoscope** | Symmetrical patterns with spiral transforms | `shaders/KaleidoscopeFractal.fs` |

## 📚 Documentation

- **[Getting Started Guide](./docs/GETTING_STARTED.md)** - Begin creating with ISF shaders
- **[ISF Guide](./docs/ISF_GUIDE.md)** - Comprehensive ISF format documentation
- **[Project Notes](./PROJECT_NOTES.md)** - Development history and session log

## 🚀 Quick Start

1. Download the [ISF Editor](https://isf.video/)
2. Open any shader from the `shaders/` directory
3. Adjust parameters in real-time
4. Use in VJ software (VDMX, Resolume, etc.)

## 🎯 Key Features

- **ISF 2.0 Compatible** - Works with all major VJ and video software
- **Interactive Parameters** - Real-time control over all visual aspects
- **Smooth Coloring** - High-quality gradient rendering
- **Performance Optimized** - Adjustable quality settings
- **Well Documented** - Comprehensive guides and examples

## 🔧 Usage Examples

### In ISF Editor
```bash
# Open editor, load shader, adjust sliders
File → Open → shaders/JuliaSet.fs
```

### In VDMX/Resolume
1. Copy shader files to your ISF directory
2. Shaders appear in generator/effect lists
3. Control parameters via MIDI/OSC

## 🎓 Learning Resources

### For Beginners
- Start with `docs/GETTING_STARTED.md`
- Try `shaders/JuliaSet.fs` first
- Experiment with parameter values

### For Advanced Users
- Read `docs/ISF_GUIDE.md` for technical details
- Study shader implementations
- Create custom variations

## 🛠️ Development

### Adding New Shaders

1. Follow ISF 2.0 specification
2. Include comprehensive metadata
3. Document parameters clearly
4. Test across multiple applications

### File Structure
```
shaders/           # ISF shader files (.fs)
docs/              # Documentation and guides
README.md          # This file
PROJECT_NOTES.md   # Development log
```

## 📋 Requirements

- ISF 2.0 compatible software
- GPU with OpenGL support
- No additional dependencies

## 🤝 Contributing

Contributions welcome! Feel free to:
- Submit new shader implementations
- Improve documentation
- Report issues
- Share interesting parameter combinations

## 📖 Additional Resources

- [ISF Specification](https://github.com/mrRay/ISF_Spec)
- [ISF Editor](https://isf.video/)
- [GLSL Reference](https://www.khronos.org/opengl/wiki/OpenGL_Shading_Language)

## 🔍 Meta Documentation

- [Understanding Agent Memory](./AGENT_MEMORY_EXPLAINED.md) - How context persists
- [Future Agent Test](./FUTURE_AGENT_TEST.md) - Memory demonstration

---

**Ready to explore fractals?** Check out the [Getting Started Guide](./docs/GETTING_STARTED.md)!