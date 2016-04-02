using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel;
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

		// TODO: Reimplement download progress bar
		private void InstallNow_Click(object sender, RoutedEventArgs e)
		{
			ShowThrobber();
			// dummy time delay for demonstration purposes
			var t = new System.Timers.Timer(5000) { AutoReset = false };
			t.Start();
			while (t.Enabled) { DoEvents(); }

			_updateManager.BeginPrepareUpdates(asyncResult =>
				{
					((UpdateProcessAsyncResult)asyncResult).EndInvoke();

					// ApplyUpdates is a synchronous method by design. Make sure to save all user work before calling
					// it as it might restart your application
					// get out of the way so the console window isn't obstructed
					Dispatcher d = Application.Current.Dispatcher;
					d.BeginInvoke(new Action(Hide));
					try
					{
						_updateManager.ApplyUpdates(true);
						d.BeginInvoke(new Action(Close));
					}
					catch
					{
						d.BeginInvoke(new Action(this.Show));
						// this.WindowState = WindowState.Normal;
						MessageBox.Show(
							"An error occurred while trying to install software updates");
					}

					_updateManager.CleanUp();
					d.BeginInvoke(new Action(this.Close));

					Action close = Close;

					if (Dispatcher.CheckAccess())
						close();
					else
						Dispatcher.Invoke(close);
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
			// TODO: the main application needs to know this was clicked?
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
