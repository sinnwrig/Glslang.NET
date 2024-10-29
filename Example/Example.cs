﻿using Glslang.NET;

namespace Application;

public static unsafe class Example
{
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
        CompilationInput input = new CompilationInput()
        {
            language = SourceType.HLSL,
            stage = ShaderStage.Vertex,
            client = ClientType.Vulkan,
            clientVersion = TargetClientVersion.Vulkan_1_3,
            targetLanguage = TargetLanguage.SPV,
            targetLanguageVersion = TargetLanguageVersion.SPV_1_5,
            code = ShaderCode.HlslCode,
            sourceEntrypoint = "vertex",
            defaultVersion = 100,
            defaultProfile = ShaderProfile.None,
            forceDefaultVersionAndProfile = false,
            forwardCompatible = false,
            fileIncluder = IncludeFunction,
            messages = MessageType.Enhanced | MessageType.ReadHlsl | MessageType.HlslLegalization,
        };

        Shader shader = new Shader(input);

        shader.SetOptions(ShaderOptions.AutoMapBindings | ShaderOptions.AutoMapLocations | ShaderOptions.MapUnusedUniforms | ShaderOptions.UseHLSLIOMapper);

        if (!shader.Preprocess())
        {
            Console.WriteLine("HLSL preprocessing failed");
            Console.WriteLine(shader.GetInfoLog());
            Console.WriteLine(shader.GetDebugLog());
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


        input.stage = ShaderStage.Fragment;
        input.sourceEntrypoint = "pixel";
        Shader shader2 = new Shader(input);

        shader2.SetOptions(ShaderOptions.AutoMapBindings | ShaderOptions.AutoMapLocations | ShaderOptions.MapUnusedUniforms | ShaderOptions.UseHLSLIOMapper);

        if (!shader2.Preprocess())
        {
            Console.WriteLine("HLSL preprocessing failed");
            Console.WriteLine(shader2.GetInfoLog());
            Console.WriteLine(shader2.GetDebugLog());
            return;
        }

        if (!shader2.Parse())
        {
            Console.WriteLine("HLSL parsing failed");
            Console.WriteLine(shader2.GetInfoLog());
            Console.WriteLine(shader2.GetDebugLog());
            Console.WriteLine(shader2.GetPreprocessedCode());
            return;
        }


        using Program program = new Program();

        program.AddShader(shader);
        program.AddShader(shader2);

        if (!program.Link(MessageType.SpvRules | MessageType.VulkanRules | MessageType.ReadHlsl))
        {
            Console.WriteLine("HLSL linking failed");
            Console.WriteLine(program.GetInfoLog());
            Console.WriteLine(program.GetDebugLog());
            return;
        }

        program.GenerateSPIRV(out uint[] words, input.stage);

        string messages = program.GetSPIRVMessages();

        if (!string.IsNullOrWhiteSpace(messages))
            Console.WriteLine(messages);

        Console.WriteLine($"Generated {words.Length} bytes of SPIR-V");

        Console.WriteLine($"Dissasembled SPIR-V:");
        Console.WriteLine(CompilationContext.DisassembleSPIRV(words));
    }
}