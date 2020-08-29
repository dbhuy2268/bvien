using System;
using System.Windows.Forms;
using Tutorial.SqlConn;

namespace QLBENHVIEN_ORCL
{
    public partial class create : Form
    {
        int flag;
        String user, role;
        Form1 mainForm;

        public create(Form1 mainForm, int inp)
        {
            this.mainForm = mainForm;
            InitializeComponent();
            flag = inp;
        }

        public create(Form1 mainForm, String User, String Role, int inp)
        {
            InitializeComponent();
            flag = inp;
            user = User;
            role = Role;
            this.mainForm = mainForm;

            label2.Visible = false;
            textBox1.Visible = false;
            this.button3.Text = "ĐỔI";

        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (flag == 1)
            {
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    MessageBox.Show("Vui long nhap du thong tin");
                }
                else
                {
                    DBUtils dbu = new DBUtils();
                    String query = "create user " + textBox1.Text + " identified by " + textBox2.Text;
                    bool res = dbu.ExecuteNonQuery(query);
                    this.Close();
                }  
            }
            if (flag == 2)
            {
                if (textBox1.Text == "")
                {
                    MessageBox.Show("Vui long nhap ten");
                }
                else
                {
                    String pass = "";
                    if (textBox2.Text != "")
                    {
                        pass += " identified by " + textBox2.Text;
                    }
                    DBUtils dbu = new DBUtils();
                    String query = "create role " + textBox1.Text + pass;
                    bool res = dbu.ExecuteNonQuery(query);
                    this.Close();
                }
            }
            if (flag == 3)
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Vui long nhap mat khau moi");
                }
                else
                {
                    DBUtils dbu = new DBUtils();
                    String query = "ALTER USER " + user + " IDENTIFIED BY " + textBox2.Text;
                    MessageBox.Show(query);

                    bool res = dbu.ExecuteNonQuery(query);
                    MessageBox.Show("ALTER USER SUCCESSFUL.");
                    this.Close();
                }
            }
            if (flag == 4)
            {
                    DBUtils dbu = new DBUtils();
                    String query = "ALTER ROLE " + role + ((textBox2.Text == "") ? " NOT IDENTIFIED " : " IDENTIFIED BY " + textBox2.Text);
                    if (dbu.ExecuteNonQuery(query))
                    {
                        MessageBox.Show("ALTER ROLE SUCCESSFUL.");
                    }
                    this.Close();
                
            }
            mainForm.LoadUser_gridView();
            mainForm.LoadRole_gridView();
        }

        private void create_Load(object sender, EventArgs e)
        {

        }
    }
}
