using System.Text;
using System.Runtime.InteropServices;

namespace Glslang.NET;


internal static unsafe class NativeUtil
{
    internal static unsafe byte* AllocateUTF8Ptr(string str, out uint len, bool nullTerminate = true)
    {
        if (nullTerminate && str[^1] != '\0')
            str += '\0';

        len = (uint)Encoding.UTF8.GetByteCount(str);
        byte* bytePtr = (byte*)Marshal.AllocHGlobal((int)len);

        fixed (char* strPtr = str)
            Encoding.UTF8.GetBytes(strPtr, str.Length, bytePtr, (int)len);

        return bytePtr;
    }


    internal static unsafe string GetUtf8(byte* utf8Bytes)
    {
        return Marshal.PtrToStringUTF8((nint)utf8Bytes) ?? "";
    }
}