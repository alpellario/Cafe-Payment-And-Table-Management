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
    public partial class TableUC : UserControl
    {
        public TableUC()
        {
            InitializeComponent();
        }

        private int _type;
        private int _id;
        private int _areaId;

        [Category("Custom Props")]
        public int Type
        {
            get { return _type; }
            set
            {
                _type = value;
                if (value == 0)
                {
                    this.BackgroundImage = Properties.Resources.table_emty;
                }
                else if (value == 1)
                {
                    this.BackgroundImage = Properties.Resources.table_full;
                }
                else if (value == 2)
                {
                    this.BackgroundImage = Properties.Resources.table_reserved;
                }
            }
        }

        [Category("Custom Props")]

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.Name = value.ToString();
                lbl_number.Text = value.ToString();
                lbl_number.Name = value.ToString();
            }
        }

        [Category("Custom Props")]

        public int AreaId
        {
            get { return _areaId; }
            set { _areaId = value; }
        }
    }
}
