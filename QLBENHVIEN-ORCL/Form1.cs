using System;
using System.Data;
using System.Windows.Forms;
using Tutorial.SqlConn;

namespace QLBENHVIEN_ORCL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUser_gridView();
            LoadRole_gridView();
        }

        public void LoadUser_gridView()
        {
            DBUtils dbu = new DBUtils();
            string query = "select * from dba_users where account_status = 'OPEN' AND USERNAME NOT IN ('SYS', 'SYSTEM', 'DBA_BV01')";
            DataTable dt = dbu.ExecuteQuery(query);
            grv_all_user.DataSource = dt;
        }

        public void LoadRole_gridView()
        {
            DBUtils dbu = new DBUtils();
            string query = "select * from dba_roles where ORACLE_MAINTAINED = 'N'";
            DataTable dt = dbu.ExecuteQuery(query);
            grv_all_role.DataSource = dt;
        }

        private void grv_all_user_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grv_all_user.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                grv_all_user.CurrentRow.Selected = true;
                string uname = grv_all_user.Rows[e.RowIndex].Cells["USERNAME"].FormattedValue.ToString();
                privs frm_priv = new privs(this, uname, 1);
                frm_priv.ShowDialog();
            }
        }

        private void grv_all_role_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (grv_all_role.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
                grv_all_role.CurrentRow.Selected = true;
                string rolename = grv_all_role.Rows[e.RowIndex].Cells["ROLE"].FormattedValue.ToString();
                privs frm_priv = new privs(this, rolename, 2);
                frm_priv.ShowDialog();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            create frm_createUsr = new create(this, 2);
            frm_createUsr.Show();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            create frm_createUsr = new create(this, 1);
            frm_createUsr.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            LoadUser_gridView();
            LoadRole_gridView();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
