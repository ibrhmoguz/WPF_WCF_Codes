using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ABC.Servisler.ETicaretServisYeni.Entities
{
    [DataContract(Namespace = "http://tempuri.org/")]
    public class Sonuc
    {
        private List<Hata> hatalar = new List<Hata>();

        [DataMember(Name="Hatalar")]
        public List<Hata> Hatalar
        {
            get { return hatalar; }
            set { hatalar = value; }
        }
        private string kayitNo;

        [DataMember(Name = "KayitNo")]
        public string KayitNo
        {
            get { return kayitNo; }
            set { kayitNo = value; }
        }
        private DateTime kayitTarihi;

        [DataMember(Name = "KayitTarihi")]
        public DateTime KayitTarihi
        {
            get { return kayitTarihi; }
            set { kayitTarihi = value; }
        }        
    }
}
