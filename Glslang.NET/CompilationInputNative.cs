using System.Runtime.InteropServices;

namespace Glslang.NET;


[StructLayout(LayoutKind.Sequential)]
internal struct CompilationInputNative 
{
    public SourceType language;
    public ShaderStage stage;
    public ClientType client;
    public TargetClientVersion clientVersion;
    public TargetLanguage targetLanguage;
    public TargetLanguageVersion targetLanguageVersion;
    public IntPtr code;
    public IntPtr entrypoint;
    public IntPtr sourceEntrypoint;
    public bool invertY;
    public int defaultVersion;
    public ShaderProfile defaultProfile;
    public int forceDefaultVersionAndProfile;
    public int forwardCompatible;
    public MessageType messages;
    public IntPtr resource;
    public IncludeCallbacksNative callbacks;
    public IntPtr callbacksCtx;


    internal static IntPtr GetPtrForCompilationInput(CompilationInput input)
    {
        CompilationInputNative nativeInput;
        nativeInput.language = input.language;
        nativeInput.stage = input.stage;
        nativeInput.client = input.client;
        nativeInput.clientVersion = input.clientVersion;
        nativeInput.targetLanguage = input.targetLanguage;
        nativeInput.targetLanguageVersion = input.targetLanguageVersion;

        nativeInput.code = NativeStringUtility.AllocUTF8Ptr(input.code, out _, true);
        nativeInput.entrypoint = NativeStringUtility.AllocUTF8Ptr(input.entrypoint ?? "main", out _, true);
        nativeInput.sourceEntrypoint = NativeStringUtility.AllocUTF8Ptr(input.sourceEntrypoint ?? "main", out _, true);

        nativeInput.invertY = input.invertY;
        nativeInput.defaultVersion = input.defaultVersion;
        nativeInput.defaultProfile = input.defaultProfile;
        nativeInput.forceDefaultVersionAndProfile = input.forceDefaultVersionAndProfile ? 1 : 0;
        nativeInput.forwardCompatible = input.forwardCompatible ? 1 : 0;
        nativeInput.messages = input.messages ?? MessageType.Default;

        // Allocate resource limits
        nativeInput.resource = Marshal.AllocHGlobal(Marshal.SizeOf<ResourceLimits>());
        Marshal.StructureToPtr(input.resourceLimits ?? ResourceLimits.DefaultResource, nativeInput.resource, false);

        nativeInput.callbacks.includeLocal = IncludeCallbacksNative.LocalFuncPtr;
        nativeInput.callbacks.includeSystem = IncludeCallbacksNative.SystemFuncPtr;
        nativeInput.callbacks.freeIncludeResult = IncludeCallbacksNative.FreeFuncPtr;
        nativeInput.callbacksCtx = input.fileIncluder != null ? Marshal.GetFunctionPointerForDelegate(input.fileIncluder) : IntPtr.Zero;

        // Allocate compiler input
        IntPtr inputPtr = Marshal.AllocHGlobal(Marshal.SizeOf<CompilationInputNative>());
        Marshal.StructureToPtr(nativeInput, inputPtr, false);
        return inputPtr;
    }


    internal static void ReleasePtrForCompilationInput(IntPtr inputPtr)
    {   
        CompilationInputNative nativeInput = Marshal.PtrToStructure<CompilationInputNative>(inputPtr);

        Marshal.FreeHGlobal(nativeInput.code);
        Marshal.FreeHGlobal(nativeInput.entrypoint);
        Marshal.FreeHGlobal(nativeInput.sourceEntrypoint);

        Marshal.FreeHGlobal(nativeInput.resource);

        Marshal.FreeHGlobal(inputPtr);
    }
}


[StructLayout(LayoutKind.Sequential)]
internal struct IncludeCallbacksNative 
{
    internal IntPtr includeSystem;
    internal IntPtr includeLocal;
    internal IntPtr freeIncludeResult;


