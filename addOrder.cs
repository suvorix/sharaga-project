// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com/
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
    public partial class addOrder : Form
    {
        public addOrder()
        {
            InitializeComponent();
        }

        // Полное закрытие программы
        private void addOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
        // Открыть форму меню
        private void button2_Click_1(object sender, EventArgs e)
        {
            menu menu = new menu();
            menu.Show();
            Hide();
        }

        // Подключение класса с функциями
        Function MainFunc = new Function();

        // Загрузка формы добавление заказа
        private void addOrder_Load(object sender, EventArgs e)
        {
            // Заполнение поля выбора механика
            OleDbDataAdapter data1 = new OleDbDataAdapter(MainFunc.getAll("mechanic"));
            DataTable table1 = new DataTable();
            data1.Fill(table1);
            for (int curRow = 0; curRow < table1.Rows.Count; curRow++)
            {
                string item = table1.Rows[curRow][0].ToString() + ": " + table1.Rows[curRow][2].ToString() + " " + table1.Rows[curRow][3].ToString() + " " + table1.Rows[curRow][4].ToString();
                comboBox1.Items.Add(item);
            }
            // Заполнение поля выбора машины
            OleDbDataAdapter data2 = new OleDbDataAdapter(MainFunc.getAll("car"));
            DataTable table2 = new DataTable();
            data2.Fill(table2);
            for (int curRow = 0; curRow < table2.Rows.Count; curRow++)
            {
                string item = table2.Rows[curRow][0].ToString() + ": " + table2.Rows[curRow][2].ToString() + " " + table2.Rows[curRow][3].ToString();
                comboBox2.Items.Add(item);
            }

            dateTimePicker1.MinDate = DateTime.Today;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e){ checkInput(); }
        private void textBox2_KeyUp(object sender, KeyEventArgs e) { checkInput(); }

        // Проверка полей
        private bool checkInput()
        {
            try
            {
                // Проверка есть ли пустые поля
                if (textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "" && comboBox1.Text != "" && comboBox2.Text != "")
                {
                    // Проверка полей на правильность ввода
                    if (MainFunc.stringTest(textBox2.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox2.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox3.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox4.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox5.Text, @"^[0-9]*$"))
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

        // Отчистка полей
        private void clearInput()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        // Кнопка добавления
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Random rnd = new Random();
                int temp = rnd.Next(0, 99999);

                if (checkInput())
                {
                    // Добавление записи
                    MainFunc.sql("INSERT INTO repair VALUES (" +
                        temp + ", " +
                        comboBox1.Text.Split(':')[0] + ", " +
                        comboBox2.Text.Split(':')[0] + ", " +
                        "'" + dateTimePicker1.Value.Day + "." + dateTimePicker1.Value.Month + "." + dateTimePicker1.Value.Year + "', " +
                        "'" + textBox3.Text + " д. " + textBox4.Text + " ч. " + textBox5.Text + " мин.', " +
                        textBox2.Text +
                        ");");

                    clearInput();

                    MessageBox.Show(this, "Запись добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
