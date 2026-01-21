/*{
    "DESCRIPTION": "Detects and highlights edges in the input image",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Stylize"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "edgeColor",
            "TYPE": "color",
            "DEFAULT": [1.0, 1.0, 1.0, 1.0]
        },
        {
            "NAME": "backgroundColor",
            "TYPE": "color",
            "DEFAULT": [0.0, 0.0, 0.0, 1.0]
        },
        {
            "NAME": "threshold",
            "TYPE": "float",
            "DEFAULT": 0.1,
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "NAME": "thickness",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.5,
            "MAX": 5.0
        },
        {
            "NAME": "invert",
            "TYPE": "bool",
            "DEFAULT": false
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 pixelSize = thickness / RENDERSIZE.xy;
    
    // Sobel operator kernels
    // Sample surrounding pixels
    float tl = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(-pixelSize.x, pixelSize.y)).rgb, vec3(0.299, 0.587, 0.114));
    float t  = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(0.0, pixelSize.y)).rgb, vec3(0.299, 0.587, 0.114));
    float tr = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(pixelSize.x, pixelSize.y)).rgb, vec3(0.299, 0.587, 0.114));
    float l  = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(-pixelSize.x, 0.0)).rgb, vec3(0.299, 0.587, 0.114));
    float r  = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(pixelSize.x, 0.0)).rgb, vec3(0.299, 0.587, 0.114));
    float bl = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(-pixelSize.x, -pixelSize.y)).rgb, vec3(0.299, 0.587, 0.114));
    float b  = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(0.0, -pixelSize.y)).rgb, vec3(0.299, 0.587, 0.114));
    float br = dot(IMG_NORM_PIXEL(inputImage, uv + vec2(pixelSize.x, -pixelSize.y)).rgb, vec3(0.299, 0.587, 0.114));
    
    // Calculate gradients
    float gx = -tl - 2.0*l - bl + tr + 2.0*r + br;
    float gy = -tl - 2.0*t - tr + bl + 2.0*b + br;
    
    // Calculate edge magnitude
    float edge = sqrt(gx*gx + gy*gy);
    
    // Apply threshold
    edge = step(threshold, edge);
    
    // Apply invert if needed
    if (invert) {
        edge = 1.0 - edge;
    }
    
    // Mix colors based on edge detection
    vec4 finalColor = mix(backgroundColor, edgeColor, edge);
    
    gl_FragColor = finalColor;
}
