﻿using DXCompiler.NET;

namespace Application;

public class Program
{        
    public static string IncludeFile(string filename)
    {
        Console.WriteLine($"Including file: {filename}");

        return "#define INCLUDED_FUNC(x) x * 10 - sin(x)";
    }

    static DXShaderCompiler compInstance;
    static DXShaderCompiler compInstance2;


    public static void Main(string[] args)
    {
        compInstance = new DXShaderCompiler();
        compInstance2 = new DXShaderCompiler();

        Task task1 = Task.Factory.StartNew(() => CompileAThing(compInstance));
        Task task2 = Task.Factory.StartNew(() => CompileAThing(compInstance));

        Task.WaitAll(task1, task2);

        compInstance.Dispose();

        compInstance2 = new DXShaderCompiler();

        CompileAThing(compInstance2);

        compInstance2.Dispose();

        Console.WriteLine("Completed all tasks");
    }


    public static void CompileAThing(DXShaderCompiler compInstance)
    {
        CompilerOptions options = new CompilerOptions(new ShaderProfile(ShaderType.Pixel, 6, 0))
        {
            entryPoint = "pixel",
            generateAsSpirV = true,
        };

        Console.WriteLine(string.Join(' ', options.GetArgumentsArray()));

        CompilationResult result = compInstance.Compile(ShaderCode.HlslCode, options, IncludeFile);

        if (result.compilationErrors != null)
        {
            Console.WriteLine("Errors compiling shader:");
            Console.WriteLine(result.compilationErrors);
            return;
        }

        Console.WriteLine($"Success! {result.objectBytes.Length} bytes generated.");
    }
}