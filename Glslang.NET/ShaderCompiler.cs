using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Glslang;


public class ShaderCompiler : IDisposable
{
    private static ShaderCompiler? _instance;

    public const string libraryPath = "library/glslang";
    private const CallingConvention Cdecl = CallingConvention.Cdecl;

    private List<GlslangShader> _shaders;
    private List<GlslangProgram> _programs;


    [DllImport(libraryPath, CallingConvention = Cdecl)] 
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


    [DllImport(libraryPath, CallingConvention = Cdecl)] 
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


    [DllImport(libraryPath, CallingConvention = Cdecl)] 
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



    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangShaderSetPreamble(IntPtr shader, string s);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangShaderShiftBinding(IntPtr shader, ResourceType res, uint shiftBase);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangShaderShiftBindingForSet(IntPtr shader, ResourceType res, uint shiftBase, uint set);

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangShaderSetOptions(IntPtr shader, int options);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangShaderSetGlslVersion(IntPtr shader, int version);

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern int GlslangShaderPreprocess(IntPtr shader, IntPtr input);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern int GlslangShaderParse(IntPtr shader, IntPtr input);

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangShaderGetPreprocessedCode(IntPtr shader); // Returns a char* string
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangShaderGetInfoLog(IntPtr shader); // Returns a char* string
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangShaderGetInfoDebugLog(IntPtr shader); // Returns a char* string



    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangProgramCreate();
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangProgramDelete(IntPtr program);

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangProgramAddShader(IntPtr program, IntPtr shader);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern int GlslangProgramLink(IntPtr program, int messages);

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangProgramAddSourceText(IntPtr program, ShaderStage stage, string text, nint length);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangProgramSetSourceFile(IntPtr program, ShaderStage stage, string file);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern int GlslangProgramMapIo(IntPtr program);

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangProgramSPIRVGenerate(IntPtr program, ShaderStage stage);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangProgramSPIRVGenerateWithOptions(IntPtr program, ShaderStage stage, ref SpirVOptions spv_options);

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern nint GlslangProgramSPIRVGetSize(IntPtr program);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern void GlslangProgramSPIRVGet(IntPtr program, [Out] uint[] output);
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangProgramSPIRVGetPtr(IntPtr program); // Returns a uint* array
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr DisassembleSPIRVBinary([In] uint[] input, nint size); // Returns a char* string

    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangProgramSPIRVGetMessages(IntPtr program); // Returns a char* string
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangProgramGetInfoLog(IntPtr program); // Returns a char* string
    [DllImport(libraryPath, CallingConvention = Cdecl)] private static extern IntPtr GlslangProgramGetInfoDebugLog(IntPtr program); // Returns a char* string



    [DllImport(libraryPath, CallingConvention = Cdecl)] 
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