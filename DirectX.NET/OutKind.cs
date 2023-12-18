namespace DirectX;

public enum OutKind
{
    None = 0,          ///< No output.
    Object = 1,        ///< IDxcBlob - Shader or library object.
    Errors = 2,        ///< IDxcBlobUtf8 or IDxcBlobWide.
    Pdb = 3,           ///< IDxcBlob.
    ShaderHash = 4,    ///< IDxcBlob - DxcShaderHash of shader or shader with source info (-Zsb/-Zss).
    Disassembly = 5,   ///< IDxcBlobUtf8 or IDxcBlobWide - from Disassemble.
    HLSL = 6,          ///< IDxcBlobUtf8 or IDxcBlobWide - from Preprocessor or Rewriter.
    Text = 7,          ///< IDxcBlobUtf8 or IDxcBlobWide - other text, such as -ast-dump or -Odump.
    Reflection = 8,    ///< IDxcBlob - RDAT part with reflection data.
    RootSignature = 9, ///< IDxcBlob - Serialized root signature output.
    ExtraOutputs = 10, ///< IDxcExtraOutputs - Extra outputs.
    Remarks = 11,      ///< IDxcBlobUtf8 or IDxcBlobWide - text directed at stdout.
    TimeReport = 12,   ///< IDxcBlobUtf8 or IDxcBlobWide - text directed at stdout.
    TimeTrace = 13,    ///< IDxcBlobUtf8 or IDxcBlobWide - text directed at stdout.
}