using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric.WinForms;

public class RenderTimer
{
    public TimeSpan Interval { get; set; }

    public double TargetFPS
    {
        get => 1000d / Interval.TotalMilliseconds;
        set => Interval = TimeSpan.FromMilliseconds(1000 / value);
    }

    readonly Task _task;

    readonly Action _action;

    public DateTime LastTick { get; private set; }

    public TimeSpan Delta {  get; private set; }

    TimeSpan _counter;

    public RenderTimer(Action action)
    {
        _action = action;
        _task = new Task(Tick);
    }

    void Tick()
    {
        while (true)
        {
            _counter += DateTime.Now - LastTick;
            LastTick = DateTime.Now;

            if (_counter > Interval)
            {
                Delta = _counter;
                _counter -= Interval;
                if (_counter > Interval)
                {
                    _counter = TimeSpan.Zero;
                }
                _action();
            }
        }
    }

    public void Start()
    {
        LastTick = DateTime.Now;
        _task.Start();
    }
}
