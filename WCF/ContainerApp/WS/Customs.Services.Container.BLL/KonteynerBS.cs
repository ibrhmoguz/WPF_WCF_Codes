using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Customs.Services.Container.Types;
using ABCWeb.Business;
using ABCWeb.DataAccess;
using ABCWeb.DataType.Common.Attributes;
using Oracle.DataAccess.Client;

namespace Customs.Services.Container.BLL
{
    [ServiceConnectionName("KonteynerConnectionString")]
    public class KonteynerBS : BusinessBase
    {
        public Sonuc TasimaBilgileriniKaydet(Tasima tasima)
        {
            string konteynerNoKontrolSonucu = String.Empty;
            IData data = GetDataObject();
            Sonuc sonuc;

            try
            {
                if (tasima == null)
                {
                    return HataliSonucOlustur("Taşıma bilgileri eksik!");
                }

                if (tasima.tasimaDetaylari == null || tasima.tasimaDetaylari.Length == 0 || tasima.tasimaDetaylari[0] == null)
                {
                    return HataliSonucOlustur("Taşıma detay bilgileri eksik!");
                }

                string varmisonuc = VerileriKontrolEt(tasima.GelisGidisBildirimNo, tasima.TasitNo);

                if (varmisonuc == "false")
                {
                    return HataliSonucOlustur("Girmiş olduğunuz bildirim no veya taşıt no sistemimizde bulunmamaktadır.");
                }

                if (String.IsNullOrEmpty(tasima.GelisGidisBildirimNo))
                {
                    return HataliSonucOlustur("Bildirim No alanı boş olamaz!");
                }

                if (tasima.GelisGidisBildirimNo.Length < 8)
                {
                    return HataliSonucOlustur("BildirimNo bilgisi format hatası!");
                }

                if (!IsletmeKoduKontrolu(tasima))
                {
                    return HataliSonucOlustur("İşletme kodu sistemde kayıtlı değil!");
                }

                if (!BildirimNoKontrolEt(tasima.GelisGidisBildirimNo))
                {
                    return HataliSonucOlustur("Var olan bildirim numarasını eklemeye çalışıyorsunuz. Taşıma bildirim numaralarını kontrol ediniz!");
                }

                foreach (TasimaDetay detay in tasima.tasimaDetaylari)
                {
                    konteynerNoKontrolSonucu = KonteynerNoKontrolEt(detay.KonteynerNo);
                    if (konteynerNoKontrolSonucu != String.Empty)
                    {
                        return HataliSonucOlustur(konteynerNoKontrolSonucu);
                    }
                }


                var tasimaBS = new TasimaBS();
                sonuc = new Sonuc();

                data.BeginTransaction();

                string tescilNo = TescilNumarasiAl(tasima.GelisGidisBildirimNo, data);
                tasima.tescilNo = tescilNo;
                sonuc.TescilNo = tescilNo;
                tasimaBS.TasimaEkle(tasima, data);

                data.CommitTransaction();
                sonuc.IslemSonucu = true;
            }
            catch (Exception exc)
            {
                data.RollbackTransaction();
                sonuc = HataliSonucOlustur(exc.Message);

                //File.WriteAllText(@"C:\Logs\" + Guid.NewGuid() + ".txt", "Hata Detayı = " + exc.Message + " " + exc.StackTrace);
            }

            return sonuc;
        }

