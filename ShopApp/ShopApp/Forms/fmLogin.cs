using ShopApp.Forms;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopApp
{
    public partial class fmLogin : Form
    {
        public fmLogin()
        {
            InitializeComponent();
        }
        private void btnOk_Click(object sender, EventArgs e)
        {
            string username = txtLogin.Text;
            string password = txtPassword.Text;
            try
            {
                List<User> users = Program.context.User.ToList();
                User u = users.FirstOrDefault(p => p.UserName == username && p.Password == password);
                if (u != null)
                {
                    MainWindow mainWindow = new MainWindow(u.Role, u.UserName);
                    mainWindow.Owner = this;
                    this.Hide();
                    txtPassword.Clear();
                    mainWindow.Show();
                }
                else
                {
                    MessageBox.Show("Не верный логин или пароль");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult x = MessageBox.Show("Вы действительно хотите закрыть приложение?", "Выйти", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (x == DialogResult.Cancel)
                e.Cancel = true;

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxVisible_Click(object sender, EventArgs e)
        {
            pictureBoxVisible.Visible = false;
            pictureBoxUnVisible.Visible = true;
            txtPassword.UseSystemPasswordChar = false;
        }

        private void pictureBoxUnVisible_Click(object sender, EventArgs e)
        {
            pictureBoxVisible.Visible = true;
            pictureBoxUnVisible.Visible = false;
            txtPassword.UseSystemPasswordChar = true;
        }
    }
}
