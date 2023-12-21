using System.Reflection;

namespace DXCompiler.NET;


public enum DenormalType { Any, Preserve, Ftz }

public enum LanguageVersion { _2016, _2017, _2018, _2021 }

public enum Linkage { Internal, External }

public enum FlowControlMode{ Avoid, Prefer}

public enum DebugInfoType { Normal, Slim, Random }

public enum OptimizationLevel { O0, O1, O2, O3 }

public enum MatrixPackMode { ColumnMajor, RowMajor }


public class CompilerOptions
{
    private enum AssignmentType { Equals, Spaced }


    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Class | AttributeTargets.Field, AllowMultiple = true)]
    private class CompilerOptionAttribute : Attribute
    {
        public string Name;
        public int Value;
        public AssignmentType Assignment;

        public CompilerOptionAttribute(string name = "", int value = 0, AssignmentType assignment = AssignmentType.Spaced)
        {
            Name = name;
            Assignment = assignment;
            Value = value;
        }
    }


    // Compilation Options    
    [CompilerOption(name:"-all-resources-bound")]
    public bool allResourcesBound = false; // Enables agressive flattening

    
    [CompilerOption(name:"-auto-binding-space")]
    public string? autoBindingSpace = null; // Set auto binding space - enables auto resource binding in libraries

    
    [CompilerOption(name:"-Cc")]
    public bool outputColorCodedListings = false; // Output color coded assembly listings

    
    [CompilerOption(name:"-default-linkage")]
    public Linkage? defaultLinkage = null; // Set default linkage for non-shader functions when compiling or linking to a library target (internal, external)

    
    [CompilerOption(name:"-denorm")]
    public DenormalType? denormalValue = null; // Select denormal value options (any, preserve, ftz). any is the default.

    
    [CompilerOption(name:"-disable-payload-qualifiers")]
    public bool disablePayloadQualifiers = false; // Disables support for payload access qualifiers for raytracing payloads in SM 6.7.

    
    private Dictionary<string, string> macros = new();

    public void SetMacro(string name, string value) => macros[name] = value; // Define a macro
    public void RemoveMacro(string name) => macros.Remove(name); // Remove a macro

    private void AddMacros(List<string> args)
    {
        foreach (var macro in macros)
        {
            args.Add("-D");
            args.Add($"{macro.Key}={macro.Value}");
        }
    }


    
    [CompilerOption(name:"-enable-16bit-types")]
    public bool enable16BitTypes = false; // Enable 16bit types and disable min precision types. Available in HLSL 2018 and shader model 6.2

    
    [CompilerOption(name:"-enable-lifetime-markers")]
    public bool enableLifetimeMarkers = false; // Enable generation of lifetime markers

    
    [CompilerOption(name:"-enable-payload-qualifiers")]
    public bool enablePayloadQualifiers = false; // Enables support for payload access qualifiers for raytracing payloads in SM 6.6.

    // Force UTF-8 since that is the encoding used by DxcCompiler when passing in shader code
    
    [CompilerOption(name:"-encoding")]
    public readonly string? encoding = "utf8"; // Set default encoding for source inputs and text outputs (utf8|utf16(win)|utf32(*nix)|wide) default=utf8

    
    [CompilerOption(name:"-export-shaders-only")]
    public bool exportShadersOnly = false; // Only export shaders when compiling a library

    // NOTE: Not too sure how this is supposed to work- when I figure it out I'll make some wrappers to make building an export list easier
    
    [CompilerOption(name:"-exports")]
    public string? exports = null; // Specify exports when compiling a library: export1[[,export1_clone,...]=internal_name][;...]

    
    [CompilerOption(name:"-E")]
    public string entryPoint = "main"; // Entry point name- defaults to main

    
    [CompilerOption(name:"-Fc")]
    public string? assemblyListingFile = null; // Output assembly code listing file

    
    [CompilerOption(name:"-fdiagnostics-show-option")]
    public bool diagnosticsShowOption = false; // Print option name with mappable diagnostics

    
    [CompilerOption(name:"-fdisable-loc-tracking")]
    public bool disableLocTracking = false; // Disable source location tracking in IR. This will break diagnostic generation for late validation. (Ignored if /Zi is passed)

    
    [CompilerOption(name:"-Fd")]
    public string? debugOutputName = null; // Write debug information to the given file, or automatically named file in directory when ending in '\'

    
    [CompilerOption(name:"-Fe")]
    public string? errorOutputName = null; // Output warnings and errors to the given file

    
    [CompilerOption(name:"-Fh")]
    public string? headerOutputName = null; // Output header file containing object code

    
    [CompilerOption(name:"-Fi")]
    public string? preprocessOutputName = null; // Set preprocess output file name (with /P)
    
    
    [CompilerOption(name:"-flegacy-macro-expansion")]
    public bool legacyExpandMacros = false; // Expand the operands before performing token-pasting operation (fxc behavior)

    
    [CompilerOption(name:"-flegacy-resource-reservation")]
    public bool legacyResourceReservation = false; // Reserve unused explicit register assignments for compatibility with shader model 5.0 and below

    
    [CompilerOption(name:"-fnew-inlining-behavior")]
    public bool newInliningBehavior = false; // Experimental option to use heuristics-driven late inlining and disable alwaysinline annotation for library shaders

    
    [CompilerOption(name:"-fno-diagnostics-show-option")]
    public bool noDiagnosticsShowOption = false; // Do not print option name with mappable diagnostics

    
    [CompilerOption(name:"-force-rootsig-ver")]
    public string? rootsigVersion = null; // Force root signature version (rootsig_1_1 if omitted)

    
    [CompilerOption(name:"-Fo")]
    public string? objectOutputName = null; // Output object file

    
    [CompilerOption(name:"-Fre")]
    public string? reflectionOutputName = null; // Output reflection to the given file

    
    [CompilerOption(name:"-Frs")]
    public string? rootSignatureOutputName = null; // Output root signature to the given file

    
    [CompilerOption(name:"-Fsh")]
    public string? hashOutputName = null; // Output shader hash to the given file

    
    [CompilerOption(name:"-ftime-report")]
    public bool timeReport = false; // Print time report

    
    [CompilerOption(name:"-ftime-trace", Assignment = AssignmentType.Equals)]
    public string? timeTrace = null; // Print hierarchial time to file- stdout if no file is specified

    
    [CompilerOption(name:"-Gec")]
    public bool backwardCompatibilityMode = false; // Enable backward compatibility mode

    
    [CompilerOption(name:"-Ges")]
    public bool strictMode = false; // Enable strict mode

    [CompilerOption(name:"-Gfa", value:(int)FlowControlMode.Avoid)]
    [CompilerOption(name:"-Gfp", value:(int)FlowControlMode.Prefer)]
    public FlowControlMode? flowControlMode;

    
    [CompilerOption(name:"-Gis")]
    public bool forceIEEEStrictness = false; // Force IEEE strictness

    
    [CompilerOption(name:"-HV")]
    public LanguageVersion? languageVersion = null; // HLSL version (2016, 2017, 2018, 2021). Default is 2021

    
    [CompilerOption(name:"-H")]
    public bool showIncludesAndNestingDepth = false; // Show header includes and nesting depth
    
    
    [CompilerOption(name:"-ignore-line-directives")]
    public bool ignoreLineDirectives = false; // Ignore line directives
    
    
    [CompilerOption(name:"-Lx")]
    public bool outputHexLiterals = false; // Output hexadecimal literals

    
    [CompilerOption(name:"-Ni")]
    public bool outputInstructionNumbers = false; // Output instruction numbers in assembly listings

    
    [CompilerOption(name:"-no-legacy-cbuf-layout")]
    public bool noLegacyCbufferLoad = false; // Do not use legacy cbuffer load

    
    [CompilerOption(name:"-no-warnings")]
    public bool suppressWarnings = false; // Suppress warnings

    
    [CompilerOption(name:"-No")]
    public bool outputByteOffsets = false; // Output instruction byte offsets in assembly listings

    
    [CompilerOption(name:"-Odump")]
    public bool printOptimizerCommands = false; // Print the optimizer commands

    
    [CompilerOption(name:"-Od")]
    public bool disableOptimization = false; // Disable optimizations

    
    [CompilerOption(name:"-pack-optimized")]
    public bool optimizeSignaturePacking = false; // Optimize signature packing assuming identical signature provided for each connecting stage

    
    [CompilerOption(name:"-pack-prefix-stable")]
    public bool packPrefixStable = false; // (default) Pack signatures preserving prefix-stable property - appended elements will not disturb placement of prior elements

    
    [CompilerOption(name:"-recompile")]
    public bool recompileFromDXIL = false; // recompile from DXIL container with Debug Info or Debug Info bitcode file

    
    [CompilerOption(name:"-res-may-alias")]
    public bool assumeResAliasing = false; // Assume that UAVs/SRVs may alias

    
    [CompilerOption(name:"-rootsig-define")]
    public string? rootSignatureDefine = null;

    
    [CompilerOption(name:"-T")]
    public ShaderProfile profile; // Set target profile

    
    [CompilerOption(name:"-Vd")]
    public bool disableValidation = false; // Disable validation

    
    [CompilerOption(name:"-verify")]
    public string? verificationDirectives = null; // Verify diagnostic output using comment directives

    
    [CompilerOption(name:"-Vi")]
    public bool displayIncludeDetails = false; // Display details about the include process.

    
    [CompilerOption(name:"-Vn")]
    public string? variableName = null; // Use <name> as variable name in header file

    
    [CompilerOption(name:"-WX")]
    public bool warningsAsErrors = false; // Treat warnings as errors


    [CompilerOption(name:"-Zi", value:(int)DebugInfoType.Normal)]
    [CompilerOption(name:"-Zs", value:(int)DebugInfoType.Slim)]
    public DebugInfoType? debugInfo = null; // Debug info type 


    [CompilerOption(name:"-Zpc", value:(int)MatrixPackMode.ColumnMajor)]
    [CompilerOption(name:"-Zpr", value:(int)MatrixPackMode.RowMajor)]
    public MatrixPackMode? matrixPackMode = null; // Should matrices be packed in column-major or row-major order

    
    [CompilerOption(name:"-Zsb")]
    public bool computeBinaryHash = false; // Compute Shader Hash considering only output binary

    
    [CompilerOption(name:"-Zss")]
    public bool computeSourceHash = false; // Compute Shader Hash considering source information

    // Optimization options

    [CompilerOption(name:"-ffinite-math-only", value:1)]
    [CompilerOption(name:"-fno-finite-math-only", value:0)]
    public bool? finiteMathOnly = null;


    [CompilerOption(name:"-O0", value:(int)OptimizationLevel.O0)]
    [CompilerOption(name:"-O1", value:(int)OptimizationLevel.O1)]
    [CompilerOption(name:"-O2", value:(int)OptimizationLevel.O2)]
    [CompilerOption(name:"-O3", value:(int)OptimizationLevel.O3)]
    public OptimizationLevel? optimization = null; // Optimization level


    // Rewriter options
    [CompilerOption(name:"-decl-global-cb")]
    public bool createGlobalCBuffer = false; // Collect all global constants outside cbuffer declarations into cbuffer GlobalCB { ... }. Still experimental, not all dependency scenarios handled.

    
    [CompilerOption(name:"-extract-entry-uniforms")]
    public bool extractEntryUniforms = false; // Move uniform parameters from entry point to global scope

    
    [CompilerOption(name:"-global-extern-by-default")]
    public bool globalExternByDefault = false; // Set extern on non-static globals

    
    [CompilerOption(name:"-keep-user-macro")]
    public bool keepUserMacro = false; // Write out user defines after rewritten HLSL

    
    [CompilerOption(name:"-line-directive")]
    public bool addLineDirective = false; // Add line directive

    
    [CompilerOption(name:"-remove-unused-functions")]
    public bool removeUnusedFunctions = false; // Remove unused functions and types

    
    [CompilerOption(name:"-remove-unused-globals")]
    public bool removeUnusedGlobals = false; // Remove unused static globals and functions

    
    [CompilerOption(name:"-skip-fn-body")]
    public bool skipFunctionBody = false; // Translate function definitions to declarations

    
    [CompilerOption(name:"-skip-static")]
    public bool skipStatic = false; // Remove static functions and globals when used with -skip-fn-body

    
    [CompilerOption(name:"-unchanged")]
    public bool rewriteUnchanged = false; // Rewrite HLSL, without changes.
    

    // SPIR-V CodeGen options
    [CompilerOption(name:"-fspv-debug", Assignment = AssignmentType.Equals)]
    public string? debugWhitelist = null; // Specify whitelist of debug info category (file -> source -> line, tool, vulkan-with-source)

    
    [CompilerOption(name:"-fspv-entrypoint-name", Assignment = AssignmentType.Equals)]
    public string? entrypointName = null; // Specify the SPIR-V entry point name. Defaults to the HLSL entry point name.

    
    [CompilerOption(name:"-fspv-extension", Assignment = AssignmentType.Equals)]
    public string? extension = null; // Specify SPIR-V extension permitted to use

    
    [CompilerOption(name:"-fspv-flatten-resource-arrays")]               
    public bool flattenResourceArrays = false; // Flatten arrays of resources so each array element takes one binding number

    
    [CompilerOption(name:"-fspv-preserve-bindings")]                     
    public bool preserveBindings = false; // Preserves all bindings declared within the module, even when those bindings are unused

    
    [CompilerOption(name:"-fspv-preserve-interface")]                    
    public bool preserveInterface = false; // Preserves all interface variables in the entry point, even when those variables are unused

    
    [CompilerOption(name:"-fspv-print-all")]                             
    public bool printAll = false; // Print the SPIR-V module before each pass and after the last one. Useful for debugging SPIR-V legalization and optimization passes.

    
    [CompilerOption(name:"-fspv-reduce-load-size")]                      
    public bool reduceLoadSize = false; // Replaces loads of composite objects to reduce memory pressure for the loads

    
    [CompilerOption(name:"-fspv-reflect")]                               
    public bool addReflectionAid = false; // Emit additional SPIR-V instructions to aid reflection

    
    [CompilerOption(name:"-fspv-target-env", Assignment = AssignmentType.Equals)]
    public string? targetEnvironment = null; // Specify the target environment: vulkan1.0 (default), vulkan1.1, vulkan1.1spirv1.4, vulkan1.2, vulkan1.3, or universal1.5

    
    [CompilerOption(name:"-fspv-use-legacy-buffer-matrix-order")]        
    public bool useLegacyBufferMatrixOrder = false; // Assume the legacy matrix order (row major) when accessing raw buffers (e.g., ByteAdddressBuffer)

    
    [CompilerOption(name:"-fvk-auto-shift-bindings")]                    
    public bool autoShiftBindings = false; // Apply fvk-*-shift to resources without an explicit register assignment.

    
    [CompilerOption(name:"-fvk-b-shift")]                                
    public string? bindingNumberShift = null; // Specify Vulkan binding number shift for b-type register: <shift> <space> 

    
    [CompilerOption(name:"-fvk-bind-globals")]                           
    public string? globalBindingNumber = null; // Specify Vulkan binding number and set number for the $Globals cbuffer: <binding> <set>

    
    [CompilerOption(name:"-fvk-bind-register")]                          
    public string? registerBindings = null; // Specify Vulkan descriptor set and binding for a specific register: <type-number> <space> <binding> <set>

    
    [CompilerOption(name:"-fvk-invert-y")]                               
    public bool invertY = false; // Negate SV_Position.y before writing to stage output in VS/DS/GS to accommodate Vulkan's coordinate system

    
    [CompilerOption(name:"-fvk-s-shift")]                                
    public string? SRegisterBindingShift = null; // Specify Vulkan binding number shift for s-type register: <shift> <space> 

    
    [CompilerOption(name:"-fvk-support-nonzero-base-instance")]          
    public bool nonzeroBaseInstance = false; // Follow Vulkan spec to use gl_BaseInstance as the first vertex instance, which makes SV_InstanceID = gl_InstanceIndex - gl_BaseInstance (without this option, SV_InstanceID = gl_InstanceIndex)
    
    
    [CompilerOption(name:"-fvk-t-shift")]                                
    public string? TRegisterBindingShift = null; // Specify Vulkan binding number shift for t-type register: <shift> <space> 

    
    [CompilerOption(name:"-fvk-u-shift")]                                
    public string? URegisterBindingShift = null; // Specify Vulkan binding number shift for u-type register: <shift> <space> 

    
    [CompilerOption(name:"-fvk-use-dx-layout")]                          
    public bool useDirectXMemoryLayout = false; // Use DirectX memory layout for Vulkan resources

    
    [CompilerOption(name:"-fvk-use-dx-position-w")]                      
    public bool useDirectXPositionW = false; // Reciprocate SV_Position.w after reading from stage input in PS to accommodate the difference between Vulkan and DirectX

    
    [CompilerOption(name:"-fvk-use-gl-layout")]                          
    public bool useOpenGLMemoryLayout = false; // Use strict OpenGL std140/std430 memory layout for Vulkan resources

    
    [CompilerOption(name:"-fvk-use-scalar-layout")]                      
    public bool useScalarMemoryLayout = false; // Use scalar memory layout for Vulkan resources

    
    [CompilerOption(name:"-Oconfig", Assignment = AssignmentType.Equals)]
    public string? spirVOptimizationConfig = null; // Specify a comma-separated list of SPIRV-Tools passes to customize optimization configuration (see http://khr.io/hlsl2spirv#optimization)

    
    [CompilerOption(name:"-spirv")]                                      
    public bool generateAsSpirV = false; // Generate SPIR-V code


    // Utility Options:    
    [CompilerOption(name:"-dumpbin")]
    public bool dumpBin = false; // Load a binary file rather than compiling

    
    [CompilerOption(name:"-extractrootsignature")]                       
    public bool extractRootSignature = false; // Extract root signature from shader bytecode (must be used with /Fo <file>)

    
    [CompilerOption(name:"-getprivate")] 
    public string? privateDataFile = null; //  Save private data from shader blob

    // No clue what the <inputs> argument is
    
    [CompilerOption(name:"-link")]                                       
    public string? libraries = null; // Link list of libraries provided in <inputs> argument separated by ';'

    
    [CompilerOption(name:"-P")]                                          
    public bool outputPreprocessedCode = false; // Preprocess to file

    
    [CompilerOption(name:"-Qembed_debug")]                               
    public bool embedDebugPDB = false; // Embed PDB in shader container (must be used with /Zi)

    
    [CompilerOption(name:"-Qstrip_debug")]                               
    public bool stripDebugPDB = false; // Strip debug information from 4_0+ shader bytecode  (must be used with /Fo <file>)

    
    [CompilerOption(name:"-Qstrip_priv")]                                
    public bool stripPrivate = false; // Strip private data from shader bytecode  (must be used with /Fo <file>)

    
    [CompilerOption(name:"-Qstrip_reflect")]                             
    public bool stripReflection = false; // Strip reflection data from shader bytecode  (must be used with /Fo <file>)

    
    [CompilerOption(name:"-Qstrip_rootsignature")]                       
    public bool stripRootSignature = false; // Strip root signature data from shader bytecode  (must be used with /Fo <file>)

    
    [CompilerOption(name:"-setprivate")]                    
    public string? additionalPrivateData = null; // Private data to add to compiled shader blob

    
    [CompilerOption(name:"-setrootsignature")]                   
    public string? attachedRootSignature = null; // Attach root signature to shader bytecode

    
    [CompilerOption(name:"-verifyrootsignature")]                 
    public string? verifyRootSignature = null; // Verify shader bytecode with root signature


    // Warning options
    private Dictionary<string, bool> warnings = new();
    public void SetWarning(string name, bool value) => warnings[name] = value; // Enable/disable a warning

    private void AddWarnings(List<string> args)
    {
        foreach (var warning in warnings)
        {
            string warnVal = warning.Value ? string.Empty : "no-";
            args.Add($"-W{warnVal}{warning.Key}");
        }
    }


