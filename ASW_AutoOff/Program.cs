using System;
using System.Threading;
using System.Windows.Forms;

namespace ASW_AutoOff
{
    static class Program
    {
        private static Mutex mutex = new Mutex(false, "ASW_AutoOff_Unique_Mutex_Name");

        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("ASW_AutoOff は既に起動しています。", "多重起動を停止しました。", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            GC.KeepAlive(mutex);
            mutex.ReleaseMutex();
        }
    }
}