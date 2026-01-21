/*{
    "DESCRIPTION": "Sierpinski Triangle fractal using the chaos game algorithm",
    "CREDIT": "ISF Shader Collection",
    "ISFVSN": "2.0",
    "CATEGORIES": ["Generator", "Fractal"],
    "INPUTS": [
        {
            "NAME": "zoom",
            "TYPE": "float",
            "MIN": 0.5,
            "MAX": 10.0,
            "DEFAULT": 1.0
        },
        {
            "NAME": "rotation",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 6.28318,
            "DEFAULT": 0.0
        },
        {
            "NAME": "iterations",
            "TYPE": "float",
            "MIN": 1.0,
            "MAX": 15.0,
            "DEFAULT": 8.0
        },
        {
            "NAME": "thickness",
            "TYPE": "float",
            "MIN": 0.001,
            "MAX": 0.1,
            "DEFAULT": 0.02
        },
        {
            "NAME": "animate",
            "TYPE": "bool",
            "DEFAULT": false
        },
        {
            "NAME": "animSpeed",
            "TYPE": "float",
            "MIN": 0.1,
            "MAX": 2.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "fillColor",
            "TYPE": "color",
            "DEFAULT": [0.2, 0.8, 0.4, 1.0]
        },
        {
            "NAME": "backgroundColor",
            "TYPE": "color",
            "DEFAULT": [0.05, 0.05, 0.1, 1.0]
        },
        {
            "NAME": "colorByDepth",
            "TYPE": "bool",
            "DEFAULT": true
        }
    ]
}*/

// Rotate a 2D point
vec2 rotate(vec2 p, float angle) {
    float c = cos(angle);
    float s = sin(angle);
    return vec2(p.x * c - p.y * s, p.x * s + p.y * c);
}

// Check if point is in the Sierpinski Triangle using recursive subdivision
float sierpinski(vec2 p, int maxIter) {
    // Start with equilateral triangle coordinates
    vec2 a = vec2(0.0, 0.866);    // Top vertex
    vec2 b = vec2(-0.5, -0.433);  // Bottom left
    vec2 c = vec2(0.5, -0.433);   // Bottom right
    
    float depth = 0.0;
    
    for (int i = 0; i < 15; i++) {
        if (i >= maxIter) break;
        
        // Find which sub-triangle the point is in
        vec2 mid_ab = (a + b) * 0.5;
        vec2 mid_bc = (b + c) * 0.5;
        vec2 mid_ca = (c + a) * 0.5;
        
        // Check which sub-triangle contains the point
        // Using barycentric-like test
        float d1 = (p.x - mid_bc.x) * (mid_ab.y - mid_bc.y) - (mid_ab.x - mid_bc.x) * (p.y - mid_bc.y);
        float d2 = (p.x - mid_ca.x) * (mid_bc.y - mid_ca.y) - (mid_bc.x - mid_ca.x) * (p.y - mid_ca.y);
        float d3 = (p.x - mid_ab.x) * (mid_ca.y - mid_ab.y) - (mid_ca.x - mid_ab.x) * (p.y - mid_ab.y);
        
        bool neg = (d1 < 0.0) || (d2 < 0.0) || (d3 < 0.0);
        bool pos = (d1 > 0.0) || (d2 > 0.0) || (d3 > 0.0);
        
        // Point is in the middle (removed) triangle
        if (!(neg && pos)) {
            return -1.0; // In the hole
        }
        
        // Determine which sub-triangle to recurse into
        // Top triangle
        if (p.y > mid_ab.y && p.y > mid_ca.y) {
            b = mid_ab;
            c = mid_ca;
        }
        // Bottom left triangle
        else if (p.x < mid_bc.x) {
            a = mid_ab;
            c = mid_bc;
        }
        // Bottom right triangle
        else {
            a = mid_ca;
            b = mid_bc;
        }
        
        depth = float(i + 1);
    }
    
    return depth;
}

// Alternative: Distance-based Sierpinski for smooth rendering
float sierpinskiSmooth(vec2 p) {
    float scale = 1.0;
    float dist = 0.0;
    
    for (int i = 0; i < 15; i++) {
        if (float(i) >= iterations) break;
        
        // Fold space to create sierpinski pattern
        p *= 2.0;
        scale *= 2.0;
        
        // Move to [0,1] range
        p.x = abs(p.x);
        
        if (p.y < 0.0) {
            p.y = -p.y;
        }
        
        // Fold along the line from (0.5, 0) to (0.25, 0.433)
        if (p.x + p.y * 0.577350269 > 0.5) {
            vec2 q = p - vec2(0.5, 0.0);
            q = q - 2.0 * vec2(-0.5, 0.866025404) * dot(q, vec2(-0.5, 0.866025404));
            p = q + vec2(0.5, 0.0);
        }
        
        // Shift down
        p.y -= 0.5;
    }
    
    return length(p) / scale;
}

// Convert HSV to RGB
vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 2.0/3.0, 1.0/3.0, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
    // Get normalized coordinates centered at origin
    vec2 uv = (gl_FragCoord.xy / RENDERSIZE.xy) * 2.0 - 1.0;
    float aspectRatio = RENDERSIZE.x / RENDERSIZE.y;
    uv.x *= aspectRatio;
    
    // Apply zoom
    uv /= zoom;
    
    // Apply rotation
    float rot = rotation;
    if (animate) {
        rot += TIME * animSpeed;
    }
    uv = rotate(uv, rot);
    
    // Calculate distance to Sierpinski Triangle
    float d = sierpinskiSmooth(uv);
    
    // Create smooth edge
    float edge = smoothstep(thickness, thickness * 0.5, d);
    
    // Calculate color
    vec3 color;
    if (colorByDepth) {
        // Color based on iteration depth
        float depth = 1.0 - d * 10.0;
        depth = clamp(depth, 0.0, 1.0);
        vec3 hsvColor = vec3(fract(depth * 0.5 + TIME * 0.1), 0.8, 0.9);
        color = mix(backgroundColor.rgb, hsv2rgb(hsvColor), edge);
    } else {
        color = mix(backgroundColor.rgb, fillColor.rgb, edge);
    }
    
    gl_FragColor = vec4(color, 1.0);
}
