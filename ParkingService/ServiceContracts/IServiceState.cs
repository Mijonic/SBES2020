using ServiceContracts.Enums;
using ServiceContracts.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    [ServiceContract]
    public interface IServiceState
    {
        [OperationContract]
        [FaultContract(typeof(ServiceStateException))]
        EServiceState CheckServiceState();


        [OperationContract]
        [FaultContract(typeof(ServiceStateException))]
        void UpdateServiceState(EServiceState state);
    }
}
