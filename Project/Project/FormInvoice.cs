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
    public partial class FormInvoice : Form
    {
        private double receivedMoney;
        private double discount;
        private double total;
        private DataAccess Da { get; set; }
        public double Total
        {
            get;
            set;
        }

        public double ReceivedMoney
        {
            get { return this.receivedMoney; }
            set { this.receivedMoney = value; }
        }
        public double Discount
        {
            get { return this.discount; }
            set { this.discount = value; }
        }
        public FormInvoice(double receivedMoney, double discount,double total)
        {
            InitializeComponent();
            this.Da = new DataAccess();

            this.ReceivedMoney = receivedMoney;
            this.Discount = discount;
            this.Total = total;

            this.PopulateGridViewInvoice();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // code
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            new FormEmployee().Show();
        }
        private void PopulateGridViewInvoice(string sql = "select * from inventory;")
        {
            var ds = this.Da.ExecuteQuery(sql);

            this.dgvInvoice.AutoGenerateColumns = false;
            this.dgvInvoice.DataSource = ds.Tables[0];
        }
        private double TotalAmount(double ttl, double dis)
        {
            double tmp = (dis / 100)*ttl;
            return (ttl-tmp);
        }
        private void FormInvoice_Load(object sender, EventArgs e)
        {
            double temp = this.TotalAmount(this.Total, this.Discount);
            this.txtSubTotal.Text = this.Total.ToString();
            this.txtPaid.Text = this.ReceivedMoney.ToString();
            this.txtDiscount.Text = this.Discount.ToString();
            this.txtTotal.Text = (temp).ToString();
            this.txtDue.Text = (temp- this.ReceivedMoney).ToString();

        }
    }
}
