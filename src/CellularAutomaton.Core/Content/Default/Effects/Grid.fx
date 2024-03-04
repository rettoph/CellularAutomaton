#define VS_SHADERMODEL vs_5_0 
#define PS_SHADERMODEL ps_5_0 

static const int4 PackedByteMask = int4(0xff000000, 0x00ff0000, 0x0000ff00, 0x000000ff);

static const int IsTraceFlag = 1 << 0;
static const int IsOuterFlag = 1 << 1;

matrix WorldViewProjection;
int Width;

struct VertexShaderInput
{
    int Index : TEXCOORD0;
    uint ColorPacked : TEXCOORD1;
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
        ByteToFloat((input.ColorPacked & PackedByteMask[3]) >> 0),
        ByteToFloat((input.ColorPacked & PackedByteMask[2]) >> 8),
        ByteToFloat((input.ColorPacked & PackedByteMask[1]) >> 16),
        1);
    

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