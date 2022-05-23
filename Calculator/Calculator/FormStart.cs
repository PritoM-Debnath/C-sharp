namespace Calculator
{
    public partial class FormStart : Form
    {
        public FormStart()
        {
            InitializeComponent();
        }

        internal double temp=0,value=0;
        internal string valueOp = "";
        internal bool checkOp = false;

        private void clickBtn(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(txtBoxBig.Text == "0" || checkOp)
            {
                txtBoxBig.Clear();
                txtBoxSmall.Clear();
            }
            string ss= button.Text;
            txtBoxBig.Text += button.Text;
            txtBoxSmall.Text +=button.Text;
            temp = Double.Parse(txtBoxBig.Text);
        }

        private void clickOperation(object sender, EventArgs e)
        {
            Button operationButton = (Button)sender;
            value = Double.Parse(txtBoxBig.Text);
            valueOp = operationButton.Text;
            checkOp = true;
            txtBoxSmall.Text +=" " + valueOp;
        }

        private void btnequal_Click(object sender, EventArgs e)
        {
            txtBoxSmall.Clear();
            switch (valueOp)
            {
                case "+":
                    txtBoxBig.Text = (value + temp).ToString();      
                    break;
                case "-":
                    txtBoxBig.Text = (value - temp).ToString();
                    break;
                case "*":
                    txtBoxBig.Text = (value * temp).ToString();
                    break;
                case "/":
                    txtBoxBig.Text = (value / temp).ToString();
                    break;
                default:
                    txtBoxBig.Text = "ERROR";
                    break;
            }
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            txtBoxBig.Clear();
            txtBoxSmall.Clear();
        }
    }
}