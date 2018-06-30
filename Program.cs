using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace rxtest
{
    class Program
    {
        static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-AU");

            Instructions();

            var keys = GetInput().ToObservable();

            var details = ObservableTransaction.MakeTransactionObservable(keys, "Diesel", 1.569M);

            using (details.Subscribe(
                detail => Console.WriteLine(detail),
                e => Console.WriteLine(e.Message)))
            {
                Console.ReadLine();
            }
        }

        private static void Instructions()
        {
            Console.WriteLine("Instructions for filling");
            Console.WriteLine();
            Console.WriteLine("Key   Action");
            Console.WriteLine(" 1    Makes dispenser idle");
            Console.WriteLine(" 2    Puts dispenser into calling mode");
            Console.WriteLine(" 3    Acknowledges dispenser");
            Console.WriteLine(" 4    Starts filling");
            Console.WriteLine(" 5    Finishes filling");
            Console.WriteLine();
            Console.WriteLine("Enter / Return to quit");
            Console.WriteLine();
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
