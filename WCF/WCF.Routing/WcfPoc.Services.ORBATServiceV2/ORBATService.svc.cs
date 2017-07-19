using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfPoc.Services.ORBATServiceV2
{
    public class ORBATService : IORBATServiceV2
    {
        public List<Query> GetSavedQueries(string missionCode)
        {
            List<Query> queryList = new List<Query>();

            for (int i = 0; i < 6; i++)
            {
                Query q = new Query()
                {
                    Name = "Air Test " + i.ToString() + " Query",
                    Description = "Airbases and Units in within the area ....",
                    TestType = "Airbase, Unit"
                };
                queryList.Add(q);
            }

            return queryList;
        }

        public bool WriteTestEntities(string missionCode, TestEntity entity)
        {
            return true;
        }
    }
}
