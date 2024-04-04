using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MarinaCafeProject
{
    public partial class SaleMainScreen : Form
    {
        class DrawingControl
        {
            [DllImport("user32.dll")]
            public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

            private const int WM_SETREDRAW = 11;

            public static void SuspendDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
            }

            public static void ResumeDrawing(Control parent)
            {
                SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
                parent.Refresh();
            }
        }

        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");
        public int activeSession;
        public string activeTables;
        public SaleMainScreen()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.DarkBlue, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_CLIPCHILDREN
                return cp;
            }
        }
        private void SaleMainScreen_Load(object sender, EventArgs e)
        {
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetDoubleBuffered(flowLayoutPanel1);
            GetAreas();

        }

        private void GoFullScreen(bool fullscreen)
        {
            if (fullscreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
        private void GetAreas()
        {
            if (conn.State != ConnectionState.Open) conn.Open();
            try
            {
                DataTable dt = new DataTable();
                OleDbDataAdapter areas = new OleDbDataAdapter("SELECT * FROM cafe_table_area", conn);
                areas.Fill(dt);
                cb_area.ValueMember = "area_id";
                cb_area.DisplayMember = "area_name";
                cb_area.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dataRow = dt.Rows[i];
                        CafeArea area = new CafeArea();
                        area.AreaId = int.Parse(dataRow["area_id"].ToString());
                        area.AreaName = dataRow["area_name"].ToString();
                        area.TableCount = int.Parse(dataRow["area_table_count"].ToString());
                        cafeAreas.Add(area);
                    }

                    cb_area.SelectedIndex = 0;                    
                    SetTables();

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

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btn_end_ses_Click(object sender, EventArgs e)
        {
            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show("Satış oturumunu sonlandırmak istiyor musunuz ?", "Onay ?", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                try
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OleDbCommand cmd = new OleDbCommand("UPDATE sessions SET session_status=@session_status WHERE session_id=@session_id", conn);
                    cmd.Parameters.AddWithValue("@session_status", "deactive");
                    cmd.Parameters.AddWithValue("@session_id", activeSession);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Oturum başarı ile sonlandırıldı. Özet ekranı görüntüleniyor.");

                    SaleHistorySummaryScreen summary = new SaleHistorySummaryScreen();
                    summary.sessionId = activeSession;
                    summary.ShowDialog();

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

        }

        private void btn_new_payment_Click(object sender, EventArgs e)
        {
            NewSale newSale = new NewSale();
            newSale.activeSession = activeSession;
            newSale.ShowDialog();
        }

        private void btn_show_sum_Click(object sender, EventArgs e)
        {
            SaleHistorySummaryScreen summary = new SaleHistorySummaryScreen();
            summary.sessionId = activeSession;
            summary.ShowDialog();
        }

        private void cb_area_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTables();
        }
        List<CafeTable> cafeTables = new List<CafeTable>();
        List<CafeArea> cafeAreas = new List<CafeArea>();
        private void SetTables()
        {
            int empty = 0;
            int full = 1;
            int reserved = 2;

            int tableCount = 0;
            var area = cafeAreas.SingleOrDefault(x => x.AreaId == Convert.ToInt16(cb_area.SelectedValue));
            if (area != null)
            {
                tableCount = area.TableCount;
            }
            Console.WriteLine("Table Count : " + tableCount);

            if (tableCount > 0)
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                DataTable tableStatus = new DataTable();
                OleDbCommand command = new OleDbCommand("SELECT table_number, status FROM session_tables_status WHERE session_id=@session_id AND area_id=@area_id", conn);
                command.Parameters.AddWithValue("@session_id", activeSession);
                command.Parameters.AddWithValue("@area_id", cb_area.SelectedValue);
                tableStatus.Load(command.ExecuteReader());
                conn.Close();

                foreach (DataRow row in tableStatus.Rows)
                {
                    Console.WriteLine(row["table_number"].ToString() + " " + row["status"].ToString());
                }

                flowLayoutPanel1.Controls.Clear();
                flowLayoutPanel1.SuspendLayout();



                TableUC[] tables = new TableUC[tableCount + 1];
                for (int i = 1; i <= tableCount; i++)
                {

                    //MyTable table = new MyTable();
                    //table.Paint += Table_Paint;
                    //table.Size = new Size(170, 190);
                    //table.BackColor = SystemColors.Control;
                    //table.Text = "";
                    //table.FlatStyle = FlatStyle.Flat;
                    //table.FlatAppearance.BorderSize = 1;
                    //table.FlatAppearance.BorderColor = Color.Black;
                    //table.TableNumber = i;
                    //table.TableId = i;
                    //table.TableAreaId = Convert.ToInt16(cb_area.SelectedValue);

                    //table.Margin = new Padding(20);


                    tables[i] = new TableUC();

                    foreach (DataRow row in tableStatus.Rows)
                    {
                        if (i == Convert.ToInt16(row["table_number"].ToString()))
                        {
                            tables[i].Type = Convert.ToInt32(row["status"].ToString());

                            //table.TableStatus = Convert.ToInt32(row["status"].ToString());
                        }
                    }

                    //flowLayoutPanel1.Controls.Add(table);
                    tables[i].Id = i;
                    tables[i].AreaId = Convert.ToInt16(cb_area.SelectedValue);
                    tables[i].Margin = new Padding(20);
                    tables[i].Click += CafeSaleMainScreen_Click;

                    flowLayoutPanel1.Controls.Add(tables[i]);

                }
                flowLayoutPanel1.ResumeLayout();
            }
            
        }

        private void Table_Paint(object sender, PaintEventArgs e)
        {            
            PictureBox pic = new PictureBox();
            pic.Size = new Size(168, 168);            
            Image img;
            using (var bmpTemp = new Bitmap(Properties.Resources.table_emty))
            {
                img = new Bitmap(bmpTemp);
            }
            pic.Image = img;
            pic.BackColor = SystemColors.Control;
            pic.SizeMode = PictureBoxSizeMode.Zoom;
            var bm = new Bitmap(pic.ClientSize.Width, pic.ClientSize.Height);
            pic.DrawToBitmap(bm, pic.ClientRectangle);

            RectangleF srcRect = new RectangleF(0, 0, 168, 168);
            GraphicsUnit units = GraphicsUnit.Pixel;

            e.Graphics.DrawImage(bm, 1, 15, 168, 168);


            MyTable btn = (MyTable)sender;
            string drawString = btn.TableNumber.ToString();
            Font drawFont = new Font("NSimSun", 45);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            RectangleF drawRect = new RectangleF(33, 14, 104, 61);
            StringFormat drawFormat = new StringFormat();
            drawFormat.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(drawString, drawFont, drawBrush, drawRect, drawFormat);

        }

        private void CafeSaleMainScreen_Click(object sender, EventArgs e)
        {
            TableUC table = (TableUC)sender;
            using (TableOptions options = new TableOptions())
            {
                options.AreaId = Convert.ToInt16(cb_area.SelectedValue);
                options.AreaName = cb_area.Text;
                //options.TableCount = 
                options.AreaTableId = Convert.ToInt16(cb_area.SelectedValue);
                options.TableNumber = table.Id;
                options.TableType = table.Type;
                options.activeSession = activeSession;
                options.ShowDialog();

                SetTables();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }

    public class MyTable : Button
    {
        public int TableId { get; set; }
        public int TableNumber { get; set; }
        public int TableStatus { get; set; }
        public int TableAreaId { get; set; }
    }




}
