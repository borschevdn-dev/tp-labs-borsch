using System;
using Lab4.Core;

namespace Lab4.Tests;

public class MathAlgorithmsTests
{
    // ===================== Factorial =====================

    [Fact]
    public void Factorial_OfZero_ReturnsOne()
    {
        // Arrange + Act
        long result = MathAlgorithms.Factorial(0);
        // Assert
        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(5, 120)]
    [InlineData(10, 3628800)]
    [InlineData(20, 2432902008176640000)] // граничное: максимум для long
    public void Factorial_ValidInput_ReturnsExpected(int n, long expected)
        => Assert.Equal(expected, MathAlgorithms.Factorial(n));

    [Theory]
    [InlineData(-1)]
    [InlineData(21)]
    public void Factorial_OutOfRange_Throws(int n)
        => Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Factorial(n));

    // ===================== Fibonacci =====================

    [Fact]
    public void Fibonacci_First6_AreCorrect()
    {
        var seq = MathAlgorithms.Fibonacci(6);
        Assert.Equal(new long[] { 0, 1, 1, 2, 3, 5 }, seq);
    }

    [Fact]
    public void Fibonacci_ZeroCount_ReturnsEmpty()
    {
        var seq = MathAlgorithms.Fibonacci(0);
        Assert.Empty(seq);
    }

    [Fact]
    public void Fibonacci_NegativeCount_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.Fibonacci(-1));

    [Theory]
    [InlineData(1, new long[] { 0 })]
    [InlineData(2, new long[] { 0, 1 })]
    public void Fibonacci_SmallCount_ReturnsExpected(int count, long[] expected)
        => Assert.Equal(expected, MathAlgorithms.Fibonacci(count));

    // ===================== CalculateA =====================

    [Fact]
    public void CalculateA_AtZero_ReturnsFiniteValue()
    {
        double a = MathAlgorithms.CalculateA(0);
        Assert.True(double.IsFinite(a));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(1.0)]
    [InlineData(-1.0)]
    [InlineData(10.0)]
    public void CalculateA_AnyFiniteX_ReturnsFinite(double x)
        => Assert.True(double.IsFinite(MathAlgorithms.CalculateA(x)));

    [Fact]
    public void CalculateA_IsDeterministic()
    {
        double a1 = MathAlgorithms.CalculateA(1.5);
        double a2 = MathAlgorithms.CalculateA(1.5);
        Assert.Equal(a1, a2);
    }

    // ===================== TaylorSin =====================

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(Math.PI / 2)]
    [InlineData(-1.2)]
    public void TaylorSin_MatchesMathSin(double x)
        => Assert.Equal(Math.Sin(x), MathAlgorithms.TaylorSin(x), 1e-5);

    [Fact]
    public void TaylorSin_AtZero_ReturnsZero()
        => Assert.Equal(0.0, MathAlgorithms.TaylorSin(0.0), 1e-10);

    [Fact]
    public void TaylorSin_NonPositiveEps_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => MathAlgorithms.TaylorSin(1.0, 0.0));
}