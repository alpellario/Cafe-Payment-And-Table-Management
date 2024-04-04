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
    public partial class TableMove : Form
    {

        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public int activeSession;
        public object area;
        public object table;
        CafeArea cafeArea = new CafeArea();
        CafeTable cafeTable = new CafeTable();
        public TableMove()
        {
            InitializeComponent();
        }

        private void TableMove_Load(object sender, EventArgs e)
        {
            cafeArea = (CafeArea)area;
            cafeTable = (CafeTable)table;

            GetEmptyTables();
            title.Text = cafeArea.AreaName + " - Table " + cafeTable.TableNumber + " Move Settings";
        }

        private void GetEmptyTables()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataTable dt = new DataTable();
                OleDbDataAdapter categories = new OleDbDataAdapter("SELECT * FROM cafe_table_area", conn);
                categories.Fill(dt);
                cb_area.ValueMember = "area_id";
                cb_area.DisplayMember = "area_name";
                cb_area.DataSource = dt;
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

        private void cb_area_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataTable dt = new DataTable();
                OleDbDataAdapter categories = new OleDbDataAdapter("SELECT * FROM session_tables_status WHERE session_id=@session_id AND area_id=@area_id AND status=0", conn);
                categories.SelectCommand.Parameters.AddWithValue("@session_id", activeSession);
                categories.SelectCommand.Parameters.AddWithValue("@area_id", cb_area.SelectedValue);
                categories.Fill(dt);
                cb_table_number.ValueMember = "table_number";
                cb_table_number.DisplayMember = "table_number";
                cb_table_number.DataSource = dt;
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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public object newArea;
        public object newTable;
        
        private void enter_Click(object sender, EventArgs e)
        {

            DialogResult resut = MessageBox.Show("Masa ve içerisindeki tüm siparişler " + cb_area.Text + " - " + cb_table_number.Text + " numaralı masaya taşınacak. Onaylıyor musunuz ?", "Onay", MessageBoxButtons.OKCancel);
            if (resut == DialogResult.OK)
            {
                CafeArea newAreaC = new CafeArea();
                CafeTable newTableC = new CafeTable();

                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OleDbCommand update = new OleDbCommand("UPDATE session_tables SET area_id=@area_id2, table_number=@table_number2 " +
                    "WHERE session_id=@session_id AND area_id=@area_id AND table_number=@table_number", conn);
                    update.Parameters.AddWithValue("area_id2", cb_area.SelectedValue);
                    update.Parameters.AddWithValue("table_number2", cb_table_number.SelectedValue);
                    update.Parameters.AddWithValue("session_id", activeSession);
                    update.Parameters.AddWithValue("area_id", cafeArea.AreaId);
                    update.Parameters.AddWithValue("table_number", cafeTable.TableNumber);
                    update.ExecuteNonQuery();

                    OleDbCommand update_status = new OleDbCommand("UPDATE session_tables_status SET status=@status " +
                    "WHERE session_id=@session_id AND area_id=@area_id AND table_number=@table_number", conn);
                    update_status.Parameters.AddWithValue("status", 0);
                    update_status.Parameters.AddWithValue("session_id", activeSession);
                    update_status.Parameters.AddWithValue("area_id", cafeArea.AreaId);
                    update_status.Parameters.AddWithValue("table_number", cafeTable.TableNumber);
                    update_status.ExecuteNonQuery();

                    OleDbCommand update_status_new = new OleDbCommand("UPDATE session_tables_status SET status=@status " +
                    "WHERE session_id=@session_id AND area_id=@area_id AND table_number=@table_number", conn);
                    update_status_new.Parameters.AddWithValue("status", 1);
                    update_status_new.Parameters.AddWithValue("session_id", activeSession);
                    update_status_new.Parameters.AddWithValue("area_id", cb_area.SelectedValue);
                    update_status_new.Parameters.AddWithValue("table_number", cb_table_number.SelectedValue);
                    update_status_new.ExecuteNonQuery();

                    newAreaC.AreaId = int.Parse(cb_area.SelectedValue.ToString());
                    newAreaC.AreaName = cb_area.Text;
                    newTableC.TableNumber = int.Parse(cb_table_number.SelectedValue.ToString());
                    newTableC.AreaId = int.Parse(cb_area.SelectedValue.ToString());
                    newTableC.TableType = 1;
                    newArea = newAreaC;
                    newTable = newTableC;

                    MessageBox.Show("Masa taşındı");
                    this.Close();
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
            else
            {
                MessageBox.Show("Taşıma iptal edildi.");
            }
        }
    }
}
