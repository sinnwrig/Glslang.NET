namespace Glslang.NET;


/// <summary>
/// Target shader pipeline stages. 
/// </summary>
public enum ShaderStage 
{
    /// <summary></summary>
    Vertex,

    /// <summary></summary>
    TessControl,

    /// <summary></summary>
    TessEvaluation,

    /// <summary></summary>
    Geometry,

    /// <summary></summary>
    Fragment,

    /// <summary></summary>
    Compute,

    /// <summary></summary>
    Raygen,

    /// <summary></summary>
    Intersect,

    /// <summary></summary>
    AnyHit,

    /// <summary></summary>
    ClosestHit,

    /// <summary></summary>
    Miss,

    /// <summary></summary>
    Callable,

    /// <summary></summary>
    Task,

    /// <summary></summary>
    Mesh,
}


/// <summary></summary>
public enum LanguageMask 
{
    /// <summary></summary>
    VertexMask = 1 << ShaderStage.Vertex,

    /// <summary></summary>
    TessControlMask = 1 << ShaderStage.TessControl,

    /// <summary></summary>
    TessEvaluationMask = 1 << ShaderStage.TessEvaluation,

    /// <summary></summary>
    GeometryMask = 1 << ShaderStage.Geometry,

    /// <summary></summary>
    FragmentMask = 1 << ShaderStage.Fragment,

    /// <summary></summary>
    ComputeMask = 1 << ShaderStage.Compute,

    /// <summary></summary>
    RaygenMask = 1 << ShaderStage.Raygen,

    /// <summary></summary>
    IntersectMask = 1 << ShaderStage.Intersect,

    /// <summary></summary>
    AnyHitMask = 1 << ShaderStage.AnyHit,

    /// <summary></summary>
    ClosestHitMask = 1 << ShaderStage.ClosestHit,

    /// <summary></summary>
    MissMask = 1 << ShaderStage.Miss,

    /// <summary></summary>
    CallableMask = 1 << ShaderStage.Callable,

    /// <summary></summary>
    TaskMask = 1 << ShaderStage.Task,

    /// <summary></summary>
    MeshMask = 1 << ShaderStage.Mesh,
}


/// <summary>
/// Shader source code language types.
/// </summary>
public enum SourceType 
{
    /// <summary>
    /// Unknown language type
    /// </summary>
    None,

    /// <summary>
    /// OpenGL Shading Sanguage.
    /// </summary>
    GLSL,

    /// <summary>
    /// High Level Shading Language
    /// </summary>
    HLSL,
}


/// <summary>
/// Shader target client type.
/// </summary>
public enum ClientType 
{
    /// <summary>
    /// Unknown client type.
    /// </summary>
    None,

    /// <summary>
    /// Vulkan client type.
    /// </summary>
    Vulkan,

    /// <summary>
    /// OpenGL client type.
    /// </summary>
    OpenGL,
}


/// <summary>
/// Shader target language type.
/// </summary>
public enum TargetLanguage 
{
    /// <summary>
    /// Unknown language type.
    /// </summary>
    None,

    /// <summary>
    /// SPIR-V language type.
    /// </summary>
    SPV,
}


/// <summary>
/// Shader target client version.
/// </summary>
public enum TargetClientVersion 
{
    /// <summary>
    /// Vulkan version 1.0
    /// </summary>
    Vulkan_1_0 = 1 << 22,

    /// <summary>
    /// Vulkan version 1.1
    /// </summary>
    Vulkan_1_1 = (1 << 22) | (1 << 12),

    /// <summary>
    /// Vulkan version 1.2
    /// </summary>
    Vulkan_1_2 = (1 << 22) | (2 << 12),

    /// <summary>
    /// Vulkan version 1.3
    /// </summary>
    Vulkan_1_3 = (1 << 22) | (3 << 12),

    /// <summary>
    /// OpenGL version 450
    /// </summary>
    OpenGL_450 = 450,
}


/// <summary>
/// Shader target language version.
/// </summary>
public enum TargetLanguageVersion 
{
    /// <summary>
    /// SPIR-V version 1.0
    /// </summary>
    SPV_1_0 = 1 << 16,

    /// <summary>
    /// SPIR-V version 1.1
    /// </summary>
    SPV_1_1 = (1 << 16) | (1 << 8),
    
