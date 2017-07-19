using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace ContainerWebServiceClient
{
    public class objSerialize
    {
        public string SerializeAnObject(object obj)
        {

            System.Xml.XmlDocument doc = new XmlDocument();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            try
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                doc.Load(stream);
                return doc.InnerXml;
            }

            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }

        }

        public object DeSerializeAnObject(string xmlOfAnObject, Type ObjType)
        {
            object myObject = Activator.CreateInstance(ObjType);
            System.IO.StringReader read = new StringReader(xmlOfAnObject);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(myObject.GetType());
            System.Xml.XmlReader reader = new XmlTextReader(read);
            try
            {
                myObject = serializer.Deserialize(reader);
                return myObject;
            }
            catch (Exception exc)
            {
                throw exc;
            }

            finally
            {
                reader.Close();
                read.Close();
                read.Dispose();
            }
        }
    }
}
