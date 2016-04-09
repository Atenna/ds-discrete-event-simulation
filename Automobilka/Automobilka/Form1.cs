﻿using Automobilka.Events;
using Automobilka.Responsivity;
using Automobilka.Simulations;
using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using Automobilka.SimulationObjects;

namespace Automobilka
{
    public partial class Form1 : Form, Responsible
    {
        private static int seed;
        private static Random seedGenerator, generatorCarA, generatorCarB, generatorCarC, generatorCarD, generatorCarE;
        private int variant;
        private int maxTime { get; set; }
        private int replications { get; set; }

        SimulationVariantA simulationA;
        SimulationVariantB simulationB;
        SimulationVariantC simulationC;


        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.Text = "Generator seed";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                textBox1.Text = "Number of replications";
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String comboBoxString = comboBox1.GetItemText(this.comboBox1.SelectedItem);
            if (comboBoxString == "Variant A")
            {
                variant = 1;
            }
            else if (comboBoxString == "Variant B")
            {
                variant = 2;
            }
            else
            {
                variant = 3;
            }
            Console.WriteLine("Variant " + variant);
        }

        public Form1()
        {
            InitializeComponent();
            seedGenerator = (seed != 0) ? new Random() : new Random(seed);
            variant = -1;
            maxTime = Int32.MaxValue;
            replications = 100;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void initializeSimulationInstances()
        {
            simulationA = new SimulationVariantA(maxTime, replications, backgroundWorker1, seedGenerator);
            Event initialEventA = new EventVehiclesInit(simulationA, 0, simulationA.getCarsInitial());
            simulationA.init = initialEventA;

            simulationB = new SimulationVariantB(maxTime, replications, backgroundWorker1, seedGenerator);
            Event initialEventB = new EventVehiclesInit(simulationB, 0, simulationB.getCarsInitial());
            simulationB.init = initialEventB;

            simulationC = new SimulationVariantC(maxTime, replications, backgroundWorker1, seedGenerator);
            Event initialEventC = new EventVehiclesInit(simulationC, 0, simulationC.getCarsInitial());
            simulationC.init = initialEventC;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // the simulation background thread can start, if we don't have any errors
            if (!backgroundWorker1.IsBusy && isReadyToSimulate())
            {
                Console.WriteLine("Replications " + replications);
                initializeSimulationInstances();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
        }

        public bool isReadyToSimulate()
        {
            label1.Text = "";

            try
            {
                replications = int.Parse(textBox1.Text);
                seed = int.Parse(textBox1.Text);
            }
            catch (Exception exc)
            {
                label1.Text += "Error: Accepted integers only \n";
                Console.WriteLine(exc.StackTrace.ToString());
            }

            String comboBoxString = comboBox1.GetItemText(this.comboBox1.SelectedItem);

            if (textBox1.Text == "Number of replications")
            {
                label1.Text += "Error: Incorrect value for number of replications \n";
            }
            if (textBox2.Text == "Generator seed")
            {
                label1.Text += "Error: Incorrect value for generator seed \n";
            }
            if (variant == -1)
            {
                label1.Text += "Error: Select a variant";
            }
            string check = label1.Text == "" ? "True" : "False";
            
            return label1.Text == "";
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if(variant == 1)
            {
                simulationA.backgroundProcess();
            } else if(variant == 2)
            {
                simulationB.backgroundProcess();
            } else if(variant == 3)
            {
                simulationC.backgroundProcess();
            }
            

            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        public void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // instancia beziacej simulacie bude updatovat GUIcko, napriklad aj progressBar
            progressBar1.Value = e.ProgressPercentage;
        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            else
            {
                // vypis na nejaky label, ze sa nema co zastavit, resp
                // bude tento butoon locked
            }
            Statistics stats;

            if (variant == 1)
            {
                stats = simulationA.getStats();
            } else if(variant == 2)
            {
                stats = simulationB.getStats();
            } else
            {
                stats = simulationC.getStats();
            }
            
            showStats(stats);
        }

        public void showStats(Statistics stats)
        {
            label2.Text = "Average run time: " + stats.getStatsMeanSimulationTime()/60;
            label3.Text = "Depo: " + stats.getStatsMeanLoadQueueLength();
            label4.Text = "Building: " + stats.getStatsMeanUnloadQueueLength();
            label5.Text = "Depo: " + stats.getStatsMeanLoadQueueTime();
            label6.Text = "Building: " + stats.getStatsMeanUnloadQueueTime();
            label7.Text = "Depo: " + stats.getStatsSumMeanLoadQueueTime()/60;
            label8.Text = "Building: " + stats.getStatsSumMeanUnloadQueueTime()/60;
        }
    }
}
