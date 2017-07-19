using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Artefact.Animation;
using ABCWeb.Business.PersonelAlim;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;


namespace EmployeeDesignation
{
    /// <summary>
    /// Interaction logic for Atama.xaml
    /// </summary>
    public partial class Atama : Window
    {
        private static ScrollViewer scrollViewer = null;
        BackgroundWorker bgw = null;
        DataTable dt = null;
        List<DataRow[]> birimBazindaAtananKisiListeleri = null;
        List<DataTable> unvanBazindaAtananKisiListeleri = null;
        DataRow[] birimeAtananKisiler = null;
        //int toplamUnvanSayisi = 0;
        int unvanIndex = 0;
        int birimIndex = 0;
        int index = 0;
        bool atamaIslemDurumu = false;
        string alimNo = String.Empty;

        // LISTBOX SCROLL OFFSET
        public static readonly DependencyProperty ListBoxScrollOffsetProperty =
        DependencyProperty.Register("ListBoxScrollOffset", typeof(double), typeof(Atama), new PropertyMetadata(0.0, OnListBoxScrollOffsetChanged));

        public double ListBoxScrollOffset
        {
            get { return (double)GetValue(ListBoxScrollOffsetProperty); }
            set { SetValue(ListBoxScrollOffsetProperty, value); }
        }

