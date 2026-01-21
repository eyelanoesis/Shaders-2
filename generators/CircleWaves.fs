/*{
    "DESCRIPTION": "Generates animated concentric circles",
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
            "DEFAULT": [0.0, 0.5, 1.0, 1.0]
        },
        {
            "NAME": "color2",
            "TYPE": "color",
            "DEFAULT": [0.0, 0.0, 0.2, 1.0]
        },
        {
            "NAME": "rings",
            "TYPE": "float",
            "DEFAULT": 10.0,
            "MIN": 1.0,
            "MAX": 50.0
        },
        {
            "NAME": "speed",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": -5.0,
            "MAX": 5.0
        },
        {
            "NAME": "centerX",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "NAME": "centerY",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        },
        {
            "NAME": "softness",
            "TYPE": "float",
            "DEFAULT": 0.5,
            "MIN": 0.0,
            "MAX": 1.0
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    vec2 center = vec2(centerX, centerY);
    
    // Calculate distance from center
    float dist = length(uv - center);
    
    // Create animated rings
    float pattern = sin((dist * rings * 6.28318) - (TIME * speed));
    
    // Apply softness
    pattern = mix(step(0.0, pattern), pattern * 0.5 + 0.5, softness);
    
    // Mix colors based on pattern
    vec4 finalColor = mix(color2, color1, pattern);
    
    gl_FragColor = finalColor;
}
