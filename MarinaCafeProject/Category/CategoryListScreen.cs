using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarinaCafeProject
{
    public partial class CategoryListScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public CategoryListScreen()
        {
            InitializeComponent();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CategoryListScreen_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            this.dataGridView1.DoubleBuffered(true);
            get_data();

            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "KATEGORİ ADI";
            dataGridView1.Columns[2].HeaderText = "KATEGORI KISA ADI";

            dataGridView1.Columns[0].Width = 80;

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                this.dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }



            dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            this.dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.Blue;
            this.dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;
        }

        private void get_data()
        {
            try
            {
                DataSet ds = new DataSet();
                BindingSource bs = new BindingSource();
                ds.Clear();
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT * FROM cafe_category ORDER BY cafe_category_id", conn);
                get.Fill(ds, "category");
                conn.Close();
                bs.DataSource = ds.Tables["category"];
                dataGridView1.DataSource = bs;
            }
            catch { }
        }

        private void tb_category_name_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tb_category_name.Text.Trim() == "")
                {
                    get_data();
                }
                else
                {
                    DataSet ds = new DataSet();
                    BindingSource bs = new BindingSource();
                    ds.Clear();
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OleDbDataAdapter get = new OleDbDataAdapter("SELECT * FROM cafe_category WHERE cafe_category_name LIKE '" + tb_category_name.Text + "%' ORDER BY cafe_category_id", conn);
                    get.Fill(ds, "category");
                    conn.Close();
                    bs.DataSource = ds.Tables["category"];
                    dataGridView1.DataSource = bs;

                }
            }
            catch { }
        }

        private void btn_export_pdf_Click(object sender, EventArgs e)
        {
            try
            {
                ExportOptions options = new ExportOptions();
                options.ShowDialog();
                int result = options.isAllData;
                if (result == 1)
                {
                    DataGridView grid = new DataGridView();
                    grid.Name = "temp_grid";
                    grid.Visible = false;

                    grid.AllowUserToAddRows = false;
                    grid.DoubleBuffered(true);
                    this.Controls.Add(grid);

                    grid.DataSource = SetGridData4ExportPdf();

                    grid.Columns[0].HeaderText = "ID";
                    grid.Columns[1].HeaderText = "KATEGORİ ADI";
                    grid.Columns[2].HeaderText = "KATEGORI KISA ADI";

                    grid.Columns[0].Width = 80;

                    for (int i = 0; i < grid.ColumnCount; i++)
                    {
                        grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        grid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    }



                    dataGridView1.EnableHeadersVisualStyles = false;
                    this.dataGridView1.ColumnHeadersHeight = 40;


                    lbl_loading.Visible = true;
                    string fileName = "MarinaCafe-CategoryList-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                    ExportGridToPdf(grid, fileName);
                    this.Controls.Remove(grid);
                    lbl_loading.Visible = false;
                }
                else if (result == 0)
                {
                    lbl_loading.Visible = true;
                    string fileName = "MarinaCafe-CategoryListSpecific-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                    ExportGridToPdf(dataGridView1, fileName);
                    lbl_loading.Visible = false;

                }

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                MessageBox.Show("Aktarma hatası!");
            }
        }
        public void ExportGridToPdf(DataGridView grid, string filename)
        {
            BaseFont baseFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.EMBEDDED);
            PdfPTable pdfPTable = new PdfPTable(grid.Columns.Count);
            pdfPTable.DefaultCell.Padding = 3;
            pdfPTable.WidthPercentage = 100;
            pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfPTable.DefaultCell.BorderWidth = 1;

            iTextSharp.text.Font text = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.NORMAL);

            foreach (DataGridViewColumn column in grid.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, text));
                cell.BackgroundColor = new iTextSharp.text.BaseColor(240, 240, 240);
                pdfPTable.AddCell(cell);
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdfPTable.AddCell(new Phrase(cell.Value.ToString(), text));
                }
            }

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = filename;
            saveFileDialog.DefaultExt = ".pdf";
            string pdfLocation = "";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    pdfLocation = saveFileDialog.FileName;
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    pdfDoc.Add(pdfPTable);
                    pdfDoc.Close();
                    stream.Close();
                }
                MessageBox.Show("PDF dışarı aktarıldı.");
                System.Diagnostics.Process.Start(pdfLocation);
            }

        }

        private BindingSource SetGridData4ExportPdf()
        {
            BindingSource bs = new BindingSource();
            try
            {
                DataSet ds = new DataSet();

                if (conn.State == ConnectionState.Closed) conn.Open();

                ds.Clear();
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT * FROM cafe_category ORDER BY cafe_category_id", conn);

                get.Fill(ds, "category");
                conn.Close();
                bs.DataSource = ds.Tables["category"];

            }
            finally
            {
                conn.Close();
            }
            return bs;
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                CategoryEditOptions categoryEdit = new CategoryEditOptions();
                categoryEdit.category_id = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                categoryEdit.ShowDialog();
                get_data();
            }
        }
    }
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
