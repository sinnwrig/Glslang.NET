using System.Reflection;
using Glslang;

namespace Application;

public class Program
{        
    public static void Main(string[] args)
    {
        using (ShaderCompiler compiler = new ShaderCompiler())
        {         
            ShaderInput vertInput = new()
            {
                language = ShaderSource.HLSL,
                stage = ShaderStage.Vertex,
                client = ShaderClient.Vulkan,
                clientVersion = ClientVersion.Vulkan_1_2,
                targetLanguage = TargetLanguage.SpirV,
                targetLanguageVersion = TargetLanguageVersion.SpirV_1_5,
                code = ShaderCode.VertexCodeHlsl,
                defaultVersion = 450,
                defaultProfile = ShaderProfile.None,
                forceDefaultVersionAndProfile = false,
                forwardCompatible = false,
                messages = Messages.Default,
                resource = ShaderResource.DefaultResource,
                includeCallbacks = new()
                {
                    includeLocalFile = IncludeLocalFile,
                    includeSystemFile = IncludeSystemFile,
                }
            };

            GlslangShader vertexShader = compiler.CreateShader(vertInput);

            if (!vertexShader.Preprocess(out string info, out string debugInfo))
            {
                Console.WriteLine($"Vertex shader preprocessing failed:\n{info}");
                return;
            }

            if (!vertexShader.Parse(out info, out debugInfo)) 
            {
                Console.WriteLine($"Vertex shader parsing failed\n{info}Preprocessed Code:{vertexShader.GetPreprocessedCode()}");
                return;
            }


            GlslangProgram program = compiler.CreateProgram(vertexShader);

            if (!program.Link(out info, out debugInfo, Messages.SpvRules | Messages.VulkanRules))
            {
                Console.WriteLine($"Shader linking failed\n{info}Debug info:{debugInfo}");
                return;
            }

            uint[] spirv = program.GenerateSpirV(ShaderStage.Vertex, out string spirVMessages);
            Console.WriteLine($"Vertex Spir-V Messages: {spirVMessages}\n\n");

            Console.WriteLine($"Spv length: {spirv.Length}");

            string disassembled = compiler.DisassembleSpirVBinary(spirv);
            Console.WriteLine($"Disassembled vertex bytecode:\n{disassembled}");
        }
    }


    public static IncludeResult IncludeLocalFile(string headerName, string includerName, int includeDepth)
    {
        return new()
        {
            headerName = headerName,
            fileContents = "/* No file contents right now */",
        };
    }


    public static IncludeResult IncludeSystemFile(string headerName, string includerName, int includeDepth)
    {
        return new()
        {
            headerName = headerName,
            fileContents = "/* No file contents right now */",
        };
    }
}

