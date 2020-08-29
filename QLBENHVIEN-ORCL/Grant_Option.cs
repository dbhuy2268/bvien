using System;
using System.Data;
using System.Windows.Forms;
using Tutorial.SqlConn;

namespace QLBENHVIEN_ORCL
{
    public partial class Grant_Option : Form
    {
        string query;
        string Cur_user;
        string Cur_role;
        int flag;
        Grant_privs mainForm = null;
        public Grant_Option(Grant_privs callingForm, int f, String cU, String cR, String q)
        {
            query = q;
            flag = f;
            Cur_user = cU;
            Cur_role = cR;
            this.mainForm = callingForm;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DBUtils dbu = new DBUtils();
            DataTable dt = dbu.ExecuteQuery(query + " with admin option");
            this.Hide();
            if (flag == 1)
            {
                mainForm.LoadAllUserSysPriv_gridView();
                mainForm.privcs.LoadUserSysPriv_gridView();
            }
            if (flag == 2)
            {
                mainForm.LoadAllRoleSysPriv_gridView();
                mainForm.privcs.LoadRoleSysPriv_gridView();
            }
            else
            { 
                mainForm.LoadAllRole_ForGrantUser();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DBUtils dbu = new DBUtils();
            DataTable dt = dbu.ExecuteQuery(query);
            this.Hide();
            if (flag == 1)
            {
                mainForm.LoadAllUserSysPriv_gridView();
                mainForm.privcs.LoadUserSysPriv_gridView();
            }
            if (flag == 2)
            {
                mainForm.LoadAllRoleSysPriv_gridView();
                mainForm.privcs.LoadRoleSysPriv_gridView();
            }
            else
            {
                mainForm.LoadAllRole_ForGrantUser();
            }
        }
    }
}
