using Automobilka.Vehicles;
using System.Collections.Generic;
using System.Linq;
using Automobilka.Simulations;

namespace Automobilka.GUI
{
    public class Graphics
    {
        // prekresluje GUIcko
        public static void Repaint(SimulationCore simulation, Form1 form)
        {
            form.label12.Text = "Material: " + simulation.MaterialA;
            form.label13.Text = "Material: " + simulation.MaterialB;
            form.label19.Text = "Actual time: " + (simulation.GetSimTime() / 60).ToString("#.000");
            // nastavenie dlzky radu pred depom a budovou

            RepaintQueueListA(simulation, form);
            RepaintQueueListB(simulation, form);
            RepaintLoad(simulation, form);
            RepaintUnload(simulation, form);
            RepaintAb(simulation, form);
            RepaintBc(simulation, form);
            RepaintCa(simulation, form);
        }

        public static void RepaintClear(Form1 form)
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

        private static void RepaintCa(SimulationCore simulation, Form1 form)
        {
            string text = "C-A ";
            List<Vehicle> ca = simulation.GetCarsCa();

            if (ca.Count == 0)
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

        private static void RepaintBc(SimulationCore simulation, Form1 form)
        {
            string text = "B-C ";
            List<Vehicle> bc = simulation.GetCarsBc();
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

        private static void RepaintUnload(SimulationCore simulation, Form1 form)
        {
            Vehicle car = simulation.CarAtUnloader;
            if (car != null)
            {
                double[] unloadProgress = simulation.GetProgressOfUnloading();
                form.label17.Text = car.Name + ": [" +
                    unloadProgress[0].ToString("#.0") + " / " +
                    unloadProgress[1].ToString("#.0") + "], " + car.Speed;
            }
            else
            {
                form.label17.Text = "";
            }
        }

        private static void RepaintLoad(SimulationCore simulation, Form1 form)
        {
            Vehicle car = simulation.CarAtLoader;
            if (car != null)
            {
                form.label16.Text = car.Name + ": [" +
                    (simulation.GetProgressOfLoading()[0]).ToString("#.0") +
                    " / " + car.Volume + "], " + car.Speed;
            }
            else
            {
                form.label16.Text = "";
            }
        }

        private static void RepaintQueueListA(SimulationCore simulation, Form1 form)
        {
            form.listBox1.Items.Clear();
            List<Vehicle> frontA = simulation.GetQueueDepo().GetList();
            if (frontA.Count() > 0)
            {
                for (int i = 0; i < frontA.Count(); i++)
                {
                    form.listBox1.Items.Add(frontA[i].toString() + "\n");
                }
            }
        }

        private static void RepaintQueueListB(SimulationCore simulation, Form1 form)
        {
            form.listBox2.Items.Clear();
            List<Vehicle> frontB = simulation.GetQueueBuilding().GetList();
            if (frontB.Count() > 0)
            {
                for (int i = 0; i < frontB.Count(); i++)
                {
                    form.listBox2.Items.Add(frontB[i].toString() + "\n");
                }
            }
        }

        private static void RepaintAb(SimulationCore simulation, Form1 form)
        {
            string text = "A-B ";
            List<Vehicle> ab = simulation.GetCarsAb();

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
    }
}
