using System;
using Automobilka.Vehicles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Simulations
{
    class SimulationVariantC : SimulationCore
    {
        private LinkedList<Vehicle> cars;

        public SimulationVariantC() : base()
        {
            cars.AddLast((Vehicle)new CarB());
            cars.AddLast((Vehicle)new CarC());
            cars.AddLast((Vehicle)new CarD());
        }
    }
}