        public YerDegisiklikSonuc KonteynerYeriniDegistir(string bildirimNo, string konteynerNo, string bulunduguYer)
        {
            string konteynerNoKontrolSonucu = String.Empty;
            YerDegisiklikSonuc sonuc;

            try
            {

                if (String.IsNullOrEmpty(bildirimNo))
                {
                    return YerDegisiklikSonucOlustur("Değişiklik yapılmadı", "Bildirim No alanı boş olamaz!");
                }

                if (bildirimNo.Length < 8)
                {
                    return YerDegisiklikSonucOlustur("Değişiklik yapılmadı", "BildirimNo bilgisi format hatası!");
                }

                if (BildirimNoKontrolEt(bildirimNo))
                {
                    return YerDegisiklikSonucOlustur("Değişiklik yapılmadı", "Sistemde olmayan bildirim numarasını gönderdiniz!");
                }

                konteynerNoKontrolSonucu = KonteynerNoKontrolEt(konteynerNo);

                if (konteynerNoKontrolSonucu != String.Empty)
                {
                    return YerDegisiklikSonucOlustur("Değişiklik yapılmadı", konteynerNoKontrolSonucu);
                }

                if (YerDegisiklikKonteynerNoKontrolEt(bildirimNo, konteynerNo))
                {
                    return YerDegisiklikSonucOlustur("Değişiklik yapılmadı", "Beyan ettiğiniz bildirim numarasına karşılık sistemde olmayan konteyner numarası gönderdiniz!");
                }

                sonuc = new YerDegisiklikSonuc();
                IData data = GetDataObject();

                string sorgu = @"update TASIMA_DETAY set BULUNDUGU_YER = :bulunduguyer 
                                 where TASIMA_SIRA_NO = (select SIRA_NO from TASIMA where BILDIRIM_NO = :bildirimno) 
                                       and KONTEYNER_NO=:konteynerno";

                data.AddOracleParameter("bulunduguyer", bulunduguYer, OracleDbType.Varchar2, 200);
                data.AddOracleParameter("bildirimno", bildirimNo, OracleDbType.Varchar2, 20);
                data.AddOracleParameter("konteynerno", konteynerNo, OracleDbType.Varchar2, 30);

                int islemSonucu = data.ExecuteStatement(sorgu, CommandType.Text);

                if (islemSonucu == 0)
                    sonuc = YerDegisiklikSonucOlustur("Değişiklik yapılmadı", "Güncellenecek kayıt bulunamadı!");
                else
                {
                    sonuc.IslemSonucu = true;
                    sonuc.Aciklama = "Değişiklik yapıldı";
                }
            }
            catch (Exception exc)
            {
                sonuc = YerDegisiklikSonucOlustur("Değişiklik yapılmadı", exc.Message);
            }

            return sonuc;
        }

        private string KonteynerNoKontrolEt(string konteynerNo)
        {
            string sonuc = String.Empty;
            string kontrolSonucu = String.Empty;
            IData oData = GetDataObject();

            string fonksiyon = "Select OSOFIX.F_CNT_CTRL(:KONTEYNER_NO,:KAYNAK) from DUAL";

            oData.AddOracleParameter("KONTEYNER_NO", konteynerNo, OracleDbType.Varchar2, 20);
            oData.AddOracleParameter("KAYNAK", "BIL", OracleDbType.Varchar2, 20);

            object obj = oData.ExecuteScalar(fonksiyon, CommandType.Text);
            kontrolSonucu = obj.ToString();

            if (kontrolSonucu == "LTIINF005")
                sonuc = "Konteyner Numarası 11 haneden oluşmalıdır.";
            if (kontrolSonucu == "LTIINF006")
                sonuc = "Konteyner Numarası hatalıdır.";

            return sonuc;
        }

        private bool IsletmeKoduKontrolu(Tasima tasimalar)
        {
            bool islemSonucu = false;
            string isletmeKodlari = tasimalar.IsletmeKodu;
            var sb = new StringBuilder();
            IData data = GetDataObject();

            try
            {
                sb.Append(@"SELECT DISTINCT ISLETME_KODU FROM KONTEYNER.LIMAN_BILGI WHERE ISLETME_KODU IN(");
                sb.Append("'");
                sb.Append(isletmeKodlari);
                sb.Append("'");
                sb.Append(")");

                DataSet ds = data.GetRecords(sb.ToString());
                islemSonucu = (ds != null && ds.Tables.Count != 0 && ds.Tables[0] != null);
            }
            catch
            {
                islemSonucu = false;
            }

            return islemSonucu;
        }

        public bool KullaniciKontrolu(string kullaniciAdi, string password)
        {
            bool islemSonucu = false;

            try
            {
                IData data = GetDataObject();

                const string sqlText = @"SELECT COUNT(*) FROM KONTEYNER.LIMAN_BILGI WHERE KULLANICI_ADI=:KULLANICI_ADI AND SIFRE=:SIFRE";

                data.AddOracleParameter("KULLANICI_ADI", kullaniciAdi, OracleDbType.Varchar2, 50);
                data.AddOracleParameter("SIFRE", password, OracleDbType.Varchar2, 25);

                DataSet ds = data.GetRecords(sqlText);

                islemSonucu = (ds != null && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0);
            }
            catch
            {
                islemSonucu = false;
            }

            return islemSonucu;
        }

