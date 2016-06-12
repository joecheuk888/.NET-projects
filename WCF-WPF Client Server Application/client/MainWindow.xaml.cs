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
using contract;
using System.ComponentModel;
using client.TickerService;

namespace client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool isClosing = false;
        private TickerClient proxy;
        public MainWindow()
        {
            InitializeComponent();
            var instanceContext = new InstanceContext(new ClientCallBack(time_block, clock_image));
            Closing += new CancelEventHandler(ClosingEvent);
            proxy = new TickerClient(instanceContext);
        }

        public void ClosingEvent(object sender, CancelEventArgs e)
        {
            if (isClosing)
                return;
            isClosing = true;
            ((MainWindow)sender).proxy.Unregister();
            while (ClientCallBack.windows.Count != 0)
            {
                MainWindow window = ClientCallBack.windows.Pop();
                try
                {
                    if (!((MainWindow)sender).Equals(window))
                    {
                        window.proxy.Unregister();
                        window.Close();
                    }
                }
                catch (Exception er)
                {
                    Console.WriteLine(er.StackTrace.ToString());
                    break;
                }
            }
           isClosing = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Register();
        }

        public void Register()
        {
            proxy.Register();
        }
    }
}
