using System;
using System.Data;
using Customs.Services.Container.Types;
using ABCWeb.Business;
using ABCWeb.DataAccess;
using ABCWeb.DataType.Common.Attributes;
using Oracle.DataAccess.Client;

namespace Customs.Services.Container.BLL {
    [ServiceConnectionName("KonteynerConnectionString")]
    public class TasimaDetayBS : BusinessBase {
        public void TasimaDetayEkle(TasimaDetay tasimaDetay, string pTasimaID, IData data) {
            string tasimaDetaySiraNo = String.Empty;
            string sqlText = String.Empty;

            if (data == null) {
                data = GetDataObject();
            }

            sqlText = @"SELECT TASIMADETAYSEQ.NEXTVAL FROM DUAL";
            tasimaDetaySiraNo = data.ExecuteScalar(sqlText, CommandType.Text).ToString();

            if (String.IsNullOrEmpty(tasimaDetaySiraNo) || tasimaDetaySiraNo == "0") {
                throw new Exception("Taşıma detay sıra numarası alınamadı!");
            }

            sqlText = @"INSERT INTO KONTEYNER.TASIMA_DETAY (
                                       SIRA_NO
                                       , TESCIL_NO
                                       , TASIMA_SENEDI_NO
                                       , KAP_ADEDI
                                       , TURU
                                       , KONTEYNER_NO
                                       , NET_AGIRLIK
                                       , ESYA_CINSI
                                       , TASIMA_SIRA_NO
                                       , BULUNDUGU_YER) 
                                    VALUES ( 
                                      :SIRA_NO ,
                                      :TESCIL_NO ,
                                      :TASIMA_SENEDI_NO ,
                                      :KAP_ADEDI ,
                                      :TURU ,
                                      :KONTEYNER_NO ,
                                      :NET_AGIRLIK ,
                                      :ESYA_CINSI ,
                                      :TASIMA_SIRA_NO ,
                                      :BULUNDUGU_YER)";

            data.AddOracleParameter("SIRA_NO", Convert.ToInt32(tasimaDetaySiraNo), OracleDbType.Int32, 4000);
            data.AddOracleParameter("TESCIL_NO", tasimaDetay.TescilNo, OracleDbType.Varchar2, 30);
            data.AddOracleParameter("TASIMA_SENEDI_NO", tasimaDetay.TasimaSenediNo, OracleDbType.Varchar2, 20);
            data.AddOracleParameter("KAP_ADEDI", tasimaDetay.KapAdedi, OracleDbType.Int32, 4000);
            data.AddOracleParameter("TURU", tasimaDetay.Turu, OracleDbType.Varchar2, 30);
            data.AddOracleParameter("KONTEYNER_NO", tasimaDetay.KonteynerNo, OracleDbType.Varchar2, 30);
            data.AddOracleParameter("NET_AGIRLIK", tasimaDetay.NetAgirlik, OracleDbType.Decimal, 30);
            data.AddOracleParameter("ESYA_CINSI", tasimaDetay.EsyaCinsi, OracleDbType.Varchar2, 200);
            data.AddOracleParameter("TASIMA_SIRA_NO", Convert.ToInt32(pTasimaID), OracleDbType.Int32, 20);
            data.AddOracleParameter("BULUNDUGU_YER", tasimaDetay.BulunduguYer, OracleDbType.Varchar2, 200);

            data.ExecuteScalar(sqlText, CommandType.Text);
        }
    }
}