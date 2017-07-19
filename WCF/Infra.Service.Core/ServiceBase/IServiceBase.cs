using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infra.Service.Core.ServiceBase
{
    public interface IServiceBase
    {
        TRet CallServiceMethod<TParam, TRet>(Func<TParam, TRet> method, TParam param) where TRet : class;
        TRet CallServiceMethod<TParam1, TParam2, TRet>(Func<TParam1, TParam2, TRet> method, TParam1 parameter1, TParam2 parameter2) where TRet : class;
        TRet CallServiceMethod<TRet>(Func<TRet> method) where TRet : class;
        void CallServiceMethod(Action method);
        void CallServiceMethod<TParam>(Action<TParam> method, TParam parameter);
    }

}
