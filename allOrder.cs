using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carShowroom
{
    public partial class allOrder : Form
    {
        public allOrder()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            menu menu = new menu();
            menu.Show();
            Hide();
        }

        private void allOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Подключение класса с функциями
        Function MainFunc = new Function();

        private string cur_id = "";

        private void allOrder_Load(object sender, EventArgs e)
        {
            updateTables();
        }

        private void updateTables()
        {
            try
            {
                // Таблица заказов
                OleDbDataAdapter data1 = new OleDbDataAdapter(MainFunc.sql("SELECT repair_id, (SELECT mechanic_surname FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_surname], (SELECT mechanic_name FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_name], (SELECT mechanic_patronymic FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_patronymic], (SELECT car_name FROM car WHERE repair.car_id = car.car_id) AS [model_car], (SELECT car_mark FROM car WHERE repair.car_id = car.car_id) AS [mark_car], repair_date, repair_cost FROM repair;"));
                DataTable table1 = new DataTable();
                data1.Fill(table1);

                table1.Columns["repair_id"].ColumnName = "ID";
                table1.Columns["m_surname"].ColumnName = "Фамилия";
                table1.Columns["m_name"].ColumnName = "Имя";
                table1.Columns["m_patronymic"].ColumnName = "Отчество";
                table1.Columns["model_car"].ColumnName = "Модель";
                table1.Columns["mark_car"].ColumnName = "Марка";
                table1.Columns["repair_date"].ColumnName = "Дата";
                table1.Columns["repair_cost"].ColumnName = "Стоимость";

                dataGridView1.DataSource = table1;
                dataGridView1.Columns[0].Visible = false;

                label13.Text = "Количество записей: " + table1.Rows.Count.ToString();
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            cur_id = dataGridView1.SelectedCells[0].Value.ToString();
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(this, "Вы точно хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    MainFunc.sql("DELETE FROM repair WHERE repair_id = " + cur_id + ";");

                    cur_id = "";
                    button1.Enabled = false;

                    updateTables();
                }
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    }
    }
}
