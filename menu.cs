using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carShowroom
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            add add = new add();
            add.Show();
            Hide();
        }

        private void menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            edit edit = new edit();
            edit.Show();
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addOrder addOrder = new addOrder();
            addOrder.Show();
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            allOrder allOrder = new allOrder();
            allOrder.Show();
            Hide();
        }
    }
}
