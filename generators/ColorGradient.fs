/*{
    "DESCRIPTION": "Generates a smooth color gradient between two colors",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Generator",
        "Color"
    ],
    "INPUTS": [
        {
            "NAME": "color1",
            "TYPE": "color",
            "DEFAULT": [1.0, 0.0, 0.0, 1.0]
        },
        {
            "NAME": "color2",
            "TYPE": "color",
            "DEFAULT": [0.0, 0.0, 1.0, 1.0]
        },
        {
            "NAME": "angle",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 360.0
        },
        {
            "NAME": "offset",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Convert angle to radians
    float rad = angle * 3.14159265359 / 180.0;
    
    // Calculate gradient direction
    vec2 dir = vec2(cos(rad), sin(rad));
    
    // Calculate gradient position
    float t = dot(uv - 0.5, dir) + offset;
    t = clamp(t, 0.0, 1.0);
    
    // Mix colors
    vec4 finalColor = mix(color1, color2, t);
    
    gl_FragColor = finalColor;
}
