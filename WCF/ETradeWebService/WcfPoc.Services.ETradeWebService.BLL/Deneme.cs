using System;
using ABC.Servisler.ETicaretServisYeni.BLL.Interfaces;
using Oracle.DataAccess.Client;

namespace ABC.Servisler.ETicaretServisYeni.BLL
{
    class Deneme : IETObjectOperations
    {
        #region IETObjectOperations Members

        public Entities.Sonuc Insert(object obj, string bilgeKullanicisi, OracleConnection con, string btGuid)
        {
            throw new NotImplementedException();
        }

        public string Check(string insertId, OracleConnection con)
        {
            throw new NotImplementedException();
        }


        public Entities.Sonuc Update(object obj)
        {
            throw new NotImplementedException();
        }

        public Entities.Sonuc Delete(string insertId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
