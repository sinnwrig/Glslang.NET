using System.Diagnostics;
using DXCompiler.NET;

namespace Application;


public class Program
{        


    public static void Main(string[] args)
    {
        Process currentProcess = Process.GetCurrentProcess();

        using ShaderCompiler compiler = new ShaderCompiler();

        CompilerOptions options = new CompilerOptions(new ShaderProfile(ShaderType.Vertex, 5, 0))
        {
            entryPoint = "main",
            generateAsSpirV = true,
            debugInfo = DebugInfoType.Slim
        };

        Console.WriteLine("Compiling shader");

        using CompilationOutput output = compiler.Compile(ShaderCode.VertexCodeHlsl, options);

        if (output.GetStatus() != null)
        {
            output.GetTextOutput(OutKind.Errors, out string errors, out _);
            Console.WriteLine($"Error:{errors}");
        }

        output.GetByteOutput(OutKind.Object, out byte[] bytes, out _);

        Console.WriteLine("Compilation success");
    }
}

