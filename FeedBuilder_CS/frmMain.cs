using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Xml;
namespace FeedBuilder
{

	public partial class frmMain
	{

		#region " Private constants/variables"
		private const string DialogFilter = "Feed configuration files (*.config)|*.config|All files (*.*)|*.*";
		private const string DefaultFileName = "FeedBuilder.config";
		private OpenFileDialog _openDialog;
			#endregion
		private ArgumentsParser _argParser;

		#region " Properties"
		public string FileName { get; set; }
		public bool ShowGui { get; set; }
		#endregion

		#region " Loading/Initialization/Lifetime"

		private void frmMain_Load(System.Object sender, System.EventArgs e)
		{
			this.Visible = false;

			string[] args = Environment.GetCommandLineArgs();
			// The first arg is the path to ourself
			//If args.Count >= 2 Then
			//    If File.Exists(args(1)) Then
			//        Dim p As New FeedBuilderSettingsProvider()
			//        p.LoadFrom(args(1))
			//        Me.FileName = args(1)
			//    End If
			//End If

			// The first arg is the path to ourself
			_argParser = new ArgumentsParser(args);
			if (_argParser.HasArgs) {
				this.FileName = _argParser.FileName;
				if (!string.IsNullOrEmpty(this.FileName)) {
					if (File.Exists(this.FileName)) {
						FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
						p.LoadFrom(this.FileName);
					} else {
						_argParser.ShowGui = true;
						_argParser.Build = false;
					}
				}
				InitializeFormSettings();
				if (_argParser.ShowGui)
					this.Show();
				if (_argParser.Build) {
					Build();
				}
				if (!_argParser.ShowGui)
					this.Close();
			}

		}

		private void InitializeFormSettings()
		{
			if (!string.IsNullOrEmpty(Settings.Default.OutputFolder) && Directory.Exists(Settings.Default.OutputFolder))
				txtOutputFolder.Text = Settings.Default.OutputFolder;
			if (!string.IsNullOrEmpty(Settings.Default.FeedXML) && Directory.Exists(Path.GetDirectoryName(Settings.Default.FeedXML)))
				txtFeedXML.Text = Settings.Default.FeedXML;
			if (!string.IsNullOrEmpty(Settings.Default.BaseURL))
				txtBaseURL.Text = Settings.Default.BaseURL;

			chkVersion.Checked = Settings.Default.CompareVersion;
			chkSize.Checked = Settings.Default.CompareSize;
			chkDate.Checked = Settings.Default.CompareDate;
			chkHash.Checked = Settings.Default.CompareHash;

			chkIgnoreSymbols.Checked = Settings.Default.IgnoreDebugSymbols;
			chkIgnoreVsHost.Checked = Settings.Default.IgnoreVsHosting;
			chkCopyFiles.Checked = Settings.Default.CopyFiles;
			chkCleanUp.Checked = Settings.Default.CleanUp;

			if (Settings.Default.IgnoreFiles == null)
				Settings.Default.IgnoreFiles = new System.Collections.Specialized.StringCollection();
			ReadFiles();
		}

		private void SaveFormSettings()
		{
			if (!string.IsNullOrEmpty(txtOutputFolder.Text.Trim()) && Directory.Exists(txtOutputFolder.Text.Trim()))
				Settings.Default.OutputFolder = txtOutputFolder.Text.Trim();
			if (!string.IsNullOrEmpty(txtFeedXML.Text.Trim()) && Directory.Exists(Path.GetDirectoryName(txtFeedXML.Text.Trim())))
				Settings.Default.FeedXML = txtFeedXML.Text.Trim();
			if (!string.IsNullOrEmpty(txtBaseURL.Text.Trim()))
				Settings.Default.BaseURL = txtBaseURL.Text.Trim();

			Settings.Default.CompareVersion = chkVersion.Checked;
			Settings.Default.CompareSize = chkSize.Checked;
			Settings.Default.CompareDate = chkDate.Checked;
			Settings.Default.CompareHash = chkHash.Checked;

			Settings.Default.IgnoreDebugSymbols = chkIgnoreSymbols.Checked;
			Settings.Default.IgnoreVsHosting = chkIgnoreVsHost.Checked;
			Settings.Default.CopyFiles = chkCopyFiles.Checked;
			Settings.Default.CleanUp = chkCleanUp.Checked;

			var _with1 = Settings.Default.IgnoreFiles;
            if (_with1 == null) return;
            _with1.Clear();
			foreach (ListViewItem thisItem in lstFiles.Items) {
				if (!thisItem.Checked)
					_with1.Add(thisItem.Text);
			}
		}

		private void frmMain_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			SaveFormSettings();
		}

