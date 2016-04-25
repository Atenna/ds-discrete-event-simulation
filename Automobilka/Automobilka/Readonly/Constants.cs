using System.Threading;

namespace Automobilka.Readonly
{
    public class Constants
    {
        public const double MaterialToLoad = 5000;
        public const double LoadMachinePerformance = 25 / 60.0;
        public const double UnloadMachinePerformance = 200 / 60.0;

        public const int AbLength = 45;
        public const int BcLength = 15;
        public const int CaLength = 35;

        public const int VolumeOfVehicleA = 10;
        public const int VolumeOfVehicleB = 20;
        public const int VolumeOfVehicleC = 25;
        public const int VolumeOfVehicleD = 5;
        public const int VolumeOfVehicleE = 40;

        public const int SpeedOfVehicleA = 60;
        public const int SpeedOfVehicleB = 50;
        public const int SpeedOfVehicleC = 45;
        public const int SpeedOfVehicleD = 70;
        public const int SpeedOfVehicleE = 30;

        public const double ProbabilityOfCrashOfVehicleA = 0.12;
        public const double ProbabilityOfCrashOfVehicleB = 0.04;
        public const double ProbabilityOfCrashOfVehicleC = 0.04;
        public const double ProbabilityOfCrashOfVehicleD = 0.11;
        public const double ProbabilityOfCrashOfVehicleE = 0.06;

        public const int TimeOfRepairOfVehicleA = 80;
        public const int TimeOfRepairOfVehicleB = 50;
        public const int TimeOfRepairOfVehicleC = 100;
        public const int TimeOfRepairOfVehicleD = 44;
        public const int TimeOfRepairOfVehicleE = 170;

        public static readonly object Gate = new object();
        public static readonly object GateF = new object();
        //kreslenie nalozeneho/ vylozeneho materialu
        public static readonly object GateG = new object();

        public static ManualResetEvent DoneEvent = new ManualResetEvent(true);
    }
}
