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

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkInput())
            {
                OleDbDataAdapter data1 = new OleDbDataAdapter(MainFunc.sql("SELECT repair_id, (SELECT mechanic_surname FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_surname], (SELECT mechanic_name FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_name], (SELECT mechanic_patronymic FROM mechanic WHERE repair.mechanic_id = mechanic.mechanic_id) AS [m_patronymic], (SELECT car_name FROM car WHERE repair.car_id = car.car_id) AS [model_car], (SELECT car_mark FROM car WHERE repair.car_id = car.car_id) AS [mark_car], repair_date, repair_cost FROM repair WHERE repair_id = " + comboBox1.Text + ";"));
                DataTable table1 = new DataTable();
                data1.Fill(table1);


                Word.Application wordApp = new Word.Application();
                Word.Document doc = wordApp.Documents.Add();
                wordApp.Visible = true;

                Word.Range range = doc.Range();
                range.Text = "Номер заказа: " + table1.Rows[0][0].ToString() + "\n" +
                    "Мастер: " + table1.Rows[0][1].ToString() + " " + table1.Rows[0][2].ToString() + " " + table1.Rows[0][3].ToString() + "\n" +
                    "Автомобиль: " + table1.Rows[0][4].ToString() + " " + table1.Rows[0][5].ToString() + "\n" +
                    "Дата начала работ: " + table1.Rows[0][6].ToString() + "\n" +
                    "Стоимость: " + table1.Rows[0][7].ToString() + "руб.\n";
            }
        }

        private void import_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void import_Load(object sender, EventArgs e)
        {
            OleDbDataAdapter data1 = new OleDbDataAdapter(MainFunc.getAll("repair"));
            DataTable table1 = new DataTable();
            data1.Fill(table1);
            for (int curRow = 0; curRow < table1.Rows.Count; curRow++)
            {
                string item = table1.Rows[curRow][0].ToString();
                comboBox1.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            menu menu = new menu();
            menu.Show();
            Hide();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            checkInput();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connect = "Provider= Microsoft.Jet.OLEDB.4.0; Data Source=" + MainFunc.db_name + ";";
            OleDbConnection ODConnect = new OleDbConnection(connect);
            ODConnect.Open();

            Excel.Application excelApp = new Excel.Application();
            var workBook = excelApp.Workbooks.Add(Type.Missing);

            var mechanic = workBook.ActiveSheet;
            mechanic.Name = "Автомеханики";

            //
            var mechanicCells = mechanic.Cells;

            mechanic.Cells[1, 1] = "ID";
            mechanic.Cells[1, 2] = "Табельный номер";
            mechanic.Cells[1, 3] = "Фамилия";
            mechanic.Cells[1, 4] = "Имя";
            mechanic.Cells[1, 5] = "Отчество";
            mechanic.Cells[1, 6] = "Стаж";
            mechanic.Cells[1, 7] = "Разряд";

            OleDbCommand mechanicData = new OleDbCommand("SELECT * FROM mechanic;");
            mechanicData.Connection = ODConnect;
            mechanicData.ExecuteNonQuery();
            OleDbDataReader readerMechanic = mechanicData.ExecuteReader();

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

            var car = workBook.Sheets.Add(After: workBook.ActiveSheet);
            car.Name = "Автомобили";

            //
            var carCells = car.Cells;

            car.Cells[1, 1] = "ID";
            car.Cells[1, 2] = "Табельный номер";
            car.Cells[1, 3] = "Марка";
            car.Cells[1, 4] = "Модель";
            car.Cells[1, 5] = "Тип кузова";
            car.Cells[1, 6] = "Год";

            OleDbCommand carData = new OleDbCommand("SELECT * FROM car;");
            carData.Connection = ODConnect;
            carData.ExecuteNonQuery();
            OleDbDataReader readerCar = carData.ExecuteReader();

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

            var repair = workBook.Sheets.Add(After: workBook.ActiveSheet);
            repair.Name = "Заказы";

            //
            var repairCells = repair.Cells;

            repair.Cells[1, 1] = "ID";
            repair.Cells[1, 2] = "ID автомеханика";
            repair.Cells[1, 3] = "ID автомобиля";
            repair.Cells[1, 4] = "Дата начала работ";
            repair.Cells[1, 5] = "Продолжительность ремонта";
            repair.Cells[1, 6] = "Стоимость";

            OleDbCommand repairData = new OleDbCommand("SELECT * FROM repair;");
            repairData.Connection = ODConnect;
            repairData.ExecuteNonQuery();
            OleDbDataReader readerRepair = repairData.ExecuteReader();

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
