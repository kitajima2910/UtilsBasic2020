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
        private DataSet dataSet;

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
                Utils.MSG(ex.Message);
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
        public void LoadDataToGridView(DataGridView dataGridView, DataTable dataTable)
        {
            dataGridView.DataSource = dataTable;
        }

        #endregion

        /// <summary>
        /// Code Mẫu:
        /// <para>public void Search(DataGridView dgView, string keyWord) {</para> 
        /// <para>string[] where = { "Phone", "Location" };</para>
        /// <para>string[] whereValues = { keyWord, keyWord };</para>
        /// <para>BookStore.crud.Search(dgView, "Publisher", where, whereValues);</para>
        /// <para>}</para> 
        /// </summary>
        public void Search(DataGridView dgView,  string tableName, string[] where, string[] whereValues, 
            string[] fields = null, string option = "or")
        {
            try
            {
                StringBuilder join = new StringBuilder();
                if(where.Length == 1)
                {
                    join.Append(where[0]).Append(" like ").Append("\'%\'+@").Append(where[0]).Append("+\'%\'");
                }
                else
                {
                    for(int i = 0; i < where.Length; i++)
                    {
                        join.Append(where[i]).Append(" like ").Append("\'%\'+@").Append(where[i]).Append("+\'%\'");
                        if(i == where.Length - 1)
                        {
                            break;
                        }
                        join.Append(" ").Append(option).Append(" ");
                    }
                }

                StringBuilder sql = new StringBuilder();

                if(fields != null && fields.Length > 0)
                {
                    string joinFields = string.Join(", ", fields);
                    sql.Append("select ").Append(joinFields).Append(" from")
                        .Append(tableName).Append(" where ");
                }
                else
                {
                    sql.Append("select * from ").Append(tableName).Append(" where ").Append(join);
                }

                command = new SqlCommand(sql.ToString(), connection);
                ParamsAndValues(command, where, whereValues);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                dgView.DataSource = dataSet.Tables[tableName];
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Xoá dữ liệu
        /// <code><paramref name="tableName"/> Tên bảng cần thao tác</code>
        /// <code><paramref name="where"/> Danh sách tham số điều kiện</code>
        /// <code><paramref name="whereValues"/> Danh sách tham số mapping với where</code>
        /// <code><paramref name="option"/> Mặc định là and</code>
        /// </summary>
        public bool Delete(string tableName, string[] where, object[] whereValues, string option = "and")
        {
            try
            {
                StringBuilder join = new StringBuilder();
                if (where.Length == 1)
                {
                    join.Append(where[0]).Append("=").Append("@").Append(where[0]);
                }
                else
                {
                    for (int i = 0; i < where.Length; i++)
                    {
                        join.Append(where[i]).Append("=").Append("@").Append(where[i]);
                        if (i == where.Length - 1)
                        {
                            break;
                        }
                        join.Append(" ").Append(option).Append(" ");
                    }
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("delete from ").Append(tableName).Append(" where ").Append(join);
                command = new SqlCommand(sql.ToString(), connection);
                ParamsAndValues(command, where, whereValues);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                return true;
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Xoá dữ liệu
        /// <code><paramref name="tableName"/> Tên bảng cần thao tác</code>
        /// <code><paramref name="where"/> Danh sách tham số điều kiện</code>
        /// <code><paramref name="whereValues"/> Danh sách tham số mapping với where</code>
        /// <code><paramref name="option"/> Mặc định là and</code>
        /// </summary>
        public bool Delete(string tableName, string[] where, string[] whereValues, string option = "and")
        {
            try
            {
                StringBuilder join = new StringBuilder();
                if(where.Length == 1)
                {
                    join.Append(where[0]).Append("=").Append("@").Append(where[0]);
                }
                else
                {
                    for(int i = 0; i < where.Length; i++)
                    {
                        join.Append(where[i]).Append("=").Append("@").Append(where[i]);
                        if(i == where.Length - 1)
                        {
                            break;
                        }
                        join.Append(" ").Append(option).Append(" ");
                    }
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("delete from ").Append(tableName).Append(" where ").Append(join);
                command = new SqlCommand(sql.ToString(), connection);
                ParamsAndValues(command, where, whereValues);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                return true;
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu
        /// <code><paramref name="tableName"/> Tên bảng cần cập nhật</code>
        /// <code><paramref name="parameters"/> Danh sách tham số cần cập nhật</code>
        /// <code><paramref name="values"/> Danh sách tham số có giá trị mapping với parameters</code>
        /// <code><paramref name="where"/> Danh sách tham số điều kiện where</code>
        /// <code><paramref name="whereValues"/> Danh sách tham số có giá trị mapping với where</code>
        /// <code><paramref name="option"/> Toán tử logic mặc định là and</code>
        /// </summary>
        public bool Update(string tableName, string[] parameters, string[] values,
            string[] where, string[] whereValues, string option = "and")
        {
            try
            {
                StringBuilder join = new StringBuilder();
                for (int i = 0; i < parameters.Length; i++)
                {
                    join.Append(parameters[i]).Append("=").Append("@").Append(parameters[i]);
                    if (i == parameters.Length - 1)
                    {
                        break;
                    }
                    join.Append(", ");
                }
                join.Append(" where ");
                if (where.Length == 1)
                {
                    join.Append(where[0]).Append("=").Append("@").Append(where[0]);
                }
                else
                {
                    for (int i = 0; i < where.Length; i++)
                    {
                        join.Append(where[i]).Append("=").Append("@").Append(where[i]);
                        if (i == where.Length - 1)
                        {
                            break;
                        }
                        join.Append(" ").Append(option).Append(" ");
                    }
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("update ").Append(tableName).Append(" set ").Append(join);

                command = new SqlCommand(sql.ToString(), connection);
                ParamsAndValues(command, parameters, values);
                ParamsAndValues(command, where, whereValues);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                return true;
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Cập nhật dữ liệu
        /// <code><paramref name="tableName"/> Tên bảng cần cập nhật</code>
        /// <code><paramref name="parameters"/> Danh sách tham số cần cập nhật</code>
        /// <code><paramref name="values"/> Danh sách tham số có giá trị mapping với parameters</code>
        /// <code><paramref name="where"/> Danh sách tham số điều kiện where</code>
        /// <code><paramref name="whereValues"/> Danh sách tham số có giá trị mapping với where</code>
        /// <code><paramref name="option"/> Toán tử logic mặc định là and</code>
        /// </summary>
        public bool Update(string tableName, string[] parameters, object[] values, 
            string[] where, object[] whereValues, string option = "and")
        {
            try
            {
                StringBuilder join = new StringBuilder();
                for(int i = 0;  i < parameters.Length; i++)
                {
                    join.Append(parameters[i]).Append("=").Append("@").Append(parameters[i]);
                    if(i == parameters.Length - 1)
                    {
                        break;
                    }
                    join.Append(", ");
                }
                join.Append(" where ");
                if(where.Length == 1)
                {
                    join.Append(where[0]).Append("=").Append("@").Append(where[0]);
                }
                else 
                { 
                    for(int i = 0; i < where.Length; i++)
                    {
                        join.Append(where[i]).Append("=").Append("@").Append(where[i]);
                        if(i == where.Length - 1)
                        {
                            break;
                        }
                        join.Append(" ").Append(option).Append(" ");
                    }
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("update ").Append(tableName).Append(" set ").Append(join);

                command = new SqlCommand(sql.ToString(), connection);
                ParamsAndValues(command, parameters, values);
                ParamsAndValues(command, where, whereValues);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                return true;
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Thêm dữ liệu và database
        /// <code><paramref name="tableName"/> Tên bảng cần thêm</code>
        /// <code><paramref name="parameters"/> Danh sách tham số mẫu</code>
        /// <code><paramref name="values"/> Danh sách tham số có dữ liệu</code>
        /// <code>Mẫu:</code>
        /// <code>string[] parametersm = {</code>
        /// <code>"Title", "Author", "PublisherName", "Edition"</code>
        /// <code>};</code>
        /// <code>string[] values = {</code>
        /// <code>txtTitle.Text, txtAuthor.Text, cbPublisherName.Text, txtEdition.Text</code>
        /// <code>}</code>
        /// <code>crud.Insert("Book", parametersm, values);</code>
        /// </summary>
        public bool Insert(string tableName, string[] parameters, string[] values)
        {

            try
            {
                string joinInto = string.Join(", ", parameters);
                StringBuilder joinValues = new StringBuilder();

                for (int i = 0; i < parameters.Length; i++)
                {
                    joinValues.Append("@").Append(parameters[i]);
                    if (i == parameters.Length - 1)
                    {
                        break;
                    }
                    joinValues.Append(", ");
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("insert ").Append("into ").Append(tableName).Append("(")
                    .Append(joinInto).Append(") ").Append(" values(").Append(joinValues).Append(")");
                command = new SqlCommand(sql.ToString(), connection);
                ParamsAndValues(command, parameters, values);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet);
                return true;
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Thêm dữ liệu và database
        /// <code><paramref name="tableName"/> Tên bảng cần thêm</code>
        /// <code><paramref name="parameters"/> Danh sách tham số mẫu</code>
        /// <code><paramref name="values"/> Danh sách tham số có dữ liệu</code>
        /// <code>Mẫu:</code>
        /// <code>string[] parametersm = {</code>
        /// <code>"Title", "Author", "PublisherName", "Edition"</code>
        /// <code>};</code>
        /// <code>object[] values = {</code>
        /// <code>book.Title, book.Author, book.PublisherName, book.Edition</code>
        /// <code>}</code>
        /// <code>crud.Insert("Book", parametersm, values);</code>
        /// </summary>
        public bool Insert(string tableName, string[] parameters, object[] values)
        {
            try
            {
                string joinInto = string.Join(", ", parameters);
                StringBuilder joinValues = new StringBuilder();

                for (int i = 0; i < parameters.Length; i++)
                {
                    joinValues.Append("@").Append(parameters[i]);
                    if (i == parameters.Length - 1)
                    {
                        break;
                    }
                    joinValues.Append(", ");
                }

                StringBuilder sql = new StringBuilder();
                sql.Append("insert ").Append("into ").Append(tableName).Append("(")
                    .Append(joinInto).Append(") ").Append(" values(").Append(joinValues).Append(")");
                command = new SqlCommand(sql.ToString(), connection);
                ParamsAndValues(command, parameters, values);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet);
                return true;
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return false;
            }
            
        }

        /// <summary>
        /// Hiển thị dữ liệu vào ComboBox theo fieldShow
        /// <code>
        /// <paramref name="comboBox"/> cần hiển thị
        /// </code>
        /// <code>
        /// <paramref name="tableName"/> tên table cần lấy dữ liệu
        /// </code>
        /// <code>
        /// <paramref name="fieldShow"/> cột cần hiển thị dữ liệu
        /// </code>
        /// <code>
        /// <paramref name="distinct"/> cột cần hiển thị dữ liệu không trùng nhau
        /// </code>
        /// </summary>
        public void LoadComboBoxDataSet(ComboBox comboBox, string tableName, string fieldShow, bool distinct = false)
        {
            try
            {
                string sql = "";
                if (!distinct)
                {
                    sql = "select * from " + tableName;
                }
                else
                {
                    sql = "select distinct " + fieldShow + " from " + tableName;
                }
                
                command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                comboBox.DataSource = dataSet.Tables[tableName];
                comboBox.DisplayMember = fieldShow;
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Đỗ dữ liệu theo disconection
        /// <code>
        /// <paramref name="tableName"/> tên bảng
        /// </code>
        /// <code>
        /// <paramref name="dgvView"/> DataGridView
        /// </code>
        /// <code>
        /// <paramref name="fields"/> Tên các trường cần lấy
        /// </code>
        /// </summary>
        public void LoadDataGridViewDataSet(string tableName, DataGridView dgvView, string[] fields = null)
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
                //string sql = "select * from @tableName";
                command = new SqlCommand(sql.ToString(), connection);
                //string[] parameters = { "tableName" };
                //object[] values = { tableName };
                //ParamsAndValues(command, parameters, values);
                adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet, tableName);
                dgvView.DataSource = dataSet.Tables[tableName];
            }
            catch (Exception ex)
            {
                Utils.MSG(ex.Message);
                return;
            }
        }

        /// <summary>
        /// SqlDataAdapter fill DataTable
        /// <code>
        /// <paramref name="table"/> Tên table
        /// </code>
        /// <code>
        /// <paramref name="fields"/> Tên các trường cần lấy
        /// </code>
        /// </summary>
        public DataTable LoadDataGridViewDataTableOne(string tableName, string[] fields = null)
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
                Utils.MSG(ex.Message);
            }
            return dataTable;
        }

        /// <summary>
        /// DataTable load SqlDataReader
        /// <code>
        /// <paramref name="table"/> Tên table
        /// </code>
        /// <code>
        /// <paramref name="fields"/> Tên các trường cần lấy
        /// </code>
        /// </summary>
        public DataTable LoadDataGridViewDataTableTwo(string tableName, string[] fields = null)
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
                Utils.MSG(ex.Message);
                connection.Close();
            }
            connection.Close();
            return dataTable;
        }
    }
}
