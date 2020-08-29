using QLBENHVIEN_ORCL;
using System;
using System.Windows.Forms;

namespace ConnectOracleWithoutClient
{
    public static class Globals
    {
        public static String curr_session_id;
        public static String curr_session_pw;
        public static bool loggedin = false;
    }
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
            //

        }
    }
}