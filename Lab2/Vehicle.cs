using System;

namespace Lab2;

/// <summary>
/// Абстрактный базовый класс транспортного средства.
/// </summary>
public abstract class Vehicle
{
    // Поля приватные — инкапсуляция
    private readonly string _name;
    private readonly double _fuelConsumption; // л на 100 км
    private readonly double _fuelPrice;       // руб за литр

    public string Name => _name;
    public double FuelConsumption => _fuelConsumption;
    public double FuelPrice => _fuelPrice;

    protected Vehicle(string name, double fuelConsumption, double fuelPrice)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Название не может быть пустым.", nameof(name));
        if (fuelConsumption <= 0)
            throw new ArgumentOutOfRangeException(nameof(fuelConsumption), "Расход должен быть > 0.");
        if (fuelPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(fuelPrice), "Цена топлива должна быть > 0.");

        _name = name;
        _fuelConsumption = fuelConsumption;
        _fuelPrice = fuelPrice;
    }

    /// <summary>Стоимость поездки на заданное расстояние, руб.</summary>
    public abstract double TripCost(double distanceKm);

    /// <summary>Описание вместимости.</summary>
    public abstract string CapacityInfo();

    /// <summary>Базовая стоимость топлива на поездку.</summary>
    protected double FuelCost(double distanceKm)
    {
        if (distanceKm < 0)
            throw new ArgumentOutOfRangeException(nameof(distanceKm), "Расстояние не может быть отрицательным.");
        return distanceKm * _fuelConsumption / 100.0 * _fuelPrice;
    }

    public override string ToString()
        => $"{GetType().Name} «{_name}»: расход {_fuelConsumption:F1} л/100км, топливо {_fuelPrice:F2} руб/л";
}