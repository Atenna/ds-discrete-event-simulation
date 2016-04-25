using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarD : Vehicle
    {
        public CarD(Random generator) : 
            base("D",Constants.VolumeOfVehicleD, Constants.SpeedOfVehicleD, Constants.ProbabilityOfCrashOfVehicleD, Constants.TimeOfRepairOfVehicleD, generator)
        {

        }
    }
}

