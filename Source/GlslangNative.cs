using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Glslang.NET;

internal unsafe struct NativeShader { }
internal unsafe struct NativeProgram { }
internal unsafe struct NativeMapper { }
internal unsafe struct NativeResolver { }


internal static partial class GlslangNative
{
    public static unsafe T* Allocate<T>() where T : unmanaged
    {
        return (T*)Marshal.AllocHGlobal(sizeof(T));
    }


    public static unsafe T* Allocate<T>(T defaultValue) where T : unmanaged
    {
        T* instance = (T*)Marshal.AllocHGlobal(sizeof(T));
        Marshal.StructureToPtr(defaultValue, (nint)instance, false);

        return instance;
    }


    public static unsafe void Free<T>(T* ptr) where T : unmanaged
    {
        if (ptr == null)
            throw new Exception("Tried to deallocate a null pointer");

        Marshal.FreeHGlobal((nint)ptr);
    }


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
    internal static unsafe partial NativeShader* CreateShader(NativeInput* input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void DeleteShader(NativeShader* shader);



    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_preamble", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetShaderPreamble(NativeShader* shader, string preamble);


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


    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_default_uniform_block_name", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetShaderDefaultUniformBlockName(NativeShader* shader, string name);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_resource_set_binding", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetShaderResourceSetBinding(NativeShader* shader, [In] string[] bindings, uint num_bindings);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_preprocess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int PreprocessShader(NativeShader* shader, NativeInput* input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_parse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int ParseShader(NativeShader* shader, NativeInput* input);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_preprocessed_code", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial byte* GetPreprocessedShaderCode(NativeShader* shader);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_preprocessed_code", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetPreprocessedShaderCode(NativeShader* shader, string code);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_log", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial byte* GetShaderInfoLog(NativeShader* shader);


    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_debug_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial byte* GetShaderInfoDebugLog(NativeShader* shader);


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
    internal static unsafe partial void AddProgramSourceText(NativeProgram* program, ShaderStage stage, byte* text, nuint len);


    [LibraryImport(LibName, EntryPoint = "glslang_program_set_source_file", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void SetProgramSourceFile(NativeProgram* program, ShaderStage stage, string file);


    [LibraryImport(LibName, EntryPoint = "glslang_program_map_io")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int MapProgramIO(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_map_io_with_resolver_and_mapper")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial int MapProgramIOWithResolverAndMapper(NativeProgram* program, NativeResolver* resolver, NativeMapper* mapper);


    [LibraryImport(LibName, EntryPoint = "glslang_program_get_info_log", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial byte* GetProgramInfoLog(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_get_info_debug_log", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial byte* GetProgramInfoDebugLog(NativeProgram* program);


    // SPIR-V generation

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void GenerateProgramSPIRV(NativeProgram* program, ShaderStage stage);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate_with_options")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void GenerateProgramSPIRVWithOptions(NativeProgram* program, ShaderStage stage, SPIRVOptions* spvOptions);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_size")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial nuint GetProgramSPIRVSize(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get")]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    internal static unsafe partial void GetProgramSPIRVBuffer(NativeProgram* program, [Out] uint[] buffer);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_ptr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial uint* GetProgramSPIRVBuffer(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_messages", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial byte* GetProgramSPIRVMessages(NativeProgram* program);


    [LibraryImport(LibName, EntryPoint = "glslang_SPIRV_disassemble", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial string DisassembleSPIRV([In] uint[] spvWords, nuint spvWordsLen);



    [LibraryImport(LibName, EntryPoint = "glslang_glsl_mapper_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial NativeMapper* CreateMapper();


    [LibraryImport(LibName, EntryPoint = "glslang_glsl_mapper_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void DeleteMapper(NativeMapper* mapper);



    [LibraryImport(LibName, EntryPoint = "glslang_glsl_resolver_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial NativeResolver* CreateResolver(NativeProgram* program, ShaderStage stage);


    [LibraryImport(LibName, EntryPoint = "glslang_glsl_resolver_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static unsafe partial void DeleteResolver(NativeResolver* resolver);
}