/*{
    "DESCRIPTION": "Interactive Mandelbrot Set fractal visualization with zoom, pan, and color controls",
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
            "DEFAULT": -0.5
        },
        {
            "NAME": "centerY",
            "TYPE": "float",
            "MIN": -1.5,
            "MAX": 1.5,
            "DEFAULT": 0.0
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
            "NAME": "colorScheme",
            "TYPE": "long",
            "VALUES": [0, 1, 2, 3],
            "LABELS": ["Rainbow", "Fire", "Ocean", "Grayscale"],
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

// Color palette based on iteration count
vec3 getColor(float iter, float maxIter) {
    if (iter >= maxIter) {
        return vec3(0.0); // Black for points inside the set
    }
    
    // Smooth coloring using continuous potential
    float smoothIter = iter + 1.0 - log(log(2.0)) / log(2.0);
    float t = smoothIter / maxIter;
    t = fract(t * colorIntensity + colorCycle);
    
    vec3 color;
    
    if (colorScheme == 0) {
        // Rainbow
        color = hsv2rgb(vec3(t, 0.8, 0.9));
    } else if (colorScheme == 1) {
        // Fire
        color = vec3(
            min(1.0, t * 3.0),
            max(0.0, min(1.0, t * 3.0 - 1.0)),
            max(0.0, min(1.0, t * 3.0 - 2.0))
        );
    } else if (colorScheme == 2) {
        // Ocean
        color = vec3(
            max(0.0, min(1.0, t * 3.0 - 2.0)),
            max(0.0, min(1.0, t * 2.0 - 0.5)),
            min(1.0, t * 2.0 + 0.3)
        );
    } else {
        // Grayscale
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
    
    // Mandelbrot iteration
    vec2 z = vec2(0.0);
    float iter = 0.0;
    float maxIter = maxIterations;
    
    for (float i = 0.0; i < 500.0; i++) {
        if (i >= maxIter) break;
        
        // z = z^2 + c
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
