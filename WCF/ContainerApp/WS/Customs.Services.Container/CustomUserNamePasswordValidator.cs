using System.IdentityModel.Selectors;
using System.ServiceModel;
using Customs.Services.Container.BLL;

namespace Customs.Services.Container {
    public class CustomUserNamePasswordValidator : UserNamePasswordValidator {
        public override void Validate(string userName, string password) {
            var konteynerBS = new KonteynerBS();
            bool islemSonucu = false;

            islemSonucu = konteynerBS.KullaniciKontrolu(userName, password);

            if (!islemSonucu) {
                throw new FaultException("User code or password wrong!");
            }
        }
    }
}