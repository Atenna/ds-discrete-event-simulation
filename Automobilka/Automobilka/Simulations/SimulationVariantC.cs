using System;
using Automobilka.Vehicles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Automobilka.Simulations
{
    class SimulationVariantC : SimulationCore
    {
        private Vehicle[] cars;

        public bool paused { get; set; }

        public SimulationVariantC(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker, seedGeneratorInit)
        {
            cars = new Vehicle[3];
            paused = false;
        }
        public void initCars(Vehicle car1, Vehicle car2, Vehicle car3)
        {
            cars[0] = car1;
            cars[1] = car2;
            cars[2] = car3;
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
            if (!base.condition())
                return false;
            return !worker.CancellationPending && !paused;
        }
    }
}
