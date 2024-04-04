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
    public partial class CafeSaleHistoryScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public CafeSaleHistoryScreen()
        {
            InitializeComponent();
        }

        private void CafeSaleHistoryScreen_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;


            this.dataGridView1.DoubleBuffered(true);

            CalculateTotalPages();
            this.dataGridView1.DataSource = GetCurrentRecords(1);

            dataGridView1.Columns[0].HeaderText = "OTURUM ID";
            dataGridView1.Columns[1].HeaderText = "OTURUM BAŞLANGIÇ TARİHİ";

            dataGridView1.Columns[0].Width = 111;


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

        private int TotalPage = 0;
        private int CurrentPageIndex = 1;
        private int PgSize = 200;
        private void CalculateTotalPages()
        {
            try
            {
                int rowCount;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM sessions", conn);
                rowCount = (int)command.ExecuteScalar();

                lbl_row_count.Text = "Toplam oturum sayısı: " + rowCount;
                TotalPage = rowCount / PgSize;
                if (rowCount % PgSize > 0)
                    TotalPage += 1;

                lbl_page.Text = "Sayfa sayısı : " + TotalPage;
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
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT session_id, session_start_date " +
                                                            "FROM sessions " +
                                                            "WHERE session_id IN " +
                                                                "(SELECT TOP 200 A.session_id FROM " +
                                                                    "[SELECT TOP " + page_coef + " session_id, session_start_date " +
                                                                    "FROM sessions " +
                                                                    "ORDER BY session_id DESC]. AS A " +
                                                                "ORDER BY A.session_id ASC) " +
                                                            "ORDER BY session_id DESC", conn);
                get.Fill(ds, "sessions");
                bs.DataSource = ds.Tables["sessions"];
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

        private void btn_next_Click(object sender, EventArgs e)
        {
            if (this.CurrentPageIndex < this.TotalPage)
            {
                this.CurrentPageIndex++;
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

                grid.Columns[0].HeaderText = "OTURUM ID";
                grid.Columns[1].HeaderText = "OTURUM BAŞLANGIÇ TARİHİ";

                grid.Columns[0].Width = 111;


                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }


                grid.ColumnHeadersHeight = 40;


                lbl_loading.Visible = true;
                string fileName = "MarinaCafe-SessionList-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                ExportGridToPdf(grid, fileName);
                this.Controls.Remove(grid);
                lbl_loading.Visible = false;
            }
            else if (result == 0)
            {
                lbl_loading.Visible = true;
                string fileName = "MarinaCafe-SessionList-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                ExportGridToPdf(dataGridView1, fileName);
                lbl_loading.Visible = false;

            }
        }

        private object SetGridData4ExportPdf()
        {
            BindingSource bs = new BindingSource();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataSet ds = new DataSet();
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT session_id, session_start_date FROM sessions ORDER BY session_id DESC", conn);
                get.Fill(ds, "sessions");
                bs.DataSource = ds.Tables["sessions"];
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
                MessageBox.Show("PDF aktarıldı.");
                System.Diagnostics.Process.Start(pdfLocation);
            }

        }

        private void tb_search_date_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tb_search_date.Text.Trim() == "")
                {
                    this.dataGridView1.DataSource = GetCurrentRecords(1);
                    btn_first.Enabled = true;
                    btn_next.Enabled = true;
                    btn_prev.Enabled = true;
                    btn_last.Enabled = true;
                }
                else
                {
                    btn_first.Enabled = false;
                    btn_next.Enabled = false;
                    btn_prev.Enabled = false;
                    btn_last.Enabled = false;

                    DataSet ds = new DataSet();
                    BindingSource bs = new BindingSource();
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OleDbDataAdapter get = new OleDbDataAdapter("SELECT session_id, session_start_date FROM sessions " +
                                                                   "WHERE session_start_date LIKE '%" + tb_search_date.Text + "%' ORDER BY session_id DESC", conn);

                    get.Fill(ds, "sessions");
                    conn.Close();
                    bs.DataSource = ds.Tables["sessions"];
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

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int sessionId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                SaleHistorySummaryScreen summary = new SaleHistorySummaryScreen();
                summary.sessionId = sessionId;
                summary.ShowDialog();
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
