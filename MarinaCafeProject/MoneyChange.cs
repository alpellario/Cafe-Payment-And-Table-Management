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
    public partial class MoneyChange : Form
    {
        public double enteredAmount;
        public MoneyChange()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_received_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void tb_received_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tb_received.Text))
                {
                    if (double.Parse(tb_received.Text) >= enteredAmount)
                    {
                        double result = double.Parse(tb_received.Text) - enteredAmount;
                        tb_result.Text = result.ToString() + " ₺";
                    }
                    else tb_result.Clear();
                }
            }
            catch
            {

            }
        }
    }
}
