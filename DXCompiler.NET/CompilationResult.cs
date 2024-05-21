using System.Reflection;
using System.Runtime.InteropServices;

namespace DXCompiler.NET;

public struct CompilationResult
{
    public byte[] objectBytes;
    public string? compilationErrors;
}