using System;
using System.Linq;
using Lab4.Core;

namespace Lab4.Tests;

public class VehicleTests
{
    // ===================== Car: инварианты =====================

    [Fact]
    public void Car_NegativeSeats_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => new Car("Test", 7.0, 55.0, 0));

    [Fact]
    public void Car_EmptyName_Throws()
        => Assert.Throws<ArgumentException>(() => new Car("", 7.0, 55.0, 5));

    [Fact]
    public void Car_ValidArguments_StoresProperties()
    {
        var car = new Car("Lada", 7.1, 55.0, 5);
        Assert.Equal("Lada", car.Name);
        Assert.Equal(7.1, car.FuelConsumption);
        Assert.Equal(5, car.Seats);
    }

    // ===================== TripCost: расчёты =====================

    [Fact]
    public void Car_TripCost_EqualsFuelCost()
    {
        var car = new Car("Lada", 7.0, 50.0, 5);
        // 300 км * 7 л/100 * 50 руб/л = 1050
        Assert.Equal(1050.0, car.TripCost(300), precision: 6);
    }

    [Fact]
    public void Truck_TripCost_IncludesPayloadSurcharge()
    {
        var truck = new Truck("КАМАЗ", 30.0, 60.0, 20.0);
        // Топливо: 300 * 30/100 * 60 = 5400
        // Надбавка: 300 * 0.5 * 20 = 3000
        // Итого: 8400
        Assert.Equal(8400.0, truck.TripCost(300), precision: 6);
    }

    [Fact]
    public void Bus_TripCost_IncludesComfortFee()
    {
        var bus = new Bus("ЛИАЗ", 40.0, 50.0, 100);
        // Топливо: 300 * 40/100 * 50 = 6000
        // Плата за комфорт: 300 * 2 * 100 = 60000
        // Итого: 66000
        Assert.Equal(66000.0, bus.TripCost(300), precision: 6);
    }

    [Fact]
    public void Vehicle_NegativeDistance_Throws()
    {
        var car = new Car("Lada", 7.0, 50.0, 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => car.TripCost(-1));
    }

    // ===================== Полиморфизм =====================

    [Fact]
    public void CapacityInfo_DifferentTypes_ReturnDifferentStrings()
    {
        Vehicle car = new Car("Lada", 7.0, 50.0, 5);
        Vehicle truck = new Truck("КАМАЗ", 30.0, 60.0, 20.0);
        Vehicle bus = new Bus("ЛИАЗ", 40.0, 50.0, 100);

        Assert.Contains("легковой", car.CapacityInfo());
        Assert.Contains("грузовой", truck.CapacityInfo());
        Assert.Contains("автобус", bus.CapacityInfo());
    }

    // ===================== Garage =====================

    [Fact]
    public void Garage_Empty_AverageFuelConsumptionIsZero()
    {
        var garage = new Garage();
        Assert.Equal(0.0, garage.AverageFuelConsumption());
    }

    [Fact]
    public void Garage_Empty_MostEconomicalIsNull()
    {
        var garage = new Garage();
        Assert.Null(garage.MostEconomical(100));
    }

    [Fact]
    public void Garage_Empty_TotalCostIsZero()
    {
        var garage = new Garage();
        Assert.Equal(0.0, garage.TotalTripCost(100));
    }

    [Fact]
    public void Garage_AddNull_Throws()
    {
        var garage = new Garage();
        Assert.Throws<ArgumentNullException>(() => garage.Add(null!));
    }

    [Fact]
    public void Garage_TotalTripCost_SumsAllVehicles()
    {
        var garage = new Garage();
        garage.Add(new Car("Lada", 7.0, 50.0, 5));   // 1050
        garage.Add(new Truck("КАМАЗ", 30.0, 60.0, 20.0)); // 8400

        Assert.Equal(9450.0, garage.TotalTripCost(300), precision: 6);
    }

    [Fact]
    public void Garage_MostEconomical_ReturnsCheapest()
    {
        var garage = new Garage();
        garage.Add(new Car("Lada", 7.0, 50.0, 5));        // 1050
        garage.Add(new Truck("КАМАЗ", 30.0, 60.0, 20.0)); // 8400
        garage.Add(new Bus("ЛИАЗ", 40.0, 50.0, 100));     // 66000

        var cheapest = garage.MostEconomical(300);
        Assert.NotNull(cheapest);
        Assert.Equal("Lada", cheapest!.Name);
    }

    [Fact]
    public void Garage_GroupByType_GroupsCorrectly()
    {
        var garage = new Garage();
        garage.Add(new Car("A", 7.0, 50.0, 5));
        garage.Add(new Car("B", 8.0, 50.0, 5));
        garage.Add(new Truck("C", 30.0, 60.0, 20.0));

        var groups = garage.GroupByType().ToList();
        Assert.Equal(2, groups.Count);
        Assert.Equal(2, groups.First(g => g.Key == "Car").Count());
        Assert.Single(groups.First(g => g.Key == "Truck"));
    }

    // ===================== Vehicle: граничные =====================

    [Fact]
    public void Vehicle_ZeroFuelConsumption_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => new Car("Test", 0.0, 50.0, 5));

    [Fact]
    public void Vehicle_NegativeFuelPrice_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(() => new Car("Test", 7.0, -1.0, 5));
}