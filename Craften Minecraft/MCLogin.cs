using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CmlLib.Launcher;
using System.Threading;
namespace Craften_Minecraft
{
    public partial class MCLogin : Form
    {
        public MCLogin()
        {
            InitializeComponent();
        }
        MLogin login = new MLogin();
        MSession session;
        bool islogged = Properties.Settings.Default.islogged;
        private void MCLogin_Load(object sender, EventArgs e)
        {
            if(islogged)
            {
                BLogin.Enabled = false;
                Email.ReadOnly = true;
                password.ReadOnly = true;
                UserName.Text = "Name: " + Properties.Settings.Default.Keks;
                password.Text = "123";
                Email.Text = Properties.Settings.Default.Email;
                Properties.Settings.Default.Save();
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            BLogin.Enabled = false;
            if (password.Text == "")
            {
                session = MSession.GetOfflineSession(Email.Text);
                MessageBox.Show("Offline login Success : " + Email.Text);
                Properties.Settings.Default.Keks = Email.Text;
                Properties.Settings.Default.islogged = true;

            }
            else
            {
                var th = new Thread(new ThreadStart(delegate
                {
                    var login = new MLogin();
                    var result = login.Authenticate(Email.Text, password.Text);
                    if (result.Result == MLoginResult.Success)
                    {
                        Properties.Settings.Default.islogged = true;

                        MessageBox.Show("Login Success");
                        Invoke((MethodInvoker)delegate { UserName.Text = "Name: " + result.Username; });
                        session = result;
                        Properties.Settings.Default.Keks = result.Username;
                        Properties.Settings.Default.Email = Email.Text;
                        Main form = (Main)this.Owner;
                        Invoke((MethodInvoker)delegate { form.McName.Text = "Eingeloggt als: " + Properties.Settings.Default.Keks; });
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.islogged = false;

                        MessageBox.Show(result.Result.ToString() + "\n" + result.Message);
                        Invoke((MethodInvoker)delegate { BLogin.Enabled = true; });
                    }
                }));
                th.Start();
            }
        }
    }
}
