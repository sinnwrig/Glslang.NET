using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The global glslang compilation context. Ensures that native glslang functions are properly initialized 
/// </summary>
public static class CompilationContext
{
    internal static List<WeakReference<IDisposable>> s_weakOnReload = [];
    internal static bool s_initialized;


    static void Main() { }


    internal static void EnsureInitialized()
    {
        if (s_initialized)
            return;

        if (GlslangNative.InitializeProcess() != 1)
            throw new FailedInitializationException("Failed to initialize glslang native process.");

        s_initialized = true;
    }


    internal static void WeakOnReloadCallback(IDisposable disposable)
    {
        s_weakOnReload.Add(new WeakReference<IDisposable>(disposable));
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
        foreach (WeakReference<IDisposable> weakDisposable in s_weakOnReload)
        {
            if (weakDisposable.TryGetTarget(out IDisposable? disposable))
                disposable.Dispose();
        }

        s_weakOnReload.Clear();

        if (s_initialized)
            GlslangNative.FinalizeProcess();

        if (GlslangNative.InitializeProcess() != 1)
            throw new FailedInitializationException("Failed to reinitialize glslang native process.");

        s_initialized = true;
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