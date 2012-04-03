using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using NAppUpdate.Framework;

namespace NAppUpdate.SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string AppVersion { get { return File.ReadAllText("CurrentVersion.txt"); } }
        private bool applyUpdates;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private NAppUpdate.Framework.Sources.IUpdateSource PrepareUpdateSource()
        {
            // Normally this would be a web based source.
            // But for the demo app, we prepare an in-memory source.

            var source = new NAppUpdate.Framework.Sources.MemorySource(File.ReadAllText("SampleAppUpdateFeed.xml"));

            // Add the update file.
            source.AddTempFile(new Uri("http://SomeSite.com/Files/NewVersion.txt"), "NewVersion.txt");

            return source;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateManager updManager = UpdateManager.Instance;

            //update configuration
            updManager.UpdateFeedReader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReader();
            updManager.UpdateSource = PrepareUpdateSource();

            updManager.CheckForUpdateAsync(updatesCount =>
            {
                Action showUpdateAction = () => ShowUpdateWindow();

                if (updatesCount > 0)
                {
                    applyUpdates = true;

                    if (Dispatcher.CheckAccess())
                        showUpdateAction();
                    else
                        Dispatcher.Invoke(showUpdateAction);
                }
            });
        }

        private void ShowUpdateWindow()
        {
            var updateWindow = new UpdateWindow();

            updateWindow.Closed += (sender, e) =>
            {
                if (UpdateManager.Instance.State == UpdateManager.UpdateProcessState.AppliedSuccessfully)
                {
                    applyUpdates = false;

                    // Update the app version
                    OnPropertyChanged("AppVersion");
                }
            };

            updateWindow.Show();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Do any updates.
            if (applyUpdates)
            {
                if (UpdateManager.Instance.PrepareUpdates())
                    UpdateManager.Instance.ApplyUpdates(false);
                else
                    UpdateManager.Instance.CleanUp();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
