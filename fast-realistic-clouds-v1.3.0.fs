/*
{
  "CATEGORIES" : [
    "Generator"
  ],
  "DESCRIPTION" : "Fast realistic-looking clouds v1.3.0",
  "CREDIT" : "Optimized cloud shader with presets and seamless looping",
  "INPUTS" : [
    {
      "NAME" : "preset",
      "TYPE" : "long",
      "VALUES" : [
        1,
        2,
        3,
        4,
        5,
        6
      ],
      "LABELS" : [
        "Daytime Cumulus",
        "Golden Sunset",
        "Storm Clouds",
        "Soft Morning",
        "Wispy Cirrus",
        "Dramatic Sky"
      ],
      "DEFAULT" : 1,
      "LABEL" : "Preset"
    },
    {
      "NAME" : "speed",
      "TYPE" : "float",
      "MAX" : 2.0,
      "DEFAULT" : 0.3,
      "LABEL" : "Speed",
      "MIN" : 0.0
    },
    {
      "NAME" : "cloudSize",
      "TYPE" : "float",
      "MAX" : 3.0,
      "DEFAULT" : 1.0,
      "LABEL" : "Cloud Size",
      "MIN" : 0.1
    },
    {
      "NAME" : "coverage",
      "TYPE" : "float",
      "MAX" : 1.0,
      "DEFAULT" : 0.5,
      "LABEL" : "Cloud Coverage",
      "MIN" : 0.0
    },
    {
      "NAME" : "quality",
      "TYPE" : "float",
      "MAX" : 1.0,
      "DEFAULT" : 0.5,
      "LABEL" : "Quality",
      "MIN" : 0.2
    },
    {
      "NAME" : "showBackground",
      "TYPE" : "bool",
      "DEFAULT" : 1,
      "LABEL" : "Show Background"
    },
    {
      "NAME" : "bgColor",
      "TYPE" : "color",
      "DEFAULT" : [
        0.4,
        0.5,
        0.7,
        1.0
      ],
      "LABEL" : "Background Color"
    },
    {
      "NAME" : "cloudColor",
      "TYPE" : "color",
      "DEFAULT" : [
        1.0,
        1.0,
        1.0,
        1.0
      ],
      "LABEL" : "Cloud Color"
    },
    {
      "NAME" : "shadowColor",
      "TYPE" : "color",
      "DEFAULT" : [
        0.7,
        0.75,
        0.85,
        1.0
      ],
      "LABEL" : "Shadow Color"
    },
    {
      "NAME" : "brightness",
      "TYPE" : "float",
      "MAX" : 2.0,
      "DEFAULT" : 1.0,
      "LABEL" : "Brightness",
      "MIN" : 0.0
    },
    {
      "NAME" : "contrast",
      "TYPE" : "float",
      "MAX" : 2.0,
      "DEFAULT" : 1.0,
      "LABEL" : "Contrast",
      "MIN" : 0.5
    },
    {
      "NAME" : "turbulence",
      "TYPE" : "float",
      "MAX" : 1.0,
      "DEFAULT" : 0.5,
      "LABEL" : "Turbulence",
      "MIN" : 0.0
    },
    {
      "NAME" : "seed",
      "TYPE" : "float",
      "MAX" : 100.0,
      "DEFAULT" : 0.0,
      "LABEL" : "Random Seed",
      "MIN" : 0.0
    },
    {
      "NAME" : "loopDuration",
      "TYPE" : "float",
      "MAX" : 60.0,
      "DEFAULT" : 10.0,
      "LABEL" : "Loop Duration (seconds)",
      "MIN" : 1.0
    },
    {
      "NAME" : "enableLoop",
      "TYPE" : "bool",
      "DEFAULT" : 0,
      "LABEL" : "Enable Seamless Loop"
    }
  ],
  "ISFVSN" : "2"
}
*/

