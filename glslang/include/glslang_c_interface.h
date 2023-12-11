/**
    This code is based on the glslang_c_interface implementation by Viktor Latypov
**/

/**
BSD 2-Clause License

Copyright (c) 2019, Viktor Latypov
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**/

#ifndef GLSLANG_C_IFACE_H_INCLUDED
#define GLSLANG_C_IFACE_H_INCLUDED

#include <stdbool.h>
#include <stdlib.h>


#include "glslang_c_shader_types.h"

#ifdef __cplusplus
extern "C" {
#endif

#ifdef GLSLANG_IS_SHARED_LIBRARY
    #ifdef _WIN32
        #ifdef GLSLANG_EXPORTING
            #define GLSLANG_EXPORT __declspec(dllexport)
        #else
            #define GLSLANG_EXPORT __declspec(dllimport)
        #endif
    #elif __GNUC__ >= 4
        #define GLSLANG_EXPORT __attribute__((visibility("default")))
    #endif
#endif // GLSLANG_IS_SHARED_LIBRARY

#ifndef GLSLANG_EXPORT
#define GLSLANG_EXPORT
#endif

GLSLANG_EXPORT int glslang_initialize_process(void);
GLSLANG_EXPORT void glslang_finalize_process(void);

GLSLANG_EXPORT glslang_shader_t* glslang_shader_create(const glslang_input_t* input);
GLSLANG_EXPORT void glslang_shader_delete(glslang_shader_t* shader);
GLSLANG_EXPORT void glslang_shader_set_preamble(glslang_shader_t* shader, const char* s);
GLSLANG_EXPORT void glslang_shader_shift_binding(glslang_shader_t* shader, glslang_resource_type_t res, unsigned int base);
GLSLANG_EXPORT void glslang_shader_shift_binding_for_set(glslang_shader_t* shader, glslang_resource_type_t res, unsigned int base, unsigned int set);
GLSLANG_EXPORT void glslang_shader_set_options(glslang_shader_t* shader, int options); // glslang_shader_options_t
GLSLANG_EXPORT void glslang_shader_set_glsl_version(glslang_shader_t* shader, int version);
GLSLANG_EXPORT int glslang_shader_preprocess(glslang_shader_t* shader, const glslang_input_t* input);
GLSLANG_EXPORT int glslang_shader_parse(glslang_shader_t* shader, const glslang_input_t* input);
GLSLANG_EXPORT const char* glslang_shader_get_preprocessed_code(glslang_shader_t* shader);
GLSLANG_EXPORT const char* glslang_shader_get_info_log(glslang_shader_t* shader);
GLSLANG_EXPORT const char* glslang_shader_get_info_debug_log(glslang_shader_t* shader);

GLSLANG_EXPORT glslang_program_t* glslang_program_create(void);
GLSLANG_EXPORT void glslang_program_delete(glslang_program_t* program);
GLSLANG_EXPORT void glslang_program_add_shader(glslang_program_t* program, glslang_shader_t* shader);
GLSLANG_EXPORT int glslang_program_link(glslang_program_t* program, int messages); // glslang_messages_t
GLSLANG_EXPORT void glslang_program_add_source_text(glslang_program_t* program, glslang_stage_t stage, const char* text, size_t len);
GLSLANG_EXPORT void glslang_program_set_source_file(glslang_program_t* program, glslang_stage_t stage, const char* file);
GLSLANG_EXPORT int glslang_program_map_io(glslang_program_t* program);
GLSLANG_EXPORT void glslang_program_SPIRV_generate(glslang_program_t* program, glslang_stage_t stage);
GLSLANG_EXPORT void glslang_program_SPIRV_generate_with_options(glslang_program_t* program, glslang_stage_t stage, glslang_spv_options_t* spv_options);
GLSLANG_EXPORT size_t glslang_program_SPIRV_get_size(glslang_program_t* program);
GLSLANG_EXPORT void glslang_program_SPIRV_get(glslang_program_t* program, unsigned int*);
GLSLANG_EXPORT unsigned int* glslang_program_SPIRV_get_ptr(glslang_program_t* program);
GLSLANG_EXPORT const char* glslang_program_SPIRV_get_messages(glslang_program_t* program);
GLSLANG_EXPORT const char* glslang_program_get_info_log(glslang_program_t* program);
GLSLANG_EXPORT const char* glslang_program_get_info_debug_log(glslang_program_t* program);

#ifdef __cplusplus
}
#endif

#endif /* #ifdef GLSLANG_C_IFACE_INCLUDED */
