using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ABCWeb.Business.PersonelAlim;
using System.Threading;

namespace EmployeeDesignation
{
    public class AtamaBusiness
    {
        public int sayac = 0;
        string alimNo = String.Empty;

        public AtamaBusiness(string _alimNo)
        {
            alimNo = _alimNo;
        }

        public bool AtamaYap()
        {
            //for (int i = 0; i < 15; i++)
            //{
            //    Thread.Sleep(200);

            //}
            //return true;

            bool islemDurumu = false;

            // Birimlerin hangi unvandan , kac kisi atanacagini tutar.
            DataTable dtAtamaBilgi = null;
            if (dtAtamaBilgi == null)
                dtAtamaBilgi = new AtamaBS().AtamaBilgiyiGetir(alimNo);

            // Kazanan kisilerin unvan ve nufusa kayitli oldugu yer vs. tutar
            DataTable dtKazananListe = null;
            if (dtKazananListe == null)
                dtKazananListe = new AtamaBS().KazananListesiGetir(alimNo);

            // Adaylarin atanamayacagi birim listesi
            DataTable dtKazananKaraListe = null;
            if (dtKazananKaraListe == null)
                dtKazananKaraListe = new AtamaBS().KazananKaraListeyiGetir(alimNo);

            // Kazananların atama listesi
            DataTable dtKazananAtamaSonuc = null;

            if (dtKazananAtamaSonuc == null)
            {
                dtKazananAtamaSonuc = new DataTable();
                dtKazananAtamaSonuc.Columns.Add("ALIMNO", Type.GetType("System.String"));
                dtKazananAtamaSonuc.Columns.Add("TCKIMLIK", Type.GetType("System.String"));
                dtKazananAtamaSonuc.Columns.Add("BIRIM_KODU_ATANDI", Type.GetType("System.String"));
                dtKazananAtamaSonuc.Columns.Add("UNVAN_KODU", Type.GetType("System.String"));
                dtKazananAtamaSonuc.AcceptChanges();
            }
            else
            {
                dtKazananAtamaSonuc.Clear();
            }

            int birimKontenjan;
            int atamayaUygunKisiSayisi;
            string atananKisiTcNo;
            string birimKodu, unvanKodu;
            bool atamaDurumu = true;
            Random seed = new Random();
            Random r = new Random(seed.Next(1, 1000));

            foreach (DataRow rowAtamaBilgi in dtAtamaBilgi.Rows)
            {
                birimKontenjan = Convert.ToInt32(rowAtamaBilgi["ATAMA_SAYI"].ToString());
                birimKodu = rowAtamaBilgi["BIRIM_KODU"].ToString();
                unvanKodu = rowAtamaBilgi["UNVAN_KODU"].ToString();

                for (int i = 0; i < birimKontenjan; i++)
                {
                    DataRow[] rowsKazananListe = dtKazananListe.Select("BUNVAN='" + unvanKodu + "'");
                    DataRow[] rowsKazananKaraListe = dtKazananKaraListe.Select("BIRIM_KODU_KARA='" + birimKodu + "'");
                    //DataRow[] rowsAtanmisKisiListe = dtKazananAtamaSonuc.Select("BIRIM_KODU_ATANDI='" + birimKodu + "'");

                    var KazananListesiUnvanBazinda = from c in rowsKazananListe.AsEnumerable()
                                                     select c.Field<string>("TCKIMLIK");
                    var AtanamayacakAdayListesiBirimBazinda = from c in rowsKazananKaraListe.AsEnumerable()
                                                              select c.Field<string>("TCKIMLIK");
                    var AtanmisAdayListesi = from c in dtKazananAtamaSonuc.AsEnumerable()
                                             select c.Field<string>("TCKIMLIK");
                    var AtamayaUygunKisiler = KazananListesiUnvanBazinda.Except(AtanamayacakAdayListesiBirimBazinda.Union(AtanmisAdayListesi));

                    atamayaUygunKisiSayisi = AtamayaUygunKisiler.Count();

                    if (atamayaUygunKisiSayisi > 0)
                    {
                        atananKisiTcNo = AtamayaUygunKisiler.ElementAt(r.Next(0, atamayaUygunKisiSayisi));

                        // Secilen kisinin atamasini yap
                        DataRow rowAtamaSonuc = dtKazananAtamaSonuc.NewRow();
                        rowAtamaSonuc["ALIMNO"] = alimNo;
                        rowAtamaSonuc["TCKIMLIK"] = atananKisiTcNo;
                        rowAtamaSonuc["BIRIM_KODU_ATANDI"] = birimKodu;
                        rowAtamaSonuc["UNVAN_KODU"] = unvanKodu;
                        dtKazananAtamaSonuc.Rows.Add(rowAtamaSonuc);
                        dtKazananAtamaSonuc.AcceptChanges();
                    }
                    else
                    {
                        atamaDurumu = false;
                        sayac++;
                        break;
                    }
                }

                if (!atamaDurumu)
                    break;
            }

            if (!atamaDurumu)
                AtamaYap();
            else
                islemDurumu = new AtamaBS().AtamaSonuclariniKaydet(dtKazananAtamaSonuc, alimNo);

            return islemDurumu;
        }
    }
}
