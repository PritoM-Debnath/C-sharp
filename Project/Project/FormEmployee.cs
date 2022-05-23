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
    public partial class FormEmployee : Form
    {
        private DataAccess Da { get; set; }
        private double total=0;

        public double Total
        {
            get { return total; }
            set { total = value; }
        }
        public FormEmployee()
        {
            InitializeComponent();
            this.Da = new DataAccess();

            this.PopulateGridViewProduct();

            this.dgvProduct.ClearSelection();
            this.Da.ExecuteQuery("delete from inventory;");

        }

        private void FormEmployee_Load(object sender, EventArgs e)
        {
            this.dgvProduct.ClearSelection();
        }
        private void PopulateGridViewProduct(string sql = "select * from Product;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvProduct.AutoGenerateColumns = false;
            this.dgvProduct.DataSource = ds.Tables[0];
        }

        private void dgvProduct_DoubleClick(object sender, EventArgs e)
        {
            this.txtProductId.Text = this.dgvProduct.CurrentRow.Cells["ID"].Value.ToString();
            this.txtProductName.Text = this.dgvProduct.CurrentRow.Cells["Name"].Value.ToString();
            this.txtProductPrice.Text = this.dgvProduct.CurrentRow.Cells["Price"].Value.ToString();
            this.txtProductQuantity.Text = this.dgvProduct.CurrentRow.Cells["Quantity"].Value.ToString();
            this.txtQMX.Text = this.dgvProduct.CurrentRow.Cells["Quantity"].Value.ToString();
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(this.cboSearchProduct.Text))
                {
                    this.cboSearchProduct.Text = "Id";
                }
                string sql = "select * from product where " + this.cboSearchProduct.Text + " = '" + this.txtSearchProduct.Text + "';";
                this.PopulateGridViewProduct(sql);
                this.txtSearchProduct.Clear();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
            }

            this.txtSearchProduct.Clear();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.PopulateGridViewProduct();
        }


        //Invoice
        public void ClearProduct()
        {
            this.txtProductName.Clear();
            this.txtProductPrice.Clear();
            this.txtProductQuantity.Clear();
            this.txtProductId.Clear();

            this.dgvProduct.ClearSelection();

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
        private void PopulateGridViewInvoice(string sql = "select * from inventory;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvInvoice.AutoGenerateColumns = false;
            this.dgvInvoice.DataSource = ds.Tables[0];
        }
        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            if (!IsValidToSaveProduct())
            {
                MessageBox.Show("Please Fill All the Fields", "Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            try
            { 
                var sql = "select * from inventory where Product_Id = '" + this.txtProductId.Text + "';";
                DataTable dt = this.Da.ExecuteQueryTable(sql);
                double total = (Convert.ToDouble(this.txtProductPrice.Text)) * (Convert.ToDouble(this.txtProductQuantity.Text));
                if (dt.Rows.Count >= 1)
                {
                    // subtotal ***                  
                    sql = "update inventory set Product_Name ='" + this.txtProductName.Text + "', Product_Price = " + (this.txtProductPrice.Text) + ", Product_Quantity = " + (this.txtProductQuantity.Text) + ", Sub_Total = " + Convert.ToString(total) + " where Product_Id = '" + this.txtProductId.Text + "';";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count >= 1)
                    {
                        int val = Convert.ToInt32(this.txtQMX.Text)- Convert.ToInt32(this.txtProductQuantity.Text);
                        sql = "update product set quantity=" + val + " where id='" + this.txtProductId.Text + "'";
                        int count1 = this.Da.ExecuteDMLQuery(sql);

                        if (count1 < 1)
                        {
                            MessageBox.Show("Failed to update quantity information");
                        }

                        MessageBox.Show("Updated the cart");
                        this.PopulateGridViewInvoice();
                        this.ClearProduct();
                    }
                    else
                        MessageBox.Show("Failed to update the cart");
                }
                else
                {
                    sql = "insert into inventory values('" + this.txtProductId.Text + "','" + this.txtProductName.Text + "'," + this.txtProductPrice.Text + "," + this.txtProductQuantity.Text + "," + Convert.ToString(total) + ");";
                    int count = this.Da.ExecuteDMLQuery(sql);

                    if (count == 1)
                    {
                        int val = Convert.ToInt32(this.txtQMX.Text) - Convert.ToInt32(this.txtProductQuantity.Text);
                        sql = "update product set quantity=" + val + " where id='" + this.txtProductId.Text + "'";
                        int count1 = this.Da.ExecuteDMLQuery(sql);

                        if (count1 < 1)
                        {
                            MessageBox.Show("Failed to update quantity information");
                        }
                        MessageBox.Show("Added to cart");
                        this.PopulateGridViewInvoice();
                        this.ClearProduct();
                    }
                    else
                    {
                        MessageBox.Show("Failed to add in the cart");
                        this.ClearProduct();
                    }
                }
                this.ClearProduct();
                this.PopulateGridViewInvoice();
            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured: " + exc.Message);
                this.ClearProduct();
            }
        }

        private void btnClearProduct_Click(object sender, EventArgs e)
        {
            this.ClearProduct();
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            if (this.dgvInvoice.SelectedRows.Count < 1)
            {
                MessageBox.Show("Please select a row first from the cart to delete", "Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                return;
            }
            try
            {
                var id = this.dgvInvoice.CurrentRow.Cells[0].Value.ToString();
                var title = this.dgvInvoice.CurrentRow.Cells[1].Value.ToString();

                var sql = "delete from inventory where Product_Id = '" + id + "';";
                var rowCount = this.Da.ExecuteDMLQuery(sql);

                if (rowCount == 1)
                {
                    MessageBox.Show(title + " has been deleetd from the Cart.");
                }
                else
                {
                    MessageBox.Show("Data deletion operation failed.");
                }

                this.PopulateGridViewInvoice();

            }
            catch (Exception exc)
            {
                MessageBox.Show("An error has occured, please try again later. Error msg: " + exc.Message);
            }
        }

        private void dgvInvoice_DoubleClick(object sender, EventArgs e)
        {
            this.txtProductId.Text = this.dgvInvoice.CurrentRow.Cells["Product_Id"].Value.ToString();
            this.txtProductName.Text = this.dgvInvoice.CurrentRow.Cells["Product_Name"].Value.ToString();
            this.txtProductPrice.Text = this.dgvInvoice.CurrentRow.Cells["Product_Price"].Value.ToString();
            this.txtProductQuantity.Text = this.dgvInvoice.CurrentRow.Cells["Product_Quantity"].Value.ToString();          
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtReceivedMoney.Text))
            {
                MessageBox.Show("Fill Received Money field", "Info", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            if (String.IsNullOrEmpty(this.txtDiscount.Text))
            {
                this.txtDiscount.Text = "0";
            }


            for (int count=0; count< dgvInvoice.Rows.Count; count++)
            {
                this.Total += Convert.ToInt32(this.dgvInvoice.Rows[count].Cells[4].Value.ToString());
            }
            //this.txtProductId.Text = Convert.ToString(Total);
           
            double receivedMoney = Convert.ToDouble(this.txtReceivedMoney.Text);          
            double discount = Convert.ToDouble(this.txtDiscount.Text);

            this.Hide();
            new FormInvoice(receivedMoney,discount,Total).Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormLogin().Show();
        }
    }
}
