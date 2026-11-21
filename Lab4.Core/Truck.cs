using System;

namespace Lab4.Core;

public class Truck : Vehicle
{
    private readonly double _payloadTons;
    public double PayloadTons => _payloadTons;

    public Truck(string name, double fuelConsumption, double fuelPrice, double payloadTons)
        : base(name, fuelConsumption, fuelPrice)
    {
        if (payloadTons <= 0)
            throw new ArgumentOutOfRangeException(nameof(payloadTons), "Грузоподъёмность должна быть > 0.");
        _payloadTons = payloadTons;
    }

    public override double TripCost(double distanceKm)
        => FuelCost(distanceKm) + distanceKm * 0.5 * _payloadTons;

    public override string CapacityInfo() => $"грузовой, до {_payloadTons:F1} т";
    public override string ToString() => base.ToString() + $", грузоподъёмность: {_payloadTons:F1} т";
}