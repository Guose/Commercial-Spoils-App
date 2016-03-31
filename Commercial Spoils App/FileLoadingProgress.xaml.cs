using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Commercial_Spoils_App
{
    /// <summary>
    /// Interaction logic for FileLoadingProgress.xaml
    /// </summary>
    public partial class FileLoadingProgress : Window
    {
        
        public FileLoadingProgress()
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitializeComponent();
            ProgressBarExecuter();
        }
        
        public void ProgressBarExecuter()
        {
                ProgressBar progressBar = new ProgressBar();
                progressBar.IsIndeterminate = false;
                progressBar.Orientation = Orientation.Horizontal;
                progressBar.Width = 300;
                progressBar.Height = 25;

                Duration duration = new Duration(TimeSpan.FromSeconds((4)));
                DoubleAnimation doubleAnima = new DoubleAnimation(200, duration);
                statusBar1.Items.Add(progressBar);

                TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

                Action MainThreadDoWork = new Action(() =>
                {
                    Thread.Sleep(4000);
                });

                Action ExecuteProgressBar = new Action(() =>
                {
                    progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnima);

                });

                Action FinalThreadDoWork = new Action(() =>
                {
                    statusBar1.Items.Remove(progressBar);
                    this.Close();
                });

                Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadDoWork());

                Task ExecuteProgressBarTask = new Task(ExecuteProgressBar);

                ExecuteProgressBarTask.RunSynchronously();

                MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWork(), uiThread);
             
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
