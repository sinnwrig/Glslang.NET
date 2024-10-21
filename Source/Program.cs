using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The shader program used to link and generate shader code.
/// </summary>
public unsafe class Program : IDisposable
{
    readonly NativeProgram* program;

    private bool isDisposed;


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
        if (isDisposed)
            return;

        isDisposed = true;
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
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        if (shader.isDisposed)
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
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        return GlslangNative.LinkProgram(program, messages) == 1;
    }


    /// <summary>
    /// Add source text to intermediate.
    /// </summary>
    public void AddSourceText(ShaderStage stage, string text)
    {
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        IntPtr textPtr = NativeStringUtility.AllocUTF8Ptr(text, out uint length, false);
        GlslangNative.AddProgramSourceText(program, stage, textPtr, length);
        Marshal.FreeHGlobal(textPtr);
    }


    /// <summary>
    /// Add source file name to intermediate.
    /// </summary>
    public void SetSourceFile(ShaderStage stage, string file)
    {
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        IntPtr filePtr = NativeStringUtility.AllocUTF8Ptr(file, out _, true);
        GlslangNative.SetProgramSourceFile(program, stage, filePtr);
        Marshal.FreeHGlobal(filePtr);
    }


    /// <summary>
    /// Map the program's imputs and outputs.
    /// </summary>
    /// <returns>True if mapping succeeded.</returns>
    public bool MapIO()
    {
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        return GlslangNative.MapProgramIO(program) == 1;
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
    public bool GenerateSPIRV(out byte[] SPIRVWords, ShaderStage stage, SPIRVOptions? options = null)
    {
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        if (options != null)
        {
            IntPtr optionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<SPIRVOptions>());
            Marshal.StructureToPtr(options.Value, optionsPtr, false);
            GlslangNative.GenerateProgramSPIRVWithOptiosn(program, stage, optionsPtr);
            Marshal.FreeHGlobal(optionsPtr);
            GlslangNative.GenerateProgramSPIRV(program, stage);
        }
        else
        {
            GlslangNative.GenerateProgramSPIRV(program, stage);
        }

        nuint size = GlslangNative.GetProgramSPIRVSize(program);
        SPIRVWords = new byte[(int)size * sizeof(uint)];
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
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        if (!generatedSPIRV)
        {
            throw new InvalidOperationException(
                "ShaderProgram.GetSPIRVMessages() called before Shader.GenerateSPIRV()." +
                "This is not allowed. Please ensure GenerateSPIRV() is called before GetSpirvMessages()."
            );
        }

        IntPtr SPIRVMessagesPtr = GlslangNative.GetProgramSPIRVMessages(program);
        return Marshal.PtrToStringUTF8(SPIRVMessagesPtr) ?? string.Empty;
    }


    /// <summary>
    /// Gets program info log.
    /// </summary>
    public string GetInfoLog()
    {
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        IntPtr infoLogPtr = GlslangNative.GetProgramInfoDebugLog(program);
        return Marshal.PtrToStringUTF8(infoLogPtr) ?? string.Empty;
    }


    /// <summary>
    /// Gets program debug and error logs.
    /// </summary>
    public string GetDebugLog()
    {
        if (isDisposed)
            throw ProgramDisposedException.Disposed;

        IntPtr debugLogPtr = GlslangNative.GetProgramInfoLog(program);
        return Marshal.PtrToStringUTF8(debugLogPtr) ?? string.Empty;
    }
}


/// <summary>
/// Returned if a shader method is called on an already disposed shader.
/// </summary>
public class ProgramDisposedException : Exception
{
    internal static ProgramDisposedException Disposed = new ProgramDisposedException("Program is disposed");

    internal ProgramDisposedException(string message) : base(message) { }
}