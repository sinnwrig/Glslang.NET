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
            stage = ShaderStage.Fragment,
            client = ClientType.Vulkan,
            clientVersion = TargetClientVersion.Vulkan_1_3,
            targetLanguage = TargetLanguage.SPV,
            targetLanguageVersion = TargetLanguageVersion.SPV_1_5,
            code = ShaderCode.HlslCode,
            sourceEntrypoint = "pixel",
            defaultVersion = 100,
            defaultProfile = ShaderProfile.None,
            forceDefaultVersionAndProfile = false,
            forwardCompatible = false,
            fileIncluder = IncludeFunction,
            messages = MessageType.Default,
            invertY = false,
        };

        Console.WriteLine("Creating shader");

        using Shader shader = new Shader(input);

        Console.WriteLine("Created shader");

        if (!shader.Preprocess())
        {
            Console.WriteLine("HLSL preprocessing failed");
            Console.WriteLine(shader.GetInfoLog());
            Console.WriteLine(shader.GetDebugLog());
            Console.WriteLine(ShaderCode.HlslCode);
            return;
        }


        Console.WriteLine("Preprocess info logs:");
        Console.WriteLine(shader.GetInfoLog());

        Console.WriteLine("Preprocess debug logs:");
        Console.WriteLine(shader.GetDebugLog());

        if (!shader.Parse())
        {
            Console.WriteLine("HLSL parsing failed");
            Console.WriteLine(shader.GetInfoLog());
            Console.WriteLine(shader.GetDebugLog());
            Console.WriteLine(shader.GetPreprocessedCode());
            return;
        }

        Console.WriteLine("Parse info logs:");
        Console.WriteLine(shader.GetInfoLog());

        Console.WriteLine("Parse debug logs:");
        Console.WriteLine(shader.GetDebugLog());

        using Program program = new Program();

        program.AddShader(shader);

        if (!program.Link(MessageType.SPVRules | MessageType.VulkanRules))
        {
            Console.WriteLine("HLSL linking failed");
            Console.WriteLine(program.GetInfoLog());
            Console.WriteLine(program.GetDebugLog());
            return;
        }

        Console.WriteLine("Program link info log:");
        Console.WriteLine(program.GetInfoLog());

        Console.WriteLine("Program link debug log:");
        Console.WriteLine(program.GetDebugLog());

        program.GenerateSPIRV(out uint[] words, input.stage);

        string messages = program.GetSPIRVMessages();

        if (!string.IsNullOrWhiteSpace(messages))
            Console.WriteLine(messages);

        Console.WriteLine($"Generated {words.Length} bytes of SPIR-V");

        Console.WriteLine($"Dissasembled SPIR-V:");
        Console.WriteLine(CompilationContext.DisassembleSPIRV(words));
    }
}