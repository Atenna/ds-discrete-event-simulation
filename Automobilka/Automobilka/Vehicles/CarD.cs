﻿using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarD : Vehicle
    {
        public CarD(Random generator) : 
            base("D",Constants.volumeOfVehicleD, Constants.speedOfVehicleD, Constants.probabilityOfCrashOfVehicleD, Constants.timeOfRepairOfVehicleD, generator)
        {

        }
    }
}

