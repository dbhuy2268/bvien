using System;
using System.Windows.Forms;
using Tutorial.SqlConn;

namespace QLBENHVIEN_ORCL
{
    public partial class grant_revoke_objectPriv : Form
    {
        Grant_privs papaform;
        privs papaformpriv;
        string qq;
        string q;
        int flag;
        string cUser;
        string cRole;
        string tabName;

        public grant_revoke_objectPriv(Grant_privs papa, privs papapriv, string query, string curUser, string curRole, int f)
        {
            this.papaform = papa;
            InitializeComponent();
            papaformpriv = papapriv;
            q = query;
            flag = f;
            qq = query;
            cUser = curUser;
            cRole = curRole;
        }

        public grant_revoke_objectPriv(Grant_privs papa, privs papapriv, string query, string curUser, string curRole, int f, string tableName)
        {
            this.papaform = papa;
            InitializeComponent();
            papaformpriv = papapriv;
            q = query;
            flag = f;
            qq = query;
            cUser = curUser;
            cRole = curRole;
            tabName = tableName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (flag != 5 && flag != 6)
            {
                q = qq;
                q = "GRANT" + q + " TO " + ((flag == 3) ? cUser : cRole);
                DialogResult res = MessageBox.Show("WITH GRANT OPTION?", "Grant option", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    q = q + " with grant option";
                }

                MessageBox.Show(q);

                DBUtils dbu = new DBUtils();
                if (dbu.ExecuteNonQuery(q))
                {
                    MessageBox.Show("GRANT THANH CONG");
                    papaformpriv.loadDT();
                    if (flag == 1 || flag == 3)
                    {
                        papaformpriv.LoadUserObjPriv_gridView();

                    }
                    else
                    {
                        papaformpriv.LoadRoleObjPriv_gridView();

                    }
                    this.Close();

                    return;
                }
                MessageBox.Show("GRANT THAT BAI");
            }

            else
            {
                q = qq;
                q = "CREATE OR REPLACE " + q;
                string qGrant = "GRANT SELECT ON " + ((flag == 5) ? cUser : cRole) + "_" + tabName + " TO " + ((flag == 5) ? cUser : cRole);

                DialogResult res = MessageBox.Show("WITH GRANT OPTION?", "Grant option", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    qGrant = qGrant + " with grant option";
                }

                MessageBox.Show(q);
                MessageBox.Show(qGrant);
                DBUtils dbu = new DBUtils();
                if (dbu.ExecuteNonQuery(q))
                {
                    DBUtils dbu1 = new DBUtils();
                    if (dbu1.ExecuteNonQuery(qGrant))
                    {
                        MessageBox.Show("GRANT THANH CONG");
                    }
                    papaformpriv.loadDT();
                    if (flag == 5)
                    {
                        papaformpriv.LoadUserObjPriv_gridView();
                    }
                    if (flag == 6)
                    {
                        papaformpriv.LoadRoleObjPriv_gridView();
                    }
                    this.Close();

                    return;
                }
                MessageBox.Show("GRANT THAT BAI");
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (flag != 5 && flag != 6)
            {
                q = qq;
                q = "REVOKE" + q + " FROM " + ((flag == 3) ? cUser : cRole);
                MessageBox.Show(q);
                DBUtils dbu = new DBUtils();
                try
                {
                    //MessageBox.Show(q);
                    if (dbu.ExecuteNonQuery(q))
                    {
                        MessageBox.Show("REVOKE THANH CONG");
                        papaformpriv.loadDT();
                        if (flag == 1 || flag == 3)
                        {
                            papaformpriv.LoadUserObjPriv_gridView();

                        }
                        if (flag == 2 || flag == 4)
                        {
                            papaformpriv.LoadRoleObjPriv_gridView();

                        }
                        this.Close();
                        return;
                    }
                    MessageBox.Show("REVOKE THAT BAI");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("REVOKE THAT BAI: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("CHUA GRANT QUYEN NAY");
            }
        }

    }
}
