using System.Runtime.InteropServices;
using System.Text;

namespace DirectX;

public partial class DxcCompiler : SafeHandle
{
    DxcIncludeHandler handler;



    // Free with DeleteCompilerInstance
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr CreateCompilerInstance();

    
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern void DeleteCompilerInstance(IntPtr compiler);


    public DxcCompiler(IncludeFileDelegate? includeDelegate = null) : base(IntPtr.Zero, true)
    {
        handle = CreateCompilerInstance();
        handler = new DxcIncludeHandler(includeDelegate);
    }


    // Takes in a compiler instance, a text buffer, the arguments, and an optional include handler
    // Assigns an IDxcResult instance which must be freed with FreeResult
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private static extern int Compile(
        IntPtr compiler, 
        DxcBuffer source, 
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 3)]
        string[] args,
        uint argsCount, 
        IntPtr includeHandler, 
        out IntPtr results
    );


    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private static extern void PrintWideString(IntPtr strPtr);


    public DxcResult Compile(string sourceCode, string[] args)
    {
        DxcBuffer srcBuffer = DxcBuffer.CreateFromString(sourceCode);

        int hres = Compile(handle, srcBuffer, args, (uint)args.Length, handler.DangerousGetHandle(), out IntPtr resultsPtr);

        srcBuffer.FreeBuffer();

        return new DxcResult(resultsPtr);
    }



    public override bool IsInvalid => handle == IntPtr.Zero || handle == new nint(-1);
    protected override bool ReleaseHandle()
    {
        DeleteCompilerInstance(handle);
        return true;
    }
}