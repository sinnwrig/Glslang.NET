using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary></summary>
[StructLayout(LayoutKind.Sequential)]
public struct ShaderLimits
{
    /// <summary></summary>
    public bool nonInductiveForLoops;

    /// <summary></summary>
    public bool whileLoops;

    /// <summary></summary>
    public bool doWhileLoops;

    /// <summary></summary>
    public bool generalUniformIndexing;

    /// <summary></summary>
    public bool generalAttributeMatrixVectorIndexing;

    /// <summary></summary>
    public bool generalVaryingIndexing;

    /// <summary></summary>
    public bool generalSamplerIndexing;

    /// <summary></summary>
    public bool generalVariableIndexing;

    /// <summary></summary>
    public bool generalConstantMatrixVectorIndexing;


    /// <summary>
    /// Default shader limits.
    /// </summary>
    public static readonly ShaderLimits DefaultLimits = new()
    {
        nonInductiveForLoops = true,
        whileLoops = true,
        doWhileLoops = true,
        generalUniformIndexing = true,
        generalAttributeMatrixVectorIndexing = true,
        generalVaryingIndexing = true,
        generalSamplerIndexing = true,
        generalVariableIndexing = true,
        generalConstantMatrixVectorIndexing = true,
    };
}


/// <summary></summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ResourceLimits
{

    /// <summary></summary>
    public int maxLights;

    /// <summary></summary>
    public int maxClipPlanes;

    /// <summary></summary>
    public int maxTextureUnits;

    /// <summary></summary>
    public int maxTextureCoords;

    /// <summary></summary>
    public int maxVertexAttribs;

    /// <summary></summary>
    public int maxVertexUniformComponents;

    /// <summary></summary>
    public int maxVaryingFloats;

    /// <summary></summary>
    public int maxVertexTextureImageUnits;

    /// <summary></summary>
    public int maxCombinedTextureImageUnits;

    /// <summary></summary>
    public int maxTextureImageUnits;

    /// <summary></summary>
    public int maxFragmentUniformComponents;

    /// <summary></summary>
    public int maxDrawBuffers;

    /// <summary></summary>
    public int maxVertexUniformVectors;

    /// <summary></summary>
    public int maxVaryingVectors;

    /// <summary></summary>
    public int maxFragmentUniformVectors;

    /// <summary></summary>
    public int maxVertexOutputVectors;

    /// <summary></summary>
    public int maxFragmentInputVectors;

    /// <summary></summary>
    public int minProgramTexelOffset;

    /// <summary></summary>
    public int maxProgramTexelOffset;

    /// <summary></summary>
    public int maxClipDistances;

    /// <summary></summary>
    public int maxComputeWorkGroupCountX;

    /// <summary></summary>
    public int maxComputeWorkGroupCountY;

    /// <summary></summary>
    public int maxComputeWorkGroupCountZ;

    /// <summary></summary>
    public int maxComputeWorkGroupSizeX;

    /// <summary></summary>
    public int maxComputeWorkGroupSizeY;

    /// <summary></summary>
    public int maxComputeWorkGroupSizeZ;

    /// <summary></summary>
    public int maxComputeUniformComponents;

    /// <summary></summary>
    public int maxComputeTextureImageUnits;

    /// <summary></summary>
    public int maxComputeImageUniforms;

    /// <summary></summary>
    public int maxComputeAtomicCounters;

    /// <summary></summary>
    public int maxComputeAtomicCounterBuffers;

    /// <summary></summary>
    public int maxVaryingComponents;

    /// <summary></summary>
    public int maxVertexOutputComponents;

    /// <summary></summary>
    public int maxGeometryInputComponents;

    /// <summary></summary>
    public int maxGeometryOutputComponents;

    /// <summary></summary>
    public int maxFragmentInputComponents;

    /// <summary></summary>
    public int maxImageUnits;

    /// <summary></summary>
    public int maxCombinedImageUnitsAndFragmentOutputs;

    /// <summary></summary>
    public int maxCombinedShaderOutputResources;

    /// <summary></summary>
    public int maxImageSamples;

    /// <summary></summary>
    public int maxVertexImageUniforms;

    /// <summary></summary>
    public int maxTessControlImageUniforms;

    /// <summary></summary>
    public int maxTessEvaluationImageUniforms;

    /// <summary></summary>
    public int maxGeometryImageUniforms;

    /// <summary></summary>
    public int maxFragmentImageUniforms;

    /// <summary></summary>
    public int maxCombinedImageUniforms;

    /// <summary></summary>
    public int maxGeometryTextureImageUnits;

    /// <summary></summary>
    public int maxGeometryOutputVertices;

    /// <summary></summary>
    public int maxGeometryTotalOutputComponents;

    /// <summary></summary>
    public int maxGeometryUniformComponents;

    /// <summary></summary>
    public int maxGeometryVaryingComponents;

    /// <summary></summary>
    public int maxTessControlInputComponents;

    /// <summary></summary>
    public int maxTessControlOutputComponents;

    /// <summary></summary>
    public int maxTessControlTextureImageUnits;

    /// <summary></summary>
    public int maxTessControlUniformComponents;

    /// <summary></summary>
    public int maxTessControlTotalOutputComponents;

    /// <summary></summary>
    public int maxTessEvaluationInputComponents;

    /// <summary></summary>
    public int maxTessEvaluationOutputComponents;

    /// <summary></summary>
    public int maxTessEvaluationTextureImageUnits;

    /// <summary></summary>
    public int maxTessEvaluationUniformComponents;

    /// <summary></summary>
    public int maxTessPatchComponents;

    /// <summary></summary>
    public int maxPatchVertices;

    /// <summary></summary>
    public int maxTessGenLevel;

    /// <summary></summary>
    public int maxViewports;

    /// <summary></summary>
    public int maxVertexAtomicCounters;

    /// <summary></summary>
    public int maxTessControlAtomicCounters;

    /// <summary></summary>
    public int maxTessEvaluationAtomicCounters;

    /// <summary></summary>
    public int maxGeometryAtomicCounters;

    /// <summary></summary>
    public int maxFragmentAtomicCounters;

    /// <summary></summary>
    public int maxCombinedAtomicCounters;

    /// <summary></summary>
    public int maxAtomicCounterBindings;

    /// <summary></summary>
    public int maxVertexAtomicCounterBuffers;

    /// <summary></summary>
    public int maxTessControlAtomicCounterBuffers;

    /// <summary></summary>
    public int maxTessEvaluationAtomicCounterBuffers;

    /// <summary></summary>
    public int maxGeometryAtomicCounterBuffers;

    /// <summary></summary>
    public int maxFragmentAtomicCounterBuffers;

    /// <summary></summary>
    public int maxCombinedAtomicCounterBuffers;

    /// <summary></summary>
    public int maxAtomicCounterBufferSize;

    /// <summary></summary>
    public int maxTransformFeedbackBuffers;

    /// <summary></summary>
    public int maxTransformFeedbackInterleavedComponents;

    /// <summary></summary>
    public int maxCullDistances;

    /// <summary></summary>
    public int maxCombinedClipAndCullDistances;

    /// <summary></summary>
    public int maxSamples;

    /// <summary></summary>
    public int maxMeshOutputVerticesNV;

    /// <summary></summary>
    public int maxMeshOutputPrimitivesNV;

    /// <summary></summary>
    public int maxMeshWorkGroupSizeX_NV;

    /// <summary></summary>
    public int maxMeshWorkGroupSizeY_NV;

    /// <summary></summary>
    public int maxMeshWorkGroupSizeZ_NV;

    /// <summary></summary>
    public int maxTaskWorkGroupSizeX_NV;

    /// <summary></summary>
    public int maxTaskWorkGroupSizeY_NV;

    /// <summary></summary>
    public int maxTaskWorkGroupSizeZ_NV;

    /// <summary></summary>
    public int maxMeshViewCountNV;

    /// <summary></summary>
    public int maxMeshOutputVerticesEXT;

    /// <summary></summary>
    public int maxMeshOutputPrimitivesEXT;

    /// <summary></summary>
    public int maxMeshWorkGroupSizeX_EXT;

    /// <summary></summary>
    public int maxMeshWorkGroupSizeY_EXT;

    /// <summary></summary>
    public int maxMeshWorkGroupSizeZ_EXT;

    /// <summary></summary>
    public int maxTaskWorkGroupSizeX_EXT;

    /// <summary></summary>
    public int maxTaskWorkGroupSizeY_EXT;

    /// <summary></summary>
    public int maxTaskWorkGroupSizeZ_EXT;

    /// <summary></summary>
    public int maxMeshViewCountEXT;

    /// <summary></summary>
    public int maxDualSourceDrawBuffersEXT;

    /// <summary></summary>
    public ShaderLimits limits;


    internal readonly ResourceLimits* Allocate()
    {
        ResourceLimits* limits = GlslangNative.Allocate<ResourceLimits>();

        limits->maxLights = maxLights;
        limits->maxClipPlanes = maxClipPlanes;
        limits->maxTextureUnits = maxTextureUnits;
        limits->maxTextureCoords = maxTextureCoords;
        limits->maxVertexAttribs = maxVertexAttribs;
        limits->maxVertexUniformComponents = maxVertexUniformComponents;
        limits->maxVaryingFloats = maxVaryingFloats;
        limits->maxVertexTextureImageUnits = maxVertexTextureImageUnits;
        limits->maxCombinedTextureImageUnits = maxCombinedTextureImageUnits;
        limits->maxTextureImageUnits = maxTextureImageUnits;
        limits->maxFragmentUniformComponents = maxFragmentUniformComponents;
        limits->maxDrawBuffers = maxDrawBuffers;
        limits->maxVertexUniformVectors = maxVertexUniformVectors;
        limits->maxVaryingVectors = maxVaryingVectors;
        limits->maxFragmentUniformVectors = maxFragmentUniformVectors;
        limits->maxVertexOutputVectors = maxVertexOutputVectors;
        limits->maxFragmentInputVectors = maxFragmentInputVectors;
        limits->minProgramTexelOffset = minProgramTexelOffset;
        limits->maxProgramTexelOffset = maxProgramTexelOffset;
        limits->maxClipDistances = maxClipDistances;
        limits->maxComputeWorkGroupCountX = maxComputeWorkGroupCountX;
        limits->maxComputeWorkGroupCountY = maxComputeWorkGroupCountY;
        limits->maxComputeWorkGroupCountZ = maxComputeWorkGroupCountZ;
        limits->maxComputeWorkGroupSizeX = maxComputeWorkGroupSizeX;
        limits->maxComputeWorkGroupSizeY = maxComputeWorkGroupSizeY;
        limits->maxComputeWorkGroupSizeZ = maxComputeWorkGroupSizeZ;
        limits->maxComputeUniformComponents = maxComputeUniformComponents;
        limits->maxComputeTextureImageUnits = maxComputeTextureImageUnits;
        limits->maxComputeImageUniforms = maxComputeImageUniforms;
        limits->maxComputeAtomicCounters = maxComputeAtomicCounters;
        limits->maxComputeAtomicCounterBuffers = maxComputeAtomicCounterBuffers;
        limits->maxVaryingComponents = maxVaryingComponents;
        limits->maxVertexOutputComponents = maxVertexOutputComponents;
        limits->maxGeometryInputComponents = maxGeometryInputComponents;
        limits->maxGeometryOutputComponents = maxGeometryOutputComponents;
        limits->maxFragmentInputComponents = maxFragmentInputComponents;
        limits->maxImageUnits = maxImageUnits;
        limits->maxCombinedImageUnitsAndFragmentOutputs = maxCombinedImageUnitsAndFragmentOutputs;
        limits->maxCombinedShaderOutputResources = maxCombinedShaderOutputResources;
        limits->maxImageSamples = maxImageSamples;
        limits->maxVertexImageUniforms = maxVertexImageUniforms;
        limits->maxTessControlImageUniforms = maxTessControlImageUniforms;
        limits->maxTessEvaluationImageUniforms = maxTessEvaluationImageUniforms;
        limits->maxGeometryImageUniforms = maxGeometryImageUniforms;
        limits->maxFragmentImageUniforms = maxFragmentImageUniforms;
        limits->maxCombinedImageUniforms = maxCombinedImageUniforms;
        limits->maxGeometryTextureImageUnits = maxGeometryTextureImageUnits;
        limits->maxGeometryOutputVertices = maxGeometryOutputVertices;
        limits->maxGeometryTotalOutputComponents = maxGeometryTotalOutputComponents;
        limits->maxGeometryUniformComponents = maxGeometryUniformComponents;
        limits->maxGeometryVaryingComponents = maxGeometryVaryingComponents;
        limits->maxTessControlInputComponents = maxTessControlInputComponents;
        limits->maxTessControlOutputComponents = maxTessControlOutputComponents;
        limits->maxTessControlTextureImageUnits = maxTessControlTextureImageUnits;
        limits->maxTessControlUniformComponents = maxTessControlUniformComponents;
        limits->maxTessControlTotalOutputComponents = maxTessControlTotalOutputComponents;
        limits->maxTessEvaluationInputComponents = maxTessEvaluationInputComponents;
        limits->maxTessEvaluationOutputComponents = maxTessEvaluationOutputComponents;
        limits->maxTessEvaluationTextureImageUnits = maxTessEvaluationTextureImageUnits;
        limits->maxTessEvaluationUniformComponents = maxTessEvaluationUniformComponents;
        limits->maxTessPatchComponents = maxTessPatchComponents;
        limits->maxPatchVertices = maxPatchVertices;
        limits->maxTessGenLevel = maxTessGenLevel;
        limits->maxViewports = maxViewports;
        limits->maxVertexAtomicCounters = maxVertexAtomicCounters;
        limits->maxTessControlAtomicCounters = maxTessControlAtomicCounters;
        limits->maxTessEvaluationAtomicCounters = maxTessEvaluationAtomicCounters;
        limits->maxGeometryAtomicCounters = maxGeometryAtomicCounters;
        limits->maxFragmentAtomicCounters = maxFragmentAtomicCounters;
        limits->maxCombinedAtomicCounters = maxCombinedAtomicCounters;
        limits->maxAtomicCounterBindings = maxAtomicCounterBindings;
        limits->maxVertexAtomicCounterBuffers = maxVertexAtomicCounterBuffers;
        limits->maxTessControlAtomicCounterBuffers = maxTessControlAtomicCounterBuffers;
        limits->maxTessEvaluationAtomicCounterBuffers = maxTessEvaluationAtomicCounterBuffers;
        limits->maxGeometryAtomicCounterBuffers = maxGeometryAtomicCounterBuffers;
        limits->maxFragmentAtomicCounterBuffers = maxFragmentAtomicCounterBuffers;
        limits->maxCombinedAtomicCounterBuffers = maxCombinedAtomicCounterBuffers;
        limits->maxAtomicCounterBufferSize = maxAtomicCounterBufferSize;
        limits->maxTransformFeedbackBuffers = maxTransformFeedbackBuffers;
        limits->maxTransformFeedbackInterleavedComponents = maxTransformFeedbackInterleavedComponents;
        limits->maxCullDistances = maxCullDistances;
        limits->maxCombinedClipAndCullDistances = maxCombinedClipAndCullDistances;
        limits->maxSamples = maxSamples;
        limits->maxMeshOutputVerticesNV = maxMeshOutputVerticesNV;
        limits->maxMeshOutputPrimitivesNV = maxMeshOutputPrimitivesNV;
        limits->maxMeshWorkGroupSizeX_NV = maxMeshWorkGroupSizeX_NV;
        limits->maxMeshWorkGroupSizeY_NV = maxMeshWorkGroupSizeY_NV;
        limits->maxMeshWorkGroupSizeZ_NV = maxMeshWorkGroupSizeZ_NV;
        limits->maxTaskWorkGroupSizeX_NV = maxTaskWorkGroupSizeX_NV;
        limits->maxTaskWorkGroupSizeY_NV = maxTaskWorkGroupSizeY_NV;
        limits->maxTaskWorkGroupSizeZ_NV = maxTaskWorkGroupSizeZ_NV;
        limits->maxMeshViewCountNV = maxMeshViewCountNV;
        limits->maxMeshOutputVerticesEXT = maxMeshOutputVerticesEXT;
        limits->maxMeshOutputPrimitivesEXT = maxMeshOutputPrimitivesEXT;
        limits->maxMeshWorkGroupSizeX_EXT = maxMeshWorkGroupSizeX_EXT;
        limits->maxMeshWorkGroupSizeY_EXT = maxMeshWorkGroupSizeY_EXT;
        limits->maxMeshWorkGroupSizeZ_EXT = maxMeshWorkGroupSizeZ_EXT;
        limits->maxTaskWorkGroupSizeX_EXT = maxTaskWorkGroupSizeX_EXT;
        limits->maxTaskWorkGroupSizeY_EXT = maxTaskWorkGroupSizeY_EXT;
        limits->maxTaskWorkGroupSizeZ_EXT = maxTaskWorkGroupSizeZ_EXT;
        limits->maxMeshViewCountEXT = maxMeshViewCountEXT;
        limits->maxDualSourceDrawBuffersEXT = maxDualSourceDrawBuffersEXT;

        limits->limits.nonInductiveForLoops = this.limits.nonInductiveForLoops;
        limits->limits.whileLoops = this.limits.whileLoops;
        limits->limits.doWhileLoops = this.limits.doWhileLoops;
        limits->limits.generalUniformIndexing = this.limits.generalUniformIndexing;
        limits->limits.generalAttributeMatrixVectorIndexing = this.limits.generalAttributeMatrixVectorIndexing;
        limits->limits.generalVaryingIndexing = this.limits.generalVaryingIndexing;
        limits->limits.generalSamplerIndexing = this.limits.generalSamplerIndexing;
        limits->limits.generalVariableIndexing = this.limits.generalVariableIndexing;
        limits->limits.generalConstantMatrixVectorIndexing = this.limits.generalConstantMatrixVectorIndexing;

        return limits;
    }


    /// <summary>
    /// Default resource limits.
    /// </summary>
    public static readonly ResourceLimits DefaultResource = new()
    {
        maxLights = 32,
        maxClipPlanes = 6,
        maxTextureUnits = 32,
        maxTextureCoords = 32,
        maxVertexAttribs = 64,
        maxVertexUniformComponents = 4096,
        maxVaryingFloats = 64,
        maxVertexTextureImageUnits = 32,
        maxCombinedTextureImageUnits = 80,
        maxTextureImageUnits = 32,
        maxFragmentUniformComponents = 4096,
        maxDrawBuffers = 32,
        maxVertexUniformVectors = 128,
        maxVaryingVectors = 8,
        maxFragmentUniformVectors = 16,
        maxVertexOutputVectors = 16,
        maxFragmentInputVectors = 15,
        minProgramTexelOffset = -8,
        maxProgramTexelOffset = 7,
        maxClipDistances = 8,
        maxComputeWorkGroupCountX = 65535,
        maxComputeWorkGroupCountY = 65535,
        maxComputeWorkGroupCountZ = 65535,
        maxComputeWorkGroupSizeX = 1024,
        maxComputeWorkGroupSizeY = 1024,
        maxComputeWorkGroupSizeZ = 64,
        maxComputeUniformComponents = 1024,
        maxComputeTextureImageUnits = 16,
        maxComputeImageUniforms = 8,
        maxComputeAtomicCounters = 8,
        maxComputeAtomicCounterBuffers = 1,
        maxVaryingComponents = 60,
        maxVertexOutputComponents = 64,
        maxGeometryInputComponents = 64,
        maxGeometryOutputComponents = 128,
        maxFragmentInputComponents = 128,
        maxImageUnits = 8,
        maxCombinedImageUnitsAndFragmentOutputs = 8,
        maxCombinedShaderOutputResources = 8,
        maxImageSamples = 0,
        maxVertexImageUniforms = 0,
        maxTessControlImageUniforms = 0,
        maxTessEvaluationImageUniforms = 0,
        maxGeometryImageUniforms = 0,
        maxFragmentImageUniforms = 8,
        maxCombinedImageUniforms = 8,
        maxGeometryTextureImageUnits = 16,
        maxGeometryOutputVertices = 256,
        maxGeometryTotalOutputComponents = 1024,
        maxGeometryUniformComponents = 1024,
        maxGeometryVaryingComponents = 64,
        maxTessControlInputComponents = 128,
        maxTessControlOutputComponents = 128,
        maxTessControlTextureImageUnits = 16,
        maxTessControlUniformComponents = 1024,
        maxTessControlTotalOutputComponents = 4096,
        maxTessEvaluationInputComponents = 128,
        maxTessEvaluationOutputComponents = 128,
        maxTessEvaluationTextureImageUnits = 16,
        maxTessEvaluationUniformComponents = 1024,
        maxTessPatchComponents = 120,
        maxPatchVertices = 32,
        maxTessGenLevel = 64,
        maxViewports = 16,
        maxVertexAtomicCounters = 0,
        maxTessControlAtomicCounters = 0,
        maxTessEvaluationAtomicCounters = 0,
        maxGeometryAtomicCounters = 0,
        maxFragmentAtomicCounters = 8,
        maxCombinedAtomicCounters = 8,
        maxAtomicCounterBindings = 1,
        maxVertexAtomicCounterBuffers = 0,
        maxTessControlAtomicCounterBuffers = 0,
        maxTessEvaluationAtomicCounterBuffers = 0,
        maxGeometryAtomicCounterBuffers = 0,
        maxFragmentAtomicCounterBuffers = 1,
        maxCombinedAtomicCounterBuffers = 1,
        maxAtomicCounterBufferSize = 16384,
        maxTransformFeedbackBuffers = 4,
        maxTransformFeedbackInterleavedComponents = 64,
        maxCullDistances = 8,
        maxCombinedClipAndCullDistances = 8,
        maxSamples = 4,
        maxMeshOutputVerticesNV = 256,
        maxMeshOutputPrimitivesNV = 512,
        maxMeshWorkGroupSizeX_NV = 32,
        maxMeshWorkGroupSizeY_NV = 1,
        maxMeshWorkGroupSizeZ_NV = 1,
        maxTaskWorkGroupSizeX_NV = 32,
        maxTaskWorkGroupSizeY_NV = 1,
        maxTaskWorkGroupSizeZ_NV = 1,
        maxMeshViewCountNV = 4,
        maxMeshOutputVerticesEXT = 256,
        maxMeshOutputPrimitivesEXT = 256,
        maxMeshWorkGroupSizeX_EXT = 128,
        maxMeshWorkGroupSizeY_EXT = 128,
        maxMeshWorkGroupSizeZ_EXT = 128,
        maxTaskWorkGroupSizeX_EXT = 128,
        maxTaskWorkGroupSizeY_EXT = 128,
        maxTaskWorkGroupSizeZ_EXT = 128,
        maxMeshViewCountEXT = 4,
        maxDualSourceDrawBuffersEXT = 1,

        limits = ShaderLimits.DefaultLimits
    };
}