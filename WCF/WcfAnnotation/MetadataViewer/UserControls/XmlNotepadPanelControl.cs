//*****************************************************************************
//    Description.....Contract Model - LocalRepositoryConcole
//                                
//    Author..........Roman Kiss, rkiss@pathcom.com
//    Copyright © 2008 ATZ Consulting Inc. (see included license.rtf file) 
//   
//    Note:
//        This code is using the XmlNotePadForm class  
//        from the XmlNotepad 2007 assembly (http://www.codeplex.com/xmlnotepad) 
//        Thanks for great XmlTree features.
//  
//              
//    Date Created:    07/07/08
//
//    Date        Modified By     Description
//-----------------------------------------------------------------------------
//    07/07/08    Roman Kiss     Initial Revision
//*****************************************************************************
//  
#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#endregion

namespace MetadataViewer.UserControls
{
    public partial class XmlNotepadPanelControl : Panel
    {
        XmlNotepadForm _form = null;

        public XmlNotepadPanelControl()
        {
            try
            {
                XmlNotepadForm form = new XmlNotepadForm();
                if (form != null)
                {
                    // form
                    form.FormClosed += new FormClosedEventHandler(form_FormClosed);
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.MaximizeBox = false;
                    form.MinimizeBox = false;
                    form.ControlBox = false;
                    form.IsMdiContainer = false;
                    form.TopLevel = false;
                    form.Location = this.Location;
                    form.Dock = DockStyle.Fill;
                    form.BringToFront();
                    form.Show();
                    this.Controls.Add(form);
                    _form = form;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            base.DestroyHandle();
        }
        public XmlNotepadForm XmlNotepadForm { get { return _form; } }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }
    }

    public class XmlNotepadForm : XmlNotepad.FormMain
    {
        public EventHandler RaiseEventOnModelChanged = null;
  
        #region Constructor
        public XmlNotepadForm()
        {
            //System.Diagnostics.Debugger.Break();
            this.Controls.Remove(this.Controls["menuStrip1"]);
            this.Controls.Remove(this.Controls["comboBoxLocation"]);
            this.Controls.Remove(this.Controls["resizer"]);
            this.Controls.Remove(this.Controls["tabControlLists"]);
            this.Controls.Remove(this.Controls["statusBar1"]);
            this.Controls.Remove(this.Controls["toolStrip1"]);
            this.Controls[0].Controls[0].Controls[1].Text = "Xml";
            TableLayoutPanel tlp = this.Controls[0].Controls[2].Controls[0].Controls[0].Controls[0] as TableLayoutPanel;
            tlp.Controls.Clear();

            this.Controls["tabControlViews"].Dock = DockStyle.Fill;
        }
        #endregion

        protected override void DestroyHandle()
        {
            base.DestroyHandle();
        }
       
        #region customize XmlNotepad for input/output xml text
        protected override object GetService(Type service)
        {
            if (service == typeof(XmlNotepad.XmlCache))
            {
                if (this.Model == null)
                {
                    return new XmlNotepad.XmlCache(this, this); ;
                }
                else
                {
                    //if (this.Model.Document.OuterXml == "")
                    //{
                    //    this.Model.Document.LoadXml("<root/>");
                    //}
                    return this.Model;
                }
            }
            return base.GetService(service);
        }
        public override bool SaveIfDirty(bool prompt)
        {
            // don't show dialog box
            return false;
        }
        public override bool Save()
        {
            // don't show dialog box
            return false;
        }
        protected override void OnModelChanged()
        {
            base.OnModelChanged();

            if (this.RaiseEventOnModelChanged != null)
                this.RaiseEventOnModelChanged(this, new EventArgs());
        }
        protected override void OnFileChanged()
        {
            base.OnFileChanged();
        }

        public void LoadXmlDocument(string xml)
        {
            if (string.IsNullOrEmpty(xml) == false)
            {
                if (this.Model != null && this.Model.Document != null)
                {
                    this.Model.Clear();
                    this.Model.Document.LoadXml(xml);
                }
                this.XmlTreeView.SetSite(this);
            }
        }
        public void ClearXmlDocument()
        {
            if(this.Model != null)
                this.Model.Clear();
            this.XmlTreeView.SetSite(this);
            this.TabControlViews.SelectedTab = this.tabPageTreeView;
        }
        public string GetCurrentXmlDocument
        {
            get 
            {
                if (this.Model != null && this.Model.Document != null)
                    return this.Model.Document.OuterXml;
                else
                    return null;
            }
        }
        #endregion

        #region Public Methods
        public void ExpandAll()
        {
            this.XmlTreeView.ExpandAll();           
        }
        public int ResizerPosition
        {
            get { return this.XmlTreeView.ResizerPosition; }
            set {  this.XmlTreeView.ResizerPosition = value; }
        }
        #endregion

    }
}


