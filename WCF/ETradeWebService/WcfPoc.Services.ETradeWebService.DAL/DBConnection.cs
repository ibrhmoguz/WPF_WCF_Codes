using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using System.Data;
using System.Configuration;

namespace ABC.Servisler.ETicaretServisYeni.DAL
{
    public class DBConnection
    {
        OracleDataAdapter oraAdap;

        public DBConnection()
        {

        }

        public OracleConnection getConnection()
        {
            try
            {
                string constr = getEticareConnectionString();

                OracleConnection con = new OracleConnection(constr);

                con.Open();
                return con;

            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        public OracleTransaction StartTransaction(OracleConnection con)
        {
            try
            {
                OracleTransaction tnx = con.BeginTransaction(IsolationLevel.ReadCommitted);
                return tnx;
            }
            catch (Exception)
            {
                throw new Exception("Transaction başlatma hatası!");
            }
        }

        public DataSet FillDs(string sqltext, DataSet ds, int table, OracleConnection OraCon)
        {
            try
            {
                if (ds.Tables.Count < 1)
                    ds.Tables.Add();
                oraAdap = new OracleDataAdapter(sqltext, OraCon);
                oraAdap.Fill(ds.Tables[table]);
                return ds;
            }
            catch (Exception exc)
            {
                throw new Exception("Fill Sırasında Data Katmanında Hata Oluştu.");
            }
        }

        public DataSet getTableRowById(string tableName, string tableIdRowName, string tableRowId, OracleConnection con)
        {
            OracleCommand cmd = con.CreateCommand();
            try
            {
                DBConnection db = new DBConnection();

                DataSet ds = new DataSet();
                string sSql = "SELECT * FROM " + tableName + " WHERE " + tableIdRowName + "='" + tableRowId + "'";
                ds = db.FillDs(sSql, ds, 0, con);

                return ds;
            }
            catch (Exception)
            {
                throw new Exception("Id karşılığı değer üretme hatası oluştu!");
            }
            finally
            {
                cmd.Dispose();
            }
        }

        public string getConnectionString(string dbName)
        {
            return ConfigurationManager.ConnectionStrings["ABC.ETicaret.DataAccess.Properties.Settings.OsofixConnectionString"].ToString();
        }

        private string getEticareConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ABC.ETicaret.DataAccess.Properties.Settings.ConnectionString"].ToString();
        }
    }
}
