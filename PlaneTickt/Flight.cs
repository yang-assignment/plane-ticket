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
    public partial class Flight : Form
    {
        public Flight()
        {
            InitializeComponent();
        }
        private void Flight_Load(object sender, EventArgs e)
        {
            Table("");
            dataGridView1.ClearSelection();
        }

        public void Table(string sql)
        {
            dataGridView1.Rows.Clear();//清空旧数据
            string basesql = "select * from flight";
            string wholesql = basesql + sql + ";";
            Dao dao = new Dao();
            IDataReader dc = dao.Read(wholesql);
            while (dc.Read())
            {
                dataGridView1.Rows.Add(dc[0].ToString(), dc[1].ToString(), dc[2].ToString(), dc[3].ToString(), dc[4].ToString(), dc[5].ToString(), dc[6].ToString()); ;
            }
            dc.Close();
            dao.DaoClose();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                dataGridView1.Rows[0].Selected = true;
            label1.Visible = true;
            label1.Text = "当前选中航班：" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 0)
                dataGridView1.Rows[0].Selected = true;
            string id = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            DialogResult dr = MessageBox.Show("确认删除吗？", "信息提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                string sql = $"delete from flight where flightid='{id}'";
                Dao dao = new Dao();
                if (dao.Execute(sql) > 0)
                {
                    MessageBox.Show("删除成功");
                    Table("");
                }
                else
                {
                    MessageBox.Show("删除失败" + sql);
                }
                dao.DaoClose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Add myad = new Add();
            myad.ShowDialog();
            Table("");
        }
    }
}
