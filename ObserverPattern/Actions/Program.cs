using System;
using System.Threading;

namespace Actions
{
    class Program
    {
        static void Main(string[] args)
        {
            Cat cat = new Cat();

            Console.WriteLine("Cat is sleeping!");

            using (Mouse tom = new Mouse("Tom"))
            using (Mouse jerry = new Mouse("jerry"))
            {
                tom.Register(cat);
                jerry.Register(cat);

                Console.WriteLine("Wait for 5 seconds");
                Thread.Sleep(5 * 1000);

                cat.Wake();
            }

            Console.ReadLine();
        }
    }
}
