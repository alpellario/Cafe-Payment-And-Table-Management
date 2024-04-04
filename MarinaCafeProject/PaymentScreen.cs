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
    public partial class PaymentScreen : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public bool isForPartPayment = false;
        public int activeSession;
        double safeTotalAmount;
        public object list;
        bool discountExist = false;
        public PaymentScreen()
        {
            InitializeComponent();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<PaymentProduct> products;
        List<PaymentProduct> safeProducts;
        PaymentAmountDetail amountDetail = new PaymentAmountDetail();


        CafeProductComputation cafeProductComputation = new CafeProductComputation();
        private void PaymentScreen_Load(object sender, EventArgs e)
        {
            amountDetail.PaidCashAmount = 0;
            amountDetail.PaidCardAmount = 0;

            cb_cash.Checked = false;
            cb_card.Checked = false;
            lbl_received.Text = "0 ₺";


            tb_tip.Enabled = false;
            tb_discount.Enabled = false;

            products = (List<PaymentProduct>)list;
            safeProducts = new List<PaymentProduct>(products.Count);

            products.ForEach((item) =>
            {
                safeProducts.Add(item.Clone());
            });

            double totalAmount = 0;
            foreach (var x in products)
            {
                x.PrintProductInfo();
                totalAmount += cafeProductComputation.CalculateRemainingAmount(x.ProductCount, x.PaidProductQty, x.ProductPrice);
            }
            Console.WriteLine("Total Amount : " + totalAmount);
            lbl_total.Text = totalAmount.ToString() + " ₺";
            lbl_remain.Text = totalAmount.ToString() + " ₺";
            safeTotalAmount = totalAmount;
            if (isForPartPayment)
            {
                cb_cash.Enabled = false;
                cb_card.Enabled = false;
                cb_tip.Enabled = false;
                cb_discount.Enabled = false;
                bunifuLabel1.Text = "Parça Hesap Ödeme Ekranı";
                btn_get_paid.Text = "Parça Ödemeyi Al";
            }
        }
        private double GetRemainingAmount()
        {
            double remainingAmount = 0;
            foreach (var x in products)
            {
                //x.PrintProductInfo();
                remainingAmount += cafeProductComputation.CalculateRemainingAmount(x.ProductCount, x.PaidProductQty, x.ProductPrice);
            }
            return remainingAmount;
        }
        private double GetReceivedAmount()
        {
            double receivedAmount = 0;
            foreach (var x in products)
            {
                //x.PrintProductInfo();
                receivedAmount += cafeProductComputation.CalculateReceivedAmount(x.PaidProductQty, x.ProductPrice);
            }
            return receivedAmount;
        }
        private void bunifuTextBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cb_card_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
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

        bool cashPerm = true;
        bool cardPerm = true;
        bool amountIsRounded = false;
        double roundedAmount = 0;

        private void tb_cash_TextChanged(object sender, EventArgs e)
        {
            if (tb_cash.Text.StartsWith(",")) tb_cash.Text = "0";
            if (cashPerm)
            {
                cardPerm = false;

                double remainingAmount = 0;

                if (!amountIsRounded)
                    remainingAmount = GetRemainingAmount();
                else remainingAmount = roundedAmount;

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
                else
                {

                }

                cardPerm = true;
            }
        }

        private void tb_card_TextChanged(object sender, EventArgs e)
        {
            if (tb_card.Text.StartsWith(",")) tb_card.Text = "0";
            if (cardPerm)
            {
                cashPerm = false;

                double remainingAmount = 0;

                if (!amountIsRounded)
                    remainingAmount = GetRemainingAmount();
                else remainingAmount = roundedAmount;

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
                else
                {

                }

                cashPerm = true;
            }
        }

        private void setTotalAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cb_cash.Checked)
            {

                if (!amountIsRounded)
                {
                    double remainingAmount = GetRemainingAmount();
                    tb_cash.Text = Convert.ToDecimal(remainingAmount).ToString();
                }
                else
                {
                    tb_cash.Text = Convert.ToDecimal(roundedAmount).ToString();
                }
            }
        }

        private void setTotalAmountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (cb_card.Checked)
            {
                if (!amountIsRounded)
                {
                    double remainingAmount = GetRemainingAmount();
                    tb_card.Text = Convert.ToDecimal(remainingAmount).ToString();
                }
                else
                {
                    tb_card.Text = Convert.ToDecimal(roundedAmount).ToString();
                }
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

        private void cb_tip_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (cb_tip.Checked)
            {
                tb_tip.Enabled = true;
            }
            else
            {
                tb_tip.Enabled = false;
                tb_tip.Clear();
            }
        }

        bool existPartPayment = false;
        double discountedValue = 0;
        private void cb_discount_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            if (cb_discount.Checked)
            {
                tb_discount.Enabled = true;
                btn_disc.Visible = true;
            }
            else
            {
                if (discountExist)
                {
                    if (existPartPayment)
                    {
                        DialogResult dialog = new DialogResult();
                        dialog = MessageBox.Show("Eğer indirimi silerseniz ayrı ayrı ödeme işlemleri sıfırlanacaktır. Devam etmek istiyor munuz?", "Onay ?", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            btn_disc.Visible = false;
                            tb_discount.Enabled = false; ;
                            tb_discount.Clear();

                            products.Clear();

                            safeProducts.ForEach((item) =>
                            {
                                products.Add(item.Clone());
                            });

                            amountDetail.ClearAmount();

                            double remainingAmount = GetRemainingAmount();

                            lbl_total.Text = remainingAmount.ToString() + " ₺";

                            ClearAmount();

                            discountExist = false;
                            existPartPayment = false;
                            amountIsRounded = false;

                            lbl_total.ContextMenuStrip = contextMenuStrip3;

                        }
                        else
                        {
                            cb_discount.Checked = true;
                            tb_discount.Text = discountedValue.ToString();
                        }
                    }
                    else
                    {
                        btn_disc.Visible = false;
                        tb_discount.Enabled = false; ;
                        tb_discount.Clear();

                        foreach (var x in safeProducts)
                        {
                            products.Where(w => w.ProductId == x.ProductId).ToList().ForEach(p => p.ProductPrice = x.ProductPrice);
                            Console.WriteLine("X Price : " + x.ProductPrice);
                        }

                        double remainingAmount = GetRemainingAmount();

                        lbl_total.Text = remainingAmount.ToString() + " ₺";

                        amountIsRounded = false;
                        ClearAmount();

                        discountExist = false;


                    }
                }
                else
                {
                    btn_disc.Visible = false;
                    tb_discount.Enabled = false; ;
                    tb_discount.Clear();
                }
            }

        }
        private void ClearAmount()
        {
            cashPerm = false;
            cardPerm = false;

            tb_cash.Clear();
            tb_card.Clear();

            if (!amountIsRounded)
            {
                double remainingAmount = GetRemainingAmount();
                lbl_remain.Text = remainingAmount.ToString() + " ₺";
            }
            else
            {
                lbl_remain.Text = roundedAmount.ToString() + " ₺";
            }

            //lbl_received.Text = "0 ₺";

            cashPerm = true;
            cardPerm = true;
        }

        private void btn_disc_Click(object sender, EventArgs e)
        {
            if (!discountExist)
            {
                if (!existPartPayment)
                {
                    if (!string.IsNullOrEmpty(tb_discount.Text))
                    {
                        double discount = double.Parse(tb_discount.Text);
                        discountedValue = discount;//for rewrite

                        if (discount >= 0)
                        {
                            foreach (var x in products)
                            {
                                PaymentProduct temp = safeProducts.FirstOrDefault(p => p.ProductId == x.ProductId);
                                string _discounted = (temp.ProductPrice - cafeProductComputation.CalculateDiscount(temp.ProductPrice, discount)).ToString("0.##");
                                Console.WriteLine("Disc : " + _discounted);
                                x.ProductPrice = double.Parse(_discounted);
                            }
                            amountIsRounded = false;
                            double remainingAmount = GetRemainingAmount();

                            lbl_total.Text = remainingAmount.ToString() + " ₺";
                            lbl_remain.Text = remainingAmount.ToString() + " ₺";



                            ClearAmount();

                            discountExist = true;
                        }
                    }

                }
                else
                {
                    if (!string.IsNullOrEmpty(tb_discount.Text))
                    {
                        DialogResult dialog = new DialogResult();
                        dialog = MessageBox.Show("Eğer indirimi onaylarsanız, fıyatlar güncelleneceği için ayrı ayrı ödeme işlemi sıfırlanacaktır. Devam etmek istiyor musunuz ?", "Confirmation ?", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {

                            products.Clear();

                            safeProducts.ForEach((item) =>
                            {
                                products.Add(item.Clone());
                            });

                            lbl_total.ContextMenuStrip = contextMenuStrip3;

                            amountDetail.ClearAmount();

                            double discount = double.Parse(tb_discount.Text);
                            discountedValue = discount;//for rewrite

                            if (discount >= 0)
                            {
                                foreach (var x in products)
                                {
                                    PaymentProduct temp = safeProducts.FirstOrDefault(p => p.ProductId == x.ProductId);
                                    string _discounted = (temp.ProductPrice - cafeProductComputation.CalculateDiscount(temp.ProductPrice, discount)).ToString("0.##");
                                    Console.WriteLine("Disc : " + _discounted);
                                    x.ProductPrice = double.Parse(_discounted);
                                }
                                amountIsRounded = false;
                                double remainingAmount = GetRemainingAmount();

                                lbl_total.Text = remainingAmount.ToString() + " ₺";
                                lbl_remain.Text = remainingAmount.ToString() + " ₺";

                                ClearAmount();

                                discountExist = true;
                                existPartPayment = false;
                            }


                        }
                        else
                        {
                            cb_discount.Checked = false;

                        }
                    }
                }

            }
            else
            {
                MessageBox.Show("Zaten indirim yapıldı.");
                tb_discount.Text = discountedValue.ToString();
            }
        }

        private void tb_discount_TextChanged(object sender, EventArgs e)
        {
            if (tb_discount.Text.StartsWith(",")) tb_discount.Text = "";
            if (!string.IsNullOrEmpty(tb_discount.Text))
            {
                double discount = double.Parse(tb_discount.Text);
                if (discount > 100)
                {
                    tb_discount.Text = "100";
                }
                else if (discount < 0)
                {
                    tb_discount.Text = "0";
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (sender as ToolStripItem);
            if (item != null)
            {
                ContextMenuStrip owner = item.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    double remainingAmount = GetRemainingAmount();
                    if (Math.Floor(remainingAmount) != remainingAmount)
                    {
                        amountIsRounded = true;
                        roundedAmount = Math.Floor(remainingAmount);

                        lbl_total.Text = roundedAmount.ToString() + " ₺";
                        ClearAmount();
                    }
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (sender as ToolStripItem);
            if (item != null)
            {
                ContextMenuStrip owner = item.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    double remainingAmount = GetRemainingAmount();
                    if (Math.Ceiling(remainingAmount) != remainingAmount)
                    {
                        amountIsRounded = true;
                        roundedAmount = Math.Ceiling(remainingAmount);

                        lbl_total.Text = roundedAmount.ToString() + " ₺";
                        ClearAmount();
                    }

                }
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (sender as ToolStripItem);
            if (item != null)
            {
                ContextMenuStrip owner = item.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    double remainingAmount = GetRemainingAmount();

                    lbl_total.Text = remainingAmount.ToString() + " ₺";
                    lbl_remain.Text = remainingAmount.ToString() + " ₺";

                    amountIsRounded = false;
                    ClearAmount();

                }
            }
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            cb_card.Checked = false;
            cb_cash.Checked = false;
            cb_discount.Checked = false;
            cb_tip.Checked = false;
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            products.Clear();

            safeProducts.ForEach((item) =>
            {
                products.Add(item.Clone());
            });

            lbl_total.ContextMenuStrip = contextMenuStrip3;

            amountDetail.ClearAmount();

            double remainingAmount = GetRemainingAmount();
            lbl_total.Text = remainingAmount.ToString() + " ₺";
            lbl_remain.Text = remainingAmount.ToString() + " ₺";
            lbl_received.Text = "0 ₺";
            amountIsRounded = false;
            existPartPayment = false;
            discountExist = false;

            cb_card.Checked = false;
            cb_cash.Checked = false;
            cb_discount.Checked = false;
            cb_tip.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            amountDetail.PrintCashCardInfo();
        }

        private void tb_tip_TextChanged(object sender, EventArgs e)
        {
            if (tb_tip.Text.StartsWith(",")) tb_tip.Text = "";
        }

        private void cmsFloor_Click(object sender, EventArgs e)
        {
            ToolStripItem item = (sender as ToolStripItem);
            if (item != null)
            {
                ContextMenuStrip owner = item.Owner as ContextMenuStrip;
                if (owner != null)
                {
                    double remainingAmount = GetRemainingAmount();
                    if (Math.Floor(remainingAmount) != remainingAmount)
                    {
                        amountIsRounded = true;
                        roundedAmount = Math.Floor(remainingAmount);

                        lbl_remain.Text = roundedAmount.ToString() + " ₺";
                        ClearAmount();
                    }
                }
            }
        }
        public object retProducts;
        List<PaymentProduct> totalReturnedProducts = new List<PaymentProduct>();
        private void btn_part_payment_Click(object sender, EventArgs e)
        {
            int unPaidProductCount = 0;
            foreach (var x in products)
            {
                unPaidProductCount += (x.ProductCount - x.PaidProductQty);
            }
            if (unPaidProductCount > 0)
            {

                if (cb_cash.Checked || cb_card.Checked)
                {
                    cb_card.Checked = false;
                    cb_cash.Checked = false;
                    MessageBox.Show("Para girişleri sıfırlanacak. Eğer farklı miktarda para girişi yapıcaksanız, bunu 'Ayrı Ayrı Ödeme' işlemleri sonunda yapınız.");
                }

                if (amountIsRounded == true)
                {
                    double remainingAmount = GetRemainingAmount();
                    lbl_total.Text = remainingAmount.ToString() + " ₺";
                    lbl_remain.Text = remainingAmount.ToString() + " ₺";
                    amountIsRounded = false;
                    MessageBox.Show("Yuvarlanan değer orjinal haline döndürülüyor.");
                }

                PartPaymentScreen paymentPartScreen = new PartPaymentScreen();
                paymentPartScreen.list = products;
                paymentPartScreen.amountDetail = amountDetail;
                paymentPartScreen.discountExist = discountExist;
                paymentPartScreen.ShowDialog();
                bool isReceived = paymentPartScreen.isReceived;
                if (isReceived)
                {
                    List<PaymentProduct> returnedProducts = (List<PaymentProduct>)paymentPartScreen.returnedList;
                    
                    Console.WriteLine("06/10/2022");
                    if (returnedProducts != null)
                    {
                        foreach (var returnedProduct in returnedProducts)
                        {
                            PaymentProduct x = totalReturnedProducts.FirstOrDefault(s => s.ProductId == returnedProduct.ProductId);
                            if(x == null)
                            {
                                totalReturnedProducts.Add(returnedProduct);
                            }
                            else
                            {
                                totalReturnedProducts.Where(w => w.ProductId == returnedProduct.ProductId)
                                    .ToList().ForEach(p => { p.ProductCount += returnedProduct.ProductCount; p.PaidProductQty += returnedProduct.PaidProductQty; });
                            }
                            //returnedProduct.PrintProductInfo();
                            products.Where(w => w.ProductId == returnedProduct.ProductId)
                                .ToList().ForEach(p => p.PaidProductQty += returnedProduct.PaidProductQty);
                            returnedProduct.PrintProductInfo();
                        }
                        //totalAmount = totalAmount - paymentPartScreen.totalAmount;

                        

                        Console.WriteLine("/n");
                        foreach (var x in totalReturnedProducts)
                        {
                            x.PrintProductInfo();
                        }
                        Console.WriteLine("+++++++++++");

                        double remainingAmount = GetRemainingAmount();
                        double receivedAmount = GetReceivedAmount();
                        lbl_received.Text = receivedAmount.ToString() + " ₺";
                        lbl_remain.Text = remainingAmount.ToString() + " ₺";
                        existPartPayment = true;
                        lbl_total.ContextMenuStrip = null;
                        //lbl_remain.ContextMenuStrip = contextMenuStrip5;

                        foreach(var x in products)
                        {
                            x.PrintProductInfo();
                        }
                    }
                    else MessageBox.Show("Alınan ürün bulunamadı.");
                }
                paymentPartScreen = null;
            }
            else MessageBox.Show("Tüm ürünler zaten ödendi.");
        }

        private void btn_get_paid_Click(object sender, EventArgs e)
        {
            int unPaidProductCount = 0;
            foreach (var x in products)
            {
                unPaidProductCount += (x.ProductCount - x.PaidProductQty);
            }

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

            double remainAmount = 0;
            if (!amountIsRounded)
                remainAmount = GetRemainingAmount();
            else remainAmount = roundedAmount;

            Console.WriteLine("Remain Amount : " + remainAmount.ToString());
            Console.WriteLine("Received Amount : " + receivedAmount.ToString());

            if (!isForPartPayment)
            {
                if (Convert.ToDecimal(remainAmount) - Convert.ToDecimal(receivedAmount) == 0)
                {
                    amountDetail.PaidCardAmount += card;
                    amountDetail.PaidCashAmount += cash;

                    double partReceived = GetReceivedAmount();
                    decimal totalDec = Convert.ToDecimal(partReceived) + Convert.ToDecimal(receivedAmount);
                    double totalDouble = Convert.ToDouble(totalDec);
                    try
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        OleDbCommand command = new OleDbCommand("INSERT INTO session_sale (session_id, total_amount, cash_amount, card_amount, tip_amount, discount_amount) " +
                                                                "VALUES (@session_id, @total_amount, @cash_amount, @card_amount, @tip_amount, @discount_amount)", conn);
                        command.Parameters.AddWithValue("@session_id", activeSession);
                        command.Parameters.AddWithValue("@total_amount", totalDouble.ToString("0.##"));
                        command.Parameters.AddWithValue("@cash_amount", amountDetail.PaidCashAmount.ToString("0.##"));
                        command.Parameters.AddWithValue("@card_amount", amountDetail.PaidCardAmount.ToString("0.##"));

                        if (!string.IsNullOrEmpty(tb_tip.Text))
                            command.Parameters.AddWithValue("@tip_amount", double.Parse(tb_tip.Text).ToString("0.##"));
                        else command.Parameters.AddWithValue("@tip_amount", 0);

                        if (!string.IsNullOrEmpty(tb_discount.Text))
                            command.Parameters.AddWithValue("@discount_amount", double.Parse(tb_discount.Text).ToString("0.##"));
                        else command.Parameters.AddWithValue("@discount_amount", 0);
                        command.ExecuteNonQuery();

                        command.CommandText = "SELECT @@IDENTITY";
                        int returnedId = Convert.ToInt32(command.ExecuteScalar().ToString());

                        foreach (var product in products)
                        {
                            command = new OleDbCommand("INSERT INTO session_sale_detail (session_sale_id, product_id, product_sale_price, product_sale_count, product_total_price) VALUES (@session_sale_id, @product_id, @product_sale_price, @product_sale_count, @product_total_price)", conn);
                            command.Parameters.AddWithValue("@session_sale_id", returnedId);
                            command.Parameters.AddWithValue("@product_id", product.ProductId);
                            command.Parameters.AddWithValue("@product_sale_price", product.ProductPrice);
                            command.Parameters.AddWithValue("@product_sale_count", product.ProductCount);
                            command.Parameters.AddWithValue("@product_total_price", product.ProductPrice * product.ProductCount);
                            command.ExecuteNonQuery();
                        }
                        MessageBox.Show("Ödeme başarıyla alındı.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                        this.Close();
                    }

                }
                else MessageBox.Show("Toplam miktar alınmadı.");
            }
            else
            {
                if (Convert.ToDecimal(safeTotalAmount) > Convert.ToDecimal(remainAmount))
                {
                    retProducts = totalReturnedProducts;
                    this.Close();
                }
                else MessageBox.Show("Ücreti alınan bir ürün yok.");
            }
            
        }
    }
}
