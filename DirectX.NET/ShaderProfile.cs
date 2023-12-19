namespace DXCompiler.NET;

public enum ShaderType : ushort
{
    Vertex,
    Pixel, 
    Domain,
    Hull,
    Mesh,
    Amplification,
    Library,
    Geometry,
    Compute,
}


public static class ShaderTypeExtensions
{
    public static ushort MinimumVersion(this ShaderType type)
    {
        return type switch {
            ShaderType.Vertex => 40,
            ShaderType.Pixel => 40,
            ShaderType.Domain => 50,
            ShaderType.Hull => 50,
            ShaderType.Mesh => 60,
            ShaderType.Amplification => 60,
            ShaderType.Library => 51,
            ShaderType.Geometry => 40,
            ShaderType.Compute => 40,
            _ => 40
        };
    }


    public static string Abbreviation(this ShaderType type)
    {
        return type switch {
            ShaderType.Vertex => "vs",
            ShaderType.Pixel => "ps",
            ShaderType.Domain => "ds",
            ShaderType.Hull => "hs",
            ShaderType.Mesh => "ms",
            ShaderType.Amplification => "as",
            ShaderType.Library => "lib",
            ShaderType.Geometry => "gs",
            ShaderType.Compute => "cs",
            
            _ => "vs"
        };
    }
}




public class ShaderProfile
{
    private ShaderType type;
    private ushort version = 5;
    private ushort subVersion = 0;


    public ShaderProfile(ShaderType type, int version, int subVersion)
    {
        this.type = type;
        this.version = (ushort)version;
        this.subVersion = (ushort)subVersion;
    }


    public bool IsValid() => type.MinimumVersion() < (version * 10) + subVersion;

    public void Validate() 
    {
        if (!IsValid())
        {
            float minVersion = (float)type.MinimumVersion() / 10;

            throw new InvalidProfileException($"{type} shader is not compatible with shader model {version}.{subVersion}. Shader model must be a minimum of {minVersion:0.0}");
        }
    }


    public ShaderType Type
    {
        get => type;
        set
        {
            type = value;
        }
    }


    public int Version
    {
        get => version;

        set
        {
            version = (ushort)Math.Clamp(value, 4, 10);
        }
    }


    public int SubVersion
    {
        get => subVersion;

        set
        {
            subVersion = (ushort)Math.Clamp(value, 4, 10);
        }
    }


    public override string ToString()
    {
        return $"{type.Abbreviation()}_{version}_{subVersion}";   
    }
}


public class InvalidProfileException : Exception
{
    public InvalidProfileException() : base() { }

    public InvalidProfileException(string message) : base(message) { }

    public InvalidProfileException(string message, Exception innerException) : base(message, innerException) { }
}
