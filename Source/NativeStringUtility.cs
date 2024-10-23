using System.Text;
using System.Runtime.InteropServices;

namespace Glslang.NET;


internal static unsafe class NativeUtil
{
    private static byte* AllocBytePtr(byte[] bytes, out uint len)
    {
        len = (uint)bytes.Length;

        IntPtr nativePtr = Marshal.AllocHGlobal((int)len);
        Marshal.Copy(bytes, 0, nativePtr, (int)len);

        return (byte*)nativePtr;
    }

    private static string Sanitize(string str, bool nullTerminate)
    {
        if (nullTerminate && str[^1] != '\0')
            return str + '\0';

        return str;
    }

    internal static byte[] GetUTF8Bytes(string str, bool nullTerminate = true)
        => Encoding.UTF8.GetBytes(Sanitize(str, nullTerminate));

    internal static byte* AllocateUTF8Ptr(string str, out uint len, bool nullTerminate = true)
        => AllocBytePtr(GetUTF8Bytes(str, nullTerminate), out len);

    internal static string GetUtf8(byte* utf8Bytes)
        => Marshal.PtrToStringUTF8((nint)utf8Bytes) ?? "";
}