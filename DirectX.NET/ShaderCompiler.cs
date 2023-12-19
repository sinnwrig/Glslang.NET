using System.Runtime.InteropServices;

namespace DXCompiler.NET;

// Equivalent of IDxcCompiler in native code
public partial class ShaderCompiler : NativeResourceHandle
{
    IncludeHandler handler;



    // Free with DeleteCompilerInstance
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr CreateCompilerInstance();

    
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl)]
    private static extern void DeleteCompilerInstance(IntPtr compiler);


    public ShaderCompiler(IncludeFileDelegate? includeDelegate = null)
    {
        handle = CreateCompilerInstance();
        handler = new IncludeHandler(includeDelegate);
    }


    // Takes in a compiler instance, a text buffer, the arguments, and an optional include handler
    // Assigns an IDxcResult instance which must be freed with FreeResult
    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private static extern int Compile(
        IntPtr compiler, 
        NativeBuffer source, 
        [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 3)]
        string[] args,
        uint argsCount, 
        IntPtr includeHandler, 
        out IntPtr results
    );


    [DllImport(Global.LibraryPath, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private static extern void PrintWideString(IntPtr strPtr);


    public CompilationOutput Compile(string sourceCode, string[] args)
    {
        NativeBuffer srcBuffer = NativeBuffer.CreateFromString(sourceCode);

        int hres = Compile(handle, srcBuffer, args, (uint)args.Length, handler.GetHandle(), out IntPtr resultsPtr);

        srcBuffer.FreeBuffer();

        return new CompilationOutput(resultsPtr);
    }


    public CompilationOutput Compile(string sourceCode, CompilerOptions options)
    {
        // Throw an InvalidProfileException if neccesary
        options.profile.Validate();

        return Compile(sourceCode, options.GetArgumentsArray());
    }


    protected override bool ReleaseHandle()
    {
        Console.WriteLine("Freeing compiler");
        DeleteCompilerInstance(handle);
        return true;
    }
}