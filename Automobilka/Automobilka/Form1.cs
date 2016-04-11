using Automobilka.Events;
using Automobilka.Responsivity;
using Automobilka.Simulations;
using System;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using Automobilka.SimulationObjects;
using Automobilka.GUI;
using Automobilka.Vehicles;
using Automobilka.Readonly;
using System.Windows.Forms.DataVisualization.Charting;

namespace Automobilka
{
    public partial class Form1 : Form, Responsible
    {
        private static int seed;
        private static Random seedGenerator;
        private int variant;
        private bool paused = false;
        private int maxTime { get; set; }
        private int replications { get; set; }

        SimulationVariantA simulationA;
        SimulationVariantB simulationB;
        SimulationVariantC simulationC;

        bool isChecked;

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
            button2.Enabled = false;
            button3.Enabled = false;
            isChecked = false;
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series.Clear();
            var series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Replications",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            this.chart1.Series.Add(series1);
        }

        private void initializeSimulationInstances()
        {
            Vehicle carA = new CarA(seedGenerator);
            Vehicle carB = new CarB(seedGenerator);
            Vehicle carC = new CarC(seedGenerator);
            Vehicle carD = new CarD(seedGenerator);
            Vehicle carE = new CarE(seedGenerator);

            simulationA = new SimulationVariantA(maxTime, replications, backgroundWorker1, seedGenerator);
            simulationA.initCars(carA, carB, carC, carD);
            Event initialEventA = new EventVehiclesInit(simulationA, 0, simulationA.getCarsInitial());
            simulationA.init = initialEventA;

            simulationB = new SimulationVariantB(maxTime, replications, backgroundWorker1, seedGenerator);
            simulationB.initCars(carA, carC, carE);
            Event initialEventB = new EventVehiclesInit(simulationB, 0, simulationB.getCarsInitial());
            simulationB.init = initialEventB;

            simulationC = new SimulationVariantC(maxTime, replications, backgroundWorker1, seedGenerator);
            simulationC.initCars(carB, carC, carD);
            Event initialEventC = new EventVehiclesInit(simulationC, 0, simulationC.getCarsInitial());
            simulationC.init = initialEventC;

            if (variant == 1)
            {
                simulationA.isVisualized = isChecked;
            }
            else if (variant == 2)
            {
                simulationB.isVisualized = isChecked;
            }
            else if (variant == 3)
            {
                simulationC.isVisualized = isChecked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // the simulation background thread can start, if we don't have any errors
            if (!backgroundWorker1.IsBusy && isReadyToSimulate())
            {
                button2.Enabled = true;
                button3.Enabled = true;
                Console.WriteLine("Replications " + replications);
                initializeSimulationInstances();
                trackBar1_Scroll(this, e);
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!paused)
            {
                Constants.doneEvent.Reset();
                paused = true;
            }
            else
            {
                Constants.doneEvent.Set();
                paused = false;
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
            
            return label1.Text == "";
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
            }

            if (variant == 1)
            {
                simulationA.backgroundProcess();
            } else if(variant == 2)
            {
                simulationB.backgroundProcess();
            } else if(variant == 3)
            {
                simulationC.backgroundProcess();
            }
        }

        public void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // instancia beziacej simulacie bude updatovat GUIcko, napriklad aj progressBar
            progressBar1.Value = e.ProgressPercentage;
            lock (Constants.gate)
            {
                lock (Constants.gateF)
                {
                    if (variant == 1)
                    {
                        if (checkBox1.Checked)
                        {
                            Graphics.repaint(simulationA, this);
                        }
                        showStats(simulationA.getStats());
                    }
                    else if (variant == 2)
                    {
                        if (checkBox1.Checked)
                        {
                            Graphics.repaint(simulationB, this);
                        }
                        showStats(simulationB.getStats());
                    }
                    else if (variant == 3)
                    {
                        if (checkBox1.Checked)
                        {
                            Graphics.repaint(simulationC, this);
                        }
                        showStats(simulationC.getStats());
                    }
                }
            }
        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Graphics.repaintClear(this);
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            else
            {
                button2.Enabled = false;
                button3.Enabled = false;
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
            //Graphics.repaintClear(this);
            showStats(stats);
            showIS(stats);
        }

        public void showStats(Statistics stats)
        {
            label2.Text = "Average run time: " + stats.getStatsMeanSimulationTime() / 60;
            label3.Text = "Depo: " + stats.getStatsMeanLoadQueueLength();
            label4.Text = "Building: " + stats.getStatsMeanUnloadQueueLength();
            label5.Text = "Depo: " + stats.getStatsMeanLoadQueueTime();
            label6.Text = "Building: " + stats.getStatsMeanUnloadQueueTime();
            label7.Text = "Depo: " + stats.getStatsSumMeanLoadQueueTime() / 60;
            label8.Text = "Building: " + stats.getStatsSumMeanUnloadQueueTime() / 60;
       }

        public void showIS(Statistics stats)
        {
            double[] _IS = stats.confidenceIntervalSimulationTime(0.9);
            label18.Text = "Interval: <" + (_IS[0] / 60).ToString("#.000") + ", " + (_IS[1] / 60).ToString("#.000") + ">";
            double[] _is = stats.confidenceIntervalSimulationTime(0.9);
            label18.Text = "Interval: <" + (_is[0] / 60).ToString("#.000") + ", " + (_is[1] / 60).ToString("#.000") + ">";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int value = trackBar1.Value;
            if (variant == 1)
            {
                simulationA.setSpeed(value);
            }
            else if (variant == 2)
            {
                simulationB.setSpeed(value);
            }
            else if(variant == 3)
            {
                simulationC.setSpeed(value);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isChecked = checkBox1.Checked;
        }
    }
}
