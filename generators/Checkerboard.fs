/*{
    "DESCRIPTION": "Generates an animated checkerboard pattern",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Generator",
        "Pattern"
    ],
    "INPUTS": [
        {
            "NAME": "color1",
            "TYPE": "color",
            "DEFAULT": [1.0, 1.0, 1.0, 1.0]
        },
        {
            "NAME": "color2",
            "TYPE": "color",
            "DEFAULT": [0.0, 0.0, 0.0, 1.0]
        },
        {
            "NAME": "tiles",
            "TYPE": "float",
            "DEFAULT": 8.0,
            "MIN": 1.0,
            "MAX": 32.0
        },
        {
            "NAME": "offsetX",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": -1.0,
            "MAX": 1.0
        },
        {
            "NAME": "offsetY",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": -1.0,
            "MAX": 1.0
        },
        {
            "NAME": "rotation",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 360.0
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Center and apply rotation
    uv -= 0.5;
    float rad = rotation * 3.14159265359 / 180.0;
    float c = cos(rad);
    float s = sin(rad);
    uv = vec2(uv.x * c - uv.y * s, uv.x * s + uv.y * c);
    uv += 0.5;
    
    // Apply offset
    uv.x += offsetX;
    uv.y += offsetY;
    
    // Create checkerboard pattern
    vec2 pos = floor(uv * tiles);
    float checker = mod(pos.x + pos.y, 2.0);
    
    // Select color based on checker value
    vec4 finalColor = mix(color1, color2, checker);
    
    gl_FragColor = finalColor;
}
