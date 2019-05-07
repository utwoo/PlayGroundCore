using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace RxNetDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting on thread :{0}", Thread.CurrentThread.ManagedThreadId);
            NewThreadScheduler.Default.Schedule("A", OuterAction);
            NewThreadScheduler.Default.Schedule("B", OuterAction);

            Console.ReadKey();
        }

        private static IDisposable OuterAction(IScheduler scheduler, string state)
        {
            Console.WriteLine("{0} start. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            scheduler.Schedule(state + ".inner", InnerAction);
            Console.WriteLine("{0} end. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            return Disposable.Empty;
        }
        private static IDisposable InnerAction(IScheduler scheduler, string state)
        {
            Console.WriteLine("{0} start. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            scheduler.Schedule(state + ".Leaf", LeafAction);
            Console.WriteLine("{0} end. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            return Disposable.Empty;
        }
        private static IDisposable LeafAction(IScheduler scheduler, string state)
        {
            Console.WriteLine("{0}. ThreadId:{1}",
            state,
            Thread.CurrentThread.ManagedThreadId);
            return Disposable.Empty;
        }
    }
}
