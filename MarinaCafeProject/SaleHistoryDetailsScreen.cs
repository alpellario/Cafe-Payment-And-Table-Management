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
    public partial class SaleHistoryDetailsScreen : Form
    {
        public int sessionId;
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public SaleHistoryDetailsScreen()
        {
            InitializeComponent();
        }

        private void SaleHistoryDetailsScreen_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;


            this.dataGridView1.DoubleBuffered(true);

            CalculateTotalPages(sessionId);
            this.dataGridView1.DataSource = GetCurrentRecords();

            dataGridView1.Columns[0].HeaderText = "Satış ID";
            dataGridView1.Columns[1].HeaderText = "Total Miktar";
            dataGridView1.Columns[2].HeaderText = "Cash Miktar";
            dataGridView1.Columns[3].HeaderText = "Kart Miktar";
            dataGridView1.Columns[4].HeaderText = "Tip Miktar";
            dataGridView1.Columns[5].HeaderText = "İndirim Miktar";

            dataGridView1.Columns[0].Width = 140;


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

        private BindingSource GetCurrentRecords()
        {
            BindingSource bs = new BindingSource();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataSet ds = new DataSet();
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT session_sale_id, total_amount & ' ₺', cash_amount & ' ₺', card_amount & ' ₺', tip_amount & ' ₺', discount_amount & ' %' FROM session_sale WHERE session_id=@session_id ORDER BY session_sale_id", conn);
                get.SelectCommand.Parameters.AddWithValue("@session_id", sessionId);
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

        private void CalculateTotalPages(int id)
        {
            try
            {
                int rowCount;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM session_sale WHERE session_id=@session_id", conn);
                command.Parameters.AddWithValue("session_id", id);

                rowCount = (int)command.ExecuteScalar();

                lbl_row_count.Text = "Oturumdaki toplam satış miktarı : " + rowCount;
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

                grid.Columns[0].HeaderText = "Satış Oturum ID";
                grid.Columns[1].HeaderText = "Total Miktar";
                grid.Columns[2].HeaderText = "Cash Miktar";
                grid.Columns[3].HeaderText = "Kart Miktar";
                grid.Columns[4].HeaderText = "Tip Miktar";
                grid.Columns[5].HeaderText = "İndirim Miktar";

                grid.Columns[0].Width = 140;


                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }


                grid.ColumnHeadersHeight = 40;


                lbl_loading.Visible = true;
                string fileName = "MarinaCafe-SaleHistoryDetail-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                ExportGridToPdf(grid, fileName);
                this.Controls.Remove(grid);
                lbl_loading.Visible = false;
            }
            else if (result == 0)
            {
                lbl_loading.Visible = true;
                string fileName = "MarinaCafe-SaleHistoryDetail-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
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
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT session_sale_id, total_amount & ' ₺', cash_amount & ' ₺', card_amount & ' ₺', tip_amount & ' ₺', discount_amount & ' %' FROM session_sale WHERE session_id=@session_id ORDER BY session_sale_id", conn);
                get.SelectCommand.Parameters.AddWithValue("@session_id", sessionId);
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
                MessageBox.Show("PDF dışarı aktarıldı.");
                System.Diagnostics.Process.Start(pdfLocation);
            }

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int sessionSaleId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                SaleHistoryDetailsXdeep deep = new SaleHistoryDetailsXdeep();
                deep.sessionSaleId = sessionSaleId;
                deep.ShowDialog();
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
