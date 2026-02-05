/*{
    "DESCRIPTION": "Burning Ship fractal - a variation of the Mandelbrot set using absolute values",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": ["Generator", "Fractal"],
    "INPUTS": [
        {
            "NAME": "zoom",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 100.0,
            "DEFAULT": 1.0
        },
        {
            "NAME": "centerX",
            "TYPE": "float",
            "MIN": -2.5,
            "MAX": 1.5,
            "DEFAULT": -0.4
        },
        {
            "NAME": "centerY",
            "TYPE": "float",
            "MIN": -2.0,
            "MAX": 1.0,
            "DEFAULT": -0.6
        },
        {
            "NAME": "maxIterations",
            "TYPE": "float",
            "MIN": 10.0,
            "MAX": 500.0,
            "DEFAULT": 100.0
        },
        {
            "NAME": "colorCycle",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "colorIntensity",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 5.0,
            "DEFAULT": 1.0
        },
        {
            "NAME": "invertY",
            "TYPE": "bool",
            "DEFAULT": true
        },
        {
            "NAME": "colorScheme",
            "TYPE": "long",
            "VALUES": [0, 1, 2, 3],
            "LABELS": ["Inferno", "Plasma", "Viridis", "Grayscale"],
            "DEFAULT": 0
        }
    ]
}*/

// Convert HSV to RGB color space
vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

// Inferno colormap approximation
vec3 inferno(float t) {
    const vec3 c0 = vec3(0.0002, 0.0016, 0.0139);
    const vec3 c1 = vec3(0.1050, 0.0364, 0.2325);
    const vec3 c2 = vec3(0.3614, 0.0907, 0.4047);
    const vec3 c3 = vec3(0.6175, 0.1654, 0.3971);
    const vec3 c4 = vec3(0.8441, 0.3258, 0.2452);
    const vec3 c5 = vec3(0.9869, 0.6453, 0.0395);
    const vec3 c6 = vec3(0.9882, 0.9982, 0.6449);
    
    if (t < 0.166) return mix(c0, c1, t / 0.166);
    if (t < 0.333) return mix(c1, c2, (t - 0.166) / 0.167);
    if (t < 0.5) return mix(c2, c3, (t - 0.333) / 0.167);
    if (t < 0.666) return mix(c3, c4, (t - 0.5) / 0.166);
    if (t < 0.833) return mix(c4, c5, (t - 0.666) / 0.167);
    return mix(c5, c6, (t - 0.833) / 0.167);
}

// Plasma colormap approximation
vec3 plasma(float t) {
    const vec3 c0 = vec3(0.0504, 0.0298, 0.5280);
    const vec3 c1 = vec3(0.4149, 0.0016, 0.6588);
    const vec3 c2 = vec3(0.6925, 0.1649, 0.5640);
    const vec3 c3 = vec3(0.8811, 0.3421, 0.3896);
    const vec3 c4 = vec3(0.9882, 0.5572, 0.1828);
    const vec3 c5 = vec3(0.9400, 0.9752, 0.1313);
    
    if (t < 0.2) return mix(c0, c1, t / 0.2);
    if (t < 0.4) return mix(c1, c2, (t - 0.2) / 0.2);
    if (t < 0.6) return mix(c2, c3, (t - 0.4) / 0.2);
    if (t < 0.8) return mix(c3, c4, (t - 0.6) / 0.2);
    return mix(c4, c5, (t - 0.8) / 0.2);
}

// Viridis colormap approximation
vec3 viridis(float t) {
    const vec3 c0 = vec3(0.2670, 0.0049, 0.3294);
    const vec3 c1 = vec3(0.2822, 0.1406, 0.4577);
    const vec3 c2 = vec3(0.2270, 0.3222, 0.5454);
    const vec3 c3 = vec3(0.1280, 0.5665, 0.5509);
    const vec3 c4 = vec3(0.3694, 0.7888, 0.3828);
    const vec3 c5 = vec3(0.9932, 0.9062, 0.1439);
    
    if (t < 0.2) return mix(c0, c1, t / 0.2);
    if (t < 0.4) return mix(c1, c2, (t - 0.2) / 0.2);
    if (t < 0.6) return mix(c2, c3, (t - 0.4) / 0.2);
    if (t < 0.8) return mix(c3, c4, (t - 0.6) / 0.2);
    return mix(c4, c5, (t - 0.8) / 0.2);
}

// Color palette based on iteration count
vec3 getColor(float iter, float maxIter) {
    if (iter >= maxIter) {
        return vec3(0.0); // Black for points inside the set
    }
    
    // Smooth coloring
    float t = iter / maxIter;
    t = fract(t * colorIntensity + colorCycle);
    
    vec3 color;
    
    if (colorScheme == 0) {
        color = inferno(t);
    } else if (colorScheme == 1) {
        color = plasma(t);
    } else if (colorScheme == 2) {
        color = viridis(t);
    } else {
        color = vec3(t);
    }
    
    return color;
}

void main() {
    // Get normalized coordinates
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Center and scale coordinates
    float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
    vec2 c;
    c.x = (uv.x - 0.5) * 4.0 * aspectRatio / zoom + centerX;
    c.y = (uv.y - 0.5) * 4.0 / zoom + centerY;
    
    // Optionally invert Y to show the "ship" right-side up
    if (invertY) {
        c.y = -c.y;
    }
    
    // Burning Ship iteration
    vec2 z = vec2(0.0);
    float iter = 0.0;
    float maxIter = maxIterations;
    
    for (float i = 0.0; i < 500.0; i++) {
        if (i >= maxIter) break;
        
        // z = (|Re(z)| + i|Im(z)|)^2 + c
        // Take absolute values before squaring
        z = abs(z);
        float xTemp = z.x * z.x - z.y * z.y + c.x;
        z.y = 2.0 * z.x * z.y + c.y;
        z.x = xTemp;
        
        // Check for escape
        if (dot(z, z) > 4.0) {
            break;
        }
        
        iter = i;
    }
    
    // Get color based on iteration count
    vec3 color = getColor(iter, maxIter);
    
    gl_FragColor = vec4(color, 1.0);
}
