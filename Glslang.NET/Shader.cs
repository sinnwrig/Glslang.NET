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
        shaderPtr = GlslangNative.CreateShader(compilerInputPtr);
    }


    internal void Release()
    {
        GlslangNative.DeleteShader(shaderPtr);
        CompilationInputNative.ReleasePtrForCompilationInput(compilerInputPtr);
    }



    public void SetPreamble(string preamble)
    {
        IntPtr preamblePtr = NativeStringUtility.AllocUTF8Ptr(preamble, out _, true);
        GlslangNative.SetShaderPreamble(shaderPtr, preamblePtr);
        Marshal.FreeHGlobal(preamblePtr);
    }


    public void ShiftBinding(ResourceType resourceType, uint shiftBase)
    {
        GlslangNative.ShiftShaderBinding(shaderPtr, resourceType, shiftBase);
    }


    public void ShiftBindingForSet(ResourceType resourceType, uint shiftBase, uint set)
    {
        GlslangNative.ShiftShaderBindingForSet(shaderPtr, resourceType, shiftBase, set);
    }


    public void SetOptions(ShaderOptions options)
    {
        GlslangNative.SetShaderOptions(shaderPtr, options);
    }


    public void SetGLSLVersion(int version)
    {
        GlslangNative.SetShaderGLSLVersion(shaderPtr, version);
    }

    
    private bool isPreprocessed = false;


    public bool Preprocess()
    {
        int result = GlslangNative.PreprocessShader(shaderPtr, compilerInputPtr);
        isPreprocessed = true;
        return result == 1; // Success
    }


    public bool Parse()
    {
        return GlslangNative.ParseShader(shaderPtr, compilerInputPtr) == 1; // Success
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

        IntPtr preprocessedCodePtr = GlslangNative.GetPreprocessedShaderCode(shaderPtr);
        return DeallocString(preprocessedCodePtr);
    }


    public string GetInfoLog()
    {
        IntPtr infoLogPtr = GlslangNative.GetShaderInfoDebugLog(shaderPtr);
        return DeallocString(infoLogPtr);
    }


    public string GetDebugLog()
    {
        IntPtr debugLogPtr = GlslangNative.GetShaderInfoLog(shaderPtr);
        return DeallocString(debugLogPtr);
    }


    private static string DeallocString(IntPtr stringPtr)
    {
        string managedString = Marshal.PtrToStringUTF8(stringPtr) ?? string.Empty;
        return managedString;
    }
}