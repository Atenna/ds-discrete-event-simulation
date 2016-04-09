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
        //private LinkedList<Vehicle> cars;

        public SimulationVariantC(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker, seedGeneratorInit)
        {
            //cars.AddLast((Vehicle)new CarB());
            //cars.AddLast((Vehicle)new CarC());
            //cars.AddLast((Vehicle)new CarD());
        }
    }
}
