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
    public partial class CategoryDeleteScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public int categoryId;
        public CategoryDeleteScreen()
        {
            InitializeComponent();
        }

        private void CategoryDeleteScreen_Load(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable dt = new DataTable();
                OleDbCommand command = new OleDbCommand("SELECT cafe_category_name FROM cafe_category WHERE cafe_category_id=@cafe_category_id", conn);
                command.Parameters.AddWithValue("@cafe_category_id", categoryId);
                OleDbDataReader read_category = command.ExecuteReader();
                dt.Load(read_category);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    lbl_category_name.Text = dr["cafe_category_name"].ToString();
                }
                else
                {
                    MessageBox.Show("Kategori bulunamadı!");
                    conn.Close();
                    this.Close();
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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbCommand del = new OleDbCommand("DELETE FROM cafe_category WHERE cafe_category_id=@cafe_category_id", conn);
                del.Parameters.AddWithValue("@cafe_category_id", categoryId);
                del.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Kategori silme işlemi başarılı.");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Kategori silme hatası!");
            }
        }
    }
}
