namespace Glslang;

// ---------------------------------------------------
// ----------------Enum Definitions-------------------
// ---------------------------------------------------

/* EShLanguage counterpart */
public enum ShaderStage 
{
    Vertex,
    TessControl,
    TessEvaluation,
    Geometry,
    Fragment,
    Compute,
    Raygen,
    RaygenNv = Raygen,
    Intersect,
    IntersectNv = Intersect,
    AnyHit,
    AnyHitNv = AnyHit,
    ClosestHit,
    ClosestHitNv = ClosestHit,
    Miss,
    MissNv = Miss,
    Callable,
    CallableNv = Callable,
    Task,
    TaskNv = Task,
    Mesh,
    MeshNv = Mesh,
}


[Flags]
public enum ShaderStageMask
{
    Vertex = 1 << ShaderStage.Vertex,
    TessControl = 1 << ShaderStage.TessControl,
    TessEvaluation = 1 << ShaderStage.TessEvaluation,
    Geometry = 1 << ShaderStage.Geometry,
    Fragment = 1 << ShaderStage.Fragment,
    Compute = 1 << ShaderStage.Compute,
    Raygen = 1 << ShaderStage.Raygen,
    RaygenNv = Raygen,
    Intersect = 1 << ShaderStage.Intersect,
    IntersectNv = Intersect,
    AnyHit = 1 << ShaderStage.AnyHit,
    AnyHitNv = AnyHit,
    ClosestHit = 1 << ShaderStage.ClosestHit,
    ClosestHitNv = ClosestHit,
    Miss = 1 << ShaderStage.Miss,
    MissNv = Miss,
    Callable = 1 << ShaderStage.Callable,
    CallableNv = Callable,
    Task = 1 << ShaderStage.Task,
    TaskNv = Task,
    Mesh = 1 << ShaderStage.Mesh,
    MeshNv = Mesh,
}


public enum ShaderSource 
{
    None,
    GLSL,
    HLSL,
}


public enum ShaderClient
{
    None,
    Vulkan,
    OpenGL,
}


public enum TargetLanguage 
{
    None,
    SpirV
}


public enum ClientVersion 
{
    Vulkan_1_0 = 1 << 22,
    Vulkan_1_1 = (1 << 22) | (1 << 12),
    Vulkan_1_2 = (1 << 22) | (2 << 12),
    Vulkan_1_3 = (1 << 22) | (3 << 12),
    OpenGL_450 = 450,
}


public enum TargetLanguageVersion 
{
    SpirV_1_0 = 1 << 16,
    SpirV_1_1 = (1 << 16) | (1 << 8),
    SpirV_1_2 = (1 << 16) | (2 << 8),
    SpirV_1_3 = (1 << 16) | (3 << 8),
    SpirV_1_4 = (1 << 16) | (4 << 8),
    SpirV_1_5 = (1 << 16) | (5 << 8),
    SpirV_1_6 = (1 << 16) | (6 << 8),
}


public enum ExecutableType 
{ 
    VertexFragment, 
    Fragment
}


public enum OptimizationLevel
{
    NoGeneration,
    None,
    Simple,
    Full,
}


public enum TextureSamplerTransformMode
{
    Keep,
    UpgradeTextureRemoveSampler,
}


[Flags]
public enum Messages 
{
    Default = 0,
    RelaxedErrors = 1 << 0,
    SuppressWarnings = 1 << 1,
    Ast = 1 << 2,
    SpvRules = 1 << 3,
    VulkanRules = 1 << 4,
    OonlyPreprocessor = 1 << 5,
    ReadHlsl = 1 << 6,
    CascadingErrors = 1 << 7,
    KeepUncalled = 1 << 8,
    HLSL_Offsets = 1 << 9,
    DebugInfo = 1 << 10,
    HLSL_Enable16BitTypes = 1 << 11,
    HLSL_Legalization = 1 << 12,
    HLSL_DX9Compatible = 1 << 13,
    BuiltinSymbolTable = 1 << 14,
    Enhanced = 1 << 15,
}


[Flags]
public enum ReflectionOptions 
{
    Default = 0,
    StrictArraySuffix = 1 << 0,
    BasicArraySuffix = 1 << 1,
    IntermediateIoo = 1 << 2,
    SeparateBuffers = 1 << 3,
    AllBlockVariables = 1 << 4,
    UnwrapIOBlocks = 1 << 5,
    AllIOVariables = 1 << 6,
    SharedStd10_UBO = 1 << 7,
    SharedStd140_UBO = 1 << 8,
}


[Flags]
public enum ShaderProfile 
{
    Bad = 0,
    None = 1 << 0,
    Core = 1 << 1,
    Compatibility = 1 << 2,
    ES = 1 << 3,
}


[Flags]
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