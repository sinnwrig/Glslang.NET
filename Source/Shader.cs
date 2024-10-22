using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The shader used to preprocess and parse shader code.
/// </summary>
/// <remarks>
/// Ensure this class is only created through `CompilationContext.CreateShader`.
/// </remarks>
public unsafe class Shader : IDisposable
{
    /// <summary>
    /// Input compilation options the shader will use during its lifecycle.
    /// </summary>
    /// <remarks>
    /// Once assigned, this setting is tied to a native structure allocation and cannot be changed.
    /// </remarks>
    public readonly CompilationInput input;


    internal readonly NativeShader* shader;

    /// <summary>
    /// Has this <see cref="Shader"/> been disposed?
    /// <para>
    /// Using this instance when this value is true is not allowed and will throw exceptions.
    /// </para>
    /// </summary>
    public bool IsDisposed { get; private set; }

    private readonly NativeInput* nativeInputPtr;


    /// <summary>
    /// Creates a new shader instance from the given input options.
    /// </summary>
    /// <param name="input"></param>
    public Shader(CompilationInput input)
    {
        this.input = input;

        nativeInputPtr = NativeInput.Allocate(input);
        shader = GlslangNative.CreateShader(nativeInputPtr);

        CompilationContext.WeakOnReloadCallback(this);
    }


    /// <summary>
    /// Disposes of the current shader instance. Using the shader after calling this is prohibited.
    /// </summary>
    public void Dispose()
    {
        if (IsDisposed)
            return;

        IsDisposed = true;
        GlslangNative.DeleteShader(shader);
        NativeInput.Free(nativeInputPtr);
        GC.SuppressFinalize(this);
    }


    /// <summary></summary>
    ~Shader()
    {
        Dispose();
    }


    /// <summary>
    /// Set preamble text that comes before any source code and after any pragma directives.
    /// </summary>
    /// <param name="preamble">Preamble text to insert.</param>
    public void SetPreamble(string preamble)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.SetShaderPreamble(shader, preamble);
    }


    /// <summary>
    /// Shift a resource binding by a given base.
    /// </summary>
    /// <param name="resourceType">Resource type to shift.</param>
    /// <param name="shiftBase">Base to shift by.</param>
    public void ShiftBinding(ResourceType resourceType, uint shiftBase)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.ShiftShaderBinding(shader, resourceType, shiftBase);
    }


    /// <summary>
    /// Shift a set of resource bindings by a given base.
    /// </summary>
    /// <param name="resourceType">Resource type to shift.</param>
    /// <param name="shiftBase">Base to shift by.</param>
    /// <param name="set">Set to shift.</param>
    public void ShiftBindingForSet(ResourceType resourceType, uint shiftBase, uint set)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.ShiftShaderBindingForSet(shader, resourceType, shiftBase, set);
    }


    /// <summary>
    /// Set shader compilation options.
    /// </summary>
    public void SetOptions(ShaderOptions options)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.SetShaderOptions(shader, options);
    }


    /// <summary>
    /// Set source file GLSL version.
    /// </summary>
    public void SetGLSLVersion(int version)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.SetShaderGLSLVersion(shader, version);
    }


    private bool isPreprocessed = false;


    /// <summary>
    /// Preprocess the shader.
    /// </summary>
    /// <returns>True if preprocessing was successful.</returns>
    public bool Preprocess()
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        int result = GlslangNative.PreprocessShader(shader, nativeInputPtr);
        isPreprocessed = true;
        return result == 1; // Success
    }


    /// <summary>
    /// Parse the shader.
    /// </summary>
    /// <returns>True if parsing was successful.</returns>
    public bool Parse()
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        return GlslangNative.ParseShader(shader, nativeInputPtr) == 1; // Success
    }


    /// <summary>
    /// Set the name of the default uniform block containing the loose uniforms of the shader module.
    /// </summary>
    /// <param name="name">The name of the default uniform block.</param>
    public void SetDefaultUniformBlockName(string name)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.SetShaderDefaultUniformBlockName(shader, name);
    }


    /// <summary>
    /// Set the bindings of the resource sets in the shader module.
    /// </summary>
    /// <param name="bindings">The resource set bindings.</param>
    public void SetResourceSetBinding(string[] bindings)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.SetShaderResourceSetBinding(shader, bindings, (uint)bindings.Length);
    }


    /// <summary>
    /// Sets or overwrites the preprocessed code string.
    /// </summary>
    /// <param name="code">The preprocessed code to set.</param>
    public void SetPreprocessedCode(string code)
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.SetPreprocessedShaderCode(shader, code);
    }


    /// <summary>
    /// Get preprocessed shader code.
    /// </summary>
    /// <returns>The preprocessed shader with macros expanded.</returns>
    public string GetPreprocessedCode()
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        if (!isPreprocessed)
        {
            Preprocess();

            ConsoleColor prev = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Warning: Shader.GetPreprocessed() called before Shader.Preprocess(), Preprocess() called implicitly." +
                "This may be a sign of bad control flow. Please ensure Preprocess() is called before GetPreprocessedCode().");
            Console.ForegroundColor = prev;
        }

        return NativeUtil.GetUtf8(GlslangNative.GetPreprocessedShaderCode(shader));
    }


    /// <summary>
    /// Get shader info log.
    /// </summary>
    public string GetInfoLog()
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        return NativeUtil.GetUtf8(GlslangNative.GetShaderInfoDebugLog(shader));
    }


    /// <summary>
    /// Get shader debug and error logs.
    /// </summary>
    public string GetDebugLog()
    {
        if (IsDisposed)
            throw ShaderDisposedException.Disposed;

        return NativeUtil.GetUtf8(GlslangNative.GetShaderInfoLog(shader));
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