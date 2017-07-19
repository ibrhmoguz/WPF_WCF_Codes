using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfPoc.Services.ORBATServiceV1
{
    [ServiceContract(Namespace="http://WcfPoc.wcfRouting.int/Increment1")]
    public interface IORBATServiceV1
    {
        [OperationContract]
        List<Query> GetSavedQueries(string missionCode);
    }

    [DataContract]
    public class Query
    {
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public string Description { get; set; }

        [DataMember(Order = 3)]
        public string TestType { get; set; }
    }
}

