using System;
using Automobilka.Vehicles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Simulations
{
    class SimulationVariantB : SimulationCore
    {
        private LinkedList<Vehicle> cars;

        public SimulationVariantB() : base()
        {
            cars.AddLast((Vehicle)new CarA());
            cars.AddLast((Vehicle)new CarC());
            cars.AddLast((Vehicle)new CarE());

        }
    }
}
