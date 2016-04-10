using Automobilka.Readonly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Vehicles
{
    class CarA : Vehicle
    {
        public CarA(Random generator) : 
            base("A", Constants.volumeOfVehicleA, Constants.speedOfVehicleA, Constants.probabilityOfCrashOfVehicleA, Constants.timeOfRepairOfVehicleA, generator)
        {

        }
    }
}