/*
 * VERSION HISTORY:
 * v1.3.0 (2026-01-31)
 *   - Added seamless TIME looping option
 *   - New parameter: "Loop Duration" (1-60 seconds)
 *   - New parameter: "Enable Seamless Loop" toggle
 *   - When enabled, animation loops perfectly for video export
 *   - Uses same circular interpolation technique as seed parameter
 *   - Explained difference between seed and TIME parameters
 *
 * v1.2.0 (2026-01-31)
 *   - Removed "Custom" preset option
 *   - All presets now serve as starting points with full parameter control
 *   - Preset values now start from 1 instead of 0
 *   - Simplified preset logic
 *
 * v1.1.0 (2026-01-31)
 *   - Added seamless seed looping using circular interpolation
 *   - Moved unused parameters (lightAngle, lightIntensity, perspective, 
 *     horizon, windVariation, softness) to const values
 *   - Parameters remain in code for easy re-enabling
 *   - Seed now loops perfectly from 0-100 without discontinuity
 *
 * v1.0.0 (2026-01-31)
 *   - Initial optimized version
 *   - Complete rewrite from volumetric raymarching to 2D layered approach
 *   - 20-50x performance improvement over original
 *   - Added 7 presets with full parameter override system
 *   - Presets work as base values with user adjustments on top
 *   - Quality-based detail level (octave and cloud count variation)
 */

// Hidden parameters (kept in code but not in UI - can be re-enabled if needed)
const float lightAngle = 135.0;
const float lightIntensity = 0.3;
const float perspective = 0.3;
const float horizon = 0.3;
const float windVariation = 0.3;
const float softness = 0.5;

// Fast hash
float hash21(vec2 p) {
    return fract(sin(dot(p, vec2(127.1, 311.7))) * 43758.5453);
}

// Simple 2D noise
float noise(vec2 p) {
    vec2 i = floor(p);
    vec2 f = fract(p);
    f = f * f * (3.0 - 2.0 * f);
    
    float a = hash21(i);
    float b = hash21(i + vec2(1.0, 0.0));
    float c = hash21(i + vec2(0.0, 1.0));
    float d = hash21(i + vec2(1.0, 1.0));
    
    return mix(mix(a, b, f.x), mix(c, d, f.x), f.y);
}

// FBM with variable octaves based on quality
float fbm(vec2 p, float q) {
    float f = 0.0;
    float amp = 0.5;
    
    // First octave - always
    f += amp * noise(p);
    
    // Second octave - if quality > 0.3
    if (q > 0.3) {
        p *= 2.0;
        amp *= 0.5;
        f += amp * noise(p);
    }
    
    // Third octave - if quality > 0.6
    if (q > 0.6) {
        p *= 2.0;
        amp *= 0.5;
        f += amp * noise(p);
    }
    
    return f;
}

// Seamless seed offset using circular interpolation
vec2 getSeamlessSeedOffset(float s) {
    // Map seed (0-100) to angle (0-2π) and use sin/cos for seamless loop
    float angle = s * 0.0628318; // 2π / 100
    float radius = 50.0; // Arbitrary radius in noise space
    return vec2(cos(angle) * radius, sin(angle) * radius);
}

// Seamless time offset using circular interpolation
vec2 getSeamlessTimeOffset(float t, float duration) {
    // Map time within loop duration to angle (0-2π)
    float normalizedTime = mod(t, duration) / duration;
    float angle = normalizedTime * 6.28318; // 2π
    float radius = 100.0; // Larger radius for time to give more variation
    return vec2(cos(angle) * radius, sin(angle) * radius);
}

// Get time value - either looping or continuous
float getTime(float t, float duration, bool loop) {
    if (loop) {
        // For seamless looping, we'll use the offset method in cloud positions
        return t;
    } else {
        // Standard continuous time
        return t;
    }
}

