using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Program;

class ParallelExecutor
{
    public int Count { get; }

    public ParallelExecutor(int count)
    {
        Count = count;
    }

    public void Run(Action<float,float> func)
    {
        if (Count == 1)
        {
            func(0, 1);
            return;
        }

        var tasks = new Task[Count];
        float fcount = Count;
        for (int i = 0; i < Count; i++)
        {
            int tempi = i;
            var action = () => func(tempi / fcount, (tempi + 1) / fcount);
            tasks[i] = Task.Run(action);
        }

        Task.WaitAll(tasks);
    }
}