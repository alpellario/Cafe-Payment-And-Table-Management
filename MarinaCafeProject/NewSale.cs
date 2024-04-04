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
    public partial class NewSale : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public int activeSession;

        bool preventEvent = false;
        public NewSale()
        {
            InitializeComponent();
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000; // WS_CLIPCHILDREN
        //        return cp;
        //    }
        //}
        private void NewSale_Load(object sender, EventArgs e)
        {

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            this.dataGridView1.DoubleBuffered(true);

            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Product Code";
            dataGridView1.Columns[2].HeaderText = "Product Name";
            dataGridView1.Columns[3].HeaderText = "Product Price";
            dataGridView1.Columns[4].HeaderText = "Quantity";

            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[2].Width = 300;
            dataGridView1.Columns[3].Width = 120;
            dataGridView1.Columns[4].Width = 120;

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                this.dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }

            dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            this.dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(54, 35, 35);
            this.dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            this.dataGridView1.DefaultCellStyle.Font = new Font("Calibri", 15);

            dataGridView1.RowsAdded += DataGridView1_RowsAdded;

            btn_back.Visible = false;

            GetAllProduct();

            GetProductCategories();

            CreateProductCategoryButtons();

            preventEvent = true;
            tb_search.Text = "Search all products";
            preventEvent = false;

            tb_search.GotFocus += Tb_search_GotFocus;
            tb_search.LostFocus += Tb_search_LostFocus;


        }

        private void Tb_search_GotFocus(object sender, EventArgs e)
        {
            preventEvent = true;
            if(search_status == 0)
            {
                if (tb_search.Text == "Search all products")
                {
                    tb_search.Text = "";
                }
            }
            else if (search_status == 1)
            {
                if (tb_search.Text == "Search in this category")
                {
                    tb_search.Text = "";
                }
            }         
            preventEvent = false;
        }
        private void Tb_search_LostFocus(object sender, EventArgs e)
        {
            preventEvent = true;
            if(search_status == 0)
            {
                if (string.IsNullOrWhiteSpace(tb_search.Text))
                {
                    tb_search.Text = "Search all products";
                }
            }
            else if(search_status == 1)
            {
                if (string.IsNullOrWhiteSpace(tb_search.Text))
                {
                    tb_search.Text = "Search in this category";
                }
            }
            preventEvent = false;
        }

        List<SaleProductCS> saleProductList = new List<SaleProductCS>();
        private void GetAllProduct()
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();
                if (conn.State != ConnectionState.Open) conn.Open();
                DataTable dataTable = new DataTable();
                OleDbCommand command = new OleDbCommand("SELECT cafe_product_id, cafe_category_id, cafe_product_name, cafe_product_image FROM cafe_product ORDER BY cafe_product_name", conn);
                OleDbDataReader reader = command.ExecuteReader();
                dataTable.Load(reader);
                if (dataTable.Rows.Count > 0)
                {


                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        SaleProductCS saleProduct = new SaleProductCS();
                        DataRow dataRow = dataTable.Rows[i];
                        saleProduct = new SaleProductCS();

                        saleProduct.ProductName = dataRow["cafe_product_name"].ToString().ToLower();
                        saleProduct.ProductNameOrj = dataRow["cafe_product_name"].ToString();
                        saleProduct.ProductId = int.Parse(dataRow["cafe_product_id"].ToString());
                        saleProduct.ProductCategoryId = int.Parse(dataRow["cafe_category_id"].ToString());
                        string image = dataRow["cafe_product_image"].ToString();

                        if (!string.IsNullOrWhiteSpace(image))
                        {
                            saleProduct.ProductImage = image;
                        }
                        else
                        {
                            saleProduct.ProductImage = "";
                        }

                        saleProductList.Add(saleProduct);

                    }
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

        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Height = 30;
        }

        List<CafeCategory> cafeCategories = new List<CafeCategory>();
        private void GetProductCategories()
        {
            try
            {
                cafeCategories.Clear();
                if (conn.State != ConnectionState.Open) conn.Open();
                DataTable dataTable = new DataTable();
                OleDbCommand cmd = new OleDbCommand("SELECT cafe_category_id, cafe_category_name FROM cafe_category " +
                                                        "ORDER BY cafe_category_name", conn);
                OleDbDataReader dataReader = cmd.ExecuteReader();
                dataTable.Load(dataReader);
                if (dataTable.Rows.Count > 0)
                {
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        DataRow dataRow = dataTable.Rows[i];
                        CafeCategory cafeCategory = new CafeCategory();
                        cafeCategory.CategoryId = int.Parse(dataRow["cafe_category_id"].ToString());
                        cafeCategory.CategoryName = dataRow["cafe_category_name"].ToString();
                        cafeCategories.Add(cafeCategory);
                    }
                }

                Console.Write(dataTable.Rows.Count + " adet Kategori çekildi ;");
                foreach (var cafeCategory in cafeCategories)
                {
                    Console.WriteLine(cafeCategory.CategoryId);
                    Console.WriteLine(cafeCategory.CategoryName);
                }
                Console.WriteLine("\n");

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
        private void CreateProductCategoryButtons()
        {
            flowLayoutPanel1.Controls.Clear();

            Button[] buttons = new Button[cafeCategories.Count];
            Bunifu.Framework.UI.BunifuElipse bunifuElipse = new Bunifu.Framework.UI.BunifuElipse();

            if (cafeCategories.Count > 14)
                panel1.Size = new Size(376, 660);
            else panel1.Size = new Size(357, 660);

            for (int i = 0; i < cafeCategories.Count; i++)
            {

                buttons[i] = new Button();
                buttons[i].Text = cafeCategories[i].CategoryName;
                buttons[i].Name = cafeCategories[i].CategoryId.ToString();
                buttons[i].Width = 170;
                buttons[i].Height = 80;
                buttons[i].Font = new Font("Calibri", 16);
                buttons[i].BackColor = Color.Red;
                buttons[i].ForeColor = Color.White;
                buttons[i].FlatStyle = FlatStyle.Flat;
                buttons[i].FlatAppearance.BorderSize = 0;
                buttons[i].Click += NewSale_Click;

                bunifuElipse.ApplyElipse(buttons[i], 15);

                flowLayoutPanel1.Controls.Add(buttons[i]);
            }
        }
        int search_status = 0;
        private void btn_back_Click(object sender, EventArgs e)
        {
            btn_back.Visible = false;
            lbl_header.Text = "Categories";
            search_status = 0;

            preventEvent = true;
            tb_search.Text = "Search all products";
            preventEvent = false;

            CreateProductCategoryButtons();
        }

        int productCategory = 0;
        private void NewSale_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int categoryId = int.Parse(btn.Name);
            productCategory = categoryId;
            btn_back.Visible = true;
            lbl_header.Text = btn.Text;
            search_status = 1;

            preventEvent = true;
            tb_search.Text = "Search in this category";
            preventEvent = false;


            flowLayoutPanel1.Controls.Clear();


            try
            {

                var watch = System.Diagnostics.Stopwatch.StartNew();
                if (conn.State != ConnectionState.Open) conn.Open();
                DataTable dataTable = new DataTable();
                OleDbCommand command = new OleDbCommand("SELECT cafe_product_id, cafe_product_name, cafe_product_image FROM cafe_product WHERE cafe_category_id=@cafe_category_id ORDER BY cafe_product_name", conn);
                command.Parameters.AddWithValue("@cafe_category_id", categoryId);
                OleDbDataReader reader = command.ExecuteReader();
                dataTable.Load(reader);
                if (dataTable.Rows.Count > 0)
                {
                    SaleProduct[] saleProducts = new SaleProduct[dataTable.Rows.Count];
                    if (saleProducts.Length > 5)
                        panel1.Size = new Size(385, 660);
                    else panel1.Size = new Size(370, 660);
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {

                        DataRow dataRow = dataTable.Rows[i];

                        myButton button = new myButton();
                        button.Paint += Button_Paint;
                        button.Size = new Size(358, 100);
                        button.ForeColor = Color.Black;
                        button.BackColor = Color.White;
                        button.Font = new Font("Segoe UI", 16);
                        button.FlatStyle = FlatStyle.Flat;
                        button.FlatAppearance.BorderSize = 1;
                        button.FlatAppearance.BorderColor = Color.FromArgb(252, 221, 130);
                        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(252, 221, 130);

                        button.HiddenText = dataRow["cafe_product_name"].ToString();
                        button.HiddenId = int.Parse(dataRow["cafe_product_id"].ToString());
                        string image = dataRow["cafe_product_image"].ToString();
                        if (!string.IsNullOrWhiteSpace(image))
                        {
                            button.HiddenImage = Application.StartupPath + @"\images\" + image;
                        }
                        else
                        {
                            button.HiddenImage = "";
                        }

                        flowLayoutPanel1.Controls.Add(button);
                        button.MouseDown += Button_MouseDown;
                        button.MouseUp += Button_MouseUp;
                        button.MouseEnter += Button_MouseEnter;
                        button.MouseLeave += Button_MouseLeave;
                        Bunifu.Framework.UI.BunifuElipse bunifuElipse = new Bunifu.Framework.UI.BunifuElipse();
                        bunifuElipse.ApplyElipse(button, 15);


                    }

                }
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Console.WriteLine("ELAPSED TIME: " + elapsedMs);
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

        private void Button_Paint(object sender, PaintEventArgs e)
        {
            myButton btn = (myButton)sender;
            string drawString = btn.HiddenText;
            Font drawFont = new Font("Segoe UI", 17);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            RectangleF drawRect = new RectangleF(120, 20, 233, 60);
            //Pen blackPen = new Pen(Color.Black);
            //e.Graphics.DrawRectangle(blackPen, 120, 20, 233, 60);
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);


            SolidBrush brush = new SolidBrush(Color.FromArgb(252, 221, 130));
            // Create rectangle.
            Rectangle rect = new Rectangle(0, 0, 115, 100);
            // Draw rectangle to screen.
            e.Graphics.FillRectangle(brush, rect);

            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(90, 80);
            if (btn.HiddenImage != "")
            {
                string imagePath = btn.HiddenImage;
                Image img;
                using (var bmpTemp = new Bitmap(imagePath))
                {
                    img = new Bitmap(bmpTemp);
                }
                pictureBox.Image = img;
            }
            else
            {
                pictureBox.Image = Properties.Resources.food;
            }

            pictureBox.BackColor = Color.FromArgb(252, 221, 130);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            var bm = new Bitmap(pictureBox.ClientSize.Width, pictureBox.ClientSize.Height);
            pictureBox.DrawToBitmap(bm, pictureBox.ClientRectangle);

            RectangleF srcRect = new RectangleF(0, 0, 90, 80);
            GraphicsUnit units = GraphicsUnit.Pixel;

            e.Graphics.DrawImage(bm, 13, 9, 90, 80);

        }
        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            myButton button = (myButton)sender;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(252, 221, 130);

        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            myButton btn = (myButton)sender;
            btn.BackColor = Color.White;
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            myButton btn = (myButton)sender;
            btn.BackColor = Color.FromArgb(252, 221, 130);
        }



        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            myButton button = (myButton)sender;
            button.FlatAppearance.BorderColor = Color.Red;
            button.FlatAppearance.BorderSize = 5;

            AddProduct(button.HiddenId);
        }
        private void NewSale_Click1(object sender, EventArgs e)
        {
            SaleProduct obj = (SaleProduct)sender;
            Console.WriteLine(obj.Id + " eklendi");
            AddProduct(obj.Id);
        }
        private void Label_Click(object sender, EventArgs e)
        {
            Bunifu.UI.WinForms.BunifuLabel label = (Bunifu.UI.WinForms.BunifuLabel)sender;
            Console.WriteLine(label.Name + " eklendi");
            AddProduct(int.Parse(label.Name));
        }


        List<PaymentProduct> products = new List<PaymentProduct>();
        private void AddProduct(int productId)
        {
            bool isExist = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value.ToString() == productId.ToString())
                {
                    products.Where(w => w.ProductId == productId).ToList().ForEach(p => p.ProductCount += 1);
                    dataGridView1.Rows[row.Index].Cells[4].Value = int.Parse(dataGridView1.Rows[row.Index].Cells[4].Value.ToString()) + 1;
                    CalculateTotalPrice();
                    isExist = true;
                }
            }
            if (!isExist)
            {
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();

                    DataTable dt = new DataTable();
                    OleDbCommand command = new OleDbCommand("SELECT cafe_product_name, cafe_product_code, cafe_product_price FROM cafe_product WHERE cafe_product_id=@cafe_product_id", conn);
                    command.Parameters.AddWithValue("@cafe_product_id", productId);
                    OleDbDataReader dataReader = command.ExecuteReader();
                    dt.Load(dataReader);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dataRow = dt.Rows[0];
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridView1);
                        row.Cells[0].Value = productId.ToString();
                        row.Cells[1].Value = dataRow["cafe_product_code"].ToString();
                        row.Cells[2].Value = dataRow["cafe_product_name"].ToString();
                        row.Cells[3].Value = dataRow["cafe_product_price"].ToString() + " ₺";
                        row.Cells[4].Value = 1;
                        dataGridView1.Rows.Add(row);

                        PaymentProduct product = new PaymentProduct();
                        product.ProductPrice = double.Parse(dataRow["cafe_product_price"].ToString());
                        product.PaidProductQty = 0;
                        product.ProductId = productId;
                        product.ProductCount = 1;
                        products.Add(product);
                    }
                    CalculateTotalPrice();

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

        private double totalAmount = 0;
        private void CalculateTotalPrice()
        {
            double totalPrice = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string[] priceString = row.Cells[3].Value.ToString().Split(' ');
                double price = double.Parse(priceString[0]);
                totalPrice += (price * int.Parse(row.Cells[4].Value.ToString()));
            }
            lbl_total_price.Text = totalPrice.ToString() + " ₺";
            totalAmount = totalPrice;
        }

        private void ResetPage()
        {
            products.Clear();
            dataGridView1.Rows.Clear();
            CalculateTotalPrice();
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            ResetPage();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int currentQuantity = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                int currentId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                NewSaleChangeQuantity quantity = new NewSaleChangeQuantity();
                quantity.quantity = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                quantity.ShowDialog();
                int newQuantity = quantity.newQuantity;
                if (newQuantity != currentQuantity)
                {
                    if (newQuantity != 0)
                    {
                        products.Where(w => w.ProductId == currentId).ToList().ForEach(p => p.ProductCount = newQuantity);
                        dataGridView1.CurrentRow.Cells[4].Value = newQuantity;
                    }

                    else
                    {
                        foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                        {
                            dataGridView1.Rows.RemoveAt(item.Index);
                        }
                        PaymentProduct product = products.SingleOrDefault(p => p.ProductId == currentId);
                        if (product != null)
                        {
                            products.Remove(product);
                        }
                    }
                }
                CalculateTotalPrice();
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            if (products.Count > 0)
            {
                List<PaymentProduct> tempProducts = new List<PaymentProduct>(products.Count);
                products.ForEach((item) =>
                {
                    tempProducts.Add(item.Clone());
                });

                PaymentScreen paymentScreen = new PaymentScreen();
                paymentScreen.activeSession = activeSession;
                paymentScreen.list = tempProducts;
                paymentScreen.ShowDialog();
                ResetPage();
                paymentScreen = null;
                btn_back.Visible = false;
                lbl_header.Text = "Categories";

                preventEvent = true;
                tb_search.Text = "Search all products";
                preventEvent = false;
                search_status = 0;

                CreateProductCategoryButtons();
            }
            else MessageBox.Show("There is no product in the table.");
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!preventEvent)
            {
                string compareTo = tb_search.Text.Trim().ToLower();
                if (search_status == 1)
                {
                    flowLayoutPanel1.Controls.Clear();
                    var products = saleProductList.Where(x => x.ProductName.Contains(tb_search.Text.ToLower())
                                                          && x.ProductCategoryId == productCategory);
                    if (products != null)
                    {
                        SaleProduct[] saleProducts = new SaleProduct[products.ToList().Count];
                        if (saleProducts.Length > 5)
                            panel1.Size = new Size(385, 660);
                        else panel1.Size = new Size(370, 660);

                        Console.WriteLine("\n");

                        foreach (var x in products)
                        {
                            myButton button = new myButton();
                            button.Paint += Button_Paint;
                            button.Size = new Size(358, 100);
                            button.ForeColor = Color.Black;
                            button.BackColor = Color.White;
                            button.Font = new Font("Segoe UI", 16);
                            button.FlatStyle = FlatStyle.Flat;
                            button.FlatAppearance.BorderSize = 1;
                            button.FlatAppearance.BorderColor = Color.FromArgb(252, 221, 130);
                            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(252, 221, 130);

                            button.HiddenText = x.ProductNameOrj;
                            button.HiddenId = x.ProductId;
                            string image = x.ProductImage;
                            if (!string.IsNullOrWhiteSpace(image))
                            {
                                button.HiddenImage = Application.StartupPath + @"\images\" + image;
                            }
                            else
                            {
                                button.HiddenImage = "";
                            }

                            flowLayoutPanel1.Controls.Add(button);
                            button.MouseDown += Button_MouseDown;
                            button.MouseUp += Button_MouseUp;
                            button.MouseEnter += Button_MouseEnter;
                            button.MouseLeave += Button_MouseLeave;
                            Bunifu.Framework.UI.BunifuElipse bunifuElipse = new Bunifu.Framework.UI.BunifuElipse();
                            bunifuElipse.ApplyElipse(button, 15);

                        }
                    }
                }
                else
                {
                    if (tb_search.Text.Trim() == "")
                    {
                        btn_back.Visible = false;
                        lbl_header.Text = "Categories";
                        CreateProductCategoryButtons();
                    }
                    else
                    {
                        try
                        {

                            flowLayoutPanel1.Controls.Clear();
                            var products = saleProductList.Where(x => x.ProductName.Contains(tb_search.Text.ToLower()));
                            if (products != null)
                            {
                                SaleProduct[] saleProducts = new SaleProduct[products.ToList().Count];
                                if (saleProducts.Length > 5)
                                    panel1.Size = new Size(385, 660);
                                else panel1.Size = new Size(370, 660);

                                Console.WriteLine("\n");

                                foreach (var x in products)
                                {
                                    myButton button = new myButton();
                                    button.Paint += Button_Paint;
                                    button.Size = new Size(358, 100);
                                    button.ForeColor = Color.Black;
                                    button.BackColor = Color.White;
                                    button.Font = new Font("Segoe UI", 16);
                                    button.FlatStyle = FlatStyle.Flat;
                                    button.FlatAppearance.BorderSize = 1;
                                    button.FlatAppearance.BorderColor = Color.FromArgb(252, 221, 130);
                                    button.FlatAppearance.MouseOverBackColor = Color.FromArgb(252, 221, 130);

                                    button.HiddenText = x.ProductNameOrj;
                                    button.HiddenId = x.ProductId;
                                    string image = x.ProductImage;
                                    if (!string.IsNullOrWhiteSpace(image))
                                    {
                                        button.HiddenImage = Application.StartupPath + @"\images\" + image;
                                    }
                                    else
                                    {
                                        button.HiddenImage = "";
                                    }

                                    flowLayoutPanel1.Controls.Add(button);
                                    button.MouseDown += Button_MouseDown;
                                    button.MouseUp += Button_MouseUp;
                                    button.MouseEnter += Button_MouseEnter;
                                    button.MouseLeave += Button_MouseLeave;
                                    Bunifu.Framework.UI.BunifuElipse bunifuElipse = new Bunifu.Framework.UI.BunifuElipse();
                                    bunifuElipse.ApplyElipse(button, 15);
                                }


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
            }

        }

        private void bunifuTextBox1_TextChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Text Changed");
        }
    }
    //class DrawFLP : FlowLayoutPanel
    //{
    //    public DrawFLP() { DoubleBuffered = true; }
    //}

    public class myButton : Button
    {
        public string HiddenText { get; set; }

        public string HiddenImage { get; set; }

        public int HiddenId { get; set; }


    }
}
