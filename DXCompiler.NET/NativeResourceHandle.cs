namespace DXCompiler.NET;


public abstract class NativeResourceHandle : IDisposable
{
    protected IntPtr handle;

    protected abstract void ReleaseHandle();


    public void Dispose()
    {
        ReleaseHandle();
        GC.SuppressFinalize(this);
    }


    ~NativeResourceHandle()
    {
        ConsoleColor prev = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Warning: Native handle for {GetType().Name} was not properly deallocated. Ensure object is disposed by manually calling Dispose() or with a using statement.");
        Console.ForegroundColor = prev;

        Dispose();
    }
}