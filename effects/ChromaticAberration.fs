/*{
    "DESCRIPTION": "Creates a chromatic aberration effect by splitting RGB channels",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Distortion",
        "Glitch"
    ],
    "INPUTS": [
        {
            "NAME": "inputImage",
            "TYPE": "image"
        },
        {
            "NAME": "amount",
            "TYPE": "float",
            "DEFAULT": 0.01,
            "MIN": 0.0,
            "MAX": 0.1
        },
        {
            "NAME": "angle",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 360.0
        },
        {
            "NAME": "radial",
            "TYPE": "bool",
            "DEFAULT": false
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    vec2 direction;
    
    if (radial) {
        // Radial aberration from center
        direction = normalize(uv - 0.5) * amount;
    } else {
        // Directional aberration
        float rad = angle * 3.14159265359 / 180.0;
        direction = vec2(cos(rad), sin(rad)) * amount;
    }
    
    // Sample each color channel with offset
    float r = IMG_NORM_PIXEL(inputImage, uv + direction).r;
    float g = IMG_NORM_PIXEL(inputImage, uv).g;
    float b = IMG_NORM_PIXEL(inputImage, uv - direction).b;
    float a = IMG_NORM_PIXEL(inputImage, uv).a;
    
    gl_FragColor = vec4(r, g, b, a);
}
