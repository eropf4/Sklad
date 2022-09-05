using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sklad
{
    public partial class AuthorizationForm : Form
    {
        DataTable DataTableUsers;
        SqlConnection ConnectionUsers;
        SqlDataAdapter dataAdapter;

        string connectionString = "Data Source=DESKTOP-RVH1N08\\EGRSERVER;Initial Catalog=newTestDB;Integrated Security=True";

        public AuthorizationForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ButtonIn_Click(object sender, EventArgs e)
        {
            DataTableUsers = new DataTable();
            ConnectionUsers = new SqlConnection(connectionString);
            ConnectionUsers.Open();
            try
            {
                SqlCommand commandUsers = ConnectionUsers.CreateCommand();
                commandUsers.Connection = ConnectionUsers;
                commandUsers.CommandText = "SELECT * FROM dbo.Users";
                dataAdapter = new SqlDataAdapter(commandUsers);
                dataAdapter.Fill(DataTableUsers);
                var UserRow = DataTableUsers.AsEnumerable().FirstOrDefault(x => x[2].ToString() == LoginBox.Text);
                if (UserRow[1].ToString() == PasswordBox.Text)
                {
                    StartForm startForm = new StartForm(UserRow);
                    startForm.ShowDialog();
                }
                else MessageBox.Show("Неправильно введен логин или пароль");
            }
            finally
            {
                ConnectionUsers.Close();
            }
        }

        private void AuthorizationForm_Load(object sender, EventArgs e)
        {

        }
    }
}
