using System.Reflection;
using System.Runtime.InteropServices;

namespace DXCompiler.NET;


internal static class DXCNative
{
    // Bundles platform and architecture for ease-of-use
    private struct PlatformInfo
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

        public static PlatformInfo GetCurrentPlatform() => new PlatformInfo(GetPlatform(), RuntimeInformation.ProcessArchitecture);
    }

    const string LibName = "machdxcompiler";

    const string WinLib = LibName + ".dll";
    const string OSXLib = "lib" + LibName + ".dylib";
    const string LinuxLib = "lib" + LibName + ".so";


    private static string CreateLibPath(string platform, string libraryName) => Path.Combine("runtimes", platform, "native", libraryName);

    private static readonly Dictionary<PlatformInfo, string> LibraryPathDict = new()
    {
        { new PlatformInfo(OSPlatform.Windows, Architecture.X64), CreateLibPath("win-x64", WinLib) },
        { new PlatformInfo(OSPlatform.Windows, Architecture.Arm64), CreateLibPath("win-arm64", WinLib) },

        { new PlatformInfo(OSPlatform.OSX, Architecture.X64), CreateLibPath("osx-x64", OSXLib) },
        { new PlatformInfo(OSPlatform.OSX, Architecture.Arm64), CreateLibPath("osx-arm64", OSXLib) },

        { new PlatformInfo(OSPlatform.Linux, Architecture.X64), CreateLibPath("linux-x64", LinuxLib) },
        { new PlatformInfo(OSPlatform.Linux, Architecture.Arm64), CreateLibPath("linux-arm64", LinuxLib) },
    };


    private static bool _assembliesResolved;

    static DXCNative() => ResolveAssemblies();

    private static void ResolveAssemblies()
    {
        if (_assembliesResolved)
            return;

        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);

        _assembliesResolved = true;
    }

    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != LibName)
            return IntPtr.Zero;

        PlatformInfo platform = PlatformInfo.GetCurrentPlatform();

        string relativeLibraryPath = LibraryPathDict[platform];        

        return NativeLibrary.Load(relativeLibraryPath, assembly, DllImportSearchPath.AssemblyDirectory | DllImportSearchPath.ApplicationDirectory);
    }

    const CallingConvention cconv = CallingConvention.Cdecl;


    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern IntPtr machDxcInit();

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern void machDxcDeinit(IntPtr compiler);

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern IntPtr machDxcCompile(
        IntPtr compilerPtr, 
        IntPtr codePtr, 
        nuint codeLength, 
        IntPtr argsPtr, 
        nuint argsLength,
        IntPtr includerPtr);


    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern IntPtr machDxcCompileResultGetError(IntPtr resultPtr);

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern IntPtr machDxcCompileResultGetObject(IntPtr resultPtr);

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern void machDxcCompileResultDeinit(IntPtr resultPtr);


    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern IntPtr machDxcCompileObjectGetBytes(IntPtr objectPtr);

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern nuint machDxcCompileObjectGetBytesLength(IntPtr objectPtr);

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern void machDxcCompileObjectDeinit(IntPtr objectPtr);


    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern IntPtr machDxcCompileErrorGetString(IntPtr errorPtr);

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern nuint machDxcCompileErrorGetStringLength(IntPtr errorPtr);

    [DllImport(LibName, CallingConvention = cconv)]
    internal static extern void machDxcCompileErrorDeinit(IntPtr errorPtr);
}