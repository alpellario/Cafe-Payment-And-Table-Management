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
    public partial class PartPaymentScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public object list;
        public object amountDetail;
        public bool discountExist;
        public PartPaymentScreen()
        {
            InitializeComponent();
        }
        CafeProductComputation computation = new CafeProductComputation();

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<PaymentProduct> products = new List<PaymentProduct>();
        List<PaymentProduct> safePaymentList = new List<PaymentProduct>();
        PaymentAmountDetail amountDet = new PaymentAmountDetail();

        private void PartPaymentScreen_Load(object sender, EventArgs e)
        {
            cb_card.Checked = false;
            cb_cash.Checked = false;
            tb_cash.Enabled = false;
            tb_card.Enabled = false;

            amountDet = (PaymentAmountDetail)amountDetail;
            safePaymentList = (List<PaymentProduct>)list;

            foreach (var x in safePaymentList)
            {
                if (x.ProductCount - x.PaidProductQty != 0)
                {

                    PaymentProduct product = new PaymentProduct();
                    product.ProductId = x.ProductId;
                    product.ProductPrice = x.ProductPrice;
                    product.PaidProductQty = 0;
                    product.ProductCount = 0;
                    products.Add(product);
                }
            }

            GetRemainProducts();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            this.dataGridView1.DoubleBuffered(true);

            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Product Code";
            dataGridView1.Columns[2].HeaderText = "Product Name";
            if (discountExist)
            {
                dataGridView1.Columns[3].HeaderText = "Product Price**";
            }
            else dataGridView1.Columns[3].HeaderText = "Product Price";
            dataGridView1.Columns[4].HeaderText = "Quantity";

            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[2].Width = 300;
            dataGridView1.Columns[3].Width = 120;
            dataGridView1.Columns[4].Width = 120;

            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                //if (i != 2)
                //{
                this.dataGridView1.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                this.dataGridView1.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                //}


            }

            dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnHeadersHeight = 40;
            this.dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(54, 35, 35);
            this.dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            this.dataGridView1.DefaultCellStyle.Font = new Font("Calibri", 15);

            dataGridView1.RowsAdded += DataGridView1_RowsAdded;
        }
        private void DataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].Height = 30;
        }

        private void GetRemainProducts()
        {
            flowLayoutPanel1.Controls.Clear();

            foreach (var product in products)
            {

                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    DataTable dataTable = new DataTable();
                    OleDbCommand command = new OleDbCommand("SELECT cafe_product_id, cafe_product_name, cafe_product_image FROM cafe_product WHERE cafe_product_id=@cafe_product_id ORDER BY cafe_product_name", conn);
                    command.Parameters.AddWithValue("@cafe_product_id", product.ProductId);
                    OleDbDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {

                        DataRow dataRow = dataTable.Rows[0];

                        SaleProduct saleProduct = new SaleProduct();
                        saleProduct.Name = product.ProductId.ToString();
                        saleProduct.ProductName = dataRow["cafe_product_name"].ToString();
                        saleProduct.Id = int.Parse(dataRow["cafe_product_id"].ToString());
                        saleProduct.Label.Name = dataRow["cafe_product_id"].ToString();

                        string image = dataRow["cafe_product_image"].ToString();

                        if (!string.IsNullOrWhiteSpace(image))
                        {
                            string imagePath = Application.StartupPath + @"\images\" + image;
                            Image img;
                            using (var bmpTemp = new Bitmap(imagePath))
                            {
                                img = new Bitmap(bmpTemp);
                            }
                            saleProduct.ProductImage = img;
                        }

                        flowLayoutPanel1.Controls.Add(saleProduct);

                        saleProduct.MouseDown += NewSale_Click1;
                        saleProduct.Label.MouseDown += Label_Click;


                        if (flowLayoutPanel1.Controls.Count > 5)
                            panel2.Size = new Size(385, 660);
                        else panel2.Size = new Size(366, 660);
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
        private void Label_Click(object sender, EventArgs e)
        {
            Bunifu.UI.WinForms.BunifuLabel label = (Bunifu.UI.WinForms.BunifuLabel)sender;
            Console.WriteLine(label.Name + " eklendi");
            AddProduct(int.Parse(label.Name));

            //PaymentProduct product = products.Find(p => p.ProductId == int.Parse(label.Name));
            //product.PrintProductInfo();

        }
        private void NewSale_Click1(object sender, EventArgs e)
        {
            SaleProduct obj = (SaleProduct)sender;
            Console.WriteLine(obj.Id + " eklendi");
            AddProduct(obj.Id);
            //PaymentProduct product = products.Find(p => p.ProductId == obj.Id);
            //product.PrintProductInfo();
        }
        private void AddProduct(int productId)
        {
            bool isExist = false;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value.ToString() == productId.ToString())
                {
                    products.Where(w => w.ProductId == productId).ToList().ForEach(p => p.ProductCount += 1);
                    products.Where(w => w.ProductId == productId).ToList().ForEach(p => p.PaidProductQty += 1);

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
                        PaymentProduct x = safePaymentList.FirstOrDefault(p => p.ProductId == productId);
                        DataRow dataRow = dt.Rows[0];
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridView1);
                        row.Cells[0].Value = productId.ToString();
                        row.Cells[1].Value = dataRow["cafe_product_code"].ToString();
                        row.Cells[2].Value = dataRow["cafe_product_name"].ToString();
                        row.Cells[3].Value = dataRow["cafe_product_price"].ToString() + " ₺";
                        row.Cells[3].Value = x.ProductPrice + " ₺";
                        row.Cells[4].Value = 1;
                        dataGridView1.Rows.Add(row);

                        products.Where(w => w.ProductId == productId).ToList().ForEach(p => p.ProductCount += 1);
                        products.Where(w => w.ProductId == productId).ToList().ForEach(p => p.PaidProductQty += 1);

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

            foreach (var safeProduct in safePaymentList)
            {
                if (safeProduct.ProductId == productId)
                {
                    PaymentProduct product = products.Find(p => p.ProductId == productId);
                    if ((safeProduct.ProductCount - safeProduct.PaidProductQty) == product.ProductCount)
                    {
                        Control c = flowLayoutPanel1.Controls[product.ProductId.ToString()];
                        c.Dispose();
                    }
                }
            }

            if (flowLayoutPanel1.Controls.Count > 5)
                panel2.Size = new Size(385, 660);
            else panel2.Size = new Size(366, 660);


        }

        private void CalculateTotalPrice()
        {
            CafeProductComputation computation = new CafeProductComputation();

            double totalAmount = 0;
            foreach (var x in products)
            {
                totalAmount += computation.CalculateRemainingAmount(x.ProductCount, 0, x.ProductPrice);
            }
            double totalPrice = 0;
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    string[] priceString = row.Cells[3].Value.ToString().Split(' ');
            //    double price = double.Parse(priceString[0]);
            //    totalPrice += (price * int.Parse(row.Cells[4].Value.ToString()));
            //}
            lbl_total.Text = totalAmount.ToString() + " ₺";
            //totalAmount = totalPrice;
            ClearAmount();
        }
        private void ClearAmount()
        {
            cashPerm = false;
            cardPerm = false;

            tb_cash.Clear();
            tb_card.Clear();

            cashPerm = true;
            cardPerm = true;
        }

        private double GetRemainingAmount()
        {
            double remainingAmount = 0;
            foreach (var x in products)
            {
                x.PrintProductInfo();
                remainingAmount += computation.CalculateRemainingAmount(x.ProductCount, 0, x.ProductPrice);
            }
            return remainingAmount;
        }

        private void tb_cash_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != ',')
            {
                e.Handled = true;
            }
        }

        private void cb_cash_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (cb_cash.Checked)
                {
                    tb_cash.Enabled = true;

                    if (cb_card.Checked)
                    {
                        if (!string.IsNullOrEmpty(tb_card.Text))
                        {
                            double cardAmount = double.Parse(tb_card.Text);

                            double remainingAmount = GetRemainingAmount();

                            tb_cash.Text = (Convert.ToDecimal(remainingAmount) - Convert.ToDecimal(cardAmount)).ToString();
                        }
                    }
                }
                else
                {
                    tb_cash.Enabled = false;
                    tb_cash.Clear();
                }
            }
            else if (cb_cash.Checked)
            {
                MessageBox.Show("Please select product.");
                tb_cash.Enabled = false;
                cb_cash.Checked = false;
            }
        }

        private void cb_card_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (cb_card.Checked)
                {
                    tb_card.Enabled = true;

                    if (cb_cash.Checked)
                    {
                        if (!string.IsNullOrEmpty(tb_cash.Text))
                        {
                            double cashAmount = double.Parse(tb_cash.Text);

                            double remainingAmount = GetRemainingAmount();

                            tb_card.Text = (Convert.ToDecimal(remainingAmount) - Convert.ToDecimal(cashAmount)).ToString();
                        }
                    }

                }
                else
                {
                    tb_card.Enabled = false;
                    tb_card.Clear();
                }
            }
            else if (cb_card.Checked)
            {
                MessageBox.Show("Please select product.");
                tb_card.Enabled = false;
                cb_card.Checked = false;
            }
        }

        bool cashPerm = true;

        private void tb_cash_TextChanged(object sender, EventArgs e)
        {
            if (tb_cash.Text.StartsWith(",")) tb_cash.Text = "0";
            if (cashPerm)
            {
                cardPerm = false;

                double remainingAmount = GetRemainingAmount();

                double cash = 0;
                if (!string.IsNullOrEmpty(tb_cash.Text))
                {
                    cash = Convert.ToDouble(tb_cash.Text);
                }

                if (cash > remainingAmount)
                {
                    tb_cash.Text = remainingAmount.ToString();
                    cash = remainingAmount;
                }
                else if (cash < 0)
                {
                    tb_cash.Text = "0";
                    cash = 0;
                }

                if (cb_card.Checked)
                {
                    decimal cardAmount = Convert.ToDecimal(remainingAmount) - Convert.ToDecimal(cash);
                    tb_card.Text = cardAmount.ToString();
                }


                cardPerm = true;
            }
        }

        bool cardPerm = true;

        private void tb_card_TextChanged(object sender, EventArgs e)
        {
            if (tb_card.Text.StartsWith(",")) tb_card.Text = "0";
            if (cardPerm)
            {
                cashPerm = false;

                double remainingAmount = GetRemainingAmount();

                double card = 0;
                if (!string.IsNullOrEmpty(tb_card.Text))
                {
                    card = Convert.ToDouble(tb_card.Text);
                }

                if (card > remainingAmount)
                {
                    tb_card.Text = remainingAmount.ToString();
                    card = remainingAmount;
                }
                else if (card < 0)
                {
                    tb_card.Text = "0";
                    card = 0;
                }


                if (cb_cash.Checked)
                {
                    decimal cashAmount = Convert.ToDecimal(remainingAmount) - Convert.ToDecimal(card);
                    tb_cash.Text = cashAmount.ToString();
                }


                cashPerm = true;
            }
        }

        private void setTotalAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cb_cash.Checked)
            {
                double remainingAmount = GetRemainingAmount();
                tb_cash.Text = Convert.ToDecimal(remainingAmount).ToString();
            }
        }

        private void moneyChangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cb_cash.Checked)
            {

                if (!string.IsNullOrEmpty(tb_cash.Text))
                {
                    MoneyChange change = new MoneyChange();
                    change.enteredAmount = double.Parse(tb_cash.Text);
                    change.ShowDialog();
                }
            }
        }

        private void setTotalAmountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (cb_card.Checked)
            {
                //ToolStripItem item = (sender as ToolStripItem);
                //if (item != null)
                //{
                //    ContextMenuStrip owner = item.Owner as ContextMenuStrip;
                //    if (owner != null)
                //    {
                //        double remainingAmount = GetRemainingAmount();

                //        owner.SourceControl.Text = remainingAmount.ToString();
                //    }
                //}
                double remainingAmount = GetRemainingAmount();
                tb_card.Text = Convert.ToDecimal(remainingAmount).ToString();
            }
        }

        private void resetTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearAmount();
            dataGridView1.Rows.Clear();
            foreach (var product in products)
            {
                products.Where(w => w.ProductId == product.ProductId).ToList().ForEach(p => p.ProductCount = 0);
                products.Where(w => w.ProductId == product.ProductId).ToList().ForEach(p => p.PaidProductQty = 0);
            }
            GetRemainProducts();
            tb_search.Clear();
            if (dataGridView1.Rows.Count == 0)
            {
                cb_card.Checked = false;
                cb_cash.Checked = false;
                tb_card.Enabled = false;
                tb_cash.Enabled = false;
            }

            lbl_total.Text = "0 ₺";
        }

        private void tb_search_TextChanged(object sender, EventArgs e)
        {
            string compareTo = tb_search.Text.Trim().ToLower();
            foreach (Control c in flowLayoutPanel1.Controls)
            {
                if (c.GetType() == typeof(SaleProduct))
                {
                    SaleProduct saleProduct = (SaleProduct)c;
                    c.Visible = saleProduct.ProductName.ToLower().Contains(compareTo);
                    Console.WriteLine(saleProduct.ProductName);
                }

            }
        }

        public object returnedList;
        public bool isReceived = false;

        private void btn_get_paid_Click(object sender, EventArgs e)
        {
            double receivedAmount = 0;
            double card = 0;
            double cash = 0;

            if (cb_card.Checked && cb_cash.Checked)
            {
                if (!string.IsNullOrEmpty(tb_cash.Text) && !string.IsNullOrEmpty(tb_card.Text))
                {
                    card = double.Parse(tb_card.Text);
                    cash = double.Parse(tb_cash.Text);
                    receivedAmount = card + cash;
                }
            }
            else if (cb_card.Checked)
            {
                if (!string.IsNullOrEmpty(tb_card.Text))
                {
                    card = double.Parse(tb_card.Text);
                    receivedAmount = card;
                }
            }
            else if (cb_cash.Checked)
            {
                if (!string.IsNullOrEmpty(tb_cash.Text))
                {
                    cash = double.Parse(tb_cash.Text);
                    receivedAmount = cash;
                }
            }

            double totalAmount = GetRemainingAmount();
            Console.WriteLine("Total Amount : " + totalAmount.ToString());
            Console.WriteLine("Received Amount : " + receivedAmount.ToString());

            if (Convert.ToDecimal(receivedAmount) == Convert.ToDecimal(totalAmount))
            {
                amountDet.PaidCardAmount += card;
                amountDet.PaidCashAmount += cash;

                List<PaymentProduct> receivedProducts = new List<PaymentProduct>();

                foreach (var x in products)
                {
                    if (x.ProductCount != 0)
                    {
                        receivedProducts.Add(x);
                    }
                }
                returnedList = receivedProducts;
                isReceived = true;
                this.Close();
            }
            else MessageBox.Show("Toplam miktar alınmadı.");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                int currentQuantity = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                int currentId = int.Parse(dataGridView1.CurrentRow.Cells[0].Value.ToString());

                PaymentProduct product = safePaymentList.Find(p => p.ProductId == currentId);
                if (product != null)
                {
                    int safeProductCount = product.ProductCount - product.PaidProductQty;

                    NewSaleChangeQuantity quantity = new NewSaleChangeQuantity();
                    quantity.quantity = int.Parse(dataGridView1.CurrentRow.Cells[4].Value.ToString());
                    quantity.isPartPayment = true;
                    quantity.maxValue = safeProductCount;
                    quantity.ShowDialog();
                    int newQuantity = quantity.newQuantity;
                    if (newQuantity != currentQuantity)
                    {
                        if (newQuantity == product.ProductCount)
                        {
                            products.Where(w => w.ProductId == currentId).ToList().ForEach(p => p.ProductCount = newQuantity);
                            products.Where(w => w.ProductId == currentId).ToList().ForEach(p => p.PaidProductQty = newQuantity);
                            dataGridView1.CurrentRow.Cells[4].Value = newQuantity;

                            Control c = flowLayoutPanel1.Controls[product.ProductId.ToString()];
                            c.Dispose();

                            if (flowLayoutPanel1.Controls.Count > 5)
                                panel2.Size = new Size(385, 660);
                            else panel2.Size = new Size(366, 660);

                        }
                        else if (newQuantity != 0)
                        {
                            products.Where(w => w.ProductId == currentId).ToList().ForEach(p => p.ProductCount = newQuantity);
                            products.Where(w => w.ProductId == currentId).ToList().ForEach(p => p.PaidProductQty = newQuantity);
                            dataGridView1.CurrentRow.Cells[4].Value = newQuantity;
                            GetRemainSingleProduct(currentId);
                        }
                        else
                        {
                            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                            {
                                dataGridView1.Rows.RemoveAt(item.Index);
                            }
                            products.Where(w => w.ProductId == currentId).ToList().ForEach(p => p.ProductCount = 0);
                            products.Where(w => w.ProductId == currentId).ToList().ForEach(p => p.PaidProductQty = 0);
                            GetRemainSingleProduct(currentId);

                            if (dataGridView1.Rows.Count == 0)
                            {
                                ClearAmount();
                                cb_card.Checked = false;
                                cb_cash.Checked = false;
                                tb_card.Enabled = false;
                                tb_cash.Enabled = false;
                            }
                        }
                    }
                    CalculateTotalPrice();
                }
            }
        }

        private void GetRemainSingleProduct(int productId)
        {
            try
            {

                Control c = flowLayoutPanel1.Controls[productId.ToString()];
                if (c == null)
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    DataTable dataTable = new DataTable();
                    OleDbCommand command = new OleDbCommand("SELECT cafe_product_id, cafe_product_name, cafe_product_image FROM cafe_product WHERE cafe_product_id=@cafe_product_id ORDER BY cafe_product_name", conn);
                    command.Parameters.AddWithValue("@cafe_product_id", productId);
                    OleDbDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);
                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow dataRow = dataTable.Rows[0];

                        SaleProduct saleProduct = new SaleProduct();
                        saleProduct.Name = productId.ToString();
                        saleProduct.ProductName = dataRow["cafe_product_name"].ToString();
                        saleProduct.Id = int.Parse(dataRow["cafe_product_id"].ToString());
                        saleProduct.Label.Name = dataRow["cafe_product_id"].ToString();

                        string image = dataRow["cafe_product_image"].ToString();

                        if (!string.IsNullOrWhiteSpace(image))
                        {
                            string imagePath = Application.StartupPath + @"\images\" + image;
                            Image img;
                            using (var bmpTemp = new Bitmap(imagePath))
                            {
                                img = new Bitmap(bmpTemp);
                            }
                            saleProduct.ProductImage = img;
                        }

                        flowLayoutPanel1.Controls.Add(saleProduct);

                        if (flowLayoutPanel1.Controls.Count > 5)
                        {
                            panel2.Size = new Size(385, 660);
                        }
                        else panel2.Size = new Size(366, 660);

                        saleProduct.MouseDown += NewSale_Click1;
                        saleProduct.Label.MouseDown += Label_Click;
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
