using Automobilka.Events;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Simulations
{
    class SimulationVariantA : SimulationCore
    {
        private Vehicle[] cars;

        public SimulationVariantA(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker, seedGeneratorInit)
        {
            cars = new Vehicle[4];

            cars[0] = (Vehicle)new CarA(new Random(seedGenerator.Next()));
            cars[1] = (Vehicle)new CarB(new Random(seedGenerator.Next()));
            cars[2] = (Vehicle)new CarC(new Random(seedGenerator.Next()));
            cars[3] = (Vehicle)new CarD(new Random(seedGenerator.Next()));
        }

        public override void resetCars()
        {
            foreach (Vehicle car in cars)
            {
                car.resetAttributes(new Random(seedGenerator.Next()));
            }
        }

        public override void addCars()
        {
            foreach (Vehicle v in cars)
            {
                cruelStats.addVehicleToStats(v);
            }
        }

        public Vehicle[] getCarsInitial()
        {
            return cars;
        }

        public override bool condition()
        {
            return !worker.CancellationPending;
        }
    }
}
