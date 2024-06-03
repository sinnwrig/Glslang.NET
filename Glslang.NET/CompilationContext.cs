using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


public class CompilationContext : NativeResourceHandle 
{
    private readonly List<Shader> activeShaders = new();
    private readonly List<ShaderProgram> activePrograms = new();


    public CompilationContext(string[]? librarySearchPaths = null)
    {
        GlslangNative.ResolveAssemblies(librarySearchPaths);
        
        int success = GlslangNative.glslang_initialize_process();

        if (success != 1)
            throw new Exception("Failed to initialize native context process."); 
    }


    public Shader CreateShader(CompilationInput input)
    {
        Shader shader = new(input);
        activeShaders.Add(shader);
        return shader;
    }


    public ShaderProgram CreateProgram()
    {
        ShaderProgram program = new();
        activePrograms.Add(program);
        return program;
    }


    public string DisassembleSPIRV(byte[] spirvWords)
    {
        IntPtr textPtr = GlslangNative.glslang_SPIRV_disassemble(spirvWords, (nuint)(spirvWords.Length / sizeof(uint)));
        string text = Marshal.PtrToStringUTF8(textPtr) ?? string.Empty;
        Marshal.FreeHGlobal(textPtr);
        return text;
    }


    protected override void ReleaseHandle()
    {
        foreach (ShaderProgram pr in activePrograms)
            pr.Release();

        foreach (Shader sh in activeShaders)
            sh.Release();

        GlslangNative.glslang_finalize_process();
    }    
}