		#endregion
		#region " Commands Events"
		private void cmdBuild_Click(System.Object sender, System.EventArgs e)
		{
			Build();
		}

		private void btnNew_Click(System.Object sender, System.EventArgs e)
		{
			Settings.Default.Reset();
			InitializeFormSettings();
		}

		private void btnOpen_Click(System.Object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = null;
			if (this._openDialog == null) {
				dlg = new OpenFileDialog();
				dlg.CheckFileExists = true;
				if (string.IsNullOrEmpty(this.FileName)) {
					dlg.FileName = DefaultFileName;
				} else {
					dlg.FileName = this.FileName;
				}
				this._openDialog = dlg;
			} else {
				dlg = this._openDialog;
			}
			dlg.Filter = DialogFilter;
			DialogResult result = dlg.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK) {
				FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
				p.LoadFrom(dlg.FileName);
				InitializeFormSettings();
				this.FileName = dlg.FileName;
			}
		}

		private void btnSave_Click(System.Object sender, System.EventArgs e)
		{
			Save(false);
		}

		private void btnSaveAs_Click(System.Object sender, System.EventArgs e)
		{
			Save(true);
		}

		private void btnRefresh_Click(System.Object sender, System.EventArgs e)
		{
			ReadFiles();
		}

		#endregion
		#region " Options Events"
		private void cmdOutputFolder_Click(System.Object sender, System.EventArgs e)
		{
			fbdOutputFolder.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (fbdOutputFolder.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
				txtOutputFolder.Text = fbdOutputFolder.SelectedPath;
				ReadFiles();
			}
		}

		private void cmdFeedXML_Click(System.Object sender, System.EventArgs e)
		{
			sfdFeedXML.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (sfdFeedXML.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				txtFeedXML.Text = sfdFeedXML.FileName;
		}

		private void chkIgnoreSymbols_CheckedChanged(object sender, System.EventArgs e)
		{
			ReadFiles();
		}

		private void chkCopyFiles_CheckedChanged(System.Object sender, System.EventArgs e)
		{
			chkCleanUp.Enabled = chkCopyFiles.Checked;
			if (!chkCopyFiles.Checked)
				chkCleanUp.Checked = false;
		}

		#endregion
		#region " Helper Methods "
		private void Build()
		{
			Console.WriteLine("Building NAppUpdater feed '{0}'", txtBaseURL.Text.Trim());
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
			XmlElement feed = null;
			XmlElement tasks = null;
			XmlElement task = null;
			XmlElement conds = null;
			XmlElement cond = null;

			doc.AppendChild(dec);
			feed = doc.CreateElement("Feed");
			if (!string.IsNullOrEmpty(txtBaseURL.Text.Trim()))
				feed.SetAttribute("BaseUrl", txtBaseURL.Text.Trim());
			doc.AppendChild(feed);

			tasks = doc.CreateElement("Tasks");

			Console.WriteLine("Processing feed items");
			int itemsCopied = 0;
			int itemsCleaned = 0;
			int itemsSkipped = 0;
			int itemsFailed = 0;
			foreach (ListViewItem thisItem in lstFiles.Items) {
				string destFile = Path.Combine(Path.GetDirectoryName(txtFeedXML.Text.Trim()), thisItem.Text);

				if (thisItem.Checked) {

					var _with2 = (FileInfoEx)thisItem.Tag;
					task = doc.CreateElement("FileUpdateTask");
					task.SetAttribute("localPath", _with2.FileInfo.Name);

					conds = doc.CreateElement("Conditions");

					//Version
					if (chkVersion.Checked && !string.IsNullOrEmpty(_with2.FileVersion)) {
						cond = doc.CreateElement("FileVersionCondition");
                        cond.SetAttribute("type", "or");
						cond.SetAttribute("what", "below");
						cond.SetAttribute("version", _with2.FileVersion);
						conds.AppendChild(cond);
					}

					//Size
					if (chkSize.Checked) {
						cond = doc.CreateElement("FileSizeCondition");
                        cond.SetAttribute("type", "or-not");
						cond.SetAttribute("what", "is");
						cond.SetAttribute("size", _with2.FileInfo.Length.ToString());
						conds.AppendChild(cond);
					}

					//Date
					if (chkDate.Checked) {
                        cond = doc.CreateElement("FileDateCondition");
                        cond.SetAttribute("type", "or");
						cond.SetAttribute("what", "older");
                        // local timestamp, not UTC
						cond.SetAttribute("timestamp", _with2.FileInfo.LastWriteTime.ToFileTime().ToString());
						conds.AppendChild(cond);
					}

					//Hash
					if (chkHash.Checked) {
						cond = doc.CreateElement("FileChecksumCondition");
                        cond.SetAttribute("type", "or-not");
						cond.SetAttribute("checksumType", "sha256");
						cond.SetAttribute("checksum", _with2.Hash);
						conds.AppendChild(cond);
					}

					task.AppendChild(conds);
					tasks.AppendChild(task);

					if (chkCopyFiles.Checked) {
						try {
							if (File.Exists(destFile))
								File.Delete(destFile);
							File.Copy(_with2.FileInfo.FullName, destFile);
							itemsCopied += 1;
						} catch (IOException) {
							itemsFailed += 1;
						}
					}
				} else {
					try {
						if (chkCleanUp.Checked & File.Exists(destFile)) {
							File.Delete(destFile);
							itemsCleaned += 1;
						} else {
							itemsSkipped += 1;
						}
					} catch (IOException) {
						itemsFailed += 1;
					}
				}
			}
			feed.AppendChild(tasks);
			doc.Save(txtFeedXML.Text.Trim());

			// open the outputs folder if we're running from the GUI or 
			// we have an explicit command line option to do so
			if (!_argParser.HasArgs || _argParser.OpenOutputsFolder) {
				OpenOutputsFolder();
			}
			Console.WriteLine("Done building feed.");
			if (itemsCopied > 0)
				Console.WriteLine("{0,5} items copied", itemsCopied);
			if (itemsCleaned > 0)
				Console.WriteLine("{0,5} items cleaned", itemsCleaned);
			if (itemsSkipped > 0)
				Console.WriteLine("{0,5} items skipped", itemsSkipped);
			if (itemsFailed > 0)
				Console.WriteLine("{0,5} items failed", itemsFailed);
		}

		private void OpenOutputsFolder()
		{
			var _with3 = new Process();
			_with3.StartInfo.UseShellExecute = true;
			_with3.StartInfo.FileName = Path.GetDirectoryName(txtFeedXML.Text.Trim());
			_with3.Start();
		}

		private int GetImageIndex(string ext)
		{
			switch (ext.Trim('.')) {
				case "bmp":
					return 1;
				case "dll":
					return 2;
				case "doc":
				case "docx":
					return 3;
				case "exe":
					return 4;
				case "htm":
				case "html":
					return 5;
				case "jpg":
				case "jpeg":
					return 6;
				case "pdf":
					return 7;
				case "png":
					return 8;
				case "txt":
					return 9;
				case "wav":
				case "mp3":
					return 10;
				case "wmv":
					return 11;
				case "xls":
				case "xlsx":
					return 12;
				case "zip":
					return 13;
				default:
					return 0;
			}
		}

		private void ReadFiles()
		{
			if (!string.IsNullOrEmpty(txtOutputFolder.Text.Trim()) && Directory.Exists(txtOutputFolder.Text.Trim())) {
				ListViewItem thisItem = null;
				FileInfoEx thisInfo = null;

				lstFiles.BeginUpdate();
				lstFiles.Items.Clear();
                var enumerator = new FindFiles.FileSystemEnumerator(txtOutputFolder.Text.Trim(),"*.*", true);
				foreach (FileInfo fi in enumerator.Matches()) {
                    string thisFile = fi.FullName;
					if ((!IsIgnorable(thisFile))) {
						thisInfo = new FileInfoEx(thisFile);
						var _with4 = thisInfo;
						thisItem = new ListViewItem(_with4.FileInfo.Name, GetImageIndex(_with4.FileInfo.Extension));
						thisItem.SubItems.Add(_with4.FileVersion);
						thisItem.SubItems.Add(_with4.FileInfo.Length.ToString());
						thisItem.SubItems.Add(_with4.FileInfo.LastWriteTime.ToString());
						thisItem.SubItems.Add(_with4.Hash);
						thisItem.Checked = (!Settings.Default.IgnoreFiles.Contains(_with4.FileInfo.Name));
						thisItem.Tag = thisInfo;
						lstFiles.Items.Add(thisItem);
					}
				}
				lstFiles.EndUpdate();
			}
		}

		private bool IsIgnorable(string filename)
		{
			string ext = Path.GetExtension(filename);
			if ((chkIgnoreSymbols.Checked && ext == ".pdb"))
				return true;
			if ((chkIgnoreVsHost.Checked && filename.ToLower().Contains("vshost.exe")))
				return true;
			return false;
		}

		private void Save(bool forceDialog)
		{
			SaveFormSettings();
			if (forceDialog || string.IsNullOrEmpty(this.FileName)) {
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = DialogFilter;
				dlg.FileName = DefaultFileName;
				DialogResult result = dlg.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK) {
					FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
					p.SaveAs(dlg.FileName);
					this.FileName = dlg.FileName;
				}
			} else {
				FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
				p.SaveAs(this.FileName);
			}
		}

		#endregion

	}
}
