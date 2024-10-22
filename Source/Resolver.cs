using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary></summary>
public unsafe class Resolver : IDisposable
{
    internal readonly NativeResolver* resolver;

    /// <summary>
    /// Has this <see cref="Resolver"/> been disposed?
    /// <para>
    /// Using this instance when this value is true is not allowed and will throw exceptions.
    /// </para>
    /// </summary>
    public bool IsDisposed { get; private set; }


    /// <summary></summary>
    public Resolver(Program program, ShaderStage stage)
    {
        CompilationContext.EnsureInitialized();

        if (program.IsDisposed)
            throw ProgramDisposedException.Disposed;

        resolver = GlslangNative.CreateResolver(program.program, stage);

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
        GlslangNative.DeleteResolver(resolver);
        GC.SuppressFinalize(this);
    }


    /// <summary></summary>
    ~Resolver()
    {
        Dispose();
    }
}


/// <summary>
/// Thrown if an already disposed <see cref="Resolver"/> is used.
/// </summary>
public class ResolverDisposedException : Exception
{
    internal static ResolverDisposedException Disposed = new ResolverDisposedException("Resolver is disposed");

    internal ResolverDisposedException(string message) : base(message) { }
}