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
    public partial class SaleHistoryDetailsXdeep : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public int sessionSaleId;
        public SaleHistoryDetailsXdeep()
        {
            InitializeComponent();
        }

        private void SaleHistoryDetailsXdeep_Load(object sender, EventArgs e)
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;


            this.dataGridView1.DoubleBuffered(true);

            CalculateTotalPages(sessionSaleId);
            this.dataGridView1.DataSource = GetCurrentRecords();

            dataGridView1.Columns[0].HeaderText = "Ürün ID";
            dataGridView1.Columns[1].HeaderText = "Ürün Adı";
            dataGridView1.Columns[2].HeaderText = "Ürün Satış Fiyat";
            dataGridView1.Columns[3].HeaderText = "Ürün Satış Adet";
            dataGridView1.Columns[4].HeaderText = "Total Fiyat";


            dataGridView1.Columns[0].Width = 120;
            dataGridView1.Columns[1].Width = 300;


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

        private void CalculateTotalPages(int id)
        {
            try
            {
                int rowCount;
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbCommand command = new OleDbCommand("SELECT COUNT(*) FROM session_sale_detail WHERE session_sale_id=@session_sale_id", conn);
                command.Parameters.AddWithValue("session_id", id);

                rowCount = (int)command.ExecuteScalar();

                lbl_row_count.Text = "Ürün Sayısı: " + rowCount;
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

        private BindingSource GetCurrentRecords()
        {
            BindingSource bs = new BindingSource();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataSet ds = new DataSet();
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT product_id, cafe_product_name, product_sale_price & ' ₺', product_sale_count, product_total_price & ' ₺' " +
                                                            "FROM session_sale_detail " +
                                                            "INNER JOIN cafe_product ON cafe_product.cafe_product_id=session_sale_detail.product_id " +
                                                            "WHERE session_sale_id=@session_sale_id ORDER BY product_id", conn);
                get.SelectCommand.Parameters.AddWithValue("@session_sale_id", sessionSaleId);
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

                grid.Columns[0].HeaderText = "Ürün ID";
                grid.Columns[1].HeaderText = "Ürün Adı";
                grid.Columns[2].HeaderText = "Ürün Satış Fiyat";
                grid.Columns[3].HeaderText = "Ürün Satış Adet";
                grid.Columns[4].HeaderText = "Total Fiyat";


                grid.Columns[0].Width = 120;
                grid.Columns[1].Width = 300;


                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    grid.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    grid.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }



                grid.ColumnHeadersHeight = 40;


                lbl_loading.Visible = true;
                string fileName = "MarinaCafe-SaleHistoryProductDetail-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
                ExportGridToPdf(grid, fileName);
                this.Controls.Remove(grid);
                lbl_loading.Visible = false;
            }
            else if (result == 0)
            {
                lbl_loading.Visible = true;
                string fileName = "MarinaCafe&Joy-SaleHistoryProductDetail-" + DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
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
                OleDbDataAdapter get = new OleDbDataAdapter("SELECT product_id, cafe_product_name, product_sale_price & ' ₺', product_sale_count, product_total_price & ' ₺' " +
                                                            "FROM session_sale_detail " +
                                                            "INNER JOIN cafe_product ON cafe_product.cafe_product_id=session_sale_detail.product_id " +
                                                            "WHERE session_sale_id=@session_sale_id ORDER BY product_id", conn);
                get.SelectCommand.Parameters.AddWithValue("@session_sale_id", sessionSaleId);
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

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
