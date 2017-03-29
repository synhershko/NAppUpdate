using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.SampleApp
{
	/// <summary>
	/// Interaction logic for UpdateWindow.xaml
	/// </summary>
	public partial class UpdateWindow : Window
	{
		private readonly UpdateManager _updateManager;
		private readonly UpdateTaskHelper _helper;
		private IList<UpdateTaskInfo> _updates;
		private int _downloadProgress;

		public UpdateWindow()
		{
			_updateManager = UpdateManager.Instance;
			_helper = new UpdateTaskHelper();
			InitializeComponent();

			var iconStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("NAppUpdate.Framework.updateicon.ico");
			if (iconStream != null)
				this.Icon = new IconBitmapDecoder(iconStream, BitmapCreateOptions.None, BitmapCacheOption.Default).Frames[0];
			this.DataContext = _helper;
		}

		private void InstallNow_Click(object sender, RoutedEventArgs e)
		{
			ShowThrobber();
			// dummy time delay for demonstration purposes
			var t = new System.Timers.Timer(2000) { AutoReset = false };
			t.Start();
			while (t.Enabled) { DoEvents(); }

			_updateManager.BeginPrepareUpdates(asyncResult =>
			{
				((UpdateProcessAsyncResult)asyncResult).EndInvoke();

				// ApplyUpdates is a synchronous method by design. Make sure to save all user work before calling
				// it as it might restart your application
				// get out of the way so the console window isn't obstructed
				try
				{
					_updateManager.ApplyUpdates(true);

					if (Dispatcher.CheckAccess())
					{
						Close();
					}
					else
					{
						Dispatcher.Invoke(new Action(Close));
					}
				}
				catch
				{
					MessageBox.Show("An error occurred while trying to install software updates");
				}
				finally
				{
					_updateManager.CleanUp();
				}
			}, null);
		}

		static void DoEvents()
		{
			var frame = new DispatcherFrame(true);
			Dispatcher.CurrentDispatcher.BeginInvoke
				(
					DispatcherPriority.Background,
					(System.Threading.SendOrPostCallback)delegate(object arg)
						{
							var f = arg as DispatcherFrame;
							if (f != null) f.Continue = false;
						},
					frame
				);
			Dispatcher.PushFrame(frame);
		}

		private void ShowThrobber()
		{
			btnInstallAtExit.Visibility = Visibility.Collapsed;
			btnInstallNow.Visibility = Visibility.Collapsed;
			imgThrobber.Height = 30;
			imgThrobber.Visibility = Visibility.Visible;
			lblDownload.Visibility = Visibility.Visible;
		}

		private void InstallAtExit_Click(object sender, RoutedEventArgs e)
		{
			Close();
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
