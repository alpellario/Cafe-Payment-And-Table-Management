using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarinaCafeProject
{

    public partial class Launcher : Form
    {
        OleDbConnection conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\marina_database.mdb;");

        public Launcher()
        {
            InitializeComponent();

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_product_settings_Click(object sender, EventArgs e)
        {
            ProductSettingScreen screen = new ProductSettingScreen();
            screen.ShowDialog();
        }

        private void bunifuImageButton1_MouseEnter(object sender, EventArgs e)
        {
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            CheckSession();
        }

        bool sessionExist = false;
        int foundSessionId = 0;
        private void CheckSession()
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                DataTable sessionTable = new DataTable();
                OleDbCommand command = new OleDbCommand("SELECT session_id, session_start_date, session_status  FROM sessions WHERE session_status=@session_status", conn);
                command.Parameters.AddWithValue("@session_status", "active");
                OleDbDataReader sessionReader = command.ExecuteReader();
                sessionTable.Load(sessionReader);
                if (sessionTable.Rows.Count > 0)
                {
                    DataRow row = sessionTable.Rows[0];
                    Console.WriteLine("Found session_id : " + row["session_id"].ToString() + ", session_start_date : " + row["session_start_date"].ToString() + ", session_status : " + row["session_status"].ToString());

                    btn_session.BackColor = Color.FromArgb(255, 255, 255);
                    btn_session.Image = Properties.Resources.con_session_soft;
                    foundSessionId = int.Parse(row["session_id"].ToString());
                    sessionExist = true;
                }
                else
                {

                    btn_session.BackColor = Color.FromArgb(128, 0, 0);
                    btn_session.Image = Properties.Resources.start_session_soft;
                    sessionExist = false;
                    Console.WriteLine("Not found active session");
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

        private void btn_session_Click(object sender, EventArgs e)
        {
            if (sessionExist)
            {
                SaleMainScreen cafeSaleMainScreen = new SaleMainScreen();
                cafeSaleMainScreen.activeSession = foundSessionId;
                cafeSaleMainScreen.ShowDialog();
            }
            else
            {

                DialogResult dialog = new DialogResult();
                dialog = MessageBox.Show("Yeni satış oturumu başlatılacak, Onaylıyor musunuz ?", "Onay ?", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (conn.State != ConnectionState.Open) conn.Open();
                    OleDbCommand command = new OleDbCommand("INSERT INTO sessions (session_start_date, session_status) VALUES (@session_start_date, @session_status)", conn);
                    command.Parameters.AddWithValue("@session_start_date", DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                    command.Parameters.AddWithValue("@session_status", "active");
                    command.ExecuteNonQuery();


                    command.CommandText = "SELECT @@IDENTITY";
                    int returnedId = Convert.ToInt32(command.ExecuteScalar().ToString());

                    DataTable table_info = new DataTable();
                    OleDbCommand command2 = new OleDbCommand("SELECT area_id, area_table_count FROM cafe_table_area", conn);
                    table_info.Load(command2.ExecuteReader());
                    foreach (DataRow row in table_info.Rows)
                    {
                        int area_id = Convert.ToInt32(row["area_id"].ToString());
                        int table_count = Convert.ToInt32(row["area_table_count"].ToString());
                        for (int i = 1; i <= table_count; i++)
                        {
                            OleDbCommand create_table_status = new OleDbCommand("INSERT INTO session_tables_status (session_id, area_id, table_number, status) VALUES (@session_id, @area_id, @table_number, @status)", conn);
                            create_table_status.Parameters.AddWithValue("@session_id", returnedId);
                            create_table_status.Parameters.AddWithValue("@area_id", area_id);
                            create_table_status.Parameters.AddWithValue("@table_number", i);
                            create_table_status.Parameters.AddWithValue("@status", 0);
                            create_table_status.ExecuteNonQuery();
                        }

                    }
                    conn.Close();

                    //string datetime = DateTime.Now.ToString("dd_MM_yyyy");
                    //int version = 1;
                    //bool check = true;
                    //while (check)
                    //{
                    //    bool exist = DbConnectionExtensions.TableExists(conn, datetime + "_tables");
                    //    if (exist)
                    //    {
                    //        version++;
                    //        datetime = datetime.Substring(0,10) + "_v" + version;
                    //    }
                    //    else
                    //    {
                    //        break;
                    //    }
                    //}


                    //OleDbCommand command_create_table = new OleDbCommand("CREATE TABLE " + datetime.ToString() + "_tables (" +
                    //                                        "[order_id] AUTOINCREMENT NOT NULL PRIMARY KEY," +
                    //                                        "[table] VARCHAR(20) NOT NULL," +
                    //                                        "[product_id] INT NOT NULL," +
                    //                                        "[quantity] INT " +
                    //                                        ")", conn);
                    //command_create_table.ExecuteNonQuery();

                    SaleMainScreen cafeSaleMainScreen = new SaleMainScreen();
                    //cafeSaleMainScreen.activeTables = datetime + "_tables";
                    cafeSaleMainScreen.activeSession = returnedId;
                    cafeSaleMainScreen.ShowDialog();
                }
                CheckSession();
            }
            CheckSession();

        }

        private void btn_sale_history_Click(object sender, EventArgs e)
        {
            CafeSaleHistoryScreen hist = new CafeSaleHistoryScreen();
            hist.ShowDialog();

        }

        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            InfoScreen infoScreen = new InfoScreen();
            infoScreen.ShowDialog();
        }


    }
    public static class DbConnectionExtensions
    {
        public static bool TableExists(this OleDbConnection conn, string table)
        {
            var exists = conn.GetSchema("Tables", new string[4] { null, null, table, "TABLE" }).Rows.Count > 0;
            return exists;
        }
    }

}
