using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary></summary>
public unsafe class Mapper : IDisposable
{
    internal readonly NativeMapper* mapper;

    /// <summary>
    /// Has this <see cref="Mapper"/> been disposed?
    /// <para>
    /// Using this instance when this value is true is not allowed and will throw exceptions.
    /// </para>
    /// </summary>
    public bool IsDisposed { get; private set; }


    /// <summary></summary>
    public Mapper()
    {
        mapper = GlslangNative.CreateMapper();

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
        GlslangNative.DeleteMapper(mapper);
        GC.SuppressFinalize(this);
    }


    /// <summary></summary>
    ~Mapper()
    {
        Dispose();
    }
}


/// <summary>
/// Thrown if an already disposed <see cref="Mapper"/> is used.
/// </summary>
public class MapperDisposedException : Exception
{
    internal static MapperDisposedException Disposed = new MapperDisposedException("Mapper is disposed");

    internal MapperDisposedException(string message) : base(message) { }
}