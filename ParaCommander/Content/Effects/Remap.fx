#if OPENGL
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
    #define VS_SHADERMODEL ps_4_0_level_9_3
    #define PS_SHADERMODEL ps_4_0_level_9_3
#endif

sampler2D input_texture = sampler_state
{
    Texture = <SpriteTexture>;
};

float4 input_color;
float input_remap;

struct VertexShaderOutput
{
    float4 Position : SV_Position;
    float4 Color : COLOR0;
    float2 UV : TEXCOORD0;
};

float4 rgba_to_hsva(float4 rgba)
{
    float cmax = max(rgba.r, max(rgba.g, rgba.b));
    float cmin = min(rgba.r, min(rgba.g, rgba.b));
    float delta = cmax - cmin;

    float hue = 0.0;

    if (delta > 0.0)
    {
        if (cmax == rgba.g)
            hue = (rgba.b - rgba.r) / delta + 2.0;
        else if (cmax == rgba.b)
            hue = (rgba.r - rgba.g) / delta + 4.0;
        else
            hue = (rgba.g - rgba.b) / delta + 6.0;

        hue = (hue % 6.0) * 60.0;
    }

    float saturation = cmax == 0.0 ? 0.0 : delta / cmax;
    float value = cmax;

    return float4(hue, saturation, value, rgba.a);
}

float4 hsva_to_rgba(float4 hsva)
{
    float c = hsva.z * hsva.y;
    float x = c * (1.0 - abs(hsva.x / 60.0 % 2.0 - 1.0));
    float m = hsva.z - c;

    float3 rgb =
        hsva.x < 60.0 ? float3(c, x, 0.0) :
        hsva.x < 120.0 ? float3(x, c, 0.0) :
        hsva.x < 180.0 ? float3(0.0, c, x) :
        hsva.x < 240.0 ? float3(0.0, x, c) :
        hsva.x < 300.0 ? float3(x, 0.0, c) :
        float3(c, 0.0, x);

    return float4(rgb + m, hsva.a);
}

float4 pixel_shader(VertexShaderOutput input) : COLOR0
{
    float4 rgba = tex2D(input_texture, input.UV);
    float4 hsva = rgba_to_hsva(rgba);

    if (abs(hsva.x - input_remap) < 0.1 && hsva.y > 0.1)
    {
        float4 input_hsva = rgba_to_hsva(input_color);

        hsva.x = input_hsva.x;
        hsva.y = input_hsva.y * hsva.y;
        hsva.z = hsva.z > 0.5 ? 1.0 - (1.0 - 2.0 * (hsva.z - 0.5)) * (1.0 - input_hsva.z) : 2.0 * hsva.z * input_hsva.z;
    }

    return hsva_to_rgba(hsva) * input.Color;
}

technique
{
    pass
    {
        PixelShader = compile PS_SHADERMODEL pixel_shader();
    }
}
