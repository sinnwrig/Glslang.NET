using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The shader program used to link and generate shader code.
/// </summary>
/// <remarks>
/// Ensure this class is only created through `CompilationContext.CreateProgram`.
/// </remarks>
public class Program
{
    readonly IntPtr programPtr;


    internal Program()
    {
        programPtr = GlslangNative.CreateProgram();
    }


    internal void Release()
    {
        GlslangNative.DeleteProgram(programPtr);
    }


    /// <summary>
    /// Add a shader to the program. 
    /// </summary>
    /// <param name="shader">The shader to add.</param>
    public void AddShader(Shader shader)
    {
        GlslangNative.AddShaderToProgram(programPtr, shader.shaderPtr);
    }
    

    /// <summary>
    /// Link added shaders together.
    /// </summary>
    /// <param name="messages">Output message types.</param>
    /// <returns>True if linking succeeded.</returns>
    public bool Link(MessageType messages)
    {
        return GlslangNative.LinkProgram(programPtr, messages) == 1;
    }
    

    /// <summary>
    /// Add source text to intermediate.
    /// </summary>
    public void AddSourceText(ShaderStage stage, string text)
    {
        IntPtr textPtr = NativeStringUtility.AllocUTF8Ptr(text, out uint length, false);
        GlslangNative.AddProgramSourceText(programPtr, stage, textPtr, (nuint)length);
        Marshal.FreeHGlobal(textPtr);
    }
    

    /// <summary>
    /// Add source file name to intermediate.
    /// </summary>
    public void SetSourceFile(ShaderStage stage, string file)
    {
        IntPtr filePtr = NativeStringUtility.AllocUTF8Ptr(file, out _, true);
        GlslangNative.SetProgramSourceFile(programPtr, stage, filePtr);
        Marshal.FreeHGlobal(filePtr);
    }


    /// <summary>
    /// Map the program's imputs and outputs.
    /// </summary>
    /// <returns>True if mapping succeeded.</returns>
    public bool MapIO()
    {
        return GlslangNative.MapProgramIO(programPtr) == 1;
    }


    // SPIR-V generation
    
    private bool generatedSPIRV = false;

    /// <summary>
    /// Outputs a byte buffer of generated SPIR-V words.
    /// </summary>
    /// <param name="SPIRVWords">The output buffer of SPIR-V words</param>
    /// <param name="stage">The shader stage to output.</param>
    /// <param name="options">The generation options to use.</param>
    /// <returns>True if generation succeeded.</returns>
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


    /// <summary>
    /// Gets SPIR-V generation messages.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
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


    /// <summary>
    /// Gets program info log.
    /// </summary>
    public string GetInfoLog()
    {
        IntPtr infoLogPtr = GlslangNative.GetProgramInfoDebugLog(programPtr);
        return DeallocString(infoLogPtr);
    }


    /// <summary>
    /// Gets program debug and error logs.
    /// </summary>
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