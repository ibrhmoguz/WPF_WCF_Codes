using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WcfPoc.Client.Common
{
    public class CustomMessageHeader : MessageHeader
    {
        private string Version { get; set; }

        public CustomMessageHeader(string version)
        {
            this.Version = version;
        }

        public override string Name => "Version";

        public override string Namespace => "http://WcfPoc.wcfRouting.int/Increment1";

        protected override void OnWriteHeaderContents(System.Xml.XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteValue(this.Version);
        }
    }
}
