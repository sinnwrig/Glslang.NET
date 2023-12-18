using System.Runtime.InteropServices;
using System.Text;

namespace DirectX;


// Callback for local file inclusion 
public delegate string IncludeFileDelegate(string filename);


public class DxcIncludeHandler : SafeHandle
{
    [StructLayout(LayoutKind.Sequential)]
    private struct IncludeContext
    {
        public IncludeFileDelegate includeFile;
    }


    private delegate DxcBuffer NativeHandlerDelegate(IntPtr context, IntPtr filenameutf8);
    private static DxcBuffer IncludeHandlerNative(IntPtr contextPtr, IntPtr filenameutf8)
    {
        Console.WriteLine("Native include handler called");

        string? filename = Marshal.PtrToStringUTF8(filenameutf8);

        if (filename == null)
            return DxcBuffer.CreateFromString("");

        IncludeContext context = Marshal.PtrToStructure<IncludeContext>(contextPtr);

        string file = context.includeFile(filename);

        return DxcBuffer.CreateFromString(file);
    }

    private static IntPtr IncludeHandlerNativePtr => Marshal.GetFunctionPointerForDelegate<NativeHandlerDelegate>(IncludeHandlerNative);


    // Keep a reference so the GC does not deallocate the delegate's internal context
    IncludeFileDelegate delegateReference;
    IntPtr contextPtr;


    // Free with DeleteIncludeHandler
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr CreateIncludeHandler(IntPtr ctx, IntPtr delegatePtr);


    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern void DeleteIncludeHandler(IntPtr handler);


    public DxcIncludeHandler(IncludeFileDelegate? includeDelegate = null) : base(IntPtr.Zero, true)
    {
        delegateReference = includeDelegate ?? DefaultIncludeHandler;

        IncludeContext context = new IncludeContext()
        {
            includeFile = delegateReference
        };

        contextPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IncludeContext>());
        Marshal.StructureToPtr(context, contextPtr, false);

        handle = CreateIncludeHandler(contextPtr, IncludeHandlerNativePtr);
    }


    private string DefaultIncludeHandler(string filename)
    {   
        try
        {
            Console.WriteLine($"Including file {filename}");
            using FileStream fs = File.Open(filename, FileMode.Open);
            using StreamReader reader = new StreamReader(fs);
            return reader.ReadToEnd();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"File {filename} not found");
        }

        return "";
    }


    public override bool IsInvalid => handle == IntPtr.Zero || handle == new nint(-1);
    protected override bool ReleaseHandle()
    {
        Marshal.FreeHGlobal(contextPtr);
        DeleteIncludeHandler(handle);

        return true;
    }
}