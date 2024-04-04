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
    public partial class CategoryCreateScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public bool isUpdate = false;
        public int category_id = 0;
        public CategoryCreateScreen()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CategoryCreateScreen_Load(object sender, EventArgs e)
        {
            if (isUpdate)
            {
                btn_save_client.Text = "Kategori Güncelle";
                bunifuLabel1.Text = "Marina Cafe Kategori Güncelle";

                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    DataTable dt = new DataTable();
                    OleDbCommand command = new OleDbCommand("SELECT * FROM cafe_category WHERE cafe_category_id=@cafe_category_id", conn);
                    command.Parameters.AddWithValue("@cafe_category_id", category_id);
                    OleDbDataReader read_category = command.ExecuteReader();
                    dt.Load(read_category);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        tb_cat_name.Text = dr["cafe_category_name"].ToString();
                        tb_cat_short.Text = dr["cafe_category_short_name"].ToString();
                        tb_cat_short.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show("Kategori bulunamadı!");
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
        }

        private void btn_save_client_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_cat_name.Text))
            {
                if (!string.IsNullOrWhiteSpace(tb_cat_short.Text))
                {
                    if (isUpdate)
                    {

                        bool category_check = false;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        DataTable control = new DataTable();
                        OleDbCommand check = new OleDbCommand("SELECT cafe_category_id, cafe_category_name, cafe_category_short_name FROM cafe_category", conn);
                        OleDbDataReader dr_check = check.ExecuteReader();
                        control.Load(dr_check);
                        for (int i = 0; i < control.Rows.Count; i++)
                        {
                            if (tb_cat_name.Text == control.Rows[i][1].ToString())
                            {
                                if (control.Rows[i][0].ToString() != category_id.ToString())
                                {
                                    category_check = true;
                                    MessageBox.Show("Kategori adı " + tb_cat_name.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir kategori adı giriniz.");
                                }
                            }
                            if (tb_cat_short.Text == control.Rows[i][2].ToString())
                            {

                                if (control.Rows[i][0].ToString() != category_id.ToString())
                                {
                                    category_check = true;
                                    MessageBox.Show("Kategori kısa adı " + tb_cat_short.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir kategori kısa adı giriniz.");
                                }
                            }
                        }
                        if (category_check != true)
                        {
                            try
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OleDbCommand command = new OleDbCommand("UPDATE cafe_category SET cafe_category_name=@cafe_category_name WHERE cafe_category_id=@cafe_category_id", conn);
                                command.Parameters.AddWithValue("@cafe_category_name", tb_cat_name.Text);
                                command.Parameters.AddWithValue("@cafe_category_id", category_id);
                                command.ExecuteNonQuery();
                                MessageBox.Show("Kategori güncellendi.");
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("CAT UP :" + exc.Message);
                            }
                            finally
                            {
                                conn.Close();
                                this.Close();
                            }

                        }

                    }
                    else
                    {

                        bool category_check = false;
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        DataTable control = new DataTable();
                        OleDbCommand check = new OleDbCommand("SELECT cafe_category_name, cafe_category_short_name FROM cafe_category", conn);
                        OleDbDataReader dr_check = check.ExecuteReader();
                        control.Load(dr_check);
                        for (int i = 0; i < control.Rows.Count; i++)
                        {
                            if (tb_cat_name.Text == control.Rows[i][0].ToString())
                            {
                                category_check = true;
                                MessageBox.Show("Kategori adı  " + tb_cat_name.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir kategori kısa adı giriniz.");
                            }
                            else if (tb_cat_short.Text == control.Rows[i][1].ToString())
                            {
                                category_check = true;
                                MessageBox.Show("Kategori kısa adı " + tb_cat_short.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir kategori kısa adı giriniz.");
                            }
                        }

                        if (category_check != true)
                        {

                            try
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OleDbCommand command = new OleDbCommand("INSERT INTO cafe_category (cafe_category_name, cafe_category_short_name) values (@cafe_category_name, @cafe_category_short_name)", conn);
                                command.Parameters.AddWithValue("@cafe_category_name", tb_cat_name.Text);
                                command.Parameters.AddWithValue("@cafe_category_short_name", tb_cat_short.Text.ToUpper());

                                command.ExecuteNonQuery();

                                command.CommandText = "SELECT @@IDENTITY";
                                int created_category_id = Convert.ToInt32(command.ExecuteScalar().ToString());

                                command = new OleDbCommand("INSERT INTO cafe_product_code_tracker (cafe_category_id, cafe_category_product_code_max) values (@cafe_category_id, @cafe_category_product_code_max)", conn);
                                command.Parameters.AddWithValue("@cafe_category_id", created_category_id);
                                command.Parameters.AddWithValue("@cafe_category_product_code_max", 0);
                                command.ExecuteNonQuery();
                                MessageBox.Show("Cafe ürün kategorisi başarıyla oluşturuldu.");
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("CAT CREATE : " + exc.Message);
                            }
                            finally
                            {
                                conn.Close();
                                this.Close();
                            }


                        }
                    }
                }
                else MessageBox.Show("Lütfen bir kategori kısa adı giriniz.");
            }
            else MessageBox.Show("Lütfen bir kategori adı giriniz.");
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
