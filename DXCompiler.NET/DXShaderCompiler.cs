using System.Runtime.InteropServices;

namespace DXCompiler.NET;


public delegate string FileIncludeHandler(string includeName); 

public class DXShaderCompiler : NativeResourceHandle
{
    internal delegate IntPtr DxcIncludeFunction(IntPtr context, IntPtr headerNameUtf8); 

    internal delegate int DxcFreeIncludeFunction(IntPtr context, IntPtr includeResult);


    [StructLayout(LayoutKind.Sequential)]
    internal struct NativeDxcIncludeCallbacks 
    {
        internal IntPtr includeContext;
        internal IntPtr includeFunction;
        internal IntPtr freeFunction;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct DxcIncludeCallbackContext
    {
        internal FileIncludeHandler? includeHandler;
    } 


    [StructLayout(LayoutKind.Sequential)]
    internal struct DxcIncludeResult
    {
        internal IntPtr headerData;
        internal nuint headerLength;
    }


    private delegate IntPtr NativeIncludeHandler(IntPtr ctx, IntPtr headerUtf8);
    private static IntPtr IncludeFunction(IntPtr nativeContext, IntPtr headerNameUtf8)
    {
        DxcIncludeCallbackContext context = Marshal.PtrToStructure<DxcIncludeCallbackContext>(nativeContext);

        string? headerName = Marshal.PtrToStringUTF8(headerNameUtf8);

        if (context.includeHandler != null && headerName != null)
        {
            string includeFile = context.includeHandler.Invoke(headerName);

            DxcIncludeResult includeResult = new()
            {
                headerData = NativeStringUtility.GetUTF8Ptr(includeFile, out uint len, false),
                headerLength = len
            };

            return AllocStruct(includeResult);
        }

        return IntPtr.Zero;
    }


    private delegate int NativeFreeHandler(IntPtr ctx, IntPtr resultStructure);    
    private static int FreeFunction(IntPtr nativeContext, IntPtr includeResult)
    {
        if (includeResult == IntPtr.Zero)
            return 0;

        DxcIncludeResult result = Marshal.PtrToStructure<DxcIncludeResult>(includeResult);

        Marshal.FreeHGlobal(result.headerData);
        Marshal.FreeHGlobal(includeResult);

        return 0;
    }


    private static IntPtr AllocStruct<T>(T structure) where T : struct
    {
        IntPtr memPtr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        Marshal.StructureToPtr(structure, memPtr, false);
        return memPtr;
    }


    public DXShaderCompiler()
    {
        handle = DXCNative.machDxcInit();
    }


    public CompilationResult Compile(string code, CompilerOptions compilationOptions, FileIncludeHandler? includeHandler = null)
    {
        byte[] codeUtf8 = NativeStringUtility.GetUTF8Bytes(code, false);

        string[] compilerArgs = compilationOptions.GetArgumentsArray();
        IntPtr[] argsUtf8 = new IntPtr[compilerArgs.Length];

        for (int i = 0; i < argsUtf8.Length; i++)
            argsUtf8[i] = NativeStringUtility.GetUTF8Ptr(compilerArgs[i], out _, true);

        DxcIncludeCallbackContext context = new()
        {
            includeHandler = includeHandler
        };

        IntPtr contextPtr = AllocStruct(context);

        NativeDxcIncludeCallbacks callbacks = new()
        {
            includeContext = contextPtr,
            includeFunction = Marshal.GetFunctionPointerForDelegate<NativeIncludeHandler>(IncludeFunction),
            freeFunction = Marshal.GetFunctionPointerForDelegate<NativeFreeHandler>(FreeFunction)
        };

        IntPtr callbacksPtr = AllocStruct(callbacks);

        GCHandle codeHandle = GCHandle.Alloc(codeUtf8, GCHandleType.Pinned);
        GCHandle argsHandle = GCHandle.Alloc(argsUtf8, GCHandleType.Pinned);

        CompilationResult result = GetResult(DXCNative.machDxcCompile(handle, 
            codeHandle.AddrOfPinnedObject(), (uint)codeUtf8.Length, 
            argsHandle.AddrOfPinnedObject(), (uint)argsUtf8.Length, 
            includeHandler != null ? callbacksPtr : IntPtr.Zero));

        codeHandle.Free();
        argsHandle.Free();        

        Marshal.FreeHGlobal(contextPtr);
        Marshal.FreeHGlobal(callbacksPtr);

        for (int i = 0; i < argsUtf8.Length; i++)
            Marshal.FreeHGlobal(argsUtf8[i]);

        return result;
    }


    static CompilationResult GetResult(IntPtr resultPtr)
    {
        IntPtr errorPtr = DXCNative.machDxcCompileResultGetError(resultPtr);

        byte[] objectBytes = Array.Empty<byte>();
        string? compilationErrors = null;

        if (errorPtr != IntPtr.Zero)
        {
            IntPtr errorStringPtr = DXCNative.machDxcCompileErrorGetString(errorPtr);
            nuint errorStringLen = DXCNative.machDxcCompileErrorGetStringLength(errorPtr);

            compilationErrors = Marshal.PtrToStringUTF8(errorStringPtr, (int)errorStringLen);
            DXCNative.machDxcCompileErrorDeinit(errorPtr);
        }
        else
        {
            IntPtr objectPtr = DXCNative.machDxcCompileResultGetObject(resultPtr);
            IntPtr objectBytesPtr = DXCNative.machDxcCompileObjectGetBytes(objectPtr);
            nuint objectBytesLen = DXCNative.machDxcCompileObjectGetBytesLength(objectPtr);

            objectBytes = new byte[(int)objectBytesLen];
            Marshal.Copy(objectBytesPtr, objectBytes, 0, (int)objectBytesLen);

            DXCNative.machDxcCompileObjectDeinit(objectPtr);
        }

        DXCNative.machDxcCompileResultDeinit(resultPtr);

        return new CompilationResult()
        {
            objectBytes = objectBytes,
            compilationErrors = compilationErrors
        };
    }


    protected override void ReleaseHandle()
    {
        DXCNative.machDxcDeinit(handle);
    }
}