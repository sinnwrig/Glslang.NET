namespace Glslang.NET;


/// <summary>
/// The input arguments to a Shader instance.
/// </summary>
public struct CompilationInput
{
    /// <summary>
    /// The language of the source code.
    /// </summary>
    public SourceType language;

    /// <summary>
    /// The target shader stage.
    /// </summary>
    public ShaderStage stage;

    /// <summary>
    /// The target client.
    /// </summary>
    public ClientType client;

    /// <summary>
    /// The target client version.
    /// </summary>
    public TargetClientVersion clientVersion;

    /// <summary>
    /// The target language to output to.
    /// </summary>
    public TargetLanguage targetLanguage;

    /// <summary>
    /// The target language version to output to.
    /// </summary>
    public TargetLanguageVersion targetLanguageVersion;

    /// <summary>
    /// The shader source code.
    /// </summary>
    public string code;

    /// <summary>
    /// The binary shader entrypoint.
    /// </summary>
    /// <remarks>
    /// Defaults to 'main'. Prefer using `sourceEntrypoint` over `entrypoint` as changing the binary entrypoint may cause backends which default to `main` as an entrypoint to become confused, such as OpenGL. 
    /// </remarks>
    public string? entrypoint;

    /// <summary>
    /// The source-code entrypoint to use.
    /// </summary>
    /// <remarks>
    /// Defaults to 'main'. Changes the name of the source-code entrypoint to match the binary `entrypoint`. 
    /// This allows setting custom entrypoint names while ensuring consistent behavior in how different shaders are treated.
    /// </remarks>
    public string? sourceEntrypoint;

    /// <summary>
    /// Invert the vertex shader's Y coordinate.
    /// </summary>
    public bool invertY;

    /// <summary>
    /// Enable additional HLSL functionality in the SPIR-V output.
    /// </summary>
    public bool hlslFunctionality1;

    /// <summary>
    /// The default GLSL version to use if no `#version` is specified
    /// </summary>
    public int defaultVersion;

    /// <summary>
    /// The default profile to use.
    /// </summary>
    public ShaderProfile defaultProfile;

    /// <summary>
    /// Forces shaders to use the default version and profile, disregarding `#version` definitions.
    /// </summary>
    public bool forceDefaultVersionAndProfile;

    /// <summary>
    /// Disallow support for deprecated GLSL features.
    /// </summary>
    public bool forwardCompatible;

    /// <summary>
    /// Enabled shader log and debug message outputs.
    /// </summary>
    public MessageType? messages;

    /// <summary>
    /// The resource limits imposed on a shader.
    /// </summary>
    /// <remarks>
    /// Defaults to ResourceLimits.DefaultResource.
    /// </remarks>
    public ResourceLimits? resourceLimits;

    /// <summary>
    /// Overrides default file inclusion behavior when the compiler encounters an `#include` directive.
    /// </summary>
    public FileIncluder? fileIncluder;
}


/// <summary>
/// The result ouput returned from an include operation.
/// </summary>
public struct IncludeResult
{
    /// <summary>
    /// The name of the included file. 
    /// </summary>
    /// <remarks>
    /// Empty or null values will end the file inclusion process.
    /// </remarks>
    public string headerName;

    /// <summary>
    /// The contents of the included file.
    /// </summary>
    /// <remarks>
    /// Empty or null values will end the file inclusion process.
    /// </remarks>
    public string headerData;
}


/// <summary>
/// Override file inclusion behavior for `#include` directives.
/// </summary>
/// <param name="headerName">The header file to include.</param>
/// <param name="includerName">The source file which the `#include` directive was found in.</param>
/// <param name="includeDepth">The current depth of includes.</param>
/// <param name="isSystemFile">If the filename was encased in system (&lt;&gt;) quotes or local ("") quotes.</param>
public delegate IncludeResult FileIncluder(string headerName, string includerName, uint includeDepth, bool isSystemFile);