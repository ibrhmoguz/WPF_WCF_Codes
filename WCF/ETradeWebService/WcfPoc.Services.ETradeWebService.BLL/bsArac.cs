using System;
using ABC.Servisler.ETicaretServisYeni.BLL.Interfaces;
using ABC.Servisler.ETicaretServisYeni.Entities;
using Oracle.DataAccess.Client;

namespace ABC.Servisler.ETicaretServisYeni.BLL
{
    public class bsArac : IETObjectOperations
    {
        #region IETObjectOperations Members
  
        public string EtgbNo {get;set;}
        public string Konum { get; set; }

        public Sonuc Insert(object obj, string bilgeKullanicisi, OracleConnection con, string btGuid)
        {
            arac arc = (arac)obj;
            OracleCommand cmd = con.CreateCommand();
            bsNoCreator noCreator = new bsNoCreator();          

            try
            {
                string aracInsertId = noCreator.CreateNewInsertId("ARAC", "arc", con);

                string sql = "INSERT INTO ARAC" +
                             "(IARC,IARCETGBNO,IARCKONUM,CARCTIP,LARCNO,CARCULK)" +
                             "VALUES(:IARC,:IARCETGBNO,:IARCKONUM,:CARCTIP,:LARCNO,:CARCULK)";

                OracleCommand command = new OracleCommand(sql, con);
               
                command.Parameters.Add("IARC", OracleDbType.Varchar2, 20);
                command.Parameters.Add("IARCETGBNO", OracleDbType.Varchar2, 20);
                command.Parameters.Add("IARCKONUM", OracleDbType.Varchar2, 20);
                command.Parameters.Add("CARCTIP", OracleDbType.Varchar2, 9);
                command.Parameters.Add("LARCNO", OracleDbType.Varchar2, 50);
                command.Parameters.Add("CARCULK", OracleDbType.Varchar2, 9);

                command.Parameters["IARC"].Value = aracInsertId;
                command.Parameters["IARCETGBNO"].Value = EtgbNo;
                command.Parameters["IARCKONUM"].Value = Konum;
                command.Parameters["CARCTIP"].Value = arc.tipi;
                command.Parameters["LARCNO"].Value = arc.numarasi;
                command.Parameters["CARCULK"].Value = arc.ulke;

                command.Prepare();
                command.ExecuteNonQuery();

                string checkResult = Check(aracInsertId, con);
                if (!checkResult.Equals("null") && !string.IsNullOrEmpty(checkResult))
                {
                    throw new Exception(checkResult);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
            Sonuc sonuc = new Sonuc();
            return sonuc;
        }

        public string Check(string insertId, OracleConnection con)
        {
            return "";
        }

        public Sonuc Update(object obj)
        {
            throw new NotImplementedException();
        }

        public Sonuc Delete(string insertId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
