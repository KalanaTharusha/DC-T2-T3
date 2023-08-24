using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ServiceModel;
using Server;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using System.Drawing;
using System.Windows.Interop;
using ServerInterface;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<DBServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DBServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            ItemCountBlock.Text = "Total Items: " + foob.GetNumEntries().ToString();

        }

        private void goBtn_Click(object sender, RoutedEventArgs e)
        {

            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            Bitmap bitmap = null;

            try
            {
                index = Int32.Parse(indexBox.Text);

                if (index >= 0 && index < foob.GetNumEntries())
                {

                    foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out bitmap);

                    fNameBox.Text = fName;
                    lNameBox.Text = lName;
                    accNoBox.Text = acct.ToString();
                    pinBox.Text = pin.ToString("D4");
                    balBox.Text = bal.ToString("C");

                    if (bitmap != null)
                    {
                        updateProfilePic(bitmap);
                    }

                }
                else
                {
                    throw new Exception("Invalid Index");
                }
            }
            catch (FaultException<IndexFault> eex)
            {
                MessageBox.Show(eex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void updateProfilePic(Bitmap bitmap)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    ImageSourceConverter converter = new ImageSourceConverter();
                    var pic = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    profilePic.Source = pic;
                }
                finally
                {
                    bitmap.Dispose();
                }
            });
        }
    }
}
