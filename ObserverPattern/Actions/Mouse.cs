using System;

namespace Actions
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
            cat.actionWaken -= Run;
        }

        public void Register(Cat cat)
        {
            this.cat = cat;
            cat.actionWaken += Run;
        }

        private void Run(DateTime dateTime)
        {
            Console.WriteLine($"Mouse[{name}] run at {dateTime.ToShortTimeString()}");
        }
    }
}
