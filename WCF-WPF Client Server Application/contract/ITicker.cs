using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace contract
{
    [ServiceContract(CallbackContract = typeof(IAlert))]
    public interface ITicker
    {
        [OperationContract]
        void Register();
        [OperationContract]
        void Unregister();
    }

    public interface IAlert
    {
        [OperationContract]
        void OnAlert(DateTime time);
    }
}
