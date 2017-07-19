using System.Runtime.Serialization;

namespace Customs.Services.Container.Types
{
    [DataContract(Namespace = "http://bilgewebservisler.ABC.gov.tr")]
    public class TasimaDetay
    {
        [DataMember(Order = 1)]
        public string TescilNo { get; set; }

        [DataMember(Order = 2)]
        public string TasimaSenediNo { get; set; }

        [DataMember(Order = 3)]
        public int KapAdedi { get; set; }

        [DataMember(Order = 4)]
        public string Turu { get; set; }

        [DataMember(Order = 5)]
        public string KonteynerNo { get; set; }

        [DataMember(Order = 6)]
        public decimal NetAgirlik { get; set; }

        [DataMember(Order = 7)]
        public string EsyaCinsi { get; set; }

        [DataMember(Order = 8)]
        public string BulunduguYer { get; set; }
    }
}