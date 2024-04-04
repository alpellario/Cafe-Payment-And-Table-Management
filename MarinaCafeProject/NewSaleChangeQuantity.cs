using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarinaCafeProject
{

    public partial class NewSaleChangeQuantity : Form
    {
        public int quantity;
        public int newQuantity;
        public bool isPartPayment = false;
        public int maxValue;
        public NewSaleChangeQuantity()
        {
            InitializeComponent();
        }

        private void bunifuLabel1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void bunifuLabel1_Validated(object sender, EventArgs e)
        {
            if (num.Text == "")
            {
                num.Text = "0";
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            newQuantity = quantity;
            this.Close();
        }

        private void NewSaleChangeQuantity_Load(object sender, EventArgs e)
        {
            num.Value = quantity;

            if (isPartPayment)
            {
                num.Maximum = maxValue;
            }
        }

        private void btn_save_amount_Click(object sender, EventArgs e)
        {
            newQuantity = Convert.ToInt16(num.Value);
            this.Close();
        }
    }
}
