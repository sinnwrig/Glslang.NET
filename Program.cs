using System.Runtime.InteropServices;
using DirectX;

namespace Application;


public class Program
{        


    public static void Main(string[] args)
    {
        Exception? exT = Marshal.GetExceptionForHR(-2147467262);

        if (exT != null)
        {
            Console.WriteLine($"Exception: {exT.Message}");
        }


        DxcCompiler compiler = new DxcCompiler();

        string[] cArgs = new string[]
        {
            "-spirv",
            "vertshader.hlsl",         // Optional shader source file name for error reporting  
            "-E", "main",              // Entry point.
            "-T", "vs_6_0",            // Target.
            "-Zs",                     // Enable debug information (slim format)
        };

        Console.WriteLine("Compiling shader");
        DxcResult result = compiler.Compile(ShaderCode.VertexCodeHlsl, cArgs);

        Exception? ex = result.GetStatus();

        if (ex != null)
        {
            if (result.GetTextOutput(OutKind.Errors, out string errors, out _))
                Console.WriteLine($"Error:{errors}");

            Console.WriteLine("Compilation failed");
            return;
        }

        if (result.GetByteOutput(OutKind.Object, out byte[] bytes, out _))
            Console.WriteLine($"{bytes.Length} bytes of spirv code outputted");

        Console.WriteLine("Compilation success");
    }
}

