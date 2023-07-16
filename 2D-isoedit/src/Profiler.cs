using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program;

internal class Profiler
{
    float fps = 0;
    float frameTime = 0;
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
        frameTime = (float)sw.Elapsed.TotalMilliseconds;
        var now = DateTime.Now;

        delta = (float)(now - lastEndTime).TotalMilliseconds;
        lastEndTime = now;

        var lastFpsDelta = now - lastFpsTime;
        if (lastFpsDelta > TimeSpan.FromSeconds(1))
        {
            fps = (float)(count / lastFpsDelta.TotalSeconds);
            count = 0;
            lastFpsTime = now;
        }
        count += 1;
    }

    public void Log()
    {
        Begin();
        End();
    }
}
