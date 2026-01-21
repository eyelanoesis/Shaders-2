/*{
    "DESCRIPTION": "Classic plasma effect generator with animated patterns",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Generator",
        "Retro"
    ],
    "INPUTS": [
        {
            "NAME": "speed",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.0,
            "MAX": 5.0
        },
        {
            "NAME": "scale",
            "TYPE": "float",
            "DEFAULT": 4.0,
            "MIN": 1.0,
            "MAX": 20.0
        },
        {
            "NAME": "complexity",
            "TYPE": "float",
            "DEFAULT": 3.0,
            "MIN": 1.0,
            "MAX": 10.0
        },
        {
            "NAME": "colorShift",
            "TYPE": "float",
            "DEFAULT": 0.0,
            "MIN": 0.0,
            "MAX": 1.0
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    float time = TIME * speed;
    
    // Scale UV coordinates
    vec2 pos = uv * scale;
    
    // Create plasma pattern
    float v = 0.0;
    v += sin(pos.x + time);
    v += sin((pos.y + time) / 2.0);
    v += sin((pos.x + pos.y + time) / 2.0);
    
    // Add complexity layers
    float cx = pos.x + 0.5 * sin(time / 5.0);
    float cy = pos.y + 0.5 * cos(time / 3.0);
    v += sin(sqrt(complexity * (cx * cx + cy * cy) + 1.0) + time);
    
    v = v / 2.0;
    
    // Create color from plasma value
    vec3 color;
    color.r = sin(v * 3.14159265359 + colorShift * 6.28318);
    color.g = sin(v * 3.14159265359 + 2.094 + colorShift * 6.28318);
    color.b = sin(v * 3.14159265359 + 4.188 + colorShift * 6.28318);
    
    // Normalize to 0-1 range
    color = color * 0.5 + 0.5;
    
    gl_FragColor = vec4(color, 1.0);
}
