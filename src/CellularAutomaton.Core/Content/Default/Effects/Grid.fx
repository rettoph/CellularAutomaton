#define VS_SHADERMODEL vs_5_0 
#define PS_SHADERMODEL ps_5_0 

static const uint RMask = 0x000000ff;
static const uint GMask = 0x0000ff00;
static const uint BMask = 0x00ff0000;
static const uint AsleepMask = 0x80000000;

matrix WorldViewProjection;
int Width;
bool RenderAsleep;

struct VertexShaderInput
{
    int Index : TEXCOORD0;
    uint Data : TEXCOORD1;
};

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

float ByteToFloat(uint byte)
{
    return ((float)byte) / ((float)255);
}

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput) 0;

    float4 unprojectPosition = float4(input.Index % Width, input.Index / Width, 0, 1);
    output.Position = mul(unprojectPosition, WorldViewProjection);
    output.Color = float4(
            ByteToFloat((input.Data & RMask) >> 0),
            ByteToFloat((input.Data & GMask) >> 8),
            ByteToFloat((input.Data & BMask) >> 16),
            1);
    
    if (RenderAsleep == true)
    {
        output.Color = lerp(output.Color, ((input.Data & AsleepMask) == 0) ? float4(0, 1, 0, 1) : float4(1, 0, 0, 1), 0.5);
    }


    return output;
}

float4 MainPS(VertexShaderOutput input) : SV_Target0
{
    return input.Color;
}

technique BasicColorDrawing
{
    pass P0
    {
        VertexShader = compile VS_SHADERMODEL MainVS();
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};