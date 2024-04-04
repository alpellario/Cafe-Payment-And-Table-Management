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
    public partial class SaleProduct : UserControl
    {
        public SaleProduct()
        {
            InitializeComponent();
        }
        private Image _icon;
        private string _name;
        private int _id;

        [Category("Custom Props")]

        public Image ProductImage
        {
            get { return _icon; }
            set { _icon = value; pictureBox1.Image = value; }
        }

        [Category("Custom Props")]

        public string ProductName
        {
            get { return _name; }
            set { _name = value; lbl_name.Text = value; lbl_name.Name = value; }
        }

        [Category("Custom Props")]

        public int Id
        {
            get { return _id; }
            set { _id = value; this.Name = _id.ToString(); }
        }

        public Bunifu.UI.WinForms.BunifuLabel Label
        {
            get { return lbl_name; }
            set { lbl_name = value; }
        }

        private void SaleProduct_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(252, 221, 130);
        }

        private void SaleProduct_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
        }

        private void SaleProduct_MouseDown(object sender, MouseEventArgs e)
        {
            this.BackColor = Color.FromArgb(253, 235, 181);
        }

        private void SaleProduct_MouseUp(object sender, MouseEventArgs e)
        {
            this.BackColor = Color.FromArgb(252, 221, 130);
        }
    }
}
