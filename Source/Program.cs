using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The shader program used to link and generate shader code.
/// </summary>
public unsafe class Program : NativeResource
{
    internal readonly unsafe NativeProgram* program;

    private bool generatedSPIRV;


    /// <summary>
    /// Creates a new <see cref="Program"/> instance. 
    /// </summary>
    public Program()
    {
        CompilationContext.EnsureInitialized();
        program = GlslangNative.CreateProgram();
        CompilationContext.WeakOnReloadCallback(this);
    }


    /// <summary>
    /// Adds a shader to the current compilation unit.
    /// </summary>
    public void AddShader(Shader shader)
    {
        Validate();
        shader.Validate();
        GlslangNative.AddShaderToProgram(program, shader.shader);
    }


    /// <summary>
    /// Adds reference source text for a given shader stage.
    /// </summary>
    public void AddSourceText(ShaderStage stage, string text)
    {
        Validate();
        ArgumentNullException.ThrowIfNull(text);

        Utf8String utf8String = new(text, false);
        GlslangNative.AddProgramSourceText(program, stage, utf8String.Bytes, (nuint)utf8String.Length);
        utf8String.Dispose();
    }


    internal override void Cleanup()
    {
        GlslangNative.DeleteProgram(program);
    }


    /// <summary>
    /// Generates and outputs the SPIR-V bytecode for a given shader stage.
    /// </summary>
    public unsafe bool GenerateSPIRV(out uint[] SPIRVWords, ShaderStage stage, SPIRVOptions? options = null)
    {
        Validate();

        if (options == null)
        {
            GlslangNative.GenerateProgramSPIRV(program, stage);
        }
        else
        {
            SPIRVOptions* optionsPtr = GlslangNative.Allocate(options.Value);
            GlslangNative.GenerateProgramSPIRVWithOptions(program, stage, optionsPtr);
            GlslangNative.Free(optionsPtr);
        }

        UIntPtr programSPIRVSize = GlslangNative.GetProgramSPIRVSize(program);

        SPIRVWords = new uint[(int)programSPIRVSize];

        GlslangNative.GetProgramSPIRVBuffer(program, SPIRVWords);

        generatedSPIRV = true;

        return programSPIRVSize != 0;
    }


    /// <summary>
    /// Get the debug output of the last performed operation.
    /// </summary>
    public string GetDebugLog()
    {
        Validate();
        return NativeUtil.GetUtf8(GlslangNative.GetProgramInfoLog(program));
    }


    /// <summary>
    /// Get the info output of the last performed operation.
    /// </summary>
    public string GetInfoLog()
    {
        Validate();
        return NativeUtil.GetUtf8(GlslangNative.GetProgramInfoDebugLog(program));
    }


    /// <summary>
    /// Get the SPIR-V message output of the last performed SPIR-V generation operation.
    /// </summary>
    public string GetSPIRVMessages()
    {
        Validate();

        if (!generatedSPIRV)
            throw new InvalidOperationException("ShaderProgram.GetSPIRVMessages() called before Shader.GenerateSPIRV().This is not allowed. Please ensure GenerateSPIRV() is called before GetSpirvMessages().");

        return NativeUtil.GetUtf8(GlslangNative.GetProgramSPIRVMessages(program));
    }


    /// <summary>
    /// Links and validates the added shaders. 
    /// </summary>
    public bool Link(MessageType messages)
    {
        Validate();
        return GlslangNative.LinkProgram(program, messages) == 1;
    }


    /// <summary>
    /// Maps the program's inputs and outputs.
    /// </summary>
    public bool MapIO()
    {
        Validate();
        return GlslangNative.MapProgramIO(program) == 1;
    }


    /// <summary>
    /// Maps the program's inputs and outputs using a given mapper and resolver pair.
    /// </summary>
    public bool MapIO(Mapper mapper, Resolver resolver)
    {
        Validate();
        mapper.Validate();
        resolver.Validate();

        return GlslangNative.MapProgramIOWithResolverAndMapper(program, resolver.resolver, mapper.mapper) == 1;
    }


    /// <summary>
    /// Sets a reference source file name for a given shader stage.
    /// </summary>
    public void SetSourceFile(ShaderStage stage, string file)
    {
        Validate();

        ArgumentNullException.ThrowIfNull(file);

        GlslangNative.SetProgramSourceFile(program, stage, file);
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