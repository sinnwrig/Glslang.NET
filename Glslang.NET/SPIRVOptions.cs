using System.Runtime.InteropServices;

namespace Glslang.NET;


[StructLayout(LayoutKind.Sequential)]
public struct SPIRVOptions
{
    public bool generateDebugInfo;
    public bool stripDebugInfo;
    public bool disableOptimizer;
    public bool optimizeSize;
    public bool disassemble;
    public bool validate;
    public bool emitNonsemanticShaderDebugInfo;
    public bool emitNonsemanticShaderDebugSource;
    public bool compileOnly;
}