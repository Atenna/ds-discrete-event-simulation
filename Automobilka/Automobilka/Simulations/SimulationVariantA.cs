using Automobilka.Vehicles;
using System;
using System.ComponentModel;

namespace Automobilka.Simulations
{
    class SimulationVariantA : SimulationCore
    {
        private Vehicle[] _cars;
        public bool Paused { get; set; }

        public SimulationVariantA(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker, seedGeneratorInit)
        {
            _cars = new Vehicle[4];
            Paused = false;
        }

        public void InitCars(Vehicle car1, Vehicle car2, Vehicle car3, Vehicle car4)
        {
            _cars[0] = car1;
            _cars[1] = car2;
            _cars[2] = car3;
            _cars[3] = car4;
        }

        public override void ResetCars()
        {
            foreach (Vehicle car in _cars)
            {
                car.ResetAttributes(new Random(SeedGenerator.Next()));
            }
        }

        public override void AddCars()
        {
            foreach (Vehicle v in _cars)
            {
                CruelStats.AddVehicleToStats(v);
            }
        }

        public Vehicle[] GetCarsInitial()
        {
            return _cars;
        }

        public override bool Condition()
        {
            return !Worker.CancellationPending && !Paused;
        }
    }
}
