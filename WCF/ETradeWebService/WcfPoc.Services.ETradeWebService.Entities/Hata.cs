using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ABC.Servisler.ETicaretServisYeni.Entities
{
    [DataContract(Namespace = "http://tempuri.org/")]
    public class Hata
    {
        private string hataKodu;

        [DataMember(Name = "HataKodu")]
        public string HataKodu
        {
            get { return hataKodu; }
            set { hataKodu = value; }
        }
        private string hataAciklamasi;

        [DataMember(Name = "HataAciklamasi")]
        public string HataAciklamasi
        {
            get { return hataAciklamasi; }
            set { hataAciklamasi = value; }
        }
    }
}
