using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Customs.Services.Container.Types
{
    [DataContract(Namespace = "http://bilgewebservisler.ABC.gov.tr")]
    public class Sonuc
    {
        private List<Hata> hatalar = new List<Hata>();

        [DataMember(Order = 1)]
        public bool IslemSonucu { get; set; }

        [DataMember(Order = 2)]
        public List<Hata> Hatalar
        {
            get { return hatalar; }
            set { hatalar = value; }
        }

        [DataMember(Order = 3)]
        public string TescilNo { get; set; }
    }

    [DataContract(Namespace = "http://bilgewebservisler.ABC.gov.tr")]
    public class Hata
    {
        [DataMember(Order = 1)]
        public string HataAciklamasi { get; set; }

        [DataMember(Order = 2)]
        public string HataKodu { get; set; }
    }

    [DataContract(Namespace = "http://bilgewebservisler.ABC.gov.tr")]
    public class YerDegisiklikSonuc
    {
        private List<Hata> hatalar = new List<Hata>();

        [DataMember(Order = 1)]
        public bool IslemSonucu { get; set; }

        [DataMember(Order = 2)]
        public List<Hata> Hatalar
        {
            get { return hatalar; }
            set { hatalar = value; }
        }

        [DataMember(Order = 3)]
        public string Aciklama { get; set; }
    }
}