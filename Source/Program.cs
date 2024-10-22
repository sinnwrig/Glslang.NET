using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The shader program used to link and generate shader code.
/// </summary>
public unsafe class Program : IDisposable
{
    internal readonly NativeProgram* program;

    /// <summary>
    /// Has this <see cref="Program"/> been disposed?
    /// <para>
    /// Using this instance when this value is true is not allowed and will throw exceptions.
    /// </para>
    /// </summary>
    public bool IsDisposed { get; private set; }


    /// <summary>
    /// Create a new program instance.
    /// </summary>
    public Program()
    {
        program = GlslangNative.CreateProgram();

        CompilationContext.WeakOnReloadCallback(this);
    }


    /// <summary>
    /// Disposes of the current program instance. Using the program after calling this is prohibited.
    /// </summary>
    public void Dispose()
    {
        if (IsDisposed)
            return;

        IsDisposed = true;
        GlslangNative.DeleteProgram(program);
        GC.SuppressFinalize(this);
    }


    /// <summary></summary>
    ~Program()
    {
        Dispose();
    }


    /// <summary>
    /// Add a shader to the program. 
    /// </summary>
    /// <param name="shader">The shader to add.</param>
    public void AddShader(Shader shader)
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        if (shader.IsDisposed)
            throw ShaderDisposedException.Disposed;

        GlslangNative.AddShaderToProgram(program, shader.shader);
    }


    /// <summary>
    /// Link added shaders together.
    /// </summary>
    /// <param name="messages">Output message types.</param>
    /// <returns>True if linking succeeded.</returns>
    public bool Link(MessageType messages)
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        return GlslangNative.LinkProgram(program, messages) == 1;
    }


    /// <summary>
    /// Add source text to intermediate.
    /// </summary>
    public void AddSourceText(ShaderStage stage, string text)
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        ArgumentNullException.ThrowIfNull(text);

        byte* textPtr = NativeUtil.AllocUTF8Ptr(text, out uint length, false);
        GlslangNative.AddProgramSourceText(program, stage, textPtr, length);
        GlslangNative.Free(textPtr);
    }


    /// <summary>
    /// Add source file name to intermediate.
    /// </summary>
    public void SetSourceFile(ShaderStage stage, string file)
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        ArgumentNullException.ThrowIfNull(file);

        GlslangNative.SetProgramSourceFile(program, stage, file);
    }


    /// <summary>
    /// Map the program's imputs and outputs.
    /// </summary>
    /// <returns>True if mapping succeeded.</returns>
    public bool MapIO()
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        return GlslangNative.MapProgramIO(program) == 1;
    }


    /// <summary>
    /// Map the program's imputs and outputs.
    /// </summary>
    /// <returns>True if mapping succeeded.</returns>
    public bool MapIO(Mapper mapper, Resolver resolver)
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        if (mapper.IsDisposed)
            throw MapperDisposedException.Disposed;

        if (resolver.IsDisposed)
            throw ResolverDisposedException.Disposed;

        return GlslangNative.MapProgramIOWithResolverAndMapper(program, resolver.resolver, mapper.mapper) == 1;
    }


    // SPIR-V generation

    private bool generatedSPIRV = false;

    /// <summary>
    /// Outputs a byte buffer of generated SPIR-V words.
    /// </summary>
    /// <param name="SPIRVWords">The output buffer of SPIR-V words</param>
    /// <param name="stage">The shader stage to output.</param>
    /// <param name="options">The generation options to use.</param>
    /// <returns>True if generation succeeded.</returns>
    public bool GenerateSPIRV(out uint[] SPIRVWords, ShaderStage stage, SPIRVOptions? options = null)
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        if (options != null)
        {
            SPIRVOptions* optionsPtr = GlslangNative.Allocate(options.Value);
            GlslangNative.GenerateProgramSPIRVWithOptions(program, stage, optionsPtr);
            GlslangNative.Free(optionsPtr);
            GlslangNative.GenerateProgramSPIRV(program, stage);
        }
        else
        {
            GlslangNative.GenerateProgramSPIRV(program, stage);
        }

        nuint size = GlslangNative.GetProgramSPIRVSize(program);
        SPIRVWords = new uint[(int)size];
        GlslangNative.GetProgramSPIRVBuffer(program, SPIRVWords);

        generatedSPIRV = true;

        return size != 0;
    }


    /// <summary>
    /// Gets SPIR-V generation messages.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public string GetSPIRVMessages()
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        if (!generatedSPIRV)
        {
            throw new InvalidOperationException(
                "ShaderProgram.GetSPIRVMessages() called before Shader.GenerateSPIRV()." +
                "This is not allowed. Please ensure GenerateSPIRV() is called before GetSpirvMessages()."
            );
        }

        return NativeUtil.GetUtf8(GlslangNative.GetProgramSPIRVMessages(program));
    }


    /// <summary>
    /// Gets program info log.
    /// </summary>
    public string GetInfoLog()
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        return NativeUtil.GetUtf8(GlslangNative.GetProgramInfoDebugLog(program));
    }


    /// <summary>
    /// Gets program debug and error logs.
    /// </summary>
    public string GetDebugLog()
    {
        if (IsDisposed)
            throw ProgramDisposedException.Disposed;

        return NativeUtil.GetUtf8(GlslangNative.GetProgramInfoLog(program));
    }
}


/// <summary>
/// Thrown if an already disposed <see cref="Program"/> is used.
/// </summary>
public class ProgramDisposedException : Exception
{
    internal static ProgramDisposedException Disposed = new ProgramDisposedException("Program is disposed");

    internal ProgramDisposedException(string message) : base(message) { }
}