        private string VerileriKontrolEt(string bildirimno, string tasitno)
        {
            IData oData = null;
            oData = GetDataObject();
            string sonuc = "false";

            try
            {
                const string fonksiyon = "Select OSOFIX.F_VARONC_CTRL(:BILDIRIM_NO,:TASIT_NO) from DUAL";

                oData.AddOracleParameter("BILDIRIM_NO", bildirimno, OracleDbType.Varchar2, 20);
                oData.AddOracleParameter("TASIT_NO", tasitno, OracleDbType.Varchar2, 20);

                object obj = oData.ExecuteScalar(fonksiyon, CommandType.Text);
                sonuc = obj.ToString();
            }
            catch
            {
                sonuc = "false";
            }


            return sonuc;
        }

        private bool BildirimNoKontrolEt(string bildirimNo)
        {
            IData oData = null;
            oData = GetDataObject();
            bool sonuc = false;

            string sql = "Select count(*) from KONTEYNER.TASIMA WHERE BILDIRIM_NO =:BILDIRIM_NO";
            oData.AddOracleParameter("BILDIRIM_NO", bildirimNo, OracleDbType.Varchar2, 20);

            object obj = oData.ExecuteScalar(sql, CommandType.Text);

            if (obj != null && Int32.Parse(obj.ToString()) == 0)
            {
                sonuc = true;
            }

            return sonuc;
        }

        private bool YerDegisiklikKonteynerNoKontrolEt(string bildirimNo, string konteynerNo)
        {
            IData oData = null;
            oData = GetDataObject();
            bool sonuc = false;

            string sql = @"SELECT count(*) from KONTEYNER.TASIMA_DETAY
                           WHERE TASIMA_SIRA_NO = (select SIRA_NO from TASIMA where BILDIRIM_NO = :bildirimno) 
                                 AND KONTEYNER_NO=:konteynerno";

            oData.AddOracleParameter("bildirimno", bildirimNo, OracleDbType.Varchar2, 20);
            oData.AddOracleParameter("konteynerno", konteynerNo, OracleDbType.Varchar2, 30);

            object obj = oData.ExecuteScalar(sql, CommandType.Text);

            if (obj != null && Int32.Parse(obj.ToString()) == 0)
            {
                sonuc = true;
            }

            return sonuc;
        }

        private string TescilNumarasiAl(string bildirimno, IData oData)
        {
            string sonuc = String.Empty;
            int yil = DateTime.Now.Year;

            string ABCKodu = bildirimno.Substring(2, 6);

            oData.AddOracleParameter("ABCkodu", ABCKodu, OracleDbType.Varchar2, 6);
            oData.AddOracleParameter("yil", yil, OracleDbType.Int32, 8);
            oData.AddOracleParameter("po_newid", OracleDbType.Int32, ParameterDirection.Output, 100);
            Dictionary<string, object> result = oData.ExecuteStatementUDI("GETNEWKAYITNO");

            return DateTime.Now.Year + ABCKodu + "KL" + result["po_newid"];
        }

        private Sonuc HataliSonucOlustur(string hataAciklama)
        {
            var sonuc = new Sonuc();
            var h = new Hata();
            h.HataAciklamasi = hataAciklama;
            sonuc.Hatalar.Add(h);
            sonuc.IslemSonucu = false;
            sonuc.TescilNo = String.Empty;
            return sonuc;
        }

        private YerDegisiklikSonuc YerDegisiklikSonucOlustur(string aciklama, string hataAciklama)
        {
            var sonuc = new YerDegisiklikSonuc();
            var h = new Hata();
            h.HataAciklamasi = hataAciklama;
            sonuc.Hatalar.Add(h);
            sonuc.IslemSonucu = false;
            sonuc.Aciklama = aciklama;
            return sonuc;
        }
    }
}