using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Tutorial.SqlConn;

namespace QLBENHVIEN_ORCL
{
    public partial class privs : Form
    {
        String curUser;
        String curRole;
        int FLAG;
        Form1 mainForm;
        public privs(Form1 mainForm, String inp, int flag)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            FLAG = flag;
            if (flag == 1)
            {
                curUser = inp;
                label1.Text = "DANH SÁCH QUYỀN CỦA USER: " + curUser;
            }
            if (flag == 2)
            {
                curRole = inp;
                label1.Text = "DANH SÁCH QUYỀN CỦA ROLE: " + curRole;
            }
            loadDT();

        }

        public void loadDT()
        {
            if (FLAG == 1)
            {
                LoadUserSysPriv_gridView();
                LoadUserObjPriv_gridView();
            }
            if (FLAG == 2)
            {
                LoadRoleSysPriv_gridView();
                LoadRoleObjPriv_gridView();
            }
        }

        public void LoadUserSysPriv_gridView()
        {
            DBUtils dbu = new DBUtils();
            string query = "select * from dba_sys_privs where grantee = '" + curUser + "'";
            DataTable dt = dbu.ExecuteQuery(query);
            dataGridView1.DataSource = dt;
        }
        public void LoadRoleSysPriv_gridView()
        {
            DBUtils dbu = new DBUtils();
            //string query = "select * from role_sys_privs where role = '" + curRole + "'";
            string query = "select * from dba_sys_privs where grantee = '" + curRole + "'";
            DataTable dt = dbu.ExecuteQuery(query);
            dataGridView1.DataSource = dt;
        }

        public void LoadUserObjPriv_gridView()
        {
            DBUtils dbu = new DBUtils();
            //string query = "SELECT GRANTEE, OWNER, NULL AS TABLE_NAME, GRANTOR, PRIVILEGE, GRANTABLE, COMMON, NULL AS TYPE, INHERITED " +
            //    "FROM DBA_COL_PRIVS " +
            //    "WHERE GRANTEE = '"+ curUser +"' " +
            //    "UNION ALL SELECT GRANTEE, OWNER, TABLE_NAME, GRANTOR, PRIVILEGE, GRANTABLE, COMMON, TYPE, INHERITED " +
            //    "FROM DBA_TAB_PRIVS " +
            //    "WHERE GRANTEE = '"+curUser+"'";
            string query = "SELECT GRANTEE, OWNER, TABLE_NAME, COLUMN_NAME, GRANTOR, PRIVILEGE, GRANTABLE, COMMON, 'COLUMN' AS TYPE, INHERITED " +
                "FROM DBA_COL_PRIVS " +
                "WHERE GRANTEE = '" + curUser + "' " +
                "UNION ALL SELECT GRANTEE, OWNER, TABLE_NAME, NULL AS COLUMN_NAME, GRANTOR, PRIVILEGE, GRANTABLE, COMMON, TYPE, INHERITED " +
                "FROM DBA_TAB_PRIVS " +
                "WHERE GRANTEE = '" + curUser + "'";
            DataTable dt = dbu.ExecuteQuery(query);
            dataGridView2.DataSource = dt;
        }

        public void LoadRoleObjPriv_gridView()
        {
            DBUtils dbu = new DBUtils();
            string query = "SELECT * from role_tab_privs where role = '" + curRole + "'";
            DataTable dt = dbu.ExecuteQuery(query);
            dataGridView2.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FLAG == 1)
            {
                create frm_createUsr = new create(this.mainForm, curUser, curRole, 3);
                frm_createUsr.Show();
            }
            if (FLAG == 2)
            {
                create frm_createUsr = new create(this.mainForm, curUser, curRole, 4);
                frm_createUsr.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Grant_privs frm_grantsys = new Grant_privs(this, FLAG, curUser, curRole);
            frm_grantsys.Show();
        }

        public void clean_litter()
        {
            List<string> listView_gonnabedropped = new List<string>();
            DBUtils dbu = new DBUtils();

            string query;
            if (FLAG == 1)
            {
                query = "SELECT GRANTEE, OWNER, TABLE_NAME, COLUMN_NAME, GRANTOR, PRIVILEGE, GRANTABLE, COMMON, 'COLUMN' AS TYPE, INHERITED " +
                "FROM DBA_COL_PRIVS " +
                "WHERE GRANTEE = '" + curUser + "' " +
                "UNION ALL SELECT GRANTEE, OWNER, TABLE_NAME, NULL AS COLUMN_NAME, GRANTOR, PRIVILEGE, GRANTABLE, COMMON, TYPE, INHERITED " +
                "FROM DBA_TAB_PRIVS " +
                "WHERE GRANTEE = '" + curUser + "'";
            }
            else //flag == 2
            {
                query = "select * from role_tab_privs where role = '"+curRole+"'";
            }

            DataTable dt = dbu.ExecuteQuery(query);
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (FLAG == 1)
                {
                    if (dr["TABLE_NAME"].ToString().Contains(curUser))
                    {
                        listView_gonnabedropped.Add(dr["TABLE_NAME"].ToString());
                        DBUtils dbu1 = new DBUtils();
                        string query_DROPVIEW = "DROP VIEW " + listView_gonnabedropped[i++];
                        if (dbu1.ExecuteNonQuery(query_DROPVIEW))
                        {
                            MessageBox.Show("TRASH CLEANED " + query_DROPVIEW);
                        }
                    }
                }
                if (FLAG == 2)
                {
                    if (dr["TABLE_NAME"].ToString().Contains(curRole))
                    {
                        listView_gonnabedropped.Add(dr["TABLE_NAME"].ToString());
                        DBUtils dbu1 = new DBUtils();
                        string query_DROPVIEW = "DROP VIEW " + listView_gonnabedropped[i++];
                        if (dbu1.ExecuteNonQuery(query_DROPVIEW))
                        {
                            MessageBox.Show("TRASH CLEANED " + query_DROPVIEW);
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FLAG == 1)
            {
                DBUtils dbu = new DBUtils();
                String query = "drop user " + curUser + " cascade";
                clean_litter();
                bool res = dbu.ExecuteNonQuery(query);
                this.Close();
                this.mainForm.LoadUser_gridView();
            }
            if (FLAG == 2)
            {
                DBUtils dbu = new DBUtils();
                String query = "drop role " + curRole;
                clean_litter();
                bool res = dbu.ExecuteNonQuery(query);
                this.Close();
                this.mainForm.LoadRole_gridView();
            }
        }

    }
}
