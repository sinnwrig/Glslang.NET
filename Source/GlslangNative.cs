using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Glslang.NET;

internal unsafe struct NativeShader { }
internal unsafe struct NativeProgram { }
internal unsafe struct NativeMapper { }
internal unsafe struct NativeResolver { }


internal static partial class GlslangNative
{
    private const string LibName = "glslang";


    public static unsafe T* Allocate<T>() where T : unmanaged
    {
        return (T*)Marshal.AllocHGlobal(sizeof(T));
    }


    public static unsafe T* Allocate<T>(T defaultValue) where T : unmanaged
    {
        T* tPointer = (T*)Marshal.AllocHGlobal(sizeof(T));

        GCHandle gCHandle = GCHandle.Alloc(defaultValue, GCHandleType.Pinned);
        Buffer.MemoryCopy((void*)gCHandle.AddrOfPinnedObject(), tPointer, sizeof(T), sizeof(T));
        gCHandle.Free();

        return tPointer;
    }


    public static unsafe void Free<T>(T* ptr) where T : unmanaged
    {
        if (ptr == null)
            throw new Exception("Tried to deallocate a null pointer");

        Marshal.FreeHGlobal((nint)ptr);
    }


    [LibraryImport(LibName, EntryPoint = "glslang_initialize_process")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial int InitializeProcess();

    [LibraryImport(LibName, EntryPoint = "glslang_finalize_process")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal static partial void FinalizeProcess();



    [LibraryImport(LibName, EntryPoint = "glslang_glsl_mapper_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial NativeMapper* CreateGLSLMapper();

    [LibraryImport(LibName, EntryPoint = "glslang_glsl_mapper_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void DeleteGLSLMapper(NativeMapper* mapper);



    [LibraryImport(LibName, EntryPoint = "glslang_glsl_resolver_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial NativeResolver* CreateGLSLResolver(NativeProgram* program, ShaderStage stage);

    [LibraryImport(LibName, EntryPoint = "glslang_glsl_resolver_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void DeleteGLSLResolver(NativeResolver* resolver);



    [LibraryImport(LibName, EntryPoint = "glslang_default_resource")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial ResourceLimits* DefaultResourceLimits();



    [LibraryImport(LibName, EntryPoint = "glslang_shader_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial NativeShader* CreateShader(NativeCompilationInput* input);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void DeleteShader(NativeShader* shader);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_preprocessed_code")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial byte* GetPreprocessedShaderCode(NativeShader* shader);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_debug_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial byte* GetShaderInfoDebugLog(NativeShader* shader);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_get_info_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial byte* GetShaderInfoLog(NativeShader* shader);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_parse")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial int ParseShader(NativeShader* shader, NativeCompilationInput* input);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_preprocess")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial int PreprocessShader(NativeShader* shader, NativeCompilationInput* input);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_default_uniform_block_name", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetDefaultUniformBlockName(NativeShader* shader, string name);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_default_uniform_block_set_and_binding")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetDefaultUniformBlockSetAndBinding(NativeShader* shader, uint set, uint binding);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_preprocessed_code", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetPreprocessedShaderCode(NativeShader* shader, string shaderCodeBytes);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_resource_set_binding")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetResourceSetBinding(NativeShader* shader, byte** bindings, uint num_bindings);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_glsl_version")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetShaderGLSLVersion(NativeShader* shader, int version);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_options")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetShaderOptions(NativeShader* shader, ShaderOptions options);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_set_preamble")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetShaderPreamble(NativeShader* shader, byte* preamble);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_shift_binding")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void ShiftShaderBinding(NativeShader* shader, ResourceType res, uint shiftBase);

    [LibraryImport(LibName, EntryPoint = "glslang_shader_shift_binding_for_set")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void ShiftShaderBindingForSet(NativeShader* shader, ResourceType res, uint shiftBase, uint set);



    [LibraryImport(LibName, EntryPoint = "glslang_program_add_source_text")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void AddProgramSourceText(NativeProgram* program, ShaderStage stage, byte* text, UIntPtr len);

    [LibraryImport(LibName, EntryPoint = "glslang_program_add_shader")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void AddShaderToProgram(NativeProgram* program, NativeShader* shader);

    [LibraryImport(LibName, EntryPoint = "glslang_program_create")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial NativeProgram* CreateProgram();

    [LibraryImport(LibName, EntryPoint = "glslang_program_delete")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void DeleteProgram(NativeProgram* program);

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void GenerateProgramSPIRV(NativeProgram* program, ShaderStage stage);

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_generate_with_options")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void GenerateProgramSPIRVWithOptions(NativeProgram* program, ShaderStage stage, SPIRVOptions* spvOptions);

    [LibraryImport(LibName, EntryPoint = "glslang_program_get_info_debug_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial byte* GetProgramInfoDebugLog(NativeProgram* program);

    [LibraryImport(LibName, EntryPoint = "glslang_program_get_info_log")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial byte* GetProgramInfoLog(NativeProgram* program);

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void GetProgramSPIRVBuffer(NativeProgram* program, [Out] uint[] buffer);

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_ptr")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial uint* GetProgramSPIRVBuffer(NativeProgram* program);

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_messages")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial byte* GetProgramSPIRVMessages(NativeProgram* program);

    [LibraryImport(LibName, EntryPoint = "glslang_program_SPIRV_get_size")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial UIntPtr GetProgramSPIRVSize(NativeProgram* program);

    [LibraryImport(LibName, EntryPoint = "glslang_program_link")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial int LinkProgram(NativeProgram* program, MessageType messages);

    [LibraryImport(LibName, EntryPoint = "glslang_program_map_io")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial int MapProgramIO(NativeProgram* program);

    [LibraryImport(LibName, EntryPoint = "glslang_program_map_io_with_resolver_and_mapper")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial int MapProgramIOWithResolverAndMapper(NativeProgram* program, NativeResolver* resolver, NativeMapper* mapper);

    [LibraryImport(LibName, EntryPoint = "glslang_program_set_source_file", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial void SetProgramSourceFile(NativeProgram* program, ShaderStage stage, string file);



    [LibraryImport(LibName, EntryPoint = "glslang_SPIRV_disassemble", StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    internal unsafe static partial string DisassembleSPIRV([In] uint[] spvWords, nuint spvWordsLen);
}