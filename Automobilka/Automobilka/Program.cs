using Automobilka.Events;
using Automobilka.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automobilka
{
    public static class Program
    {
        private static int seed;
        private static Random seedGenerator, generatorCarA, generatorCarB, generatorCarC, generatorCarD, generatorCarE;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
            if(seed == 0)
            {
                seedGenerator = new Random();
            } else
            {
                seedGenerator = new Random(seed);
            }

            // inicializacia generatorov
            generatorCarA = new Random(seedGenerator.Next());
            generatorCarB = new Random(seedGenerator.Next());
            generatorCarC = new Random(seedGenerator.Next());
            generatorCarD = new Random(seedGenerator.Next());
            generatorCarE = new Random(seedGenerator.Next());

            double maxTime = 500000;
            int replications = 99;

            // vytvorenie simulacie pre kazdu moznost s generatormi pre auta
            SimulationVariantA simulationA = new SimulationVariantA(generatorCarA, generatorCarB, generatorCarC, generatorCarD, maxTime, replications);
            
            Event initialEvent = new EventVehiclesInit(simulationA, 0, simulationA.getCarsInitial());
            simulationA.simulation(10000);
            /*
            // resetovacia metoda na vymazanie statistik
            SimulationVariantB simulationB = new SimulationVariantB();
            initialEvent = new EventVehiclesInit(simulationA, 0, simulationA.getCarsInitial());
            simulationA.simulation(10000);

            // resetovacia metoda na vymazanie statistik
            SimulationVariantC simulationC = new SimulationVariantC();
            initialEvent = new EventVehiclesInit(simulationA, 0, simulationA.getCarsInitial());
            simulationA.simulation(10000);
            */
        }
    }
}
