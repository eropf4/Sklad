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


namespace Sklad
{
    public partial class Form1 : Form
    {
        DataTable DataTableProducts;
        SqlDataAdapter dataAdapter;
        SqlConnection ConnectionProducts;

        DataTable DataTableRequests;
        SqlDataAdapter dataAdapterRequests;
        SqlConnection ConnectionRequests;

        string Product;
        int Count;
        int UserId;
        int WarehouseIdSend;
        int WarehouseIdGet;
        string WarehouseNameSend;
        string WarehouseNameGet;
        string LocationGet;
        string LocationSend;
        int CountInSklad;

        string connectionString = "Data Source=DESKTOP-RVH1N08\\EGRSERVER;Initial Catalog=newTestDB;Integrated Security=True";

        bool flag = false;

        public Form1(int warehouseId, string locationGet, int userId, string warehouseName)
        {
            InitializeComponent();

            WarehouseIdGet = warehouseId;
            LocationGet = locationGet;
            UserId = userId;
            WarehouseNameGet = warehouseName;

            DataTableProducts = new DataTable();
            ConnectionProducts = new SqlConnection(connectionString);
            ConnectionProducts.Open();
            try
            {
                SqlCommand commandUsers = ConnectionProducts.CreateCommand();
                commandUsers.Connection = ConnectionProducts;
                commandUsers.CommandText = "SELECT Warehouses.Warehouse_Id, Warehouses.Name, Warehouses.locatioin,product_name, type, unit, price,productCount FROM Products " +
                    "INNER JOIN WarehouseProduct ON WarehouseProduct.product_id = Products.product_id " +
                    "INNER JOIN Warehouses ON Warehouses.Warehouse_Id=WarehouseProduct.warehous_Id ";
                dataAdapter = new SqlDataAdapter(commandUsers);
                dataAdapter.Fill(DataTableProducts);
                dataGridView1.DataSource = DataTableProducts;
                dataGridView1.ReadOnly = true;
            }
            finally
            {
                ConnectionProducts.Close();
            }


            DataTableRequests = new DataTable();
            ConnectionRequests = new SqlConnection(connectionString);
            ConnectionRequests.Open();
            try
            {
                SqlCommand commandRequests = ConnectionRequests.CreateCommand();
                commandRequests.Connection = ConnectionRequests;
                commandRequests.CommandText = "SELECT Product, Count, SendWarehouseName, LocationSend,RequestOk FROM Requests " +
                    "WHERE GetWarehouseId = @Warehouse_Id";
                commandRequests.Parameters.Add("@Warehouse_Id", SqlDbType.VarChar, 11);
                commandRequests.Parameters[0].Value = WarehouseIdGet;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "newTestDBDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.newTestDBDataSet.Products);

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null && FindBox1.Text != "")
                        if (dataGridView1.Rows[i].Cells[3].Value.ToString().Contains(FindBox1.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                dataGridView1.Rows[i].Selected = false;
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null && FindLocationBox.Text != "")
                        if (dataGridView1.Rows[i].Cells[2].Value.ToString().Contains(FindLocationBox.Text))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
            }
        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            var valueType = "";
            if (flag == true)
            {
                valueType = dataGridView3.SelectedCells[0].Value.ToString();

                DataTableProducts = new DataTable();
                ConnectionProducts = new SqlConnection(connectionString);
                ConnectionProducts.Open();
                try
                {
                    SqlCommand commandUsers = ConnectionProducts.CreateCommand();
                    commandUsers.Connection = ConnectionProducts;
                    commandUsers.CommandText = "SELECT Warehouses.Warehouse_Id, Warehouses.Name, Warehouses.locatioin,product_name, type, unit, price,productCount FROM Products " +
                        "INNER JOIN WarehouseProduct ON WarehouseProduct.product_id = Products.product_id " +
                        "INNER JOIN Warehouses ON Warehouses.Warehouse_Id=WarehouseProduct.warehous_Id " +
                        "WHERE Products.type = @Type";

                    commandUsers.Parameters.Add("@Type", SqlDbType.VarChar, 11);
                    commandUsers.Parameters[0].Value = valueType;

                    dataAdapter = new SqlDataAdapter(commandUsers);
                    dataAdapter.Fill(DataTableProducts);
                    dataGridView1.DataSource = DataTableProducts;
                    dataGridView1.ReadOnly = true;
                }
                finally
                {
                    ConnectionProducts.Close();
                }
            }
            flag = true;
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
                commandUsers.CommandText = "SELECT Warehouses.Warehouse_Id, Warehouses.Name, Warehouses.locatioin,product_name, type, unit, price,productCount FROM Products " +
                    "INNER JOIN WarehouseProduct ON WarehouseProduct.product_id = Products.product_id " +
                    "INNER JOIN Warehouses ON Warehouses.Warehouse_Id=WarehouseProduct.warehous_Id ";
                dataAdapter = new SqlDataAdapter(commandUsers);
                dataAdapter.Fill(DataTableProducts);
                dataGridView1.DataSource = DataTableProducts;
                dataGridView1.ReadOnly = true;
            }
            finally
            {
                ConnectionProducts.Close();
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;

            var sklad = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
            var productName = dataGridView1.Rows[selectedRowIndex].Cells[3].Value.ToString();
            var location = dataGridView1.Rows[selectedRowIndex].Cells[2].Value.ToString();

            WarehouseIdSend = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString());
            CountInSklad = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[7].Value.ToString());

