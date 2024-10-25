using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// A shader used to preprocess and parse shader code.
/// </summary>
public unsafe class Shader : NativeResource
{
    /// <summary>
    /// The input settings provided when creating this shader.
    /// </summary>
    public readonly CompilationInput input;

    internal readonly unsafe NativeShader* shader;

    private readonly unsafe NativeCompilationInput* nativeInputPtr;


    /// <summary>
    /// Creates a new <see cref="Shader"/> instance. 
    /// </summary>
    /// <param name="input">The input to use when compiling.</param>
    public Shader(CompilationInput input)
    {
        nativeInputPtr = NativeCompilationInput.Allocate(input);

        CompilationContext.EnsureInitialized();

        this.input = input;

        shader = GlslangNative.CreateShader(nativeInputPtr);

        CompilationContext.WeakOnReloadCallback(this);
    }


    internal override void Cleanup()
    {
        GlslangNative.DeleteShader(shader);
        NativeCompilationInput.Free(nativeInputPtr);
    }


    /// <summary>
    /// Get the debug output of the last performed operation.
    /// </summary>
    public string GetDebugLog()
    {
        Validate();
        return NativeUtil.GetUtf8(GlslangNative.GetShaderInfoLog(shader));
    }


    /// <summary>
    /// Get the debug output of the last performed operation.
    /// </summary>
    public string GetInfoLog()
    {
        Validate();
        return NativeUtil.GetUtf8(GlslangNative.GetShaderInfoDebugLog(shader));
    }


    /// <summary>
    /// Get the preprocessed shader string.
    /// </summary>
    public string GetPreprocessedCode()
    {
        Validate();
        return NativeUtil.GetUtf8(GlslangNative.GetPreprocessedShaderCode(shader));
    }


    /// <summary>
    /// Parse the preprocessed shader string into an AST.
    /// </summary>
    public bool Parse()
    {
        Validate();
        return GlslangNative.ParseShader(shader, nativeInputPtr) == 1;
    }


    /// <summary>
    /// Preprocess the input shader string, expand macros, and resolve include directives.
    /// </summary>
    public bool Preprocess()
    {
        Validate();
        return GlslangNative.PreprocessShader(shader, nativeInputPtr) == 1;
    }


    /// <summary>
    /// Set the default global uniform block's name.
    /// </summary>
    public void SetDefaultUniformBlockName(string name)
    {
        Validate();
        GlslangNative.SetDefaultUniformBlockName(shader, name);
    }


    /// <summary>
    /// Set the default global uniform block's set and binding.
    /// </summary>
    public void SetDefaultUniformBlockSetAndBinding(uint set, uint binding)
    {
        Validate();
        GlslangNative.SetDefaultUniformBlockSetAndBinding(shader, set, binding);
    }


    /// <summary>
    /// Set the active GLSL version to reference when preprocessing and parsing.
    /// </summary>
    public void SetGLSLVersion(int version)
    {
        Validate();
        GlslangNative.SetShaderGLSLVersion(shader, version);
    }


    /// <summary>
    /// Set the shader options.
    /// </summary>
    public void SetOptions(ShaderOptions options)
    {
        Validate();
        GlslangNative.SetShaderOptions(shader, options);
    }


    /// <summary>
    /// Set the preamble text that is processed before any other part of the source code string.
    /// </summary>
    public void SetPreamble(string preamble)
    {
        Validate();
        Utf8String utf8String = new Utf8String(preamble, true);

        AddSubresource("_preamble", utf8String);

        GlslangNative.SetShaderPreamble(shader, utf8String.Bytes);
    }


    /// <summary>
    /// Set the preprocessed shader code, skipping the need to call <see cref="Preprocess"/> 
    /// </summary>
    public void SetPreprocessedShaderCode(string code)
    {
        Validate();
        GlslangNative.SetPreprocessedShaderCode(shader, code);
    }


    /// <summary>
    /// Shift the binding of the given resource type by a base.
    /// </summary>
    public void ShiftBinding(ResourceType resourceType, uint shiftBase)
    {
        Validate();
        GlslangNative.ShiftShaderBinding(shader, resourceType, shiftBase);
    }


    /// <summary>
    /// Shift the set binding of a given resource type by a base.
    /// </summary>
    public void ShiftBindingForSet(ResourceType resourceType, uint shiftBase, uint set)
    {
        Validate();
        GlslangNative.ShiftShaderBindingForSet(shader, resourceType, shiftBase, set);
    }
}


/// <summary>
/// Thrown if an already disposed <see cref="Shader"/> is used.
/// </summary>
public class ShaderDisposedException : Exception
{
    internal static ShaderDisposedException Disposed = new ShaderDisposedException("Shader is disposed");

    internal ShaderDisposedException(string message) : base(message) { }
}