using System.Text;
using System.Runtime.InteropServices;

namespace DXCompiler.NET;


public static class StringUtility
{
    public static byte[] GetUTF8Bytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str + '\0');
    }


    // Because FUCK UTF-16
    public static IntPtr GetUtf8Ptr(string str, out uint length)
    {
        byte[] bytes = GetUTF8Bytes(str);
        IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
        Marshal.Copy(bytes, 0, ptr, bytes.Length);
        length = (uint)bytes.Length;

        return ptr;
    }
}