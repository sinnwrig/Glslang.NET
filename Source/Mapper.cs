using System;
using System.Runtime.CompilerServices;

namespace Glslang.NET;


/// <summary></summary>
public unsafe class Mapper : NativeResource
{
    internal readonly unsafe NativeMapper* mapper;


    /// <summary></summary>
    public Mapper()
    {
        CompilationContext.EnsureInitialized();
        this.mapper = GlslangNative.CreateGLSLMapper();
        CompilationContext.WeakOnReloadCallback(this);
    }


    internal override void Cleanup()
    {
        GlslangNative.DeleteGLSLMapper(this.mapper);
    }
}