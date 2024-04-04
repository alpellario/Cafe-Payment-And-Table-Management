using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarinaCafeProject
{
    public partial class ProductDeleteScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public int product_id;
        string imagePath = "";
        public ProductDeleteScreen()
        {
            InitializeComponent();
        }

        private void ProductDeleteScreen_Load(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable dt = new DataTable();
                OleDbCommand command = new OleDbCommand("SELECT cafe_product_name, cafe_product_image FROM cafe_product WHERE cafe_product_id=@cafe_product_id", conn);
                command.Parameters.AddWithValue("@cafe_product_id", product_id);
                OleDbDataReader read_category = command.ExecuteReader();
                dt.Load(read_category);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    lbl_category_name.Text = dr["cafe_product_name"].ToString();
                    imagePath = Application.StartupPath + @"\images\" + dr["cafe_product_image"].ToString();
                }
                else
                {
                    MessageBox.Show("Ürün bulunamadı!");
                    conn.Close();
                    this.Close();
                }
                conn.Close();
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                OleDbCommand del = new OleDbCommand("DELETE FROM cafe_product WHERE cafe_product_id=@cafe_product_id", conn);
                del.Parameters.AddWithValue("@cafe_product_id", product_id);
                del.ExecuteNonQuery();
                conn.Close();

                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }

                MessageBox.Show("Ürün başarıyla silindi.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
