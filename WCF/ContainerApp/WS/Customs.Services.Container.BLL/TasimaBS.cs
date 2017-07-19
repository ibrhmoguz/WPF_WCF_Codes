using System;
using System.Data;
using Customs.Services.Container.Types;
using ABCWeb.Business;
using ABCWeb.DataAccess;
using ABCWeb.DataType.Common.Attributes;
using Oracle.DataAccess.Client;

namespace Customs.Services.Container.BLL
{
    [ServiceConnectionName("KonteynerConnectionString")]
    public class TasimaBS : BusinessBase
    {
        public void TasimaEkle(Tasima tasima, IData data)
        {
            var tasimaDetayBS = new TasimaDetayBS();
            string tasimaSiraNo = String.Empty;
            string sqlText = String.Empty;

            if (data == null)
            {
                data = GetDataObject();
            }

            sqlText = @"SELECT TASIMASEQ.NEXTVAL FROM DUAL";
            tasimaSiraNo = data.ExecuteScalar(sqlText, CommandType.Text).ToString();

            if (String.IsNullOrEmpty(tasimaSiraNo) || tasimaSiraNo == "0")
            {
                throw new Exception("Taşıma sıra numarası alınamadı!");
            }

            sqlText = @"INSERT INTO KONTEYNER.TASIMA (
                                SIRA_NO
                                , OZET_BEYAN_NO
                                , TASIT_ADI
                                , SEFER_NO
                                , TASIT_NO
                                , TARIH
                                , BILDIRIM_NO
                                , ISLETME_KODU
                                , YONU
                                , TIPI
                                , KAYITTAR
                                ,TESCIL_NO) 
                            VALUES ( 
                                 :SIRA_NO ,
                                 :OZET_BEYAN_NO ,
                                 :TASIT_ADI ,
                                 :SEFER_NO ,
                                 :TASIT_NO ,
                                 :TARIH ,
                                 :BILDIRIM_NO ,
                                 :ISLETME_KODU ,
                                 :YONU ,
                                 :TIPI ,
                                 SYSDATE,
                                 :TESCIL_NO)";

            data.AddOracleParameter("SIRA_NO", Convert.ToInt32(tasimaSiraNo), OracleDbType.Int32, 4000);
            data.AddOracleParameter("OZET_BEYAN_NO", tasima.OzetBeyanNo, OracleDbType.Varchar2, 30);
            data.AddOracleParameter("TASIT_ADI", tasima.TasitAdi, OracleDbType.Varchar2, 50);
            data.AddOracleParameter("SEFER_NO", tasima.SeferNo, OracleDbType.Varchar2, 20);
            data.AddOracleParameter("TASIT_NO", tasima.TasitNo, OracleDbType.Varchar2, 20);
            data.AddOracleParameter("TARIH", tasima.GelisGidisTarihi, OracleDbType.Date, 30);
            data.AddOracleParameter("BILDIRIM_NO", tasima.GelisGidisBildirimNo, OracleDbType.Varchar2, 20);
            data.AddOracleParameter("ISLETME_KODU", tasima.IsletmeKodu, OracleDbType.Varchar2, 20);
            data.AddOracleParameter("YONU", tasima.Yonu, OracleDbType.Varchar2, 8);
            data.AddOracleParameter("TIPI", tasima.Tipi, OracleDbType.Varchar2, 2);
            data.AddOracleParameter("TESCIL_NO", tasima.tescilNo, OracleDbType.Varchar2, 20);

            data.ExecuteScalar(sqlText, CommandType.Text);

            foreach (TasimaDetay tasimaDetay in tasima.tasimaDetaylari)
            {
                if (tasimaDetay != null)
                {
                    tasimaDetayBS.TasimaDetayEkle(tasimaDetay, tasimaSiraNo, data);
                }
            }
        }
    }
}