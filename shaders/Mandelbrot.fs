/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "Mandelbrot fractal with dynamic coloring",
    "CREDIT": "Original implementation for Shaders-2",
    "CATEGORIES": ["Fractal", "Generator"],
    "INPUTS": [
        {
            "NAME": "viewCenterX",
            "TYPE": "float",
            "DEFAULT": -0.6,
            "MIN": -2.5,
            "MAX": 1.5
        },
        {
            "NAME": "viewCenterY",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": -1.5,
            "MAX": 1.5
        },
        {
            "NAME": "magnification",
            "TYPE": "float",
            "DEFAULT": 1.2,
            "MIN": 0.01,
            "MAX": 100.0
        },
        {
            "NAME": "maxSteps",
            "TYPE": "float",
            "DEFAULT": 150,
            "MIN": 20,
            "MAX": 500
        },
        {
            "NAME": "hueRotation",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 6.283
        },
        {
            "NAME": "saturationLevel",
            "TYPE": "float",
            "DEFAULT": 0.85,
            "MIN": 0.0,
            "MAX": 1.0
        }
    ]
}*/

// Custom color transformation function
vec3 hsvColorTransform(vec3 hsv) {
    vec4 factors = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 adjusted = abs(fract(hsv.xxx + factors.xyz) * 6.0 - factors.www);
    return hsv.z * mix(factors.xxx, clamp(adjusted - factors.xxx, 0.0, 1.0), hsv.y);
}

void main() {
    vec2 normalizedCoords = isf_FragNormCoord;
    vec2 aspectCorrected = (normalizedCoords - 0.5) * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0);
    vec2 complexPlane = aspectCorrected / magnification + vec2(viewCenterX, viewCenterY);
    
    vec2 accumulator = vec2(0.0);
    int stepCount;
    float escapeRadius = 4.0;
    int maxIterations = int(maxSteps);
    
    for(stepCount = 0; stepCount < maxIterations; stepCount++) {
        float accX = accumulator.x * accumulator.x - accumulator.y * accumulator.y + complexPlane.x;
        float accY = 2.0 * accumulator.x * accumulator.y + complexPlane.y;
        accumulator = vec2(accX, accY);
        
        if(dot(accumulator, accumulator) > escapeRadius) break;
    }
    
    vec3 outputColor;
    if(stepCount < maxIterations) {
        float smoothedStep = float(stepCount) - log2(log2(dot(accumulator, accumulator)) * 0.5);
        float normalizedValue = smoothedStep / float(maxIterations);
        
        float hueValue = mod(normalizedValue * 4.5 + hueRotation, 6.283) / 6.283;
        vec3 hsvColor = vec3(hueValue, saturationLevel, sqrt(normalizedValue));
        outputColor = hsvColorTransform(hsvColor);
    } else {
        outputColor = vec3(0.0, 0.0, 0.05);
    }
    
    gl_FragColor = vec4(outputColor, 1.0);
}
