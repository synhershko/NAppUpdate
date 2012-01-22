using System;
using System.IO;
using System.Reflection;
using System.Windows;
using NAppUpdate.Framework;

namespace NAppUpdate.SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string AppVersion { get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); } }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateManager updManager = UpdateManager.Instance;

            //update configuration
            updManager.UpdateFeedReader = new NAppUpdate.Framework.FeedReaders.AppcastReader();
            updManager.UpdateSource = new NAppUpdate.Framework.Sources.SimpleWebSource();

            updManager.CheckForUpdateAsync(new NAppUpdate.Framework.Sources.MemorySource(File.ReadAllText("sampleappupdatefeed.xml"))
                , updatesCount =>
                {
                    Action showUpdateAction = () => new UpdateWindow(UpdateManager.Instance).Show();

                    if (updatesCount > 0)
                    {
                        if (Dispatcher.CheckAccess())
                            showUpdateAction();
                        else
                            Dispatcher.Invoke(showUpdateAction);
                    }
                });
        }
    }
}
