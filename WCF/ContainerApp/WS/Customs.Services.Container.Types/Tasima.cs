using System;
using System.Runtime.Serialization;

namespace Customs.Services.Container.Types {
    [DataContract(Namespace = "http://bilgewebservisler.ABC.gov.tr")]
    public class Tasima {
        [DataMember(Order = 1)]
        public string OzetBeyanNo { get; set; }

        [DataMember(Order = 2)]
        public string TasitAdi { get; set; }

        [DataMember(Order = 3)]
        public string SeferNo { get; set; }

        [DataMember(Order = 4)]
        public string TasitNo { get; set; }

        [DataMember(Order = 5)]
        public DateTime GelisGidisTarihi { get; set; }

        [DataMember(Order = 6)]
        public string GelisGidisBildirimNo { get; set; }

        [DataMember(Order = 7)]
        public string IsletmeKodu { get; set; }

        [DataMember(Order = 8)]
        public string Tipi { get; set; }

        [DataMember(Order = 9)]
        public string Yonu { get; set; }

        [DataMember(Order = 10)]
        public TasimaDetay[] tasimaDetaylari { get; set; }

        [DataMember(Order = 11)]
        public string tescilNo { get; set; }
    }
}