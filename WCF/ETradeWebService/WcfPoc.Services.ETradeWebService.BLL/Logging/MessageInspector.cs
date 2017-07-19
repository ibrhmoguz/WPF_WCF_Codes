using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Xml;
using ABC.Servisler.ETicaretServisYeni.BLL;
using System.Web;

namespace MessageListener.Instrumentation
{
    public class MessageInspector : IDispatchMessageInspector
    {
        const string LogDir = @"C:\Logs\CoffeeMakingService\";

        private Message TraceMessage(MessageBuffer buffer)
        {
            //Must use a buffer rather than the origonal message, because the Message's body can be processed only once.
            Message msg = buffer.CreateMessage();

            //Setup StringWriter to use as input for our StreamWriter
            //This is needed in order to capture the body of the message, because the body is streamed.
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            //msg.WriteMessage(xmlTextWriter);
            xmlTextWriter.Flush();
            xmlTextWriter.Close();


            //Setup filename to write to
            if (!Directory.Exists(LogDir))
                Directory.CreateDirectory(LogDir);
            
            DateTime now = DateTime.Now;
            string datePart = now.Year.ToString() + '-' + now.Month.ToString() + '-' + now.Day.ToString() + '-' + now.Hour + '-' + now.Minute + '-' + now.Second;
            string fileName = LogDir + "\\" + datePart + '-' + "SoapEnv.xml";
            
            //Write to file
            using (StreamWriter sw = new StreamWriter(fileName))
                sw.Write(stringWriter.ToString());

            //Return copy of origonal message with unalterd State
            return buffer.CreateMessage();
        }
        
        //BeforeSendReply is called after the response has been constructed by the service operation
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            string storedValue = HttpContext.Current.Items["InsertID"].ToString();
            HttpContext.Current.Items.Remove("InsertID");
            bslogger.UpdateSoapMessage(reply.ToString(), storedValue);
        }
        bsLogger bslogger = new bsLogger();
        //The AfterReceiveRequest method is fired after the message has been received but prior to invoking the service operation
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(request.ToString());

            string pGuid = string.Empty;

            if (null != doc.FirstChild.ChildNodes[1].ChildNodes[0].ChildNodes[3].FirstChild)
                pGuid = doc.FirstChild.ChildNodes[1].ChildNodes[0].ChildNodes[3].FirstChild.Value;
            else
                pGuid = doc.FirstChild.ChildNodes[1].ChildNodes[0].ChildNodes[4].FirstChild.Value;

            HttpContext.Current.Items["InsertID"] = pGuid;
            bslogger.InsertSoapMessage(request.ToString(), pGuid);
            return null;
        }

       
    }
}  
