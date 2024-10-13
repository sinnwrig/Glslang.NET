using System.Runtime.InteropServices;

namespace Glslang.NET;


/// <summary>
/// The context in which all glslang member functions should be invoked. 
/// </summary>
/// <remarks>
/// Ensure this context is properly freed with a `using` statement or by manually calling Dispose
/// </remarks>
public class CompilationContext : IDisposable
{
    static void Main() { }

    private static bool _instanceExists = false;

    private readonly List<Shader> activeShaders = new();
    private readonly List<Program> activePrograms = new();


    /// <summary>
    /// Create a new compialtion context.
    /// </summary>
    /// <remarks>
    /// Only one CompilationContext instance should be active at any part of the program's lifetime. If compilation is multithreaded, a singleton of this class should be used across threads.
    /// </remarks>
    /// <exception cref="MultipleInstanceException"></exception>
    /// <exception cref="FailedInitializationException"></exception>
    public CompilationContext()
    {
        if (_instanceExists)
            throw new MultipleInstanceException("Multiple CompilationContext instances detected, this is not allowed. Ensure a single active instance is shared across the program.");

        _instanceExists = true;

        if (GlslangNative.InitializeProcess() != 1)
            throw new FailedInitializationException("Failed to initialize glslang native process.");
    }


    /// <summary>
    /// Create a managed compiler shader instance.
    /// </summary>
    /// <param name="input">Input compilation options the shader will use during its lifecycle. Once assigned, this setting is tied to a native structure allocation and cannot be changed.</param>
    /// <remarks>
    /// This is the only proper way to create a Shader as the parent CompilationContext ensures any related native allocations are freed at the end of the object lifecycle.
    /// </remarks>
    public Shader CreateShader(CompilationInput input)
    {
        Shader shader = new(input);
        activeShaders.Add(shader);
        return shader;
    }


    /// <summary>
    /// Create a managed compiler program instance.
    /// </summary>
    /// <remarks>
    /// This is the only proper way to create a Program as the parent CompilationContext ensures any related native allocations are freed at the end of the object lifecycle.
    /// </remarks>
    public Program CreateProgram()
    {
        Program program = new();
        activePrograms.Add(program);
        return program;
    }


    /// <summary>
    /// Disassemble SPIR-V binary into human-readabe format.
    /// </summary>
    /// <param name="spirvWords">Binary buffer of SPIR-V words.</param>
    /// <returns>String containing human-readable SPIR-V instructions.</returns>
    public string DisassembleSPIRV(byte[] spirvWords)
    {
        IntPtr textPtr = GlslangNative.DisassembleSPIRV(spirvWords, (nuint)(spirvWords.Length / sizeof(uint)));
        string text = Marshal.PtrToStringUTF8(textPtr) ?? string.Empty;
        Marshal.FreeHGlobal(textPtr);
        return text;
    }


    /// <summary>
    /// Dispose of allocated shader resources and programs, and finalize the native process.
    /// </summary>
    public void Dispose()
    {
        _instanceExists = false;

        foreach (Program pr in activePrograms)
            pr.Release();

        foreach (Shader sh in activeShaders)
            sh.Release();

        GlslangNative.FinalizeProcess();
        GC.SuppressFinalize(this);
    }


    /// <summary></summary>
    ~CompilationContext()
    {
        ConsoleColor prev = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Warning: CompilationContext was not properly deallocated. Ensure object is disposed by manually calling Dispose() or by utilizing a `using` statement.");
        Console.ForegroundColor = prev;

        Dispose();
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


/// <summary>
/// Returned if multiple instances of a class are detected.
/// </summary>
public class MultipleInstanceException : Exception
{
    /// <summary></summary>
    public MultipleInstanceException() { }

    /// <summary></summary>
    public MultipleInstanceException(string message) : base(message) { }

    /// <summary></summary>
    public MultipleInstanceException(string message, Exception inner) : base(message, inner) { }

}