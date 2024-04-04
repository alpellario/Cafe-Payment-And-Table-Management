using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarinaCafeProject
{
    public partial class ProductOptions : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public ProductOptions()
        {
            InitializeComponent();
        }

        private void btn_create_product_MouseEnter(object sender, EventArgs e)
        {
            btn_create_product.BackgroundImage = Properties.Resources.create_product_hover;
            btn_create_product.BackColor = Color.FromArgb(61, 106, 105);
        }

        private void btn_create_product_MouseLeave(object sender, EventArgs e)
        {
            btn_create_product.BackgroundImage = Properties.Resources.create_product;
            btn_create_product.BackColor = Color.White;
        }

        private void btn_product_list_MouseEnter(object sender, EventArgs e)
        {
            btn_product_list.BackgroundImage = Properties.Resources.product_list_hover;
            btn_product_list.BackColor = Color.FromArgb(61, 106, 105);
        }

        private void btn_product_list_MouseLeave(object sender, EventArgs e)
        {
            btn_product_list.BackgroundImage = Properties.Resources.product_list;
            btn_product_list.BackColor = Color.White;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_create_product_Click(object sender, EventArgs e)
        {

            if (conn.State == ConnectionState.Closed) conn.Open();
            OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM cafe_category", conn);
            int catCount = (int)command.ExecuteScalar();
            conn.Close();
            if (catCount > 0)
            {
                ProductCreateScreen productCreateScreen = new ProductCreateScreen();
                productCreateScreen.ShowDialog();

            }
            else MessageBox.Show("Kategori bulunamadı. Lüften ürün oluşturmadan önce kategori oluşturunuz.");
            
        }

        private void btn_product_list_Click(object sender, EventArgs e)
        {
            ProductListScreen list = new ProductListScreen();
            list.ShowDialog();
        }
    }
}
