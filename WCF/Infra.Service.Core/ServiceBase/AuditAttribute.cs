using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using System.Reflection;
using System.Web;

namespace Infra.Service.Core.ServiceBase
{
    [Serializable]
    public class AuditAttribute : OnMethodBoundaryAspect
    {
        private string _methodName;
        private string _url;

        public bool ExcludeMethodName { get; set; }
        public bool IncludeUrl { get; set; }
        public string EntryMessage { get; set; }
        public string ExitMessage { get; set; }
        public string Message { get; set; }


        public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        {
            this._methodName = method.DeclaringType.FullName + "." + method.Name;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            this._methodName = this.ExcludeMethodName ? string.Empty : args.Method.Name;
            this._url = this.IncludeUrl ? string.Empty : (HttpContext.Current == null) ? string.Empty : HttpContext.Current.Request.Url.ToString();

            CommonLogService.ExtendedLoggingServiceClient myService = new CommonLogService.ExtendedLoggingServiceClient("CustomBinding_IExtendedLoggingService");
            
            string msg = "Method Name: " + this._methodName + " Url: " + this._url + " Entry Message: " + this.EntryMessage + " Message : " + this.Message;
            myService.LogWriteC(msg, "AIC", CrossCutting.Logging.BaseLogging.LogType.Information);

            // Store the StringBuilder in a 'magic' location so that it can be used later in 
            // the same target method.
            //args.MethodExecutionTag = stringBuilder;
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            //// Take the StringBuilder from where we stored it in OnEntry,
            //// and initialize it.
            //StringBuilder stringBuilder = (StringBuilder)args.MethodExecutionTag;
            //stringBuilder.Length = 0;

            //// Create the trace string.
            //stringBuilder.Append("- Exiting ");
            //AppendContext(stringBuilder, args.Instance, args.Arguments);
            //stringBuilder.Append(" with value ");
            //AppendObject(stringBuilder, args.ReturnValue);

            //// Add the string to the trace.
            //System.Diagnostics.Trace.Unindent();
            //System.Diagnostics.Trace.WriteLine(stringBuilder.ToString());
        }

        public override void OnException(MethodExecutionArgs args)
        {
            //// Take the StringBuilder from where we stored it in OnEntry,
            //// and initialize it.
            //StringBuilder stringBuilder = (StringBuilder)args.MethodExecutionTag;
            //stringBuilder.Length = 0;

            //// Create the trace string.
            //stringBuilder.Append("! Exiting ");
            //AppendContext(stringBuilder, args.Instance, args.Arguments);
            //stringBuilder.Append(" with exception ");
            //stringBuilder.Append(args.Exception.GetType().Name);
            //stringBuilder.Append(": ");
            //stringBuilder.Append(args.Exception.Message);

            //// Add the string to the trace.
            //System.Diagnostics.Trace.Unindent();
            //System.Diagnostics.Trace.WriteLine(stringBuilder.ToString());
        }
        public override void OnExit(MethodExecutionArgs args)
        {
            base.OnExit(args);
        }

        private void AppendContext(StringBuilder builder, object instance, Arguments arguments)
        {
            builder.Append(this._methodName);
            builder.Append('(');

            bool comma = false;
            if (instance != null)
            {
                builder.Append("this=");
                AppendObject(builder, instance);
                comma = true;
            }

            for (int i = 0; i < arguments.Count; i++)
            {
                if (comma)
                    builder.Append(", ");
                else
                    comma = true;

                AppendObject(builder, arguments[i]);
            }
            builder.Append(')');
        }

        private static void AppendObject(StringBuilder builder, object obj)
        {
            if (obj == null)
            {
                builder.Append("null");
            }
            else
            {
                builder.Append('{');
                builder.Append(obj.ToString());
                builder.Append('}');
            }
        }
    }
}
