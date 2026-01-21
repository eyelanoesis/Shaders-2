/*{
    "DESCRIPTION": "Generates a pulsing radial burst pattern",
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
            "DEFAULT": [1.0, 0.8, 0.0, 1.0]
        },
        {
            "NAME": "color2",
            "TYPE": "color",
            "DEFAULT": [1.0, 0.2, 0.0, 1.0]
        },
        {
            "NAME": "backgroundColor",
            "TYPE": "color",
            "DEFAULT": [0.0, 0.0, 0.0, 1.0]
        },
        {
            "NAME": "rays",
            "TYPE": "float",
            "DEFAULT": 12.0,
            "MIN": 2.0,
            "MAX": 36.0
        },
        {
            "NAME": "speed",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": -5.0,
            "MAX": 5.0
        },
        {
            "NAME": "pulseSpeed",
            "TYPE": "float",
            "DEFAULT": 2.0,
            "MIN": 0.0,
            "MAX": 10.0
        },
        {
            "NAME": "falloff",
            "TYPE": "float",
            "DEFAULT": 1.5,
            "MIN": 0.1,
            "MAX": 5.0
        }
    ]
}*/

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv = uv * 2.0 - 1.0;
    
    // Correct aspect ratio
    uv.x *= RENDERSIZE.x / RENDERSIZE.y;
    
    // Calculate polar coordinates
    float angle = atan(uv.y, uv.x);
    float dist = length(uv);
    
    // Create rotating rays
    float rotatedAngle = angle + TIME * speed;
    float rayPattern = sin(rotatedAngle * rays) * 0.5 + 0.5;
    
    // Create pulsing effect
    float pulse = sin(TIME * pulseSpeed) * 0.5 + 0.5;
    
    // Apply distance falloff
    float intensity = rayPattern * pow(max(0.0, 1.0 - dist * falloff), 2.0);
    intensity *= 0.7 + pulse * 0.3;
    
    // Color mixing
    vec3 rayColor = mix(color2.rgb, color1.rgb, pulse);
    vec3 finalColor = mix(backgroundColor.rgb, rayColor, intensity);
    
    gl_FragColor = vec4(finalColor, 1.0);
}
