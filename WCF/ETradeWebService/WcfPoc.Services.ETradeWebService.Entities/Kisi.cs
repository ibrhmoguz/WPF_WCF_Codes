using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABC.Servisler.ETicaretServisYeni.Entities
{
    public class Kisi
    {
        public string adiUnvani { get; set; }

        public string vergiTCNo { get; set; }
    }

    public class AliciKisi
    {
        public string adi { get; set; }

        public string unvani { get; set; }

        public string vergiTCNo { get; set; }

        public string caddeSokakNo { get; set; }

        public string ilKodu { get; set; }

        public string ilce { get; set; }

        public string postaKodu { get; set; }
    }

    public class asilSorumlu : Kisi
    {

    }

    public class gondericiIhracatci : Kisi
    {

    }

    public class tasiyiciTemsilci : Kisi
    {

    }

    public class beyanSahibiTemsilci : Kisi
    {

    }

    public class gonderici : Kisi
    {
    }

    public class alici : Kisi
    {

    }

    public class bildirimTarafi : Kisi
    {
        
    }

    public class acente:Kisi
    {

    }

    public class gdyIsleticisiTemsilci : Kisi
    {

    }
}
