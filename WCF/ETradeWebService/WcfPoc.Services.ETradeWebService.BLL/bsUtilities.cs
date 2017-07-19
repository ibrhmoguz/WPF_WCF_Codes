using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ABC.Servisler.ETicaretServisYeni.Entities;
using ABC.Servisler.ETicaretServisYeni.DAL;
using Oracle.DataAccess.Client;
using System.Data;
using System.Globalization;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace ABC.Servisler.ETicaretServisYeni.BLL
{
    public class bsUtilities
    {
        public string getmeaningfulErrorMessage(string hataKodu)
        {
            string[] parametreler = null;
            DBConnection db = new DBConnection();
            OracleConnection con = db.getConnection();
            OracleCommand cmd = con.CreateCommand();
            DataSet ds = new DataSet();

            if (hataKodu.Contains("/"))
            {
                parametreler = hataKodu.Split(new char[] { '/' });
            }

            if (parametreler != null && parametreler.Length > 0)
            {
                hataKodu = parametreler[0];
            }
            // Issue 1540 'a göre v_aykd_tanim kullanılmaya başlandı
            //string sSql = "SELECT * FROM AYKD.AYKD_TANIM WHERE DISAYKDKODU='" + hataKodu + "'";
            //string sSql = "SELECT * FROM ETICARET.v_aykd_tanim WHERE DISAYKDKODU='" + hataKodu + "'";
            string sSql = "SELECT * FROM ETICARET.V_AYKD_TANIM WHERE DISAYKDKODU='" + hataKodu + "'";
            ds = db.FillDs(sSql, ds, 0, con);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return "Hata Kodu Karşılığı yok!";
            }
            string strAnlamli = ds.Tables[0].Rows[0][4].ToString();

            int index = 0;
            while (strAnlamli.Contains("%s"))
            {
                int occuranceIndex = strAnlamli.IndexOf("%s");
                strAnlamli = strAnlamli.Substring(0, occuranceIndex) + "{" + index.ToString() + "}" + strAnlamli.Substring(occuranceIndex + 2, strAnlamli.Length - occuranceIndex - 2);
                index++;
            }

            if (parametreler != null && parametreler.Length == 3)
            {
                return String.Format(strAnlamli, parametreler[1], parametreler[2]);
            }
            else if (parametreler != null && parametreler.Length == 2)
            {
                return String.Format(strAnlamli, parametreler[1]);
            }

            return strAnlamli;
        }

        public Sonuc Parse(string excStr)
        {
            Sonuc sonuc = new Sonuc();
            Hata hata;

            sonuc.KayitTarihi = DateTime.Now;
            sonuc.KayitNo = "Hata";
            sonuc.Hatalar = new List<Hata>();
            bsUtilities bs = new bsUtilities();
            if (excStr.Contains('|') || excStr.Contains("_ERR_") || excStr.Contains("ERR"))
            {
                if (excStr.StartsWith("|"))
                {
                    string newStr = excStr.Substring(1, excStr.Length - 1);
                    excStr = newStr;
                }

                string[] hatalar = excStr.Split(new char[] { '|' });
                foreach (string strHata in hatalar)
                {
                    hata = new Hata();
                    if (strHata.StartsWith("$"))
                    {
                        hata.HataAciklamasi = "Hatalı Kayıt: " + strHata.Substring(1, strHata.Length - 1);
                    }
                    else
                    {
                        hata.HataAciklamasi = bs.getmeaningfulErrorMessage(strHata);
                    }
                    sonuc.Hatalar.Add(hata);
                }
            }
            else // AYKD ile ilgli bir hata değil de programdan gelen bir hata ise olduğu gibi yaz.
            {
                hata = new Hata();
                hata.HataAciklamasi = excStr;
                sonuc.Hatalar.Add(hata);
            }
            sonuc.KayitTarihi = DateTime.MinValue;
            return sonuc;
        }

        public static decimal toDecimal(string value)
        {
            decimal result;
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("tr-TR");
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();
            nfi.NumberDecimalDigits = 2;
            nfi.NumberDecimalSeparator = ".";
            decimal.TryParse(value, NumberStyles.Any, nfi, out result);
            return result;
        }

        public XDocument ConvertToXDoc(string objXml)
        {
            XDocument xmlObj = new XDocument();
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(objXml);
                xmlObj = XDocument.Parse(doc.OuterXml);
            }
            catch (Exception exc)
            {
                throw new Exception("Gönderilen XML hatalı! Kontrol edip tekrar deneyiniz.");
            }
            return xmlObj;
        }

        public void CheckNamespace(string objXml)
        {
            bool nameSpaceAvailable = false;

            if (objXml.StartsWith("<TCGBTescil xmlns=\"http://tempuri.org/\">"))
            {
                nameSpaceAvailable = true;
            }
            else if (objXml.StartsWith("<TCGB xmlns=\"http://tempuri.org/\">"))
            {
                nameSpaceAvailable = true;
            }
            else if (objXml.StartsWith("<tamamlayiciBeyan xmlns=\"http://tempuri.org/\">"))
            {
                nameSpaceAvailable = true;
            }
            else if (objXml.StartsWith("<BosaltmaListesi xmlns=\"http://tempuri.org/\">"))
            {
                nameSpaceAvailable = true;
            }
          
            if (!nameSpaceAvailable)
            {
                throw new Exception("Xml namespace hatası. Örnek kullanım : <TCGB xmlns=\"http://tempuri.org/\">");
            }
        }

        public static Type getBsType(Type ObjType)
        {
            Type bsType = null;
            switch (ObjType.Name)
            {
                case "TCGB":
                    bsType = typeof(bsTcgb);
                    break;

                case "TCGBTescil":
                    bsType = typeof(bsTcgbTescil);
                    break;

                case "tamamlayiciBeyan":
                    bsType = typeof(bsTamamlayiciBeyan);
                    break;

                case "BosaltmaListesi":
                    bsType = typeof(bsBosaltmaListesi);
                    break;

                default:
                    throw new Exception("Gönderilen XML tipi bulunamadı!");
            }

            return bsType;
        }

        public static Type getObjectType(XDocument objXml)
        {
            string xmlTipi = getXmlType(objXml);
            Type ObjType;

            switch (xmlTipi)
            {
                case "TCGB":
                    ObjType = typeof(TCGB);
                    break;
                case "TCGBTescil":
                    ObjType = typeof(TCGBTescil);
                    break;
                case "tamamlayiciBeyan":
                    ObjType = typeof(tamamlayiciBeyan);
                    break;

                case "BosaltmaListesi":
                    ObjType = typeof(BosaltmaListesi);
                    break;

                default:
                    ObjType = null;
                    throw new Exception("Gönderdiğiniz XML tipi belirlenemedi!");
            }

            if (ObjType == null)
                throw new Exception("Gönderilen Xml beklenen fotmatta değil!");

            return ObjType;
        }

        public static string getXmlType(XDocument objXml)
        {
            string tip = string.Empty;

            switch (objXml.Root.Name.LocalName)
            {               
                case "TCGBTescil":
                    tip = "TCGBTescil";
                    break;
                case "TCGB":
                    tip = "TCGB";
                    break;
                case "tamamlayiciBeyan":
                    tip = "tamamlayiciBeyan";
                    break;
                case "BosaltmaListesi":
                    tip = "BosaltmaListesi";
                    break;
            }
            return tip;
        }

        public string KullaniciYetkiKontrol(string islemYapanTCNo, string islemYapilanFirmaVergiNo, OracleConnection con)
        {
            try
            {                
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "F_KULLANICI_YETKI_KONTROL";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = con;

                OracleParameter retval = new OracleParameter("myretval", OracleDbType.Varchar2, 2000);
                retval.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(retval);

                cmd.Parameters.Add(new OracleParameter("islemYapanTCNo", OracleDbType.Varchar2, 40)).Value = islemYapanTCNo;
                cmd.Parameters.Add(new OracleParameter("islemYapilanFirmaVergiNo", OracleDbType.Varchar2, 40)).Value = islemYapilanFirmaVergiNo;
                cmd.ExecuteNonQuery();
                
                return cmd.Parameters["myretval"].Value.ToString();
            }
            catch (Exception exc)
            {
                throw new Exception("Kullanıcı yetki prosedüründe hata oluştu!");
            }
        }
    }
}