// -----------------------------------------------------------
// --------------       Argument Builder        --------------
// -----------------------------------------------------------

    // Cache reflection fields along with CompilerOptionAttribute
    private static readonly Dictionary<string, (FieldInfo, CompilerOptionAttribute[])> fields; 
    
    static CompilerOptions()
    {
        fields = new();
        foreach (FieldInfo fi in typeof(CompilerOptions).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            CompilerOptionAttribute[] attribs = fi.GetCustomAttributes<CompilerOptionAttribute>().ToArray();

            if (attribs.Length > 0)
                fields[fi.Name] = (fi, attribs);
        }
    }

    public CompilerOptions(ShaderProfile profile)
    {
        this.profile = profile;
    }
    

    private void SetBoolOption(List<string> args, CompilerOptionAttribute[] options, bool value)
    {
        // Enable/Disable compiler option based on boolean 
        if (options.Length < 2)
        {
            if (value)
                args.Add(options[0].Name);
            
            return;
        }

        // Set compiler option based on true/false
        if (value)
            args.Add(options[0].Value == 1 ? options[0].Name : options[1].Name);
        else
            args.Add(options[0].Value == 0 ? options[0].Name : options[1].Name);
    }   


    private void SetStringOption(List<string> args, CompilerOptionAttribute option, string? str)
    {
        // String is empty but argument isn't null- add option only
        if (string.IsNullOrWhiteSpace(str))
        {
            args.Add(option.Name);
            return;  
        } 

        if (option.Assignment == AssignmentType.Equals)
        {
            args.Add($"{option.Name}={str.ToLower()}");
            return;
        }

        args.Add(option.Name);
        args.Add(str);
    }


    private void SetEnumOption(List<string> args, CompilerOptionAttribute[] options, Enum enumValue)
    {
        // Only one option- use enum as string value
        if (options.Length == 1)
        {
            SetStringOption(args, options[0], enumValue.ToString().Remove('_'));
            return;
        }

        // Find matching CompilerOption and add that to compiler args
        int enumVal = Convert.ToInt32(enumValue);
        CompilerOptionAttribute? matching = Array.Find(options, x => x.Value == enumVal);

        if (matching == null)
            return;
        
        args.Add(matching.Name);
    }


    private void AddOption(List<string> args, (FieldInfo, CompilerOptionAttribute[]) field)
    {
        object? nullableVal = field.Item1.GetValue(this);

        if (nullableVal == null)
            return;

        object value = nullableVal;

        if (value is bool boolValue)
            SetBoolOption(args, field.Item2, boolValue);
        else if (value is Enum enumValue)
            SetEnumOption(args, field.Item2, enumValue);
        else
            SetStringOption(args, field.Item2[0], value.ToString());
    }


    public string[] GetArgumentsArray()
    {
        List<string> args = new List<string>();

        foreach (var pair in fields)
        {
            AddOption(args, pair.Value);
        }

        AddMacros(args);
        AddWarnings(args);

        return args.ToArray();
    }
}