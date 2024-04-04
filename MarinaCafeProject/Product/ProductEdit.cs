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
    public partial class ProductEdit : Form
    {
        public int productId;
        public ProductEdit()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_edit_product_Click(object sender, EventArgs e)
        {
            ProductCreateScreen productEdit = new ProductCreateScreen();
            productEdit.isUpdate = true;
            productEdit.productId = productId;
            productEdit.ShowDialog();
            this.Close();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            ProductDeleteScreen productDelete = new ProductDeleteScreen();
            productDelete.product_id = productId;
            productDelete.ShowDialog();
            this.Close();
        }
    }
}
