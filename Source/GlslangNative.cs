using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Glslang.NET;


internal unsafe struct NativeShader { }
internal unsafe struct NativeProgram { }


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
    internal static unsafe partial NativeShader* CreateShader(IntPtr input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void DeleteShader(NativeShader* shader);



    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_preamble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetShaderPreamble(NativeShader* shader, IntPtr preamble);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_shift_binding")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void ShiftShaderBinding(NativeShader* shader, ResourceType res, uint shiftBase);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_shift_binding_for_set")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void ShiftShaderBindingForSet(NativeShader* shader, ResourceType res, uint shiftBase, uint set);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_options")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetShaderOptions(NativeShader* shader, ShaderOptions options);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_glsl_version")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetShaderGLSLVersion(NativeShader* shader, int version);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_preprocess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int PreprocessShader(NativeShader* shader, IntPtr input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_parse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int ParseShader(NativeShader* shader, IntPtr input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_preprocessed_code")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial IntPtr GetPreprocessedShaderCode(NativeShader* shader);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial IntPtr GetShaderInfoLog(NativeShader* shader);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_debug_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial IntPtr GetShaderInfoDebugLog(NativeShader* shader);


    // glslang_program_t

    [LibraryImport(LibName, EntryPoint = "glslang_program_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial NativeProgram* CreateProgram();


    [LibraryImport(LibName, EntryPoint = "glslang_program_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void DeleteProgram(NativeProgram* program);



    [LibraryImport(LibName, EntryPoint = "glslang_program_add_shader")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void AddShaderToProgram(NativeProgram* program, NativeShader* shader);


    [LibraryImport(LibName, EntryPoint = "glslang_program_link")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int LinkProgram(NativeProgram* program, MessageType messages);


    [LibraryImport(LibName, EntryPoint = "glslang_program_add_source_text")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void AddProgramSourceText(NativeProgram* program, ShaderStage stage, IntPtr text, nuint len);


    [LibraryImport(LibName, EntryPoint = "glslang_program_set_source_file")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetProgramSourceFile(NativeProgram* program, ShaderStage stage, IntPtr file);


    [LibraryImport(LibName, EntryPoint = "glslang_program_map_io")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int MapProgramIO(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_get_info_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial IntPtr GetProgramInfoLog(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_get_info_debug_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial IntPtr GetProgramInfoDebugLog(NativeProgram* program);


    // SPIR-V generation

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void GenerateProgramSPIRV(NativeProgram* program, ShaderStage stage);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate_with_options")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void GenerateProgramSPIRVWithOptiosn(NativeProgram* program, ShaderStage stage, IntPtr spvOptions);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_size")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial nuint GetProgramSPIRVSize(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    internal static unsafe partial void GetProgramSPIRVBuffer(NativeProgram* program, [Out] byte[] buffer); // Output type is actually uint, so buffer must be 4x the size


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_ptr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial IntPtr GetProgramSPIRVBuffer(NativeProgram* program); // Allocates a buffer we have to free. Prefer SPIRV_get to this.




    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_messages")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial IntPtr GetProgramSPIRVMessages(NativeProgram* program);



    [LibraryImport(LibName, EntryPoint = "glslang_SPIRV_disassemble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial IntPtr DisassembleSPIRV([In] byte[] spvWords, nuint spvWordsLen); // Input type is actually uint, so words length must be 1/4 the size
}