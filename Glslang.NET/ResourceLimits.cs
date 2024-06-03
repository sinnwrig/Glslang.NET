using System;
using System.Runtime.InteropServices;

namespace Glslang.NET;


[StructLayout(LayoutKind.Sequential)]
public struct ShaderLimits
{
    public bool nonInductiveForLoops;
    public bool whileLoops;
    public bool doWhileLoops;
    public bool generalUniformIndexing;
    public bool generalAttributeMatrixVectorIndexing;
    public bool generalVaryingIndexing;
    public bool generalSamplerIndexing;
    public bool generalVariableIndexing;
    public bool generalConstantMatrixVectorIndexing;

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


[StructLayout(LayoutKind.Sequential)]
public struct ResourceLimits 
{
    public int maxLights;
    public int maxClipPlanes;
    public int maxTextureUnits;
    public int maxTextureCoords;
    public int maxVertexAttribs;
    public int maxVertexUniformComponents;
    public int maxVaryingFloats;
    public int maxVertexTextureImageUnits;
    public int maxCombinedTextureImageUnits;
    public int maxTextureImageUnits;
    public int maxFragmentUniformComponents;
    public int maxDrawBuffers;
    public int maxVertexUniformVectors;
    public int maxVaryingVectors;
    public int maxFragmentUniformVectors;
    public int maxVertexOutputVectors;
    public int maxFragmentInputVectors;
    public int minProgramTexelOffset;
    public int maxProgramTexelOffset;
    public int maxClipDistances;
    public int maxComputeWorkGroupCountX;
    public int maxComputeWorkGroupCountY;
    public int maxComputeWorkGroupCountZ;
    public int maxComputeWorkGroupSizeX;
    public int maxComputeWorkGroupSizeY;
    public int maxComputeWorkGroupSizeZ;
    public int maxComputeUniformComponents;
    public int maxComputeTextureImageUnits;
    public int maxComputeImageUniforms;
    public int maxComputeAtomicCounters;
    public int maxComputeAtomicCounterBuffers;
    public int maxVaryingComponents;
    public int maxVertexOutputComponents;
    public int maxGeometryInputComponents;
    public int maxGeometryOutputComponents;
    public int maxFragmentInputComponents;
    public int maxImageUnits;
    public int maxCombinedImageUnitsAndFragmentOutputs;
    public int maxCombinedShaderOutputResources;
    public int maxImageSamples;
    public int maxVertexImageUniforms;
    public int maxTessControlImageUniforms;
    public int maxTessEvaluationImageUniforms;
    public int maxGeometryImageUniforms;
    public int maxFragmentImageUniforms;
    public int maxCombinedImageUniforms;
    public int maxGeometryTextureImageUnits;
    public int maxGeometryOutputVertices;
    public int maxGeometryTotalOutputComponents;
    public int maxGeometryUniformComponents;
    public int maxGeometryVaryingComponents;
    public int maxTessControlInputComponents;
    public int maxTessControlOutputComponents;
    public int maxTessControlTextureImageUnits;
    public int maxTessControlUniformComponents;
    public int maxTessControlTotalOutputComponents;
    public int maxTessEvaluationInputComponents;
    public int maxTessEvaluationOutputComponents;
    public int maxTessEvaluationTextureImageUnits;
    public int maxTessEvaluationUniformComponents;
    public int maxTessPatchComponents;
    public int maxPatchVertices;
    public int maxTessGenLevel;
    public int maxViewports;
    public int maxVertexAtomicCounters;
    public int maxTessControlAtomicCounters;
    public int maxTessEvaluationAtomicCounters;
    public int maxGeometryAtomicCounters;
    public int maxFragmentAtomicCounters;
    public int maxCombinedAtomicCounters;
    public int maxAtomicCounterBindings;
    public int maxVertexAtomicCounterBuffers;
    public int maxTessControlAtomicCounterBuffers;
    public int maxTessEvaluationAtomicCounterBuffers;
    public int maxGeometryAtomicCounterBuffers;
    public int maxFragmentAtomicCounterBuffers;
    public int maxCombinedAtomicCounterBuffers;
    public int maxAtomicCounterBufferSize;
    public int maxTransformFeedbackBuffers;
    public int maxTransformFeedbackInterleavedComponents;
    public int maxCullDistances;
    public int maxCombinedClipAndCullDistances;
    public int maxSamples;
    public int maxMeshOutputVerticesNV;
    public int maxMeshOutputPrimitivesNV;
    public int maxMeshWorkGroupSizeX_NV;
    public int maxMeshWorkGroupSizeY_NV;
    public int maxMeshWorkGroupSizeZ_NV;
    public int maxTaskWorkGroupSizeX_NV;
    public int maxTaskWorkGroupSizeY_NV;
    public int maxTaskWorkGroupSizeZ_NV;
    public int maxMeshViewCountNV;
    public int maxMeshOutputVerticesEXT;
    public int maxMeshOutputPrimitivesEXT;
    public int maxMeshWorkGroupSizeX_EXT;
    public int maxMeshWorkGroupSizeY_EXT;
    public int maxMeshWorkGroupSizeZ_EXT;
    public int maxTaskWorkGroupSizeX_EXT;
    public int maxTaskWorkGroupSizeY_EXT;
    public int maxTaskWorkGroupSizeZ_EXT;
    public int maxMeshViewCountEXT;
    public int maxDualSourceDrawBuffersEXT;

    public ShaderLimits limits;

    
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