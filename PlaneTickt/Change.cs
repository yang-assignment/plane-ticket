using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneTickt
{
    public partial class Change : Form
    {
        public Change()
        {
            InitializeComponent();
        }
        public Change(string route, string oticket)
        {
            InitializeComponent();
            label1.Text = route;
            label10.Text = oticket;
            
        }
        private void Change_Load(object sender, EventArgs e)
        {
            string route = label1.Text;
            string oticket = label10.Text;
            int mid = route.IndexOf("-");
            string start = route.Substring(0, mid);
            string end = route.Substring(mid + 1);
            Table($"and beginplace = '{start}' and endplace = '{end}' and flightid != (select flightid from ticket where ticketid ='{oticket}')");
            dataGridView1.ClearSelection();
        }
        public void Table(string sql)
        {
            dataGridView1.Rows.Clear();//清空旧数据
            string basesql = "select DATE_FORMAT(begintime,'%Y-%m-%d'), flightid, price from flight,line where line.lineid=flight.lineid ";
            string wholesql = basesql + sql + ";";
            Dao dao = new Dao();
            IDataReader dc = dao.Read(wholesql);
            int idx = 1;
            while (dc.Read())
            {
                dataGridView1.Rows.Add(idx.ToString(), dc[0].ToString(), dc[1].ToString(), dc[2].ToString()); ;
                idx++;
            }
            dc.Close();
            dao.DaoClose();
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                dataGridView1.Rows[0].Selected = true;
            string id = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string sql1 = $"select price, remaining,  DATE_FORMAT(begintime,'%H:%i'),  DATE_FORMAT(endtime,'%H:%i') from flight where flight.flightid = '{id}'";
            string sql2 = $"select company, type from plane where planeid = (select planeid from flight where flightid = '{id}')";
            Dao dao = new Dao();
            IDataReader dc = dao.Read(sql1);
            dc.Read();
            double price = Convert.ToDouble(dc[0]);
            string remain = dc[1].ToString();
            string begintime = dc[2].ToString();
            string endtime = dc[3].ToString();
            dc.Close();
            IDataReader dr = dao.Read(sql2);
            dr.Read();
            string company = dr[0].ToString();
            string type = dr[1].ToString();
            dc.Close();
            dao.DaoClose();
            label9.Visible = true;
            comboBox1.Visible = true;
            button1.Visible = true;
            label2.Text = "航班日期：" + dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            label3.Text = "当前航班" + id;
            label3.Text = "航空公司：" + company + "(" + type + ")";
            label5.Text = "飞行时间：" + begintime + "—" + endtime;
            label6.Text = "剩余票数：" + remain;
            label7.Text = "价格：" + (price * 1.0).ToString();
            label8.Text = "订票数量：";
            string oldticket = label10.Text;
            string sql3 = $"select number from ticket where ticketid = '{oldticket}'";
            Dao dao1 = new Dao();
            IDataReader dd = dao1.Read(sql3);
            dd.Read();
            label12.Text = dd[0].ToString();
            dd.Close();
            dao1.DaoClose();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string id = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string sql = $"select price from flight where flight.flightid = '{id}'";
            Dao dao = new Dao();
            IDataReader dc = dao.Read(sql);
            dc.Read();
            double price = Convert.ToDouble(dc[0]);
            dc.Close();
            if (comboBox1.Text == "成人票")
                label7.Text = "价格：" + (price * 1.0).ToString();
            if (comboBox1.Text == "儿童票")
                label7.Text = "价格：" + (price * 0.5).ToString();
            if (comboBox1.Text == "学生票")
                label7.Text = "价格：" + (price * 0.8).ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldticket = label10.Text;
            string newflightid = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            int remain = int.Parse(label6.Text.Substring(5));
            double price = double.Parse(label7.Text.Substring(3));
            string type = comboBox1.Text;

            int number = int.Parse(label12.Text);
            if (number > remain)
            {
                MessageBox.Show("航班座位不足");
            }
            else
            {
                string sql = $"INSERT into ticket(custid, flightid, type, price,number,detail) VALUES ('{Data.UID}','{newflightid}','{type}','{price}','{number}','{label1.Text}');" +
                    $"update flight set remaining=remaining-{number} where flightid='{newflightid}';" +
                    $"update flight set remaining=remaining+{number} where flightid=(select flightid from ticket where ticketid = '{oldticket}');" +
                    $"delete from ticket where ticketid ='{oldticket}';";
                Dao dao2 = new Dao();
                if (dao2.Execute(sql) == 4)//执行了两条sql，大于1才是都成功的
                {
                    MessageBox.Show("改签成功");
                    this.Close();
                }
            }

        }
    }
}
