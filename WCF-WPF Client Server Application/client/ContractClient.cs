using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using contract;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel;

namespace client
{
    public class ContractClient : DuplexClientBase<ITicker>
    {
        public ContractClient(object callback) : base(callback) { }
    }

    public class ClientCallBack : TickerService.ITickerCallback
    {
        public static Stack<MainWindow> windows = new Stack<MainWindow>();
        [Import]
        private IDrawClockFace drawer;
        private TextBlock textblock;
        private System.Windows.Controls.Image clock_image;
        public ClientCallBack(TextBlock tb, System.Windows.Controls.Image img)
        {
            var catalog = new DirectoryCatalog(@"..\..\..\drawservice\bin\Debug");
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
            textblock = tb;
            clock_image = img;
        }

        public void OnAlert(DateTime time)
        {
            if (time == DateTime.MinValue)
            {
                var cloned = new MainWindow();
                cloned.Register();
                windows.Push(cloned);
                cloned.Closing += new CancelEventHandler(cloned.ClosingEvent);
                cloned.Show();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(
                  DispatcherPriority.Background, new Action(() => ClientActions(time))
                );
            }
        }

        private void ClientActions(DateTime time)
        {
            textblock.Text = time.ToString();
            AlertClock(time);
        }

        private void AlertClock(DateTime time)
        {
            Bitmap clockBit = drawer.DrawClockFace(time, (int)clock_image.Width, (int)clock_image.Height);
            var hbmp = clockBit.GetHbitmap();
            var options = BitmapSizeOptions.FromEmptyOptions();
            clock_image.Source = Imaging.CreateBitmapSourceFromHBitmap(hbmp, IntPtr.Zero, Int32Rect.Empty, options);
        }
    }
}
