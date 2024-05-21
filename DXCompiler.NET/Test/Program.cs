using DXCompiler.NET;
using System.Runtime.InteropServices;
using System.Text;

namespace Application;

public class Program
{        
    public static IntPtr GetUtf8Ptr(string managedString)
    {
        int len = Encoding.UTF8.GetByteCount(managedString);
        byte[] buffer = new byte[len + 1];
        Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
        IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
        return nativeUtf8;
    }


    public static string IncludeFile(string filename)
    {
        Console.WriteLine(filename);

        return "Random bullshit";
    }


    public static void Main(string[] args)
    {
        CompilerOptions options = new CompilerOptions(new ShaderProfile(ShaderType.Pixel, 6, 0))
        {
            entryPoint = "pixel",
            generateAsSpirV = true,
        };

        using DXShaderCompiler compiler = new DXShaderCompiler();

        CompilationResult result = compiler.Compile(ShaderCode.HlslCode, options, IncludeFile);

        if (result.compilationErrors != null)
        {
            Console.WriteLine("Errors compiling shader:");
            Console.WriteLine(result.compilationErrors);
            return;
        }

        Console.WriteLine($"Success! {result.objectBytes.Length} bytes generated.");
    }
}
