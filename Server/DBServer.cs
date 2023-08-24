using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DataLibrary;
using System.Drawing;
using ServerInterface;

namespace Server
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    internal class DBServer : DBServerInterface
    {
        Database database;

        public DBServer()
        {
            database = new Database();
        }

        public int GetNumEntries()
        {
            return database.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap bitmap)
        {
            try
            {
                acctNo = database.GetAcctNoByIndex(index);
                pin = database.GetPINByIndex(index);
                bal = database.GetBalanceByIndex(index);
                fName = database.GetFirstNameByIndex(index);
                lName = database.GetLastNameByIndex(index);
                bitmap = database.GetBitmapByIndex(index);
            }
            catch (IndexOutOfRangeException)
            {
                IndexFault indexFault = new IndexFault();
                indexFault.Message = "Index Out of Bound";
                throw new FaultException<IndexFault>(indexFault);
            }
        }
    }
}
