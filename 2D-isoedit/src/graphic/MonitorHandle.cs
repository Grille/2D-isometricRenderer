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

    public readonly T Object;

    public MonitorHandle(T obj)
    {
        Object = obj;

        Monitor.Enter(Object);
    }

    public void Dispose()
    {
        if (disposed)
            return;

        Monitor.Exit(Object);

        disposed = true;
    }
}
