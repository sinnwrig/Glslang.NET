using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The shader program used to link and generate shader code.
/// </summary>
public unsafe class Program : SafeHandle
{
    internal unsafe NativeProgram* ProgramPtr => (NativeProgram*)handle;

    private List<Shader> _trackedShaders;
    private bool _generatedSPIRV;

    /// <inheritdoc/>
    public override bool IsInvalid => handle < 1;


    /// <summary>
    /// Creates a new <see cref="Program"/> instance. 
    /// </summary>
    public Program() : base(-1, true)
    {
        CompilationContext.EnsureInitialized();
        handle = (nint)GlslangNative.CreateProgram();
        _trackedShaders = [];
    }


    /// <summary>
    /// Adds a shader to the current compilation unit.
    /// </summary>
    public void AddShader(Shader shader)
    {
        _trackedShaders.Add(shader);
        GlslangNative.AddShaderToProgram(ProgramPtr, shader.ShaderPtr);
    }


    /// <summary>
    /// Adds reference source text for a given shader stage.
    /// </summary>
    public void AddSourceText(ShaderStage stage, string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        byte* utf8String = NativeUtil.AllocateUTF8Ptr(text, out uint len, false);
        GlslangNative.AddProgramSourceText(ProgramPtr, stage, utf8String, len);
        GlslangNative.Free(utf8String);
    }


    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        _trackedShaders.Clear();
        GlslangNative.DeleteProgram(ProgramPtr);
        handle = -1;

        return true;
    }


    /// <summary>
    /// Generates and outputs the SPIR-V bytecode for a given shader stage.
    /// </summary>
    public unsafe bool GenerateSPIRV(out uint[] SPIRVWords, ShaderStage stage, SPIRVOptions? options = null)
    {
        if (options == null)
        {
            GlslangNative.GenerateProgramSPIRV(ProgramPtr, stage);
        }
        else
        {
            SPIRVOptions* optionsPtr = GlslangNative.Allocate(options.Value);
            GlslangNative.GenerateProgramSPIRVWithOptions(ProgramPtr, stage, optionsPtr);
            GlslangNative.Free(optionsPtr);
        }

        UIntPtr programSPIRVSize = GlslangNative.GetProgramSPIRVSize(ProgramPtr);

        SPIRVWords = new uint[(int)programSPIRVSize];

        GlslangNative.GetProgramSPIRVBuffer(ProgramPtr, SPIRVWords);

        _generatedSPIRV = true;

        return programSPIRVSize != 0;
    }


    /// <summary>
    /// Get the debug output of the last performed operation.
    /// </summary>
    public string GetDebugLog()
    {
        return NativeUtil.GetUtf8(GlslangNative.GetProgramInfoLog(ProgramPtr));
    }


    /// <summary>
    /// Get the info output of the last performed operation.
    /// </summary>
    public string GetInfoLog()
    {
        return NativeUtil.GetUtf8(GlslangNative.GetProgramInfoDebugLog(ProgramPtr));
    }


    /// <summary>
    /// Get the SPIR-V message output of the last performed SPIR-V generation operation.
    /// </summary>
    public string GetSPIRVMessages()
    {
        if (!_generatedSPIRV)
            throw new InvalidOperationException("ShaderProgram.GetSPIRVMessages() called before Shader.GenerateSPIRV().This is not allowed. Please ensure GenerateSPIRV() is called before GetSpirvMessages().");

        return NativeUtil.GetUtf8(GlslangNative.GetProgramSPIRVMessages(ProgramPtr));
    }


    /// <summary>
    /// Links and validates the added shaders. 
    /// </summary>
    public bool Link(MessageType messages)
    {
        if (_trackedShaders.Any(x => x.IsInvalid))
            throw new ShaderDisposedException("Attempted to link with disposed shader");

        return GlslangNative.LinkProgram(ProgramPtr, messages) == 1;
    }


    /// <summary>
    /// Maps the program's inputs and outputs.
    /// </summary>
    public bool MapIO()
    {
        return GlslangNative.MapProgramIO(ProgramPtr) == 1;
    }


    /// <summary>
    /// Maps the program's inputs and outputs using a given mapper and resolver pair.
    /// </summary>
    public bool MapIO(Mapper mapper, Resolver resolver)
    {
        ArgumentNullException.ThrowIfNull(mapper);
        ArgumentNullException.ThrowIfNull(resolver);

        return GlslangNative.MapProgramIOWithResolverAndMapper(ProgramPtr, resolver.ResolverPtr, mapper.MapperPtr) == 1;
    }


    /// <summary>
    /// Sets a reference source file name for a given shader stage.
    /// </summary>
    public void SetSourceFile(ShaderStage stage, string file)
    {
        ArgumentNullException.ThrowIfNull(file);

        GlslangNative.SetProgramSourceFile(ProgramPtr, stage, file);
    }
}