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
    public partial class CafeTableList : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public CafeTableList()
        {
            InitializeComponent();
        }

        private void CafeTableList_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            this.dataGridView1.DoubleBuffered(true);

            GetAreas();


            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Alan Adi";
            dataGridView1.Columns[2].HeaderText = "Masa Sayısı";


            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[2].Width = 150;

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                this.dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }

            dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            this.dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(54, 35, 35);
            this.dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            this.dataGridView1.DefaultCellStyle.Font = new Font("Calibri", 15);
        }

        private void GetAreas()
        {
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();

                BindingSource bindingSource = new BindingSource();
                DataSet ds = new DataSet();
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT area_id, area_name, area_table_count FROM cafe_table_area ORDER BY area_id", conn);
                get.Fill(ds, "areas");
                bindingSource.DataSource = ds.Tables["areas"];
                dataGridView1.DataSource = bindingSource;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            CafeCreateTableArea createTable = new CafeCreateTableArea();
            createTable.ShowDialog();
            GetAreas();
        }

        private void btn_add_MouseDown(object sender, MouseEventArgs e)
        {
            btn_add.BackgroundImage = Properties.Resources.add_table_hover;
        }

        private void btn_add_MouseUp(object sender, MouseEventArgs e)
        {
            btn_add.BackgroundImage = Properties.Resources.add_table;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int selectedId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                CafeCreateTableArea update = new CafeCreateTableArea();
                update.isUpdate = true;
                update.areadId = selectedId;
                update.ShowDialog();
                GetAreas();
            }
        }
    }
}
