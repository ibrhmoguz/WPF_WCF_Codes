using System;
using System.Xml.Linq;
using System.ServiceModel.Activation;
using ABC.Servisler.ETicaretServisYeni.BLL;
using ABC.Servisler.ETicaretServisYeni.BLL.Interfaces;
using ABC.Servisler.ETicaretServisYeni.Entities;

namespace Customs.Services.ETradeWebService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in Web.config and in the associated .svc file.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ETicaretServis : IETicaretServis
    {

        #region IETicaretServis Members

        public Sonuc RecieveXml(string objXml, string bilgeKullanici, string imzaliVeri, string refId, string btGuid)
        {
            //System.Diagnostics.Debugger.Break();

            string tmpstr = objXml.ToString().Replace("Dogrulama", "dogrulama");
            objXml = tmpstr;
            
            objSerialize sr = new objSerialize();
            Sonuc sonuc = new Sonuc();
            bsUtilities bsUtil = new bsUtilities();
            bsValidator validator = new bsValidator();
            bsOperate bsOperator = new bsOperate();
            bsKisi bskisi = new bsKisi();
            string objXmlOrj = objXml;

            try
            {
                if (string.IsNullOrEmpty(objXml))
                {
                    throw new Exception("Gönderilen xml boş olamaz!");
                }

                bsUtil.CheckNamespace(objXml);
                objXml = "<?xml version=\"1.0\"?>" + objXml;

                XDocument xDoc = bsUtil.ConvertToXDoc(objXml);
                xDoc.Declaration.Encoding = "UTF-8";

                Type ObjType = bsUtilities.getObjectType(xDoc);
                Type bsType = bsUtilities.getBsType(ObjType);

                validator.ValidateAgainstSchema(objXmlOrj, ObjType.Name);
                                
                Object instance = Activator.CreateInstance(ObjType);
                objXml = objXml.Replace(" xmlns=\"http://tempuri.org/\"", "");
                instance = sr.DeSerializeAnObject(objXml, ObjType);


                IETObjectOperations bsIns = (IETObjectOperations)Activator.CreateInstance(bsType);
                                
                sonuc = bsOperator.Operate(bsIns, instance, bilgeKullanici, btGuid);
            }
            catch (Exception exc)
            {
                sonuc = bsUtil.Parse(exc.Message);
            }
            return sonuc;
        }

        #endregion
    }
}
