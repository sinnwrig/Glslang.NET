using Glslang.NET;


internal static class MainProgram
{
    const string fragmentSource = @"
#include ""./SomeFile.hlsl""

struct Input
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

float4 pixel(Input input) : SV_Target
{    
    return input.Color;
}
";

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

        Program program = context.CreateProgram();

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