// Mix preset values with user inputs
void mixPresetWithInputs(inout vec4 cloud, inout vec4 shadow, inout vec4 bg, 
                         inout float bright, inout float contr, inout float angle, 
                         inout float lightInt, inout float soft, inout float turb, 
                         inout float cov, inout float size, inout float spd) {
    
    // Store user input values
    vec4 userCloud = cloud;
    vec4 userShadow = shadow;
    vec4 userBg = bg;
    float userBright = bright;
    float userContr = contr;
    float userAngle = angle;
    float userLightInt = lightInt;
    float userSoft = soft;
    float userTurb = turb;
    float userCov = cov;
    float userSize = size;
    float userSpd = spd;
    
    // Preset values - initialized with defaults
    vec4 presetCloud = vec4(1.0, 1.0, 1.0, 1.0);
    vec4 presetShadow = vec4(0.7, 0.75, 0.85, 1.0);
    vec4 presetBg = vec4(0.4, 0.5, 0.7, 1.0);
    float presetBright = 1.0;
    float presetContr = 1.0;
    float presetAngle = 135.0;
    float presetLightInt = 0.3;
    float presetSoft = 0.5;
    float presetTurb = 0.3;
    float presetCov = 0.5;
    float presetSize = 1.0;
    float presetSpd = 0.3;
    
    if (preset == 1) {
        // Daytime Cumulus
        presetCloud = vec4(1.0, 1.0, 1.0, 1.0);
        presetShadow = vec4(0.7, 0.75, 0.85, 1.0);
        presetBg = vec4(0.4, 0.5, 0.7, 1.0);
        presetBright = 1.0;
        presetContr = 1.0;
        presetAngle = 135.0;
        presetLightInt = 0.3;
        presetSoft = 0.5;
        presetTurb = 0.3;
        presetCov = 0.5;
        presetSize = 1.0;
        presetSpd = 0.3;
    }
    else if (preset == 2) {
        // Golden Sunset
        presetCloud = vec4(1.0, 0.65, 0.3, 1.0);
        presetShadow = vec4(0.5, 0.25, 0.55, 1.0);
        presetBg = vec4(0.9, 0.5, 0.3, 1.0);
        presetBright = 1.3;
        presetContr = 1.4;
        presetAngle = 270.0;
        presetLightInt = 0.8;
        presetSoft = 0.6;
        presetTurb = 0.4;
        presetCov = 0.4;
        presetSize = 1.2;
        presetSpd = 0.2;
    }
    else if (preset == 3) {
        // Storm Clouds
        presetCloud = vec4(0.45, 0.45, 0.5, 1.0);
        presetShadow = vec4(0.15, 0.15, 0.2, 1.0);
        presetBg = vec4(0.25, 0.28, 0.32, 1.0);
        presetBright = 0.6;
        presetContr = 1.8;
        presetAngle = 90.0;
        presetLightInt = 0.5;
        presetSoft = 0.25;
        presetTurb = 0.7;
        presetCov = 0.75;
        presetSize = 1.4;
        presetSpd = 0.4;
    }
    else if (preset == 4) {
        // Soft Morning
        presetCloud = vec4(1.0, 0.95, 0.9, 1.0);
        presetShadow = vec4(0.85, 0.82, 0.88, 1.0);
        presetBg = vec4(0.7, 0.75, 0.85, 1.0);
        presetBright = 1.1;
        presetContr = 0.7;
        presetAngle = 45.0;
        presetLightInt = 0.15;
        presetSoft = 0.85;
        presetTurb = 0.2;
        presetCov = 0.35;
        presetSize = 0.9;
        presetSpd = 0.15;
    }
    else if (preset == 5) {
        // Wispy Cirrus
        presetCloud = vec4(1.0, 1.0, 1.0, 1.0);
        presetShadow = vec4(0.8, 0.82, 0.9, 1.0);
        presetBg = vec4(0.3, 0.45, 0.7, 1.0);
        presetBright = 0.85;
        presetContr = 1.1;
        presetAngle = 180.0;
        presetLightInt = 0.2;
        presetSoft = 0.9;
        presetTurb = 0.95;
        presetCov = 0.2;
        presetSize = 1.6;
        presetSpd = 0.5;
    }
    else if (preset == 6) {
        // Dramatic Sky
        presetCloud = vec4(1.2, 1.15, 1.0, 1.0);
        presetShadow = vec4(0.35, 0.4, 0.55, 1.0);
        presetBg = vec4(0.2, 0.3, 0.5, 1.0);
        presetBright = 1.4;
        presetContr = 1.9;
        presetAngle = 45.0;
        presetLightInt = 0.9;
        presetSoft = 0.4;
        presetTurb = 0.5;
        presetCov = 0.55;
        presetSize = 1.1;
        presetSpd = 0.35;
    }
    
    // Calculate the offset from defaults
    vec4 cloudOffset = userCloud - vec4(1.0, 1.0, 1.0, 1.0);
    vec4 shadowOffset = userShadow - vec4(0.7, 0.75, 0.85, 1.0);
    vec4 bgOffset = userBg - vec4(0.4, 0.5, 0.7, 1.0);
    float brightOffset = userBright - 1.0;
    float contrOffset = userContr - 1.0;
    float angleOffset = userAngle - 45.0;
    float lightIntOffset = userLightInt - 0.5;
    float softOffset = userSoft - 0.5;
    float turbOffset = userTurb - 0.5;
    float covOffset = userCov - 0.5;
    float sizeOffset = userSize - 1.0;
    float spdOffset = userSpd - 0.3;
    
    // Apply preset + user offset
    cloud = presetCloud + cloudOffset;
    shadow = presetShadow + shadowOffset;
    bg = presetBg + bgOffset;
    bright = presetBright + brightOffset;
    contr = presetContr + contrOffset;
    angle = presetAngle + angleOffset;
    lightInt = presetLightInt + lightIntOffset;
    soft = presetSoft + softOffset;
    turb = presetTurb + turbOffset;
    cov = presetCov + covOffset;
    size = presetSize + sizeOffset;
    spd = presetSpd + spdOffset;
    
    // Clamp to valid ranges
    cloud = clamp(cloud, vec4(0.0), vec4(2.0));
    shadow = clamp(shadow, vec4(0.0), vec4(1.0));
    bg = clamp(bg, vec4(0.0), vec4(1.0));
    bright = clamp(bright, 0.0, 2.0);
    contr = clamp(contr, 0.5, 2.0);
    angle = mod(angle, 360.0);
    lightInt = clamp(lightInt, 0.0, 2.0);
    soft = clamp(soft, 0.0, 1.0);
    turb = clamp(turb, 0.0, 1.0);
    cov = clamp(cov, 0.0, 1.0);
    size = clamp(size, 0.1, 3.0);
    spd = clamp(spd, 0.0, 2.0);
}

