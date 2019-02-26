using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carShowroom
{
    public partial class add : Form
    {
        public add()
        {
            InitializeComponent();
        }

        private void add_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Кнопка назад 
        private void button2_Click(object sender, EventArgs e)
        {
            menu menu = new menu();
            menu.Show();
            Hide();
        }

        // Подключение класса с функциями
        Function MainFunc = new Function();

        private void add_Load(object sender, EventArgs e)
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

                label13.Text = "Количество записей: " + table2.Rows.Count.ToString();
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            textBox5.Text = "";
        }

        private void addMechanic()
        {
            try
            {
                Random rnd = new Random();
                int temp = rnd.Next(0, 99999);

                if (checkInputMechanic())
                {
                    // Добавление записи
                    MainFunc.sql("INSERT INTO mechanic VALUES (" +
                        temp + ", " +
                        textBox4.Text + ", " +
                        "'" + textBox1.Text + "', " +
                        "'" + textBox2.Text + "', " +
                        "'" + textBox6.Text + "', " +
                        "'" + textBox5.Text + "', " +
                        comboBox1.Text +
                        ");");

                    clearInputMechanic();

                    // Обновление таблиц
                    updateTables();

                    MessageBox.Show(this, "Запись добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            textBox7.Text = "";
        }

        private void addCar()
        {
            try
            {
                Random rnd = new Random();
                int temp = rnd.Next(0, 99999);

                if (checkInputCar())
                {
                    // Добавление записи
                    MainFunc.sql("INSERT INTO car VALUES (" +
                        temp + ", " +
                        textBox8.Text + ", " +
                        "'" + textBox9.Text + "', " +
                        "'" + textBox10.Text + "', " +
                        "'" + comboBox2.Text + "', " +
                        textBox7.Text +
                        ");");

                    clearInputCar();

                    // Обновление таблиц
                    updateTables();

                    MessageBox.Show(this, "Запись добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Определяем что добавляем
            if (tabControl1.SelectedIndex == 0)
            {
                // Добавляем механика
                addMechanic();
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                // Добавляем машину
                addCar();
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl1.SelectedIndex == 0) { checkInputMechanic(); }
            else if (tabControl1.SelectedIndex == 1) { checkInputCar(); }
        }
    }
}
