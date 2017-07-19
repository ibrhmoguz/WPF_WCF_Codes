using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using ABC.Servisler.ETicaretServisYeni.Entities;
using ABC.Servisler.ETicaretServisYeni.DAL;

namespace ABC.Servisler.ETicaretServisYeni.BLL
{
    public class bsLogger
    {
        public bsLogger() { }


        public void InsertSoapMessage(string soapMessage,string insertId)
        {
            DBConnection dbCon = new DBConnection();
            OracleConnection con = dbCon.getConnection();
            OracleTransaction txn = dbCon.StartTransaction(con);

            OracleCommand command = null;
            Sonuc sonuc = new Sonuc();

            try
            {
                string sql = "INSERT INTO SOAPMESAJ (IMESAJ,GELENMESAJ,GIDENMESAJ,DGELENMESAJ,DGIDENMESAJ) values(" +
                             ":IMESAJ,:GELENMESAJ,:GIDENMESAJ,:DGELENMESAJ,:DGIDENMESAJ) ";

                command = new OracleCommand(sql, con);

                command.Parameters.Add("IMESAJ", OracleDbType.Varchar2, 40);
                command.Parameters.Add("GELENMESAJ", OracleDbType.XmlType);
                command.Parameters.Add("GIDENMESAJ", OracleDbType.XmlType);
                command.Parameters.Add("DGELENMESAJ", OracleDbType.Date);
                command.Parameters.Add("DGIDENMESAJ", OracleDbType.Date);

                command.Parameters["IMESAJ"].Value = insertId;
                command.Parameters["GELENMESAJ"].Value = soapMessage;
                command.Parameters["DGELENMESAJ"].Value = DateTime.Now;
                command.Parameters["GIDENMESAJ"].Value = null;
                command.Parameters["DGIDENMESAJ"].Value = null;

                command.Prepare();
                command.ExecuteNonQuery();

                txn.Commit();
            }
            catch (Exception exc)
            {
                txn.Rollback();
                throw exc;
            }
            finally
            {
                if (txn != null)
                    txn.Dispose();
                if (command != null)
                    command.Dispose();
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        public void UpdateSoapMessage(string soapMessage,string insertId)
        {
            DBConnection dbCon = new DBConnection();
            OracleConnection con = dbCon.getConnection();
            OracleTransaction txn = dbCon.StartTransaction(con);

            OracleCommand command = null;
            Sonuc sonuc = new Sonuc();

            try
            {
                string sql = "UPDATE SOAPMESAJ SET GIDENMESAJ =" +
                             ":GIDENMESAJ , DGIDENMESAJ = :DGIDENMESAJ where IMESAJ='" + insertId + "'";

                command = new OracleCommand(sql, con);

                command.Parameters.Add("GIDENMESAJ", OracleDbType.XmlType);
                command.Parameters.Add("DGIDENMESAJ", OracleDbType.Date);

                command.Parameters["GIDENMESAJ"].Value = soapMessage;
                command.Parameters["DGIDENMESAJ"].Value = DateTime.Now;

                command.Prepare();
                command.ExecuteNonQuery();

                txn.Commit();
            }
            catch (Exception exc)
            {
                txn.Rollback();
                throw exc;
            }
            finally
            {
                if (txn != null)
                    txn.Dispose();
                if (command != null)
                    command.Dispose();
                if (con != null)
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }
    }
}
