using System.Reflection;
using System.Runtime.InteropServices;

namespace DXCompiler.NET;

public static class Global
{
    public const string LibraryPath = "libdxcwrapper.so";


    private static bool _assembliesResolved;

    public static void ResolveAssemblies()
    {
        if (_assembliesResolved)
            return;

        _assembliesResolved = true;
    }


    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        Console.WriteLine("resolving " + libraryName);

        string? assemblyDirectory = Path.GetDirectoryName(assembly.Location);
        string libraryDirectory = Path.Combine(assemblyDirectory, "DirectX", "library");

        Console.WriteLine("lib dir path: " + libraryDirectory);


        if (libraryName == "dxcwrapper" || libraryName == "dxcompiler")
        {
            OSPlatform platform = GetPlatform();

            string library = platform == OSPlatform.Linux ? "lib" : string.Empty;
            library += libraryName;
            library += platform == OSPlatform.Linux ? ".so" : ".dll";

            string libPath = Path.Combine(libraryDirectory, library);

            Console.WriteLine("full lib path: " + libPath);

            return NativeLibrary.Load(libPath, assembly, searchPath);
        }

        // Otherwise, fallback to default import resolver.
        return IntPtr.Zero;
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