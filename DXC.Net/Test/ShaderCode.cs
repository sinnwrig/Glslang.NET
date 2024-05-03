public static class ShaderCode
{
    public const string HlslCode = """
struct VertexInput
{
    float2 Position : POSITION;
    float4 Color : COLOR0;
};

struct VertexOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};


VertexOutput vertex(VertexInput input)
{
    VertexOutput output;
    output.Position = float4(input.Position, 0, 1);
    output.Color = input.Color;
    return output;
}


float4 pixel(VertexOutput input) : SV_Target
{
    return input.Color;
}
""";
}