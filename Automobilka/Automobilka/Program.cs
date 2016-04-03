﻿using Automobilka.Simulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Automobilka
{
    static class Program
    {
        private static int seed;
        private static Random seedGenerator, generatorCarA, generatorCarB, generatorCarC, generatorCarD, generatorCarE;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
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

            // vytvorenie simulacie pre kazdu moznost s generatormi pre auta
            SimulationCore simulationA = new SimulationVariantA();
            SimulationCore simulationB = new SimulationVariantB();
            SimulationCore simulationC = new SimulationVariantC();
        }
    }
}
