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
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;

namespace carShowroom
{
    public partial class import : Form
    {
        public import()
        {
            InitializeComponent();
        }

        // Подключение класса с функциями
        Function MainFunc = new Function();

        // Проверка поля на правильность ввода
        private bool checkInput()
        {
            try
            {
                // Проверка есть ли пустые поля
                if (comboBox1.Text != "")
                {
                    button1.Enabled = true;
                    return true;
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

        // Создание чека
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверка полей
                if (checkInput())
                {
                    // Получение данных из таблицы
                    OleDbDataAdapter data1 = new OleDbDataAdapter(MainFunc.sql("SELECT " +
                        "repair_id, " +
                        "(SELECT mechanic_surname FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_surname], " +
                        "(SELECT mechanic_name FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_name], " +
                        "(SELECT mechanic_patronymic FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_patronymic], " +
                        "(SELECT car_name FROM car WHERE repair.car_id = car.car_id) AS [model_car], " +
                        "(SELECT car_mark FROM car WHERE repair.car_id = car.car_id) AS [mark_car], " +
                        "repair_date, " +
                        "repair_cost " +
                        "FROM repair " +
                        "WHERE repair_id = " + comboBox1.Text.Split(':')[0] + ";"));
                    DataTable table1 = new DataTable();
                    data1.Fill(table1);

                    // Создание объекта ворда
                    Word.Application wordApp = new Word.Application();
                    // Получение шаблона чека
                    Word.Document doc = wordApp.Documents.Add(Environment.CurrentDirectory + "\\check.docx");
                    // Показать документ
                    wordApp.Visible = true;

                    // Вывод данных в документ
                    doc.Bookmarks["number"].Range.Text = table1.Rows[0][0].ToString();
                    doc.Bookmarks["date"].Range.Text = table1.Rows[0][6].ToString();
                    doc.Bookmarks["worker"].Range.Text = table1.Rows[0][1].ToString() + " " + table1.Rows[0][2].ToString() + " " + table1.Rows[0][3].ToString();
                    doc.Bookmarks["car"].Range.Text = table1.Rows[0][5].ToString() + " " + table1.Rows[0][4].ToString();
                    doc.Bookmarks["orderName"].Range.Text = "Ремонт автомобиля " + table1.Rows[0][5].ToString() + " " + table1.Rows[0][4].ToString();
                    doc.Bookmarks["price"].Range.Text = table1.Rows[0][7].ToString();
                }
            }
            catch
            {
                MessageBox.Show(this, "Произошла критическая ошибка!", "Критическая ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Полное закрытие программы
        private void import_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        // Загрузка формы импорта
        private void import_Load(object sender, EventArgs e)
        {
            // Получение данных о заказах
            OleDbDataAdapter data1 = new OleDbDataAdapter(MainFunc.sql("SELECT " +
                "repair_id, " +
                "(SELECT mechanic_surname FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_surname], " +
                "(SELECT mechanic_name FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_name], " +
                "(SELECT mechanic_patronymic FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_patronymic], " +
                "(SELECT car_name FROM car WHERE repair.car_id = car.car_id) AS [model_car], " +
                "(SELECT car_mark FROM car WHERE repair.car_id = car.car_id) AS [mark_car] " +
                "FROM repair"));
            DataTable table1 = new DataTable();
            data1.Fill(table1);

            // Добавление записей в поле выбора
            for (int curRow = 0; curRow < table1.Rows.Count; curRow++)
            {
                string item = table1.Rows[curRow][0].ToString() + ": " + table1.Rows[curRow][1].ToString() + " " + table1.Rows[curRow][2].ToString().Substring(0, 1) + ". " + table1.Rows[curRow][3].ToString().Substring(0, 1) + ". - " + table1.Rows[curRow][4].ToString() + " " + table1.Rows[curRow][5].ToString();
                comboBox1.Items.Add(item);
            }
        }

        // Открыть форму меню
        private void button3_Click(object sender, EventArgs e)
        {
            menu menu = new menu();
            menu.Show();
            Hide();
        }

        // Проверка правильности ввода при изменении поля выбора
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        // Импорт всех таблиц в Excel
        private void button2_Click(object sender, EventArgs e)
        {
            // Соединение с базой
            string connect = "Provider= Microsoft.Jet.OLEDB.4.0; Data Source=" + MainFunc.db_name + ";";
            OleDbConnection ODConnect = new OleDbConnection(connect);
            ODConnect.Open();

            // Создание Excel документа
            Excel.Application excelApp = new Excel.Application();
            // Добавление листа
            var workBook = excelApp.Workbooks.Add(Type.Missing);

            // Получение листа
            var mechanic = workBook.ActiveSheet;
            // Имя листа
            mechanic.Name = "mechanic";

            // Объект таблицы
            var mechanicCells = mechanic.Cells;

            // Шапка
            mechanic.Cells[1, 1] = "mechanic_id";
            mechanic.Cells[1, 2] = "mechanic_number";
            mechanic.Cells[1, 3] = "mechanic_surname";
            mechanic.Cells[1, 4] = "mechanic_name";
            mechanic.Cells[1, 5] = "mechanic_patronymic";
            mechanic.Cells[1, 6] = "mechanic_exp";
            mechanic.Cells[1, 7] = "mechanic_rank";

            // Получение данных
            OleDbCommand mechanicData = new OleDbCommand("SELECT * FROM mechanic;");
            mechanicData.Connection = ODConnect;
            mechanicData.ExecuteNonQuery();
            OleDbDataReader readerMechanic = mechanicData.ExecuteReader();

            // Добовление данных в таблицу
            int temp = 2;
            while (readerMechanic.Read())
            {
                mechanic.Cells[temp, 1] = readerMechanic[0].ToString();
                mechanic.Cells[temp, 2] = readerMechanic[1].ToString();
                mechanic.Cells[temp, 3] = readerMechanic[2].ToString();
                mechanic.Cells[temp, 4] = readerMechanic[3].ToString();
                mechanic.Cells[temp, 5] = readerMechanic[4].ToString();
                mechanic.Cells[temp, 6] = readerMechanic[5].ToString();
                mechanic.Cells[temp, 7] = readerMechanic[6].ToString();
                temp++;
            }
            mechanic.Columns.AutoFit();
            mechanic.Rows.AutoFit();

            // Добавление листа
            var car = workBook.Sheets.Add(After: workBook.ActiveSheet);
            // Имя листа
            car.Name = "car";

            // Объект таблицы
            var carCells = car.Cells;

            // Шапка
            car.Cells[1, 1] = "car_id";
            car.Cells[1, 2] = "car_number";
            car.Cells[1, 3] = "car_mark";
            car.Cells[1, 4] = "car_name";
            car.Cells[1, 5] = "car_type";
            car.Cells[1, 6] = "car_year";

            // Получение данных
            OleDbCommand carData = new OleDbCommand("SELECT * FROM car;");
            carData.Connection = ODConnect;
            carData.ExecuteNonQuery();
            OleDbDataReader readerCar = carData.ExecuteReader();

            // Добовление данных в таблицу
            temp = 2;
            while (readerCar.Read())
            {
                car.Cells[temp, 1] = readerCar[0].ToString();
                car.Cells[temp, 2] = readerCar[1].ToString();
                car.Cells[temp, 3] = readerCar[2].ToString();
                car.Cells[temp, 4] = readerCar[3].ToString();
                car.Cells[temp, 5] = readerCar[4].ToString();
                car.Cells[temp, 6] = readerCar[5].ToString();
                temp++;
            }
            car.Columns.AutoFit();
            car.Rows.AutoFit();

            // Добавление листа
            var repair = workBook.Sheets.Add(After: workBook.ActiveSheet);
            // Имя листа
            repair.Name = "repair";

            // Объект таблицы
            var repairCells = repair.Cells;

            // Шапка
            repair.Cells[1, 1] = "repair_id";
            repair.Cells[1, 2] = "mechanic_id";
            repair.Cells[1, 3] = "car_id";
            repair.Cells[1, 4] = "repair_date";
            repair.Cells[1, 5] = "repair_time";
            repair.Cells[1, 6] = "repair_cost";

            // Получение данных
            OleDbCommand repairData = new OleDbCommand("SELECT * FROM repair;");
            repairData.Connection = ODConnect;
            repairData.ExecuteNonQuery();
            OleDbDataReader readerRepair = repairData.ExecuteReader();

            // Добовление данных в таблицу
            temp = 2;
            while (readerRepair.Read())
            {
                repair.Cells[temp, 1] = readerRepair[0].ToString();
                repair.Cells[temp, 2] = readerRepair[1].ToString();
                repair.Cells[temp, 3] = readerRepair[2].ToString();
                repair.Cells[temp, 4] = readerRepair[3].ToString();
                repair.Cells[temp, 5] = readerRepair[4].ToString();
                repair.Cells[temp, 6] = readerRepair[5].ToString();
                temp++;
            }
            repair.Columns.AutoFit();
            repair.Rows.AutoFit();

            excelApp.Visible = true;

            ODConnect.Close();
        }
    }
}
