using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Glslang.NET;


internal unsafe class Utf8String : NativeResource
{
    public readonly unsafe byte* Bytes;

    public readonly int Length;


    public unsafe Utf8String(string source, bool nullTerminate)
    {
        Bytes = NativeUtil.AllocateUTF8Ptr(source, out uint len, nullTerminate);
        Length = (int)len;
    }


    internal override void Cleanup()
    {
        Marshal.FreeHGlobal((nint)Bytes);
    }
}