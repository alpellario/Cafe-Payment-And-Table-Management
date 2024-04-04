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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarinaCafeProject
{
    public partial class ProductListScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public int categoryId = 0;
        public int selectedProductId = 0;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        public ProductListScreen()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductListScreen_Load(object sender, EventArgs e)
        {
            cb_category.Enabled = false;
            checkBox1.Checked = false;

            GetCategories();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            this.dataGridView1.DoubleBuffered(true);

            CalculateTotalPages();
            this.dataGridView1.DataSource = GetCurrentRecords(1);

            dataGridView1.Columns[0].HeaderText = "ÜRÜN ID";
            dataGridView1.Columns[1].HeaderText = "ÜRÜN KODU";
            dataGridView1.Columns[2].HeaderText = "ÜRÜN ADI";
            dataGridView1.Columns[3].HeaderText = "KATEGORİ ADI";
            dataGridView1.Columns[4].HeaderText = "ÜRÜN FİYATI(₺)";

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
        private void GetCategories()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataTable dt = new DataTable();
                OleDbDataAdapter categories = new OleDbDataAdapter("SELECT * FROM cafe_category", conn);
                categories.Fill(dt);
                cb_category.ValueMember = "cafe_category_id";
                cb_category.DisplayMember = "cafe_category_name";
                cb_category.DataSource = dt;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private int TotalPage = 0;
        private int CurrentPageIndex = 1;
        private int PgSize = 200;
        private void CalculateTotalPages()
        {
            try
            {
                int rowCount = 0;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM cafe_product", conn);
                rowCount = (int)command.ExecuteScalar();

                lbl_row_count.Text = "Toplam ürün sayısı: " + rowCount;
                TotalPage = rowCount / PgSize;
                if (rowCount % PgSize > 0)
                    TotalPage += 1;

                lbl_page.Text = "Toplam sayfa sayısı : " + TotalPage;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private BindingSource GetCurrentRecords(int page)
        {
            BindingSource bs = new BindingSource();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                int page_coef = 200 * page;
                lbl_page_number.Text = "Page : " + page;

                DataSet ds = new DataSet();

                OleDbDataAdapter get = new OleDbDataAdapter("SELECT CP.cafe_product_id, CP.cafe_product_code, CP.cafe_product_name, CC.cafe_category_name, CP.cafe_product_price " +
                                                            "FROM cafe_product AS CP INNER JOIN cafe_category AS CC ON CC.cafe_category_id=CP.cafe_category_id " +
                                                            "WHERE CP.cafe_product_id IN " +
                                                                "(SELECT TOP 200 A.cafe_product_id FROM " +
                                                                    "[SELECT TOP " + page_coef + " CP.cafe_product_id, CP.cafe_product_code, CP.cafe_product_name, CC.cafe_category_name, CP.cafe_product_price " +
                                                                    "FROM cafe_product AS CP INNER JOIN cafe_category AS CC ON CC.cafe_category_id=CP.cafe_category_id " +
                                                                    "ORDER BY CP.cafe_product_id DESC]. AS A " +
                                                                "ORDER BY A.cafe_product_id ASC) " +
                                                           "ORDER BY CP.cafe_product_id DESC", conn);

                get.Fill(ds, "cafe_product");
                conn.Close();
                bs.DataSource = ds.Tables["cafe_product"];

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }

            return bs;
        }

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex < this.TotalPage)
            {
                this.CurrentPageIndex++;
                this.dataGridView1.DataSource = GetCurrentRecords(this.CurrentPageIndex);
            }
        }

        private void btn_first_Click(object sender, EventArgs e)
        {
            this.CurrentPageIndex = 1;
            this.dataGridView1.DataSource = GetCurrentRecords(this.CurrentPageIndex);
        }

        private void btn_prev_Click(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex > 1)
            {
                this.CurrentPageIndex--;
                this.dataGridView1.DataSource = GetCurrentRecords(this.CurrentPageIndex);
            }
        }

        private void btn_last_Click(object sender, EventArgs e)
        {
            this.CurrentPageIndex = TotalPage;
            this.dataGridView1.DataSource = GetCurrentRecords(this.CurrentPageIndex);
        }

        private void btn_export_pdf_Click(object sender, EventArgs e)
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

                grid.Columns[0].HeaderText = "ÜRÜN ID";
                grid.Columns[1].HeaderText = "ÜRÜN KODU";
                grid.Columns[2].HeaderText = "ÜRÜN ADI";
                grid.Columns[3].HeaderText = "KATEGORİ ADI";
                grid.Columns[4].HeaderText = "ÜRÜN FİYATI(₺)";

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }


                grid.Columns[0].Width = 80;
                grid.Columns[1].Width = 140;
                grid.Columns[2].Width = 300;

                grid.Columns[0].Width = 80;

                grid.ColumnHeadersHeight = 40;


                lbl_loading.Visible = true;
                string fileName = "MarinaCafe-CafeProductList-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                ExportGridToPdf(grid, fileName);
                this.Controls.Remove(grid);
                lbl_loading.Visible = false;
            }
            else if (result == 0)
            {
                lbl_loading.Visible = true;
                string fileName = "JinnMarinaCafeJoy-ProductList-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                ExportGridToPdf(dataGridView1, fileName);
                lbl_loading.Visible = false;

            }
        }
        private object SetGridData4ExportPdf()
        {
            BindingSource bs = new BindingSource();
            try
            {

                DataSet ds = new DataSet();

                if (conn.State == ConnectionState.Closed) conn.Open();

                OleDbDataAdapter get = new OleDbDataAdapter("SELECT CP.cafe_product_id, CP.cafe_product_code, CP.cafe_product_name, CC.cafe_category_name, CP.cafe_product_price " +
                                                            "FROM cafe_product AS CP INNER JOIN cafe_category AS CC ON CC.cafe_category_id=CP.cafe_category_id " +
                                                            "ORDER BY CP.cafe_product_id", conn);


                get.Fill(ds, "cafe_product");
                conn.Close();
                bs.DataSource = ds.Tables["cafe_product"];
            }
            finally
            {
                conn.Close();
            }
            return bs;

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
                MessageBox.Show("PDF is exported.");
                System.Diagnostics.Process.Start(pdfLocation);
            }

        }

        private void tb_search_code_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tb_search_code.Text.Trim() == "")
                {
                    this.dataGridView1.DataSource = GetCurrentRecords(1);
                    btn_first.Enabled = true;
                    btn_next.Enabled = true;
                    btn_prev.Enabled = true;
                    btn_last.Enabled = true;
                }
                else
                {
                    checkBox1.Checked = false;
                    btn_first.Enabled = false;
                    btn_next.Enabled = false;
                    btn_prev.Enabled = false;
                    btn_last.Enabled = false;

                    DataSet ds = new DataSet();
                    BindingSource bs = new BindingSource();
                    ds.Clear();
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OleDbDataAdapter get = new OleDbDataAdapter("SELECT CP.cafe_product_id, CP.cafe_product_code, CP.cafe_product_name, CC.cafe_category_name, CP.cafe_product_price " +
                                                            "FROM cafe_product AS CP INNER JOIN cafe_category AS CC ON CC.cafe_category_id=CP.cafe_category_id " +
                                                            "WHERE CP.cafe_product_code LIKE '" + tb_search_code.Text + "%' " +
                                                           "ORDER BY CP.cafe_product_id DESC", conn);
                    get.Fill(ds, "product");
                    conn.Close();
                    bs.DataSource = ds.Tables["product"];
                    dataGridView1.DataSource = bs;

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void tb_search_product_name_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tb_search_product_name.Text.Trim() == "")
                {
                    this.dataGridView1.DataSource = GetCurrentRecords(1);
                    btn_first.Enabled = true;
                    btn_next.Enabled = true;
                    btn_prev.Enabled = true;
                    btn_last.Enabled = true;
                }
                else
                {
                    checkBox1.Checked = false;
                    btn_first.Enabled = false;
                    btn_next.Enabled = false;
                    btn_prev.Enabled = false;
                    btn_last.Enabled = false;

                    DataSet ds = new DataSet();
                    BindingSource bs = new BindingSource();
                    ds.Clear();
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OleDbDataAdapter get = new OleDbDataAdapter("SELECT CP.cafe_product_id, CP.cafe_product_code, CP.cafe_product_name, CC.cafe_category_name, CP.cafe_product_price " +
                                                            "FROM cafe_product AS CP INNER JOIN cafe_category AS CC ON CC.cafe_category_id=CP.cafe_category_id " +
                                                            "WHERE CP.cafe_product_name LIKE '%" + tb_search_product_name.Text + "%' " +
                                                           "ORDER BY CP.cafe_product_id DESC", conn);
                    get.Fill(ds, "product");
                    conn.Close();
                    bs.DataSource = ds.Tables["product"];
                    dataGridView1.DataSource = bs;

                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                if (checkBox1.Checked)
                {
                    tb_search_code.Clear();
                    tb_search_product_name.Clear();
                    cb_category.Enabled = true;
                    cb_category.SelectedIndex = 0;
                    Get4CategoryData();
                    btn_first.Enabled = false;
                    btn_next.Enabled = false;
                    btn_prev.Enabled = false;
                    btn_last.Enabled = false;


                }
                else
                {
                    btn_first.Enabled = true;
                    btn_next.Enabled = true;
                    btn_prev.Enabled = true;
                    btn_last.Enabled = true;
                    cb_category.Enabled = false;
                    CalculateTotalPages();
                    this.dataGridView1.DataSource = GetCurrentRecords(1);
                }
            }
            
        }

        private void cb_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Get4CategoryData();
            }
        }

        private void Get4CategoryData()
        {
            BindingSource bs = new BindingSource();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataSet ds = new DataSet();

                OleDbDataAdapter get = new OleDbDataAdapter("SELECT CP.cafe_product_id, CP.cafe_product_code, CP.cafe_product_name, CC.cafe_category_name, CP.cafe_product_price " +
                                                            "FROM cafe_product AS CP INNER JOIN cafe_category AS CC ON CC.cafe_category_id=CP.cafe_category_id " +
                                                            "WHERE CP.cafe_category_id=@cafe_category_id " +
                                                           "ORDER BY CP.cafe_product_id DESC", conn);
                get.SelectCommand.Parameters.AddWithValue("@cafe_category_id", cb_category.SelectedValue);

                get.Fill(ds, "cafe_product");
                conn.Close();
                bs.DataSource = ds.Tables["cafe_product"];
                dataGridView1.DataSource = bs;

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                selectedProductId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                int selectedRow = dataGridView1.CurrentCell.RowIndex;
                ProductEdit options = new ProductEdit();
                options.productId = selectedProductId;
                options.ShowDialog();

                this.dataGridView1.DataSource = GetCurrentRecords(CurrentPageIndex);

                if (dataGridView1.Rows.Count > 3)
                {
                    int diffrence = dataGridView1.Rows.Count - selectedRow;
                    if (dataGridView1.Rows.Count - selectedRow > 9)
                        this.dataGridView1.CurrentCell = dataGridView1.Rows[selectedRow + 9].Cells[0];
                    else this.dataGridView1.CurrentCell = dataGridView1.Rows[selectedRow + diffrence - 1].Cells[0];
                    this.dataGridView1.CurrentCell = dataGridView1.Rows[selectedRow].Cells[0];
                }


            }
        }
    }
}
