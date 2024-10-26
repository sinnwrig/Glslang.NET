using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Glslang.NET;


internal unsafe class Utf8String : SafeHandle
{
    public unsafe byte* Bytes => (byte*)handle;

    public readonly int Length;

    /// <inheritdoc/>
    public override bool IsInvalid => handle < 1;


    public unsafe Utf8String(string source, bool nullTerminate) : base(-1, true)
    {
        handle = (nint)NativeUtil.AllocateUTF8Ptr(source, out uint len, nullTerminate);
        Length = (int)len;
    }


    /// <inheritdoc/>
    protected override bool ReleaseHandle()
    {
        NativeMemory.Free(Bytes);
        handle = -1;

        return true;
    }
}