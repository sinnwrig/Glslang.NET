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




public const string VertexCodeHlsl = @"
struct appdata
{
    float2 position : POSITION;
    float4 color : COLOR;
};


struct v2f
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};


v2f main(appdata input)
{
    v2f output;

    output.position = float4(input.position.xy, 0, 1);
    output.color = input.color;

    return output;
}";

    public const string FragmentCodeHlsl = @"
struct v2f
{
    float4 position : SV_POSITION;
    float4 color : COLOR;
};

float4 main(v2f input) : SV_COLOR
{
    return input.color;
}";
}