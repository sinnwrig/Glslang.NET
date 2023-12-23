using System.Runtime.InteropServices;

namespace DXCompiler.NET;


// Equivalent of IDxcResult in native code
public class CompilationOutput : NativeResourceHandle
{
    public CompilationOutput(IntPtr nativePointer)
    {
        handle = nativePointer;
    }


    [DllImport(Global.Library, CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeResult(IntPtr result);

    // Allocates an output and name buffer based on the result output kind. These buffers must be released with FreeBuffer
    [DllImport(Global.Library, CallingConvention = CallingConvention.Cdecl)]
    private static extern int GetResultOutput(IntPtr result, OutKind kind, out NativeBuffer output, out NativeBuffer shaderName);

    // Get the status of the program
    [DllImport(Global.Library, CallingConvention = CallingConvention.Cdecl)]
    private static extern int GetStatus(IntPtr result);



    public void GetByteOutput(OutKind kind, out byte[] byteOutput, out string shaderName)
    {
        int hres = GetResultOutput(handle, kind, out NativeBuffer output, out NativeBuffer shaderBuf);

        Marshal.ThrowExceptionForHR(hres);

        output.TryGetBytes(out byteOutput);
        output.FreeBuffer();
        shaderBuf.TryGetString(out shaderName);
        shaderBuf.FreeBuffer();
    }


    public void GetTextOutput(OutKind kind, out string textOutput, out string shaderName)
    {
        int hres = GetResultOutput(handle, kind, out NativeBuffer output, out NativeBuffer shaderBuf);

        Marshal.ThrowExceptionForHR(hres);

        output.TryGetString(out textOutput);
        output.FreeBuffer();

        shaderBuf.TryGetString(out shaderName);
        shaderBuf.FreeBuffer();
    }


    public Exception? GetStatus()
    {
        int status = GetStatus(handle);
        return Marshal.GetExceptionForHR(status);
    }


    protected override bool ReleaseHandle()
    {
        Console.WriteLine("Freeing");
        FreeResult(handle);

        return true;
    }
}