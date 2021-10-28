using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PlaneTickt
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                string sql = $"INSERT into customer(id, psw, name, phone, birth) VALUES ('{textBox1.Text}','{textBox2.Text}','{textBox3.Text}','{textBox4.Text}','{dateTimePicker1.Value}');";
                Dao dao = new Dao();
                if (dao.Execute(sql) > 0)
                {
                    MessageBox.Show("注册成功");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public bool Check()
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "")
            {
                string id = textBox1.Text;
                string phone = textBox4.Text;

                string sql1 = $"select * from user where id='{id}'";
                Dao dao = new Dao();
                IDataReader reader = dao.Read(sql1);
                if (reader.Read())
                {
                    MessageBox.Show("该用户名已被注册");
                    return false;
                }
                reader.Close();
                dao.DaoClose();
                if (CheckPhoneIsAble(phone) == false)
                {
                    MessageBox.Show("手机号码格式不正确");
                    return false;
                }
                return true;

            }
            else
            {
                MessageBox.Show("输入有空项，请重新输入");
                return false;
            }

        }

        private bool CheckPhoneIsAble(string input)
        {
            if (input.Length < 11)
            {
                return false;
            }
            //电信手机号码正则
            string dianxin = @"^1[3578][01379]\d{8}$";
            Regex regexDX = new Regex(dianxin);
            //联通手机号码正则
            string liantong = @"^1[34578][01256]\d{8}";
            Regex regexLT = new Regex(liantong);
            //移动手机号码正则
            string yidong = @"^(1[012345678]\d{8}|1[345678][012356789]\d{8})$";
            Regex regexYD = new Regex(yidong);
            if (regexDX.IsMatch(input) || regexLT.IsMatch(input) || regexYD.IsMatch(input))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
