using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Vehicles
{
    class CarD : Vehicle
    {
        public CarD(Random generator) : base(5, 70, 0.75, 44, generator)
        {

        }
    }
}

