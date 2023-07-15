using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program;

internal class WorkData
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Size { get; private set; }

    public RenderDataCell[] Buffer { get; private set; }


    public WorkData(int width, int height)
    {
        Init(width, height);
    }

    void Init(int width, int height)
    {
        Width = width;
        Height = height;
        Size = Width * Height;

        Buffer = new RenderDataCell[Size];
    }

    public void Clear()
    {
        Array.Clear(Buffer);
    }
}
