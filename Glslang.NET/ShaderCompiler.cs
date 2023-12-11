using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Glslang;


public class ShaderCompiler : IDisposable
{
    public const string libraryPath = "library/glslang";

    private static ShaderCompiler? _instance;

    private List<GlslangShader> _shaders;
    private List<GlslangProgram> _programs;



    [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangInitializeProcess();

    public ShaderCompiler()
    {
        Console.WriteLine("Initializing Shader Compiler...\n");

        if (_instance != null && _instance != this)
            throw new InvalidOperationException("Attempted to create ShaderCompiler while another instance was active. Make sure that there is only one ShaderCompiler instance at a given time.");

        _shaders = new List<GlslangShader>();
        _programs = new List<GlslangProgram>();

        GlslangInitializeProcess();
        _instance = this;
    }


    [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern IntPtr GlslangCreateShader(IntPtr input);

    public GlslangShader CreateShader(ShaderInput input) 
    {
        Console.WriteLine($"Creating shader...");
        IntPtr inputPtr = input.AllocateNativePtr();
        IntPtr shaderPtr = GlslangCreateShader(inputPtr);

        GlslangShader shader = new GlslangShader(this, input, shaderPtr);
        _shaders.Add(shader);

        Console.WriteLine($"Created shader with pointer: {shader.nativePointer}\n");

        return shader;
    }


    [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangDeleteShader(IntPtr shader);

    public void DeleteShader(GlslangShader shader)
    {
        // Confirm that shader was created by this compiler
        if (_shaders.Remove(shader))
        {
            Console.WriteLine($"Deleting shader...");
            GlslangDeleteShader(shader.nativePointer);
            shader.input.FreeNativePtr();
            Console.WriteLine($"Deleted shader with pointer: {shader.nativePointer}");
        }
    }


    [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr GlslangProgramCreate();
    [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)] private static extern void GlslangProgramDelete(IntPtr program);


    [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)] private static extern IntPtr DisassembleSPIRVBinary([In] uint[] input, nint size); // Returns a char* string


    [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)] 
    private static extern void GlslangFinalizeProcess();

    public void Dispose()
    {
        Console.WriteLine("Disposing Shader Compiler...\n");
        foreach (GlslangShader shader in _shaders.ToArray())
            DeleteShader(shader);

        GlslangFinalizeProcess();
        _instance = null;

        GC.SuppressFinalize(this);
    }


    ~ShaderCompiler()
    {
        if (_instance != null)
        {
            Console.Write(new WarningException(
                "Glslang Shader Compiler was not properly disposed of. Please make sure to call Dispose() or wrap compiler in a using statement."
            ).ToString());

            Dispose();
        }
    }
}