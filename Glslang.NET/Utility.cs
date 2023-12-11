using System.Runtime.InteropServices;

namespace Glslang;


// Wrapper for Marshal allocation functions so I can test for memory leaks
public static class AllocUtility
{

    public static IntPtr AllocStruct<T>(T value) where T : struct
    {
        IntPtr allocation = Marshal.AllocHGlobal(Marshal.SizeOf<T>()); 
        Marshal.StructureToPtr(value, allocation, false);

        Console.WriteLine($"Allocated native struct at: {allocation}");

        return allocation;
    }


    public static void Free(IntPtr allocation)
    {
        Marshal.FreeHGlobal(allocation);
        Console.WriteLine($"Freed memory at: {allocation}");
    }


    public static IntPtr GetDelegatePtr<T>(T deleg) where T : Delegate
    {
        return Marshal.GetFunctionPointerForDelegate<T>(deleg);
    }


    public static string AutoString(IntPtr nativePtr)
    {
        return Marshal.PtrToStringAuto(nativePtr) ?? string.Empty;
    }
}