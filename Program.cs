using System.Diagnostics;
using DXCompiler.NET;

namespace Application;


public class Program
{        


    public static void Main(string[] args)
    {
        Process currentProcess = Process.GetCurrentProcess();

        Console.WriteLine($"Private Memory Size: {currentProcess.PrivateMemorySize64 / 1024} KB");
        Console.WriteLine($"Virtual Memory Size: {currentProcess.VirtualMemorySize64 / 1024} KB");
        Console.WriteLine($"Working Set: {currentProcess.WorkingSet64 / 1024} KB");


        using ShaderCompiler compiler = new ShaderCompiler();

        CompilerOptions options = new CompilerOptions(new ShaderProfile(ShaderType.Vertex, 5, 0))
        {
            entryPoint = "main",
            generateAsSpirV = true,
            debugInfo = DebugInfoType.Slim
        };

        Console.WriteLine("Compiling shader multiple times");

        for (int i = 0; i < 100; i++)
        {
            using CompilationOutput output = compiler.Compile(ShaderCode.VertexCodeHlsl, options);

            if (output.GetStatus() != null)
            {
                output.GetTextOutput(OutKind.Errors, out string errors, out _);
                //Console.WriteLine($"Error:{errors}");
            }

            output.GetByteOutput(OutKind.Object, out byte[] bytes, out _);
        }


        GC.Collect();
        
        Console.WriteLine($"Private Memory Size: {currentProcess.PrivateMemorySize64 / 1024} KB");
        Console.WriteLine($"Virtual Memory Size: {currentProcess.VirtualMemorySize64 / 1024} KB");
        Console.WriteLine($"Working Set: {currentProcess.WorkingSet64 / 1024} KB");

        Console.WriteLine("Compilation success");
    }
}

