using System.Windows;
using System.Windows.Threading;
using System.Threading;

namespace Commercial_Spoils_App
{
    class Helper
    {
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(DoEvents_DoNothing));
        }

        private static void DoEvents_DoNothing()
        {

        }
    }
}
