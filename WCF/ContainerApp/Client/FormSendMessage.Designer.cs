namespace ContainerWebServiceClient
{
    partial class FormSendMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSendMessage));
            this.lblDosyaYoluLocal = new System.Windows.Forms.Label();
            this.btnDosyaSecLocal = new System.Windows.Forms.Button();
            this.txtRequest = new System.Windows.Forms.TextBox();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGonder = new System.Windows.Forms.Button();
            this.radioButtonTasimaBilgileriKaydet = new System.Windows.Forms.RadioButton();
            this.radioButtonKonteynerYeriniDegistir = new System.Windows.Forms.RadioButton();
            this.btnTemizle = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDosyaYoluLocal
            // 
            this.lblDosyaYoluLocal.AutoSize = true;
            this.lblDosyaYoluLocal.Location = new System.Drawing.Point(107, 20);
            this.lblDosyaYoluLocal.Name = "lblDosyaYoluLocal";
            this.lblDosyaYoluLocal.Size = new System.Drawing.Size(95, 13);
            this.lblDosyaYoluLocal.TabIndex = 54;
            this.lblDosyaYoluLocal.Text = "Selected file path";
            // 
            // btnDosyaSecLocal
            // 
            this.btnDosyaSecLocal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDosyaSecLocal.Location = new System.Drawing.Point(12, 12);
            this.btnDosyaSecLocal.Name = "btnDosyaSecLocal";
            this.btnDosyaSecLocal.Size = new System.Drawing.Size(82, 28);
            this.btnDosyaSecLocal.TabIndex = 53;
            this.btnDosyaSecLocal.Text = "Select file";
            this.btnDosyaSecLocal.UseVisualStyleBackColor = true;
            this.btnDosyaSecLocal.Click += new System.EventHandler(this.btnDosyaSecLocal_Click);
            // 
            // txtRequest
            // 
            this.txtRequest.Location = new System.Drawing.Point(12, 163);
            this.txtRequest.Multiline = true;
            this.txtRequest.Name = "txtRequest";
            this.txtRequest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRequest.Size = new System.Drawing.Size(350, 397);
            this.txtRequest.TabIndex = 56;
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(373, 163);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.Size = new System.Drawing.Size(350, 397);
            this.txtResponse.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(9, 145);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 15);
            this.label1.TabIndex = 57;
            this.label1.Text = "Request:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(370, 145);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 58;
            this.label2.Text = "Response:";
            // 
            // btnGonder
            // 
            this.btnGonder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnGonder.Location = new System.Drawing.Point(537, 49);
            this.btnGonder.Name = "btnGonder";
            this.btnGonder.Size = new System.Drawing.Size(81, 27);
            this.btnGonder.TabIndex = 60;
            this.btnGonder.Text = "Send";
            this.btnGonder.UseVisualStyleBackColor = true;
            this.btnGonder.Click += new System.EventHandler(this.btnGonder_Click);
            // 
            // radioButtonTasimaBilgileriKaydet
            // 
            this.radioButtonTasimaBilgileriKaydet.AutoSize = true;
            this.radioButtonTasimaBilgileriKaydet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radioButtonTasimaBilgileriKaydet.Location = new System.Drawing.Point(11, 26);
            this.radioButtonTasimaBilgileriKaydet.Name = "radioButtonTasimaBilgileriKaydet";
            this.radioButtonTasimaBilgileriKaydet.Size = new System.Drawing.Size(124, 17);
            this.radioButtonTasimaBilgileriKaydet.TabIndex = 61;
            this.radioButtonTasimaBilgileriKaydet.TabStop = true;
            this.radioButtonTasimaBilgileriKaydet.Text = "Save";
            this.radioButtonTasimaBilgileriKaydet.UseVisualStyleBackColor = true;
            // 
            // radioButtonKonteynerYeriniDegistir
            // 
            this.radioButtonKonteynerYeriniDegistir.AutoSize = true;
            this.radioButtonKonteynerYeriniDegistir.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radioButtonKonteynerYeriniDegistir.Location = new System.Drawing.Point(141, 26);
            this.radioButtonKonteynerYeriniDegistir.Name = "radioButtonKonteynerYeriniDegistir";
            this.radioButtonKonteynerYeriniDegistir.Size = new System.Drawing.Size(140, 17);
            this.radioButtonKonteynerYeriniDegistir.TabIndex = 62;
            this.radioButtonKonteynerYeriniDegistir.TabStop = true;
            this.radioButtonKonteynerYeriniDegistir.Text = "Change";
            this.radioButtonKonteynerYeriniDegistir.UseVisualStyleBackColor = true;
            // 
            // btnTemizle
            // 
            this.btnTemizle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnTemizle.Location = new System.Drawing.Point(624, 49);
            this.btnTemizle.Name = "btnTemizle";
            this.btnTemizle.Size = new System.Drawing.Size(81, 27);
            this.btnTemizle.TabIndex = 63;
            this.btnTemizle.Text = "Reset";
            this.btnTemizle.UseVisualStyleBackColor = true;
            this.btnTemizle.Click += new System.EventHandler(this.btnTemizle_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonTasimaBilgileriKaydet);
            this.groupBox1.Controls.Add(this.btnTemizle);
            this.groupBox1.Controls.Add(this.btnGonder);
            this.groupBox1.Controls.Add(this.radioButtonKonteynerYeriniDegistir);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.groupBox1.Location = new System.Drawing.Point(12, 57);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(711, 81);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operation";
            // 
            // FormMesajGonder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 569);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtRequest);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.lblDosyaYoluLocal);
            this.Controls.Add(this.btnDosyaSecLocal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSendMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Container Web Service Client Application";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDosyaYoluLocal;
        private System.Windows.Forms.Button btnDosyaSecLocal;
        private System.Windows.Forms.TextBox txtRequest;
        private System.Windows.Forms.TextBox txtResponse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnGonder;
        private System.Windows.Forms.RadioButton radioButtonTasimaBilgileriKaydet;
        private System.Windows.Forms.RadioButton radioButtonKonteynerYeriniDegistir;
        private System.Windows.Forms.Button btnTemizle;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

