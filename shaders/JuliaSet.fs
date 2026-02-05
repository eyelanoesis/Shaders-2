/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "Interactive Julia set with morphing parameters",
    "CREDIT": "Original implementation for Shaders-2",
    "CATEGORIES": ["Fractal", "Generator"],
    "INPUTS": [
        {
            "NAME": "paramReal",
            "TYPE": "float",
            "DEFAULT": -0.73,
            "MIN": -2.0,
            "MAX": 2.0
        },
        {
            "NAME": "paramImaginary",
            "TYPE": "float",
            "DEFAULT": 0.19,
            "MIN": -2.0,
            "MAX": 2.0
        },
        {
            "NAME": "viewScale",
            "TYPE": "float",
            "DEFAULT": 1.8,
            "MIN": 0.1,
            "MAX": 8.0
        },
        {
            "NAME": "bailoutThreshold",
            "TYPE": "float",
            "DEFAULT": 4.5,
            "MIN": 2.0,
            "MAX": 100.0
        },
        {
            "NAME": "iterationLimit",
            "TYPE": "float",
            "DEFAULT": 120,
            "MIN": 15,
            "MAX": 400
        },
        {
            "NAME": "colorCycle",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.1,
            "MAX": 5.0
        },
        {
            "NAME": "brightnessBoost",
            "TYPE": "float",
            "DEFAULT": 1.2,
            "MIN": 0.5,
            "MAX": 2.5
        }
    ]
}*/

// Original HSV to RGB conversion with custom blending
vec3 hsvColorTransform(vec3 hsv) {
    vec4 factors = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 adjusted = abs(fract(hsv.xxx + factors.xyz) * 6.0 - factors.www);
    return hsv.z * mix(factors.xxx, clamp(adjusted - factors.xxx, 0.0, 1.0), hsv.y);
}

void main() {
    vec2 screenCoord = isf_FragNormCoord;
    vec2 centeredCoord = (screenCoord - 0.5) * 2.0;
    vec2 aspectRatio = vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0);
    vec2 startingPoint = centeredCoord * aspectRatio * viewScale;
    
    vec2 juliaConstant = vec2(paramReal, paramImaginary);
    vec2 iteratingValue = startingPoint;
    
    int escapeIteration;
    int maxIterCount = int(iterationLimit);
    
    for(escapeIteration = 0; escapeIteration < maxIterCount; escapeIteration++) {
        float nextReal = iteratingValue.x * iteratingValue.x - iteratingValue.y * iteratingValue.y;
        float nextImag = 2.0 * iteratingValue.x * iteratingValue.y;
        iteratingValue = vec2(nextReal, nextImag) + juliaConstant;
        
        float magnitudeSquared = dot(iteratingValue, iteratingValue);
        if(magnitudeSquared > bailoutThreshold) break;
    }
    
    vec3 finalColor;
    if(escapeIteration < maxIterCount) {
        float magnitude = length(iteratingValue);
        float continuousEscape = float(escapeIteration) - log2(log(magnitude) / log(bailoutThreshold));
        float colorParam = continuousEscape / float(maxIterCount);
        
        float hueAngle = fract(colorParam * colorCycle + TIME * 0.05);
        float saturation = 0.7 + 0.3 * sin(colorParam * 12.0);
        float brightness = pow(colorParam, 0.6) * brightnessBoost;
        
        finalColor = hsvColorTransform(vec3(hueAngle, saturation, brightness));
    } else {
        float interiorShade = length(startingPoint) * 0.03;
        finalColor = vec3(interiorShade, interiorShade * 0.5, interiorShade * 1.5);
    }
    
    gl_FragColor = vec4(finalColor, 1.0);
}
