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
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Sklad
{
    public partial class StartForm : Form
    {
        bool flag = false;

        int UserId;
        int WarhouseId;
        string NameUser;
        string WarehouseLocation;
        string WarehouseName;

        DataTable DataTableProducts;
        SqlDataAdapter dataAdapterProducts;
        SqlConnection ConnectionProducts;

        DataTable DataTableWarehouses;
        SqlDataAdapter dataAdapterWarehouses;
        SqlConnection ConnectionWarehouses;

        string connectionString = "Data Source=DESKTOP-RVH1N08\\EGRSERVER;Initial Catalog=newTestDB;Integrated Security=True";

        public StartForm(DataRow userRow)
        {
            InitializeComponent();

            UserId = Int16.Parse(userRow[0].ToString());
            WarhouseId = Int16.Parse(userRow[3].ToString());
            NameUser = userRow[2].ToString();

            DataTableProducts = new DataTable();
            ConnectionProducts = new SqlConnection(connectionString);
            ConnectionProducts.Open();
            try
            {
                SqlCommand commandUsers = ConnectionProducts.CreateCommand();
                commandUsers.Connection = ConnectionProducts;
                commandUsers.CommandText = "SELECT product_name, type, unit, price,productCount FROM Products " +
                    "INNER JOIN WarehouseProduct ON WarehouseProduct.product_id = Products.product_id " +
                    "INNER JOIN Warehouses ON Warehouses.Warehouse_Id=WarehouseProduct.warehous_Id " +
                    "WHERE Warehouses.Warehouse_Id = @Warehouse_Id";
                commandUsers.Parameters.Add("@Warehouse_Id", SqlDbType.VarChar, 11);
                commandUsers.Parameters[0].Value = WarhouseId;
                dataAdapterProducts = new SqlDataAdapter(commandUsers);
                dataAdapterProducts.Fill(DataTableProducts);
                dataGridViewProducts.DataSource = DataTableProducts;
                dataGridViewProducts.ReadOnly = true;
            }
            finally
            {
                ConnectionProducts.Close();
            }

            DataTableWarehouses = new DataTable();
            ConnectionWarehouses = new SqlConnection(connectionString);
            ConnectionWarehouses.Open();
            try
            {
                SqlCommand commandWarehouses = ConnectionWarehouses.CreateCommand();
                commandWarehouses.Connection = ConnectionWarehouses;
                commandWarehouses.CommandText = "SELECT Name, Budget, locatioin FROM Warehouses " +
                    "WHERE Warehouses.Warehouse_Id = @Warehouse_Id";
                commandWarehouses.Parameters.Add("@Warehouse_Id", SqlDbType.VarChar, 11);
                commandWarehouses.Parameters[0].Value = WarhouseId;
                dataAdapterWarehouses = new SqlDataAdapter(commandWarehouses);
                dataAdapterWarehouses.Fill(DataTableWarehouses);
                var UserRow = DataTableWarehouses.AsEnumerable().First();

                BudgetText.Text = UserRow[1].ToString();
                label5.Text = "\"" + UserRow[0].ToString() + "\"";
                WarehouseLocation = UserRow[2].ToString();
                WarehouseName = UserRow[0].ToString();
            }
            finally
            {
                ConnectionWarehouses.Close();
            }

        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "newTestDBDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.newTestDBDataSet.Products);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "newTestDBDataSet.Products". При необходимости она может быть перемещена или удалена.
            // TODO: данная строка кода позволяет загрузить данные в таблицу "newTestDBDataSet.Warehouses". При необходимости она может быть перемещена или удалена.
            this.warehousesTableAdapter.Fill(this.newTestDBDataSet.Warehouses);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "newTestDBDataSet.Warehouses". При необходимости она может быть перемещена или удалена.
            this.warehousesTableAdapter.Fill(this.newTestDBDataSet.Warehouses);

        }

        private void warehousesBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.warehousesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.newTestDBDataSet);

        }

        private void warehousesBindingNavigatorSaveItem_Click_1(object sender, EventArgs e)
        {
            this.Validate();
            this.warehousesBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.newTestDBDataSet);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var a = new Form2(WarhouseId);
            a.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = new Form1(WarhouseId,WarehouseLocation ,UserId, WarehouseName);
            a.ShowDialog();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var valueType = "";
            if (flag == true)
            {
                valueType = dataGridView1.SelectedCells[0].Value.ToString();

                DataTableProducts = new DataTable();
                ConnectionProducts = new SqlConnection(connectionString);
                ConnectionProducts.Open();
                try
                {
                    SqlCommand commandUsers = ConnectionProducts.CreateCommand();
                    commandUsers.Connection = ConnectionProducts;
                    commandUsers.CommandText = "SELECT product_name, type, unit, price,productCount FROM Products " +
                        "INNER JOIN WarehouseProduct ON WarehouseProduct.product_id = Products.product_id " +
                        "INNER JOIN Warehouses ON Warehouses.Warehouse_Id=WarehouseProduct.warehous_Id " +
                        "WHERE Warehouses.Warehouse_Id = @Warehouse_Id AND Products.type = @Type";
                    commandUsers.Parameters.Add("@Warehouse_Id", SqlDbType.VarChar, 11);
                    commandUsers.Parameters[0].Value = WarhouseId;

                    commandUsers.Parameters.Add("@Type", SqlDbType.VarChar, 11);
                    commandUsers.Parameters[1].Value = valueType;

                    dataAdapterProducts = new SqlDataAdapter(commandUsers);
                    dataAdapterProducts.Fill(DataTableProducts);
                    dataGridViewProducts.DataSource = DataTableProducts;
                    dataGridViewProducts.ReadOnly = true;


                }
                finally
                {
                    ConnectionProducts.Close();
                }
            }
            flag = true;
        }

        private void FindBox_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridViewProducts.RowCount; i++)
            {
                dataGridViewProducts.Rows[i].Selected = false;
                for (int j = 0; j < dataGridViewProducts.ColumnCount; j++)
                    if (dataGridViewProducts.Rows[i].Cells[j].Value != null && FindBox.Text != "")
                        if (dataGridViewProducts.Rows[i].Cells[0].Value.ToString().Contains(FindBox.Text))
                        {
                            dataGridViewProducts.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DataTableProducts = new DataTable();
            ConnectionProducts = new SqlConnection(connectionString);
            ConnectionProducts.Open();
            try
            {
                SqlCommand commandUsers = ConnectionProducts.CreateCommand();
                commandUsers.Connection = ConnectionProducts;
                commandUsers.CommandText = "SELECT product_name, type, unit, price,productCount FROM Products " +
                    "INNER JOIN WarehouseProduct ON WarehouseProduct.product_id = Products.product_id " +
                    "INNER JOIN Warehouses ON Warehouses.Warehouse_Id=WarehouseProduct.warehous_Id " +
                    "WHERE Warehouses.Warehouse_Id = @Warehouse_Id";
                commandUsers.Parameters.Add("@Warehouse_Id", SqlDbType.VarChar, 11);
                commandUsers.Parameters[0].Value = WarhouseId;
                dataAdapterProducts = new SqlDataAdapter(commandUsers);
                dataAdapterProducts.Fill(DataTableProducts);
                dataGridViewProducts.DataSource = DataTableProducts;
                dataGridViewProducts.ReadOnly = true;
            }
            finally
            {
                ConnectionProducts.Close();
            }

            DataTableWarehouses = new DataTable();
            ConnectionWarehouses = new SqlConnection(connectionString);
            ConnectionWarehouses.Open();
            try
            {
                SqlCommand commandWarehouses = ConnectionWarehouses.CreateCommand();
                commandWarehouses.Connection = ConnectionWarehouses;
                commandWarehouses.CommandText = "SELECT Name, Budget, locatioin FROM Warehouses " +
                    "WHERE Warehouses.Warehouse_Id = @Warehouse_Id";
                commandWarehouses.Parameters.Add("@Warehouse_Id", SqlDbType.VarChar, 11);
                commandWarehouses.Parameters[0].Value = WarhouseId;
                dataAdapterWarehouses = new SqlDataAdapter(commandWarehouses);
                dataAdapterWarehouses.Fill(DataTableWarehouses);
                var UserRow = DataTableWarehouses.AsEnumerable().First();

                BudgetText.Text = UserRow[1].ToString();
            }
            finally
            {
                ConnectionWarehouses.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var a = new Dobavlenie(WarhouseId);
            a.ShowDialog();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            var newMatrix = new string[dataGridViewProducts.RowCount + 1, dataGridViewProducts.ColumnCount];

            newMatrix[0, 0] = "Наименование";
            newMatrix[0, 1] = "Тип продукта";
            newMatrix[0, 2] = "Единица измерения";
            newMatrix[0, 3] = "Цена";
            newMatrix[0, 4] = "Количество";

            for (int i = 0; i < dataGridViewProducts.RowCount; i++)
            {
                for (int j = 0; j < dataGridViewProducts.ColumnCount; j++)
                    if (dataGridViewProducts.Rows[i].Cells[j].Value != null)
                        newMatrix[i+1,j] = dataGridViewProducts.Rows[i].Cells[j].Value.ToString();
            }

            oXL = new Excel.Application();
            oXL.Visible = true;

            oWB = (Excel._Workbook)(oXL.Workbooks.Add( Type.Missing ));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            for (int i = 0; i < dataGridViewProducts.RowCount + 1; i++)
            {
                for (int j = 0; j < dataGridViewProducts.ColumnCount; j++)
                    oSheet.Cells[i+1, j+1] = newMatrix[i, j];
            }
        }
    }
}
