using System;
using System.Windows;
using NAppUpdate.Framework;

namespace NAppUpdate.SampleApp
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        public UpdateWindow()
        {
            InitializeComponent();
        }

        private void InstallNow_Click(object sender, RoutedEventArgs e)
        {
            UpdateManager updateManager = UpdateManager.Instance;

            updateManager.PrepareUpdatesAsync(finished =>
            {
                if (finished)
                    updateManager.ApplyUpdates();
                else
                    updateManager.CleanUp();

                Action close = () => Close();

                if (Dispatcher.CheckAccess())
                    close();
                else
                    Dispatcher.Invoke(close);
            });
        }

        private void InstallOnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}