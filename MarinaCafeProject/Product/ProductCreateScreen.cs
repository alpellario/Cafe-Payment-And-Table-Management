using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarinaCafeProject
{
    public partial class ProductCreateScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public bool isUpdate = false;

        public int categoryId = 0;
        public int productId = 0;
        public ProductCreateScreen()
        {
            InitializeComponent();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProductCreateScreen_Load(object sender, EventArgs e)
        {
            get_categories();

            if (!isUpdate)
            {
                create_product_code();
            }
            else
            {
                //cb_product_cat.Enabled = false;
                bunifuLabel1.Text = "Marina Kafe Ürün Güncelleme";
                btn_save_product.Text = "Ürün Güncelle";
                getData4Update();
            }

            tb_product_name.Focus();
        }

        private void get_categories()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                DataTable dt = new DataTable();
                OleDbDataAdapter categories = new OleDbDataAdapter("SELECT * FROM cafe_category", conn);
                categories.Fill(dt);
                cb_product_cat.ValueMember = "cafe_category_id";
                cb_product_cat.DisplayMember = "cafe_category_name";
                cb_product_cat.DataSource = dt;
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

        int productCodeNumber = 0;
        private void create_product_code()
        {
            string productCodeString = "";

            productCodeNumber = 0;
            string productCode = "";
            if (conn.State == ConnectionState.Closed) conn.Open();
            DataTable control = new DataTable();
            OleDbCommand check = new OleDbCommand("SELECT CPT.cafe_category_product_code_max, CC.cafe_category_short_name " +
                                                    "FROM cafe_product_code_tracker AS CPT " +
                                                        "INNER JOIN cafe_category as CC ON CC.cafe_category_id=CPT.cafe_category_id WHERE CPT.cafe_category_id=@cafe_category_id", conn);
            check.Parameters.AddWithValue("@cafe_category_id", cb_product_cat.SelectedValue.ToString());
            OleDbDataReader dr_check = check.ExecuteReader();
            control.Load(dr_check);
            for (int i = 0; i < control.Rows.Count; i++)
            {
                productCodeNumber = int.Parse(control.Rows[i][0].ToString());
                productCode = control.Rows[i][1].ToString();
            }
            productCodeNumber++;
            productCodeString = productCode + productCodeNumber.ToString("D4");
            tb_product_code.Text = productCodeString;
        }

        double price;
        bool permMoney = false;
        private void getData4Update()
        {
            try
            {
                if (productId != 0)
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    DataTable dt = new DataTable();
                    OleDbCommand command = new OleDbCommand("SELECT CC.cafe_category_name, CP.cafe_product_name, CP.cafe_product_code, CP.cafe_product_image, CP.cafe_product_description, CP.cafe_product_price " +
                                                                "FROM cafe_product as CP " +
                                                                    "INNER JOIN cafe_category as CC ON CC.cafe_category_id=CP.cafe_category_id " +
                                                                        "WHERE CP.cafe_product_id=@cafe_product_id", conn);
                    command.Parameters.AddWithValue("@cafe_product_id", productId);
                    OleDbDataReader read = command.ExecuteReader();
                    dt.Load(read);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        if (dr["cafe_product_image"].ToString() != "")
                        {
                            imagePath = Application.StartupPath + @"\images\" + dr["cafe_product_image"].ToString();
                            imageLocation = imagePath;
                        }
                        if (imagePath != "")
                        {
                            try
                            {
                                Image img;
                                using (var bmpTemp = new Bitmap(imagePath))
                                {
                                    img = new Bitmap(bmpTemp);
                                }
                                pictureBox1.Image = img;

                            }
                            catch { }

                        }
                        oldPicture = dr["cafe_product_image"].ToString();

                        tb_product_code.Text = dr["cafe_product_code"].ToString();
                        tb_product_name.Text = dr["cafe_product_name"].ToString();
                        cb_product_cat.SelectedIndex = cb_product_cat.FindStringExact(dr["cafe_category_name"].ToString());
                        tb_desc.Text = dr["cafe_product_description"].ToString();
                        price = double.Parse(dr["cafe_product_price"].ToString());


                        tb_price.Text = price.ToString();

                        Double value;
                        if (Double.TryParse(tb_price.Text, out value))
                        {
                            tb_price.Text = String.Format(CultureInfo.CreateSpecificCulture("pl_PL"), "{0:C2}", value);
                            price = value;

                        }
                        else
                            tb_price.Text = String.Empty;

                        permMoney = true;
                        btn_save_product.Focus();
                    }


                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Ürün bulunamadı.");
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

        string oldPicture;
        string new_imagePath = "";
        string imagePath = "";
        string imageLocation = null;
        private void btn_add_picture_Click(object sender, EventArgs e)
        {
            if (isUpdate)
            {
                try
                {
                    OpenFileDialog file = new OpenFileDialog();
                    file.FilterIndex = 2;
                    file.RestoreDirectory = true;
                    file.CheckFileExists = false;

                    file.Filter = "";

                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    string sep = string.Empty;

                    foreach (var c in codecs)
                    {
                        string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                        file.Filter = String.Format("{0}{1}{2} ({3})|{3}", file.Filter, sep, codecName, c.FilenameExtension);
                        sep = "|";
                    }
                    file.DefaultExt = ".png";

                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        new_imagePath = file.FileName;
                        imageLocation = new_imagePath;
                        string DosyaAdi = file.SafeFileName;


                        Image img;
                        using (var bmpTemp = new Bitmap(new_imagePath))
                        {
                            img = new Bitmap(bmpTemp);
                        }
                        pictureBox1.Image = img;
                        imageLocation = new_imagePath;
                    }

                }
                catch
                {
                    new_imagePath = "";
                    pictureBox1.ImageLocation = null;

                }
            }
            else
            {
                try
                {
                    OpenFileDialog file = new OpenFileDialog();
                    file.FilterIndex = 2;
                    file.RestoreDirectory = true;
                    file.CheckFileExists = false;

                    file.Filter = "";

                    ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                    string sep = string.Empty;

                    foreach (var c in codecs)
                    {
                        string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                        file.Filter = String.Format("{0}{1}{2} ({3})|{3}", file.Filter, sep, codecName, c.FilenameExtension);
                        sep = "|";
                    }
                    file.DefaultExt = ".png";


                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        imagePath = file.FileName;
                        string DosyaAdi = file.SafeFileName;

                        Image img;
                        using (var bmpTemp = new Bitmap(imagePath))
                        {
                            img = new Bitmap(bmpTemp);
                        }
                        pictureBox1.Image = img;
                        imageLocation = imagePath;
                    }
                }
                catch
                {
                    imagePath = "";
                    pictureBox1.ImageLocation = null;
                }
            }
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            imageLocation = null;
        }

        private void cb_product_cat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isUpdate)
            {
                create_product_code();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tb_price_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            //check if '.' , ',' pressed
            char sepratorChar = 's';
            if (e.KeyChar == ',')
            {
                // check if it's in the beginning of text not accept
                if (tb_price.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (tb_price.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(tb_price.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (tb_price.SelectionStart != tb_price.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = tb_price.Text.Substring(tb_price.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            //check if a number pressed

            if (Char.IsDigit(e.KeyChar))
            {
                //check if a coma or dot exist
                if (alreadyExist(tb_price.Text, ref sepratorChar))
                {
                    int sepratorPosition = tb_price.Text.IndexOf(sepratorChar);
                    string afterSepratorString = tb_price.Text.Substring(sepratorPosition + 1);
                    if (tb_price.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }

                }
            }
        }
        private bool alreadyExist(string _text, ref char KeyChar)
        {
            if (_text.IndexOf(',') > -1)
            {
                KeyChar = ',';
                return true;
            }
            return false;
        }

        private void tb_price_TextChanged(object sender, EventArgs e)
        {
            permMoney = false;
        }

        private void btn_check_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tb_price.Text) && price != 0)
            {
                permMoney = true;
            }
            else MessageBox.Show("Ürün fiyatı boş veya 0 olamaz.");
        }

        private void tb_price_Leave(object sender, EventArgs e)
        {
            Double value;
            if (Double.TryParse(tb_price.Text, out value))
            {
                tb_price.Text = String.Format(CultureInfo.CreateSpecificCulture("tr_TR"), "{0:C2}", value);
                price = value;

            }
            else
                tb_price.Text = String.Empty;
        }

        private void btn_save_product_Click(object sender, EventArgs e)
        {
            bool product_check = false;
            int created_product_id;

            if (!string.IsNullOrWhiteSpace(tb_product_code.Text))
            {
                if (!string.IsNullOrWhiteSpace(tb_product_name.Text))
                {
                    if (!string.IsNullOrWhiteSpace(cb_product_cat.SelectedValue.ToString()))
                    {
                        if (permMoney)
                        {
                            try
                            {
                                if (isUpdate)
                                {

                                    string new_name = "not empty";
                                    if (imageLocation == null)
                                    {
                                        MessageBox.Show("Bir ürün fotoğrafı yüklemediniz. Varsayılan ürün fotoğrafı kullanılacak.");
                                        new_name = "";
                                        if (File.Exists(imagePath))
                                        {
                                            File.Delete(imagePath);
                                        }
                                    }
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    DataTable control = new DataTable();
                                    OleDbCommand check = new OleDbCommand("SELECT cafe_product_id, cafe_product_name FROM cafe_product", conn);
                                    OleDbDataReader dr_check = check.ExecuteReader();
                                    control.Load(dr_check);
                                    for (int i = 0; i < control.Rows.Count; i++)
                                    {
                                        if (tb_product_name.Text == control.Rows[i][1].ToString())
                                        {
                                            if (productId.ToString() != control.Rows[i][0].ToString())
                                            {
                                                product_check = true;
                                                MessageBox.Show("Ürün adı " + tb_product_name.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir ürün adı giriniz.");
                                            }
                                        }
                                    }
                                    if (product_check != true)
                                    {
                                        if (new_name != "")
                                        {
                                            if (imagePath != imageLocation)
                                            {
                                                if (File.Exists(imagePath))
                                                {
                                                    File.Delete(imagePath);
                                                }
                                                string source = new_imagePath;
                                                string target = Application.StartupPath + @"\images\";
                                                new_name = Guid.NewGuid() + ".jpg";
                                                File.Copy(source, target + new_name);

                                            }
                                            else
                                            {
                                                new_name = oldPicture;
                                            }
                                        }
                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        OleDbCommand command = new OleDbCommand("UPDATE cafe_product SET cafe_category_id=@cafe_category_id, cafe_product_name=@cafe_product_name, cafe_product_image=@cafe_product_image, cafe_product_description=@cafe_product_description, cafe_product_price=@cafe_product_price WHERE cafe_product_id=@cafe_product_id", conn);
                                        command.Parameters.AddWithValue("@cafe_category_id", cb_product_cat.SelectedValue.ToString());
                                        command.Parameters.AddWithValue("@cafe_product_name", tb_product_name.Text);
                                        command.Parameters.AddWithValue("@cafe_product_image", new_name);
                                        command.Parameters.AddWithValue("@cafe_product_description", tb_desc.Text);
                                        command.Parameters.AddWithValue("@cafe_product_price", Convert.ToDecimal(price).ToString());



                                        command.Parameters.AddWithValue("@cafe_product_id", productId);
                                        command.ExecuteNonQuery();


                                        MessageBox.Show("Ürün başarıyla güncellendi.");
                                    }

                                }
                                else
                                {
                                    if (imageLocation == null)
                                    {
                                        MessageBox.Show("Bir ürün fotoğrafı yüklemediniz. Varsayılan ürün fotoğrafı kullanılacak.");
                                        imagePath = "";
                                    }
                                    if (conn.State == ConnectionState.Closed) conn.Open();
                                    DataTable control = new DataTable();
                                    OleDbCommand check = new OleDbCommand("SELECT cafe_product_name FROM cafe_product", conn);
                                    OleDbDataReader dr_check = check.ExecuteReader();
                                    control.Load(dr_check);
                                    for (int i = 0; i < control.Rows.Count; i++)
                                    {
                                        if (tb_product_name.Text == control.Rows[i][0].ToString())
                                        {
                                            product_check = true;
                                            MessageBox.Show("Ürün adı " + tb_product_name.Text.ToUpper() + " zaten mevcut.\nLütfen başka bir ürün adı giriniz.");
                                        }

                                    }
                                    if (product_check != true)
                                    {
                                        string new_pic_name = "";
                                        if (imagePath != "")
                                        {
                                            string source = imagePath;
                                            string hedef = Application.StartupPath + @"\images\";
                                            new_pic_name = Guid.NewGuid() + ".jpg";
                                            File.Copy(source, hedef + new_pic_name);
                                        }

                                        if (conn.State == ConnectionState.Closed) conn.Open();
                                        OleDbCommand command = new OleDbCommand("INSERT INTO cafe_product (cafe_category_id, cafe_product_name, cafe_product_code, cafe_product_image, cafe_product_description, cafe_product_price) values (@cafe_category_id, @cafe_product_name, @cafe_product_code, @cafe_product_image, @cafe_product_description, @cafe_product_price)", conn);
                                        command.Parameters.AddWithValue("@cafe_category_id", cb_product_cat.SelectedValue.ToString());
                                        command.Parameters.AddWithValue("@cafe_product_name", tb_product_name.Text);
                                        command.Parameters.AddWithValue("@cafe_product_code", tb_product_code.Text);
                                        command.Parameters.AddWithValue("@cafe_product_image", new_pic_name);
                                        command.Parameters.AddWithValue("@cafe_product_description", tb_desc.Text);
                                        command.Parameters.AddWithValue("@cafe_product_price", price);

                                        

                                        command.ExecuteNonQuery();
                                        Console.WriteLine("Product data is created");

                                        command.CommandText = "SELECT @@IDENTITY";
                                        created_product_id = Convert.ToInt32(command.ExecuteScalar().ToString());

                                        command = new OleDbCommand("UPDATE cafe_product_code_tracker SET cafe_category_product_code_max=@cafe_category_product_code_max WHERE cafe_category_id=@cafe_category_id", conn);
                                        command.Parameters.AddWithValue("@cafe_category_product_code_max", productCodeNumber);
                                        command.Parameters.AddWithValue("@cafe_category_id", cb_product_cat.SelectedValue.ToString());
                                        command.ExecuteNonQuery();
                                        Console.WriteLine("product_code_tracker data is created");

                                        MessageBox.Show("Ürün başarıyla kaydedildi.");
                                        DialogResult dialog = new DialogResult();

                                    }
                                }
                            }
                            catch (Exception exc)
                            {


                                MessageBox.Show(exc.Message);
                            }
                            finally
                            {
                                conn.Close();
                                this.Close();
                            }
                        }
                        else MessageBox.Show("Lütfen girilen ürün fiyatını onaylamak için 'Kontrol' butonuna basınız.");
                    }
                    else MessageBox.Show("Kategori seçimi yapmadınız.");
                }
                else MessageBox.Show("Ürün adı gerekli.");
            }
            else MessageBox.Show("Hata: Ürün kodu oluşturulamadı.");
        }
    }
}
