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
    public partial class FormManager : Form
    {
        private DataAccess Da { get; set; }
        public FormManager()
        {
            InitializeComponent();
            this.Da = new DataAccess();

            this.PopulateGridViewProduct();
            this.PopulateGridViewEmployee();

            this.dgvProduct.ClearSelection();
            this.dgvEmployee.ClearSelection();

        }
        private void FormManager_Load(object sender, EventArgs e)
        {
            this.dgvEmployee.ClearSelection();
            this.dgvProduct.ClearSelection();

            this.AutoIdProduct();
            this.AutoIdEmployee();

        }
        // Product operation
        private void PopulateGridViewProduct(string sql = "select * from Product;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvProduct.AutoGenerateColumns = false;
            this.dgvProduct.DataSource = ds.Tables[0];
        }
        private bool IsValidToSaveProduct()
        {
            if (String.IsNullOrEmpty(this.txtProductName.Text) || String.IsNullOrEmpty(this.txtProductPrice.Text) ||
                String.IsNullOrEmpty(this.txtProductQuantity.Text) || String.IsNullOrEmpty(this.txtProductId.Text))
            {
                return false;
            }
            else
                return true;
        }

        private void btnShowProduct_Click(object sender, EventArgs e)
        {
            this.PopulateGridViewProduct();
            this.dgvProduct.ClearSelection();
        }
        public void ClearProduct()
        {
            this.txtProductName.Clear();
            this.txtProductPrice.Clear();
            this.txtProductQuantity.Clear();
            this.txtProductId.Clear();

            this.dgvProduct.ClearSelection();
            
        }
        private void RefreshContentProduct()
        {
            this.txtProductId.Clear();
            this.txtProductName.Clear();
            this.txtProductPrice.Clear();
            this.txtProductQuantity.Clear();

            this.AutoIdProduct();
            this.dgvProduct.ClearSelection();
        }
        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            if (!IsValidToSaveProduct())
            {
                MessageBox.Show("Please Fill All the Fields", "Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            Product product = new Product(this.txtProductId.Text,this.txtProductName.Text, Convert.ToDouble(this.txtProductPrice.Text), Convert.ToInt32(this.txtProductQuantity.Text));

            try
            {
                var sql = "select * from product where Id = '" + this.txtProductId.Text + "';";
                DataTable dt = this.Da.ExecuteQueryTable(sql);
                if (dt.Rows.Count >= 1)
                {
                    sql = "update product set NAME ='" + this.txtProductName.Text + "', PRICE = " + (this.txtProductPrice.Text) + ", QUANTITY = " + (this.txtProductQuantity.Text) + " where ID = '" + this.txtProductId.Text + "';";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count >= 1)
                    {
                        MessageBox.Show("Data updated successfully");
                        this.PopulateGridViewProduct();
                        this.ClearProduct();
                    }
                    else
                        MessageBox.Show("Data upgradation failed");
                }
                else
                {
                    sql = "insert into product values('" + product.Id + "','" + product.Name + "'," + product.Price + "," + product.Quantity + ");";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count == 1)
                    {
                        MessageBox.Show("Data insertion successfull");
                        this.PopulateGridViewProduct();
                        this.ClearProduct();
                    }
                    else
                    {
                        MessageBox.Show("Data insertion failed");
                        this.ClearProduct();
                    }
                }
                this.RefreshContentProduct();
                this.PopulateGridViewProduct();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
                this.ClearProduct();
            }
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    string id = this.txtProductId.Text;
            //    int count = 0;
            //    var sql = "delete from product where Id = '"+this.txtProductId.Text+"';";
            //    count = this.Da.ExecuteDMLQuery(sql);

            //    if (count == 1)
            //    {
            //        MessageBox.Show(id + " has been deleted successfully");
            //        this.PopulateGridViewProduct();
            //        this.ClearProduct();
            //    }
                    
            //    else
            //        MessageBox.Show("Data deletion failed");
            //}
            //catch (Exception exc)
            //{
            //    MessageBox.Show("An error has occured: " + exc.Message);
            //}

            if (this.dgvProduct.SelectedRows.Count < 1)
            {
                MessageBox.Show("Please select a row first to delete", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                return;
            }
            try
            {
                var id = this.dgvProduct.CurrentRow.Cells[0].Value.ToString();
                var title = this.dgvProduct.CurrentRow.Cells[1].Value.ToString();

                var sql = "delete from product where Id = '" + id + "';";
                var rowCount = this.Da.ExecuteDMLQuery(sql);

                if (rowCount == 1)
                {
                    MessageBox.Show(title + " has been deleetd from the list.");
                }
                else
                {
                    MessageBox.Show("Data deletion operation failed.");
                }

                this.PopulateGridViewProduct();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured, please try again later. Error msg: " + exc.Message);
            }
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (String.IsNullOrEmpty(this.cboSearchProduct.Text))
                {
                    this.cboSearchProduct.Text = "Id";
                }
                string sql = "select * from product where "+this.cboSearchProduct.Text+" = '" + this.txtSearchProduct.Text + "';";
                this.PopulateGridViewProduct(sql);
                this.txtSearchProduct.Clear();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
            }

            this.txtSearchProduct.Clear();
        }
   
        private void dgvProduct_DoubleClick(object sender, EventArgs e)
        {
            this.txtProductId.Text = this.dgvProduct.CurrentRow.Cells["ID"].Value.ToString();
            this.txtProductName.Text = this.dgvProduct.CurrentRow.Cells["Name"].Value.ToString();
            this.txtProductPrice.Text = this.dgvProduct.CurrentRow.Cells["Price"].Value.ToString();
            this.txtProductQuantity.Text = this.dgvProduct.CurrentRow.Cells["Quantity"].Value.ToString();
        }

        private void btnClearProduct_Click(object sender, EventArgs e)
        {
            this.RefreshContentProduct();
        }

        private void AutoIdProduct()
        {
            try
            {
                var sql = "select * from product order by Id desc;";
                var dt = this.Da.ExecuteQueryTable(sql);
                var lastId = dt.Rows[0][0].ToString();
                string[] temp = lastId.Split('-');
                int no = Convert.ToInt32(temp[1]);
                string newId = "P-" + (++no).ToString("d3");
                this.txtProductId.Text = newId;
            }
            catch (Exception ex)
            {
                this.txtProductId.Text = "P-001";
            }          
        }


        // Employee Operation

        private void PopulateGridViewEmployee(string sql = "select * from employee_records;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvEmployee.AutoGenerateColumns = false;
            this.dgvEmployee.DataSource = ds.Tables[0];
        }

        private bool IsValidToSaveEmployee()
        {
            if (String.IsNullOrEmpty(this.txtEmployeeName.Text) || String.IsNullOrEmpty(this.txtEmployeePassword.Text) ||
                String.IsNullOrEmpty(this.dtpEmployeeJoiningDate.Text) || String.IsNullOrEmpty(this.txtEmployeeSalary.Text))
            {
                return false;
            }
            else
                return true;
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if(dgvEmployee.SelectedRows.Count < 1)
            {
                MessageBox.Show("Please select a row first to delete","Info",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                return;
            }
            try
            {
                var id=this.dgvEmployee.CurrentRow.Cells[0].Value.ToString();
                var name= this.dgvEmployee.CurrentRow.Cells[1].Value.ToString();

                var sql = "delete from employee_records where e_user_name= '" + id + "';";
                int count = this.Da.ExecuteDMLQuery(sql);

                if (count == 1)
                {
                    MessageBox.Show(name + " has been deleted successfully");                   
                    //this.ClearEmployee();
                }

                else
                    MessageBox.Show("Data deletion failed");

                this.PopulateGridViewEmployee();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
            }
        }

        private void btnShowEmployee_Click(object sender, EventArgs e)
        {
            this.PopulateGridViewEmployee();
            this.dgvEmployee.ClearSelection();
        }
        public void ClearEmployee()
        {
            this.txtEmployeeName.Clear();
            this.txtEmployeePassword.Clear();
            this.txtEmployeeSalary.Clear();
            this.txtEmployeeUserName.Clear();
            this.dtpEmployeeJoiningDate.Text = "";
        }

        private void RefreshContentEmployee()
        {
            this.txtEmployeeName.Clear();
            this.txtEmployeePassword.Clear();
            this.txtEmployeeSalary.Clear();
            this.txtEmployeeUserName.Clear();
            this.dtpEmployeeJoiningDate.Text = "";

            this.AutoIdEmployee();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            if (!IsValidToSaveEmployee())
            {
                MessageBox.Show("Please Fill All the Fields", "Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }         
            try
            {
                var sql = "select * from employee_records where e_user_name = '" + this.txtEmployeeUserName.Text + "';";
                DataTable dt = this.Da.ExecuteQueryTable(sql);
                if (dt.Rows.Count >= 1)
                {
                    sql = "update employee_records set e_name ='" + this.txtEmployeeName.Text + "', e_password = '" + (this.txtEmployeePassword.Text) + "', e_joining_date = '" + (this.dtpEmployeeJoiningDate.Text) + "', e_salary="+this.txtEmployeeSalary.Text+" where e_user_name = '" + this.txtEmployeeUserName.Text + "';";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count >= 1)
                    {
                        MessageBox.Show("Data updated successfully");
                        this.PopulateGridViewProduct();
                        this.ClearProduct();
                    }
                    else
                        MessageBox.Show("Data upgradation failed");
                }
                else
                {                   
                    sql = "  insert into employee_records values ('"+ this.txtEmployeeUserName.Text + "','"+ this.txtEmployeeName.Text + "','"+ this.txtEmployeePassword.Text + "','"+ this.dtpEmployeeJoiningDate.Text + "',"+ this.txtEmployeeSalary.Text + ");";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count == 1)
                    {
                        MessageBox.Show("Data insertion successfull");
                        this.PopulateGridViewEmployee();
                        this.ClearEmployee();
                    }
                    else
                    {
                        MessageBox.Show("Data insertion failed");
                        this.ClearEmployee();
                    }
                }
                this.RefreshContentEmployee();
                this.PopulateGridViewEmployee();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
                this.ClearEmployee();
            }
        }

        private void dgvEmployee_DoubleClick(object sender, EventArgs e)
        {
            this.txtEmployeeUserName.Text = this.dgvEmployee.CurrentRow.Cells["e_user_name"].Value.ToString();
            this.txtEmployeeName.Text = this.dgvEmployee.CurrentRow.Cells["e_name"].Value.ToString();
            this.txtEmployeePassword.Text = this.dgvEmployee.CurrentRow.Cells["e_password"].Value.ToString();
            this.txtEmployeeSalary.Text = this.dgvEmployee.CurrentRow.Cells["e_salary"].Value.ToString();
            this.dtpEmployeeJoiningDate.Text = this.dgvEmployee.CurrentRow.Cells["e_joining_date"].Value.ToString();
        }

        private void AutoIdEmployee()
        {
            try
            {
                var sql = "select * from employee_records order by e_user_name desc;";
                var dt = this.Da.ExecuteQueryTable(sql);
                var lastId = dt.Rows[0][0].ToString();
                string[] temp = lastId.Split('-');
                int no = Convert.ToInt32(temp[1]);
                string newId = "E-" + (++no).ToString("d3");
                this.txtEmployeeUserName.Text = newId;
            }
            catch (Exception ex)
            {
                this.txtEmployeeUserName.Text = "E-001";
            }
        }

        private void btnClearEmployee_Click(object sender, EventArgs e)
        {
            this.RefreshContentEmployee();
            this.dgvEmployee.ClearSelection();
        }

        private string ConvertExp(string x)
        {
            string a="";
            switch (x)
            {
                case "UserName":
                    a = "e_user_name";
                    break;
                case "Name":
                    a = "e_name";
                    break;
                case "Password":
                    a = "e_password";
                    break;
                case "Joining Date":
                    a = "e_joining_date";
                    break;
                case "Salary":
                    a = "e_salary";
                    break;
                default:
                    a = "e_user_name";
                    break;
            }
            return a;
        }
        private void btnSearchEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(this.cboSearchEmployee.Text))
                {
                    this.cboSearchEmployee.Text = "e_user_name";
                }
                string sql = "select * from employee_records where " + this.ConvertExp(this.cboSearchEmployee.Text) + " = '" + this.txtSearchEmployee.Text + "';";
                this.PopulateGridViewEmployee(sql);
                this.txtSearchEmployee.Clear();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
            }

            this.txtSearchEmployee.Clear();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormLogin().Show();
        }
    }
}
