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
    public partial class CategoryEditOptions : Form
    {
        public int category_id = 0;
        public CategoryEditOptions()
        {
            InitializeComponent();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            CategoryCreateScreen categoryCreate = new CategoryCreateScreen();
            categoryCreate.isUpdate = true;
            categoryCreate.category_id = category_id;
            categoryCreate.ShowDialog();
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            CategoryDeleteScreen categoryDelete = new CategoryDeleteScreen();
            categoryDelete.categoryId = category_id;
            categoryDelete.ShowDialog();
            this.Close();
        }
    }
}
