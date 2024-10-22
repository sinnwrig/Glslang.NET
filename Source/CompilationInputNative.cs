using System.Runtime.InteropServices;

namespace Glslang.NET;


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal unsafe struct NativeInput
{
    public SourceType language;
    public ShaderStage stage;
    public ClientType client;
    public TargetClientVersion client_version;
    public TargetLanguage target_language;
    public TargetLanguageVersion target_language_version;

    public byte* code;
    public byte* entrypoint;
    public byte* source_entrypoint;

    public bool invert_y;
    public int default_version;
    public ShaderProfile default_profile;
    public int force_default_version_and_profile;
    public int forward_compatible;
    public MessageType messages;
    public ResourceLimits* resource;
    internal NativeIncludeCallbacks callbacks;
    public void* callbacks_ctx;


    public static NativeInput* Allocate(CompilationInput input)
    {
        NativeInput* nativeInput = GlslangNative.Allocate<NativeInput>();

        nativeInput->language = input.language;
        nativeInput->stage = input.stage;
        nativeInput->client = input.client;
        nativeInput->client_version = input.clientVersion;
        nativeInput->target_language = input.targetLanguage;
        nativeInput->target_language_version = input.targetLanguageVersion;

        nativeInput->code = NativeUtil.AllocUTF8Ptr(input.code ?? "", out _, true);
        nativeInput->entrypoint = NativeUtil.AllocUTF8Ptr(input.entrypoint ?? "main", out _, true);
        nativeInput->source_entrypoint = NativeUtil.AllocUTF8Ptr(input.sourceEntrypoint ?? "main", out _, true);

        nativeInput->invert_y = input.invertY;
        nativeInput->default_version = input.defaultVersion;
        nativeInput->default_profile = input.defaultProfile;
        nativeInput->force_default_version_and_profile = input.forceDefaultVersionAndProfile ? 1 : 0;
        nativeInput->forward_compatible = input.forwardCompatible ? 1 : 0;
        nativeInput->messages = input.messages ?? MessageType.Default;

        // Allocate resource limits
        ResourceLimits resource = ResourceLimits.DefaultResource;
        nativeInput->resource = GlslangNative.Allocate<ResourceLimits>();
        resource.Set(nativeInput->resource);

        nativeInput->callbacks.include_local = IncludeLocalPtr;
        nativeInput->callbacks.include_system = IncludeSystemPtr;
        nativeInput->callbacks.free_include_result = FreeIncludePtr;
        nativeInput->callbacks_ctx = (void*)(input.fileIncluder != null ? Marshal.GetFunctionPointerForDelegate(input.fileIncluder) : IntPtr.Zero);

        return nativeInput;
    }


    internal static void Free(NativeInput* inputPtr)
    {
        GlslangNative.Free(inputPtr->code);
        GlslangNative.Free(inputPtr->entrypoint);
        GlslangNative.Free(inputPtr->source_entrypoint);

        GlslangNative.Free(inputPtr->resource);

        GlslangNative.Free(inputPtr);
    }


    private static readonly void* IncludeLocalPtr = (void*)Marshal.GetFunctionPointerForDelegate(IncludeLocal);
    internal static NativeGLSLIncludeResult* IncludeLocal(void* context, byte* headerName, byte* includerName, nuint includeDepth)
    {
        return Include(context, headerName, includerName, includeDepth, false);
    }


    private static readonly void* IncludeSystemPtr = (void*)Marshal.GetFunctionPointerForDelegate(IncludeSystem);
    internal static NativeGLSLIncludeResult* IncludeSystem(void* context, byte* headerName, byte* includerName, nuint includeDepth)
    {
        return Include(context, headerName, includerName, includeDepth, true);
    }


    private static NativeGLSLIncludeResult* Include(void* context, byte* headerName, byte* includerName, nuint includeDepth, bool isSystemFile)
    {
        FileIncluder includer = context != null ? Marshal.GetDelegateForFunctionPointer<FileIncluder>((nint)context) : DefaultIncluder;

        IncludeResult result = includer.Invoke(
            Marshal.PtrToStringUTF8((nint)headerName) ?? "",
            Marshal.PtrToStringUTF8((nint)includerName) ?? "",
            (uint)includeDepth,
            isSystemFile);

        NativeGLSLIncludeResult* resultPtr = GlslangNative.Allocate<NativeGLSLIncludeResult>();
        resultPtr->header_name = NativeUtil.AllocUTF8Ptr(result.headerName, out _, true);
        resultPtr->header_data = NativeUtil.AllocUTF8Ptr(result.headerData, out uint len, false);
        resultPtr->header_length = len;

        return resultPtr;
    }


    private static readonly void* FreeIncludePtr = (void*)Marshal.GetFunctionPointerForDelegate(FreeInclude);
    internal static int FreeInclude(void* context, NativeGLSLIncludeResult* result)
    {
        GlslangNative.Free(result->header_name);
        GlslangNative.Free(result->header_data);

        GlslangNative.Free(result);

        return 0;
    }


    private static IncludeResult DefaultIncluder(string header, string includer, uint depth, bool isSystemFile)
    {
        IncludeResult result = new()
        {
            headerName = header,
            headerData = ""
        };

        return result;
    }
}


[StructLayout(LayoutKind.Sequential)]
internal unsafe struct NativeGLSLIncludeResult
{
    /* Header file name or NULL if inclusion failed */
    public byte* header_name;

    /* Header contents or NULL */
    public byte* header_data;
    public nuint header_length;
}


[StructLayout(LayoutKind.Sequential)]
internal unsafe struct NativeIncludeCallbacks
{
    public void* include_system;
    public void* include_local;
    public void* free_include_result;
}
