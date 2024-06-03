using System.Runtime.InteropServices;

namespace Glslang.NET;


public struct CompilationInput
{
    public SourceType language;
    public ShaderStage stage;
    public ClientType client;
    public TargetClientVersion clientVersion;
    public TargetLanguage targetLanguage;
    public TargetLanguageVersion targetLanguageVersion;
    public string code;
    public string? entrypoint;
    public string? sourceEntrypoint;
    public bool invertY;
    public int defaultVersion;
    public ShaderProfile defaultProfile;
    public bool forceDefaultVersionAndProfile;
    public bool forwardCompatible;
    public MessageType messages;
    public ResourceLimits resourceLimits;
    public FileIncluder fileIncluder;
}


public struct IncludeResult
{
    public string headerName;
    public string headerData;
}


public delegate IncludeResult FileIncluder(string headerName, string includerName, uint includeDepth, bool isSystemFile);