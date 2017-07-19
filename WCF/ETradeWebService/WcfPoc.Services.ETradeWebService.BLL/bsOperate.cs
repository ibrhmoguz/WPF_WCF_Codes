using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABC.Servisler.ETicaretServisYeni.BLL.Interfaces;
using Oracle.DataAccess.Client;
using ABC.Servisler.ETicaretServisYeni.Entities;

namespace ABC.Servisler.ETicaretServisYeni.BLL
{
    public class bsOperate
    {
        public Sonuc Operate(IETObjectOperations bsObject, Object instance, string bilgeKullanici, string btGuid)
        { 
            Sonuc sonuc = bsObject.Insert(instance, bilgeKullanici, null, btGuid); // Bu metodu ws içinde değil burda yapmamın sebebi, web servis içerisinde Oracle'a ref vermemek içindi.
            return sonuc;
        }
    }
}
