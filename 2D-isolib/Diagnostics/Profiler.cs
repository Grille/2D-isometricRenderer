using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.Diagnostics;

public class Profiler
{
    float fps = 0;
    float frameTime = 0;
    float cumulativeFrameTime = 0;
    float delta;

    int count = 0;

    DateTime lastEndTime = DateTime.Now;
    DateTime lastFpsTime = DateTime.Now;
    Stopwatch sw = new Stopwatch();

    public float FPS => fps;
    public float FrameTime => frameTime;
    public float Delta => delta;

    public void Begin()
    {
        sw.Restart();
    }

    public void End()
    {
        sw.Stop();

        count += 1;

        cumulativeFrameTime += (float)sw.Elapsed.TotalMilliseconds;

        var now = DateTime.Now;

        delta = (float)(now - lastEndTime).TotalMilliseconds;
        lastEndTime = now;

        var lastFpsDelta = now - lastFpsTime;
        if (lastFpsDelta > TimeSpan.FromSeconds(1))
        {
            fps = (float)(count / lastFpsDelta.TotalSeconds);
            frameTime = cumulativeFrameTime / count;
            count = 0;
            lastFpsTime = now;
            cumulativeFrameTime = 0;
        }
    }

    public void Log()
    {
        Begin();
        End();
    }

    public override string ToString()
    {
        return $"{FPS:F2}fps {FrameTime:F2}ms";
    }
}
