using System;

namespace EventHandlers
{
    public class Mouse : IDisposable
    {
        private readonly string name;

        private Cat cat { get; set; }

        public Mouse(string name)
        {
            this.name = name;
        }

        public void Dispose()
        {
            cat.WakeEventHandler -= Run;
        }

        public void Register(Cat cat)
        {
            this.cat = cat;
            cat.WakeEventHandler += Run;
        }

        private void Run(object obj, WakeEventArgs args)
        {
            Console.WriteLine($"Mouse[{name}] run at {args.dateWake.ToShortTimeString()}");
        }
    }
}
