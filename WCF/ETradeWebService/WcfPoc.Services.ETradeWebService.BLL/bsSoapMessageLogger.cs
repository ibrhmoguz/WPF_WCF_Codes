using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Services.Protocols;
using System.Web;

namespace ABC.Servisler.ETicaretServisYeni.BLL
{
    public class SoapMessageLogger : SoapExtension
    {
        Stream oldStream;
        Stream newStream;

        bsLogger bslogger = new bsLogger();

        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return "";
        }

        public override object GetInitializer(Type WebServiceType)
        {
            return "";
        }

        public override void Initialize(object initializer)
        {
        }

        public override void ProcessMessage(SoapMessage mesaj)
        {
            switch (mesaj.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;

                case SoapMessageStage.AfterSerialize:
                    string storedValue = HttpContext.Current.Items["InsertID"].ToString();
                    HttpContext.Current.Items.Remove("InsertID");
                    WriteOutput(mesaj, storedValue);
                    break;

                case SoapMessageStage.BeforeDeserialize:
                    string newGuid = Guid.NewGuid().ToString();
                    HttpContext.Current.Items["InsertID"] = newGuid;
                    WriteInput(mesaj, newGuid);
                    break;
          
                case SoapMessageStage.AfterDeserialize:
                    break;

                default:
                    throw new Exception("invalid stage");
            }
        }

        void Copy(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }

        public void WriteInput(SoapMessage message, string insertId)
        {
            Copy(oldStream, newStream);
            newStream.Position = 0;
            StreamReader reader = new StreamReader(newStream);
            string contents = reader.ReadToEnd();
            bslogger.InsertSoapMessage(contents, insertId);
            newStream.Position = 0;
        }

        public void WriteOutput(SoapMessage message, string insertId)
        {
            newStream.Position = 0;
            StreamReader reader = new StreamReader(newStream);
            string contents = reader.ReadToEnd();
            bslogger.UpdateSoapMessage(contents, insertId);
            newStream.Position = 0;
            Copy(newStream, oldStream);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SoapMessageLoggerAttribute : SoapExtensionAttribute
    {
        private int oncelik;

        public override Type ExtensionType
        {
            get { return typeof(SoapMessageLogger); }
        }

        public override int Priority
        {
            get { return oncelik; }
            set { oncelik = value; }
        }
    }
}
