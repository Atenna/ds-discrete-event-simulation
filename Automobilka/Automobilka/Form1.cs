using Automobilka.Events;
using Automobilka.Responsivity;
using Automobilka.Simulations;
using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

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


        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if(textBox2.Text=="")
            {
                textBox2.Text = "Generator seed";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
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
            if (comboBoxString == "Variant A") {
                variant = 1;
            } else if(comboBoxString == "Variant B")
            {
                variant = 2;
            } else
            {
                variant = 3;
            }
            Console.WriteLine("Variant "+variant);
        }

        public Form1()
        {
            InitializeComponent();
            seedGenerator = (seed != 0) ? new Random() : new Random(seed);
            variant = -1;
            maxTime = 1000;
            replications = 100;

            // inicializacia generatorov
            generatorCarA = new Random(seedGenerator.Next());
            generatorCarB = new Random(seedGenerator.Next());
            generatorCarC = new Random(seedGenerator.Next());
            generatorCarD = new Random(seedGenerator.Next());
            generatorCarE = new Random(seedGenerator.Next());

            // vytvorenie simulacie pre kazdu moznost s generatormi pre auta
            simulationA = new SimulationVariantA(generatorCarA, generatorCarB, generatorCarC, generatorCarD, maxTime, replications, backgroundWorker1);

            Event initialEvent = new EventVehiclesInit(simulationA, 0, simulationA.getCarsInitial());

            simulationA.init = initialEvent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // the simulation background thread can start, if we don't have any errors
            if (!backgroundWorker1.IsBusy && isReadyToSimulate())
            {
                Console.WriteLine("Priputajte sa");
                backgroundWorker1.RunWorkerAsync();
            }
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
            //Console.WriteLine("Ready to simulate " + check);
            return label1.Text == "";
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // tu pobezi simulacia vytvorena niekde vyssie, jej instancia bude volat v ProgressChanged
            // Simulaca test = new Simulacia(backgroundWorker1);
            // test.simulate();
            //Console.WriteLine("Odlietame");
            
            simulationA.backgroundProcess();
            //
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        public void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // instancia beziacej simulacie bude updatovat GUIcko, napriklad aj progressBar
            progressBar1.Value = e.ProgressPercentage;
            //Console.WriteLine("Tu by sme sa mali dostat");
        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            } else
            {
                // vypis na nejaky label, ze sa nema co zastavit, resp
                // bude tento butoon locked
            }
        }
    }
}
