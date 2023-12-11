using System.Runtime.InteropServices;

namespace Glslang;



[StructLayout(LayoutKind.Sequential)]
internal struct ShaderInput_Native
{
    public ShaderSource language;
    public ShaderStage stage;
    public ShaderClient client;
    public ClientVersion clientVersion;
    public TargetLanguage targetLanguage;
    public TargetLanguageVersion targetLanguageVersion;
    public string code;
    public int defaultVersion;
    public ShaderProfile defaultProfile;
    public int forceDefaultVersionAndProfile;
    public int forwardCompatible;
    public Messages messages;
    public IntPtr resource;
    public IncludeCallbacks_Native callbacks;
    public IntPtr callbacksCtx;
}


public struct ShaderInput
{
    public ShaderSource language;
    public ShaderStage stage;
    public ShaderClient client;
    public ClientVersion clientVersion;
    public TargetLanguage targetLanguage;
    public TargetLanguageVersion targetLanguageVersion;
    public string code;
    public int defaultVersion;
    public ShaderProfile defaultProfile;
    public int forceDefaultVersionAndProfile;
    public int forwardCompatible;
    public Messages messages;
    public ShaderResource resource;
    public IncludeCallbacks includeCallbacks;


    internal IntPtr NativePointer { get; private set; }

    internal IntPtr AllocateNativePtr()
    {
        if (NativePointer != IntPtr.Zero)
            throw new InvalidOperationException("Pointer to ShaderInput_Native is already allocated");

        ShaderInput_Native nativeStruct = new()
        {
            language = language,
            stage = stage,
            client = client,
            clientVersion = clientVersion,
            targetLanguage = targetLanguage,
            targetLanguageVersion = targetLanguageVersion,
            code = code,
            defaultVersion = defaultVersion,
            defaultProfile = defaultProfile,
            forceDefaultVersionAndProfile = forceDefaultVersionAndProfile,
            forwardCompatible = forwardCompatible,
            messages = messages,
            resource = AllocUtility.AllocStruct(resource), // Allocate resource memory
            callbacks = includeCallbacks.GetCallbacks(),
            callbacksCtx = includeCallbacks.AllocateNativePtr()
        };

        // Allocate struct once everything is set
        NativePointer = AllocUtility.AllocStruct(nativeStruct);
        return NativePointer;
    }


    internal void FreeNativePtr()
    {
        if (NativePointer == IntPtr.Zero)
            return;

        ShaderInput_Native nativeStruct = Marshal.PtrToStructure<ShaderInput_Native>(NativePointer);

        AllocUtility.Free(nativeStruct.resource); // Free allocated resource
        includeCallbacks.FreeNativePtr(); // Free callback resource(s)
        AllocUtility.Free(NativePointer); // Free struct itself
        
        NativePointer = IntPtr.Zero;
    }
}