/*{
    "DESCRIPTION": "Kaleidoscopic fractal pattern generator with hypnotic visuals",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": ["Generator", "Fractal", "Kaleidoscope"],
    "INPUTS": [
        {
            "NAME": "segments",
            "TYPE": "float",
            "MIN": 3.0,
            "MAX": 16.0,
            "DEFAULT": 6.0
        },
        {
            "NAME": "zoom",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 5.0,
            "DEFAULT": 1.0
        },
        {
            "NAME": "iterations",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 20.0,
            "DEFAULT": 10.0
        },
        {
            "NAME": "rotation",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 6.28318,
            "DEFAULT": 0.0
        },
        {
            "NAME": "twist",
            "TYPE": "float",
            "MIN": -2.0,
            "MAX": 2.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "animate",
            "TYPE": "bool",
            "DEFAULT": true
        },
        {
            "NAME": "animSpeed",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 2.0,
            "DEFAULT": 0.3
        },
        {
            "NAME": "colorSpeed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 2.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "colorSaturation",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.8
        },
        {
            "NAME": "colorBrightness",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.9
        },
        {
            "NAME": "patternType",
            "TYPE": "long",
            "VALUES": [0, 1, 2, 3],
            "LABELS": ["Spiral", "Flower", "Crystal", "Hyperbolic"],
            "DEFAULT": 0
        }
    ]
}*/

#define PI 3.14159265359
#define TAU 6.28318530718

// Convert HSV to RGB
vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

// Kaleidoscope fold
vec2 kaleidoscope(vec2 p, float segs) {
    float angle = TAU / segs;
    float a = atan(p.y, p.x);
    a = mod(a, angle);
    a = abs(a - angle * 0.5);
    float r = length(p);
    return vec2(cos(a), sin(a)) * r;
}

// Rotate 2D point
vec2 rotate2D(vec2 p, float a) {
    float c = cos(a);
    float s = sin(a);
    return vec2(p.x * c - p.y * s, p.x * s + p.y * c);
}

// Pattern functions
float spiralPattern(vec2 p, float time) {
    float r = length(p);
    float a = atan(p.y, p.x);
    return sin(r * 10.0 - a * 3.0 + time * 2.0);
}

float flowerPattern(vec2 p, float time) {
    float r = length(p);
    float a = atan(p.y, p.x);
    float petals = 6.0;
    return sin(a * petals + time) * sin(r * 8.0 - time);
}

float crystalPattern(vec2 p, float time) {
    float d = 0.0;
    vec2 q = p;
    for (int i = 0; i < 5; i++) {
        q = abs(q) - 0.5;
        q = rotate2D(q, time * 0.2);
        d += sin(length(q) * 10.0);
    }
    return d * 0.2;
}

float hyperbolicPattern(vec2 p, float time) {
    float r = length(p);
    float a = atan(p.y, p.x);
    float d = 1.0 / (r + 0.1);
    return sin(d * 3.0 + a * 2.0 + time);
}

void main() {
    // Normalized coordinates centered at origin
    vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0;
    float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
    uv.x *= aspectRatio;
    
    // Apply zoom
    uv *= zoom;
    
    // Time for animation
    float time = animate ? TIME * animSpeed : 0.0;
    
    // Apply global rotation
    uv = rotate2D(uv, rotation + time * 0.5);
    
    // Apply twist based on distance from center
    float dist = length(uv);
    uv = rotate2D(uv, dist * twist);
    
    // Kaleidoscope fold
    uv = kaleidoscope(uv, segments);
    
    // Iterative fractal transformation
    float colorValue = 0.0;
    vec2 p = uv;
    
    int maxIter = int(iterations);
    for (int i = 0; i < 20; i++) {
        if (i >= maxIter) break;
        
        // Fold and transform
        p = abs(p) - 0.5;
        p = rotate2D(p, time * 0.1 + float(i) * 0.1);
        
        // Scale
        p *= 1.5;
        
        // Add to color value
        colorValue += 1.0 / (1.0 + length(p));
    }
    
    // Get pattern value based on type
    float pattern;
    if (patternType == 0) {
        pattern = spiralPattern(uv, time);
    } else if (patternType == 1) {
        pattern = flowerPattern(uv, time);
    } else if (patternType == 2) {
        pattern = crystalPattern(uv, time);
    } else {
        pattern = hyperbolicPattern(uv, time);
    }
    
    // Combine pattern with fractal iterations
    colorValue = colorValue * 0.1 + pattern * 0.5 + 0.5;
    
    // Create color from HSV
    float hue = fract(colorValue * 0.3 + time * colorSpeed);
    vec3 color = hsv2rgb(vec3(hue, colorSaturation, colorBrightness));
    
    // Add some glow at center
    color += vec3(1.0) * exp(-dist * 3.0) * 0.2;
    
    gl_FragColor = vec4(color, 1.0);
}
