using ContainerWebServiceClient.KonteynerWS_Gateway;
using System;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ContainerWebServiceClient.Entities;

namespace ContainerWebServiceClient
{
    public partial class FormSendMessage : Form
    {
        public FormSendMessage()
        {
            InitializeComponent();
        }

        private void btnDosyaSecLocal_Click(object sender, EventArgs e)
        {
            string initialDirectory = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter =
               "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            dialog.InitialDirectory = initialDirectory;
            dialog.Title = "Select a text file";
            string selectedFile = (dialog.ShowDialog() == DialogResult.OK) ? dialog.FileName : null;
            if (!string.IsNullOrEmpty(selectedFile))
            {
                string dosyaIcerik = SecilenDosyayiOku(selectedFile);

                lblDosyaYoluLocal.Text = selectedFile;
                txtRequest.Text = IndentXMLString(dosyaIcerik);
            }
            else
            {
                MessageBox.Show("File selection error!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string SecilenDosyayiOku(string dosyaYolu)
        {
            string str = "";
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(dosyaYolu);
                str = xmlDoc.InnerXml.ToString();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(str);
            }
            catch (Exception exc)
            {
                MessageBox.Show("The error occured while reading the file. Details:" + exc.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return str;
        }

        private string IndentXMLString(string xml)
        {
            string outXml = string.Empty;
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.Unicode);
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(xml);
                xtw.Formatting = Formatting.Indented;
                doc.WriteContentTo(xtw);
                xtw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return string.Empty;
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            txtRequest.Text = String.Empty;
            txtResponse.Text = String.Empty;
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            if (radioButtonTasimaBilgileriKaydet.Checked == false && radioButtonKonteynerYeriniDegistir.Checked == false)
            {
                MessageBox.Show("Select operation!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (String.IsNullOrEmpty(txtRequest.Text))
            {
                MessageBox.Show("Select the message!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly);
            binding.SendTimeout = TimeSpan.FromSeconds(125);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Transport.Realm = "";
            binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

            EndpointAddress address = new EndpointAddress("http://ws.xxx.gov.tr:8080/EXT/ABC/Konteyner/Provider");
            KonteynerClient client = new KonteynerClient(binding, address);

            client.ClientCredentials.UserName.UserName = "xxxxx";
            client.ClientCredentials.UserName.Password = "xxxxx";

            objSerialize sr = new objSerialize();

            if (radioButtonTasimaBilgileriKaydet.Checked)
            {
                object msgTasima = sr.DeSerializeAnObject(txtRequest.Text, typeof(Tasima));
                Sonuc tasimaKayitSonuc = client.TasimaBilgileriKaydet((Tasima)msgTasima);

                txtResponse.Text = IndentXMLString(sr.SerializeAnObject(tasimaKayitSonuc));
            }
            else if (radioButtonKonteynerYeriniDegistir.Checked)
            {
                ContainerPlaceChange kyd = (ContainerPlaceChange)sr.DeSerializeAnObject(txtRequest.Text, typeof(ContainerPlaceChange));
                YerDegisiklikSonuc yerSonuc = client.KonteynerYeriniDegistir(kyd.NotificationNo, kyd.ContainerNo, kyd.Placeholder);

                txtResponse.Text = IndentXMLString(sr.SerializeAnObject(yerSonuc));
            }
        }
    }
}
