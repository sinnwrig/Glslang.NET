public static class ShaderCode
{
    public const string VertexCodeGlsl = @"
#version 450

layout(location = 0) in vec2 Position;
layout(location = 1) in vec4 Color;

layout(location = 0) out vec4 fsin_Color;


void main()
{
    gl_Position = vec4(Position, 0, 1);
    fsin_Color = Color;
}";

    public const string FragmentCodeGlsl = @"
#version 450

layout(location = 0) in vec4 fsin_Color;
layout(location = 0) out vec4 fsout_Color;

void main()
{
    fsout_Color = fsin_Color;
}";




public const string VertexCodeHlsl = """

#include "somefile.hlsl"

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

VertexOutput main(VertexInput input)
{
    VertexOutput output;
    output.Position = float4(input.Position, 0, 1);
    output.Color = input.Color;
    return output;
    vec3 googoo;
}
""";

    public const string FragmentCodeHlsl = """

#include "somefile.hlsl"

struct VertexOutput
{
    float4 Color : COLOR0;
};

float4 main(VertexOutput input) : SV_Target
{
    return input.Color;
}
""";
}