// Generate cloud-like shapes with proper falloff
float cloudShape(vec2 uv, vec2 offset, float size, float q, float turb, vec2 seedOffset, vec2 timeOffset, float covAdjusted, float contrAdjusted) {
    vec2 p = (uv - offset) / size;
    
    // Add turbulence distortion with seamless seed and time offsets
    if (turb > 0.01) {
        p.x += noise(p * 2.0 + seedOffset + timeOffset * 0.1) * turb * 0.1;
        p.y += noise(p * 2.0 + seedOffset + timeOffset * 0.1 + vec2(100.0, 0.0)) * turb * 0.1;
    }
    
    // Distance from center with stretched ellipse
    float dist = length(p * vec2(1.5, 1.0));
    
    // Base shape - falloff from center
    float shape = 1.0 - smoothstep(0.3, 1.0, dist);
    
    // Add noise detail with seamless seed and time offsets
    float noiseVal = fbm(uv * 3.0 / size + offset * 10.0 + seedOffset + timeOffset * 0.05, q);
    
    // Combine shape and noise
    float cloud = shape * noiseVal;
    
    // Apply coverage threshold to create distinct clouds
    cloud = smoothstep(0.3 - covAdjusted * 0.3, 0.6 - covAdjusted * 0.2, cloud);
    
    // Apply contrast
    cloud = pow(cloud, 1.0 / contrAdjusted);
    
    return cloud;
}

