using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NAppUpdate.Updater
{
    public partial class ConsoleForm : Form
    {
        public ConsoleForm()
        {
            InitializeComponent();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            rtbConsole.Clear();
        }

        public void WriteLine()
        {
            rtbConsole.AppendText(Environment.NewLine);
        }

        public void WriteLine(string message)
        {
            rtbConsole.AppendText(message);
            rtbConsole.AppendText(Environment.NewLine);
        }

        public void WriteLine(string message, params object[] args)
        {
            WriteLine(string.Format(message, args));
        }

        public void ReadKey()
        {
            // attach the keypress event and then wait for it to receive something
            this.KeyPress += ConsoleForm_KeyPress;
            rtbConsole.ReadOnly = false;
            while (_keyPresses == 0) Application.DoEvents();
        }

        private int _keyPresses;
        private void ConsoleForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.KeyPress -= ConsoleForm_KeyPress;
            _keyPresses += 1;
        }

    }
}
