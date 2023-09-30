using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Program;

public class MonitorHandle<T> : IDisposable
{
    bool disposed;

    public readonly T Value;

    public MonitorHandle(T obj)
    {
        Value = obj;

        Monitor.Enter(Value);
    }

    public void Dispose()
    {
        if (disposed)
            return;

        Monitor.Exit(Value);

        disposed = true;
    }
}
