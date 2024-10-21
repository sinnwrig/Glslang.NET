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
    internal bool isDisposed;

    private readonly IntPtr compilerInputPtr;


    /// <summary>
    /// Creates a new shader instance from the given input options.
    /// </summary>
    /// <param name="input"></param>
    public Shader(CompilationInput input)
    {
        this.input = input;

        compilerInputPtr = CompilationInputNative.GetPtrForCompilationInput(input);
        shader = GlslangNative.CreateShader(compilerInputPtr);

        CompilationContext.WeakOnReloadCallback(this);
    }


    /// <summary>
    /// Disposes of the current shader instance. Using the shader after calling this is prohibited.
    /// </summary>
    public void Dispose()
    {
        if (isDisposed)
            return;

        isDisposed = true;
        GlslangNative.DeleteShader(shader);
        CompilationInputNative.ReleasePtrForCompilationInput(compilerInputPtr);
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
        if (isDisposed)
            throw ShaderDisposedException.Disposed;

        IntPtr preamblePtr = NativeStringUtility.AllocUTF8Ptr(preamble, out _, true);
        GlslangNative.SetShaderPreamble(shader, preamblePtr);
        Marshal.FreeHGlobal(preamblePtr);
    }


    /// <summary>
    /// Shift a resource binding by a given base.
    /// </summary>
    /// <param name="resourceType">Resource type to shift.</param>
    /// <param name="shiftBase">Base to shift by.</param>
    public void ShiftBinding(ResourceType resourceType, uint shiftBase)
    {
        if (isDisposed)
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
        if (isDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.ShiftShaderBindingForSet(shader, resourceType, shiftBase, set);
    }


    /// <summary>
    /// Set shader compilation options.
    /// </summary>
    public void SetOptions(ShaderOptions options)
    {
        if (isDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.SetShaderOptions(shader, options);
    }


    /// <summary>
    /// Set source file GLSL version.
    /// </summary>
    public void SetGLSLVersion(int version)
    {
        if (isDisposed)
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
        if (isDisposed)
            throw ShaderDisposedException.Disposed;

        int result = GlslangNative.PreprocessShader(shader, compilerInputPtr);
        isPreprocessed = true;
        return result == 1; // Success
    }


    /// <summary>
    /// Parse the shader.
    /// </summary>
    /// <returns>True if parsing was successful.</returns>
    public bool Parse()
    {
        if (isDisposed)
            throw ShaderDisposedException.Disposed;

        return GlslangNative.ParseShader(shader, compilerInputPtr) == 1; // Success
    }


    /// <summary>
    /// Get preprocessed shader code.
    /// </summary>
    /// <returns>The preprocessed shader with macros expanded.</returns>
    /// <exception cref="WarningException"></exception>
    public string GetPreprocessedCode()
    {
        if (isDisposed)
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

        IntPtr preprocessedCodePtr = GlslangNative.GetPreprocessedShaderCode(shader);
        return Marshal.PtrToStringUTF8(preprocessedCodePtr) ?? string.Empty;
    }


    /// <summary>
    /// Get shader info log.
    /// </summary>
    public string GetInfoLog()
    {
        if (isDisposed)
            throw ShaderDisposedException.Disposed;

        IntPtr infoLogPtr = GlslangNative.GetShaderInfoDebugLog(shader);
        return Marshal.PtrToStringUTF8(infoLogPtr) ?? string.Empty;
    }


    /// <summary>
    /// Get shader debug and error logs.
    /// </summary>
    public string GetDebugLog()
    {
        if (isDisposed)
            throw ShaderDisposedException.Disposed;

        IntPtr debugLogPtr = GlslangNative.GetShaderInfoLog(shader);
        return Marshal.PtrToStringUTF8(debugLogPtr) ?? string.Empty;
    }
}


/// <summary>
/// Returned if a shader method is called on an already disposed shader.
/// </summary>
public class ShaderDisposedException : Exception
{
    internal static ShaderDisposedException Disposed = new ShaderDisposedException("Shader is disposed");

    internal ShaderDisposedException(string message) : base(message) { }
}