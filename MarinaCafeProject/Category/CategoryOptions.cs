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
    public partial class CategoryOptions : Form
    {
        public CategoryOptions()
        {
            InitializeComponent();
        }

        private void btn_create_category_MouseEnter(object sender, EventArgs e)
        {
            btn_create_category.BackgroundImage = Properties.Resources.create_category_hover;
            btn_create_category.BackColor = Color.FromArgb(61, 106, 105);
        }

        private void btn_create_category_MouseLeave(object sender, EventArgs e)
        {
            btn_create_category.BackgroundImage = Properties.Resources.create_category;
            btn_create_category.BackColor = Color.White;
        }

        private void btn_category_list_MouseEnter(object sender, EventArgs e)
        {
            btn_category_list.BackgroundImage = Properties.Resources.category_list_hover;
            btn_category_list.BackColor = Color.FromArgb(61, 106, 105);
        }

        private void btn_category_list_MouseLeave(object sender, EventArgs e)
        {
            btn_category_list.BackgroundImage = Properties.Resources.category_list;
            btn_category_list.BackColor = Color.White;
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_create_category_Click(object sender, EventArgs e)
        {
            CategoryCreateScreen categoryCreateScreen = new CategoryCreateScreen();
            categoryCreateScreen.ShowDialog();
        }

        private void btn_category_list_Click(object sender, EventArgs e)
        {
            CategoryListScreen categoryListScreen = new CategoryListScreen();
            categoryListScreen.ShowDialog();
        }
    }
}
