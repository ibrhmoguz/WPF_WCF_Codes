using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using EmployeeDesignation.YetkiKontrol;

namespace EmployeeDesignation
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        DataSet dsYetkiler = null;

        public Login()
        {
            InitializeComponent();
        }

        private void btnTamam_Click(object sender, RoutedEventArgs e)
        {
            GirisYap();
        }

        private void GirisYap()
        {
            string tcno = txtTcno.Text;
            string sifre = txtsifre.Password;
            bool memurMu = true;

            if (String.IsNullOrEmpty(tcno) || String.IsNullOrEmpty(sifre))
            {
                lblUyari.Content = "Kullanıcı Adı veya şifre Giriniz!";
                return;
            }

            dsYetkiler = new DataSet();
            YetkileriGetir kullanici = new YetkileriGetir();

            if (memurMu)
            {
                if (txtTcno.Text.Length < 5)
                {
                    for (int i = txtTcno.Text.Length; i < 5; i++)
                    {
                        tcno = "0" + tcno;
                    }
                }
            }

            try
            {
                Yetkiler[] ytk = new Yetkiler[15];
                ytk = kullanici.KullaniciYetkileriArray(tcno, sifre, memurMu);
            }
            catch
            {
                lblUyari.Content = "Yetki web servisine ulaşılamıyor!!";
            }

            Yetki yetkiler = null;

            try
            {
                yetkiler = kullanici.KullaniciYetkileri(tcno, sifre, memurMu);
            }
            catch (Exception)
            {
                lblUyari.Content = "Bilgiler alınamadı.Lütfen tekrar deneyiniz!";
            }

            if (yetkiler == null)
            {
                lblUyari.Content = "Kullanıcı yetkileri alınamadı, Lütfen daha sonra tekrar deneyiniz!";
            }
            else
            {
                dsYetkiler = yetkiler.Ds;
                //Session["Kullanici"] = txtTcno.Text;

                string msg = yetkiler.Msg;
                if (String.IsNullOrEmpty(msg))
                {
                    bool yetki = YetkiKontrol();
                    if (!yetki)
                    {
                        lblUyari.Content = "Projeye Yetkiniz Yoktur..";
                    }
                    else
                    {
                        this.Visibility = System.Windows.Visibility.Hidden;
                        Atama atamaForm = new Atama();
                        atamaForm.Show();
                    }
                }
                else
                    lblUyari.Content = msg;
            }
        }

        private bool YetkiKontrol()
        {
            string str = String.Empty;
            string strYetki = String.Empty;

            if (dsYetkiler == null || dsYetkiler.Tables.Count == 0)
            {
                lblUyari.Content = "Yetkiler alınamadı, Tekrar giriş yapmanız sorunu çözebilir.";
                return false;
            }

            for (int i = 0; i < dsYetkiler.Tables[0].Rows.Count; i++)
            {
                strYetki = dsYetkiler.Tables[0].Rows[i]["CPFL"].ToString();
                switch (strYetki)
                {
                    case "pa":
                        str = "yetkili";
                        break;
                }
                               
            }

            if (str=="yetkili")
                return true;
            else
                return false;
        }

        private void btnIptal_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}