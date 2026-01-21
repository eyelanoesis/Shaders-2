/*{
    "DESCRIPTION": "Applies a Gaussian blur effect to the input image",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Blur"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "blurAmount",
            "TYPE": "float",
            "DEFAULT": 5.0,
            "MIN": 0.0,
            "MAX": 20.0
        },
        {
            "NAME": "quality",
            "TYPE": "long",
            "DEFAULT": 1,
            "VALUES": [0, 1, 2],
            "LABELS": ["Low", "Medium", "High"]
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 pixelSize = 1.0 / RENDERSIZE.xy;
    
    vec4 color = vec4(0.0);
    float total = 0.0;
    
    // Determine sample count based on quality
    int samples;
    if (quality == 0) {
        samples = 4;
    } else if (quality == 1) {
        samples = 8;
    } else {
        samples = 16;
    }
    
    float blurSize = blurAmount * pixelSize.x;
    
    // Sample in a circular pattern
    for (int i = 0; i < 16; i++) {
        if (i >= samples) break;
        
        float angle = float(i) * 6.28318 / float(samples);
        
        for (int j = 1; j <= 3; j++) {
            float dist = float(j) / 3.0;
            vec2 offset = vec2(cos(angle), sin(angle)) * blurSize * dist;
            float weight = 1.0 - dist * 0.5;
            
            color += IMG_NORM_PIXEL(inputImage, uv + offset) * weight;
            total += weight;
        }
    }
    
    // Add center sample
    color += IMG_NORM_PIXEL(inputImage, uv);
    total += 1.0;
    
    gl_FragColor = color / total;
}
