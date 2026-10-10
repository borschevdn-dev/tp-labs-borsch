using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2;

/// <summary>Автопарк: хранит транспорт и считает статистику.</summary>
public class Garage
{
    private readonly List<Vehicle> _vehicles = new();

    public IReadOnlyList<Vehicle> Vehicles => _vehicles;

    public void Add(Vehicle vehicle)
    {
        if (vehicle is null) throw new ArgumentNullException(nameof(vehicle));
        _vehicles.Add(vehicle);
    }

    /// <summary>Суммарная стоимость всех поездок на distanceKm.</summary>
    public double TotalTripCost(double distanceKm)
        => _vehicles.Sum(v => v.TripCost(distanceKm));

    /// <summary>Самый экономичный транспорт на distanceKm.</summary>
    public Vehicle? MostEconomical(double distanceKm)
        => _vehicles.OrderBy(v => v.TripCost(distanceKm)).FirstOrDefault();

    /// <summary>Средний расход по автопарку.</summary>
    public double AverageFuelConsumption()
        => _vehicles.Count == 0 ? 0 : _vehicles.Average(v => v.FuelConsumption);

    /// <summary>Группировка по типу транспорта.</summary>
    public IEnumerable<IGrouping<string, Vehicle>> GroupByType()
        => _vehicles.GroupBy(v => v.GetType().Name);
}