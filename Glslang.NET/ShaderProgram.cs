using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


public class ShaderProgram
{
    IntPtr programPtr;



    internal ShaderProgram()
    {
        programPtr = GlslangNative.CreateProgram();
    }


    internal void Release()
    {
        GlslangNative.DeleteProgram(programPtr);
    }



    public void AddShader(Shader shader)
    {
        GlslangNative.AddShaderToProgram(programPtr, shader.shaderPtr);
    }
    

    public bool Link(MessageType messages)
    {
        return GlslangNative.LinkProgram(programPtr, messages) == 1;
    }
    

    public void AddSourceText(ShaderStage stage, string text)
    {
        IntPtr textPtr = NativeStringUtility.AllocUTF8Ptr(text, out uint length, false);
        GlslangNative.AddProgramSourceText(programPtr, stage, textPtr, (nuint)length);
        Marshal.FreeHGlobal(textPtr);
    }
    

    public void SetSourceFile(ShaderStage stage, string file)
    {
        IntPtr filePtr = NativeStringUtility.AllocUTF8Ptr(file, out _, true);
        GlslangNative.SetProgramSourceFile(programPtr, stage, filePtr);
        Marshal.FreeHGlobal(filePtr);
    }


    public bool MapIO()
    {
        return GlslangNative.MapProgramIO(programPtr) == 1;
    }


    // SPIR-V generation
    
    private bool generatedSPIRV = false;

    public bool GenerateSPIRV(out byte[] SPIRVWords, ShaderStage stage, SPIRVOptions? options = null)
    {
        if (options != null)
        {
            IntPtr optionsPtr = Marshal.AllocHGlobal(Marshal.SizeOf<SPIRVOptions>());
            Marshal.StructureToPtr(options.Value, optionsPtr, false);
            GlslangNative.GenerateProgramSPIRVWithOptiosn(programPtr, stage, optionsPtr);
            Marshal.FreeHGlobal(optionsPtr);
            GlslangNative.GenerateProgramSPIRV(programPtr, stage);
        }
        else
        {
            GlslangNative.GenerateProgramSPIRV(programPtr, stage);
        }

        nuint size = GlslangNative.GetProgramSPIRVSize(programPtr);
        SPIRVWords = new byte[(int)size * sizeof(uint)];
        GlslangNative.GetProgramSPIRVBuffer(programPtr, SPIRVWords);

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

        IntPtr SPIRVMessagesPtr = GlslangNative.GetProgramSPIRVMessages(programPtr);
        return DeallocString(SPIRVMessagesPtr);
    }


    public string GetInfoLog()
    {
        IntPtr infoLogPtr = GlslangNative.GetProgramInfoDebugLog(programPtr);
        return DeallocString(infoLogPtr);
    }


    public string GetDebugLog()
    {
        IntPtr debugLogPtr = GlslangNative.GetProgramInfoLog(programPtr);
        return DeallocString(debugLogPtr);
    }


    private static string DeallocString(IntPtr stringPtr)
    {
        string managedString = Marshal.PtrToStringUTF8(stringPtr) ?? string.Empty;
        return managedString;
    }
}