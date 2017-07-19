using System;
using System.ServiceModel.Configuration;

namespace MessageListener.Instrumentation
{
    public class LoggingBehaviorExtensionElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(LoggingEndpointBehavior);
            }
        }

        protected override object CreateBehavior()
        {
            return new LoggingEndpointBehavior();
        }
    }
}
