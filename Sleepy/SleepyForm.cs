using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sleepy
{
    public partial class SleepyForm : Form
    {
        private int SC_MONITORPOWER = 0xF170;
        private uint WM_SYSCOMMAND = 0x0112;

        //For sleeping monitors
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public SleepyForm()
        {
            InitializeComponent();
        }

        private const int MYACTION_HOTKEY_ID = 1;
        private void RegisterHotkeys()
        {
            // Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            // Compute the addition of each combination of the keys you want to be pressed
            // ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...
            int Ctrl = 1;
            int Alt = 2;

            RegisterHotKey(this.Handle, MYACTION_HOTKEY_ID, Ctrl + Alt, (int)Keys.S);
        }

        private void SchnozOff()
        {
            SendMessage(this.Handle, WM_SYSCOMMAND, (IntPtr)SC_MONITORPOWER, (IntPtr)2);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == MYACTION_HOTKEY_ID)
            {
                SchnozOff();
            }
            base.WndProc(ref m);
        }

        private void sysTrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SchnozOff();
            } else if (e.Button == MouseButtons.Middle)
            {
                Application.Exit();
            }
        }

        private void SleepyForm_Shown(object sender, EventArgs e)
        {
            RegisterHotkeys();
            Hide();
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
