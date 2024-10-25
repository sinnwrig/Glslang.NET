using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// A shader used to preprocess and parse shader code.
/// </summary>
public unsafe class Shader : SafeHandle
{
    /// <summary>
    /// The input settings provided when creating this shader.
    /// </summary>
    public readonly CompilationInput input;

    internal unsafe NativeShader* ShaderPtr => (NativeShader*)handle;

    /// <inheritdoc/>
    public override bool IsInvalid => handle < 1;

    private readonly unsafe NativeCompilationInput* nativeInputPtr;

    private Utf8String? _preamble = null;


    /// <summary>
    /// Creates a new <see cref="Shader"/> instance. 
    /// </summary>
    /// <param name="input">The input to use when compiling.</param>
    public Shader(CompilationInput input) : base(-1, true)
    {
        nativeInputPtr = NativeCompilationInput.Allocate(input);

        CompilationContext.EnsureInitialized();

        this.input = input;

        handle = (nint)GlslangNative.CreateShader(nativeInputPtr);
    }


    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        GlslangNative.DeleteShader(ShaderPtr);
        NativeCompilationInput.Free(nativeInputPtr);

        handle = -1;
        return true;
    }


    /// <summary>
    /// Get the debug output of the last performed operation.
    /// </summary>
    public string GetDebugLog()
    {
        return NativeUtil.GetUtf8(GlslangNative.GetShaderInfoLog(ShaderPtr));
    }


    /// <summary>
    /// Get the debug output of the last performed operation.
    /// </summary>
    public string GetInfoLog()
    {
        return NativeUtil.GetUtf8(GlslangNative.GetShaderInfoDebugLog(ShaderPtr));
    }


    /// <summary>
    /// Get the preprocessed shader string.
    /// </summary>
    public string GetPreprocessedCode()
    {
        return NativeUtil.GetUtf8(GlslangNative.GetPreprocessedShaderCode(ShaderPtr));
    }


    /// <summary>
    /// Parse the preprocessed shader string into an AST.
    /// </summary>
    public bool Parse()
    {
        return GlslangNative.ParseShader(ShaderPtr, nativeInputPtr) == 1;
    }


    /// <summary>
    /// Preprocess the input ShaderPtr string, expand macros, and resolve include directives.
    /// </summary>
    public bool Preprocess()
    {
        return GlslangNative.PreprocessShader(ShaderPtr, nativeInputPtr) == 1;
    }


    /// <summary>
    /// Set the default global uniform block's name.
    /// </summary>
    public void SetDefaultUniformBlockName(string name)
    {
        GlslangNative.SetDefaultUniformBlockName(ShaderPtr, name);
    }


    /// <summary>
    /// Set the default global uniform block's set and binding.
    /// </summary>
    public void SetDefaultUniformBlockSetAndBinding(uint set, uint binding)
    {
        GlslangNative.SetDefaultUniformBlockSetAndBinding(ShaderPtr, set, binding);
    }


    /// <summary>
    /// Set the active GLSL version to reference when preprocessing and parsing.
    /// </summary>
    public void SetGLSLVersion(int version)
    {
        GlslangNative.SetShaderGLSLVersion(ShaderPtr, version);
    }


    /// <summary>
    /// Set the shader options.
    /// </summary>
    public void SetOptions(ShaderOptions options)
    {
        GlslangNative.SetShaderOptions(ShaderPtr, options);
    }


    /// <summary>
    /// Set the preamble text that is processed before any other part of the source code string.
    /// </summary>
    public void SetPreamble(string preamble)
    {
        _preamble = new Utf8String(preamble, true);
        GlslangNative.SetShaderPreamble(ShaderPtr, _preamble.Bytes);
    }


    /// <summary>
    /// Set the preprocessed shader code, skipping the need to call <see cref="Preprocess"/> 
    /// </summary>
    public void SetPreprocessedShaderCode(string code)
    {
        GlslangNative.SetPreprocessedShaderCode(ShaderPtr, code);
    }


    /// <summary>
    /// Shift the binding of the given resource type by a base.
    /// </summary>
    public void ShiftBinding(ResourceType resourceType, uint shiftBase)
    {
        GlslangNative.ShiftShaderBinding(ShaderPtr, resourceType, shiftBase);
    }


    /// <summary>
    /// Shift the set binding of a given resource type by a base.
    /// </summary>
    public void ShiftBindingForSet(ResourceType resourceType, uint shiftBase, uint set)
    {
        GlslangNative.ShiftShaderBindingForSet(ShaderPtr, resourceType, shiftBase, set);
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