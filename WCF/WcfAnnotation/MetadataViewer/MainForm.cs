using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows.Forms;

namespace MetadataViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.xmlNotepadPanelControl1.XmlNotepadForm.ResizerPosition = this.ClientSize.Width / 2;
            this.comboBoxContracts.Visible = false;
            this.labelContract.Visible = false;
                
        }

        #region Clicks
        private void buttonGet_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.xmlNotepadPanelControl1.XmlNotepadForm.ClearXmlDocument();
                MetadataSet metadata = null;
                if (this.checkBoxAssembly.Checked)
                {
                    this.FromAssembly(this.textBoxUrl.Text);
                }
                else
                {
                    metadata = MetadataHelper.GetMetadataSet(this.textBoxUrl.Text);
                }
                if (metadata != null)
                {
                    StringBuilder sb = new StringBuilder();
                    metadata.WriteTo(XmlWriter.Create(sb));
                    this.xmlNotepadPanelControl1.XmlNotepadForm.LoadXmlDocument(sb.ToString());
                }
                this.buttonGet.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(MetadataHelper.ErrorMessage(ex), "Loading metadata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.xmlNotepadPanelControl1.XmlNotepadForm.ClearXmlDocument();
            this.comboBoxContracts.Items.Clear();
            this.buttonGet.Enabled = true;
        }
        #endregion

        #region Events
        private void comboBoxImportUrl_TextChanged(object sender, EventArgs e)
        {
            this.textBoxUrl.Text = comboBoxImportUrl.Text;
            this.comboBoxImportUrl.Visible = false;
        }

        private void textBoxUrl_TextChanged(object sender, EventArgs e)
        {          
            if (string.IsNullOrEmpty(this.textBoxUrl.Text))
            {
                this.buttonGet.Enabled = false;
            }
            else
            {
                this.buttonGet.Enabled = true;
            }
        }

        private void labelBrowser_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxUrl.Text = this.openFileDialog1.FileName;
                this.comboBoxContracts.Items.Clear();              
                this.buttonGet.Enabled = true;
            }
        }

        private void checkBoxAssembly_Click(object sender, EventArgs e)
        {
            this.comboBoxContracts.Items.Clear();
            if (this.checkBoxAssembly.Checked)
            {
                this.labelUrl.Text = "asm:";
                this.labelBrowser.Visible = true;
                this.comboBoxContracts.Visible = true;
                this.labelContract.Visible = true;
            }
            else
            {
                this.labelUrl.Text = "url:";
                this.labelBrowser.Visible = false;
                this.comboBoxContracts.Visible = false;
                this.labelContract.Visible = false;
            }
            this.textBoxUrl.Text = string.Empty;
            this.textBoxUrl.Tag = null;
        }

        private void labelUrl_DoubleClick(object sender, EventArgs e)
        {
            // for test purpose only
            this.comboBoxImportUrl.Visible = true;
            this.comboBoxImportUrl.DroppedDown = true;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            this.comboBoxImportUrl.Visible = false;
        }
        #endregion

        private void FromAssembly(string filepath)
        {
            try
            {
                if (File.Exists(filepath) == false)
                    throw new FileLoadException("File doesn't exist", filepath);

                Assembly asm = Assembly.LoadFrom(filepath);
                var query = from t in asm.GetTypes() where t.IsInterface && t.GetCustomAttributes(typeof(ServiceContractAttribute), true) != null select t;

                this.textBoxUrl.Tag = asm.FullName;
                this.comboBoxContracts.Items.Clear();

                foreach (var item in query)
                {
                    this.comboBoxContracts.Items.Add(item.FullName);
                }

                if (this.comboBoxContracts.Items.Count > 0)
                {
                    this.comboBoxContracts.Tag = asm;
                    this.comboBoxContracts.Text = (string)this.comboBoxContracts.Items[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MetadataHelper.ErrorMessage(ex), "Loading resource", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxContracts_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender is ComboBox && (sender as ComboBox).Tag != null && !string.IsNullOrEmpty(this.comboBoxContracts.Text))
                {
                    Assembly asm = (sender as ComboBox).Tag as Assembly;

                    WsdlExporter exporter = new WsdlExporter();

                    // generate contract from the clr type
                    ContractDescription contract = ContractDescription.GetContract(asm.GetType(this.comboBoxContracts.Text));

                    // dummy endpoint
                    exporter.ExportEndpoint(new ServiceEndpoint(contract, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost")));

                    // generate metadata
                    MetadataSet set = exporter.GetGeneratedMetadata();
                    if (set != null)
                    {
                        // show in the notepad
                        StringBuilder sb = new StringBuilder();
                        set.WriteTo(XmlWriter.Create(sb));
                        this.xmlNotepadPanelControl1.XmlNotepadForm.LoadXmlDocument(sb.ToString());
                    }
                }
                else
                {
                    this.xmlNotepadPanelControl1.XmlNotepadForm.ClearXmlDocument();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(MetadataHelper.ErrorMessage(ex), "Loading resource", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }     
    }
}
