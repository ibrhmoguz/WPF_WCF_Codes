using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Artefact.Animation;

namespace EmployeeDesignation
{
    /// <summary>
    /// Interaction logic for Deneme.xaml
    /// </summary>
    public partial class Deneme : Window
    {
        DataTable dt = null;
        List<DataRow[]> birimBazindaAtananKisiListeleri = null;
        DataRow[] birimeAtananKisiler = null;
        int birimIndex = 0;
        int index = 0;

        public Deneme()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Deneme_Loaded);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Deneme_MouseLeftButtonDown);
        }

        void Deneme_Loaded(object sender, RoutedEventArgs e)
        {
            lblBirimAdi.Content = String.Empty;
            DemoVeriYarat();
        }

        private void DemoVeriYarat()
        {
            dt = new DataTable();

            dt.Columns.Add("TCKIMLIK", Type.GetType("System.String"));
            dt.Columns.Add("AD", Type.GetType("System.String"));
            dt.Columns.Add("UNVAN", Type.GetType("System.String"));
            dt.Columns.Add("BIRIMADI", Type.GetType("System.String"));
            dt.Columns.Add("BIRIMKODU", Type.GetType("System.String"));
            dt.AcceptChanges();

            for (int i = 0; i < 5; i++)
            {
                DataRow row = dt.NewRow();
                row[0] = "12345678910";
                row[1] = "İbrahim OĞUZ";
                row[2] = "MUHAFAZA MEMURU";
                row[3] = "Kayseri Müdürlüğü";
                row[4] = "1";
                dt.Rows.Add(row);

                DataRow row2 = dt.NewRow();
                row2[0] = "98765432198";
                row2[1] = "Ahmet Taner";
                row2[2] = "MUAYENE MEMURU";
                row2[3] = "İzmir Müdürlüğü";
                row2[4] = "2";
                dt.Rows.Add(row2);
            }

            dt.AcceptChanges();
        }

        void Deneme_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(null);

            // ease
            ArtefactAnimator.AddEase(ball3, AnimationTypes.X, p.X, .8, AnimationTransitions.BackEaseIn, 0);
            ArtefactAnimator.AddEase(ball3, AnimationTypes.Y, p.Y, .8, AnimationTransitions.BackEaseIn, 0);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ArtefactAnimator.AddEase(ball, AnimationTypes.X, 680, .7, AnimationTransitions.BackEaseIn, 0);
            ArtefactAnimator.AddEase(ball, AnimationTypes.Y, 10, .7, AnimationTransitions.BackEaseIn, 0);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            ArtefactAnimator.AddEase(ball, AnimationTypes.X, 20, .7, AnimationTransitions.BackEaseOut, 0);
            ArtefactAnimator.AddEase(ball, AnimationTypes.Y, 10, .7, AnimationTransitions.BackEaseOut, 0);
        }

        private void btnEkle_Click(object sender, RoutedEventArgs e)
        {
            lbSonuclar.Items.Clear();
            SonuclariGoster();
        }

        private void SonuclariGoster()
        {
            BirimlereGoreVerileriAyir();
            BirimAdiAyarla();
        }

        private void BirimlereGoreVerileriAyir()
        {
            DataTable dtBirimler = dt.DefaultView.ToTable(true, "BIRIMKODU");
            birimBazindaAtananKisiListeleri = new List<DataRow[]>();

            foreach (DataRow rowBirimID in dtBirimler.Rows)
            {
                DataRow[] rowKisiler = dt.Select("BIRIMKODU='" + rowBirimID[0].ToString() + "'");
                birimBazindaAtananKisiListeleri.Add(rowKisiler);
            }
        }

        private void BirimAdiAyarla()
        {
            lblBirimAdi.Content = String.Empty;
            ArtefactAnimator.AddEase(ball, AnimationTypes.X, 680, .7, AnimationTransitions.BackEaseIn, 0);
            EaseObject baslikEase = ArtefactAnimator.AddEase(ball, AnimationTypes.Y, 10, .7, AnimationTransitions.BackEaseIn, 0);
            baslikEase.Complete += new EaseObjectHandler(baslikEase_Complete);
        }

        void baslikEase_Complete(EaseObject easeObject, double percent)
        {
            if (birimBazindaAtananKisiListeleri.Count == birimIndex)
            {
                lblBirimAdi.Content = String.Empty;
                lbSonuclar.Items.Clear();
                birimIndex = 0;
                return;
            }

            lblBirimAdi.Content = ((DataRow[])birimBazindaAtananKisiListeleri[birimIndex])[0][3].ToString();

            ArtefactAnimator.AddEase(ball, AnimationTypes.X, 20, .7, AnimationTransitions.BackEaseOut, 0);
            ArtefactAnimator.AddEase(ball, AnimationTypes.Y, 10, .7, AnimationTransitions.BackEaseOut, .9);

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
                Content = birimeAtananKisiler[index][0].ToString() + "\t" + birimeAtananKisiler[index][1].ToString() + "\t" + birimeAtananKisiler[index][2].ToString(),
                RenderTransform = new TranslateTransform(-lbSonuclar.Width, 0)
            };

            lbSonuclar.Items.Add(lbi);

            // Animasyon
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
    }
}
