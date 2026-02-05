# Sketchbook Reveal Mixer (ISF)

A content-aware transition shader for Resolume Wire that creates an animated "sketchbook drawing reveal" effect.

## Features

- **Content-Aware**: Uses Sobel edge detection to reveal sketch lines from actual image contours
- **Luminance-Driven Hatching**: Denser line fills in darker areas for realistic sketch shading
- **Animated Growth**: Lines appear progressively with jittery, hand-drawn quality
- **Highly Customizable**: 11 parameters for fine control

## Installation

1. Copy `SketchbookReveal_v4.1.isf` to your Resolume ISF folder:
   - **Windows**: `Documents\Resolume Arena\ISF`
   - **macOS**: `~/Library/Application Support/Resolume Arena/ISF`

2. Open Resolume Wire (Menu > Show Wire)
3. Create new Mixer patch (File > New Patch > Mixer)
4. Add ISF node and select "Sketchbook Reveal v4.1"
5. Connect:
   - Texture Left → startImage
   - Texture Right → endImage
   - Float Amount → progress
   - ISF output → Texture Output

## Parameters

| Parameter | Range | Default | Description |
|-----------|-------|---------|-------------|
| **Progress** | 0.0–1.0 | 0.0 | Transition progress (link to crossfader Amount) |
| **Edge Reveal Strength** | 0.0–3.0 | 1.4 | How much edges reveal first (higher = more edge-following) |
| **Luminance Hatch Factor** | 0.0–3.0 | 0.8 | Hatching density in dark areas (higher = denser fills) |
| **Hatch Intensity** | 0.0–1.0 | 0.5 | Overall strength of line patterns |
| **Base Hatch Density** | 5.0–90.0 | 35.0 | Line frequency (higher = finer lines) |
| **Line Jitter** | 0.0–1.0 | 0.25 | Hand-drawn wobble amount |
| **Overlap Duration** | 0.1–1.0 | 0.45 | Temporal overlap between clips |
| **Overlap Softness** | 0.0–0.5 | 0.18 | Edge blend softness |
| **Anim Speed** | 0.0–1.0 | 0.09 | Animation speed (0 = static) |

## Recommended Settings

### For High-Contrast Sketches
- Edge Reveal Strength: 1.8–2.2
- Luminance Hatch Factor: 0.5–1.0
- Base Hatch Density: 40.0–50.0
- Line Jitter: 0.2–0.3

### For Softer, Painterly Look
- Edge Reveal Strength: 0.8–1.2
- Hatch Intensity: 0.3–0.4
- Base Hatch Density: 20.0–30.0
- Line Jitter: 0.35–0.5

### For Fast, Dynamic Transitions
- Overlap Duration: 0.25–0.35
- Anim Speed: 0.15–0.25

## Best Practices

- **Clip Format**: Works best with high-contrast monochrome sketches on transparent backgrounds (DXV3 codec recommended)
- **Background**: Use black or neutral background in composition to highlight white sketch lines
- **Performance**: Moderate GPU load; test on target hardware with multiple layers
- **Crossfader Setup**: Link Progress parameter to Crossfader Amount in Arena's composition settings

## Troubleshooting

**Lines not filling smoothly:**
- Increase Overlap Duration
- Adjust Overlap Softness for gradual blend

**Effect too subtle:**
- Increase Edge Reveal Strength and Hatch Intensity
- Lower Base Hatch Density for bolder lines

**Too jittery/chaotic:**
- Reduce Line Jitter and Anim Speed
- Increase Base Hatch Density for finer, calmer lines

## Technical Notes

- **Edge Detection**: Sobel operator on incoming image luminance
- **Hatching**: Multi-directional sine waves (45°, 135°, horizontal) modulated by noise
- **Growth**: Progress-driven smoothstep with per-line randomization for organic reveal
- **ISF Version**: 2 (compatible with Resolume 7.x+)

## References

Inspired by:
- [YouTube Sketch Reveal Demo](https://www.youtube.com/watch?v=aLtvbTQ41FE)
- Shadertoy sketch effects (iq, XdfGDn)
- Lygia GLSL library (edge detection)
- Vidvox ISF-Files (mixer templates)

## License

Creative Commons Attribution 4.0 International (CC BY 4.0)

## Version

v4.1 (2026-02-05) – ISF/Resolume compatible, content-aware edge reveal
