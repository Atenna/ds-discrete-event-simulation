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
    public partial class Form1 : Form, IResponsible
    {
        private static int _seed;
        private static Random _seedGenerator;
        private int _variant;
        private bool _paused = false;
        private int MaxTime { get; set; }
        private int Replications { get; set; }
        private int _replicationsDone;
        private int _lastRepaintReplication;
        private double _cutRate = 0.3;

        private double _maxSimTime = 0;
        private double _minSimTime = Double.MaxValue;
        private int _lastUpdated;
        private int _graphJumpRate = 1000;

        SimulationVariantA _simulationA;
        SimulationVariantB _simulationB;
        SimulationVariantC _simulationC;

        bool _isChecked;

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
                _variant = 1;
            }
            else if (comboBoxString == "Variant B")
            {
                _variant = 2;
            }
            else
            {
                _variant = 3;
            }
            Console.WriteLine("Variant " + _variant);
        }

        public Form1()
        {
            InitializeComponent();
            trackBar1.Minimum = 200;
            trackBar1.Maximum = 3000;
            _seedGenerator = (_seed != 0) ? new Random() : new Random(_seed);
            _variant = -1;
            MaxTime = Int32.MaxValue;
            Replications = 100;
            backgroundWorker1.WorkerSupportsCancellation = true;
            button2.Enabled = false;
            button3.Enabled = false;
            _isChecked = false;
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

        private void InitializeSimulationInstances()
        {
            Vehicle carA = new CarA(_seedGenerator);
            Vehicle carB = new CarB(_seedGenerator);
            Vehicle carC = new CarC(_seedGenerator);
            Vehicle carD = new CarD(_seedGenerator);
            Vehicle carE = new CarE(_seedGenerator);

            _simulationA = new SimulationVariantA(MaxTime, Replications, backgroundWorker1, _seedGenerator);
            _simulationA.InitCars(carA, carB, carC, carD);
            Event initialEventA = new EventVehiclesInit(_simulationA, 0, _simulationA.GetCarsInitial());
            _simulationA.Init = initialEventA;

            _simulationB = new SimulationVariantB(MaxTime, Replications, backgroundWorker1, _seedGenerator);
            _simulationB.InitCars(carA, carC, carE);
            Event initialEventB = new EventVehiclesInit(_simulationB, 0, _simulationB.GetCarsInitial());
            _simulationB.Init = initialEventB;

            _simulationC = new SimulationVariantC(MaxTime, Replications, backgroundWorker1, _seedGenerator);
            _simulationC.InitCars(carB, carC, carD);
            Event initialEventC = new EventVehiclesInit(_simulationC, 0, _simulationC.GetCarsInitial());
            _simulationC.Init = initialEventC;

            if (_variant == 1)
            {
                _simulationA.IsVisualized = _isChecked;
            }
            else if (_variant == 2)
            {
                _simulationB.IsVisualized = _isChecked;
            }
            else if (_variant == 3)
            {
                _simulationC.IsVisualized = _isChecked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // the simulation background thread can start, if we don't have any errors
            if (!backgroundWorker1.IsBusy && IsReadyToSimulate())
            {
                button2.Enabled = true;
                button3.Enabled = true;
                Console.WriteLine("Replications " + Replications);
                InitializeSimulationInstances();
                trackBar1_Scroll(this, e);
                backgroundWorker1.RunWorkerAsync();

                foreach (var series in chart1.Series)
                {
                    series.Points.Clear();
                }
                _maxSimTime = 0;
                _minSimTime = Double.MaxValue;
                _lastUpdated = 0;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!_paused)
            {
                Constants.DoneEvent.Reset();
                _paused = true;
            }
            else
            {
                Constants.DoneEvent.Set();
                _paused = false;
            }
        }

        public bool IsReadyToSimulate()
        {
            label1.Text = "";

            try
            {
                Replications = int.Parse(textBox1.Text);
                _seed = int.Parse(textBox1.Text);
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
            if (_variant == -1)
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

            if (_variant == 1)
            {
                _simulationA.BackgroundProcess();
            }
            else if (_variant == 2)
            {
                _simulationB.BackgroundProcess();
            }
            else if (_variant == 3)
            {
                _simulationC.BackgroundProcess();
            }
        }

        public void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // instancia beziacej simulacie bude updatovat GUIcko, napriklad aj progressBar
            lock (Constants.Gate)
            {
                lock (Constants.GateF)
                {
                    if (_variant == 1)
                    {
                        _replicationsDone = _simulationA.RetIterator;
                        if (checkBox1.Checked)
                        {
                            Graphics.Repaint(_simulationA, this);
                        }
                        if (_lastRepaintReplication != _replicationsDone)
                        {
                            ShowStats(_simulationA.GetStats());
                            if (_replicationsDone > Replications * _cutRate && _lastUpdated + Replications / _graphJumpRate < _replicationsDone)
                            {
                                _lastUpdated = _replicationsDone;
                                UpdateChart(_simulationA);
                            }
                        }
                    }
                    else if (_variant == 2)
                    {
                        _replicationsDone = _simulationB.RetIterator;
                        if (checkBox1.Checked)
                        {
                            Graphics.Repaint(_simulationB, this);
                        }
                        if (_lastRepaintReplication != _replicationsDone)
                        {
                            ShowStats(_simulationB.GetStats());
                            if (_replicationsDone > Replications * _cutRate && _lastUpdated + Replications / _graphJumpRate < _replicationsDone)
                            {
                                _lastUpdated = _replicationsDone;
                                UpdateChart(_simulationB);
                            }
                        }
                    }
                    else if (_variant == 3)
                    {
                        _replicationsDone = _simulationC.RetIterator;
                        if (checkBox1.Checked)
                        {
                            Graphics.Repaint(_simulationC, this);
                        }
                        if (_lastRepaintReplication != _replicationsDone)
                        {
                            ShowStats(_simulationC.GetStats());
                            if (_replicationsDone > Replications * _cutRate && _lastUpdated + Replications / _graphJumpRate < _replicationsDone)
                            {
                                _lastUpdated = _replicationsDone;
                                UpdateChart(_simulationC);
                            }
                        }
                    }
                    _lastRepaintReplication = _replicationsDone;
                }
            }
            progressBar1.Value = e.ProgressPercentage;
        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Graphics.RepaintClear(this);
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

            if (_variant == 1)
            {
                stats = _simulationA.GetStats();
            }
            else if (_variant == 2)
            {
                stats = _simulationB.GetStats();
            }
            else
            {
                stats = _simulationC.GetStats();
            }
            //Graphics.repaintClear(this);
            ShowStats(stats);
            ShowIs(stats);
        }

        public void ShowStats(Statistics stats)
        {
            label2.Text = "Average run time: " + stats.GetStatsMeanSimulationTime() / 60;
            label3.Text = "Depo: " + stats.GetStatsMeanLoadQueueLength();
            label4.Text = "Building: " + stats.GetStatsMeanUnloadQueueLength();
            label5.Text = "Depo: " + stats.GetStatsMeanLoadQueueTime();
            label6.Text = "Building: " + stats.GetStatsMeanUnloadQueueTime();
            label7.Text = "Depo: " + stats.GetStatsSumMeanLoadQueueTime() / 60;
            label8.Text = "Building: " + stats.GetStatsSumMeanUnloadQueueTime() / 60;
        }

        public void ShowIs(Statistics stats)
        {
            double[] _IS = stats.ConfidenceIntervalSimulationTime(0.9);
            label18.Text = "Interval: <" + (_IS[0] / 60).ToString("#.000") + ", " + (_IS[1] / 60).ToString("#.000") + ">";
            double[] _is = stats.ConfidenceIntervalSimulationTime(0.9);
            label18.Text = "Interval: <" + (_is[0] / 60).ToString("#.000") + ", " + (_is[1] / 60).ToString("#.000") + ">";
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int value = trackBar1.Value;
            if (_variant == 1)
            {
                _simulationA.SetSpeed(value);
            }
            else if (_variant == 2)
            {
                _simulationB.SetSpeed(value);
            }
            else if (_variant == 3)
            {
                _simulationC.SetSpeed(value);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _isChecked = checkBox1.Checked;

            if (_variant == 1)
            {
                if (_simulationA != null)
                {
                    _simulationA.IsVisualized = _isChecked;
                }
            }
            else if (_variant == 2)
            {
                if (_simulationA != null)
                {
                    _simulationB.IsVisualized = _isChecked;
                }
            }
            else if (_variant == 3)
            {
                if (_simulationA != null)
                {
                    _simulationC.IsVisualized = _isChecked;
                }
            }
        }


        private void UpdateChart(SimulationCore simulation)
        {
            double simTime = simulation.GetStats().GetStatsMeanSimulationTime() / 60;
            double time = simulation.GetSimTime();
            int iterator = simulation.GetActualReplication();


            _maxSimTime = simTime > _maxSimTime ? simTime : _maxSimTime;
            _minSimTime = simTime < _minSimTime ? simTime : _minSimTime;
            try
            {
                chart1.ChartAreas[0].AxisY.ScaleView.Zoom(_minSimTime, _maxSimTime);
                chart1.Series[0].Points.AddXY(iterator, simTime);
            }
            catch (NullReferenceException)
            {
            }
        }
    }
}
