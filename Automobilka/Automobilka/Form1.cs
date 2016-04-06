using Automobilka.Events;
using Automobilka.Simulations;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Automobilka
{
    public partial class Form1 : Form
    {
        private static int seed;
        private static Random seedGenerator, generatorCarA, generatorCarB, generatorCarC, generatorCarD, generatorCarE;
        private int variant;
        private int maxTime { get; set; }
        private int replications { get; set; }


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

            // inicializacia generatorov
            generatorCarA = new Random(seedGenerator.Next());
            generatorCarB = new Random(seedGenerator.Next());
            generatorCarC = new Random(seedGenerator.Next());
            generatorCarD = new Random(seedGenerator.Next());
            generatorCarE = new Random(seedGenerator.Next());

            // vytvorenie simulacie pre kazdu moznost s generatormi pre auta
            SimulationVariantA simulationA = new SimulationVariantA(generatorCarA, generatorCarB, generatorCarC, generatorCarD, maxTime, replications);

            Event initialEvent = new EventVehiclesInit(simulationA, 0, simulationA.getCarsInitial());

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "";

            try
            {
                replications = int.Parse(textBox1.Text);
                seed = int.Parse(textBox1.Text);
            }
            catch(Exception exc)
            {
                label1.Text += "Error: Accepted integers only \n";
                Console.WriteLine(exc.StackTrace.ToString());
            }

            String comboBoxString = comboBox1.GetItemText(this.comboBox1.SelectedItem);

            if (textBox1.Text == "Number of replications")
            {
                label1.Text += "Error: Incorrect value for number of replications \n";
            }
            if(textBox2.Text == "Generator seed")
            {
                label1.Text += "Error: Incorrect value for generator seed \n";
            }
            if(variant == -1)
            {
                label1.Text += "Error: Select a variant";
            }
        }
    }
}
