using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary/>
public unsafe class Resolver : SafeHandle
{
    internal unsafe NativeResolver* ResolverPtr => (NativeResolver*)handle;


    /// <summary/>
    public Resolver(Program program, ShaderStage stage) : base(-1, true)
    {
        ArgumentNullException.ThrowIfNull(program);

        CompilationContext.EnsureInitialized();
        handle = (nint)GlslangNative.CreateGLSLResolver(program.ProgramPtr, stage);
    }

    /// <inheritdoc/>
    public override bool IsInvalid => handle < 1;

    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        GlslangNative.DeleteGLSLResolver(ResolverPtr);
        handle = -1;

        return true;
    }
}