    /// <summary>
    /// SPIR-V version 1.2
    /// </summary>
    SPV_1_2 = (1 << 16) | (2 << 8),

    /// <summary>
    /// SPIR-V version 1.3
    /// </summary>
    SPV_1_3 = (1 << 16) | (3 << 8),

    /// <summary>
    /// SPIR-V version 1.4
    /// </summary>
    SPV_1_4 = (1 << 16) | (4 << 8),
    
    /// <summary>
    /// SPIR-V version 1.5
    /// </summary>
    SPV_1_5 = (1 << 16) | (5 << 8),
    
    /// <summary>
    /// SPIR-V version 1.6
    /// </summary>
    SPV_1_6 = (1 << 16) | (6 << 8),
}


/// <summary></summary>
public enum ExecutableType 
{ 
    /// <summary></summary>
    VertexFragment, 

    /// <summary></summary>
    Fragment 
}


/// <summary></summary>
public enum TextureSamplerTransformMode 
{
    /// <summary></summary>
    Keep,

    /// <summary></summary>
    UpgradeTextureRemoveSampler,
}


/// <summary>
/// Bit-flag of shader compilation message types.
/// </summary>
[Flags]
public enum MessageType 
{
    /// <summary></summary>
    Default                 = 0,

    /// <summary></summary>
    RelaxedErrors           = 1 << 0,

    /// <summary></summary>
    SuppressWarnings        = 1 << 1,

    /// <summary></summary>
    AST                     = 1 << 2,
    
    /// <summary></summary>
    SPVRules                = 1 << 3,

    /// <summary></summary>
    VulkanRules             = 1 << 4,

    /// <summary></summary>
    OnlyPreprocessor        = 1 << 5,

    /// <summary></summary>
    ReadHLSL                = 1 << 6,

    /// <summary></summary>
    CascadingErrors         = 1 << 7,

    /// <summary></summary>
    KeepUncalled            = 1 << 8,

    /// <summary></summary>
    HLSLOffsets             = 1 << 9,

    /// <summary></summary>
    DebugInfo               = 1 << 10,

    /// <summary></summary>
    Enable16BitHLSLTypes    = 1 << 11,

    /// <summary></summary>
    LegalizeHLSL            = 1 << 12,

    /// <summary></summary>
    DX9CompatibleHLSL       = 1 << 13,

    /// <summary></summary>
    BuiltinSymbolTable      = 1 << 14,

    /// <summary></summary>
    Enhanced                = 1 << 15,

    /// <summary></summary>
    AbsolutePath            = 1 << 16,
}


/// <summary></summary>
[Flags]
public enum ReflectionOptions 
{
    /// <summary></summary>
    Default = 0,

    /// <summary></summary>
    StrictArraySuffix = 1 << 0,

    /// <summary></summary>
    BasicArraySuffix = 1 << 1,

    /// <summary></summary>
    IntermediateIOO = 1 << 2,

    /// <summary></summary>
    SeparateBuffers = 1 << 3,

    /// <summary></summary>
    AllBlockVariables = 1 << 4,

    /// <summary></summary>
    UnwrapIOBlocks = 1 << 5,

    /// <summary></summary>
    AllIOVariables = 1 << 6,
    
    /// <summary></summary>
    SharedSTD140_SSBO = 1 << 7,

    /// <summary></summary>
    SharedSTD140_UBO = 1 << 8,
}


/// <summary></summary>
public enum ShaderProfile 
{
    /// <summary></summary>
    Bad = 0,

    /// <summary></summary>
    None = 1 << 0,

    /// <summary></summary>
    CoreProfile = 1 << 1,

    /// <summary></summary>
    Compatibility = 1 << 2,

    /// <summary></summary>
    ES = 1 << 3,
}


/// <summary></summary>
public enum ShaderOptions
{
    /// <summary></summary>
    Default = 0,

    /// <summary></summary>
    AutoMapBindings = 1 << 0,

    /// <summary></summary>
    AutoMapLocations = 1 << 1,

    /// <summary></summary>
    VulkanRulesRelaxed = 1 << 2,
} 


/// <summary></summary>
public enum ResourceType 
{
    /// <summary></summary>
    Sampler,
    /// <summary></summary>
    Texture,
    /// <summary></summary>
    Image,
    /// <summary></summary>
    UBO,
    /// <summary></summary>
    SSBO,
    /// <summary></summary>
    UAV,
}
