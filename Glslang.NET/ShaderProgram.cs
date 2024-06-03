using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


public class ShaderProgram
{
    IntPtr programPtr;



    internal ShaderProgram()
    {
        programPtr = GlslangNative.glslang_program_create();
    }


    internal void Release()
    {
        GlslangNative.glslang_program_delete(programPtr);
    }



    public void AddShader(Shader shader)
    {
        GlslangNative.glslang_program_add_shader(programPtr, shader.shaderPtr);
    }
    

    public bool Link(MessageType messages)
    {
        return GlslangNative.glslang_program_link(programPtr, messages) == 1;
    }
    

    public void AddSourceText(ShaderStage stage, string text)
    {
        IntPtr textPtr = NativeStringUtility.AllocUTF8Ptr(text, out uint length, false);
        GlslangNative.glslang_program_add_source_text(programPtr, stage, textPtr, (nuint)length);
        Marshal.FreeHGlobal(textPtr);
    }
    

    public void SetSourceFile(ShaderStage stage, string file)
    {
        IntPtr filePtr = NativeStringUtility.AllocUTF8Ptr(file, out _, true);
        GlslangNative.glslang_program_set_source_file(programPtr, stage, filePtr);
        Marshal.FreeHGlobal(filePtr);
    }


    public bool MapIO()
    {
        return GlslangNative.glslang_program_map_io(programPtr) == 1;
    }


    // SPIR-V generation
    
    private bool generatedSPIRV = false;

    public bool GenerateSPIRV(out byte[] SPIRVWords, ShaderStage stage, SPIRVOptions? options = null)
    {
        if (options != null)
        {
            IntPtr optionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<SPIRVOptions>());
            Marshal.StructureToPtr(options.Value, optionsPtr, false);
            GlslangNative.glslang_program_SPIRV_generate_with_options(programPtr, stage, optionsPtr);
            Marshal.FreeHGlobal(optionsPtr);
            GlslangNative.glslang_program_SPIRV_generate(programPtr, stage);
        }
        else
        {
            GlslangNative.glslang_program_SPIRV_generate(programPtr, stage);
        }

        nuint size = GlslangNative.glslang_program_SPIRV_get_size(programPtr);
        SPIRVWords = new byte[(int)size * sizeof(uint)];
        GlslangNative.glslang_program_SPIRV_get(programPtr, SPIRVWords);

        generatedSPIRV = true;

        return size != 0;
    }


    public string GetSPIRVMessages()
    {
        if (!generatedSPIRV)
        {
            throw new InvalidOperationException(
                "ShaderProgram.GetSPIRVMessages() called before Shader.GenerateSPIRV()." + 
                "This is not allowed. Please ensure GenerateSPIRV() is called before GetSpirvMessages()."
            );
        }

        IntPtr SPIRVMessagesPtr = GlslangNative.glslang_program_SPIRV_get_messages(programPtr);
        return DeallocString(SPIRVMessagesPtr);
    }


    public string GetInfoLog()
    {
        IntPtr infoLogPtr = GlslangNative.glslang_program_get_info_debug_log(programPtr);
        return DeallocString(infoLogPtr);
    }


    public string GetDebugLog()
    {
        IntPtr debugLogPtr = GlslangNative.glslang_program_get_info_log(programPtr);
        return DeallocString(debugLogPtr);
    }


    private static string DeallocString(IntPtr stringPtr)
    {
        string managedString = Marshal.PtrToStringUTF8(stringPtr) ?? string.Empty;
        return managedString;
    }
}