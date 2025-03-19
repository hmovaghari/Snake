using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake.Desktop
{
    public partial class frmLoading : Form
    {
        public frmLoading()
        {
            InitializeComponent();
        }

        private void frmLoading_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;
            }
            else
            {
                lblPressKey.Visible = true;
                timer.Stop();
                timer.Enabled = false;
            }
        }

        private void frmLoading_KeyUp(object sender, KeyEventArgs e)
        {
            if (!timer.Enabled)
            {
                var location = Location;
                this.Hide();
                var frmGame = new frmGame();
                frmGame.Location = location;
                frmGame.ShowDialog();
                Environment.Exit(0);
            }
        }
    }
}
