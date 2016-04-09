using System;
using Automobilka.Vehicles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Automobilka.Simulations
{
    class SimulationVariantB : SimulationCore
    {
        private Vehicle[] cars;

        public SimulationVariantB(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker, seedGeneratorInit)
        {
            cars = new Vehicle[3];

            cars[0] = (Vehicle)new CarA(new Random(seedGenerator.Next()));
            cars[1] = (Vehicle)new CarC(new Random(seedGenerator.Next()));
            cars[2] = (Vehicle)new CarE(new Random(seedGenerator.Next()));

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
