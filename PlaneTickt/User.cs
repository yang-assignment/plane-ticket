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
    public partial class User : Form
    {
        public User()
        {
            InitializeComponent();
        }
        private void User_Load(object sender, EventArgs e)
        {
            string sql = "";
            Table(sql);
            dataGridView1.ClearSelection();
        }
        public void Table(string sql)
        {
            dataGridView1.Rows.Clear();//清空旧数据
            string basesql = "select DATE_FORMAT(begintime,'%Y-%m-%d'),flightid,beginplace,endplace from flight, line where line.lineid=flight.lineid ";
            string wholesql = basesql + sql+";";
            Dao dao = new Dao();
            IDataReader dc = dao.Read(wholesql);
            int idx = 1;
            while (dc.Read())
            {
                dataGridView1.Rows.Add(idx.ToString(), dc[0].ToString(), dc[1].ToString(), dc[2].ToString(), dc[3].ToString()); ;
                idx++;
            }
            dc.Close();
            dao.DaoClose();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count == 0)
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
            string begin = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string end = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            label1.Visible = true;
            comboBox1.Visible = true;
            button1.Visible = true;
            textBox3.Visible = true;
            label2.Text = "航班日期："+dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            label3.Text = "航班路线：" + begin + "->" + end;
            label4.Text = "航空公司：" + company + "(" + type + ")";
            label5.Text = "飞行时间：" + begintime + "—" + endtime;
            label6.Text = "剩余票数：" + remain;
            label7.Text = "价格：" + (price * 1.0).ToString();
            label8.Text = "订票数量：";
            textBox3.Text = "1";
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
            string flightid = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            string begin = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string end = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            int remain = int.Parse(label6.Text.Substring(5));
            string route = begin + "-" + end;
            double price = double.Parse(label7.Text.Substring(3));
            string type = comboBox1.Text;
            try
            {
                int number = int.Parse(textBox3.Text);
                if (number > remain)
                {
                    MessageBox.Show("航班座位不足");
                }
                else
                {
                    string sql = $"INSERT into ticket(custid, flightid, type, price,number,detail) VALUES ('{Data.UID}','{flightid}','{type}','{price}','{number}','{route}');update flight set remaining=remaining-{number} where flightid='{flightid}'";
                    Dao dao = new Dao();
                    if (dao.Execute(sql) > 1)//执行了两条sql，大于1才是都成功的
                    {
                        MessageBox.Show("good");
                        //Table();
                    }
                }
            }   
            catch
            {
                MessageBox.Show("请在订票数目中输入数字");
            }
            
        }

        private void 我的订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Order myod = new Order();
            myod.ShowDialog();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                (sender as TextBox).SelectAll();
            });
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            BeginInvoke((Action)delegate
            {
                (sender as TextBox).SelectAll();
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string start = textBox1.Text;
            string end = textBox2.Text;
            if (start == "" && end == "")
                MessageBox.Show("请填写出发地和目的地");
            else if(checkBox1.Checked==false)
            {
                string sql = $"and beginplace = '{start}' and endplace = '{end}'";
                Table(sql);
                dataGridView1.ClearSelection();
            }
            else
            {
                string time = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                //MessageBox.Show(time);
                string sql = $"and beginplace = '{start}' and endplace = '{end}' and DATE_FORMAT(begintime,'%Y-%m-%d')='{time}'";
                Table(sql);
                dataGridView1.ClearSelection();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                dateTimePicker1.Visible = true;
                checkBox1.Text = "";
            }
            else
            {
                checkBox1.Text = "选择日期";
                dateTimePicker1.Visible = false;
            }

        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
