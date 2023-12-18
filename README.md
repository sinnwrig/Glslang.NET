# DirectXShaderCompiler cross-platform wrapper for .NET

### Baby's first .NET wrapper

A wrapper for the DirectXShaderCompiler that does not use Windows-Specific COM interfaces<br><br>

To Compile: Run build_deps.py to build DirectXShaderCompiler from source and expose a C-Style interface in the library<br><br>

To use:<br>
Create a new DxCompiler<br>
Call DxCompiler.Compile() with the shader code and compiler arguments<br>
see https://github.com/microsoft/DirectXShaderCompiler/wiki/Using-dxc.exe-and-dxcompiler.dll for a list of how to create the argument array<br><br>
Get outputs with the DxResult that Compile() returns<br>


