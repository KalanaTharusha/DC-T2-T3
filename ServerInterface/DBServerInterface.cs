using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using ServerInterface;

namespace Server
{
    [ServiceContract]
    public interface DBServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(IndexFault))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap bitmap);
    }
}
