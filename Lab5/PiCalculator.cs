using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Lab5;

/// <summary>Число π по ряду Лейбница: π/4 = 1 − 1/3 + 1/5 − …</summary>
public static class PiCalculator
{
    /// <summary>Последовательная сумма N членов ряда (без множителя 4).</summary>
    public static double SumSequential(long n)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));

        double sum = 0.0;
        for (long i = 0; i < n; i++)
        {
            double term = 1.0 / (2 * i + 1);
            if ((i & 1) == 1) term = -term;
            sum += term;
        }
        return sum;
    }

    /// <summary>Параллельная сумма N членов ряда через Parallel.For.</summary>
    public static double SumParallel(long n, int threadCount)
    {
        if (n < 0) throw new ArgumentOutOfRangeException(nameof(n));
        if (threadCount <= 0) throw new ArgumentOutOfRangeException(nameof(threadCount));

        double total = 0.0;
        object lockObj = new();

        var options = new ParallelOptions { MaxDegreeOfParallelism = threadCount };

        Parallel.For(0, threadCount, options, t =>
        {
            long chunk = n / threadCount;
            long from = t * chunk;
            long to = (t == threadCount - 1) ? n : from + chunk;

            double localSum = 0.0;
            for (long i = from; i < to; i++)
            {
                double term = 1.0 / (2 * i + 1);
                if ((i & 1) == 1) term = -term;
                localSum += term;
            }

            lock (lockObj) { total += localSum; }
        });

        return total;
    }

    /// <summary>Результат замера: значение, время, число потоков.</summary>
    public record Measurement(double Sum, long ElapsedMs, int Threads);

    /// <summary>Замер последовательной версии: 3 прогона, берём минимум.</summary>
    public static Measurement MeasureSequential(long n, int runs = 3)
    {
        long best = long.MaxValue;
        double sum = 0;

        for (int i = 0; i < runs; i++)
        {
            var sw = Stopwatch.StartNew();
            sum = SumSequential(n);
            sw.Stop();
            if (sw.ElapsedMilliseconds < best) best = sw.ElapsedMilliseconds;
        }

        return new Measurement(sum, best, 1);
    }

    /// <summary>Замер параллельной версии: 3 прогона, берём минимум.</summary>
    public static Measurement MeasureParallel(long n, int threads, int runs = 3)
    {
        long best = long.MaxValue;
        double sum = 0;

        for (int i = 0; i < runs; i++)
        {
            var sw = Stopwatch.StartNew();
            sum = SumParallel(n, threads);
            sw.Stop();
            if (sw.ElapsedMilliseconds < best) best = sw.ElapsedMilliseconds;
        }

        return new Measurement(sum, best, threads);
    }
}