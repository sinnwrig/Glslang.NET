using DXCompiler.NET;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

namespace Application;

public class Program
{        
    const string compPath = "./libtest.so";

    const CallingConvention cconv = CallingConvention.Cdecl;

    [DllImport(compPath, CallingConvention = cconv)]
    public static extern IntPtr machDxcInit();

    [DllImport(compPath, CallingConvention = cconv)]
    public static extern void machDxcDeinit(IntPtr compiler);

    [DllImport(compPath, CallingConvention = cconv)]
    public static extern IntPtr machDxcCompile(
        IntPtr compiler, 
        [MarshalAs(UnmanagedType.LPUTF8Str)] string lpString, 
        nuint codeLength, 
        [In] string[] stringArray, 
        nuint arrayLength,
        IntPtr includer);

    [DllImport(compPath, CallingConvention = cconv)]
    public static extern IntPtr machDxcCompileResultGetError(IntPtr result);

    [DllImport(compPath, CallingConvention = cconv)]
    public static extern IntPtr machDxcCompileErrorGetString(IntPtr err);


    public static IntPtr GetUtf8Ptr(string managedString)
    {
        int len = Encoding.UTF8.GetByteCount(managedString);
        byte[] buffer = new byte[len + 1];
        Encoding.UTF8.GetBytes(managedString, 0, managedString.Length, buffer, 0);
        IntPtr nativeUtf8 = Marshal.AllocHGlobal(buffer.Length);
        Marshal.Copy(buffer, 0, nativeUtf8, buffer.Length);
        return nativeUtf8;
    }


    public static IntPtr IncludeFile(IntPtr context, IntPtr filenameUtf16)
    {
        string? toManaged = Marshal.PtrToStringAuto(filenameUtf16);

        Console.WriteLine($"From C++: {toManaged}");

        return StringUtility.GetUtf8Ptr("// Random bullshit", out _);
    }


    public static void Main(string[] args)
    {
        CompilerOptions options = new CompilerOptions(new ShaderProfile(ShaderType.Pixel, 6, 0))
        {
            entryPoint = "pixel",
            generateAsSpirV = true,
        };

        IntPtr compiler = machDxcInit();

        string[] compilerArgs = options.GetArgumentsArray();

        Console.WriteLine(string.Join(" ", compilerArgs));

        IntPtr result = machDxcCompile(compiler, 
            ShaderCode.HlslCode, 
            (uint)ShaderCode.HlslCode.Length, 
            compilerArgs, 
            (uint)compilerArgs.Length,
            IntPtr.Zero);

        IntPtr err = machDxcCompileResultGetError(result);

        if (err != IntPtr.Zero)
        {
            string? errMsg = Marshal.PtrToStringAuto(machDxcCompileErrorGetString(err));
            Console.WriteLine(errMsg);
        }

        machDxcDeinit(compiler);
    }
}
