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
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
        }
        private void Order_Load(object sender, EventArgs e)
        {
            Table("");
            if (dataGridView1.SelectedRows.Count == 0)
                dataGridView1.Rows[0].Selected = true;
            string ticketid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            label1.Text = "订单号：" + ticketid;
        }
        public void Table(string sql)
        {
            dataGridView1.Rows.Clear();//清空旧数据
            string basesql = $"select ticketid, flightid, detail, type, price, number, price*number from ticket where custid = '{Data.UID}'";
            string wholesql = basesql + sql + ";";
            Dao dao = new Dao();
            IDataReader dc = dao.Read(wholesql);
            int idx = 1;
            while (dc.Read())
            {
                dataGridView1.Rows.Add(dc[0].ToString(), dc[1].ToString(), dc[2].ToString(), dc[3].ToString(), dc[4].ToString(), dc[5].ToString(), dc[6].ToString()); ;
                idx++;
            }
            dc.Close();
            dao.DaoClose();
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            string ticketid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            label1.Text = "订单号：" + ticketid;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string oticket = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string route = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            Change mycg = new Change(route,oticket);
            mycg.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr =  MessageBox.Show("确认退票？", "提示", MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if (dr == DialogResult.Yes)
            {
                string ticketid = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                string flightid = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                int number = int.Parse(dataGridView1.SelectedRows[0].Cells[5].Value.ToString());
                string sql = $"delete from ticket where ticketid='{ticketid}';update flight set remaining=remaining+{number} where flightid = '{flightid}'";
                Dao dao = new Dao();
                if (dao.Execute(sql) > 1)//执行了两条sql，大于1才是都成功的
                {
                    MessageBox.Show("退票成功");
                    Table("");
                }
            }
        }
    }
}
