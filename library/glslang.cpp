#include <string>
#include <cstring>
#include <sstream>

#define GLSLANG_IS_SHARED_LIBRARY

#include <glslang_c_interface.h>
#include <disassemble.h>

#include <stdbool.h>
#include <stdlib.h>

#include "glslang_c_shader_types.h"
#include "resource_limits_c.h"

#ifdef __cplusplus
extern "C" {
#endif


GLSLANG_EXPORT const glslang_resource_t* GetDefaultResource(void) { return glslang_default_resource(); }
GLSLANG_EXPORT const char* DefaultResourceString() { return glslang_default_resource_string(); }
GLSLANG_EXPORT void DecodeResourceLimits(glslang_resource_t* resources, char* config) { return glslang_decode_resource_limits(resources, config); }


// VERY IMPORTANT TO DO BEFORE ANYTHING ELSE
GLSLANG_EXPORT int GlslangInitializeProcess(void) { return glslang_initialize_process(); }
GLSLANG_EXPORT void GlslangFinalizeProcess(void) { glslang_finalize_process(); }


GLSLANG_EXPORT glslang_shader_t* GlslangCreateShader(const glslang_input_t* input) { return glslang_shader_create(input); }
GLSLANG_EXPORT void GlslangDeleteShader(glslang_shader_t* shader) { glslang_shader_delete(shader); }

GLSLANG_EXPORT void GlslangShaderSetPreamble(glslang_shader_t* shader, const char* s) { glslang_shader_set_preamble(shader, s); }

GLSLANG_EXPORT void GlslangShaderShiftBinding(glslang_shader_t* shader, glslang_resource_type_t res, unsigned int base) { glslang_shader_shift_binding(shader, res, base); }
GLSLANG_EXPORT void GlslangShaderShiftBindingForSet(glslang_shader_t* shader, glslang_resource_type_t res, unsigned int base, unsigned int set) { glslang_shader_shift_binding_for_set(shader, res, base, set); }

GLSLANG_EXPORT void GlslangShaderSetOptions(glslang_shader_t* shader, int options) { glslang_shader_set_options(shader, options); }
GLSLANG_EXPORT void GlslangShaderSetGlslVersion(glslang_shader_t* shader, int version) { glslang_shader_set_glsl_version(shader, version); }

GLSLANG_EXPORT int GlslangShaderPreprocess(glslang_shader_t* shader, const glslang_input_t* input) { return glslang_shader_preprocess(shader, input); }
GLSLANG_EXPORT int GlslangShaderParse(glslang_shader_t* shader, const glslang_input_t* input) { return glslang_shader_parse(shader, input); }

GLSLANG_EXPORT const char* GlslangShaderGetPreprocessedCode(glslang_shader_t* shader) { return glslang_shader_get_preprocessed_code(shader); }
GLSLANG_EXPORT const char* GlslangShaderGetInfoLog(glslang_shader_t* shader) { return glslang_shader_get_info_log(shader); }
GLSLANG_EXPORT const char* GlslangShaderGetInfoDebugLog(glslang_shader_t* shader) { return glslang_shader_get_info_debug_log(shader); }


GLSLANG_EXPORT glslang_program_t* GlslangProgramCreate(void) { return glslang_program_create(); }
GLSLANG_EXPORT void GlslangProgramDelete(glslang_program_t* program) { glslang_program_delete(program); }

GLSLANG_EXPORT void GlslangProgramAddShader(glslang_program_t* program, glslang_shader_t* shader) { glslang_program_add_shader(program, shader); }
GLSLANG_EXPORT int GlslangProgramLink(glslang_program_t* program, int messages) { return glslang_program_link(program, messages); }

GLSLANG_EXPORT void GlslangProgramAddSourceText(glslang_program_t* program, glslang_stage_t stage, const char* text, size_t len) { glslang_program_add_source_text(program, stage, text, len); }
GLSLANG_EXPORT void GlslangProgramSetSourceFile(glslang_program_t* program, glslang_stage_t stage, const char* file) { glslang_program_set_source_file(program, stage, file); }
GLSLANG_EXPORT int GlslangProgramMapIo(glslang_program_t* program) { return glslang_program_map_io(program); }

GLSLANG_EXPORT void GlslangProgramSPIRVGenerate(glslang_program_t* program, glslang_stage_t stage) { glslang_program_SPIRV_generate(program, stage); }
GLSLANG_EXPORT void GlslangProgramSPIRVGenerateWithOptions(glslang_program_t* program, glslang_stage_t stage, glslang_spv_options_t* spv_options) { glslang_program_SPIRV_generate_with_options(program, stage, spv_options); }

GLSLANG_EXPORT size_t GlslangProgramSPIRVGetSize(glslang_program_t* program) { return glslang_program_SPIRV_get_size(program); }
GLSLANG_EXPORT void GlslangProgramSPIRVGet(glslang_program_t* program, unsigned int* output) { glslang_program_SPIRV_get(program, output); }
GLSLANG_EXPORT unsigned int* GlslangProgramSPIRVGetPtr(glslang_program_t* program) { return glslang_program_SPIRV_get_ptr(program); }

GLSLANG_EXPORT const char* GlslangProgramSPIRVGetMessages(glslang_program_t* program) { return glslang_program_SPIRV_get_messages(program); }
GLSLANG_EXPORT const char* GlslangProgramGetInfoLog(glslang_program_t* program) { return glslang_program_get_info_log(program); }
GLSLANG_EXPORT const char* GlslangProgramGetInfoDebugLog(glslang_program_t* program) { return glslang_program_get_info_debug_log(program); }


// Dissasembles and copies bytecode into a default C# string pointer
GLSLANG_EXPORT char* DisassembleSPIRVBinary(const uint32_t *words, const size_t size) 
{ 
    if (size > 0) 
    { 
        std::vector<unsigned int> spirvCode(words, words + size);
        std::stringstream stream; spv::Disassemble(stream, spirvCode); std::string string = stream.str();
        char* result = new char[string.length() + 1]; std::strcpy(result, string.c_str());
        return result; 
    } 
    else
    { 
        printf("Invalid SpirVBinary size: %zu\n", size);
        return nullptr; 
    }
}

#ifdef __cplusplus
}
#endif
