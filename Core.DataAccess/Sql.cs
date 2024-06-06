using System;
using System.Data;
using System.Data.SqlClient;

namespace Core.DataAccess
{
    public class Sql 
    {
        public ConnectionInfo Connection { get; set; }

        public Sql()
        {
            Connection = new ConnectionInfo();
            Connection.DataSource = string.Empty;
            Connection.InitialCatalog = string.Empty;
            Connection.User = string.Empty;
            Connection.Password = string.Empty;
        }

        /// <summary>
        /// Ejecuta sentencias UPDATE, DELETE
        /// </summary>
        /// <param name="strSql"></param>
        public int Execute(string strSql)
        {
            SqlCommand SqlComand = new SqlCommand();
            SqlConnection SqlConnect = new SqlConnection(Connection.StrConnection);
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();

            try
            {
                SqlConnect.Open();

                if (SqlConnect.State == ConnectionState.Open)
                {
                    SqlComand.CommandType = CommandType.Text;
                    SqlComand.CommandText = strSql;
                    SqlComand.Connection = SqlConnect;
                    SqlComand.CommandTimeout = 120;
                    SqlAdapter.SelectCommand = SqlComand;

                    return SqlComand.ExecuteNonQuery();
                }

                return -1;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
                SqlComand.Dispose();
                SqlConnect.Dispose();
            }
        }

        /// <summary>
        /// Ejecuta sentencias SELECT
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public DataSet GetData(string strSql)
        {
            try
            {
                SqlCommand SqlComand = new SqlCommand();
                SqlConnection SqlConnect = new SqlConnection(Connection.StrConnection);
                SqlDataAdapter SqlAdapter = new SqlDataAdapter();
                DataSet SqlDataSet = new DataSet();

                SqlConnect.Open();

                if (SqlConnect.State == ConnectionState.Open)
                {
                    SqlComand.CommandType = CommandType.Text;
                    SqlComand.CommandText = strSql;
                    SqlComand.Connection = SqlConnect;
                    SqlComand.CommandTimeout = 120;
                    SqlAdapter.SelectCommand = SqlComand;
                    SqlAdapter.Fill(SqlDataSet);
                }
                SqlConnect.Close();
                return SqlDataSet;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Ejecuta Stored Procedures
        /// </summary>
        /// <param name="nameSP"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExecuteSP(string nameSP, SqlParameter[] param)
        {
            SqlCommand SqlComand = new SqlCommand();
            SqlConnection SqlConnect = new SqlConnection(Connection.StrConnection);
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();
            DataSet SqlDataSet = new DataSet();
                
            try
            {
                SqlConnect.Open();
                if (SqlConnect.State == ConnectionState.Open)
                {
                    SqlComand.CommandType = CommandType.StoredProcedure;
                    SqlComand.CommandText = nameSP;
                    SqlComand.Connection = SqlConnect;
                    SqlComand.CommandTimeout = 120;
                    SqlComand.Parameters.AddRange(param);
                            
                    SqlAdapter.SelectCommand = SqlComand;
                    SqlAdapter.Fill(SqlDataSet);
                }
                return SqlDataSet.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
        }

        /// <summary>
        /// Ejecuta Stored Procedures
        /// </summary>
        /// <param name="nameSP"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExecuteSP(string nameSP, SqlParameter param)
        {
            SqlCommand SqlComand = new SqlCommand();
            SqlConnection SqlConnect = new SqlConnection(Connection.StrConnection);
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();
            DataSet SqlDataSet = new DataSet();

            try
            {
                SqlConnect.Open();
                if (SqlConnect.State == ConnectionState.Open)
                {
                    SqlComand.CommandType = CommandType.StoredProcedure;
                    SqlComand.CommandText = nameSP;
                    SqlComand.Connection = SqlConnect;
                    SqlComand.CommandTimeout = 120;
                    SqlComand.Parameters.Add(param);

                    SqlAdapter.SelectCommand = SqlComand;
                    SqlAdapter.Fill(SqlDataSet);
                }
                return SqlDataSet.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
        }

        /// <summary>
        /// Ejecuta Stored Procedures
        /// </summary>
        /// <param name="nameSP"></param>
        /// <returns></returns>
        public DataTable ExecuteSP(string nameSP)
        {
            SqlCommand SqlComand = new SqlCommand();
            SqlConnection SqlConnect = new SqlConnection(Connection.StrConnection);
            SqlDataAdapter SqlAdapter = new SqlDataAdapter();
            DataSet SqlDataSet = new DataSet();

            try
            {
                SqlConnect.Open();
                if (SqlConnect.State == ConnectionState.Open)
                {
                    SqlComand.CommandType = CommandType.StoredProcedure;
                    SqlComand.CommandText = nameSP;
                    SqlComand.Connection = SqlConnect;
                    SqlComand.CommandTimeout = 120;

                    SqlAdapter.SelectCommand = SqlComand;
                    SqlAdapter.Fill(SqlDataSet);
                }
                return SqlDataSet.Tables[0];
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                SqlConnect.Close();
            }
        }

        public class Transaction
        {
            SqlCommand SqlComand = new SqlCommand();
            SqlTransaction SqlTrans;
            SqlConnection SqlConnect;

            public Transaction(string Connection)
            {
                SqlConnect = new SqlConnection(Connection);
                SqlConnect.Open();

                if (SqlConnect.State == ConnectionState.Open)
                {
                    SqlTrans = SqlConnect.BeginTransaction();
                    SqlComand.Connection = SqlConnect;
                    SqlComand.Transaction = SqlTrans;
                    SqlComand.CommandType = CommandType.Text;
                }
                else
                    throw new Exception("Cannot open database connection.");
            }

            public void Execute(string query)
            {
                SqlComand.CommandText = query;
                SqlComand.ExecuteNonQuery();
            }

            public void Commit()
            {
                try
                {
                    SqlTrans.Commit();
                }
                catch (Exception ex)
                {
                    SqlTrans.Rollback();
                    throw ex;
                }
                finally
                {
                    SqlConnect.Close();
                }
            }
        }
    }
}
