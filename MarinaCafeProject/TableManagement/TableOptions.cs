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
    public partial class TableOptions : Form
    {
        CafeArea area = new CafeArea();
        CafeTable table = new CafeTable();
        public int activeSession;

        public int AreaId
        {
            get { return area.AreaId; }
            set { area.AreaId = value; }
        }

        public string AreaName
        {
            get { return area.AreaName; }
            set { area.AreaName = value; }
        }

        public int TableCount
        {
            get { return area.TableCount; }
            set { area.TableCount = value; }
        }

        public int AreaTableId
        {
            get { return table.AreaId; }
            set { table.AreaId = value; }

        }
        public int TableNumber
        {
            get { return table.TableNumber; }
            set { table.TableNumber = value; }
        }

        public int TableType
        {
            get { return table.TableType; }
            set { table.TableType = value; }
        }

        public TableOptions()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TableOptions_Load(object sender, EventArgs e)
        {
            bunifuLabel1.Text = "Table Settings - " + area.AreaName;
            TableUC tableUC = new TableUC();
            tableUC.Id = table.TableNumber;
            tableUC.Type = table.TableType;
            tableUC.Left = 12;
            panel2.Controls.Add(tableUC);

            if (table.TableType == 0)
            {
                btn_open.Text = "START SERVICE";
                btn_move.Enabled = false;
                btn_join.Enabled = false;
                btn_reserved.Enabled = true;
                btn_payment.Enabled = false;
            }
            else if (table.TableType == 1)
            {
                btn_open.Text = "ORDERS";
                btn_move.Enabled = true;
                btn_join.Enabled = true;
                btn_reserved.Enabled = false;
                btn_payment.Enabled = true;
            }
            else if (table.TableType == 2)
            {
                btn_open.Text = "START SERVICE";
                btn_reserved.Text = "CANCEL RESERVATION";
                btn_move.Enabled = true;
                btn_join.Enabled = false;
                btn_reserved.Enabled = true;
                btn_payment.Enabled = false;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //table.TableType = 1;

            CafeTableConfigure tableConfigure = new CafeTableConfigure();
            tableConfigure.table = table;
            tableConfigure.area = area;
            tableConfigure.activeSession = activeSession;
            tableConfigure.ShowDialog();

            area = (CafeArea)tableConfigure.area;
            table = (CafeTable)tableConfigure.table;

            bunifuLabel1.Text = "Table Settings - " + area.AreaName;
            panel2.Controls.Clear();
            TableUC tableUC = new TableUC();
            tableUC.Id = table.TableNumber;
            tableUC.Type = table.TableType;
            tableUC.Left = 12;
            panel2.Controls.Add(tableUC);

            //this.Close();
        }

        private void btn_move_Click(object sender, EventArgs e)
        {
            TableMove tableMove = new TableMove();
            tableMove.activeSession = activeSession;
            tableMove.area = area;
            tableMove.table = table;
            tableMove.ShowDialog();
            this.Close();
        }

        private void btn_reserved_Click(object sender, EventArgs e)
        {
            table.TableType = 3;
        }
    }
}
