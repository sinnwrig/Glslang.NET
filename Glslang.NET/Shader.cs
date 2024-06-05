using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The shader used to preprocess and parse shader code.
/// </summary>
/// <remarks>
/// Ensure this class is only created through `CompilationContext.CreateShader`.
/// </remarks>
public class Shader
{   
    /// <summary>
    /// Input compilation options the shader will use during its lifecycle.
    /// </summary>
    /// <remarks>
    /// Once assigned, this setting is tied to a native structure allocation and cannot be changed.
    /// </remarks>
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


    /// <summary>
    /// Set preamble text that comes before any source code and after any pragma directives.
    /// </summary>
    /// <param name="preamble">Preamble text to insert.</param>
    public void SetPreamble(string preamble)
    {
        IntPtr preamblePtr = NativeStringUtility.AllocUTF8Ptr(preamble, out _, true);
        GlslangNative.SetShaderPreamble(shaderPtr, preamblePtr);
        Marshal.FreeHGlobal(preamblePtr);
    }


    /// <summary>
    /// Shift a resource binding by a given base.
    /// </summary>
    /// <param name="resourceType">Resource type to shift.</param>
    /// <param name="shiftBase">Base to shift by.</param>
    public void ShiftBinding(ResourceType resourceType, uint shiftBase)
    {
        GlslangNative.ShiftShaderBinding(shaderPtr, resourceType, shiftBase);
    }


    /// <summary>
    /// Shift a set of resource bindings by a given base.
    /// </summary>
    /// <param name="resourceType">Resource type to shift.</param>
    /// <param name="shiftBase">Base to shift by.</param>
    /// <param name="set">Set to shift.</param>
    public void ShiftBindingForSet(ResourceType resourceType, uint shiftBase, uint set)
    {
        GlslangNative.ShiftShaderBindingForSet(shaderPtr, resourceType, shiftBase, set);
    }


    /// <summary>
    /// Set shader compilation options.
    /// </summary>
    public void SetOptions(ShaderOptions options)
    {
        GlslangNative.SetShaderOptions(shaderPtr, options);
    }


    /// <summary>
    /// Set source file GLSL version.
    /// </summary>
    public void SetGLSLVersion(int version)
    {
        GlslangNative.SetShaderGLSLVersion(shaderPtr, version);
    }

    
    private bool isPreprocessed = false;


    /// <summary>
    /// Preprocess the shader.
    /// </summary>
    /// <returns>True if preprocessing was successful.</returns>
    public bool Preprocess()
    {
        int result = GlslangNative.PreprocessShader(shaderPtr, compilerInputPtr);
        isPreprocessed = true;
        return result == 1; // Success
    }


    /// <summary>
    /// Parse the shader.
    /// </summary>
    /// <returns>True if parsing was successful.</returns>
    public bool Parse()
    {
        return GlslangNative.ParseShader(shaderPtr, compilerInputPtr) == 1; // Success
    }

    
    /// <summary>
    /// Get preprocessed shader code.
    /// </summary>
    /// <returns>The preprocessed shader with macros expanded.</returns>
    /// <exception cref="WarningException"></exception>
    public string GetPreprocessedCode()
    {
        if (!isPreprocessed)
        {
            Preprocess();

            ConsoleColor prev = Console.ForegroundColor;
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: Shader.GetPreprocessed() called before Shader.Preprocess(), Preprocess() called implicitly." + 
                "This may be a sign of bad control flow. Please ensure Preprocess() is called before GetPreprocessedCode().");
            Console.ForegroundColor = prev;
        }

        IntPtr preprocessedCodePtr = GlslangNative.GetPreprocessedShaderCode(shaderPtr);
        return DeallocString(preprocessedCodePtr);
    }


    /// <summary>
    /// Get shader info log.
    /// </summary>
    public string GetInfoLog()
    {
        IntPtr infoLogPtr = GlslangNative.GetShaderInfoDebugLog(shaderPtr);
        return DeallocString(infoLogPtr);
    }

    
    /// <summary>
    /// Get shader debug and error logs.
    /// </summary>
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