using DXCompiler.NET;
using System.Runtime.InteropServices;

namespace Application;


public class Program
{        
    [DllImport("machdxcompiler")]
    public static extern IntPtr machDxcInit();

    [DllImport("machdxcompiler")]
    public static extern void machDxcDeinit(IntPtr compiler);

    [DllImport("machdxcompiler")]
    public static extern IntPtr machDxcCompile(IntPtr compiler, [MarshalAs(UnmanagedType.LPStr)] string lpString, nuint codeLength, [In, Out] string[] stringArray, nuint arrayLength);

    [DllImport("machdxcompiler")]
    public static extern IntPtr machDxcCompileResultGetError(IntPtr result);

    [DllImport("machdxcompiler")]
    public static extern IntPtr machDxcCompileErrorGetString(IntPtr err);


    public static void Main(string[] args)
    {
        CompilerOptions options = new CompilerOptions(new ShaderProfile(ShaderType.Vertex, 6, 0))
        {
            entryPoint = "vertex",
            debugInfo = DebugInfoType.Normal,
            generateAsSpirV = true,
        };

        IntPtr compiler = machDxcInit();

        string[] compilerArgs = options.GetArgumentsArray();

        IntPtr result = machDxcCompile(compiler, ShaderCode.HlslCode, (uint)ShaderCode.HlslCode.Length, compilerArgs, (uint)compilerArgs.Length);

        IntPtr err = machDxcCompileResultGetError(result);

        if (err != IntPtr.Zero)
        {
            string errMsg = Marshal.PtrToStringAuto(machDxcCompileErrorGetString(err));
            Console.WriteLine(errMsg);
        }

        machDxcDeinit(compiler);

        /*
        using ShaderCompiler compiler = new ShaderCompiler();

        CompilerOptions options = new CompilerOptions(new ShaderProfile(ShaderType.Vertex, 6, 0))
        {
            entryPoint = "vertex",
            debugInfo = DebugInfoType.Normal,
            generateAsSpirV = true,
        };

        Console.WriteLine("Compiling vertex");

        using CompilationOutput vertexOutput = compiler.Compile(ShaderCode.HlslCode, options);

        if (vertexOutput.GetStatus() != null)
        {
            vertexOutput.GetTextOutput(OutKind.Errors, out string errors, out _);
            Console.Write(errors);
            return;
        }

        vertexOutput.GetByteOutput(OutKind.Object, out byte[] vertexBytes, out _);

        Console.WriteLine("Compiling pixel");

        options.entryPoint = "pixel";
        options.profile.Type = ShaderType.Pixel;

        using CompilationOutput pixelOutput = compiler.Compile(ShaderCode.HlslCode, options);

        if (pixelOutput.GetStatus() != null)
        {
            pixelOutput.GetTextOutput(OutKind.Errors, out string errors, out _);
            Console.Write(errors);
            return;
        }

        pixelOutput.GetByteOutput(OutKind.Object, out byte[] pixelBytes, out _);


        Console.WriteLine($"{vertexBytes.Length} bytes of vertex SPIR-V, {pixelBytes.Length} bytes of pixel SPIR-V");
        */
    }
}

