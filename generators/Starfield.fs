/*{
    "DESCRIPTION": "Generates an animated starfield effect",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": [
        "Generator",
        "Space"
    ],
    "INPUTS": [
        {
            "NAME": "starColor",
            "TYPE": "color",
            "DEFAULT": [1.0, 1.0, 1.0, 1.0]
        },
        {
            "NAME": "backgroundColor",
            "TYPE": "color",
            "DEFAULT": [0.0, 0.0, 0.05, 1.0]
        },
        {
            "NAME": "speed",
            "TYPE": "float",
            "DEFAULT": 1.0,
            "MIN": 0.0,
            "MAX": 5.0
        },
        {
            "NAME": "density",
            "TYPE": "float",
            "DEFAULT": 0.02,
            "MIN": 0.001,
            "MAX": 0.1
        },
        {
            "NAME": "layers",
            "TYPE": "float",
            "DEFAULT": 3.0,
            "MIN": 1.0,
            "MAX": 5.0
        }
    ]
}*/

// Hash function for pseudo-random numbers
float hash(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453);
}

// Star layer function
float starLayer(vec2 uv, float scale, float t) {
    vec2 gv = fract(uv * scale) - 0.5;
    vec2 id = floor(uv * scale);
    
    float star = 0.0;
    
    // Check neighboring cells
    for (int y = -1; y <= 1; y++) {
        for (int x = -1; x <= 1; x++) {
            vec2 offset = vec2(float(x), float(y));
            float n = hash(id + offset);
            
            // Only create a star if random value is below density threshold
            if (n < density) {
                vec2 starPos = gv - offset - (vec2(hash(id + offset + 0.1), hash(id + offset + 0.2)) - 0.5) * 0.8;
                float d = length(starPos);
                
                // Star brightness with twinkle
                float twinkle = sin(t * (n * 5.0 + 1.0)) * 0.5 + 0.5;
                float brightness = (0.02 / d) * twinkle * (n + 0.5);
                star += brightness;
            }
        }
    }
    
    return star;
}

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv -= 0.5;
    
    float time = TIME * speed;
    float stars = 0.0;
    
    // Multiple layers for depth
    for (float i = 0.0; i < 5.0; i++) {
        if (i >= layers) break;
        
        float scale = 20.0 + i * 10.0;
        float layerSpeed = 0.1 + i * 0.05;
        vec2 layerUV = uv + vec2(time * layerSpeed, 0.0);
        
        stars += starLayer(layerUV, scale, time) * (1.0 - i * 0.15);
    }
    
    // Combine with background
    vec3 finalColor = backgroundColor.rgb + starColor.rgb * stars;
    finalColor = clamp(finalColor, 0.0, 1.0);
    
    gl_FragColor = vec4(finalColor, 1.0);
}
