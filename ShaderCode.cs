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
#version 450

layout(location = 0) in vec2 Position;
layout(location = 1) in vec4 Color;

layout(location = 0) out vec4 fsin_Color;


void main()
{
    gl_Position = vec4(Position, 0, 1);
    fsin_Color = Color;
}";

    public const string FragmentCodeHlsl = @"
#version 450

layout(location = 0) in vec4 fsin_Color;
layout(location = 0) out vec4 fsout_Color;

void main()
{
    fsout_Color = fsin_Color;
}";
}