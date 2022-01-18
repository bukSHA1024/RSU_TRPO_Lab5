using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lab5_mag
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int writersCount = 14;
            const int readersCount = 15;

            Console.WriteLine("Enter N:");
            var n = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Simple buffer.");

            var simpleBuffer = new SimpleBuffer(writersCount, readersCount, n);
            var watch = Stopwatch.StartNew();
            var results = (await simpleBuffer.DoWork()).ToList();
            watch.Stop();
            Console.WriteLine($"Simple buffer, time of execution {watch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Readed messages by each reader: {string.Join(" ", results)}. Sum: {results.Sum()}");
            Console.WriteLine();
            Console.WriteLine();

            var mutexBuffer = new MutexBuffer(writersCount, readersCount, n);
            watch = Stopwatch.StartNew();
            results = (await mutexBuffer.DoWork()).ToList();
            watch.Stop();
            Console.WriteLine($"Mutex buffer, time of execution {watch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Readed messages by each reader: {string.Join(" ", results)}. Sum: {results.Sum()}");
            Console.WriteLine();
            Console.WriteLine();

            var monitorBuffer = new MonitorBuffer(writersCount, readersCount, n);
            watch = Stopwatch.StartNew();
            results = (await monitorBuffer.DoWork()).ToList();
            watch.Stop();
            Console.WriteLine($"Monitor buffer, time of execution {watch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Readed messages by each reader: {string.Join(" ", results)}. Sum: {results.Sum()}");
            Console.WriteLine();
            Console.WriteLine();

            var eventBuffer = new EventBuffer(writersCount, readersCount, n);
            watch = Stopwatch.StartNew();
            results = (await eventBuffer.DoWork()).ToList();
            watch.Stop();
            Console.WriteLine($"Event buffer, time of execution {watch.ElapsedMilliseconds} ms");
            Console.WriteLine($"Readed messages by each reader: {string.Join(" ", results)}. Sum: {results.Sum()}");
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
