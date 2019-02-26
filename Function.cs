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

        public OleDbCommand getAll(string table)
        {
            return this.sql("SELECT * FROM " + table + ";");
        }
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
