using Automobilka.Events;
using Automobilka.Responsivity;
using Automobilka.Simulations;
using System;
using System.Windows.Forms;
using System.ComponentModel;
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

        private Series series1;

        bool isChecked;

        private Vehicle carA;
        private Vehicle carB;
        private Vehicle carC;
        private Vehicle carD;
        private Vehicle carE;

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
            series1 = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Replications",
                Color = System.Drawing.Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line
            };

            this.chart1.Series.Add(series1);

            //chart1.ChartAreas[0].AxisY.ScaleView.Zoom(posYStart, posYFinish);
            /*
            for(int i = 0; i < 100; i++)
            {
                series1.Points.AddXY(i, i);
            }
            */
        }

        private void initializeSimulationInstances()
        {
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
        // run button
        private void button1_Click(object sender, EventArgs e)
        {
            
            // the simulation background thread can start, if we don't have any errors
            if (!backgroundWorker1.IsBusy && isReadyToSimulate())
            {
                seedGenerator = (seed != 0) ? new Random() : new Random(seed);

                carA = new CarA(seedGenerator);
                carB = new CarB(seedGenerator);
                carC = new CarC(seedGenerator);
                carD = new CarD(seedGenerator);
                carE = new CarE(seedGenerator);

                button2.Enabled = true;
                button3.Enabled = true;
                if (!isChecked)
                {
                    // nebude mozne posuvas simulaciu ak nie je zapnuta vizualizacia
                    trackBar1.Enabled = false;
                } else
                {
                    trackBar1.Enabled = true;
                }
                //Console.WriteLine("Replications " + replications);
                initializeSimulationInstances();
                trackBar1_Scroll(this, e);
                button1.Enabled = false;
                backgroundWorker1.RunWorkerAsync();
            }
        }

        // stop button
        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            Graphics.repaintClearStop(this);
            button1.Enabled = true;
            restartSimulations();
        }
        // po zruseni simulacie tlacidlom Stop sa resetuju vsetky nastavenia simulacie
        private void restartSimulations()
        {
            simulationA.prePreSetup();
            simulationA.preSetup();
            simulationB.prePreSetup();
            simulationB.preSetup();
            simulationC.prePreSetup();
            simulationC.preSetup();
        }

        // pause button
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
        int replication = 0;
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
                        Graphics.repaint(simulationA, this, isChecked);
                        if(simulationA.getActualReplication() > replication && (simulationA.getActualReplication()%30==0))
                        {
                            this.chart1.ChartAreas[0].AxisY.ScaleView.Zoom(simulationA.getStats().getMinAvgSimTime(), simulationA.getStats().getMaxAvgSimTime());
                            Graphics.repaintGraph(simulationA, this, series1);
                            replication = simulationA.getActualReplication();
                        }
                    }
                    else if (variant == 2)
                    {
                        Graphics.repaint(simulationB, this, isChecked);
                    }
                    else if (variant == 3)
                    {
                        Graphics.repaint(simulationC, this, isChecked);
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
            Graphics.repaintClear(this);
            showStats(stats);
            showIS(stats);
            button1.Enabled = true;
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

        private static double minSimTime = 0;
        private static double simTimeCumulative = 0, maxSimTime = 0;
        private static int iterator = 0;

        private static void updateChart(SimulationCore simulation, Form1 form)
        {
            double simTime = simulation.getStats().getStatsMeanSimulationTime();
            double time = simulation.getSimTime();
            iterator = simulation.getActualReplication();
            simTimeCumulative += time;
            maxSimTime = simTime > maxSimTime ? simTime : maxSimTime;
            minSimTime = simTime < minSimTime ? simTime : minSimTime;

            form.chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, simTimeCumulative);
            form.chart1.ChartAreas[0].AxisY.ScaleView.Zoom(minSimTime, maxSimTime);

            form.chart1.Series[0].Points.AddXY(iterator, simTime / 60);
            form.chart1.Series[0].Points.AddXY(iterator, simTime / 60);
        }
    }
}
