using System.ServiceModel;
using Customs.Services.Container.Types;

namespace Customs.Services.Container
{
    [ServiceContract(Namespace = "http://bilgewebservisler.ABC.gov.tr")]
    public interface IKonteyner
    {
        [OperationContract]
        Sonuc TasimaBilgileriKaydet(Tasima tasimalar);

        [OperationContract]
        YerDegisiklikSonuc KonteynerYeriniDegistir(string bildirimNo, string konteynerNo, string bulunduguYer);
    }
}