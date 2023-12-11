using System.Runtime.InteropServices;

namespace Glslang;


public class GlslangProgram
{
    private bool _isInitialized = false;

    internal readonly IntPtr nativePointer;
    internal readonly HashSet<GlslangShader> shaders;
    internal readonly ShaderCompiler compiler;


    
    internal GlslangProgram(ShaderCompiler? compiler, GlslangShader[] shaders, IntPtr? nativePointer)
    {
        this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        this.shaders = new HashSet<GlslangShader>();
        this.nativePointer = nativePointer ?? throw new ArgumentNullException(nameof(nativePointer));
        _isInitialized = true;

        for (int i = 0; i < shaders.Length; i++)
            AddShader(shaders[i]);
    }


    private void ValidateInitialized()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GlslangShader has not been properly initialized with ShaderCompiler");
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramAddShader(IntPtr program, IntPtr shader);
    public void AddShader(GlslangShader shader)
    {
        ValidateInitialized();

        if (shaders.Contains(shader))
            throw new ArgumentException("Duplicate shader provided to shader program");
        
        GlslangProgramAddShader(nativePointer, shader.nativePointer);
        shaders.Add(shader);
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern int GlslangProgramLink(IntPtr program, Messages messages);
    public bool Link(out string infoLog, out string debugLog, Messages messages = Messages.Default)
    {
        ValidateInitialized();

        bool success = GlslangProgramLink(nativePointer, messages) == 1;
        
        infoLog = AllocUtility.AutoString(GlslangProgramGetInfoLog(nativePointer));
        debugLog = AllocUtility.AutoString(GlslangProgramGetInfoDebugLog(nativePointer));

        return success;
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramAddSourceText(IntPtr program, ShaderStage stage, string text, nint length);
    public void AddSourceText(ShaderStage stage, string text) 
    {
        ValidateInitialized();
        GlslangProgramAddSourceText(nativePointer, stage, text, text.Length);
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSetSourceFile(IntPtr program, ShaderStage stage, string file);
    public void SetSourceFile(ShaderStage stage, string file) 
    {
        ValidateInitialized();
        GlslangProgramSetSourceFile(nativePointer, stage, file);
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern int GlslangProgramMapIo(IntPtr program);
    public bool MapIO() 
    {
        ValidateInitialized();
        return GlslangProgramMapIo(nativePointer) == 1;
    }



    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSPIRVGenerate(IntPtr program, ShaderStage stage);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSPIRVGenerateWithOptions(IntPtr program, ShaderStage stage, ref SpirVOptions spvOptions);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern nint GlslangProgramSPIRVGetSize(IntPtr program);

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangProgramSPIRVGet(IntPtr program, [Out] uint[] output);

// More overhead since the pointer would have to be copied to managed memory
/*
    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramSPIRVGetPtr(IntPtr program); // Returns a uint* array
*/
    public uint[] GenerateSpirV(ShaderStage stage, out string messages, SpirVOptions? options = null)
    {
        SpirVOptions nOptions = options ?? default;

        if (options != null)
            GlslangProgramSPIRVGenerateWithOptions(nativePointer, stage, ref nOptions);
        else
            GlslangProgramSPIRVGenerate(nativePointer, stage);

        nint size = GlslangProgramSPIRVGetSize(nativePointer);
        uint[] spirv = new uint[size];

        GlslangProgramSPIRVGet(nativePointer, spirv);

        messages = AllocUtility.AutoString(GlslangProgramSPIRVGetMessages(nativePointer));

        return spirv;
    }   




    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramSPIRVGetMessages(IntPtr program); // Returns a char* string

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramGetInfoLog(IntPtr program); // Returns a char* string

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangProgramGetInfoDebugLog(IntPtr program); // Returns a char* string
}