            SkladBox.Text = sklad;
            NameBox.Text = productName;
            LocationBox.Text = location;

            WarehouseNameSend = dataGridView1.Rows[selectedRowIndex].Cells[1].Value.ToString();
            WarehouseIdSend = int.Parse(dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString());
            Product = productName;
            LocationSend = location;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Count = int.Parse(textBox5.Text);

            if (Count > CountInSklad || Count == 0) { MessageBox.Show("На складе нет нужного количества товара"); return; }

            if (Count > 10000) { MessageBox.Show("Недостаточный бюджет");return; }

            DataTableRequests = new DataTable();
            ConnectionRequests = new SqlConnection(connectionString);
            ConnectionRequests.Open();
            try
            {
                SqlCommand commandRequests = ConnectionRequests.CreateCommand();
                commandRequests.Connection = ConnectionRequests;
                commandRequests.CommandText = "INSERT INTO Requests (Product, Count, UserId, RequestOk, SendWarehouseId,GetWarehouseId,SendWarehouseName,GetWarehouseName,LocationGet,LocationSend)" + 
                    " VALUES (@Product, @Count, @UserId, @RequestOk, @SendWarehouseId, @GetWarehouseId, @SendWarehouseName, @GetWarehouseName, @LocationGet, @LocationSend)";
                
                commandRequests.Parameters.Add("@Product", SqlDbType.VarChar, 50);
                commandRequests.Parameters[0].Value = Product;

                commandRequests.Parameters.Add("@Count", SqlDbType.Int);
                commandRequests.Parameters[1].Value = Count;

                commandRequests.Parameters.Add("@UserId", SqlDbType.Int);
                commandRequests.Parameters[2].Value = UserId;

                commandRequests.Parameters.Add("@RequestOk", SqlDbType.Bit);
                commandRequests.Parameters[3].Value = false;

                commandRequests.Parameters.Add("@SendWarehouseId", SqlDbType.Int);
                commandRequests.Parameters[4].Value = WarehouseIdSend;

                commandRequests.Parameters.Add("@GetWarehouseId", SqlDbType.Int);
                commandRequests.Parameters[5].Value = WarehouseIdGet;

                commandRequests.Parameters.Add("@SendWarehouseName", SqlDbType.VarChar, 50);
                commandRequests.Parameters[6].Value = WarehouseNameSend;

                commandRequests.Parameters.Add("@GetWarehouseName", SqlDbType.VarChar, 50);
                commandRequests.Parameters[7].Value = WarehouseNameGet;

                commandRequests.Parameters.Add("@LocationGet", SqlDbType.VarChar, 50);
                commandRequests.Parameters[8].Value = LocationGet;//

                commandRequests.Parameters.Add("@LocationSend", SqlDbType.VarChar, 50);
                commandRequests.Parameters[9].Value = LocationSend;



                commandRequests.CommandText += "\nSELECT Product, Count, SendWarehouseName, LocationSend,RequestOk FROM Requests " +
                    "WHERE GetWarehouseId = @Warehouse_Id";
                commandRequests.Parameters.Add("@Warehouse_Id", SqlDbType.Int);
                commandRequests.Parameters[10].Value = WarehouseIdGet;

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

        private void fillByToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.productsTableAdapter.FillBy(this.newTestDBDataSet.Products);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            Excel.Range oRng;

            var newMatrix = new string[RequestsDataGridView.RowCount + 1, RequestsDataGridView.ColumnCount];

            newMatrix[0, 0] = "Наименование";
            newMatrix[0, 1] = "Количество";
            newMatrix[0, 2] = "Наименование склада";
            newMatrix[0, 3] = "Город склада";
            newMatrix[0, 4] = "Запрос успешно выполнен";

            for (int i = 0; i < RequestsDataGridView.RowCount; i++)
            {
                for (int j = 0; j < RequestsDataGridView.ColumnCount; j++)
                    if (RequestsDataGridView.Rows[i].Cells[j].Value != null)
                        newMatrix[i + 1, j] = RequestsDataGridView.Rows[i].Cells[j].Value.ToString();
            }

            oXL = new Excel.Application();
            oXL.Visible = true;

            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Type.Missing));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;

            for (int i = 0; i < RequestsDataGridView.RowCount + 1; i++)
            {
                for (int j = 0; j < RequestsDataGridView.ColumnCount; j++)
                    oSheet.Cells[i + 1, j + 1] = newMatrix[i, j];
            }
        }
    }
}
