/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "Dynamic kaleidoscope with fractal reflections",
    "CREDIT": "Original implementation for Shaders-2",
    "CATEGORIES": ["Fractal", "Generator", "Kaleidoscope"],
    "INPUTS": [
        {
            "NAME": "segmentCount",
            "TYPE": "float",
            "DEFAULT": 8,
            "MIN": 3,
            "MAX": 24
        },
        {
            "NAME": "reflectionDepth",
            "TYPE": "float",
            "DEFAULT": 4,
            "MIN": 1,
            "MAX": 8
        },
        {
            "NAME": "spiralTwist",
            "TYPE": "float",
            "DEFAULT": 0.4,
            "MIN": 0.0,
            "MAX": 2.0
        },
        {
            "NAME": "patternScale",
            "TYPE": "float",
            "DEFAULT": 1.2,
            "MIN": 0.3,
            "MAX": 4.0
        },
        {
            "NAME": "animationRate",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 3.0
        }
    ]
}*/

// Angle folding for kaleidoscope effect
vec2 kaleidoscopeFold(vec2 point, float segments) {
    float angleStep = 6.28318 / segments;
    float currentAngle = atan(point.y, point.x);
    float foldedAngle = mod(abs(currentAngle), angleStep);
    float distance = length(point);
    return vec2(cos(foldedAngle), sin(foldedAngle)) * distance;
}

// Spiral transformation
vec2 spiralTransform(vec2 point, float amount) {
    float dist = length(point);
    float angle = atan(point.y, point.x) + dist * amount;
    return vec2(cos(angle), sin(angle)) * dist;
}

// Pattern generation function
float patternGenerator(vec2 coord) {
    vec2 gridPos = fract(coord) - 0.5;
    float dist = length(gridPos);
    float pattern = sin(dist * 15.0 - TIME * 2.0) * 0.5 + 0.5;
    return pattern * smoothstep(0.5, 0.0, dist);
}

// Color transformation
vec3 hsvColorTransform(vec3 hsv) {
    vec4 factors = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 adjusted = abs(fract(hsv.xxx + factors.xyz) * 6.0 - factors.www);
    return hsv.z * mix(factors.xxx, clamp(adjusted - factors.xxx, 0.0, 1.0), hsv.y);
}

void main() {
    vec2 centerCoord = (isf_FragNormCoord - 0.5) * 2.0;
    centerCoord.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    // Apply spiral and kaleidoscope transformations
    vec2 spiraled = spiralTransform(centerCoord, spiralTwist);
    vec2 transformed = spiraled;
    
    // Multiple reflection iterations
    int iterations = int(reflectionDepth);
    float accumulated = 0.0;
    float scaleFactor = patternScale;
    
    for(int iter = 0; iter < 8; iter++) {
        if(iter >= iterations) break;
        
        transformed = kaleidoscopeFold(transformed, segmentCount);
        transformed *= 1.5;
        
        float layerPattern = patternGenerator(transformed * scaleFactor + TIME * animationRate * 0.1);
        accumulated += layerPattern / float(iter + 1);
        
        scaleFactor *= 1.3;
    }
    
    accumulated /= float(iterations);
    
    // Color generation
    float radiusFromCenter = length(centerCoord);
    float hueAngle = fract(accumulated * 2.0 + radiusFromCenter * 0.5 + TIME * 0.1);
    float saturation = 0.8 - accumulated * 0.2;
    float brightness = accumulated;
    
    vec3 finalColor = hsvColorTransform(vec3(hueAngle, saturation, brightness));
    
    gl_FragColor = vec4(finalColor, 1.0);
}
