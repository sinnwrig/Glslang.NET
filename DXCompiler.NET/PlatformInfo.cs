using System.Runtime.InteropServices;

namespace DXCompiler.NET;

public struct PlatformInfo
{
    public OSPlatform platform;
    public Architecture architecture;

    public PlatformInfo(OSPlatform platform, Architecture architecture)
    {
        this.platform = platform;
        this.architecture = architecture;
    }

    public static OSPlatform GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return OSPlatform.OSX;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) 
            return OSPlatform.Linux;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) 
            return OSPlatform.Windows;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            return OSPlatform.FreeBSD;
        throw new Exception("Cannot determine operating system.");
    }

    public static PlatformInfo GetCurrentPlatform()
    {
        return new PlatformInfo(GetPlatform(), RuntimeInformation.ProcessArchitecture);
    }
}