        public Atama()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Atama_Loaded);
        }

        void Atama_Loaded(object sender, RoutedEventArgs e)
        {
            bgw = new BackgroundWorker();
            bgw.DoWork += new DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);

            // SCROLL INTERACTION
            scrollViewer = FindVisualChild<ScrollViewer>(lbSonuclar);
            var bar = FindVisualChild<ScrollBar>(scrollViewer);
            if (bar != null) bar.ValueChanged += (s, args) => SetValue(ListBoxScrollOffsetProperty, args.NewValue);

            unvanBirimCanvas.Width = this.ActualWidth - 15;
            unvanRectangle.Width = this.ActualWidth - 15;
            birimRectangle.Width = this.ActualWidth - 15;
            lblBirimAdi.Content = String.Empty;
            lblUnvanAdi.Content = String.Empty;
            AtamaYapilacakAlimlariGetir();
            //DemoVeriYarat();
        }

        private void AtamaYapilacakAlimlariGetir()
        {
            DataTable dt = new BasvuruAlimBS().AtamaYapilacakAlimListele();
            comboBoxAlimlar.DataContext = dt;
        }

        private void btnAtamaYap_Click(object sender, RoutedEventArgs e)
        {
            AtamaYap();
        }

        private void AtamaYap()
        {
            DataTable dtAtamaYapilacakAlimlar = new BasvuruAlimBS().AtamaYapilacakAlimListele();

            if (dtAtamaYapilacakAlimlar.Rows.Count == 0)
            {
                MessageBox.Show("Personel ataması yapılmıştır.", "Uyarı", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            borderProgressBar.Visibility = System.Windows.Visibility.Visible;
            btnAtamaYap.Visibility = System.Windows.Visibility.Hidden;
            alimNo = comboBoxAlimlar.SelectedValue.ToString();
            bgw.RunWorkerAsync();
        }

        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            AtamaBusiness ab = new AtamaBusiness(alimNo);
            atamaIslemDurumu = ab.AtamaYap();
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (atamaIslemDurumu)
            {
                lbSonuclar.Items.Clear();
                lblBirimAdi.Visibility = System.Windows.Visibility.Visible;
                lblUnvanAdi.Visibility = System.Windows.Visibility.Visible;
                birimRectangle.Visibility = System.Windows.Visibility.Visible;
                unvanRectangle.Visibility = System.Windows.Visibility.Visible;
                lbSonuclar.Visibility = System.Windows.Visibility.Visible;
                borderProgressBar.Visibility = System.Windows.Visibility.Hidden;

                SonuclariGoster();

                //MessageBox.Show("Atama işlemi başarı ile tamamlandı.", "Başarı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Sistemsel hata oluştu!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void SonuclariGoster()
        {
            dt = new AtamaBS().AtamaSonucGetir(comboBoxAlimlar.SelectedValue.ToString());
            UnvanaGoreVerileriAyir();
            UnvanAdiAyarla();
        }

        private void UnvanaGoreVerileriAyir()
        {
            DataTable dtUnvanlar = dt.DefaultView.ToTable(true, "UNVAN");
            unvanBazindaAtananKisiListeleri = new List<DataTable>();

            foreach (DataRow rowUnvan in dtUnvanlar.Rows)
            {
                DataRow[] rowKisiler = dt.Select("UNVAN='" + rowUnvan[0].ToString() + "'");
                unvanBazindaAtananKisiListeleri.Add(rowKisiler.CopyToDataTable());
            }
        }

        private void UnvanAdiAyarla()
        {
            lblUnvanAdi.Content = String.Empty;
            ArtefactAnimator.AddEase(unvanRectangle, AnimationTypes.X, this.ActualWidth, 1.1, AnimationTransitions.BackEaseIn, 0);
            EaseObject unvanBaslikEase = ArtefactAnimator.AddEase(unvanRectangle, AnimationTypes.Y, 0, 1.1, AnimationTransitions.BackEaseIn, 0);
            unvanBaslikEase.Complete += new EaseObjectHandler(unvanBaslikEase_Complete);
        }

        void unvanBaslikEase_Complete(EaseObject easeObject, double percent)
        {
            if (unvanBazindaAtananKisiListeleri.Count == unvanIndex)
            {
                lblUnvanAdi.Content = String.Empty;
                unvanIndex = 0;
                MessageBox.Show("Atama işlemi başarı ile tamamlandı.", "Başarı", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            lblUnvanAdi.Content = unvanBazindaAtananKisiListeleri[unvanIndex].Rows[0]["UNVAN"].ToString();

            ArtefactAnimator.AddEase(unvanRectangle, AnimationTypes.X, 1, .7, AnimationTransitions.BackEaseOut, 0);
            ArtefactAnimator.AddEase(unvanRectangle, AnimationTypes.Y, 0, .7, AnimationTransitions.BackEaseOut, .9);

            BirimlereGoreVerileriAyir();
            BirimAdiAyarla();
        }

        private void BirimlereGoreVerileriAyir()
        {
            DataTable dtUnvanKisiler = unvanBazindaAtananKisiListeleri[unvanIndex];
            birimBazindaAtananKisiListeleri = new List<DataRow[]>();

            DataTable dtBirimler = dt.Select("UNVAN='" + dtUnvanKisiler.Rows[0]["UNVAN"].ToString() + "'").CopyToDataTable().DefaultView.ToTable(true, "BIRIM_KODU_ATANDI");

            foreach (DataRow rowBirimID in dtBirimler.Rows)
            {
                DataRow[] rowKisiler = dt.Select("UNVAN='" + dtUnvanKisiler.Rows[0]["UNVAN"].ToString() + "' AND BIRIM_KODU_ATANDI='" + rowBirimID[0].ToString() + "'");
                birimBazindaAtananKisiListeleri.Add(rowKisiler);
            }
        }

        private void BirimAdiAyarla()
        {
            lblBirimAdi.Content = String.Empty;
            ArtefactAnimator.AddEase(birimRectangle, AnimationTypes.X, this.ActualWidth, 1.1, AnimationTransitions.BackEaseIn, 0);
            EaseObject baslikEase = ArtefactAnimator.AddEase(birimRectangle, AnimationTypes.Y, 51, 1.1, AnimationTransitions.BackEaseIn, 0);
            baslikEase.Complete += new EaseObjectHandler(birimBaslikEase_Complete);
        }

        void birimBaslikEase_Complete(EaseObject easeObject, double percent)
        {
            if (birimBazindaAtananKisiListeleri.Count == birimIndex)
            {
                lblBirimAdi.Content = String.Empty;
                lbSonuclar.Items.Clear();
                birimIndex = 0;
                unvanIndex++;
                UnvanAdiAyarla();
                return;
            }

            lblBirimAdi.Content = ((DataRow[])birimBazindaAtananKisiListeleri[birimIndex])[0]["BIRIM_ADI"].ToString();

            ArtefactAnimator.AddEase(birimRectangle, AnimationTypes.X, 1, .7, AnimationTransitions.BackEaseOut, 0);
            ArtefactAnimator.AddEase(birimRectangle, AnimationTypes.Y, 51, .7, AnimationTransitions.BackEaseOut, .9);

            lbSonuclar.Items.Clear();
            ListeyeElemanEkle();
        }

        private void ListeyeElemanEkle()
        {
            if (birimBazindaAtananKisiListeleri.Count == birimIndex)
                return;

            birimeAtananKisiler = birimBazindaAtananKisiListeleri[birimIndex];

            var lbi = new ListBoxItem
            {
                Content = birimeAtananKisiler[index]["AD"].ToString() + " " + birimeAtananKisiler[index]["SOYAD"].ToString(),
                RenderTransform = new TranslateTransform(-lbSonuclar.ActualWidth, 0),
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
            };

            lbSonuclar.Items.Add(lbi);

            // Animasyon
            ArtefactAnimator.AddEase(this, ListBoxScrollOffsetProperty, scrollViewer.ScrollableHeight + 3, 1, AnimationTransitions.BackEaseOut, 0);
            EaseObject obj = ArtefactAnimator.AddEase(lbi.RenderTransform, TranslateTransform.XProperty, 0, 1, AnimationTransitions.BackEaseOut, .2);
            obj.Complete += new EaseObjectHandler(obj_Complete);

        }

        void obj_Complete(EaseObject easeObject, double percent)
        {
            if (birimeAtananKisiler.Length - 1 == index)
            {
                index = 0;
                birimIndex++;
                BirimAdiAyarla();

            }
            else
            {
                index++;
                ListeyeElemanEkle();
            }
        }

        private static void OnListBoxScrollOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            scrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        public static childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    var childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
