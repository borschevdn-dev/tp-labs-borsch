using System;
using System.Collections.Generic;

namespace Lab4.Core;

/// <summary>Алгоритмы из лабораторной 1 (вариант 1).</summary>
public static class MathAlgorithms
{
    /// <summary>Факториал n. Допустимо 0..20 (дальше переполнение long).</summary>
    public static long Factorial(int n)
    {
        if (n < 0)  throw new ArgumentOutOfRangeException(nameof(n), "n не может быть отрицательным.");
        if (n > 20) throw new ArgumentOutOfRangeException(nameof(n), "n > 20 даёт переполнение long.");

        long result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    /// <summary>Числа Фибоначчи: возвращает count элементов, начиная с 0.</summary>
    public static long[] Fibonacci(int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

        var result = new long[count];
        if (count >= 1) result[0] = 0;
        if (count >= 2) result[1] = 1;
        for (int i = 2; i < count; i++)
            result[i] = result[i - 1] + result[i - 2];
        return result;
    }

    /// <summary>A(x) = sqrt(ln(4/3)) + (x + 9/7) - e^(sin(1.3x - 0.7)).</summary>
    public static double CalculateA(double x)
    {
        double part1 = Math.Sqrt(Math.Log(4.0 / 3.0));
        double part2 = x + 9.0 / 7.0;
        double part3 = Math.Exp(Math.Sin(1.3 * x - 0.7));
        return part1 + part2 - part3;
    }

    /// <summary>sin(x) рядом Тейлора с точностью eps.</summary>
    public static double TaylorSin(double x, double eps = 1e-6)
    {
        if (eps <= 0) throw new ArgumentOutOfRangeException(nameof(eps), "Точность должна быть > 0.");

        double sum = 0.0;
        double term = x;
        int n = 0;
        while (Math.Abs(term) > eps)
        {
            sum += term;
            n++;
            term *= -x * x / ((2.0 * n) * (2.0 * n + 1.0));
        }
        return sum;
    }
}