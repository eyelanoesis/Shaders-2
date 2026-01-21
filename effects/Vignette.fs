/*{
    "DESCRIPTION": "Creates a vignette effect around the edges of the image",
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
            "NAME": "vignetteColor",
            "TYPE": "color",
            "DEFAULT": [0.0, 0.0, 0.0, 1.0]
        },
        {
            "NAME": "amount",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "NAME": "size",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "NAME": "softness",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.01,
            "MAX": 1.0
        },
        {
            "NAME": "roundness",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.0,
            "MAX": 2.0
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec4 color = IMG_NORM_PIXEL(inputImage, uv);
    
    // Calculate distance from center
    vec2 center = uv - 0.5;
    
    // Correct for aspect ratio
    float aspect = RENDERSIZE.x / RENDERSIZE.y;
    center.x *= mix(1.0, aspect, roundness * 0.5);
    
    // Calculate vignette
    float dist = length(center);
    float vignette = smoothstep(size - softness, size, dist);
    vignette = 1.0 - vignette;
    
    // Apply vignette
    vignette = mix(1.0, vignette, amount);
    
    // Mix with vignette color
    vec3 finalColor = mix(vignetteColor.rgb, color.rgb, vignette);
    
    gl_FragColor = vec4(finalColor, color.a);
}
