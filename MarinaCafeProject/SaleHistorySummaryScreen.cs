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
    public partial class SaleHistorySummaryScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public int sessionId;
        public SaleHistorySummaryScreen()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaleHistorySummaryScreen_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.SetColumnSpan(lbl_raw, 2);
            tableLayoutPanel1.Controls.Add(lbl_raw, 0, 5);

            tableLayoutPanel1.SetColumnSpan(btn_raw, 2);
            tableLayoutPanel1.Controls.Add(btn_raw, 0, 6);

            GetSummary(sessionId);
        }

        private void GetSummary(int sessionId)
        {
            try
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                DataTable dt = new DataTable();
                OleDbCommand command = new OleDbCommand("SELECT SUM(total_amount) AS totalAmount, SUM(cash_amount) AS cashAmount, SUM(card_amount) AS cardAmount, SUM(tip_amount) AS tipAmount FROM session_sale WHERE session_id=@session_id", conn);
                command.Parameters.AddWithValue("session_id", sessionId);
                OleDbDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    lbl_total_amount.Text = row["totalAmount"].ToString() + " ₺";
                    lbl_cash.Text = row["cashAmount"].ToString() + " ₺";
                    lbl_card.Text = row["cardAmount"].ToString() + " ₺";
                    lbl_tip.Text = row["tipAmount"].ToString() + " ₺";
                }
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

        private void btn_raw_Click(object sender, EventArgs e)
        {
            SaleHistoryDetailsScreen detailsScreen = new SaleHistoryDetailsScreen();
            detailsScreen.sessionId = sessionId;
            detailsScreen.ShowDialog();
        }
    }
}
