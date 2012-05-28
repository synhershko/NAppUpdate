using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using NAppUpdate.Framework;

namespace NAppUpdate.SampleApp
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window, INotifyPropertyChanged
    {
        private readonly UpdateManager _updateManager;
        private UpdateTaskHelper _helper;
        private IList<UpdateTaskInfo> _updates;
        private int _downloadProgress;

        public UpdateWindow(UpdateManager updateManager)
        {
            _updateManager = UpdateManager.Instance;
            _helper = new UpdateTaskHelper();
            InitializeComponent();
            System.IO.Stream iconStream = _updateManager.IconStream;
            this.Icon = new IconBitmapDecoder(iconStream, BitmapCreateOptions.None, BitmapCacheOption.Default).Frames[0];
            this.grdUpdates.ItemsSource = _helper.TaskListInfo;
            this.DataContext = _helper;
        }

        // TODO: Reimplement download progress bar
        public int DownloadProgress
        {
            get { return _downloadProgress; }
            set
            {
                _downloadProgress = value;
                InvokePropertyChanged("DownloadProgress");
            }
        }

        private void InstallNow_Click(object sender, RoutedEventArgs e)
        {
            _updateManager.PrepareUpdatesAsync(finished =>
                                                   {
                                                       try
                                                       {
                                                           if (finished)
                                                           {
                                                               _updateManager.ApplyUpdates();
                                                               // ApplyUpdates is a synchronous method by design. Make sure to save all user work before calling
                                                               // it as it might restart your application
                                                               // get out of the way so the console window isn't obstructed
                                                               Dispatcher d = Application.Current.Dispatcher;
                                                               d.BeginInvoke(new Action(() => this.Hide()));
                                                               if (!_updateManager.ApplyUpdates(true, true, true))
                                                               {
                                                                   d.BeginInvoke(new Action(() => this.Show())); // this.WindowState = WindowState.Normal;
                                                                   MessageBox.Show("An error occurred while trying to install software updates");
                                                               }
                                                               else
                                                               {
                                                                   d.BeginInvoke(new Action(() => this.Close()));
                                                               }
                                                               _updateManager.CleanUp();
                                                               App.Current.Dispatcher.BeginInvoke(new Action(() => this.Close()));
                                                           }
                                                           else
                                                               _updateManager.CleanUp();
                                                       }
                                                       catch (System.Exception ex)
                                                       {
                                                           MessageBox.Show("Error", "There was a problem with the update: \n" + ex.Message);
                                                       }
                                                   });
                                                /*progressPercent =>
                                                   {
                                                       this.DownloadProgress = progressPercent;
                                                   }*/
        }

        private void InstallAtExit_Click(object sender, RoutedEventArgs e)
        {
            // TODO: the main application needs to know this was clicked?
            this.Close();
        }


        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        public void InvokePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}