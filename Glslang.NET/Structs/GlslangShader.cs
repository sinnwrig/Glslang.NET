using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Glslang;


public class GlslangShader
{
    internal readonly IntPtr nativePointer;
    internal readonly ShaderInput input;
    internal readonly ShaderCompiler compiler;


    
    internal GlslangShader(ShaderCompiler? compiler, ShaderInput? input, IntPtr? nativePointer)
    {
        this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        this.input = input ?? throw new ArgumentNullException(nameof(input));
        this.nativePointer = nativePointer ?? throw new ArgumentNullException(nameof(nativePointer));
    }
}