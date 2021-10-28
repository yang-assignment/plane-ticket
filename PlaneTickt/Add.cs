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
    public partial class Add : Form
    {
        public Add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
                MessageBox.Show("含有空项，请重新填写");
            else
            {
                int seat;
                string sql1 = $"select flightid from flight where flightid = '{textBox1.Text}'";
                string sql2 = $"select planeid,seat from plane where planeid = '{textBox2.Text}'";
                string sql3 = $"select lineid from line where lineid = '{textBox3.Text}'";
                
                Dao dao = new Dao();
                IDataReader dc = dao.Read(sql1);
                if(dc.Read() == true)
                {
                    MessageBox.Show("已存在该航线，请重新填写");
                    dc.Close();
                    dao.DaoClose();
                    return;
                }
                dc = dao.Read(sql2);
                if (dc.Read() == false)
                {
                    MessageBox.Show("不存在该飞机型号，请重新填写");
                    dc.Close();
                    dao.DaoClose();
                    return;
                }
                else
                {
                    seat = Convert.ToInt32(dc[1]);
                    dc.Close();
                }
                dc = dao.Read(sql3);
                if (dc.Read() == false)
                {
                    MessageBox.Show("不存在该航线，请重新填写");
                    dc.Close();
                    dao.DaoClose();
                    return;
                }
                string sql4 = $"insert into flight value('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{dateTimePicker1.Value}','{dateTimePicker2.Value}','{textBox4.Text}','{seat}');";
                if(dao.Execute(sql4)>0)
                {
                    MessageBox.Show("添加成功");
                }
                dao.DaoClose();
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }
    }
}
