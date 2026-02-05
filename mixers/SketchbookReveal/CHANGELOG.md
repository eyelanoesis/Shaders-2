# Changelog - Sketchbook Reveal Mixer

All notable changes to this shader are documented here.

## [4.1] - 2026-02-05

### Fixed
- Replaced undefined `tex_2D_size` with correct ISF `RENDERSIZE` builtin
- Fixed Sobel edge detection pixel addressing for ISF compatibility
- All functions properly scoped and declared

### Added
- Content-aware edge detection using Sobel operator
- Luminance-driven hatching density modulation
- Multi-directional animated sketch patterns (3 layers at different angles)

### Changed
- Switched from pure noise-based reveal to edge-seeded growth
- Improved line "filling" behavior with progress-driven smoothstep
- Optimized hash function for better jitter distribution

## [4.0] - 2026-02-05

### Added (Failed Build)
- Initial attempt at Sobel edge detection
- Luminance-based hatching
- ❌ Used wrong builtin (`tex_2D_size`), did not compile

## [3.2–3.3] - 2026-02-05

### Attempted (Failed Builds)
- Basic hatching with jitter
- Experimental directional growth
- ❌ Missing function definitions, compilation errors

## [2.0–2.2] - 2026-02-05

### Added (Regressed)
- Line elongation parameter
- Content-aware features
- ❌ Visual results degraded; features non-functional

## [1.9] - 2026-02-05

### Changed
- Added temporal overlap offset control
- ⚠️ User feedback: not as good as v1.8

## [1.8] - 2026-02-05 ✅ **User Preferred Base**

### Added
- Multi-directional hatching (45°, 135°, horizontal)
- Selectable noise types (Perlin FBM, Voronoi, Simplex)
- Spread reveal with organic noise base
- Adjustable hatch density and intensity

### Fixed
- All compilation errors resolved
- Proper ISF builtin usage

## [1.0–1.7] - 2026-02-05

### Initial Development
- Basic noise + hatching iterations
- Burn-film-style spread reveal
- Simple crossfading logic
