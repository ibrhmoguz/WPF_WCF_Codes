using System.ServiceModel;
using ABC.Servisler.ETicaretServisYeni.Entities;

namespace Customs.Services.ETradeWebService
{
    [ServiceContract(Namespace = "")]
    public interface IETicaretServis
    {
        [OperationContract]
        Sonuc RecieveXml(string objXml, string bilgeKullanici, string imzaliVeri, string refId, string btGuid); 
    }
}
