// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com/
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace carShowroom
{
    class Function
    {
        public string db_name = "db.mdb";

        // Функция SQL запроса
        public OleDbCommand sql(string query)
        {
            string connect = "Provider= Microsoft.Jet.OLEDB.4.0; Data Source=" + this.db_name + ";";
            OleDbConnection ODConnect = new OleDbConnection(connect);
            ODConnect.Open();
            OleDbCommand sql = new OleDbCommand(query);
            sql.Connection = ODConnect;
            sql.ExecuteNonQuery();
            ODConnect.Close();
            return sql;
        }

        // Функция получения данных таблицы
        public OleDbCommand getAll(string table)
        {
            return this.sql("SELECT * FROM " + table + ";");
        }

        // Проверка строки на соответствие регулярному выражению
        public bool stringTest(string str, string reg)
        {
            Regex regex = new Regex(reg);
            MatchCollection matches = regex.Matches(str);
            if (matches.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
