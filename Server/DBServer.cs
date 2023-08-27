using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using DataLibrary;
using System.Drawing;
using ServerInterface;
using System.IO;

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

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out String bitmap)
        {
            try
            {
                acctNo = database.GetAcctNoByIndex(index);
                pin = database.GetPINByIndex(index);
                bal = database.GetBalanceByIndex(index);
                fName = database.GetFirstNameByIndex(index);
                lName = database.GetLastNameByIndex(index);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Save the Bitmap to the MemoryStream in a specified format
                    database.GetBitmapByIndex(index).Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                    // Convert the MemoryStream to a byte array
                    byte[] imageBytes = memoryStream.ToArray();

                    // Convert the byte array to a base64 encoded string
                    bitmap = Convert.ToBase64String(imageBytes);
                }
                // bitmap = database.GetBitmapByIndex(index);
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
