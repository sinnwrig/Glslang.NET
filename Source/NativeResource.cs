using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Glslang.NET;


/// <summary>
/// A native resource handle wrapper.
/// </summary>
public unsafe class NativeResource : IDisposable
{
    private Dictionary<string, NativeResource> _subresources = [];


    /// <summary>
    /// Whether or not this resource instance is disposed. Using the resource when this is true is prohibited. 
    /// </summary>
    public bool IsDisposed { get; private set; }


    internal void AddSubresource(string ID, NativeResource subresource)
    {
        _subresources ??= [];

        if (_subresources.TryGetValue(ID, out NativeResource? nativeResource) && nativeResource != null)
            nativeResource.Dispose();

        _subresources[ID] = subresource;
    }


    internal virtual void Cleanup() { }


    /// <summary>
    /// Disposes of this resource instance. Using the resource after calling this is prohibited.
    /// </summary>
    public void Dispose()
    {
        if (IsDisposed)
            return;

        Cleanup();
        IsDisposed = true;

        foreach (NativeResource value in _subresources.Values)
            value.Dispose();

        GC.SuppressFinalize(this);
    }


    /// <summary></summary>
    ~NativeResource()
    {
        Dispose();
    }


    internal void Validate()
    {
        if (IsDisposed)
            throw new ResourceDisposedException(this);
    }
}


/// <summary>
/// Thrown if an already disposed <see cref="NativeResource"/> is used.
/// </summary>
public class ResourceDisposedException : Exception
{
    internal ResourceDisposedException(NativeResource resource) : base($"{resource.GetType().Name} is already disposed.") { }
}