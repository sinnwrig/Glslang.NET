using System.Runtime.InteropServices;
using System.Text;


namespace DirectX;


public class DxcResult : SafeHandle
{
    public DxcResult(IntPtr nativePointer) : base(IntPtr.Zero, true)
    {
        handle = nativePointer;
    }


    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern void FreeResult(IntPtr result);

    // Allocates an output and name buffer based on the result output kind. These buffers must be released with FreeBuffer
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern bool GetResultOutput(IntPtr result, OutKind kind, out DxcBuffer output, out DxcBuffer shaderName);

    // Get the status of the program
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern int GetStatus(IntPtr result);



    public bool GetByteOutput(OutKind kind, out byte[] byteOutput, out string shaderName)
    {
        bool success = GetResultOutput(handle, kind, out DxcBuffer output, out DxcBuffer shaderBuf);

        output.TryGetBytes(out byteOutput);
        output.FreeBuffer();
        shaderBuf.TryGetString(out shaderName);
        shaderBuf.FreeBuffer();

        return success;
    }


    public bool GetTextOutput(OutKind kind, out string textOutput, out string shaderName)
    {
        Console.WriteLine("Getting text output");

        if (!GetResultOutput(handle, kind, out DxcBuffer output, out DxcBuffer shaderBuf))
        {
            textOutput = string.Empty;
            shaderName = string.Empty;
            return false;
        }

        output.TryGetString(out textOutput);
        output.FreeBuffer();

        shaderBuf.TryGetString(out shaderName);
        shaderBuf.FreeBuffer();
        Console.WriteLine("Parsed");

        return true;
    }


    public Exception? GetStatus()
    {
        int status = GetStatus(handle);
        return Marshal.GetExceptionForHR(status);
    }



    public override bool IsInvalid => handle == IntPtr.Zero || handle == new nint(-1);
    protected override bool ReleaseHandle()
    {
        FreeResult(handle);

        return true;
    }
}