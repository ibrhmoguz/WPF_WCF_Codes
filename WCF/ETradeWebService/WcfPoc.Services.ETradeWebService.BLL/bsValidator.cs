using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Web.Hosting;

namespace ABC.Servisler.ETicaretServisYeni.BLL
{
    public class bsValidator
    {
         XmlDocument doc;
        XmlSchema xSchema;
        //XmlSchemaSet xSet;
        ValidationEventHandler validationEventHandler;

        String strSchemaMess;
        String strDocMess;

        public bsValidator()
        {
            validationEventHandler = new ValidationEventHandler(Schema_ValidationEventHandler);
            xSchema = new XmlSchema();
            doc = new XmlDocument();

            //xSet.ValidationEventHandler += new ValidationEventHandler(xSet_ValidationEventHandler);          
        }

        void xSet_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void Schema_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            strSchemaMess = e.Message;
        }

        public string ValidateSchema(string schemaFile)
        {
            strSchemaMess = string.Empty;
            try
            {
                xSchema = XmlSchema.Read(XmlReader.Create(schemaFile), Schema_ValidationEventHandler);
            }
            catch (Exception e)
            {
                strSchemaMess = e.Message;
            }
  
            return strSchemaMess;
        }

        public string ValidateDocument(string xmldoc)
        {
            strDocMess = string.Empty;
            try
            {
                doc.Load(xmldoc);
            }
            catch(Exception e)
            {
                strDocMess = e.Message;
            }

            return strDocMess;
        }

        public string ValidateAgainstSchema(string xdoc, string XMLType)
        {         
            string schema = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            schema += "ValidationEntities\\" + XMLType + ".xsd";
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(XmlSchema.Read(XmlReader.Create(schema), Schema_ValidationEventHandler));
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(settings_ValidationEventHandler);

            XmlReader rdr = XmlReader.Create(new StringReader(xdoc), settings);
            try
            {
                while (rdr.Read()) ;
            }
            catch (Exception e)
            {
                strSchemaMess = e.Message;
                rdr.Close();
            }
            
            if (!string.IsNullOrEmpty(strSchemaMess))
                throw new Exception(strSchemaMess);

            return strSchemaMess;
        }

        void settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            strSchemaMess = e.Message;
        }
    }
}
