using Automobilka.SimulationObjects;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Automobilka.GUI
{
    public class Graphics
    {
        // prekresluje GUIcko
        public static void repaint(SimulationCore simulation, Form1 form)
        {
            form.label12.Text = "Material: " + simulation.materialA;
            form.label13.Text = "Material: " + simulation.materialB;
            form.label19.Text = "Actual time: " + (simulation.getSimTime() / 60).ToString("#.000");
            // nastavenie dlzky radu pred depom a budovou

            repaintQueueListA(simulation, form);
            repaintQueueListB(simulation, form);
            repaintLoad(simulation, form);
            repaintUnload(simulation, form);
            repaintAB(simulation, form);
            repaintBC(simulation, form);
            repaintCA(simulation, form);
            updateChart(simulation, form);
        }

        public static void repaintClear(Form1 form)
        {
            form.label12.Text = "Material: " + 5000;
            form.label13.Text = "Material: " + 0;

            form.label11.Text = "C-A ";
            form.label10.Text = "B-C ";
            form.label17.Text = "";
            form.label16.Text = "";
            form.listBox1.Items.Clear();
            form.listBox2.Items.Clear();
            form.label9.Text = "A-B ";
        }

        private static void repaintCA(SimulationCore simulation, Form1 form)
        {
            string text = "C-A ";
            List<Vehicle> ca = simulation.getCarsCA();
            
            if(ca.Count == 0)
            {
                form.label11.Text = text;
                return;
            }
            foreach (Vehicle v in ca)
            {
                text += v.toString();
            }
            form.label11.Text = text;
        }

        private static void repaintBC(SimulationCore simulation, Form1 form)
        {
            string text = "B-C ";
            List<Vehicle> bc = simulation.getCarsBC();
            if (bc.Count == 0)
            {
                form.label10.Text = text;
                return;
            }
            foreach (Vehicle v in bc)
            {
                text += v.toString();
            }
            form.label10.Text = text;
        }

        private static void repaintUnload(SimulationCore simulation, Form1 form)
        {
            Vehicle car = simulation.carAtUnloader;
            if(car != null)
            {
                form.label17.Text = car.toString();
            } else
            {
                form.label17.Text = "";
            }
        }

        private static void repaintLoad(SimulationCore simulation, Form1 form)
        {
            Vehicle car = simulation.carAtLoader;
            if (car != null)
            {
                form.label16.Text = car.toString();
            }
            else
            {
                form.label16.Text = "";
            }
        }

        private static void repaintQueueListA(SimulationCore simulation, Form1 form)
        {
            form.listBox1.Items.Clear();
            List<Vehicle> frontA = simulation.getQueueDepo().getList();
            if (frontA.Count() > 0)
            {
                for (int i = 0; i < frontA.Count(); i++)
                {
                    form.listBox1.Items.Add(frontA[i].toString() + "\n");
                }
            }
        }

        private static void repaintQueueListB(SimulationCore simulation, Form1 form)
        {
            form.listBox2.Items.Clear();
            List<Vehicle> frontB = simulation.getQueueBuilding().getList();
            if (frontB.Count() > 0)
            {
                for (int i = 0; i < frontB.Count(); i++)
                {
                    form.listBox2.Items.Add(frontB[i].toString() + "\n");
                }
            }
        }

        private static void repaintAB(SimulationCore simulation, Form1 form)
        {
            string text = "A-B ";
            List<Vehicle> ab = simulation.getCarsAB();
            
            if (ab.Count == 0)
            {
                form.label9.Text = text;
                return;
            }
            foreach (Vehicle v in ab)
            {
                text += v.toString();
            }
            form.label9.Text = text;
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

            form.chart1.Series[0].Points.AddXY(iterator, simTime/60);
            form.chart1.Series[0].Points.AddXY(iterator, simTime / 60);
        }
    }
}
