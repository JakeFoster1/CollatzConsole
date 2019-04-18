using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class CollatzMain
    {
        static void Main(string[] args)
        {
            ulong start, end;
            int numCores;

            Console.WriteLine("********** THE COLLATZ CALCULATOR **********");

            while (true)
            {
                Console.WriteLine("Please enter a starting number greater than or equal to 1: ");
                string val = Console.ReadLine();

                if (ulong.TryParse(val, out start))
                {
                    break;
                }

                Console.WriteLine("Value enterd is not a number. Try again!\n");
            }
            while (true)
            {
                Console.WriteLine("Please enter a ending number greater than " + start + ":");
                string val = Console.ReadLine();

                if (ulong.TryParse(val, out end))
                {
                    break;
                }

                Console.WriteLine("Value enterd is not a number. Try again!\n");
            }
            while (true)
            {
                Console.WriteLine("Please enter the number of threads: ");
                string val = Console.ReadLine();

                if (int.TryParse(val, out numCores))
                {
                    break;
                }

                Console.WriteLine("Value enterd is not a number. Try again!\n");
            }

            Collatz myColl = new Collatz(start, end, numCores);
            myColl.execute();
            List<Entry> myList = myColl.getEntryList();

            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine();





            for (int i = 0; i < myList.Count; i++)
            {
                Console.WriteLine(myList[i].number + " " + myList[i].stepCount);
            }

            Console.ReadLine();
        }
    }
}