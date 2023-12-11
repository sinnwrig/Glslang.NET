using System.Runtime.InteropServices;

namespace Glslang;


// Managed wrapper for glslang_include_result_t
public struct IncludeResult
{
    [StructLayout(LayoutKind.Sequential)]
    private struct IncludeResult_Native
    {
        public IntPtr headerName;
        public IntPtr headerData;
        public nint headerLength; // Length of header data characters
    }


    public string headerName;
    public string fileContents;


    public IntPtr AllocateNativePtr()
    {
        // Allocate native pointer strings
        IncludeResult_Native resultNative = new()
        {
            headerName = Marshal.StringToHGlobalAuto(headerName),
            headerData = Marshal.StringToHGlobalAuto(fileContents),
            headerLength = fileContents.Length
        };

        // Allocate struct into native memory
        return AllocUtility.AllocStruct(resultNative); 
    }


    public static void FreeNativePtr(IntPtr nativePtr)
    {
        IncludeResult_Native result = Marshal.PtrToStructure<IncludeResult_Native>(nativePtr);

        // Free both allocated strings
        AllocUtility.Free(result.headerName);
        AllocUtility.Free(result.headerData);

        // Free the struct itself
        AllocUtility.Free(nativePtr);
    }
}


[StructLayout(LayoutKind.Sequential)]
internal struct IncludeCallbacks_Native
{
    public IntPtr includeLocalFile;
    public IntPtr includeSystemFile;
    public IntPtr freeIncludeResult;
}


[StructLayout(LayoutKind.Sequential)]
public struct IncludeCallbacks
{
    public delegate IncludeResult IncludeFile(string headerName, string includerName, int includeDepth);

    public IncludeFile includeLocalFile;
    public IncludeFile includeSystemFile;  


    private delegate IntPtr Include_Native(IntPtr ctx, IntPtr headerPtr, IntPtr includerPtr, nint includeDepth);
    private delegate int Free_Native(IntPtr ctx, IntPtr resultPtr);


    private static IntPtr IncludeLocalFile_Native(IntPtr ctx, IntPtr headerPtr, IntPtr includerPtr, nint includeDepth)
    {
        IncludeCallbacks context = Marshal.PtrToStructure<IncludeCallbacks>(ctx);
        return context.includeLocalFile(AllocUtility.AutoString(headerPtr), AllocUtility.AutoString(includerPtr), (int)includeDepth).AllocateNativePtr(); 
    }


    private static IntPtr IncludeSystemFile_Native(IntPtr ctx, IntPtr headerPtr, IntPtr includerPtr, nint includeDepth)
    {
        IncludeCallbacks context = Marshal.PtrToStructure<IncludeCallbacks>(ctx);
        return context.includeSystemFile(AllocUtility.AutoString(headerPtr), AllocUtility.AutoString(includerPtr), (int)includeDepth).AllocateNativePtr();
    }
    

    private static int FreeInclude_Native(IntPtr ctx, IntPtr resultPtr) 
    { 
        IncludeResult.FreeNativePtr(resultPtr); 
        return 0; 
    }


    internal IncludeCallbacks_Native GetCallbacks()
    {
        return new IncludeCallbacks_Native()
        {
            includeLocalFile = AllocUtility.GetDelegatePtr<Include_Native>(IncludeLocalFile_Native),
            includeSystemFile = AllocUtility.GetDelegatePtr<Include_Native>(IncludeSystemFile_Native),
            freeIncludeResult = AllocUtility.GetDelegatePtr<Free_Native>(FreeInclude_Native)
        };
    }


    internal IntPtr ContextPointer { get; private set; }

    internal IntPtr AllocateNativePtr()
    {
        if (ContextPointer != IntPtr.Zero)
            throw new InvalidOperationException("Pointer to IncludeCallbacks is already allocated");

        ContextPointer = AllocUtility.AllocStruct(this); // Allocate context struct in native memory
        return ContextPointer;
    }

    internal void FreeNativePtr()
    {
        if (ContextPointer == IntPtr.Zero)
            return;

        AllocUtility.Free(ContextPointer); // Free the context struct's native memory
        ContextPointer = IntPtr.Zero;
    }
}