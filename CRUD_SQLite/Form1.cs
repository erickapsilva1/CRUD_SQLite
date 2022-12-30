using System.Data;
using System.Data.SQLite;

namespace CRUD_SQLite
{
    public partial class Form1 : Form
    {
        string _connectionString = @"Data Source=C:\temp\Cadastro.s3db.db";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvStudents.DataSource = ReadData3<SQLiteConnection, SQLiteDataAdapter>("SELECT * FROM STUDENT");
        }

        private void ReadData()
        {

            // Non-Pragmactic Read Data Method

            DataTable dt = new DataTable();
            SQLiteConnection conn = null;

            String sql = "SELECT * FROM STUDENT";
            String strConn = @"Data Source=C:\temp\Cadastro.s3db.db";

            try
            {
                conn = new SQLiteConnection(strConn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(sql, strConn);
                da.Fill(dt);
                dgvStudents.DataSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (conn.State != ConnectionState.Open) 
                { 
                    conn.Close();
                }
            }
        }

        public DataTable ReadData3<S, T>(String query) where S : IDbConnection, new()
            where T: IDbDataAdapter, IDisposable, new()
        {
            // Pragmactic Read Data Method (by using generics)

            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = _connectionString;
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }


    }
}