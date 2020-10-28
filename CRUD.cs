using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UtilsBasic2020
{
    public class CRUD
    {
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable dataTable;
        private SqlDataReader reader;
        private StringBuilder sql;

        public CRUD()
        {

        }

        /// <summary>
        /// Chuỗi kết nối tới Database
        /// <code>
        /// <paramref name="url"/> 
        /// @"Data Source=.\SQLEXPRESS;Initial Catalog=BankDB;User ID=sa;Password=848028;Pooling=False"
        /// </code>
        /// </summary>
        public CRUD(string url)
        {
            try
            {
                connection = new SqlConnection(url);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                Utils.MSG("Kết nối tới Database thất bại: " + ex.Message);
            }
            
        }

        public SqlConnection Connection { get => connection; set => connection = value; }

        #region Orther

        /// <summary>
        /// Check SqlConnection is Open
        /// </summary>
        public void CheckConnection(SqlConnection conn)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
        }

        /// <summary>
        /// Chạy tham số và giá trị
        /// </summary>
        public void ParamsAndValues(SqlCommand cmd, string[] parameters, object[] values)
        {
            for(int i = 0; i < parameters.Length; i++)
            {
                cmd.Parameters.AddWithValue(parameters[i], values[i]);
            }
        }

        /// <summary>
        /// Đỗ dữ liệu trong DataTable vào DataGridView
        /// </summary>
        public void LoadDataGridView(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
        }

        #endregion

        /// <summary>
        /// SqlDataAdapter fill DataTable
        /// <code>
        /// <paramref name="table"/> Tên table
        /// </code>
        /// </summary>
        public DataTable LoadDataTableOne(string tableName, string[] fields = null)
        {
            try
            {
                if (fields != null && fields.Length > 0)
                {
                    string strJoin = string.Join(",", fields);
                    sql = new StringBuilder();
                    sql.Append("select ").Append(strJoin).Append(" from ");
                }
                else
                {
                    sql = new StringBuilder("select * from ");
                }

                sql.Append(tableName);
                //command = new SqlCommand("select * from Account", connection);
                command = new SqlCommand(sql.ToString(), connection);
                //command.Parameters.AddWithValue("@table", table);
                adapter = new SqlDataAdapter(command);
                dataTable = new DataTable();
                adapter.Fill(dataTable);
                adapter.Dispose();
            }
            catch (Exception ex)
            {
                Utils.MSG("DataTable không nhận được dữ liệu: " + ex.Message);
            }
            return dataTable;
        }

        /// <summary>
        /// DataTable load SqlDataReader
        /// <code>
        /// <paramref name="table"/> Tên table
        /// </code>
        /// </summary>
        public DataTable LoadDataTableTwo(string tableName, string[] fields = null)
        {
            if(connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            
            try
            {

                if (fields != null && fields.Length > 0)
                {
                    string strJoin = string.Join(",", fields);
                    sql = new StringBuilder();
                    sql.Append("select ").Append(strJoin).Append(" from ");
                }
                else
                {
                    sql = new StringBuilder("select * from ");
                }

                sql.Append(tableName);

                command = new SqlCommand(sql.ToString(), connection);
                //command.Parameters.AddWithValue("@table", table);
                reader = command.ExecuteReader();
                dataTable = new DataTable();
                dataTable.Load(reader);
                reader.Dispose();
            }
            catch (Exception ex)
            {
                Utils.MSG("DataTable không tải được dữ liệu: " + ex.Message);
                connection.Close();
            }
            connection.Close();
            return dataTable;
        }
    }
}
