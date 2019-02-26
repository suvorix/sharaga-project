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
    public partial class edit : Form
    {
        public edit()
        {
            InitializeComponent();
        }

        private string cur_id = "";

        private void add_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            menu menu = new menu();
            menu.Show();
            Hide();
        }

        // Подключение класса с функциями
        Function MainFunc = new Function();

        private void edit_Load(object sender, EventArgs e)
        {
            updateTables();
        }

        // Обновление таблиц
        private void updateTables()
        {
            try
            {
                // Таблица механиков
                OleDbDataAdapter data1 = new OleDbDataAdapter(MainFunc.getAll("mechanic"));
                DataTable table1 = new DataTable();
                data1.Fill(table1);

                table1.Columns["mechanic_id"].ColumnName = "ID";
                table1.Columns["mechanic_number"].ColumnName = "Табельный номер";
                table1.Columns["mechanic_surname"].ColumnName = "Фамилия";
                table1.Columns["mechanic_name"].ColumnName = "Имя";
                table1.Columns["mechanic_patronymic"].ColumnName = "Отчество";
                table1.Columns["mechanic_exp"].ColumnName = "Стаж";
                table1.Columns["mechanic_rank"].ColumnName = "Разряд";

                dataGridView1.DataSource = table1;
                dataGridView1.Columns[0].Visible = false;

                label11.Text = "Количество записей: " + table1.Rows.Count.ToString();

                // Таблица авто
                OleDbDataAdapter data2 = new OleDbDataAdapter(MainFunc.getAll("car"));
                DataTable table2 = new DataTable();
                data2.Fill(table2);

                table2.Columns["car_id"].ColumnName = "ID";
                table2.Columns["car_number"].ColumnName = "Табельный номер";
                table2.Columns["car_mark"].ColumnName = "Марка";
                table2.Columns["car_name"].ColumnName = "Модель";
                table2.Columns["car_type"].ColumnName = "Тип кузова";
                table2.Columns["car_year"].ColumnName = "Год";

                dataGridView2.DataSource = table2;
                dataGridView2.Columns[0].Visible = false;

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

            textBox4.Text = dataGridView1.SelectedCells[1].Value.ToString();
            textBox1.Text = dataGridView1.SelectedCells[2].Value.ToString();
            textBox2.Text = dataGridView1.SelectedCells[3].Value.ToString();
            textBox6.Text = dataGridView1.SelectedCells[4].Value.ToString();
            textBox5.Text = dataGridView1.SelectedCells[5].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedCells[6].Value.ToString();

            button1.Enabled = true;
            button3.Enabled = true;

            checkInputMechanic();
        }

        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            cur_id = dataGridView2.SelectedCells[0].Value.ToString();

            textBox8.Text = dataGridView2.SelectedCells[1].Value.ToString();
            textBox9.Text = dataGridView2.SelectedCells[2].Value.ToString();
            textBox10.Text = dataGridView2.SelectedCells[3].Value.ToString();
            comboBox2.Text = dataGridView2.SelectedCells[4].Value.ToString();
            textBox7.Text = dataGridView2.SelectedCells[5].Value.ToString();

            button1.Enabled = true;
            button3.Enabled = true;

            checkInputCar();
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e) { checkInputMechanic(); }
        private void comboBox1_TextChanged(object sender, EventArgs e) { checkInputMechanic(); }
        private void textBox9_KeyUp(object sender, KeyEventArgs e) { checkInputCar(); }
        private void comboBox2_TextChanged(object sender, EventArgs e) { checkInputCar(); }

        private bool checkInputMechanic()
        {
            try
            {
                // Проверка есть ли пустые поля
                if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && comboBox1.Text != "")
                {
                    // Проверка полей на правильность ввода
                    if (MainFunc.stringTest(textBox1.Text, @"^[a-zA-Zа-яА-Я]*$") && MainFunc.stringTest(textBox2.Text, @"^[a-zA-Zа-яА-Я]*$") && MainFunc.stringTest(textBox6.Text, @"^[a-zA-Zа-яА-Я]*$") && MainFunc.stringTest(textBox4.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox5.Text, @"^[0-9]*$"))
                    {
                        button1.Enabled = true;
                        return true;
                    }
                    else
                    {
                        button1.Enabled = false;
                    }
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void clearInputMechanic()
        {
            cur_id = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            textBox5.Text = "";

            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void editMechanic()
        {
            try
            {
                if (checkInputMechanic())
                {
                    // Добавление записи
                    MainFunc.sql("UPDATE mechanic SET " +
                        "mechanic_number = " + textBox4.Text + ", " +
                        "mechanic_surname = '" + textBox1.Text + "', " +
                        "mechanic_name = '" + textBox2.Text + "', " +
                        "mechanic_patronymic = '" + textBox6.Text + "', " +
                        "mechanic_exp = '" + textBox5.Text + "', " +
                        "mechanic_rank = " + comboBox1.Text + " " +
                        "WHERE mechanic_id = " + cur_id + ";");

                    clearInputMechanic();

                    // Обновление таблиц
                    updateTables();
                }
            }
            catch
            {

                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool checkInputCar()
        {
            try
            {
                // Проверка есть ли пустые поля
                if (textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && textBox10.Text != "" && comboBox2.Text != "")
                {
                    if (MainFunc.stringTest(textBox9.Text, @"^[a-zA-Zа-яА-Я]*$") && MainFunc.stringTest(textBox7.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox8.Text, @"^[0-9]*$"))
                    {
                        button1.Enabled = true;
                        return true;
                    }
                    else
                    {
                        button1.Enabled = false;
                    }
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        private void clearInputCar()
        {
            cur_id = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox7.Text = "";

            button1.Enabled = false;
            button3.Enabled = false;
        }

        private void editCar()
        {
            try
            {
                if (checkInputCar())
                {
                    // Добавление записи
                    MainFunc.sql("UPDATE car SET " +
                        "car_number = " + textBox8.Text + ", " +
                        "car_mark = '" + textBox9.Text + "', " +
                        "car_name = '" + textBox10.Text + "', " +
                        "car_type = '" + comboBox2.Text + "', " +
                        "car_year = " + textBox7.Text + " " +
                        "WHERE car_id = " + cur_id + ";");

                    clearInputCar();

                    // Обновление таблиц
                    updateTables();
                }
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "Вы точно хотите изменить запись?", "Изменение записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Определяем что добавляем
                if (tabControl1.SelectedIndex == 0)
                {
                    // Добавляем механика
                    editMechanic();
                }
                else if (tabControl1.SelectedIndex == 1)
                {
                    // Добавляем машину
                    editCar();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show(this, "Вы точно хотите удалить запись?", "Удаление записи", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    // Определяем что добавляем
                    if (tabControl1.SelectedIndex == 0)
                    {
                        MainFunc.sql("DELETE FROM mechanic WHERE mechanic_id = " + cur_id + ";");

                        clearInputMechanic();

                        // Обновление таблиц
                        updateTables();
                    }
                    else if (tabControl1.SelectedIndex == 1)
                    {

                        MainFunc.sql("DELETE FROM car WHERE car_id = " + cur_id + ";");

                        clearInputCar();

                        // Обновление таблиц
                        updateTables();
                    }
                }
            } catch
            {
                MessageBox.Show(this, "Запись, которую вы хотите удалить, используется.", "Ошибка удаления", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
