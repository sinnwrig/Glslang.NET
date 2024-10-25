using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary/>
public unsafe class Mapper : SafeHandle
{
    internal unsafe NativeMapper* MapperPtr => (NativeMapper*)handle;


    /// <summary/>
    public Mapper() : base(-1, true)
    {
        CompilationContext.EnsureInitialized();
        handle = (nint)GlslangNative.CreateGLSLMapper();
    }

    /// <inheritdoc/>
    public override bool IsInvalid => handle < 1;

    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        GlslangNative.DeleteGLSLMapper(MapperPtr);
        handle = -1;

        return true;
    }
}