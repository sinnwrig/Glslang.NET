# Glslang.NET: cross-platform C# wrapper for Khronos Group's glslang HLSL/GLSL compiler.

A cross-platform .NET wrapper written in C# to enable generating SPIR-V from GLSL/HLSL using Khronos' glslang compiler.
# Usage

This project wraps functionality from glslang into managed classes, which can be used to compile shader code with various options in a similar fashion to the native glslang interface.<br>
The following is a short example showcasing how a shader can be compiled using this wrapper, along with how source file inclusion can be overridden from C#.

```cs
using Glslang.NET;

namespace Application;

public class Program
{        
    const string fragmentSource = @"
#include ""./IncludeFile.hlsl""

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

    static IncludeResult IncludeFunction(string headerName, string includerName, uint depth, bool isSystemFile)
    {
        Console.WriteLine($"Including a {(isSystemFile ? "system" : "local")} file, `{headerName}` from `{includerName}` at depth {depth}.");
        IncludeResult result;

        result.headerData = "// Generated include";
        result.headerName = headerName;
        
        return result;
    }


    // If this looks familiar, that's because it is. See glslang's C interface: https://github.com/KhronosGroup/glslang?tab=readme-ov-file#c-functional-interface-new
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
```

# Native Details

To support cross-platform compilation and to simplify the native build process, Glslang.NET uses a [fork of glslang](https://github.com/sinnwrig/glslang-zig) built with zig instead of CMake or GN/Ninja. As Zig's compiler supports cross-compilation out of the box, it allows glslang to build on any platform, for any platform. Additional functionality is also included in the repository to allow glslang to export the ability to disassemble SPIR-V bytecode into a human-readable format. 

## Building Native Libraries

To build native libraries, run build_libraries.py, specicying the target architecture with -A [x86_64, arm64, all] and your target platform with -P [windows, linux, macos, all].<br>If the command is being run for the first time, it will pull the glslang-zig source repository.

Native build requirements:
- Zig version 0.13.0 or higher.
- Python 3 (Required for build_libraries.py, upgate_glslang_sources.py, gen_extension_headers.py. Future releases will aim to reduce and potentially cut off the dependency on python)

Pre-built binaries are bundled in the NuGet package for the following operating systems:
- Windows x64
- Windows arm64
- OSX x64
- OSX arm64
- Linux x64
- Linux arm64
