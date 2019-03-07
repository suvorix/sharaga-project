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

        // Закрытие программы
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Переход на форму добавления
        private void button2_Click(object sender, EventArgs e)
        {
            add add = new add();
            add.Show();
            Hide();
        }

        // Полное закрытие программы
        private void menu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Переход на форму редактирования
        private void button1_Click(object sender, EventArgs e)
        {
            edit edit = new edit();
            edit.Show();
            Hide();
        }

        // Переход на форму добавления заказа
        private void button3_Click(object sender, EventArgs e)
        {
            addOrder addOrder = new addOrder();
            addOrder.Show();
            Hide();
        }

        // Переход на форму просмотра заказов
        private void button4_Click(object sender, EventArgs e)
        {
            allOrder allOrder = new allOrder();
            allOrder.Show();
            Hide();
        }

        // Переход на форму импорта данных
        private void button5_Click(object sender, EventArgs e)
        {
            import import = new import();
            import.Show();
            Hide();
        }
    }
}
