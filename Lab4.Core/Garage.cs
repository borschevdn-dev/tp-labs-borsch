using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab4.Core;

public class Garage
{
    private readonly List<Vehicle> _vehicles = new();
    public IReadOnlyList<Vehicle> Vehicles => _vehicles;

    public void Add(Vehicle vehicle)
    {
        if (vehicle is null) throw new ArgumentNullException(nameof(vehicle));
        _vehicles.Add(vehicle);
    }

    public double TotalTripCost(double distanceKm)
        => _vehicles.Sum(v => v.TripCost(distanceKm));

    public Vehicle? MostEconomical(double distanceKm)
        => _vehicles.OrderBy(v => v.TripCost(distanceKm)).FirstOrDefault();

    public double AverageFuelConsumption()
        => _vehicles.Count == 0 ? 0 : _vehicles.Average(v => v.FuelConsumption);

    public IEnumerable<IGrouping<string, Vehicle>> GroupByType()
        => _vehicles.GroupBy(v => v.GetType().Name);
}