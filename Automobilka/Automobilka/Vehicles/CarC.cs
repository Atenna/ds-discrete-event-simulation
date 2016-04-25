using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarC : Vehicle
    {
        public CarC(Random generator) : 
            base("C",Constants.VolumeOfVehicleC, Constants.SpeedOfVehicleC, Constants.ProbabilityOfCrashOfVehicleC, Constants.TimeOfRepairOfVehicleC, generator)
        {

        }
    }
}

