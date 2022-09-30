using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    private static Lazy<SingleInitializationSingleton> _lazy =
        new (() => new SingleInitializationSingleton());

    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;

    public const int DefaultDelay = 3_000;
    
    public int Delay { get; }

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    internal static void Reset()
    {
        if (_isInitialized)
        {
            lock (Locker)
            {
                _lazy = new(() => new SingleInitializationSingleton());
                _isInitialized = false;
            }
        }
    }

    public static void Initialize(int delay)
    {
        if (_isInitialized)
            throw new InvalidOperationException("Double initialization");
        lock (Locker)
            {
                if (_isInitialized)
                    throw new InvalidOperationException("Double initialization");
                _lazy = new(() => new SingleInitializationSingleton(delay));
                _isInitialized = true;
            }
    }

    public static SingleInitializationSingleton Instance {
        get
        {
            return _lazy.Value;
        }
    }
}
