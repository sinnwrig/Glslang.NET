using System.Reflection;
using System.Runtime.InteropServices;

namespace Glslang.NET;


internal static class GlslangNative
{
    // Bundles platform and architecture for ease-of-use
    private struct PlatformInfo
    {
        public OSPlatform platform;
        public Architecture architecture;

        public PlatformInfo(OSPlatform platform, Architecture architecture)
        {
            this.platform = platform;
            this.architecture = architecture;
        }

        public static OSPlatform GetPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSPlatform.OSX;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
                return OSPlatform.Linux;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
                return OSPlatform.Windows;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                return OSPlatform.FreeBSD;
            throw new Exception("Cannot determine operating system.");
        }

        public static PlatformInfo GetCurrentPlatform() => new PlatformInfo(GetPlatform(), RuntimeInformation.ProcessArchitecture);
    }

    const string LibName = "glslang";

    const string WinLib = LibName + ".dll";
    const string OSXLib = "lib" + LibName + ".dylib";
    const string LinuxLib = "lib" + LibName + ".so";


    private static string CreateLibPath(string platform) => Path.Combine("runtimes", platform, "native");

    private static readonly Dictionary<PlatformInfo, (string, string)> LibraryPaths = new()
    {
        { new PlatformInfo(OSPlatform.Windows, Architecture.X64), (CreateLibPath("win-x64"), WinLib) },
        { new PlatformInfo(OSPlatform.Windows, Architecture.Arm64), (CreateLibPath("win-arm64"), WinLib) },

        { new PlatformInfo(OSPlatform.OSX, Architecture.X64), (CreateLibPath("osx-x64"), OSXLib) },
        { new PlatformInfo(OSPlatform.OSX, Architecture.Arm64), (CreateLibPath("osx-arm64"), OSXLib) },

        { new PlatformInfo(OSPlatform.Linux, Architecture.X64), (CreateLibPath("linux-x64"), LinuxLib) },
        { new PlatformInfo(OSPlatform.Linux, Architecture.Arm64), (CreateLibPath("linux-arm64"), LinuxLib) },
    };


    private static bool _assembliesResolved = false;
    private static string[]? additionalSearchPaths;


    internal static void ResolveAssemblies(string[]? additionalSearchPaths)
    {
        if (_assembliesResolved)
            return;

        GlslangNative.additionalSearchPaths = additionalSearchPaths;

        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);

        _assembliesResolved = true;
    }

    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != LibName)
            return IntPtr.Zero;

        PlatformInfo platform = PlatformInfo.GetCurrentPlatform();

        (string, string) libraryPath = LibraryPaths[platform];

        /*string applicationPath = AppContext.BaseDirectory;
        string assemblyPath = Path.GetDirectoryName(assembly.Location) ?? applicationPath;

        List<string> searchPaths = new()
        {
            // Possible library locations in release build
            applicationPath, // App path
            assemblyPath,    // Assembly path

            // Possible library locations in debug build
            Path.Join(applicationPath, libraryPath.Item1), 
            Path.Join(assemblyPath, libraryPath.Item1),  
        };
        
        // Add other possible library paths
        if (additionalSearchPaths != null)
        {
            foreach (string path in additionalSearchPaths)
            {
                // Root path, no need to combine
                if (Path.IsPathRooted(path))
                {
                    searchPaths.Add(path);
                }
                else
                {
                    // Add possible application and assembly paths.
                    searchPaths.Add(Path.Join(applicationPath, path));
                    searchPaths.Add(Path.Join(assemblyPath, path));
                }
            }
        }

        string bestPath = "/"; 
        foreach (string path in searchPaths)
        {
            string filePath = Path.Join(path, libraryPath.Item2);

            if (File.Exists(filePath))
                bestPath = filePath;
        }*/

        IntPtr library = NativeLibrary.Load(libraryPath.Item2, assembly, DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.ApplicationDirectory);

        if (library == IntPtr.Zero)
            throw new DllNotFoundException($"Could not find {libraryPath.Item2} shared library");

        return library;
    }

    const CallingConvention cconv = CallingConvention.Cdecl;



    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_initialize_process")]
    internal static extern int InitializeProcess();


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_finalize_process")]
    internal static extern void FinalizeProcess();


    // glslang_shader_t

    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_create")]
    internal static extern IntPtr CreateShader(IntPtr input);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_delete")]
    internal static extern void DeleteShader(IntPtr shader);



    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_set_preamble")]
    internal static extern void SetShaderPreamble(IntPtr shader, IntPtr preamble);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_shift_binding")]
    internal static extern void ShiftShaderBinding(IntPtr shader, ResourceType res, uint shiftBase);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_shift_binding_for_set")]
    internal static extern void ShiftShaderBindingForSet(IntPtr shader, ResourceType res, uint shiftBase, uint set);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_set_options")]
    internal static extern void SetShaderOptions(IntPtr shader, ShaderOptions options);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_set_glsl_version")]
    internal static extern void SetShaderGLSLVersion(IntPtr shader, int version);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_preprocess")]
    internal static extern int PreprocessShader(IntPtr shader, IntPtr input);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_parse")]
    internal static extern int ParseShader(IntPtr shader, IntPtr input);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_get_preprocessed_code")]
    internal static extern IntPtr GetPreprocessedShaderCode(IntPtr shader);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_get_info_log")]
    internal static extern IntPtr GetShaderInfoLog(IntPtr shader);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_shader_get_info_debug_log")]
    internal static extern IntPtr GetShaderInfoDebugLog(IntPtr shader);


    // glslang_program_t

    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_create")]
    internal static extern IntPtr CreateProgram();


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_delete")]
    internal static extern void DeleteProgram(IntPtr program);



    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_add_shader")]
    internal static extern void AddShaderToProgram(IntPtr program, IntPtr shader);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_link")]
    internal static extern int LinkProgram(IntPtr program, MessageType messages); 


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_add_source_text")]
    internal static extern void AddProgramSourceText(IntPtr program, ShaderStage stage, IntPtr text, nuint len);

    
    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_set_source_file")]
    internal static extern void SetProgramSourceFile(IntPtr program, ShaderStage stage, IntPtr file);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_map_io")]
    internal static extern int MapProgramIO(IntPtr program);


    // SPIR-V generation

    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_generate")]
    internal static extern void GenerateProgramSPIRV(IntPtr program, ShaderStage stage);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_generate_with_options")]
    internal static extern void GenerateProgramSPIRVWithOptiosn(IntPtr program, ShaderStage stage, IntPtr spvOptions);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_get_size")]
    internal static extern nuint GetProgramSPIRVSize(IntPtr program);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_get")]
    internal static extern void GetProgramSPIRVBuffer(IntPtr program, [Out] byte[] buffer); // Output type is actually uint, so buffer must be 4x the size


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_get_ptr")]
    internal static extern IntPtr GetProgramSPIRVBuffer(IntPtr program); // Allocates a buffer we have to free. Prefer SPIRV_get to this.


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_get_messages")]
    internal static extern IntPtr GetProgramSPIRVMessages(IntPtr program);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_get_info_log")]
    internal static extern IntPtr GetProgramInfoLog(IntPtr program);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_program_SPIRV_get_info_debug_log")]
    internal static extern IntPtr GetProgramInfoDebugLog(IntPtr program);


    [DllImport(LibName, CallingConvention = cconv, ExactSpelling = true, EntryPoint = "glslang_SPIRV_disassemble")]
    internal static extern IntPtr DisassembleSPIRV([In] byte[] spvWords, nuint spvWordsLen); // Input type is actually uint, so words length must be 1/4 the size
}