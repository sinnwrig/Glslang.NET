using System.Reflection;
using Glslang;

namespace Application;

public class Program
{        
    public static void Main(string[] args)
    {
        using (ShaderCompiler compiler = new ShaderCompiler())
        {         
            ShaderInput input = new()
            {
                language = ShaderSource.GLSL,
                stage = ShaderStage.Vertex,
                client = ShaderClient.Vulkan,
                clientVersion = ClientVersion.Vulkan_1_2,
                targetLanguage = TargetLanguage.SpirV,
                targetLanguageVersion = TargetLanguageVersion.SpirV_1_5,
                code = ShaderCode.VertexCodeGlsl,
                defaultVersion = 100,
                defaultProfile = ShaderProfile.None,
                forceDefaultVersionAndProfile = 0,
                forwardCompatible = 0,
                messages = Messages.Default,
                resource = ShaderResource.DefaultResource,
                includeCallbacks = new()
                {
                    includeLocalFile = IncludeLocalFile,
                    includeSystemFile = IncludeSystemFile,
                }
            };

            GlslangShader shader = compiler.CreateShader(input);

            if (!shader.Preprocess(out string info, out string debugInfo))	
            {
                Console.WriteLine($"GLSL preprocessing failed:\n{info}");
            }

            if (!shader.Parse(out info, out debugInfo)) 
            {
                Console.WriteLine($"GLSL parsing failed\n{info}Preprocessed Code:{shader.GetPreprocessedCode()}");
            }


            GlslangProgram program = compiler.CreateProgram();
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

