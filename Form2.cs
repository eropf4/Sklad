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
    public partial class Form2 : Form
    {
        DataTable DataTableRequests;
        SqlDataAdapter dataAdapterRequests;
        SqlConnection ConnectionRequests;

        string connectionString = "Data Source=DESKTOP-RVH1N08\\EGRSERVER;Initial Catalog=newTestDB;Integrated Security=True";

        int CurrentWarehousesId;
        int ProductId;
        int ProductPrice;

        public Form2(int currentWarehouseId)
        {
            InitializeComponent();

            CurrentWarehousesId = currentWarehouseId;

            DataTableRequests = new DataTable();
            ConnectionRequests = new SqlConnection(connectionString);
            ConnectionRequests.Open();
            try
            {
                SqlCommand commandRequests = ConnectionRequests.CreateCommand();
                commandRequests.Connection = ConnectionRequests;
                commandRequests.CommandText = "SELECT RequestId,Product, Count, GetWarehouseId ,GetWarehouseName, LocationGet,RequestOk FROM Requests " +
                    "WHERE SendWarehouseId = @Warehouse_Id";
                commandRequests.Parameters.Add("@Warehouse_Id", SqlDbType.VarChar, 11);
                commandRequests.Parameters[0].Value = currentWarehouseId;
                dataAdapterRequests = new SqlDataAdapter(commandRequests);
                dataAdapterRequests.Fill(DataTableRequests);
                RequestsDataGridView.DataSource = DataTableRequests;
                RequestsDataGridView.ReadOnly = true;
            }
            finally
            {
                ConnectionRequests.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
        }

        DataTable DataTableSendWarehouses;
        SqlDataAdapter dataAdapterSendWarehouses;
        SqlConnection ConnectionSendWarehouses;

        DataTable DataTableGetWarehouses;
        SqlDataAdapter dataAdapterDataTableGetWarehouses;
        SqlConnection ConnectionDataTableGetWarehouses;

        DataTable DataTableProducts;
        SqlDataAdapter dataAdapterDataTableProducts;
        SqlConnection ConnectionDataTableProducts;

        private void button1_Click(object sender, EventArgs e)
        {

            var selectedRowIndex = RequestsDataGridView.SelectedCells[0].RowIndex;

            var requestsId = RequestsDataGridView.Rows[selectedRowIndex].Cells[0].Value.ToString();
            var productName = RequestsDataGridView.Rows[selectedRowIndex].Cells[1].Value.ToString();
            var productCount = RequestsDataGridView.Rows[selectedRowIndex].Cells[2].Value.ToString();
            var getWarehouseId = RequestsDataGridView.Rows[selectedRowIndex].Cells[3].Value.ToString();

            DataTableProducts = new DataTable();
            ConnectionDataTableProducts = new SqlConnection(connectionString);
            ConnectionDataTableProducts.Open();
            try
            {
                SqlCommand commandProducts = ConnectionDataTableProducts.CreateCommand();
                commandProducts.Connection = ConnectionDataTableProducts;
                commandProducts.CommandText = "SELECT product_Id,price FROM Products " +
                    "WHERE product_name = @ProductName";
                commandProducts.Parameters.Add("@ProductName", SqlDbType.VarChar, 11);
                commandProducts.Parameters[0].Value = productName;
                dataAdapterDataTableProducts = new SqlDataAdapter(commandProducts);
                dataAdapterDataTableProducts.Fill(DataTableProducts);
                var UserRow = DataTableProducts.AsEnumerable().First();

                ProductId = int.Parse(UserRow[0].ToString());
                ProductPrice = int.Parse(UserRow[1].ToString()) ;
            }
            finally
            {
                ConnectionDataTableProducts.Close();
            }

            DataTableSendWarehouses = new DataTable();
            ConnectionSendWarehouses = new SqlConnection(connectionString);
            ConnectionSendWarehouses.Open();
            try
            {
                SqlCommand commandSendWarehouses = ConnectionSendWarehouses.CreateCommand();
                commandSendWarehouses.Connection = ConnectionSendWarehouses;
                commandSendWarehouses.CommandText = "UPDATE WarehouseProduct SET productCount = productCount - @Count WHERE warehous_Id = @Warehouse_Id AND product_Id = @product_Id";
                commandSendWarehouses.CommandText += "\n" + "UPDATE Warehouses SET Budget = Budget + @Count * @Price WHERE Warehouse_Id = @Warehouse_Id";
                commandSendWarehouses.CommandText += "\n" + "UPDATE Requests SET RequestOk = 1 WHERE RequestId = @RequestId";


                commandSendWarehouses.CommandText += "\n" + "UPDATE WarehouseProduct SET productCount = productCount + @Count WHERE warehous_Id = @Warehouse_IdGet AND product_Id = @Product_Id";
                commandSendWarehouses.CommandText += "\n" + "UPDATE Warehouses SET Budget = Budget - @Count * @Price WHERE Warehouse_Id = @Warehouse_IdGet";
                commandSendWarehouses.CommandText += "\n" + "SELECT RequestId,Product, Count, GetWarehouseId ,GetWarehouseName, LocationGet,RequestOk FROM Requests " +
                    "WHERE SendWarehouseId = @Warehouse_Id";

                commandSendWarehouses.Parameters.Add("@Warehouse_Id", SqlDbType.Int);
                commandSendWarehouses.Parameters[0].Value = CurrentWarehousesId;

                commandSendWarehouses.Parameters.Add("@Product_Id", SqlDbType.Int);
                commandSendWarehouses.Parameters[1].Value = ProductId;

                commandSendWarehouses.Parameters.Add("@Price", SqlDbType.Int);
                commandSendWarehouses.Parameters[2].Value = ProductPrice;

                commandSendWarehouses.Parameters.Add("@Count", SqlDbType.Int);
                commandSendWarehouses.Parameters[3].Value = int.Parse(productCount);

                commandSendWarehouses.Parameters.Add("@Warehouse_IdGet", SqlDbType.Int);
                commandSendWarehouses.Parameters[4].Value = int.Parse(getWarehouseId);

                commandSendWarehouses.Parameters.Add("@RequestId", SqlDbType.Int);
                commandSendWarehouses.Parameters[5].Value = int.Parse(requestsId);


                dataAdapterSendWarehouses = new SqlDataAdapter(commandSendWarehouses);
                dataAdapterSendWarehouses.Fill(DataTableSendWarehouses);

                RequestsDataGridView.DataSource = DataTableSendWarehouses;
                RequestsDataGridView.ReadOnly = true;
            }
            finally
            {
                ConnectionSendWarehouses.Close();
            }

        }
    }
}
