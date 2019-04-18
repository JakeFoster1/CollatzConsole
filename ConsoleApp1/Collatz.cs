using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public struct Entry
    {
        public ulong number;
        public ulong stepCount;
    };

    public class Collatz
    {
        private ulong start, end;
        private int numCores;
        private List<Entry> entryList;

        public Collatz()
        {
            numCores = 1;
            start = 1;
            end = 100;
            entryList = new List<Entry>(20);
        }

        public Collatz(ulong start, ulong end, int numCores)
        {
            this.numCores = numCores;
            this.start = start;
            this.end = end;
            entryList = new List<Entry>(20);
        }

        public void execute()
        {
            ulong dimension = end - start;

            List<ulong> nums = new List<ulong>();

            for (int i = 0; i < numCores; i++)
            {
                nums.Add(dimension / (ulong)numCores);
            }
            for (ulong i = 0; i < dimension % (ulong)numCores; i++)
            {
                nums[(int)i] += 1;
            }

            List<Thread> threads = new List<Thread>();
            List<ThreadStart> threadStarts = new List<ThreadStart>();

            List<List<Entry>> results = new List<List<Entry>>(numCores);

            for (int i = 0; i < numCores; i++)
            {
                results.Add(new List<Entry>());
            }

            List<ulong> startList = new List<ulong>();
            List<ulong> endList = new List<ulong>();
            List<int> indexList = new List<int>();
            startList.Add(start);
            for(int i = 0; i < numCores; i++)
            {
                endList.Add(startList[i] + nums[i]);
                indexList.Add(i);

                startList.Add(endList[i] + 1);
            }

            for (int i = 0; i < startList.Count-2; i++)
            {
                threadStarts.Add(delegate () { computeColatz(startList[i], endList[i], results[indexList[i]]); });
                threads.Add(new Thread(threadStarts[i]));
                
                //threads[i].Start();
            }

            //ThreadStart threadStart = delegate() { computeColatz(start, end, results[0]); };
            //Thread thread = new Thread(threadStart);
            //thread.Start();
            //thread.Join();

            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }

            List<Entry> newResults = new List<Entry>();
            for (int i = 0; i < numCores; i++)
            {
                //for (int j = 0; j < results[i].Count; j++)
                //{
                //    Entry temp = new Entry();
                //    temp.number = results[i][j].number;
                //    temp.stepCount = results[i][j].stepCount;
                //    newResults.Add(temp);
                //}
                newResults.AddRange(results[i]);
            }

            newResults.Sort((s1, s2) => s1.stepCount.CompareTo(s2.stepCount));
            newResults.Reverse();

            entryList.AddRange(newResults);
        }

        public List<Entry> getEntryList()
        {
            return entryList;
        }

        public static void computeColatz(ulong start, ulong end, List<Entry> currList)
        {
            ulong current, modRes;
            Entry curr;
            for (ulong i = start; i <= end; i++)
            {
                current = i;
                curr.number = i;
                curr.stepCount = 0;

                while (current > 1)
                {
                    modRes = current % 2;

                    switch (modRes)
                    {
                        case 0:
                            current /= 2;
                            break;
                        default:
                            current *= 3;
                            current += 1;
                            break;
                    }

                    curr.stepCount++;
                }

                currList.Add(curr);
                currList.Sort((s1, s2) => s1.stepCount.CompareTo(s2.stepCount));

                if (currList.Count > 10)
                    currList.RemoveAt(0);

                currList.TrimExcess();
            }
        }
    }
}
