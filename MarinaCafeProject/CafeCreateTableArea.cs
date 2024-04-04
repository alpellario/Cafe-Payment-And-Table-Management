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
    public partial class CafeCreateTableArea : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public int areadId;
        public bool isUpdate = false;
        public CafeCreateTableArea()
        {
            InitializeComponent();
        }

        private void CafeCreateTableArea_Load(object sender, EventArgs e)
        {
            button1.Visible = false;
            if (isUpdate)
            {
                bunifuLabel1.Text = "Masa Düzeni Güncelle";
                btn_save.Text = "Alan Güncelle";
                button1.Visible = true;

                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    DataTable dt = new DataTable();
                    OleDbCommand command = new OleDbCommand("SELECT * FROM cafe_table_area WHERE area_id=@area_id", conn);
                    command.Parameters.AddWithValue("@area_id", areadId);
                    OleDbDataReader read_area = command.ExecuteReader();
                    dt.Load(read_area);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        tb_name.Text = dr["area_name"].ToString();
                        num.Text = dr["area_table_count"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Alan Bulunamadı!");
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

        private void num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void num_Validated(object sender, EventArgs e)
        {
            if (num.Text == "")
            {
                num.Text = "0";
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            bool area_check = false;

            if (!string.IsNullOrEmpty(tb_name.Text))
            {
                if (num.Value >= 1)
                {
                    if (isUpdate)
                    {
                        try
                        {
                         
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            DataTable control = new DataTable();
                            OleDbCommand check = new OleDbCommand("SELECT area_id, area_name FROM cafe_table_area", conn);
                            OleDbDataReader dr_check = check.ExecuteReader();
                            control.Load(dr_check);
                            for (int i = 0; i < control.Rows.Count; i++)
                            {
                                if (tb_name.Text.ToLower() == control.Rows[i][1].ToString().ToLower())
                                {
                                    if (control.Rows[i][0].ToString() != areadId.ToString())
                                    {
                                        area_check = true;
                                        MessageBox.Show("Alan adi " + tb_name.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir alan giriniz.");
                                    }
                                }

                            }
                            if (!area_check)
                            {
                                if (conn.State == ConnectionState.Closed) conn.Open();
                                OleDbCommand cmd = new OleDbCommand("UPDATE cafe_table_area SET area_name=@area_name, area_table_count=@area_table_count WHERE area_id=@area_id", conn);
                                cmd.Parameters.AddWithValue("@area_name", tb_name.Text);
                                cmd.Parameters.AddWithValue("@area_table_count", Convert.ToInt16(num.Value));
                                cmd.Parameters.AddWithValue("@area_id", areadId);
                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Alan ve masa sayıları başarı ile güncellendi.");
                                this.Close();
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
                    else
                    {                       
                        try
                        {
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            DataTable control = new DataTable();
                            OleDbCommand check = new OleDbCommand("SELECT area_name FROM cafe_table_area", conn);
                            OleDbDataReader dr_check = check.ExecuteReader();
                            control.Load(dr_check);
                            for (int i = 0; i < control.Rows.Count; i++)
                            {
                                if (tb_name.Text == control.Rows[i][0].ToString())
                                {
                                    area_check = true;
                                    MessageBox.Show("Alan adı " + tb_name.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir alan adı giriniz.");
                                }

                            }
                            if(area_check != true)
                            {
                                OleDbCommand cmd = new OleDbCommand("INSERT INTO cafe_table_area (area_name, area_table_count) values (@area_name, @area_table_count)", conn);
                                cmd.Parameters.AddWithValue("@area_name", tb_name.Text);
                                cmd.Parameters.AddWithValue("@area_table_count", Convert.ToInt16(num.Value));
                                cmd.ExecuteNonQuery();

                                MessageBox.Show("Alan ve masalar başarı ile kaydedildi.");
                                this.Close();
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

                }
                else MessageBox.Show("Masa Saysı 0 dan büyük olmalıdır.");
            }
            else MessageBox.Show("Arae name is required.");

        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show("Alaní silmek istediğinize emin misiniz ? Eğer alanı silerseniz alandaki tüm masalarda silinecektir ? Devam ediyor musunuz ?", "Confirmation ?", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                try
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    OleDbCommand del = new OleDbCommand("DELETE FROM cafe_table_area WHERE area_id=@area_id", conn);
                    del.Parameters.AddWithValue("@area_id", areadId);
                    del.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Alan silme işlemi başarılı.");
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Alan silme hatası!");
                }
            }
            else MessageBox.Show("Silme iptal edildi.");
        }
    }
}
