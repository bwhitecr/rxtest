using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace rxtest
{
    class Program
    {
        static bool done = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Press a Key");

            var keys = GetInput().ToObservable();

            var details = ObservableTransaction.MakeObservable(keys);

            using (details.Subscribe(
                detail => Console.WriteLine(detail),
                e => Console.WriteLine(e.Message),
                () => done = true))
            {
                while (!done)
                {
                    ;
                }
            }
            Console.WriteLine("done");
        }

        private static IEnumerable<char> GetInput()
        {
            while (true)
            {
                var key = Console.ReadKey();
                if (key.KeyChar == 'x')
                {
                    break;
                }
                else
                {
                    yield return key.KeyChar;
                }
            }
        }
    }
}
