using Automobilka.Events;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Simulations
{
    class SimulationVariantA : SimulationCore
    {
        private Vehicle[] cars;
        // vytvorit novu triedu na rad aut 
        

        public SimulationVariantA(Random gCarA, Random gCarB, Random gCarC, Random gCarD, double maxTime, int replications, BackgroundWorker worker) : base(maxTime, replications, worker)
        {
            cars = new Vehicle[4];
            cars[0] = (Vehicle)new CarA(gCarA);
            cars[1] = (Vehicle)new CarB(gCarB);
            cars[2] = (Vehicle)new CarC(gCarC);
            cars[3] = (Vehicle)new CarD(gCarD);
        }
        
        public Vehicle[] getCarsInitial()
        {
            return cars;
        }


    }
}
