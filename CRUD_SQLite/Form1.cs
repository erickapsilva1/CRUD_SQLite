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
            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Student _student = null;
                Data_Maintanence dm = new Data_Maintanence(_student);
                dm.Show();
                LoadData();
            }catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Data_Maintanence dm = new Data_Maintanence(GetGridData());
                dm.Show();
                LoadData();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private Student GetGridData()
        {
            try
            {
                int line;
                line = dgvStudents.CurrentRow.Index;
                Student student = new Student();
                student.id = Convert.ToInt32(dgvStudents[0, line].Value);
                student.name = dgvStudents[1, line].Value.ToString();
                student.email = dgvStudents[2, line].Value.ToString();
                student.age = Convert.ToInt32(dgvStudents[3, line].Value);
                return student;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void LoadData()
        {
            dgvStudents.DataSource = ReadData3<SQLiteConnection, SQLiteDataAdapter>("SELECT * FROM STUDENT");
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult response = MessageBox.Show("Do you want to delete this registration? ", "Delete line", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                if (response == DialogResult.Yes)
                {
                    DeleteData(GetGridData());
                    LoadData();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public int DeleteData(Student student)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "DELETE FROM STUDENT WHERE ID = @id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", student.id);
                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            return result;
        }

        private void txtFindByName_TextChanged(object sender, EventArgs e)
        {
            dgvStudents.DataSource = SearchData();
        }

        private DataTable SearchData()
        {
            string sql = "SELECT ID, NAME, EMAIL, AGE FROM STUDENT WHERE NAME LIKE '%" + txtFindByName.Text + "%'";
            using(SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using(SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            dgvStudents.DataSource = SearchData();
        }

    }
}