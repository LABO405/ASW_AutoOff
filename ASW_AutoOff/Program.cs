using System;
using System.Threading; // これが必要
using System.Windows.Forms;

namespace ASW_AutoOff
{
    static class Program
    {
        // アプリ固有の識別名（適当な英数字の羅列でOKです）
        private static Mutex mutex = new Mutex(false, "ASW_AutoOff_Unique_Mutex_Name");

        [STAThread]
        static void Main()
        {
            // Mutexの所有権を試みる
            if (!mutex.WaitOne(0, false))
            {
                // すでに起動している場合はメッセージを出して終了
                MessageBox.Show("ASW_AutoOff は既に起動しています。", "多重起動を停止しました。", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

            // 終了時にMutexを解放
            GC.KeepAlive(mutex);
            mutex.ReleaseMutex();
        }
    }
}