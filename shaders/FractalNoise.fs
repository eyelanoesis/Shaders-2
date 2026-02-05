/*{
    "DESCRIPTION": "Fractal Brownian Motion (fBM) noise pattern generator",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": ["Generator", "Fractal", "Noise"],
    "INPUTS": [
        {
            "NAME": "scale",
            "TYPE": "float",
            "MIN": 0.5,
            "MAX": 20.0,
            "DEFAULT": 4.0
        },
        {
            "NAME": "octaves",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 8.0,
            "DEFAULT": 5.0
        },
        {
            "NAME": "persistence",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 0.9,
            "DEFAULT": 0.5
        },
        {
            "NAME": "lacunarity",
            "TYPE": "float",
            "MIN": 1.5,
            "MAX": 3.0,
            "DEFAULT": 2.0
        },
        {
            "NAME": "animSpeed",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 2.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "offsetX",
            "TYPE": "float",
            "MIN": -10.0,
            "MAX": 10.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "offsetY",
            "TYPE": "float",
            "MIN": -10.0,
            "MAX": 10.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "colorize",
            "TYPE": "bool",
            "DEFAULT": true
        },
        {
            "NAME": "hueShift",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "saturation",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.7
        },
        {
            "NAME": "noiseType",
            "TYPE": "long",
            "VALUES": [0, 1, 2],
            "LABELS": ["Value Noise", "Simplex-like", "Turbulence"],
            "DEFAULT": 0
        }
    ]
}*/

// Pseudo-random function
float hash(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453123);
}

// 2D value noise
float valueNoise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    
    // Smooth interpolation
    vec2 u = f * f * (3.0 - 2.0 * f);
    
    // Four corners
    float a = hash(i);
    float b = hash(i + vec2(1.0, 0.0));
    float c = hash(i + vec2(0.0, 1.0));
    float d = hash(i + vec2(1.0, 1.0));
    
    // Bilinear interpolation
    return mix(mix(a, b, u.x), mix(c, d, u.x), u.y);
}

// Simplex-like noise (simplified)
vec2 hash2(vec2 p) {
    p = vec2(dot(p, vec2(127.1, 311.7)),
             dot(p, vec2(269.5, 183.3)));
    return -1.0 + 2.0 * fract(sin(p) * 43758.5453123);
}

float simplexLike(vec2 p) {
    const float K1 = 0.366025404; // (sqrt(3)-1)/2
    const float K2 = 0.211324865; // (3-sqrt(3))/6
    
    vec2 i = floor(p + (p.x + p.y) * K1);
    vec2 a = p - i + (i.x + i.y) * K2;
    float m = step(a.y, a.x);
    vec2 o = vec2(m, 1.0 - m);
    vec2 b = a - o + K2;
    vec2 c = a - 1.0 + 2.0 * K2;
    vec3 h = max(0.5 - vec3(dot(a, a), dot(b, b), dot(c, c)), 0.0);
    vec3 n = h * h * h * h * vec3(dot(a, hash2(i)),
                                    dot(b, hash2(i + o)),
                                    dot(c, hash2(i + 1.0)));
    return dot(n, vec3(70.0));
}

// Get noise value based on type
float getNoise(vec2 p, int type) {
    if (type == 0) {
        return valueNoise(p);
    } else if (type == 1) {
        return simplexLike(p) * 0.5 + 0.5;
    } else {
        // Turbulence (absolute value noise)
        return abs(simplexLike(p));
    }
}

// Fractal Brownian Motion
float fbm(vec2 p) {
    float value = 0.0;
    float amplitude = 0.5;
    float frequency = 1.0;
    float totalAmplitude = 0.0;
    
    int octaveCount = int(octaves);
    int noiseTypeInt = int(noiseType);
    
    for (int i = 0; i < 8; i++) {
        if (i >= octaveCount) break;
        
        value += amplitude * getNoise(p * frequency, noiseTypeInt);
        totalAmplitude += amplitude;
        amplitude *= persistence;
        frequency *= lacunarity;
    }
    
    return value / totalAmplitude;
}

// Convert HSV to RGB
vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
    // Get normalized coordinates
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Apply scale and offset
    vec2 p = uv * scale + vec2(offsetX, offsetY);
    
    // Add time-based animation
    p += vec2(TIME * animSpeed * 0.3, TIME * animSpeed * 0.2);
    
    // Calculate fractal noise
    float n = fbm(p);
    
    // Output color
    vec3 color;
    if (colorize) {
        // Create colorful output based on noise
        float hue = fract(n + hueShift);
        color = hsv2rgb(vec3(hue, saturation, n * 0.8 + 0.2));
    } else {
        // Grayscale output
        color = vec3(n);
    }
    
    gl_FragColor = vec4(color, 1.0);
}