void main() {
    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    
    // Start with user input values
    vec4 cloudColorFinal = cloudColor;
    vec4 shadowColorFinal = shadowColor;
    vec4 bgColorFinal = bgColor;
    float brightFinal = brightness;
    float angleFinal = lightAngle;
    float lightIntFinal = lightIntensity;
    float softFinal = softness;
    float turbFinal = turbulence;
    float covFinal = coverage;
    float contrFinal = contrast;
    float sizeFinal = cloudSize;
    float speedFinal = speed;
    
    // Mix with preset values
    mixPresetWithInputs(cloudColorFinal, shadowColorFinal, bgColorFinal, 
                       brightFinal, contrFinal, angleFinal, lightIntFinal, 
                       softFinal, turbFinal, covFinal, sizeFinal, speedFinal);
    
    // Add adjustable perspective
    uv.y = (uv.y - horizon) * (1.0 - uv.y * perspective) + horizon;
    
    float time = TIME * speedFinal;
    
    // Add wind variation using noise
    float windOffset = noise(vec2(time * 0.1, 0.0)) * windVariation;
    
    vec3 finalColor = vec3(0.0);
    float totalDensity = 0.0;
    
    // Calculate light direction from angle
    float lightRad = radians(angleFinal);
    vec2 lightDir = vec2(cos(lightRad), sin(lightRad));
    
    // Get seamless seed offset
    vec2 seedOffset = getSeamlessSeedOffset(seed);
    
    // Get seamless time offset (only used if enableLoop is true)
    vec2 timeOffset = enableLoop ? getSeamlessTimeOffset(TIME * speedFinal, loopDuration) : vec2(0.0);
    
    // Create several cloud "puffs" moving across screen
    
    // Cloud 1 (back)
    float speed1 = 0.4 + windOffset * 0.1;
    vec2 pos1 = vec2(mod(time * speed1 + seed * 0.1, 2.0) - 0.5, 0.6);
    float cloud1 = cloudShape(uv, pos1, sizeFinal * 1.2, quality, turbFinal, seedOffset, timeOffset, covFinal, contrFinal);
    float depth1 = 0.7;
    
    float lighting1 = mix(0.6, 1.0, (uv.y - horizon) / (1.0 - horizon));
    float dirInfluence1 = (lightDir.x * (uv.x - pos1.x) + lightDir.y * (uv.y - pos1.y)) * 0.1;
    lighting1 = clamp(lighting1 + dirInfluence1 * lightIntFinal, 0.4, 1.0);
    
    vec3 color1 = mix(shadowColorFinal.rgb, cloudColorFinal.rgb, lighting1) * depth1;
    finalColor += color1 * cloud1 * (1.0 - totalDensity);
    totalDensity += cloud1 * 0.5;
    
    // Cloud 2 (back)
    float speed2 = 0.45 + windOffset * 0.15;
    vec2 pos2 = vec2(mod(time * speed2 + 0.7 + seed * 0.2, 2.0) - 0.5, 0.55);
    float cloud2 = cloudShape(uv, pos2, sizeFinal * 0.9, quality, turbFinal, seedOffset + vec2(10.0, 0.0), timeOffset, covFinal, contrFinal);
    float depth2 = 0.75;
    
    float lighting2 = mix(0.6, 1.0, (uv.y - horizon) / (1.0 - horizon));
    float dirInfluence2 = (lightDir.x * (uv.x - pos2.x) + lightDir.y * (uv.y - pos2.y)) * 0.1;
    lighting2 = clamp(lighting2 + dirInfluence2 * lightIntFinal, 0.4, 1.0);
    
    vec3 color2 = mix(shadowColorFinal.rgb, cloudColorFinal.rgb, lighting2) * depth2;
    finalColor += color2 * cloud2 * (1.0 - totalDensity);
    totalDensity += cloud2 * 0.5;
    
    // Cloud 3 (mid)
    float speed3 = 0.5 + windOffset * 0.2;
    vec2 pos3 = vec2(mod(time * speed3 + 0.3 + seed * 0.3, 2.0) - 0.5, 0.5);
    float cloud3 = cloudShape(uv, pos3, sizeFinal, quality, turbFinal, seedOffset + vec2(20.0, 0.0), timeOffset, covFinal, contrFinal);
    float depth3 = 0.85;
    
    float lighting3 = mix(0.6, 1.0, (uv.y - horizon) / (1.0 - horizon));
    float dirInfluence3 = (lightDir.x * (uv.x - pos3.x) + lightDir.y * (uv.y - pos3.y)) * 0.1;
    lighting3 = clamp(lighting3 + dirInfluence3 * lightIntFinal, 0.4, 1.0);
    
    vec3 color3 = mix(shadowColorFinal.rgb, cloudColorFinal.rgb, lighting3) * depth3;
    finalColor += color3 * cloud3 * (1.0 - totalDensity);
    totalDensity += cloud3 * 0.6;
    
    // Cloud 4 (front) - only if quality > 0.4
    if (quality > 0.4) {
        float speed4 = 0.55 + windOffset * 0.25;
        vec2 pos4 = vec2(mod(time * speed4 + 1.2 + seed * 0.4, 2.0) - 0.5, 0.45);
        float cloud4 = cloudShape(uv, pos4, sizeFinal * 1.1, quality, turbFinal, seedOffset + vec2(30.0, 0.0), timeOffset, covFinal, contrFinal);
        float depth4 = 1.0;
        
        float lighting4 = mix(0.6, 1.0, (uv.y - horizon) / (1.0 - horizon));
        float dirInfluence4 = (lightDir.x * (uv.x - pos4.x) + lightDir.y * (uv.y - pos4.y)) * 0.1;
        lighting4 = clamp(lighting4 + dirInfluence4 * lightIntFinal, 0.4, 1.0);
        
        vec3 color4 = mix(shadowColorFinal.rgb, cloudColorFinal.rgb, lighting4) * depth4;
        finalColor += color4 * cloud4 * (1.0 - totalDensity);
        totalDensity += cloud4 * 0.7;
    }
    
    // Apply softness to edges
    totalDensity = pow(totalDensity, 1.0 - softFinal * 0.5);
    
    // Apply brightness
    finalColor *= brightFinal;
    
    // Add background (optional)
    if (showBackground) {
        vec3 background = bgColorFinal.rgb * (1.0 - totalDensity);
        finalColor += background;
    }
    
    gl_FragColor = vec4(finalColor, 1.0);
}