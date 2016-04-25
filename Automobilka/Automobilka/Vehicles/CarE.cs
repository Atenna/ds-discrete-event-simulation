using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarE : Vehicle
    {
        public CarE(Random generator) : 
            base("E",Constants.VolumeOfVehicleE, Constants.SpeedOfVehicleE, Constants.ProbabilityOfCrashOfVehicleE, Constants.TimeOfRepairOfVehicleE, generator)
        {

        }
    }
}

