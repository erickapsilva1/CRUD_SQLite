using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_SQLite
{
    public partial class Data_Maintanence : Form
    {
        Student _student;
        string option = "";
        string _connectionString = @"Data Source=C:\Temp\Cadastro.s3db.db";

        public Data_Maintanence(Student student)
        {
            InitializeComponent();
            _student = student;
        }

        private void Data_Maintanence_Load(object sender, EventArgs e)
        {
            if (_student == null)
            {
                txtName.Focus();
                option = "Insert";
            }
            else
            {
                option = "Change";
                txtName.Text = _student.name;
                txtEmail.Text = _student.email;
                txtAge.Text = _student.age.ToString();
            }
        }

        private int InsertData(Student student)
        {
            int result = -1;
            using(SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using(SQLiteCommand cmd = new SQLiteCommand(conn)) {
                    cmd.CommandText = "INSERT INTO STUDENT(NAME, EMAIL, AGE) VALUES (@name, @email, @age)";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@name", student.name);
                    cmd.Parameters.AddWithValue("@email", student.email);
                    cmd.Parameters.AddWithValue("@age", student.age);
                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
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

        private int UpdateData(Student student)
        {
            int result = -1;
            using (SQLiteConnection conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(conn))
                {
                    cmd.CommandText = "UPDATE STUDENT SET NAME=@name, EMAIL=@email, AGE=@age WHERE ID=@id";
                    cmd.Prepare();
                    cmd.Parameters.AddWithValue("@id", student.id);
                    cmd.Parameters.AddWithValue("@name", student.name);
                    cmd.Parameters.AddWithValue("@email", student.email);
                    cmd.Parameters.AddWithValue("@age", student.age);
                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Student student = new Student();
                student.name = txtName.Text;
                student.email = txtEmail.Text;
                student.age = Convert.ToInt32(txtAge.Text);

                try
                {
                    if(option == "Insert")
                    {
                        if(InsertData(student) > 0)
                        {
                            MessageBox.Show("Data has been included.");
                            
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Data were not included.");
                        }
                    }
                    else
                    {
                        student.id = _student.id;
                        if (UpdateData(student) > 0)
                        {
                            MessageBox.Show("Data has been updated.");
                        }
                        else
                        {
                            MessageBox.Show("Data were not updated.");
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Invalid data.");
            }
        }

        private Boolean ValidateData()
        {
            bool val_return = true;

            if (txtName.Text == string.Empty)
                val_return = false;
            if (txtEmail.Text == string.Empty)
                val_return = false;
            if (txtAge.Text == string.Empty)
                val_return = false;

            return val_return;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
