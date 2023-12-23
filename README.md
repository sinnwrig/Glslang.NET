# DirectXShaderCompiler cross-platform wrapper for .NET

### Baby's first .NET wrapper

A wrapper for the DirectXShaderCompiler that does not use Windows-Specific COM interfaces<br><br>

To Compile: Run build_deps.py to build DirectXShaderCompiler from source and expose a C-Style interface in the library<br><br>

To use:<br>
Create a new ShaderCompiler<br>
Create a CompilerOptions object and set all the desired properties<br>
Call ShaderCompiler.Compile() with the shader code and compiler options<br>
Get outputs with the CompilationResult object that Compile() returns<br>


