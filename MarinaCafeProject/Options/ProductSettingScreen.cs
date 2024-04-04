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
    public partial class ProductSettingScreen : Form
    {
        public ProductSettingScreen()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_category_Click(object sender, EventArgs e)
        {
            CategoryOptions categoryOptions = new CategoryOptions();
            categoryOptions.ShowDialog();
        }

        private void btn_product_Click(object sender, EventArgs e)
        {
            ProductOptions productOptions = new ProductOptions();
            productOptions.ShowDialog();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.BackgroundImage = Properties.Resources.category_hover;
            button1.BackColor = Color.FromArgb(61, 106, 105);
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.BackgroundImage = Properties.Resources.category;
            button1.BackColor = Color.White;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources.product_hover;
            button2.BackColor = Color.FromArgb(61, 106, 105);
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.BackgroundImage = Properties.Resources.product;
            button2.BackColor = Color.White;
        }
    }
}
