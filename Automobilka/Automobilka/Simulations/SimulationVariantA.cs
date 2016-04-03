using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Simulations
{
    class SimulationVariantA : SimulationCore
    {
        private LinkedList<Vehicle> cars;

        public SimulationVariantA() : base()
        {
            cars.AddLast((Vehicle)new CarA());
            cars.AddLast((Vehicle)new CarB());
            cars.AddLast((Vehicle)new CarC());
            cars.AddLast((Vehicle)new CarD());
        }
        
        

    }
}
