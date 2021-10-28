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
    public partial class MainFrm : Form
    {
        public MainFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                Login();
            }
            else
            {
                MessageBox.Show("用户名或密码为空，请重新输入");
            }
        }

        public void Login()
        {
            //用户
            if (radioButton2.Checked == true)
            {
                Dao dao = new Dao();
                string sql = $"select * from customer where id='{textBox1.Text}' and psw='{textBox2.Text}'";
                IDataReader reader = dao.Read(sql);
                if (reader.Read())
                {
                    Data.UID = reader["id"].ToString();
                    Data.Name = reader["name"].ToString();

                    //MessageBox.Show("登录成功");

                    User user = new User();
                    this.Hide();
                    user.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误，请重新输入");
                }
                dao.DaoClose();

            }
            //管理员
            else if (radioButton1.Checked == true)
            {
                Dao dao = new Dao();
                string sql = $"select * from admin where id='{textBox1.Text}' and psw='{textBox2.Text}'";
                IDataReader reader = dao.Read(sql);
                if (reader.Read())
                {
                    MessageBox.Show("登录成功");
                    Admin admin = new Admin();
                    this.Hide();
                    admin.ShowDialog();
                    this.Show();
                }
                else
                {
                    MessageBox.Show("登录失败");
                }
                dao.DaoClose();
            }
            else
            {
                MessageBox.Show("请选中单选框");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.ShowDialog();
        }
    }
}
