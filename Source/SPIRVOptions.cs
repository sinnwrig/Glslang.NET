using System.Runtime.InteropServices;

namespace Glslang.NET;

/// <summary>
/// SPIR-V generation options.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct SPIRVOptions
{
    /// <summary></summary>
    public bool generateDebugInfo;

    /// <summary></summary>
    public bool stripDebugInfo;

    /// <summary></summary>
    public bool disableOptimizer;

    /// <summary></summary>
    public bool optimizeSize;

    /// <summary></summary>
    public bool disassemble;

    /// <summary></summary>
    public bool validate;

    /// <summary></summary>
    public bool emitNonsemanticShaderDebugInfo;

    /// <summary></summary>
    public bool emitNonsemanticShaderDebugSource;

    /// <summary></summary>
    public bool compileOnly;
}