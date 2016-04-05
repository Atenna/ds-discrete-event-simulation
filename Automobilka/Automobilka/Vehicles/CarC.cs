using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Vehicles
{
    class CarC : Vehicle
    {
        public CarC(Random generator) : base(25, 45, 0.04, 100, generator)
        {

        }
    }
}

