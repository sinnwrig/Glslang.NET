using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Glslang.NET;


internal static partial class GlslangNative
{
    const string LibName = "glslang";

    [LibraryImport(LibName, EntryPoint = "glslang_initialize_process")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int InitializeProcess();


    [LibraryImport(LibName, EntryPoint = "glslang_finalize_process")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void FinalizeProcess();


    // glslang_shader_t

    [LibraryImport(LibName, EntryPoint = "glslang_shader_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr CreateShader(IntPtr input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void DeleteShader(IntPtr shader);



    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_preamble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SetShaderPreamble(IntPtr shader, IntPtr preamble);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_shift_binding")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void ShiftShaderBinding(IntPtr shader, ResourceType res, uint shiftBase);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_shift_binding_for_set")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void ShiftShaderBindingForSet(IntPtr shader, ResourceType res, uint shiftBase, uint set);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_options")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SetShaderOptions(IntPtr shader, ShaderOptions options);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_glsl_version")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SetShaderGLSLVersion(IntPtr shader, int version);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_preprocess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int PreprocessShader(IntPtr shader, IntPtr input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_parse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int ParseShader(IntPtr shader, IntPtr input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_preprocessed_code")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr GetPreprocessedShaderCode(IntPtr shader);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr GetShaderInfoLog(IntPtr shader);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_debug_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr GetShaderInfoDebugLog(IntPtr shader);


    // glslang_program_t

    [LibraryImport(LibName, EntryPoint = "glslang_program_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr CreateProgram();


    [LibraryImport(LibName, EntryPoint = "glslang_program_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void DeleteProgram(IntPtr program);



    [LibraryImport(LibName, EntryPoint = "glslang_program_add_shader")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void AddShaderToProgram(IntPtr program, IntPtr shader);


    [LibraryImport(LibName, EntryPoint = "glslang_program_link")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int LinkProgram(IntPtr program, MessageType messages);


    [LibraryImport(LibName, EntryPoint = "glslang_program_add_source_text")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void AddProgramSourceText(IntPtr program, ShaderStage stage, IntPtr text, nuint len);


    [LibraryImport(LibName, EntryPoint = "glslang_program_set_source_file")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void SetProgramSourceFile(IntPtr program, ShaderStage stage, IntPtr file);


    [LibraryImport(LibName, EntryPoint = "glslang_program_map_io")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int MapProgramIO(IntPtr program);


    // SPIR-V generation

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void GenerateProgramSPIRV(IntPtr program, ShaderStage stage);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate_with_options")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void GenerateProgramSPIRVWithOptiosn(IntPtr program, ShaderStage stage, IntPtr spvOptions);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_size")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial nuint GetProgramSPIRVSize(IntPtr program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    internal static partial void GetProgramSPIRVBuffer(IntPtr program, [Out] byte[] buffer); // Output type is actually uint, so buffer must be 4x the size


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_ptr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr GetProgramSPIRVBuffer(IntPtr program); // Allocates a buffer we have to free. Prefer SPIRV_get to this.


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_messages")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr GetProgramSPIRVMessages(IntPtr program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_info_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr GetProgramInfoLog(IntPtr program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_info_debug_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr GetProgramInfoDebugLog(IntPtr program);


    [LibraryImport(LibName, EntryPoint = "glslang_SPIRV_disassemble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr DisassembleSPIRV([In] byte[] spvWords, nuint spvWordsLen); // Input type is actually uint, so words length must be 1/4 the size
}