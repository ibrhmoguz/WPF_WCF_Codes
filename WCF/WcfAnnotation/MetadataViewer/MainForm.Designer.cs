namespace MetadataViewer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxContracts = new System.Windows.Forms.ComboBox();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.labelBrowser = new System.Windows.Forms.Label();
            this.checkBoxAssembly = new System.Windows.Forms.CheckBox();
            this.comboBoxImportUrl = new System.Windows.Forms.ComboBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonGet = new System.Windows.Forms.Button();
            this.labelContract = new System.Windows.Forms.Label();
            this.labelUrl = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.xmlNotepadPanelControl1 = new MetadataViewer.UserControls.XmlNotepadPanelControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Lavender;
            this.panel1.Controls.Add(this.comboBoxContracts);
            this.panel1.Controls.Add(this.textBoxUrl);
            this.panel1.Controls.Add(this.labelBrowser);
            this.panel1.Controls.Add(this.checkBoxAssembly);
            this.panel1.Controls.Add(this.comboBoxImportUrl);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonGet);
            this.panel1.Controls.Add(this.labelContract);
            this.panel1.Controls.Add(this.labelUrl);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(971, 116);
            this.panel1.TabIndex = 2;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // comboBoxContracts
            // 
            this.comboBoxContracts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxContracts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxContracts.FormattingEnabled = true;
            this.comboBoxContracts.Location = new System.Drawing.Point(60, 76);
            this.comboBoxContracts.Name = "comboBoxContracts";
            this.comboBoxContracts.Size = new System.Drawing.Size(504, 21);
            this.comboBoxContracts.TabIndex = 17;
            this.comboBoxContracts.TextChanged += new System.EventHandler(this.comboBoxContracts_TextChanged);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrl.Location = new System.Drawing.Point(60, 54);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(696, 20);
            this.textBoxUrl.TabIndex = 16;
            this.textBoxUrl.Text = "net.pipe://localhost/wsdldoc/mex";
            this.textBoxUrl.Click += new System.EventHandler(this.textBoxUrl_TextChanged);
            // 
            // labelBrowser
            // 
            this.labelBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBrowser.AutoSize = true;
            this.labelBrowser.BackColor = System.Drawing.Color.AliceBlue;
            this.labelBrowser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelBrowser.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBrowser.Location = new System.Drawing.Point(946, 54);
            this.labelBrowser.Name = "labelBrowser";
            this.labelBrowser.Size = new System.Drawing.Size(18, 15);
            this.labelBrowser.TabIndex = 14;
            this.labelBrowser.Text = "...";
            this.labelBrowser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelBrowser.Visible = false;
            this.labelBrowser.Click += new System.EventHandler(this.labelBrowser_Click);
            // 
            // checkBoxAssembly
            // 
            this.checkBoxAssembly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAssembly.AutoSize = true;
            this.checkBoxAssembly.Location = new System.Drawing.Point(847, 54);
            this.checkBoxAssembly.Name = "checkBoxAssembly";
            this.checkBoxAssembly.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxAssembly.Size = new System.Drawing.Size(93, 17);
            this.checkBoxAssembly.TabIndex = 13;
            this.checkBoxAssembly.Text = "FromAssembly";
            this.checkBoxAssembly.UseVisualStyleBackColor = true;
            this.checkBoxAssembly.Click += new System.EventHandler(this.checkBoxAssembly_Click);
            // 
            // comboBoxImportUrl
            // 
            this.comboBoxImportUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxImportUrl.FormattingEnabled = true;
            this.comboBoxImportUrl.Items.AddRange(new object[] {
            "http://service.ecocoma.com/video/video.asmx?WSDL",
            "http://webservices.amazon.com/AWSECommerceService/AWSECommerceService.wsdl",
            "http://s3.amazonaws.com/doc/2006-03-01/AmazonS3.wsdl?",
            "http://s3.amazonaws.com/ec2-downloads/ec2.wsdl?",
            "",
            "http://wsearch.amazonaws.com/doc/2007-03-15/WebSearch.wsdl",
            "http://service.ecocoma.com/marketing/google.asmx?WSDL",
            "http://ws.keyfortravel.com/webservices/K4TAirSell.asmx?WSDL",
            "http://api.google.com/GoogleSearch.wsdl",
            "",
            "http://schemas.xmlsoap.org/ws/2004/08/eventing/eventing.wsdl",
            "http://schemas.xmlsoap.org/ws/2004/09/mex/MetadataExchange.wsdl",
            "http://schemas.xmlsoap.org/ws/2006/08/resourceTransfer/wsrt.wsdl",
            "http://schemas.xmlsoap.org/ws/2005/04/discovery/ws-discovery.wsdl",
            "",
            "",
            "net.pipe://localhost/dada2/mex",
            "net.pipe://localhost/repository/mex?",
            "net.pipe://localhost/wsdldoc/mex"});
            this.comboBoxImportUrl.Location = new System.Drawing.Point(289, 20);
            this.comboBoxImportUrl.Name = "comboBoxImportUrl";
            this.comboBoxImportUrl.Size = new System.Drawing.Size(675, 21);
            this.comboBoxImportUrl.TabIndex = 12;
            this.comboBoxImportUrl.Text = "net.pipe://localhost/fafa6/mex";
            this.comboBoxImportUrl.Visible = false;
            this.comboBoxImportUrl.Click += new System.EventHandler(this.comboBoxImportUrl_TextChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(909, 84);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(55, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonGet
            // 
            this.buttonGet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGet.Location = new System.Drawing.Point(762, 54);
            this.buttonGet.Name = "buttonGet";
            this.buttonGet.Size = new System.Drawing.Size(41, 20);
            this.buttonGet.TabIndex = 7;
            this.buttonGet.Text = "Get";
            this.buttonGet.UseVisualStyleBackColor = true;
            this.buttonGet.Click += new System.EventHandler(this.buttonGet_Click);
            // 
            // labelContract
            // 
            this.labelContract.AutoSize = true;
            this.labelContract.Location = new System.Drawing.Point(12, 76);
            this.labelContract.Name = "labelContract";
            this.labelContract.Size = new System.Drawing.Size(50, 13);
            this.labelContract.TabIndex = 5;
            this.labelContract.Text = "Contract:";
            this.labelContract.Click += new System.EventHandler(this.labelUrl_DoubleClick);
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(30, 59);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(23, 13);
            this.labelUrl.TabIndex = 5;
            this.labelUrl.Text = "Url:";
            this.labelUrl.Click += new System.EventHandler(this.labelUrl_DoubleClick);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.Brown;
            this.labelTitle.Location = new System.Drawing.Point(13, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(190, 31);
            this.labelTitle.TabIndex = 3;
            this.labelTitle.Text = "Get Metadata";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Lavender;
            this.panel2.Controls.Add(this.xmlNotepadPanelControl1);
            this.panel2.Location = new System.Drawing.Point(0, 113);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(971, 517);
            this.panel2.TabIndex = 3;
            // 
            // xmlNotepadPanelControl1
            // 
            this.xmlNotepadPanelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xmlNotepadPanelControl1.Location = new System.Drawing.Point(0, 0);
            this.xmlNotepadPanelControl1.Name = "xmlNotepadPanelControl1";
            this.xmlNotepadPanelControl1.Size = new System.Drawing.Size(971, 517);
            this.xmlNotepadPanelControl1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 627);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(600, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Metadata Viewer  (rkiss@pathcom.com)";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Label labelBrowser;
        private System.Windows.Forms.CheckBox checkBoxAssembly;
        private System.Windows.Forms.ComboBox comboBoxImportUrl;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonGet;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private MetadataViewer.UserControls.XmlNotepadPanelControl xmlNotepadPanelControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox comboBoxContracts;
        private System.Windows.Forms.Label labelContract;

    }
}

