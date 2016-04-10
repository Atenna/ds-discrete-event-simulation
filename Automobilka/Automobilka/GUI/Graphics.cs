using Automobilka.SimulationObjects;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.GUI
{
    public class Graphics
    {
        // prekresluje GUIcko
        public static void repaint(SimulationCore simulation, Form1 form)
        {
            form.label12.Text = "Material: " + simulation.materialA;
            form.label13.Text = "Material: " + simulation.materialB;

            // nastavenie dlzky radu pred depom a budovou

            repaintQueueListA(simulation, form);
            repaintQueueListB(simulation, form);
            repaintLoad(simulation, form);
            repaintUnload(simulation, form);
            repaintAB(simulation, form);
            repaintBC(simulation, form);
            repaintCA(simulation, form);
        }

        private static void repaintCA(SimulationCore simulation, Form1 form)
        {
            List<Vehicle> ca = simulation.getCarsCA();
            string text = "C-A ";
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
            List<Vehicle> bc = simulation.getCarsBC();
            string text = "B-C ";
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
            List<Vehicle> ab = simulation.getCarsAB();
            string text = "A-B ";
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
