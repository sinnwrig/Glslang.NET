using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Glslang.NET;


public class Shader
{   
    public readonly CompilationInput input;

    private readonly IntPtr compilerInputPtr;
    internal readonly IntPtr shaderPtr;



    internal Shader(CompilationInput input)
    {
        compilerInputPtr = CompilationInputNative.GetPtrForCompilationInput(input);
        shaderPtr = GlslangNative.glslang_shader_create(compilerInputPtr);
    }


    internal void Release()
    {
        GlslangNative.glslang_shader_delete(shaderPtr);
        CompilationInputNative.ReleasePtrForCompilationInput(compilerInputPtr);
    }



    public void SetPreamble(string preamble)
    {
        IntPtr preamblePtr = NativeStringUtility.AllocUTF8Ptr(preamble, out _, true);
        GlslangNative.glslang_shader_set_preamble(shaderPtr, preamblePtr);
        Marshal.FreeHGlobal(preamblePtr);
    }


    public void ShiftBinding(ResourceType resourceType, uint shiftBase)
    {
        GlslangNative.glslang_shader_shift_binding(shaderPtr, resourceType, shiftBase);
    }


    public void ShiftBindingForSet(ResourceType resourceType, uint shiftBase, uint set)
    {
        GlslangNative.glslang_shader_shift_binding_for_set(shaderPtr, resourceType, shiftBase, set);
    }


    public void SetOptions(ShaderOptions options)
    {
        GlslangNative.glslang_shader_set_options(shaderPtr, options);
    }


    public void SetGLSLVersion(int version)
    {
        GlslangNative.glslang_shader_set_glsl_version(shaderPtr, version);
    }

    
    private bool isPreprocessed = false;


    public bool Preprocess()
    {
        int result = GlslangNative.glslang_shader_preprocess(shaderPtr, compilerInputPtr);
        isPreprocessed = true;
        return result == 1; // Success
    }


    public bool Parse()
    {
        return GlslangNative.glslang_shader_parse(shaderPtr, compilerInputPtr) == 1; // Success
    }

    
    public string GetPreprocessedCode()
    {
        if (!isPreprocessed)
        {
            Preprocess();
            throw new WarningException(
                "Shader.GetPreprocessed() called before Shader.Preprocess(), Preprocess() called implicitly." + 
                "This may be a sign of bad control flow. Please ensure Preprocess() is called before GetPreprocessedCode()."
            );
        }

        IntPtr preprocessedCodePtr = GlslangNative.glslang_shader_get_preprocessed_code(shaderPtr);
        return DeallocString(preprocessedCodePtr);
    }


    public string GetInfoLog()
    {
        IntPtr infoLogPtr = GlslangNative.glslang_shader_get_info_debug_log(shaderPtr);
        return DeallocString(infoLogPtr);
    }


    public string GetDebugLog()
    {
        IntPtr debugLogPtr = GlslangNative.glslang_shader_get_info_log(shaderPtr);
        return DeallocString(debugLogPtr);
    }


    private static string DeallocString(IntPtr stringPtr)
    {
        string managedString = Marshal.PtrToStringUTF8(stringPtr) ?? string.Empty;
        return managedString;
    }
}