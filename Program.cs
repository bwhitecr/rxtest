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
            var keys = GetInput().ToObservable();

            var details = ObservableTransaction.MakeTransactionObservable(keys, "Diesel", 1.569M);

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
        }

        private static IEnumerable<char> GetInput()
        {
            while (true)
            {
                var key = Console.ReadKey();
                yield return key.KeyChar;
            }
        }
    }
}
