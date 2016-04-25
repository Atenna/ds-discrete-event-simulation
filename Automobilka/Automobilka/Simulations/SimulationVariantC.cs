using System;
using Automobilka.Vehicles;
using System.ComponentModel;

namespace Automobilka.Simulations
{
    class SimulationVariantC : SimulationCore
    {
        private Vehicle[] _cars;

        public bool Paused { get; set; }

        public SimulationVariantC(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker, seedGeneratorInit)
        {
            _cars = new Vehicle[3];
            Paused = false;
        }
        public void InitCars(Vehicle car1, Vehicle car2, Vehicle car3)
        {
            _cars[0] = car1;
            _cars[1] = car2;
            _cars[2] = car3;
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
