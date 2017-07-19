using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infra.Service.Core.ServiceBase
{
    public class ServiceBase : IServiceBase
    {
        public TRet CallServiceMethod<TParam, TRet>(Func<TParam, TRet> method, TParam param) where TRet : class
        {
            try
            {
                this.ExtractHeaderInformation();
                return method(param);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public TRet CallServiceMethod<TParam1, TParam2, TRet>(Func<TParam1, TParam2, TRet> method, TParam1 parameter1, TParam2 parameter2) where TRet : class
        {
            try
            {
                this.ExtractHeaderInformation();
                return method(parameter1, parameter2);
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public TRet CallServiceMethod<TRet>(Func<TRet> method) where TRet : class
        {
            try
            {
                this.ExtractHeaderInformation();
                return method();
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public void CallServiceMethod(Action method)
        {
            try
            {
                this.ExtractHeaderInformation();
                method();

            }
            catch (Exception exception)
            {

            }
        }

        public void CallServiceMethod<TParam>(Action<TParam> method, TParam parameter)
        {
            try
            {
                this.ExtractHeaderInformation();
                method(parameter);
            }
            catch (Exception exception)
            {

            }
        }

        private string ExtractHeaderInformation()
        {
            return string.Empty;
        }
    }
}
