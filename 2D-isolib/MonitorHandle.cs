using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Grille.Graphics.Isometric;

public class MonitorHandle<T> : IDisposable where T : notnull
{
    internal bool disposed;

    readonly object _key;
    public readonly T Value;

    public MonitorHandle(T obj, object key)
    {
        Value = obj;
        _key = key;

        Monitor.Enter(_key);
    }

    public void Dispose()
    {
        if (disposed)
            return;

        Monitor.Exit(_key);

        disposed = true;
    }
}
