using System.Runtime.InteropServices;

namespace Glslang;


public class GlslangProgram
{
    private bool _isInitialized = false;

    internal readonly IntPtr nativePointer;
    internal readonly GlslangShader shader;
    internal readonly ShaderCompiler compiler;


    
    internal GlslangProgram(ShaderCompiler? compiler, GlslangShader? shader, IntPtr? nativePointer)
    {
        this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        this.shader = shader ?? throw new ArgumentNullException(nameof(shader));
        this.nativePointer = nativePointer ?? throw new ArgumentNullException(nameof(nativePointer));
        _isInitialized = true;
    }


    private void ValidateInitialized()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GlslangShader has not been properly initialized with ShaderCompiler");
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramAddShader(IntPtr program, IntPtr shader);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern int GlslangProgramLink(IntPtr program, int messages);



    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramAddSourceText(IntPtr program, ShaderStage stage, string text, nint length);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSetSourceFile(IntPtr program, ShaderStage stage, string file);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern int GlslangProgramMapIo(IntPtr program);



    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSPIRVGenerate(IntPtr program, ShaderStage stage);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSPIRVGenerateWithOptions(IntPtr program, ShaderStage stage, ref SpirVOptions spv_options);



    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern nint GlslangProgramSPIRVGetSize(IntPtr program);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSPIRVGet(IntPtr program, [Out] uint[] output);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramSPIRVGetPtr(IntPtr program); // Returns a uint* array



    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramSPIRVGetMessages(IntPtr program); // Returns a char* string

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramGetInfoLog(IntPtr program); // Returns a char* string

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramGetInfoDebugLog(IntPtr program); // Returns a char* string
}