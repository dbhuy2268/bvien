using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Tutorial.SqlConn;

namespace QLBENHVIEN_ORCL
{
    public partial class Grant_privs : Form
    {
        String Cur_user;
        String Cur_role;
        public privs privcs;
        int flag;
        String tableName;
        public Grant_privs(privs privcs, int inp, string cur_user, string cur_role)
        {
            this.privcs = privcs;
            Cur_user = cur_user;
            Cur_role = cur_role;
            flag = inp;
            InitializeComponent();
        }

        public Grant_privs(privs privcs, int inp, string cur_user, string cur_role, string tabName)
        {
            this.privcs = privcs;
            Cur_user = cur_user;
            Cur_role = cur_role;
            tableName = tabName;
            flag = inp;
            InitializeComponent();
        }

        //public List<string> get_granted_Roles()
        //{
        //    List<string> kq = new List<string>();
        //    DBUtils dbu = new DBUtils();

        //    string query = "select * from dba_role_privs where grantee = '"+Cur_user+"'";

        //    DataTable dt = dbu.ExecuteQuery(query);

        //    foreach(DataRow dr in dt.Rows)
        //    {
        //        string grantedRole = dr["GRANTED_ROLE"].ToString();
        //        kq.Add(grantedRole);
        //    }

        //    return kq;
        //}

        private void Grant_SYS_privs_Load(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                LoadAllUserSysPriv_gridView();
                LoadAllUserObjPriv_gridView();
                LoadAllRole_ForGrantUser();
            }
            if (flag == 2)
            {
                this.tabControl1.TabPages.Remove(tabPage3);
                LoadAllRoleSysPriv_gridView();
                LoadAllUserObjPriv_gridView();
            }
            if (flag == 3 || flag == 4) //load column
            {
                this.tabControl1.TabPages.Remove(tabPage3);
                LoadAllTabColumns_gridView();
            }
        }

