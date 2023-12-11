using System.Runtime.InteropServices;

namespace Glslang;


public class GlslangShader
{
    private bool _isInitialized = false;

    internal readonly IntPtr nativePointer;
    internal readonly ShaderInput input;
    internal readonly ShaderCompiler compiler;


    
    internal GlslangShader(ShaderCompiler? compiler, ShaderInput? input, IntPtr? nativePointer)
    {
        this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        this.input = input ?? throw new ArgumentNullException(nameof(input));
        this.nativePointer = nativePointer ?? throw new ArgumentNullException(nameof(nativePointer));
        _isInitialized = true;
    }


    private void ValidateInitialized()
    {
        if (!_isInitialized)
            throw new InvalidOperationException("GlslangShader has not been properly initialized with ShaderCompiler");
    }



    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangShaderSetPreamble(IntPtr shader, string s);

    public void SetPreamble(string s)
    {
        ValidateInitialized();
        GlslangShaderSetPreamble(nativePointer, s);
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangShaderShiftBinding(IntPtr shader, ResourceType res, uint shiftBase);

    public void ShiftBinding(ResourceType res, uint shiftBase)
    {
        ValidateInitialized();
        GlslangShaderShiftBinding(nativePointer, res, shiftBase);
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangShaderShiftBindingForSet(IntPtr shader, ResourceType res, uint shiftBase, uint set);

    public void ShiftBindingForSet(ResourceType res, uint shiftBase, uint set)
    {
        ValidateInitialized();
        GlslangShaderShiftBindingForSet(nativePointer, res, shiftBase, set);
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangShaderSetOptions(IntPtr shader, ShaderOptions options);

    public void SetOptions(ShaderOptions options)
    {
        ValidateInitialized();
        GlslangShaderSetOptions(nativePointer, options);
    }



    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangShaderSetGlslVersion(IntPtr shader, int version);

    public void SetGlslVersion(int version)
    {
        ValidateInitialized();
        GlslangShaderSetGlslVersion(nativePointer, version);
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern int GlslangShaderPreprocess(IntPtr shader, IntPtr input);

    public bool Preprocess(out string infoLog, out string debugLog)
    {
        bool success = GlslangShaderPreprocess(nativePointer, input.NativePointer) == 1;
        
        infoLog = AllocUtility.AutoString(GlslangShaderGetInfoLog(nativePointer));
        debugLog = AllocUtility.AutoString(GlslangShaderGetInfoDebugLog(nativePointer));

        return success;
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern int GlslangShaderParse(IntPtr shader, IntPtr input);

    public bool Parse(out string infoLog, out string debugLog)
    {
        bool success = GlslangShaderParse(nativePointer, input.NativePointer) == 1;
        
        infoLog = AllocUtility.AutoString(GlslangShaderGetInfoLog(nativePointer));
        debugLog = AllocUtility.AutoString(GlslangShaderGetInfoDebugLog(nativePointer));

        return success;
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangShaderGetPreprocessedCode(IntPtr shader); // Returns a char* string

    public string GetPreprocessedCode()
    {
        return AllocUtility.AutoString(GlslangShaderGetPreprocessedCode(nativePointer));
    }


    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangShaderGetInfoLog(IntPtr shader); // Returns a char* string

    [DllImport(ShaderCompiler.libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangShaderGetInfoDebugLog(IntPtr shader); // Returns a char* string
}