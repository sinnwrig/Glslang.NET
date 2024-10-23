using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;

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
    /// Returns the string representation of this <see cref="ShaderLimits"/> instance. 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.AppendLine(nameof(ShaderLimits));

        FieldInfo[] fields = typeof(ShaderLimits).GetFields(BindingFlags.Instance | BindingFlags.Public);

        foreach (var field in fields)
        {
            sb.AppendLine($"\t{field.Name}: {field.GetValue(this)}");
        }

        return sb.ToString();
    }
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


    /// <summary>
    /// Default resource limits.
    /// </summary>
    public static readonly ResourceLimits DefaultResource = *GlslangNative.DefaultResourceLimits();


    /// <summary>
    /// Returns the string representation of this <see cref="ResourceLimits"/> instance. 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder sb = new();

        sb.AppendLine(nameof(ResourceLimits));

        FieldInfo[] fields = typeof(ResourceLimits).GetFields(BindingFlags.Instance | BindingFlags.Public);

        foreach (var field in fields)
        {
            sb.AppendLine($"\t{field.Name}: {field.GetValue(this)}");
        }

        return sb.ToString();
    }
}