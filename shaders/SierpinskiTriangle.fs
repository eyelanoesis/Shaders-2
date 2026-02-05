/*{
    "ISFVSN": "2.0",
    "DESCRIPTION": "Animated Sierpinski triangle with recursive patterns",
    "CREDIT": "Original implementation for Shaders-2",
    "CATEGORIES": ["Fractal", "Generator", "Geometric"],
    "INPUTS": [
        {
            "NAME": "recursionDepth",
            "TYPE": "float",
            "DEFAULT": 7,
            "MIN": 2,
            "MAX": 12
        },
        {
            "NAME": "rotationSpeed",
            "TYPE": "float",
            "DEFAULT": 0.2,
            "MIN": 0.0,
            "MAX": 2.0
        },
        {
            "NAME": "scaleAmount",
            "TYPE": "float",
            "DEFAULT": 1.5,
            "MIN": 0.5,
            "MAX": 3.0
        },
        {
            "NAME": "colorFrequency",
            "TYPE": "float",
            "DEFAULT": 1.8,
            "MIN": 0.5,
            "MAX": 5.0
        },
        {
            "NAME": "contrastLevel",
            "TYPE": "float",
            "DEFAULT": 0.85,
            "MIN": 0.3,
            "MAX": 1.5
        }
    ]
}*/

// Rotation matrix generator
mat2 rotationMatrix(float angle) {
    float cosA = cos(angle);
    float sinA = sin(angle);
    return mat2(cosA, -sinA, sinA, cosA);
}

// Triangle distance calculation
float triangleDistance(vec2 point, float size) {
    const float sqrtThree = 1.732;
    point = abs(point);
    return max(point.y - size, (point.x * sqrtThree + point.y) * 0.5 - size);
}

// Sierpinski pattern generator
float sierpinskiPattern(vec2 position, int depth) {
    float patternValue = 1.0;
    vec2 currentPos = position;
    float currentScale = 1.0;
    
    for(int level = 0; level < 12; level++) {
        if(level >= depth) break;
        
        float triDist = triangleDistance(currentPos, currentScale);
        patternValue = min(patternValue, abs(triDist));
        
        // Fold space for next iteration
        currentPos = abs(currentPos);
        if(currentPos.x < currentPos.y) {
            currentPos = currentPos.yx;
        }
        
        currentPos.x -= currentScale * 0.5;
        currentScale *= 0.5;
        currentPos *= 2.0;
    }
    
    return patternValue;
}

// Color transformation function
vec3 hsvColorTransform(vec3 hsv) {
    vec4 factors = vec4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
    vec3 adjusted = abs(fract(hsv.xxx + factors.xyz) * 6.0 - factors.www);
    return hsv.z * mix(factors.xxx, clamp(adjusted - factors.xxx, 0.0, 1.0), hsv.y);
}

void main() {
    vec2 screenCoord = (isf_FragNormCoord - 0.5) * 2.0;
    screenCoord.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    // Apply rotation animation
    float rotationAngle = TIME * rotationSpeed;
    mat2 rotation = rotationMatrix(rotationAngle);
    vec2 rotatedPos = rotation * screenCoord;
    
    // Scale and compute pattern
    vec2 scaledPos = rotatedPos * scaleAmount;
    int depthInt = int(recursionDepth);
    float pattern = sierpinskiPattern(scaledPos, depthInt);
    
    // Enhanced visualization
    float edgeDetect = smoothstep(0.02, 0.0, pattern);
    float gradient = 1.0 - smoothstep(0.0, 0.5, pattern);
    
    // Color mapping with time variation
    float hueValue = fract(gradient * colorFrequency + TIME * 0.15);
    float saturation = 0.7 + 0.3 * sin(pattern * 20.0);
    float brightness = pow(gradient, contrastLevel) * edgeDetect;
    
    vec3 outputColor = hsvColorTransform(vec3(hueValue, saturation, brightness));
    
    // Add edge highlighting
    outputColor += vec3(edgeDetect * 0.3);
    
    gl_FragColor = vec4(outputColor, 1.0);
}
