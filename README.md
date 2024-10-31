# Glslang.NET: cross-platform C# wrapper for the Khronos Group's reference GLSL/SPIR-V compiler 

A cross-platform .NET 8.0 wrapper for the Khronos GLSL reference compiler, written in C#. 

[![NuGet](https://img.shields.io/nuget/v/Glslang.NET.svg)](https://www.nuget.org/packages/Glslang.NET)

# Usage

This project wraps functionality from glslang into managed classes, which can be used to compile shader code with various options in a similar fashion to the native glslang interface.<br> However, it appropriately modifies the interface to fit naturally with C# and behaves accordingly. To see an example of usage, refer to [Example.cs](https://github.com/sinnwrig/Glslang.NET/blob/main/Example/Example.cs) in source.

# Native Details

To support cross-platform compilation and to simplify the native build process, Glslang.NET uses a [fork of glslang](https://github.com/sinnwrig/glslang-zig) built with zig instead of CMake or GN/Ninja. As Zig's compiler supports cross-compilation out of the box, it allows glslang to build on any platform, for any platform. Additional functionality is also included in the repository to allow glslang to export the ability to disassemble SPIR-V bytecode into a human-readable format. 

## Building Native Libraries

To build native libraries, run the `BuildNative.cs` file inside the _Native_ folder, specicying your target architecture [x64, arm64, all] with -A and your target platform [windows, linux, macos, all] with -P.
 
Native build requirements:
-Zig compiler version present on your `PATH` of at least version 0.14.0+. You can get the compiler from [Zig's download page](https://ziglang.org/download/) or [from a package manager](https://github.com/ziglang/zig/wiki/Install-Zig-from-a-Package-Manager)
 
Pre-built binaries are bundled in the NuGet package for the following operating systems:
- Windows x64
- Windows arm64
- OSX x64
- OSX arm64 (Apple silicon)
- Linux x64
- Linux arm64
