/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "Multi-octave fractal noise generator with layering",
    "CREDIT": "Original implementation for Shaders-2",
    "CATEGORIES": ["Fractal", "Noise", "Generator"],
    "INPUTS": [
        {
            "NAME": "layerCount",
            "TYPE": "float",
            "DEFAULT": 6,
            "MIN": 1,
            "MAX": 12
        },
        {
            "NAME": "baseFrequency",
            "TYPE": "float",
            "DEFAULT": 2.5,
            "MIN": 0.5,
            "MAX": 10.0
        },
        {
            "NAME": "amplitudeDecay",
            "TYPE": "float",
            "DEFAULT": 0.55,
            "MIN": 0.1,
            "MAX": 0.9
        },
        {
            "NAME": "frequencyGrowth",
            "TYPE": "float",
            "DEFAULT": 2.1,
            "MIN": 1.5,
            "MAX": 3.5
        },
        {
            "NAME": "timeInfluence",
            "TYPE": "float",
            "DEFAULT": 0.3,
            "MIN": 0.0,
            "MAX": 2.0
        },
        {
            "NAME": "colorVariation",
            "TYPE": "float",
            "DEFAULT": 0.7,
            "MIN": 0.0,
            "MAX": 2.0
        }
    ]
}*/

// Hash function for pseudo-random values
float hashValue(vec2 coord) {
    float dotResult = dot(coord, vec2(127.37, 311.91));
    return fract(sin(dotResult) * 43758.5453);
}

// Smooth noise interpolation
float smoothNoise(vec2 position) {
    vec2 cellCoord = floor(position);
    vec2 localCoord = fract(position);
    
    // Smooth interpolation curve
    vec2 curveFactor = localCoord * localCoord * (3.0 - 2.0 * localCoord);
    
    // Sample corners
    float cornerA = hashValue(cellCoord);
    float cornerB = hashValue(cellCoord + vec2(1.0, 0.0));
    float cornerC = hashValue(cellCoord + vec2(0.0, 1.0));
    float cornerD = hashValue(cellCoord + vec2(1.0, 1.0));
    
    // Bilinear blend
    float blendX1 = mix(cornerA, cornerB, curveFactor.x);
    float blendX2 = mix(cornerC, cornerD, curveFactor.x);
    return mix(blendX1, blendX2, curveFactor.y);
}

// Layered fractal noise accumulation
float fractalNoiseValue(vec2 coord, float octaves) {
    float accumulated = 0.0;
    float currentAmplitude = 1.0;
    float currentFrequency = baseFrequency;
    float amplitudeTotal = 0.0;
    
    int octaveCount = int(octaves);
    for(int layer = 0; layer < 12; layer++) {
        if(layer >= octaveCount) break;
        
        vec2 samplePoint = coord * currentFrequency;
        accumulated += smoothNoise(samplePoint) * currentAmplitude;
        amplitudeTotal += currentAmplitude;
        
        currentAmplitude *= amplitudeDecay;
        currentFrequency *= frequencyGrowth;
    }
    
    return accumulated / amplitudeTotal;
}

// Custom color transformation
vec3 hsvColorTransform(vec3 hsv) {
    vec4 factors = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 adjusted = abs(fract(hsv.xxx + factors.xyz) * 6.0 - factors.www);
    return hsv.z * mix(factors.xxx, clamp(adjusted - factors.xxx, 0.0, 1.0), hsv.y);
}

void main() {
    vec2 screenPos = isf_FragNormCoord * vec2(RENDERSIZE.x / RENDERSIZE.y, 1.0);
    vec2 animatedPos = screenPos + vec2(TIME * timeInfluence * 0.1, TIME * timeInfluence * 0.07);
    
    float primaryNoise = fractalNoiseValue(animatedPos, layerCount);
    float secondaryNoise = fractalNoiseValue(animatedPos * 1.7 + vec2(100.0), layerCount * 0.7);
    
    float combinedNoise = primaryNoise * 0.7 + secondaryNoise * 0.3;
    
    // Generate color variation
    float hueBase = combinedNoise * colorVariation;
    float saturationValue = 0.6 + 0.4 * secondaryNoise;
    float brightnessValue = 0.4 + 0.6 * primaryNoise;
    
    vec3 finalColor = hsvColorTransform(vec3(hueBase, saturationValue, brightnessValue));
    
    gl_FragColor = vec4(finalColor, 1.0);
}
