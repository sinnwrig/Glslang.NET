using Glslang.NET;


internal static class MainProgram
{
    const string fragmentSource = @"
#include ""./SomeFile.hlsl""

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

#define DO_SOMETHING(x) x * 10 + 4 - 8 + sqrt(x) / abs(x)


float4 pixel(VertexOutput input) : SV_Target
{
    float value = DO_SOMETHING(input.Color.r);

    float value2 = DO_SOMETHING(value);

    float value3 = DO_SOMETHING(value2);

    input.Color *= 10;

    input.Color /= 43.55;

    input.Color.g = value2;
    input.Color.b = value;
    input.Color.a = value3;
    
    return input.Color;
}
";

    static IncludeResult IncludeFunction(string headerName, string includerName, uint depth, bool isSystemFile)
    {
        Console.WriteLine($"Including a {(isSystemFile ? "system" : "local")} file, `{headerName}` from `{includerName}` at depth {depth}.");
        IncludeResult result;

        result.headerData = "// Nothing to see here";
        result.headerName = headerName;
        
        return result;
    }

    static void Main()
    {
        using CompilationContext context = new CompilationContext();

        CompilationInput input = new CompilationInput() 
        {
            language = SourceType.HLSL,
            stage = ShaderStage.Fragment,
            client = ClientType.Vulkan,
            clientVersion = TargetClientVersion.Vulkan_1_2,
            targetLanguage = TargetLanguage.SPV,
            targetLanguageVersion = TargetLanguageVersion.SPV_1_5,
            code = fragmentSource,
            sourceEntrypoint = "pixel",
            defaultVersion = 100,
            defaultProfile = ShaderProfile.None,
            forceDefaultVersionAndProfile = false,
            forwardCompatible = false,
            messages = MessageType.Default,
            resourceLimits = ResourceLimits.DefaultResource,
            fileIncluder = IncludeFunction
        };

        Shader shader = context.CreateShader(input);

        if (!shader.Preprocess())	
        {
            Console.WriteLine("HLSL preprocessing failed");
            Console.WriteLine(shader.GetInfoLog());
            Console.WriteLine(shader.GetDebugLog());
            Console.WriteLine(fragmentSource);
            return;
        }

        if (!shader.Parse()) 
        {
            Console.WriteLine("HLSL parsing failed");
            Console.WriteLine(shader.GetInfoLog());
            Console.WriteLine(shader.GetDebugLog());
            Console.WriteLine(shader.GetPreprocessedCode());
            return;
        }

        ShaderProgram program = context.CreateProgram();

        program.AddShader(shader);

        if (!program.Link(MessageType.SPVRules | MessageType.VulkanRules)) 
        {
            Console.WriteLine("HLSL linking failed");
            Console.WriteLine(program.GetInfoLog());
            Console.WriteLine(program.GetDebugLog());
            return;
        }

        program.GenerateSPIRV(out byte[] words, input.stage);

        string messages = program.GetSPIRVMessages();

        if (!string.IsNullOrWhiteSpace(messages))
            Console.WriteLine(messages);

        Console.WriteLine($"Generated {words.Length} bytes of SPIR-V");

        Console.WriteLine($"Dissasembled SPIR-V:");
        Console.WriteLine(context.DisassembleSPIRV(words));
    }
}