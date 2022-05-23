using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class FormLogin : Form
    {
        private DataAccess Da { get; set; }
        public FormLogin()
        {
            InitializeComponent();
            this.Da = new DataAccess();
        }

        internal void ClearAll()
        {
            txtUserName.Clear();
            txtPassword.Clear();
        }
        private bool isValid()
        {
            if (String.IsNullOrEmpty(this.txtUserName.Text) || String.IsNullOrEmpty(this.txtPassword.Text))
            {
                return false;
            }
            else
                return true;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isValid())
                {
                    MessageBox.Show("Please Fill All the Fields", "Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                string username = txtUserName.Text;
                if (username.Substring(0, 1) == "E")
                {
                    string sql = "SELECT *FROM employee_records WHERE e_user_name = '" + this.txtUserName.Text + "' and e_password='" + this.txtPassword.Text + "' ;";

                    var ds = this.Da.ExecuteQuery(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Employee Login");
                        this.Hide();
                        new FormEmployee().Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Information ");
                        this.ClearAll();
                        this.txtUserName.Focus();
                    }
                }
                else if(username.Substring(0, 1) == "M")
                {
                    string sql = "SELECT *FROM manager_records WHERE m_user_name = '" + this.txtUserName.Text + "' and m_password='" + this.txtPassword.Text + "' ;";

                    var ds = this.Da.ExecuteQuery(sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {                      
                        MessageBox.Show("Manager Login");
                        this.Hide();
                        new FormManager().Show();

                    }
                    else
                    {
                        MessageBox.Show("Incorrect Information ");
                        this.ClearAll();
                        this.txtUserName.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect Information ");
                    this.ClearAll();
                    this.txtUserName.Focus();
                }

                Da.Sqlcon.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.ClearAll();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
