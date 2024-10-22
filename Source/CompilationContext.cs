using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The global glslang compilation context. Ensures that native glslang functions are properly initialized 
/// </summary>
public static class CompilationContext
{
    internal static List<WeakReference<IDisposable>> weakOnReload = [];


    static void Main() { }


    internal static void EnsureInitialized()
    {
        weakOnReload = [];

        if (GlslangNative.InitializeProcess() != 1)
            throw new FailedInitializationException("Failed to initialize glslang native process.");

        Console.WriteLine("Initialized glslang process");
    }


    internal static void WeakOnReloadCallback(IDisposable disposable)
    {
        weakOnReload.Add(new WeakReference<IDisposable>(disposable));
    }


    /// <summary>
    /// Disassemble SPIR-V binary into human-readabe format.
    /// </summary>
    /// <param name="spirvWords">Binary buffer of SPIR-V words.</param>
    /// <returns>String containing human-readable SPIR-V instructions.</returns>
    public static string DisassembleSPIRV(uint[] spirvWords)
    {
        return GlslangNative.DisassembleSPIRV(spirvWords, (uint)spirvWords.Length);
    }


    /// <summary>
    /// Dispose of allocated shader resources and programs, and finalize the native process before reinitializing it.
    /// </summary>
    public static void ReloadNativeProcess()
    {
        foreach (WeakReference<IDisposable> weakDisposable in weakOnReload)
        {
            if (weakDisposable.TryGetTarget(out IDisposable? disposable))
                disposable.Dispose();
        }

        weakOnReload.Clear();

        GlslangNative.FinalizeProcess();

        if (GlslangNative.InitializeProcess() != 1)
            throw new FailedInitializationException("Failed to reinitialize glslang native process.");
    }
}


/// <summary>
/// Returned if native library process initialization failed.
/// </summary>
public class FailedInitializationException : Exception
{
    /// <summary></summary>
    public FailedInitializationException() { }

    /// <summary></summary>
    public FailedInitializationException(string message) : base(message) { }

    /// <summary></summary>
    public FailedInitializationException(string message, Exception inner) : base(message, inner) { }

}