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
    public partial class Dobavlenie : Form
    {
        int CurrentWarehouseId;
        string connectionString = "Data Source=DESKTOP-RVH1N08\\EGRSERVER;Initial Catalog=newTestDB;Integrated Security=True";

        public Dobavlenie(int currentWarehouseId)
        {
            CurrentWarehouseId = currentWarehouseId;
            InitializeComponent();
        }

        private void Dobavlenie_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "newTestDBDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.newTestDBDataSet.Products);

        }

        DataTable DataTableSendWarehouses;
        SqlDataAdapter dataAdapterSendWarehouses;
        SqlConnection ConnectionSendWarehouses;

        DataTable DataTableUsers;
        SqlConnection ConnectionUsers;
        SqlDataAdapter dataAdapter;

        private void button1_Click(object sender, EventArgs e)
        {
            var flagD = false;

            var selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

            var ProductId = dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString();
            var Count = int.Parse(textBox1.Text);

            DataTableUsers = new DataTable();
            ConnectionUsers = new SqlConnection(connectionString);
            ConnectionUsers.Open();
            try
            {
                SqlCommand commandUsers = ConnectionUsers.CreateCommand();
                commandUsers.Connection = ConnectionUsers;
                commandUsers.CommandText = "SELECT * FROM WarehouseProduct";
                dataAdapter = new SqlDataAdapter(commandUsers);
                dataAdapter.Fill(DataTableUsers);
                var UserRow = DataTableUsers.AsEnumerable().FirstOrDefault(x => int.Parse(x[1].ToString()) == int.Parse(ProductId) && int.Parse(x[3].ToString()) == CurrentWarehouseId);
                if (!(UserRow == null))
                {
                    flagD = true;
                }
                else flagD = false;
            }
            finally
            {
                ConnectionUsers.Close();
            }


            DataTableSendWarehouses = new DataTable();
            ConnectionSendWarehouses = new SqlConnection(connectionString);
            ConnectionSendWarehouses.Open();
            try
            {
                SqlCommand commandSendWarehouses = ConnectionSendWarehouses.CreateCommand();
                commandSendWarehouses.Connection = ConnectionSendWarehouses;
                if (flagD)
                    commandSendWarehouses.CommandText = "UPDATE WarehouseProduct SET productCount = productCount + @Count WHERE warehous_Id = @Warehouse_Id AND product_Id = @product_Id";
                else
                    commandSendWarehouses.CommandText = "INSERT INTO WarehouseProduct (product_Id, productCount, warehous_Id) VALUES (@Product_Id, @Count, @Warehouse_Id)";

                commandSendWarehouses.Parameters.Add("@Warehouse_Id", SqlDbType.Int);
                commandSendWarehouses.Parameters[0].Value = CurrentWarehouseId;

                commandSendWarehouses.Parameters.Add("@Product_Id", SqlDbType.Int);
                commandSendWarehouses.Parameters[1].Value = ProductId;

                commandSendWarehouses.Parameters.Add("@Count", SqlDbType.Int);
                commandSendWarehouses.Parameters[2].Value = Count;

                dataAdapterSendWarehouses = new SqlDataAdapter(commandSendWarehouses);
                dataAdapterSendWarehouses.Fill(DataTableSendWarehouses);
            }
            finally
            {
                ConnectionSendWarehouses.Close();
            }
        }
    }
}