    internal static readonly IncludeCallbacksNative DefaultCallbacks = new() 
    {
        includeSystem = SystemFuncPtr,
        includeLocal = LocalFuncPtr,
        freeIncludeResult = FreeFuncPtr,
    };


    private delegate IntPtr IncludeFunctionNative(IntPtr context, IntPtr headerNameUTF8, IntPtr includerNameUTF8, nuint includeDepth);
    private delegate int FreeFunctionNative(IntPtr context, IntPtr includeResult);


    private static IntPtr NativeIncluder(IntPtr context, IntPtr headerPtr, IntPtr includerPtr, nuint depth, bool system)
    {
        string headerStr = Marshal.PtrToStringUTF8(headerPtr) ?? string.Empty;
        string includerStr = Marshal.PtrToStringUTF8(includerPtr) ?? string.Empty; 

        if (context == IntPtr.Zero)
            return IncludeResultNative.CreateNative(string.Empty, string.Empty);

        FileIncluder includer = Marshal.GetDelegateForFunctionPointer<FileIncluder>(context);
        IncludeResult result = includer.Invoke(headerStr, includerStr, (uint)depth, system);
        
        return IncludeResultNative.CreateNative(result.headerName, result.headerData);
    }


    // Redirect native includer to local file delegate
    internal static readonly IntPtr LocalFuncPtr = Marshal.GetFunctionPointerForDelegate<IncludeFunctionNative>(IncludeFunctionLocal);

    private static IntPtr IncludeFunctionLocal(IntPtr context, IntPtr headerPtr, IntPtr includerPtr, nuint depth) => 
        NativeIncluder(context, headerPtr, includerPtr, depth, false);


    // Redirect native includer to system file delegate
    internal static readonly IntPtr SystemFuncPtr = Marshal.GetFunctionPointerForDelegate<IncludeFunctionNative>(IncludeFunctionSystem);

    private static IntPtr IncludeFunctionSystem(IntPtr context, IntPtr headerPtr, IntPtr includerPtr, nuint depth) => 
        NativeIncluder(context, headerPtr, includerPtr, depth, true);


    // Free allocated include results
    internal static readonly IntPtr FreeFuncPtr = Marshal.GetFunctionPointerForDelegate<FreeFunctionNative>(FreeFunction);
    private static int FreeFunction(IntPtr context, IntPtr result)
    {
        if (result == IntPtr.Zero)
            return 1;

        IncludeResultNative.FreeNative(result);

        return 0;
    }
}


[StructLayout(LayoutKind.Sequential)]
internal struct IncludeResultNative 
{
    private IntPtr headerName;
    private IntPtr headerData;
    private nuint headerLength;


    internal static IntPtr CreateNative(string headerName, string headerData)
    {
        IncludeResultNative resultNative;

        if (!string.IsNullOrWhiteSpace(headerName) && !string.IsNullOrWhiteSpace(headerData))
        {
            resultNative.headerName = NativeStringUtility.AllocUTF8Ptr(headerName, out _, true);
            resultNative.headerData = NativeStringUtility.AllocUTF8Ptr(headerData, out uint headerLen, false);
            resultNative.headerLength = headerLen;
        }
        else
        {
            resultNative.headerName = IntPtr.Zero;
            resultNative.headerData = IntPtr.Zero;
            resultNative.headerLength = 0;
        }

        IntPtr resultPtr = Marshal.AllocHGlobal(Marshal.SizeOf<IncludeResultNative>());
        Marshal.StructureToPtr(resultNative, resultPtr, false);

        return resultPtr;
    }


    internal static void FreeNative(IntPtr resultPtr)
    {
        IncludeResultNative resultNative = Marshal.PtrToStructure<IncludeResultNative>(resultPtr);

        if (resultNative.headerName != IntPtr.Zero)
            Marshal.FreeHGlobal(resultNative.headerName); 
        
        if (resultNative.headerData != IntPtr.Zero)
            Marshal.FreeHGlobal(resultNative.headerData); 

        Marshal.FreeHGlobal(resultPtr);
    }
}
