using System;

// Меню выбора задания
Console.WriteLine("Лабораторная работа 1, вариант 1");
Console.WriteLine("Выберите задание (1-4):");

if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 4)
{
    Console.WriteLine("Ошибка: введите число от 1 до 4.");
    return;
}

switch (choice)
{
    case 1: Task1(); break;
    case 2: Task2(); break;
    case 3: Task3(); break;
    case 4: Task4(); break;
}

// ================== Задание 1. Факториал ==================
static void Task1()
{
    Console.Write("Введите n (0..20): ");
    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0 || n > 20)
    {
        Console.WriteLine("Ошибка: нужно целое число от 0 до 20.");
        return;
    }
    Console.WriteLine($"{n}! = {Factorial(n)}");
}

static long Factorial(int n)
{
    long result = 1;
    for (int i = 2; i <= n; i++)
        result *= i;
    return result;
}

// ================== Задание 2. Числа Фибоначчи ==================
static void Task2()
{
    Console.Write("Введите n (>= 0): ");
    if (!int.TryParse(Console.ReadLine(), out int n) || n < 0)
    {
        Console.WriteLine("Ошибка: нужно целое неотрицательное число.");
        return;
    }
    Console.WriteLine(string.Join(", ", Fibonacci(n)));
}

static long[] Fibonacci(int n)
{
    var result = new long[n + 1];
    if (n >= 0) result[0] = 0;
    if (n >= 1) result[1] = 1;
    for (int i = 2; i <= n; i++)
        result[i] = result[i - 1] + result[i - 2];
    return result;
}

// ================== Задание 3. Функция A(x) ==================
// A = sqrt(ln(4/3)) + (x + 9/7) - e^(sin(1.3x - 0.7))
static void Task3()
{
    Console.Write("Введите x: ");
    if (!double.TryParse(Console.ReadLine(), out double x))
    {
        Console.WriteLine("Ошибка: введите число.");
        return;
    }

    // ln(4/3) — константа, всегда > 0, корень извлекается всегда
    double lnConst = Math.Log(4.0 / 3.0);
    if (lnConst < 0)
    {
        Console.WriteLine("Ошибка: ln(4/3) < 0, корень не определён.");
        return;
    }

    double a = CalculateA(x);
    Console.WriteLine($"A({x}) = {a:F6}");
}

static double CalculateA(double x)
{
    double part1 = Math.Sqrt(Math.Log(4.0 / 3.0));
    double part2 = x + 9.0 / 7.0;
    double part3 = Math.Exp(Math.Sin(1.3 * x - 0.7));
    return part1 + part2 - part3;
}

// ================== Задание 4. Ряд Тейлора для sin(x) ==================
// sin(x) = x - x^3/3! + x^5/5! - x^7/7! + ...
// Общий член: (-1)^n * x^(2n+1) / (2n+1)!, n = 0, 1, 2, ...
static void Task4()
{
    Console.Write("Введите x (в радианах): ");
    if (!double.TryParse(Console.ReadLine(), out double x))
    {
        Console.WriteLine("Ошибка: введите число.");
        return;
    }

    const double eps = 1e-6;
    double sum = TaylorSin(x, eps, out int terms);
    double lib = Math.Sin(x);

    Console.WriteLine($"Сумма ряда:        {sum:F10}");
    Console.WriteLine($"Math.Sin(x):       {lib:F10}");
    Console.WriteLine($"Разница:           {Math.Abs(sum - lib):E3}");
    Console.WriteLine($"Просуммировано членов: {terms}");
}

static double TaylorSin(double x, double eps, out int terms)
{
    double sum = 0.0;
    double term = x;       // первый член: x
    int n = 0;
    terms = 0;

    while (Math.Abs(term) > eps)
    {
        sum += term;
        terms++;
        n++;
        // Следующий член: (-1)^n * x^(2n+1) / (2n+1)!
        // Рекуррентно из предыдущего:
        // term_{n} = term_{n-1} * (-x^2) / ((2n)(2n+1))
        term *= -x * x / ((2.0 * n) * (2.0 * n + 1.0));
    }
    return sum;
}