using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Grille.Graphics.Isometric;

internal class ParallelExecutor : IDisposable
{
    readonly BlockingCollection<Action> _workQueue;
    readonly ManualResetEventSlim _queueEmptyEvent;

    Task[] _tasks;

    public int Count { get; }

    int counter = 0;

    public ParallelExecutor(int count)
    {
        Count = count;

        _workQueue = new BlockingCollection<Action>();
        _queueEmptyEvent = new ManualResetEventSlim(true);

        _tasks = new Task[count];
        for (int i = 0; i < Count; i++)
        {
            var task = new Task(Worker, TaskCreationOptions.LongRunning);
            task.Start();
            _tasks[i] = task;
        }
    }

    void Worker()
    {
        foreach (var item in _workQueue.GetConsumingEnumerable())
        {
            item();

            Interlocked.Decrement(ref counter);

            if (counter == 0)
            {
                _queueEmptyEvent.Set();
            }
        }
    }

    void EnqueueWorkItem(Action action)
    {
        _workQueue.Add(action);
    }

    public void Run(Action<float,float> func)
    {
        if (Count == 1)
        {
            func(0, 1);
            return;
        }

        _queueEmptyEvent.Reset();
        counter = Count;

        float fcount = Count;
        for (int i = 0; i < Count; i++)
        {
            int tempi = i;
            var action = () => func(tempi / fcount, (tempi + 1) / fcount);
            EnqueueWorkItem(action);
        }

        _queueEmptyEvent.Wait();
    }

    public void Dispose()
    {
        _workQueue.CompleteAdding();
        _workQueue.Dispose();
        _queueEmptyEvent.Dispose();
    }
}