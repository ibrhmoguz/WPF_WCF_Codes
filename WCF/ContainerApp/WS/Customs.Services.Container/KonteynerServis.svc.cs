using System.ServiceModel;
using Customs.Services.Container.BLL;
using Customs.Services.Container.Types;

namespace Customs.Services.Container
{
    [ServiceBehavior(Namespace = "http://bilgewebservisler.ABC.gov.tr")]
    public class KonteynerServis : IKonteyner
    {
        #region IKonteyner Members

        public Sonuc TasimaBilgileriKaydet(Tasima tasimalar)
        {
            return new KonteynerBS().TasimaBilgileriniKaydet(tasimalar);
        }

        public YerDegisiklikSonuc KonteynerYeriniDegistir(string bildirimNo, string konteynerNo, string bulunduguYer)
        {
            return new KonteynerBS().KonteynerYeriniDegistir(bildirimNo, konteynerNo, bulunduguYer);
        }

        #endregion
    }
}