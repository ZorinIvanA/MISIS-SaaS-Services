using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WebServices.AsyncWorks
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Threads
            Thread firstThread = new Thread(() =>
            {
                WriteSomething(1);
            });

            Thread secondThread = new Thread(() =>
            {
                WriteSomething(2);
            });

            //Both operations start in separate threads, and we cannot
            //guarantee who finishes first
            firstThread.Start();
            secondThread.Start();

            //Blocks the current (main) thread until first and second threads retrun
            firstThread.Join();
            secondThread.Join();

            //Taking thread from a pool instead of creating new instance
            ThreadPool.QueueUserWorkItem((state) => { WriteSomething(3); });
            #endregion

            Console.Clear();

            #region Tasks
            //Both tasks start in separate threads and automatically return 
            //to main thread
            Task task1 = Task.Run(() =>
            {
                WriteSomething(1);
            });
            Task task2 = Task.Run(() =>
            {
                WriteSomething(2);
            });

            Thread.Sleep(1000);

            //If tasks are long enough - the first task finishes earlier than
            //second one because Wait() blocks the current thread.
            //But as far as they're very quick - we cannot guarantee who
            //finishes first
            task1.Wait();
            task2.Wait();

            //These tasks return value. These formats are equivalent, 
            //but the second one is shorter
            Task<int> myTaskWithReturnValue1 = Task.Run(() =>
            {
                return 3;
            });

            Task<int> myTaskWithReturnValue2 = Task.Run(() => 3);
            #endregion

            Console.Read();
        }

        static void WriteSomething(int threadNumber)
        {
            Console.WriteLine($"Hello {threadNumber}");
        }
    }
}
