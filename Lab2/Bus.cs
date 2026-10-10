using System;

namespace Lab2;

/// <summary>Автобус.</summary>
public class Bus : Vehicle
{
    private readonly int _passengerSeats;

    public int PassengerSeats => _passengerSeats;

    public Bus(string name, double fuelConsumption, double fuelPrice, int passengerSeats)
        : base(name, fuelConsumption, fuelPrice)
    {
        if (passengerSeats <= 0)
            throw new ArgumentOutOfRangeException(nameof(passengerSeats), "Число пассажиров должно быть > 0.");
        _passengerSeats = passengerSeats;
    }

    // Плата за комфорт пассажиров: 2 руб за пассажира на км
    public override double TripCost(double distanceKm)
        => FuelCost(distanceKm) + distanceKm * 2.0 * _passengerSeats;

    public override string CapacityInfo() => $"автобус, {_passengerSeats} пассажиров";

    public override string ToString() => base.ToString() + $", пассажиров: {_passengerSeats}";
}