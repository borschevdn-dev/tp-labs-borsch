using System;
using System.Collections.Generic;
using Lab5;

Console.WriteLine("=== Lab5. Вариант 1: число π по ряду Лейбница ===");
Console.WriteLine("π/4 = 1 − 1/3 + 1/5 − 1/7 + …");
Console.WriteLine();

// 1. Ввод N (число членов ряда)
long n;
while (true)
{
    Console.Write("Введите N (число членов ряда, например 1000000000): ");
    if (long.TryParse(Console.ReadLine(), out n) && n > 0 && n <= 1_000_000_000)
        break;
    Console.WriteLine("Ошибка: нужно целое число от 1 до 1 000 000 000.");
}

// 2. Набор чисел потоков для таблицы
int[] threadCounts = { 1, 2, 4, 8, 12 };

Console.WriteLine();
Console.WriteLine($"Прогон для N = {n:N0}, потоки: {string.Join(", ", threadCounts)}");
Console.WriteLine("Каждый замер — 3 прогона, берётся минимум.");
Console.WriteLine();

// Прогрев: JIT + пул потоков
_ = PiCalculator.SumSequential(1000);
_ = PiCalculator.SumParallel(1000, 4);

// 3. Последовательная версия — один раз, она база для ускорения
var seq = PiCalculator.MeasureSequential(n);
double piSeq = 4.0 * seq.Sum;
double errSeq = Math.Abs(piSeq - Math.PI);

Console.WriteLine($"Последовательно: π ≈ {piSeq:F10}, ошибка {errSeq:E3}, {seq.ElapsedMs} мс");
Console.WriteLine();

// 4. Параллельные версии — по всем числам потоков
var rows = new List<(int Threads, long Ms, double Pi, double Diff)>();
foreach (int t in threadCounts)
{
    var par = PiCalculator.MeasureParallel(n, t);
    double piPar = 4.0 * par.Sum;
    double diff = Math.Abs(piSeq - piPar);
    rows.Add((t, par.ElapsedMs, piPar, diff));

    Console.WriteLine($"  {t,2} потоков: {par.ElapsedMs,5} мс, π ≈ {piPar:F10}, разница с последовательной {diff:E3}");
}

Console.WriteLine();

// 5. Итоговая таблица
Console.WriteLine("=== Таблица замеров ===");
Console.WriteLine($"{"Потоков",-10} {"Время, мс",-12} {"Ускорение",-12} {"Совпадение",-12}");
Console.WriteLine(new string('-', 50));
Console.WriteLine($"{"1 (посл.)",-10} {seq.ElapsedMs,-12} {"1.00×",-12} {"—",-12}");

foreach (var r in rows)
{
    double speedup = r.Ms > 0 ? (double)seq.ElapsedMs / r.Ms : 0;
    string match = r.Diff < 1e-9 ? "ДА" : "НЕТ";
    Console.WriteLine($"{r.Threads,-10} {r.Ms,-12} {speedup,-12:F2}× {match,-12}");
}

Console.WriteLine();
Console.WriteLine($"Ядер доступно: {Environment.ProcessorCount}");
Console.WriteLine("Примечание: ускорение < числа потоков из-за закона Амдала и накладных расходов Parallel.For.");
Console.WriteLine("На 1 потоке Parallel.For медленнее последовательной версии — это нормально.");