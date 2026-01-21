/*{
    "DESCRIPTION": "Interactive Julia Set fractal visualization with animated c parameter",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": ["Generator", "Fractal"],
    "INPUTS": [
        {
            "NAME": "zoom",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 50.0,
            "DEFAULT": 1.0
        },
        {
            "NAME": "centerX",
            "TYPE": "float",
            "MIN": -2.0,
            "MAX": 2.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "centerY",
            "TYPE": "float",
            "MIN": -2.0,
            "MAX": 2.0,
            "DEFAULT": 0.0
        },
        {
            "NAME": "cReal",
            "TYPE": "float",
            "MIN": -1.5,
            "MAX": 1.5,
            "DEFAULT": -0.7
        },
        {
            "NAME": "cImag",
            "TYPE": "float",
            "MIN": -1.5,
            "MAX": 1.5,
            "DEFAULT": 0.27015
        },
        {
            "NAME": "animateC",
            "TYPE": "bool",
            "DEFAULT": false
        },
        {
            "NAME": "animSpeed",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 2.0,
            "DEFAULT": 0.5
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
            "DEFAULT": 1.5
        },
        {
            "NAME": "colorScheme",
            "TYPE": "long",
            "VALUES": [0, 1, 2, 3, 4],
            "LABELS": ["Rainbow", "Fire", "Ocean", "Electric", "Grayscale"],
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
    
    // Smooth coloring
    float t = iter / maxIter;
    t = fract(t * colorIntensity + colorCycle);
    
    vec3 color;
    
    if (colorScheme == 0) {
        // Rainbow
        color = hsv2rgb(vec3(t, 0.85, 0.95));
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
    } else if (colorScheme == 3) {
        // Electric
        color = vec3(
            sin(t * 6.28318) * 0.5 + 0.5,
            sin(t * 6.28318 + 2.094) * 0.5 + 0.5,
            sin(t * 6.28318 + 4.188) * 0.5 + 0.5
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
    vec2 z;
    z.x = (uv.x - 0.5) * 4.0 * aspectRatio / zoom + centerX;
    z.y = (uv.y - 0.5) * 4.0 / zoom + centerY;
    
    // Julia set constant c
    vec2 c;
    if (animateC) {
        // Animate c along a circular path in the complex plane
        float t = TIME * animSpeed;
        c.x = 0.7885 * cos(t);
        c.y = 0.7885 * sin(t);
    } else {
        c.x = cReal;
        c.y = cImag;
    }
    
    // Julia set iteration
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
