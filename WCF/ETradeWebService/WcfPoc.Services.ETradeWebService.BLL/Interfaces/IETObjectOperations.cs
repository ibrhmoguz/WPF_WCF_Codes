using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABC.Servisler.ETicaretServisYeni.Entities;
using Oracle.DataAccess.Client;

namespace ABC.Servisler.ETicaretServisYeni.BLL.Interfaces
{
    public interface IETObjectOperations
    {
        Sonuc Insert(Object obj, string bilgeKullanicisi, OracleConnection con, string btGuid);

        string Check(string insertId,OracleConnection con);

        Sonuc Update(Object obj);

        Sonuc Delete(string insertId);
    }
}
