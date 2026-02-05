/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "Burning Ship fractal with custom rendering",
    "CREDIT": "Original implementation for Shaders-2",
    "CATEGORIES": ["Fractal", "Generator"],
    "INPUTS": [
        {
            "NAME": "focusX",
            "TYPE": "float",
            "DEFAULT": -0.5,
            "MIN": -2.0,
            "MAX": 1.0
        },
        {
            "NAME": "focusY",
            "TYPE": "float",
            "DEFAULT": -0.6,
            "MIN": -1.5,
            "MAX": 0.5
        },
        {
            "NAME": "zoomLevel",
            "TYPE": "float",
            "DEFAULT": 0.8,
            "MIN": 0.01,
            "MAX": 50.0
        },
        {
            "NAME": "computeSteps",
            "TYPE": "float",
            "DEFAULT": 180,
            "MIN": 30,
            "MAX": 600
        },
        {
            "NAME": "paletteShift",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        }
    ]
}*/

// Custom color palette generator
vec3 hsvColorTransform(vec3 hsv) {
    vec4 factors = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 adjusted = abs(fract(hsv.xxx + factors.xyz) * 6.0 - factors.www);
    return hsv.z * mix(factors.xxx, clamp(adjusted - factors.xxx, 0.0, 1.0), hsv.y);
}

void main() {
    vec2 pixelCoord = isf_FragNormCoord;
    float aspectCorrection = RENDERSIZE.x / RENDERSIZE.y;
    vec2 normalizedPos = (pixelCoord - 0.5) * vec2(aspectCorrection, 1.0);
    vec2 worldPos = normalizedPos / zoomLevel + vec2(focusX, focusY);
    
    vec2 trajectory = vec2(0.0);
    int completedSteps;
    int stepLimit = int(computeSteps);
    float divergenceLimit = 16.0;
    
    for(completedSteps = 0; completedSteps < stepLimit; completedSteps++) {
        // Burning ship uses absolute values before squaring
        float absReal = abs(trajectory.x);
        float absImag = abs(trajectory.y);
        
        float newReal = absReal * absReal - absImag * absImag + worldPos.x;
        float newImag = 2.0 * absReal * absImag + worldPos.y;
        trajectory = vec2(newReal, newImag);
        
        if(dot(trajectory, trajectory) > divergenceLimit) break;
    }
    
    vec3 pixelColor;
    if(completedSteps < stepLimit) {
        float smoothValue = float(completedSteps) - log2(log2(dot(trajectory, trajectory)));
        float intensity = smoothValue / float(stepLimit);
        
        float hue = fract(intensity * 3.0 + paletteShift);
        float sat = 0.8 - intensity * 0.3;
        float val = pow(intensity, 0.7);
        
        pixelColor = hsvColorTransform(vec3(hue, sat, val));
    } else {
        pixelColor = vec3(0.02, 0.01, 0.03);
    }
    
    gl_FragColor = vec4(pixelColor, 1.0);
}
