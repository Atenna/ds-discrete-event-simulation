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
        //private LinkedList<Vehicle> cars;

        public SimulationVariantB(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker, seedGeneratorInit)
        {
            //cars.AddLast((Vehicle)new CarA());
            //cars.AddLast((Vehicle)new CarC());
            //cars.AddLast((Vehicle)new CarE());

        }
    }
}
