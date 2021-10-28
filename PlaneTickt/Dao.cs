using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PlaneTickt
{
    class Dao
    {
        MySqlConnection conn;
        public MySqlConnection Connet()
        {
            String connetStr = "server=127.0.0.1;port=3306;user=root;password=;database=dbfinal;";
            MySqlConnection con = new MySqlConnection(connetStr);
            con.Open();
            conn = con;
            return con;
        }
        public MySqlCommand Command(string sql)
        {
            MySqlCommand cmd = new MySqlCommand(sql, Connet());
            return cmd;
        }
        public int Execute(string sql) //更新操作
        {
            return Command(sql).ExecuteNonQuery();
        }
        public MySqlDataReader Read(string sql) //读取操作
        {
            return Command(sql).ExecuteReader();
        }
        public void DaoClose()
        {
            conn.Close();
        }
    }
}
