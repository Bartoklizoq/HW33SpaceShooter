using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HW33SpaceShooter.Objects;

namespace HW33SpaceShooter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Form form = new Form();
            form.Width = 1200;
            form.Height = 740;
            if (form.Width > 1200 || form.Width < 0 || form.Height > 1000 || form.Height < 0)
                throw new ArgumentOutOfRangeException("Incorrect Screen size!");

            form.KeyDown += Ship.UpdateOnKeyDown;
            form.KeyUp += Ship.UpdateOnKeyUp;
            form.FormClosing += form_FormClosing;

            form.Show();
            Game.Init(form);
            Game.Draw();

            Game.timer.Stop();
            Game.timer.Tick -= Game.Timer_Tick;

            Game.timer.Start();
            Game.timer.Tick += Game.Timer_Tick;
        }

        private void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Game.timer.Stop();
            MessageBox.Show($"Game Over. Your score is {Game.score}");
            Game._objsBullets.Clear();
            Game._objsStar.Clear();
            Game._objsInteraction.Clear();
            Game._objsShip.Clear();
        }
    }
}

