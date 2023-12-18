using System.Text;
using System.Runtime.InteropServices;

namespace DirectX;


public static class StringUtility
{
    public static byte[] GetUTF8Bytes(this string str)
    {
        return Encoding.UTF8.GetBytes(str + '\0');
    }


    public static byte[] GetUTF16Bytes(this string str)
    {
        return Encoding.Unicode.GetBytes(str + '\0');
    }


    public static IntPtr GetUtf8Ptr(string str, out uint length)
    {
        byte[] bytes = GetUTF8Bytes(str);
        IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
        Marshal.Copy(bytes, 0, ptr, bytes.Length);
        length = (uint)bytes.Length;

        return ptr;
    }


    public static IntPtr GetUtf16Ptr(string str, out uint length)
    {
        byte[] bytes = GetUTF16Bytes(str);
        IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
        Marshal.Copy(bytes, 0, ptr, bytes.Length);
        length = (uint)bytes.Length;

        return ptr;
    }
}