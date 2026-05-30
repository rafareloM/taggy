using System;
using taggyManagement.Domain.ValueObjects;

namespace taggyManagement.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; private set; }
        public string Plate { get; private set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; }
        public PropulsionType Propulsion { get; private set; }

        // Combustion-specific
        public decimal? FuelConsumptionKmPerLiter { get; private set; }
        public decimal? CO2GramsPerKm { get; private set; }

        // Electric-specific
        public decimal? BatteryKwhPerKm { get; private set; }

        public Vehicle(string plate, string brand, string model, int year, PropulsionType propulsion)
        {
            Id = Guid.NewGuid();
            Plate = plate ?? throw new ArgumentNullException(nameof(plate));
            Brand = brand;
            Model = model;
            Year = year;
            Propulsion = propulsion;
        }

        public void SetCombustionMetrics(decimal kmPerLiter, decimal co2GramsPerKm)
        {
            if (Propulsion != PropulsionType.Combustion) throw new InvalidOperationException("Vehicle is not combustion propulsion");
            if (kmPerLiter <= 0) throw new ArgumentOutOfRangeException(nameof(kmPerLiter));
            if (co2GramsPerKm < 0) throw new ArgumentOutOfRangeException(nameof(co2GramsPerKm));
            FuelConsumptionKmPerLiter = kmPerLiter;
            CO2GramsPerKm = co2GramsPerKm;
        }

        public void SetElectricMetrics(decimal kwhPerKm)
        {
            if (Propulsion != PropulsionType.Electric) throw new InvalidOperationException("Vehicle is not electric propulsion");
            if (kwhPerKm <= 0) throw new ArgumentOutOfRangeException(nameof(kwhPerKm));
            BatteryKwhPerKm = kwhPerKm;
        }
    }
}