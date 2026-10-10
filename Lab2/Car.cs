using System;

namespace Lab2;

/// <summary>Легковой автомобиль.</summary>
public class Car : Vehicle
{
    private readonly int _seats;

    public int Seats => _seats;

    public Car(string name, double fuelConsumption, double fuelPrice, int seats)
        : base(name, fuelConsumption, fuelPrice)
    {
        if (seats <= 0)
            throw new ArgumentOutOfRangeException(nameof(seats), "Число мест должно быть > 0.");
        _seats = seats;
    }

    public override double TripCost(double distanceKm) => FuelCost(distanceKm);

    public override string CapacityInfo() => $"легковой, {_seats} мест";

    public override string ToString() => base.ToString() + $", мест: {_seats}";
}