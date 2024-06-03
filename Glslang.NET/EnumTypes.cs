namespace Glslang.NET;


public enum ShaderStage 
{
    Vertex,
    TessControl,
    TessEvaluation,
    Geometry,
    Fragment,
    Compute,
    Raygen,
    Intersect,
    AnyHit,
    ClosestHit,
    Miss,
    Callable,
    Task,
    Mesh,
}


public enum LanguageMask 
{
    VertexMask = 1 << ShaderStage.Vertex,
    TessControlMask = 1 << ShaderStage.TessControl,
    TessEvaluationMask = 1 << ShaderStage.TessEvaluation,
    GeometryMask = 1 << ShaderStage.Geometry,
    FragmentMask = 1 << ShaderStage.Fragment,
    ComputeMask = 1 << ShaderStage.Compute,
    RaygenMask = 1 << ShaderStage.Raygen,
    IntersectMask = 1 << ShaderStage.Intersect,
    AnyHitMask = 1 << ShaderStage.AnyHit,
    ClosestHitMask = 1 << ShaderStage.ClosestHit,
    MissMask = 1 << ShaderStage.Miss,
    CallableMask = 1 << ShaderStage.Callable,
    TaskMask = 1 << ShaderStage.Task,
    MeshMask = 1 << ShaderStage.Mesh,
}


public enum SourceType 
{
    None,
    GLSL,
    HLSL,
}


public enum ClientType 
{
    None,
    Vulkan,
    OpenGL,
}


public enum TargetLanguage 
{
    None,
    SPV,
}


public enum TargetClientVersion 
{
    Vulkan_1_0 = 1 << 22,
    Vulkan_1_1 = (1 << 22) | (1 << 12),
    Vulkan_1_2 = (1 << 22) | (2 << 12),
    Vulkan_1_3 = (1 << 22) | (3 << 12),
    OpenGL_450 = 450,
}


public enum TargetLanguageVersion 
{
    SPV_1_0 = 1 << 16,
    SPV_1_1 = (1 << 16) | (1 << 8),
    SPV_1_2 = (1 << 16) | (2 << 8),
    SPV_1_3 = (1 << 16) | (3 << 8),
    SPV_1_4 = (1 << 16) | (4 << 8),
    SPV_1_5 = (1 << 16) | (5 << 8),
    SPV_1_6 = (1 << 16) | (6 << 8),
}


public enum ExecutableType 
{ 
    VertexFragment, 
    Fragment 
}


public enum TextureSamplerTransformMode 
{
    Keep,
    UpgradeTextureRemoveSampler,
}


[Flags]
public enum MessageType 
{
    Default                 = 0,
    RelaxedErrors           = 1 << 0,
    SuppressWarnings        = 1 << 1,
    AST                     = 1 << 2,
    SPVRules                = 1 << 3,
    VulkanRules             = 1 << 4,
    OnlyPreprocessor        = 1 << 5,
    ReadHLSL                = 1 << 6,
    CascadingErrors         = 1 << 7,
    KeepUncalled            = 1 << 8,
    HLSLOffsets             = 1 << 9,
    DebugInfo               = 1 << 10,
    Enable16BitHLSLTypes    = 1 << 11,
    LegalizeHLSL            = 1 << 12,
    DX9CompatibleHLSL       = 1 << 13,
    BuiltinSymbolTable      = 1 << 14,
    Enhanced                = 1 << 15,
    AbsolutePath            = 1 << 16,
}


[Flags]
public enum ReflectionOptions 
{
    Default = 0,
    StrictArraySuffix = 1 << 0,
    BasicArraySuffix = 1 << 1,
    IntermediateIOO = 1 << 2,
    SeparateBuffers = 1 << 3,
    AllBlockVariables = 1 << 4,
    UnwrapIOBlocks = 1 << 5,
    AllIOVariables = 1 << 6,
    SharedSTD140_SSBO = 1 << 7,
    SharedSTD140_UBO = 1 << 8,
}


public enum ShaderProfile 
{
    Bad = 0,
    None = 1 << 0,
    CoreProfile = 1 << 1,
    Compatibility = 1 << 2,
    ES = 1 << 3,
}


public enum ShaderOptions
{
    Default = 0,
    AutoMapBindings = 1 << 0,
    AutoMapLocations = 1 << 1,
    VulkanRulesRelaxed = 1 << 2,
} 


public enum ResourceType 
{
    Sampler,
    Texture,
    Image,
    UBO,
    SSBO,
    UAV,
}