        public void LoadAllRole_ForGrantUser()
        {
            DBUtils dbu = new DBUtils();
            string query = "select GRANTED_ROLE, 1 ENABLED from dba_role_privs where grantee = '"+Cur_user+"'" +
                " UNION ALL select ROLE AS GRANTED_ROLE, 0 ENABLED from dba_roles WHERE ORACLE_MAINTAINED = 'N'" +
                " AND ROLE NOT IN(select GRANTED_ROLE from dba_role_privs where grantee = '"+Cur_user+"')";

            DataTable dt = dbu.ExecuteQuery(query);

            dataGridView3.DataSource = dt;
            ChangeColumnDataType(dt, "ENABLED", typeof(Boolean));

            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        public void LoadAllUserSysPriv_gridView()
        {
            DBUtils dbu = new DBUtils();
            string query =
                "SELECT PRIVILEGE, 1 as ENABLED FROM DBA_SYS_PRIVS WHERE GRANTEE ='" + Cur_user + "' " +
                "UNION ALL SELECT PRIVILEGE, 0 AS ENABLED FROM DBA_SYS_PRIVS " +
                "WHERE GRANTEE = 'XX' " +
                "AND PRIVILEGE NOT IN(" +
                "SELECT PRIVILEGE " +
                "FROM DBA_SYS_PRIVS " +
                "WHERE GRANTEE ='" + Cur_user + "')";

            DataTable dt = dbu.ExecuteQuery(query);

            ChangeColumnDataType(dt, "ENABLED", typeof(Boolean));

            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        public void LoadAllRoleSysPriv_gridView()
        {
            DBUtils dbu = new DBUtils();
            string query =
                "SELECT PRIVILEGE, 1 as ENABLED FROM DBA_SYS_PRIVS WHERE GRANTEE ='" + Cur_role + "' " +
                "UNION ALL SELECT PRIVILEGE, 0 AS ENABLED FROM DBA_SYS_PRIVS " +
                "WHERE GRANTEE = 'XX' " +
                "AND PRIVILEGE NOT IN(" +
                "SELECT PRIVILEGE " +
                "FROM DBA_SYS_PRIVS " +
                "WHERE GRANTEE ='" + Cur_role + "')";

            DataTable dt = dbu.ExecuteQuery(query);

            ChangeColumnDataType(dt, "ENABLED", typeof(Boolean));

            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        public void LoadAllTabColumns_gridView()
        {
            string query = "SELECT column_name, data_type, data_length FROM USER_TAB_COLUMNS WHERE TABLE_NAME = '" + tableName + "'";
            DBUtils dbu = new DBUtils();

            DataTable dt = dbu.ExecuteQuery(query);

            dataGridView2.DataSource = dt;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        public void LoadAllUserObjPriv_gridView()
        {
            DBUtils dbu = new DBUtils();
            string query = "select OBJECT_NAME, OWNER, OBJECT_TYPE, CREATED from dba_objects WHERE object_type = 'TABLE' AND OWNER = 'DBA_BV01' order by created DESC";

            DataTable dt = dbu.ExecuteQuery(query);

            dataGridView2.DataSource = dt;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        }

        public static bool ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            if (table.Columns.Contains(columnname) == false)
                return false;

            DataColumn column = table.Columns[columnname];
            if (column.DataType == newtype)
                return true;

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newtype);
                table.Columns.Add(newcolumn);
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row["temporary"] = Convert.ChangeType(row[columnname], newtype);
                    }
                    catch
                    {
                    }
                }
                table.Columns.Remove(columnname);
                newcolumn.ColumnName = columnname;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dataGridView1.CurrentRow.Selected = true;
                    string priv = dataGridView1.Rows[e.RowIndex].Cells["PRIVILEGE"].FormattedValue.ToString();
                    string enabled = dataGridView1.Rows[e.RowIndex].Cells["ENABLED"].FormattedValue.ToString();

                    if (flag == 1)
                    {
                        if (enabled == "False")
                        {
                            string query = "grant " + priv + " to " + Cur_user;
                            Form go = new Grant_Option(this, 1, Cur_user, Cur_role, query);
                            go.ShowDialog();
                        }
                        else
                        {
                            string query = "revoke " + priv + " from " + Cur_user;
                            DBUtils dbu = new DBUtils();
                            DataTable dt = dbu.ExecuteQuery(query);
                            LoadAllUserSysPriv_gridView();
                            this.privcs.LoadUserSysPriv_gridView();
                        }
                    }

                    else
                    {
                        if (enabled == "False")
                        {
                            string query = "grant " + priv + " to " + Cur_role;
                            Form go = new Grant_Option(this, 2, Cur_user, Cur_role, query);
                            go.Show();
                        }
                        else
                        {
                            string query = "revoke " + priv + " from " + Cur_role;
                            DBUtils dbu = new DBUtils();
                            DataTable dt = dbu.ExecuteQuery(query);
                            LoadAllRoleSysPriv_gridView();
                            this.privcs.LoadRoleSysPriv_gridView();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
            
        }

        public DataTable data
        {
            set { dataGridView1.DataSource = value; }
        }

        private List<string> get_CurrentViewStatus()
        {
            List<string> kq = new List<string>();
            string ten = (flag == 1 || flag == 3 ? Cur_user : Cur_role);
            kq.Add(ten);


            string query = "SELECT table_name, column_name, data_type" +
                " FROM all_tab_columns" +
                " WHERE table_name = '" + ten + "_" + tableName + "'" +
                " AND owner = 'DBA_BV01'" +
                " ORDER BY column_id";

            DBUtils dbu = new DBUtils();
            DataTable dt = dbu.ExecuteQuery(query);

            foreach (DataRow dr in dt.Rows)
            {
                string colName = dr["COLUMN_NAME"].ToString();
                kq.Add(colName);
            }

            return kq;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //GRANT MỊN TRÊN CỘT, FLAG == 3 LÀ USER, FLAG == 4 LÀ ROLE
                if (flag == 3 || flag == 4)
                {
                    // grant update(malichtruc) on lichtruc to ACCMASTER;
                    // dataGridView2[4, e.RowIndex].Value.ToString()
                    if (e.ColumnIndex == 0) //SELECT
                    {
                        List<string> kq = get_CurrentViewStatus();
                        if (kq.Contains(dataGridView2[4, e.RowIndex].Value.ToString()))
                        {
                            DialogResult res = MessageBox.Show("DA TON TAI VIEW VOI QUYEN NAY. REVOKE?", "REVOKE?", MessageBoxButtons.YesNo);
                            if (res == DialogResult.Yes)
                            {
                                kq.Remove(dataGridView2[4, e.RowIndex].Value.ToString());
                                string query_revoke = "CREATE OR REPLACE VIEW " + (flag == 3 ? Cur_user : Cur_role) + "_" + tableName + " as " + "select ";
                                for (int i = 1; i < kq.Count - 1; i++)
                                {
                                    query_revoke += kq[i] + ", ";
                                }

                                query_revoke += kq[kq.Count - 1];
                                query_revoke += " from " + tableName;

                                DBUtils dbu = new DBUtils();

                                if (kq.Count == 1)
                                {
                                    query_revoke = "DROP VIEW " + (flag == 3 ? Cur_user : Cur_role) + "_" + tableName;
                                }
                                MessageBox.Show(query_revoke);
                                if (dbu.ExecuteNonQuery(query_revoke))
                                {
                                    MessageBox.Show("REVOKE THANH CONG");
                                    privcs.loadDT();
                                    if (flag == 1 || flag == 3)
                                    {
                                        privcs.LoadUserObjPriv_gridView();
                                    }
                                    if (flag == 2 || flag == 4)
                                    {
                                        privcs.LoadRoleObjPriv_gridView();
                                    }
                                    return;
                                }
                                MessageBox.Show("REVOKE THAT BAI");

                            }
                            return;
                        }
                        kq.Add(dataGridView2[4, e.RowIndex].Value.ToString());
                        string query = " VIEW " + (flag == 3 ? Cur_user : Cur_role) + "_" + tableName + " as " + "select ";
                        for (int i = 1; i < kq.Count - 1; i++)
                        {
                            query += kq[i] + ", ";
                        }

                        query += kq[kq.Count - 1];
                        query += " from " + tableName;

                        grant_revoke_objectPriv grant_Revoke_ObjectPriv = new grant_revoke_objectPriv(this, privcs, query, Cur_user, Cur_role, (flag == 4 ? 6 : 5), tableName);
                        grant_Revoke_ObjectPriv.ShowDialog();
                    }
                    if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                    {
                        MessageBox.Show("KHONG HO TRO INSERT VA DELETE MIN TREN COT");
                        return;
                    }

                    if (e.ColumnIndex == 3) //UPDATE
                    {
                        string query = " UPDATE(" + dataGridView2[4, e.RowIndex].Value.ToString() + ") on " + tableName;

                        grant_revoke_objectPriv grant_Revoke_ObjectPriv = new grant_revoke_objectPriv(this, privcs, query, Cur_user, Cur_role, flag);
                        grant_Revoke_ObjectPriv.ShowDialog();
                    }
                }
                else
                {
                    if (e.ColumnIndex == 0) //SELECT
                    {
                        string query = " SELECT ON " + dataGridView2[5, e.RowIndex].Value.ToString() + "." + dataGridView2[4, e.RowIndex].Value.ToString();
                        grant_revoke_objectPriv grant_Revoke_ObjectPriv = new grant_revoke_objectPriv(this, privcs, query, Cur_user, Cur_role, (flag == 2 ? 4 : 3));
                        grant_Revoke_ObjectPriv.ShowDialog();
                    }
                    if (e.ColumnIndex == 1) //INSERT
                    {
                        string query = " INSERT ON " + dataGridView2[5, e.RowIndex].Value.ToString() + "." + dataGridView2[4, e.RowIndex].Value.ToString();
                        grant_revoke_objectPriv grant_Revoke_ObjectPriv = new grant_revoke_objectPriv(this, privcs, query, Cur_user, Cur_role, (flag == 2 ? 4 : 3));
                        grant_Revoke_ObjectPriv.ShowDialog();
                    }
                    if (e.ColumnIndex == 2) //DELETE
                    {
                        string query = " DELETE ON " + dataGridView2[5, e.RowIndex].Value.ToString() + "." + dataGridView2[4, e.RowIndex].Value.ToString();
                        grant_revoke_objectPriv grant_Revoke_ObjectPriv = new grant_revoke_objectPriv(this, privcs, query, Cur_user, Cur_role, (flag == 2 ? 4 : 3));
                        grant_Revoke_ObjectPriv.ShowDialog();
                    }
                    if (e.ColumnIndex == 3) //UPDATE
                    {
                        string query = " UPDATE ON " + dataGridView2[5, e.RowIndex].Value.ToString() + "." + dataGridView2[4, e.RowIndex].Value.ToString();
                        grant_revoke_objectPriv grant_Revoke_ObjectPriv = new grant_revoke_objectPriv(this, privcs, query, Cur_user, Cur_role, (flag == 2 ? 4 : 3));
                        grant_Revoke_ObjectPriv.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
            
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //SELECT column_name, data_type, data_length FROM USER_TAB_COLUMNS WHERE TABLE_NAME = ''
                if (e.ColumnIndex == 0) //SELECT
                {
                    return;
                }
                if (e.ColumnIndex == 1) //INSERT
                {
                    return;
                }
                if (e.ColumnIndex == 2) //DELETE
                {
                    return;
                }
                if (e.ColumnIndex == 3) //UPDATE
                {
                    return;
                }
                Grant_privs new_gp = new Grant_privs(privcs, (flag == 1 ? 3 : 4), Cur_user, Cur_role, dataGridView2.Rows[e.RowIndex].Cells["OBJECT_NAME"].Value.ToString());
                new_gp.tabControl1.TabPages.Remove(new_gp.tabPage1);
                new_gp.ShowDialog();
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView3.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    dataGridView3.CurrentRow.Selected = true;
                    string Granted_role = dataGridView3.Rows[e.RowIndex].Cells["GRANTED_ROLE"].FormattedValue.ToString();
                    string enabled = dataGridView3.Rows[e.RowIndex].Cells["ENABLED"].FormattedValue.ToString();

                    string query;
                    if (enabled == "False")
                    {
                        query = "grant " + Granted_role + " to " + Cur_user;
                        Grant_Option go = new Grant_Option(this, 3, Cur_user, Cur_role, query);
                        go.ShowDialog();
                    }
                    else
                    {
                        query = "revoke " + Granted_role + " from " + Cur_user;
                        DBUtils dbu = new DBUtils();
                        if (dbu.ExecuteNonQuery(query))
                        {
                            this.LoadAllRole_ForGrantUser();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }


        //private void grv_all_role_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        // SELECT COLUMN_NAME, DATA_TYPE, DATA_LENGTH, NULLABLE FROM DBA_TAB_COLS WHERE TABLE_NAME = 'THUOC';
        //    if (grv_all_role.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
        //    {
        //        grv_all_role.CurrentRow.Selected = true;
        //        string rolename = grv_all_role.Rows[e.RowIndex].Cells["ROLE"].FormattedValue.ToString();
        //        privs frm_priv = new privs(this, rolename, 2);
        //        frm_priv.ShowDialog();
        //    }
        //}
    }
}
