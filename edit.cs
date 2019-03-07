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

        // ID выбранной записи
        private string cur_id = "";

        // Полное закрытие программы
        private void add_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Открыть форму меню
        private void button2_Click(object sender, EventArgs e)
        {
            menu menu = new menu();
            menu.Show();
            Hide();
        }

        // Подключение класса с функциями
        Function MainFunc = new Function();

        // Загрузка формы
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

                label13.Text = "Количество записей: " + table2.Rows.Count.ToString();
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Выбор записи в таблице автомехаников
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Получение ID записи  
            cur_id = dataGridView1.SelectedCells[0].Value.ToString();

            // Подстановка данных в поля
            textBox4.Text = dataGridView1.SelectedCells[1].Value.ToString();
            textBox1.Text = dataGridView1.SelectedCells[2].Value.ToString();
            textBox2.Text = dataGridView1.SelectedCells[3].Value.ToString();
            textBox6.Text = dataGridView1.SelectedCells[4].Value.ToString();
            int exp = Convert.ToInt32(dataGridView1.SelectedCells[5].Value.ToString());
            int exp_year = Convert.ToInt32(exp / 365);
            int exp_month = Convert.ToInt32((exp - exp_year * 365) / 30);
            int exp_day = exp - exp_year * 365 - exp_month * 30;
            textBox3.Text = exp_day.ToString();
            textBox5.Text = exp_month.ToString();
            textBox11.Text = exp_year.ToString();
            comboBox1.Text = dataGridView1.SelectedCells[6].Value.ToString();
            // Активация кнопок
            button1.Enabled = true;
            button3.Enabled = true;
            // Проверка полей на валидность
            checkInputMechanic();
        }

        // Выбор записи в таблице автомобилей
        private void dataGridView2_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Получение ID записи  
            cur_id = dataGridView2.SelectedCells[0].Value.ToString();

            // Подстановка данных в поля
            textBox8.Text = dataGridView2.SelectedCells[1].Value.ToString();
            textBox9.Text = dataGridView2.SelectedCells[2].Value.ToString();
            textBox10.Text = dataGridView2.SelectedCells[3].Value.ToString();
            comboBox2.Text = dataGridView2.SelectedCells[4].Value.ToString();
            textBox7.Text = dataGridView2.SelectedCells[5].Value.ToString();
            // Активация кнопок
            button1.Enabled = true;
            button3.Enabled = true;
            // Проверка полей на валидность
            checkInputCar();
        }

        // Проверка полей при изменении
        private void textBox4_KeyUp(object sender, KeyEventArgs e) { checkInputMechanic(); }
        private void comboBox1_TextChanged(object sender, EventArgs e) { checkInputMechanic(); }
        private void textBox9_KeyUp(object sender, KeyEventArgs e) { checkInputCar(); }
        private void comboBox2_TextChanged(object sender, EventArgs e) { checkInputCar(); }

        // Проверка полей механика
        private bool checkInputMechanic()
        {
            try
            {
                // Проверка есть ли пустые поля
                if (textBox1.Text != "" && textBox2.Text != "" && textBox4.Text != "" && textBox5.Text != "" && textBox6.Text != "" && comboBox1.Text != "")
                {
                    // Проверка полей на правильность ввода
                    if (MainFunc.stringTest(textBox1.Text, @"^[a-zA-Zа-яА-Я]*$") && MainFunc.stringTest(textBox2.Text, @"^[a-zA-Zа-яА-Я]*$") && MainFunc.stringTest(textBox6.Text, @"^[a-zA-Zа-яА-Я]*$") && MainFunc.stringTest(textBox4.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox3.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox5.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox11.Text, @"^[0-9]*$"))
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

        // Отчистка полей механика
        private void clearInputMechanic()
        {
            cur_id = "";
            textBox4.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            textBox3.Text = "";
            textBox5.Text = "";
            textBox11.Text = "";

            button1.Enabled = false;
            button3.Enabled = false;
        }

        // Изменение механиков
        private void editMechanic()
        {
            try
            {
                if (checkInputMechanic())
                {
                    int exp = Convert.ToInt32(textBox3.Text) + Convert.ToInt32(textBox5.Text) * 30 + Convert.ToInt32(textBox11.Text) * 365;
                    // Добавление записи
                    MainFunc.sql("UPDATE mechanic SET " +
                        "mechanic_number = " + textBox4.Text + ", " +
                        "mechanic_surname = '" + textBox1.Text + "', " +
                        "mechanic_name = '" + textBox2.Text + "', " +
                        "mechanic_patronymic = '" + textBox6.Text + "', " +
                        "mechanic_exp = '" + exp + "', " +
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
        
        // Проверка полей автомобиля
        private bool checkInputCar()
        {
            try
            {
                // Проверка есть ли пустые поля
                if (textBox7.Text != "" && textBox8.Text != "" && textBox9.Text != "" && textBox10.Text != "" && comboBox2.Text != "")
                {
                    if (MainFunc.stringTest(textBox9.Text, @"^[a-zA-Zа-яА-Я\-]*$") && MainFunc.stringTest(textBox7.Text, @"^[0-9]*$") && MainFunc.stringTest(textBox8.Text, @"^[0-9]*$"))
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

        // Отчистка полей машины
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

        // Изменение данных машины
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

        // Кнопка изменения
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

        // Кнопка удаления
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
