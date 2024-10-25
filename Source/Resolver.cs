using System;
using System.Runtime.CompilerServices;

namespace Glslang.NET;


/// <summary></summary>
public unsafe class Resolver : NativeResource
{
    internal readonly unsafe NativeResolver* resolver;


    /// <summary></summary>
    public Resolver(Program program, ShaderStage stage)
    {
        program.Validate();
        CompilationContext.EnsureInitialized();
        this.resolver = GlslangNative.CreateGLSLResolver(program.program, stage);
        CompilationContext.WeakOnReloadCallback(this);
    }


    internal override void Cleanup()
    {
        GlslangNative.DeleteGLSLResolver(this.resolver);
    }
}