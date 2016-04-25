using System.ComponentModel;

namespace Automobilka.Responsivity
{
    // interface for Windows Forms
    interface IResponsible
    {
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e);
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e);
        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);
    }
}
