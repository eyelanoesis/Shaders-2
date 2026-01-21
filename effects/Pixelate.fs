/*{
    "DESCRIPTION": "Applies a pixelation/mosaic effect to the input image",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Stylize",
        "Retro"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "pixelSize",
            "TYPE": "float",
            "DEFAULT": 10.0,
            "MIN": 1.0,
            "MAX": 100.0
        },
        {
            "NAME": "shape",
            "TYPE": "long",
            "DEFAULT": 0,
            "VALUES": [0, 1, 2],
            "LABELS": ["Square", "Circle", "Diamond"]
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Calculate pixel block size
    vec2 blockSize = vec2(pixelSize) / RENDERSIZE.xy;
    
    // Get the center of the current block
    vec2 blockCenter = (floor(uv / blockSize) + 0.5) * blockSize;
    
    // Sample the color at block center
    vec4 color = IMG_NORM_PIXEL(inputImage, blockCenter);
    
    // Calculate position within block
    vec2 blockPos = fract(uv / blockSize) - 0.5;
    
    // Apply shape masking
    float mask = 1.0;
    
    if (shape == 1) {
        // Circle shape
        mask = step(length(blockPos), 0.45);
    } else if (shape == 2) {
        // Diamond shape
        mask = step(abs(blockPos.x) + abs(blockPos.y), 0.45);
    }
    
    gl_FragColor = vec4(color.rgb * mask, color.a);
}
