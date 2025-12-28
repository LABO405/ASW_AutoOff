using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace ASW_AutoOff
{
    public partial class Form1 : Form
    {
        private bool isDetected = false;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Visible = false;

            string runKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(runKey, false))
            {
                if (key.GetValue("ASW_AutoOff") != null)
                {
                    AutoStart.Checked = true;
                }
            }
        }
        private async void timer1_Tick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcessesByName("OculusDash");

            if (processes.Length > 0 && !isDetected)
            {
                isDetected = true;

                await System.Threading.Tasks.Task.Delay(10000);

                SendCtrlNum1();

                notifyIcon1.ShowBalloonTip(3000, "ASW_AutoOff", "スムーズ機能無効化のために Ctrl+Num1 のコマンドを送信しました", ToolTipIcon.Info);
            }
            else if (processes.Length == 0)
            {
                isDetected = false;
            }
        }

        private void SendCtrlNum1()
        {
            const byte VK_CONTROL = 0x11;
            const byte VK_NUMPAD1 = 0x61;
            const uint KEYEVENTF_KEYUP = 0x0002;

            keybd_event(VK_CONTROL, 0, 0, 0);
            keybd_event(VK_NUMPAD1, 0, 0, 0);
            keybd_event(VK_NUMPAD1, 0, KEYEVENTF_KEYUP, 0);
            keybd_event(VK_CONTROL, 0, KEYEVENTF_KEYUP, 0);
        }
        private void autoStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string runKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
            string appName = "ASW_AutoOff";
            string appPath = Application.ExecutablePath;

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(runKey, true))
                {
                    if (AutoStart.Checked)
                    {
                        key.SetValue(appName, "\"" + appPath + "\"");
                        MessageBox.Show("スタートアップに登録しました。\rPC起動時に自動で起動します。", "スタートアップ登録");
                    }
                    else
                    {
                        key.DeleteValue(appName, false);
                        MessageBox.Show("スタートアップから解除しました。", "スタートアップ解除");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("設定の変更に失敗しました: " + ex.Message);
            }
        }

        private void ExitApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}