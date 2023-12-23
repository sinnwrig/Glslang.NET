using System.Reflection;
using System.Runtime.InteropServices;

namespace DXCompiler.NET;

public static class Global
{
    private static readonly string[] LibraryPath = new string[] { "DirectX", "library" };

    public const string Library = "dxcwrapper";


    private static bool _assembliesResolved;

    public static void ResolveAssemblies()
    {
        if (_assembliesResolved)
            return;

        NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);

        _assembliesResolved = true;
    }


    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != Library)
            return IntPtr.Zero;

        string? assemblyDirectory = Path.GetDirectoryName(assembly.Location);

        if (assemblyDirectory == null)
            return IntPtr.Zero;

        string libraryDirectory = Path.Combine(assemblyDirectory, LibraryPath[0], LibraryPath[1]);

        OSPlatform platform = GetPlatform();

        string library = platform == OSPlatform.Linux ? "lib" : string.Empty;
        library += libraryName;
        library += platform == OSPlatform.Linux ? ".so" : ".dll";

        string libPath = Path.Combine(libraryDirectory, library);

        return NativeLibrary.Load(libPath, assembly, searchPath);
    }


    public static OSPlatform GetPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OSPlatform.OSX;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return OSPlatform.Linux;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OSPlatform.Windows;
        }

        throw new Exception("Cannot determine operating system!");
    }
}