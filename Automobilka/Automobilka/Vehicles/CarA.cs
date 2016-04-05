using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Vehicles
{
    class CarA : Vehicle
    {
        public CarA(Random generator) : base(10, 60, 0.12, 80, generator)
        {
            
        }
    }
}
