using System;
using Lab2;

// Создаём автопарк
var garage = new Garage();

garage.Add(new Car("Lada Vesta", 7.1, 55.0, 5));
garage.Add(new Car("Toyota Camry", 8.5, 60.0, 5));
garage.Add(new Truck("КАМАЗ-5490", 30.0, 58.0, 20.0));
garage.Add(new Truck("ГАЗель Next", 15.0, 56.0, 1.5));
garage.Add(new Bus("ЛИАЗ-5292", 40.0, 57.0, 100));

Console.WriteLine("=== Автопарк ===");
foreach (Vehicle v in garage.Vehicles)
    Console.WriteLine(v);

Console.WriteLine();

// Поездки на 300 км — полиморфизм: у каждого свой TripCost
const double distance = 300.0;
Console.WriteLine($"=== Поездки на {distance} км ===");
foreach (Vehicle v in garage.Vehicles)
{
    Console.WriteLine($"{v.Name,-15} | {v.CapacityInfo(),-30} | стоимость: {v.TripCost(distance):F2} руб");
}

Console.WriteLine();
Console.WriteLine($"Суммарная стоимость всех поездок: {garage.TotalTripCost(distance):F2} руб");
Console.WriteLine($"Средний расход по автопарку:     {garage.AverageFuelConsumption():F2} л/100км");

var cheapest = garage.MostEconomical(distance);
if (cheapest is not null)
    Console.WriteLine($"Самый экономичный:               {cheapest.Name} ({cheapest.TripCost(distance):F2} руб)");

Console.WriteLine();
Console.WriteLine("=== Группировка по типу ===");
foreach (var group in garage.GroupByType())
    Console.WriteLine($"{group.Key}: {group.Count()} шт.");