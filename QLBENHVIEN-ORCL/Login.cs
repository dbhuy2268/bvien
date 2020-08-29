using ConnectOracleWithoutClient;
using Oracle.DataAccess.Client;
using System;
using System.Windows.Forms;
using Tutorial.SqlConn;

namespace QLBENHVIEN_ORCL
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //";DBA Privilege = SYSDBA;"
            Globals.curr_session_id = textBox1.Text;
            Globals.curr_session_pw = textBox2.Text;

            OracleConnection conn = DBUtils.GetDBConnection();
            try
            {
                conn.Open();
                MessageBox.Show("Test connection to OracleDB OK");
                MessageBox.Show(Globals.curr_session_id + " | " + Globals.curr_session_pw);

                //if (Globals.curr_session_id == "DBA_BV01")
                //{
                Globals.loggedin = true;
                this.Hide();
                Form1 f1 = new Form1();
                f1.ShowDialog();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("## ERROR: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.AcceptButton = button1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
