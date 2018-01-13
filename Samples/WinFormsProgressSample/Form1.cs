using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Sources;

namespace WinFormsProgressSample
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			progressBar1.Minimum = 0;
			progressBar1.Maximum = 100;
			progressBar1.Step = 1;

			// UpdateManager initialization
			UpdateManager updManager = UpdateManager.Instance;
			updManager.UpdateFeedReader = new DummyReader();
			updManager.UpdateSource = new MemorySource(string.Empty);

			// Setup UI progress notifications
			updManager.ReportProgress += status =>
											{
												lblDetails.Invoke(new Action(() => lblDetails.Text = status.Message));
												lblOverview.Invoke(new Action(() => lblOverview.Text = string.Format("Phase: {0}, executing task #{1}: {2}",
																							   UpdateManager.Instance.State,
																							   status.TaskId,
																							   status.TaskDescription
																					)));

												progressBar1.Invoke(new Action(() =>
																				{
																					progressBar1.Value = status.Percentage;
																				}));

												if (!status.StillWorking)
													btnStart.Invoke(new Action(() => btnStart.Enabled = true));
											};
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			btnStart.Enabled = false;
			progressBar1.Step = 0;

			UpdateManager.Instance.BeginCheckForUpdates(asyncResult => UpdateManager.Instance.BeginPrepareUpdates(ar2 =>
																											{
																												//UpdateManager.
																												//    Instance.
																												//    ApplyUpdates(false);
																											}
																	, null), null);
		}
	}
}
