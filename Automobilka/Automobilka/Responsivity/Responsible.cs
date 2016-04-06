using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Responsivity
{
    // interface for Windows Forms
    interface Responsible
    {
        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e);
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e);
        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e);
    }
}
