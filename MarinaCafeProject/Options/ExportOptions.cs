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
    public partial class ExportOptions : Form
    {
        public int isAllData = -1;
        public ExportOptions()
        {
            InitializeComponent();
        }

        private void ExportOptions_Load(object sender, EventArgs e)
        {

        }

        private void btn_registered_product_Click(object sender, EventArgs e)
        {
            isAllData = 1;
            this.Close();
        }

        private void btn_new_product_Click(object sender, EventArgs e)
        {
            isAllData = 0;
            this.Close();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
