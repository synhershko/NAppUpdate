using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using FeedBuilder.Properties;

namespace FeedBuilder
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

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
		private bool loadedSettings = false;
		private async void frmMain_Load(Object sender, EventArgs e)
		{


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

			if (!_argParser.HasArgs)
			{
				FreeConsole();
				return;
			}

			Visible = false;
			FileName = _argParser.FileName;
			if (!string.IsNullOrEmpty(FileName))
			{
				if (File.Exists(FileName))
				{
					FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
					p.LoadFrom(FileName);
					await InitializeFormSettings();
				}
				else
				{
					_argParser.ShowGui = true;
					_argParser.Build = false;
					UpdateTitle();
				}
			}

			if (_argParser.ShowGui) Show();
			if (_argParser.Build) Build();
			if (!_argParser.ShowGui) Close();
		}
		private async void frmMain_Shown(object sender, EventArgs e)
		{
			if (!loadedSettings)
				await InitializeFormSettings();

		}
		private async Task InitializeFormSettings()
		{
			txtOutputFolder.Text = !string.IsNullOrEmpty(Settings.Default.OutputFolder) && Directory.Exists(Settings.Default.OutputFolder) ? Settings.Default.OutputFolder : string.Empty;
			txtFeedXML.Text = string.IsNullOrEmpty(Settings.Default.FeedXML) ? string.Empty : Settings.Default.FeedXML;
			txtBaseURL.Text = string.IsNullOrEmpty(Settings.Default.BaseURL) ? string.Empty : Settings.Default.BaseURL;
			txtMatchFileSpec.Text = string.IsNullOrEmpty(Settings.Default.MatchFileSpec) ? "*.*" : Settings.Default.MatchFileSpec;
			txtExcludeFileSpec.Text = string.IsNullOrEmpty(Settings.Default.ExcludeFileSpec) ? string.Empty : Settings.Default.ExcludeFileSpec;

			chkVersion.Checked = Settings.Default.CompareVersion;
			chkSize.Checked = Settings.Default.CompareSize;
			chkDate.Checked = Settings.Default.CompareDate;
			chkHash.Checked = Settings.Default.CompareHash;

			chkIgnoreSymbols.Checked = Settings.Default.IgnoreDebugSymbols;
			chkIgnoreVsHost.Checked = Settings.Default.IgnoreVsHosting;
			chkCopyFiles.Checked = Settings.Default.CopyFiles;
			chkCleanUp.Checked = Settings.Default.CleanUp;
			txtAddExtension.Text = Settings.Default.AddExtension;

			if (Settings.Default.IgnoreFiles == null) Settings.Default.IgnoreFiles = new StringCollection();
			await ReadFiles();
			UpdateTitle();
			loadedSettings = true;
		}

		private void UpdateTitle()
		{
			if (string.IsNullOrEmpty(FileName)) Text = "Feed Builder";
			else Text = "Feed Builder - " + FileName;
		}

		private void SaveFormSettings()
		{
			if (!string.IsNullOrEmpty(txtOutputFolder.Text.Trim()) && Directory.Exists(txtOutputFolder.Text.Trim())) Settings.Default.OutputFolder = txtOutputFolder.Text.Trim();
			// ReSharper disable AssignNullToNotNullAttribute
			if (!string.IsNullOrEmpty(txtFeedXML.Text.Trim()) && Directory.Exists(Path.GetDirectoryName(txtFeedXML.Text.Trim()))) Settings.Default.FeedXML = txtFeedXML.Text.Trim();
			// ReSharper restore AssignNullToNotNullAttribute
			if (!string.IsNullOrEmpty(txtBaseURL.Text.Trim())) Settings.Default.BaseURL = txtBaseURL.Text.Trim();

			Settings.Default.MatchFileSpec = string.IsNullOrWhiteSpace(txtMatchFileSpec.Text) ? "*.*" : txtMatchFileSpec.Text;
			Settings.Default.ExcludeFileSpec = string.IsNullOrWhiteSpace(txtMatchFileSpec.Text) ? string.Empty : txtExcludeFileSpec.Text;

			if (!string.IsNullOrEmpty(txtAddExtension.Text.Trim())) Settings.Default.AddExtension = txtAddExtension.Text.Trim();
			Settings.Default.CompareVersion = chkVersion.Checked;
			Settings.Default.CompareSize = chkSize.Checked;
			Settings.Default.CompareDate = chkDate.Checked;
			Settings.Default.CompareHash = chkHash.Checked;

			Settings.Default.IgnoreDebugSymbols = chkIgnoreSymbols.Checked;
			Settings.Default.IgnoreVsHosting = chkIgnoreVsHost.Checked;
			Settings.Default.CopyFiles = chkCopyFiles.Checked;
			Settings.Default.CleanUp = chkCleanUp.Checked;

			if (Settings.Default.IgnoreFiles == null) Settings.Default.IgnoreFiles = new StringCollection();
			Settings.Default.IgnoreFiles.Clear();
			foreach (ListViewItem thisItem in lstFiles.Items)
			{
				if (!thisItem.Checked) Settings.Default.IgnoreFiles.Add(thisItem.Text);
			}
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveFormSettings();
			Settings.Default.Save();
		}

		#endregion

		#region " Commands Events"

		private void cmdBuild_Click(Object sender, EventArgs e)
		{
			Build();
		}

		private void btnOpenOutputs_Click(object sender, EventArgs e)
		{
			OpenOutputsFolder();
		}

		private async void btnNew_Click(Object sender, EventArgs e)
		{
			Settings.Default.Reset();
			await InitializeFormSettings();
		}

		private async void btnOpen_Click(Object sender, EventArgs e)
		{
			OpenFileDialog dlg;
			if (_openDialog == null)
			{
				dlg = new OpenFileDialog
				{
					CheckFileExists = true,
					FileName = string.IsNullOrEmpty(FileName) ? DefaultFileName : FileName
				};
				_openDialog = dlg;
			}
			else dlg = _openDialog;
			dlg.Filter = DialogFilter;
			if (dlg.ShowDialog() != DialogResult.OK) return;
			FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
			p.LoadFrom(dlg.FileName);
			FileName = dlg.FileName;
			await InitializeFormSettings();
		}

		private void btnSave_Click(Object sender, EventArgs e)
		{
			Save(false);
		}

		private void btnSaveAs_Click(Object sender, EventArgs e)
		{
			Save(true);
		}

		private async void btnRefresh_Click(Object sender, EventArgs e)
		{
			await ReadFiles();
		}

		#endregion

		#region " Options Events"

		private async void cmdOutputFolder_Click(Object sender, EventArgs e)
		{
			fbdOutputFolder.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (fbdOutputFolder.ShowDialog(this) != DialogResult.OK) return;
			txtOutputFolder.Text = fbdOutputFolder.SelectedPath;
			await ReadFiles();
		}

		private void cmdFeedXML_Click(Object sender, EventArgs e)
		{
			sfdFeedXML.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			if (sfdFeedXML.ShowDialog(this) == DialogResult.OK) txtFeedXML.Text = sfdFeedXML.FileName;
		}

		private async void chkIgnoreSymbols_CheckedChanged(object sender, EventArgs e)
		{
			await ReadFiles();
		}

		private void chkCopyFiles_CheckedChanged(Object sender, EventArgs e)
		{
			chkCleanUp.Enabled = chkCopyFiles.Checked;
			if (!chkCopyFiles.Checked) chkCleanUp.Checked = false;
		}

		#endregion

		#region " Helper Methods "

		private async void Build()
		{
			SaveFormSettings();
			AttachConsole(ATTACH_PARENT_PROCESS);

			Console.WriteLine("Building NAppUpdater feed '{0}'", txtBaseURL.Text.Trim());
			if (string.IsNullOrEmpty(txtFeedXML.Text))
			{
				const string msg = "The feed file location needs to be defined.\n" + "The outputs cannot be generated without this.";
				if (_argParser.ShowGui) MessageBox.Show(msg);
				Console.WriteLine(msg);
				return;
			}
			// If the target folder doesn't exist, create a path to it
			string dest = txtFeedXML.Text.Trim();
			var destDir = Directory.GetParent(new FileInfo(dest).FullName);
			if (!Directory.Exists(destDir.FullName)) Directory.CreateDirectory(destDir.FullName);

			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);

			doc.AppendChild(dec);
			XmlElement feed = doc.CreateElement("Feed");
			if (!string.IsNullOrEmpty(txtBaseURL.Text.Trim())) feed.SetAttribute("BaseUrl", txtBaseURL.Text.Trim());
			doc.AppendChild(feed);

			XmlElement tasks = doc.CreateElement("Tasks");

			Console.WriteLine("Refreshing file data...");
			if (_argParser.Build || await ReadFiles())// Force refresh if we didn't build from cmdline
			{
				Console.WriteLine("Processing feed items");
				int itemsCopied = 0;
				int itemsCleaned = 0;
				int itemsSkipped = 0;
				int itemsFailed = 0;
				int itemsMissingConditions = 0;
				foreach (ListViewItem thisItem in lstFiles.Items)
				{
					string destFile = "";
					string folder = "";
					string filename = "";
					try
					{
						folder = Path.GetDirectoryName(txtFeedXML.Text.Trim());
						filename = thisItem.Text;
						if (folder != null) destFile = Path.Combine(folder, filename);
					}
					catch { }
					if (destFile == "" || folder == "" || filename == "")
					{
						string msg = string.Format("The file could not be pathed:\nFolder:'{0}'\nFile:{1}", folder, filename);
						if (_argParser.ShowGui) MessageBox.Show(msg);
						Console.WriteLine(msg);
						continue;
					}

					if (thisItem.Checked)
					{
						var fileInfoEx = (FileInfoEx)thisItem.Tag;
						XmlElement task = doc.CreateElement("FileUpdateTask");
						task.SetAttribute("localPath", fileInfoEx.RelativeName);
						// generate FileUpdateTask metadata items
						task.SetAttribute("lastModified", fileInfoEx.FileInfo.LastWriteTime.ToFileTime().ToString(CultureInfo.InvariantCulture));
						if (!string.IsNullOrEmpty(txtAddExtension.Text)) task.SetAttribute("updateTo", fileInfoEx.RelativeName + (txtAddExtension.Text.Trim().StartsWith(".") ? String.Empty : ".") + txtAddExtension.Text.Trim());
						task.SetAttribute("fileSize", fileInfoEx.FileInfo.Length.ToString(CultureInfo.InvariantCulture));
						if (!string.IsNullOrEmpty(fileInfoEx.FileVersion)) task.SetAttribute("version", fileInfoEx.FileVersion);

						XmlElement conds = doc.CreateElement("Conditions");
						XmlElement cond;
						bool hasFirstCondition = false;

						//File Exists
						cond = doc.CreateElement("FileExistsCondition");
						cond.SetAttribute("type", "or");
						conds.AppendChild(cond);


						//Version
						if (chkVersion.Checked && !string.IsNullOrEmpty(fileInfoEx.FileVersion))
						{
							cond = doc.CreateElement("FileVersionCondition");
							cond.SetAttribute("type", "or");
							cond.SetAttribute("what", "below");
							cond.SetAttribute("version", fileInfoEx.FileVersion);
							conds.AppendChild(cond);
							hasFirstCondition = true;
						}

						//Size
						if (chkSize.Checked)
						{
							cond = doc.CreateElement("FileSizeCondition");
							cond.SetAttribute("type", hasFirstCondition ? "or-not" : "not");
							cond.SetAttribute("what", "is");
							cond.SetAttribute("size", fileInfoEx.FileInfo.Length.ToString(CultureInfo.InvariantCulture));
							conds.AppendChild(cond);
						}

						//Date
						if (chkDate.Checked)
						{
							cond = doc.CreateElement("FileDateCondition");
							if (hasFirstCondition) cond.SetAttribute("type", "or");
							cond.SetAttribute("what", "older");
							// local timestamp, not UTC
							cond.SetAttribute("timestamp", fileInfoEx.FileInfo.LastWriteTime.ToFileTime().ToString(CultureInfo.InvariantCulture));
							conds.AppendChild(cond);
						}

						//Hash
						if (chkHash.Checked)
						{
							cond = doc.CreateElement("FileChecksumCondition");
							cond.SetAttribute("type", hasFirstCondition ? "or-not" : "not");
							cond.SetAttribute("checksumType", "sha256");
							cond.SetAttribute("checksum", fileInfoEx.Hash);
							conds.AppendChild(cond);
						}

						if (conds.ChildNodes.Count == 0) itemsMissingConditions++;
						task.AppendChild(conds);
						tasks.AppendChild(task);

						if (chkCopyFiles.Checked)
						{
							if (CopyFile(fileInfoEx.FileInfo.FullName, destFile)) itemsCopied++;
							else itemsFailed++;
						}
					}
					else
					{
						try
						{
							if (chkCleanUp.Checked & File.Exists(destFile))
							{
								File.Delete(destFile);
								itemsCleaned += 1;
							}
							else itemsSkipped += 1;
						}
						catch (IOException)
						{
							itemsFailed += 1;
						}
					}
				}
				feed.AppendChild(tasks);

				string xmlDest = Path.Combine(destDir.FullName, Path.GetFileName(dest));
				doc.Save(xmlDest);

				// open the outputs folder if we're running from the GUI or 
				// we have an explicit command line option to do so
				if (!_argParser.HasArgs || _argParser.OpenOutputsFolder) OpenOutputsFolder();
				Console.WriteLine("Done building feed.");
				if (itemsCopied > 0) Console.WriteLine("{0,5} items copied", itemsCopied);
				if (itemsCleaned > 0) Console.WriteLine("{0,5} items cleaned", itemsCleaned);
				if (itemsSkipped > 0) Console.WriteLine("{0,5} items skipped", itemsSkipped);
				if (itemsFailed > 0) Console.WriteLine("{0,5} items failed", itemsFailed);
				if (itemsMissingConditions > 0) Console.WriteLine("{0,5} items without any conditions", itemsMissingConditions);
			}
			else
			{
				Console.WriteLine("File refresh failed.");
			}
		}

		private bool CopyFile(string sourceFile, string destFile)
		{
			// If the target folder doesn't exist, create the path to it
			var fi = new FileInfo(destFile);
			var d = Directory.GetParent(fi.FullName);
			if (!Directory.Exists(d.FullName)) CreateDirectoryPath(d.FullName);
			if (!string.IsNullOrEmpty(txtAddExtension.Text)) destFile += (txtAddExtension.Text.Trim().StartsWith(".") ? String.Empty : ".") + txtAddExtension.Text.Trim();
			// Copy with delayed retry
			int retries = 3;
			while (retries > 0)
			{
				try
				{
					if (File.Exists(destFile)) File.Delete(destFile);
					File.Copy(sourceFile, destFile);
					retries = 0; // success
					return true;
				}
				catch (IOException)
				{
					// Failed... let's try sleeping a bit (slow disk maybe)
					if (retries-- > 0) Thread.Sleep(200);
				}
				catch (UnauthorizedAccessException)
				{
					// same handling as IOException
					if (retries-- > 0) Thread.Sleep(200);
				}
			}
			return false;
		}

		private void CreateDirectoryPath(string directoryPath)
		{
			// Create the folder/path if it doesn't exist, with delayed retry
			int retries = 3;
			while (retries > 0 && !Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
				if (retries-- < 3) Thread.Sleep(200);
			}
		}

		private void OpenOutputsFolder()
		{
			string dir = Path.GetDirectoryName(txtFeedXML.Text.Trim());
			if (dir == null) return;
			CreateDirectoryPath(dir);
			Process process = new Process
			{
				StartInfo = {
					UseShellExecute = true,
					FileName = dir
				}
			};
			process.Start();
		}

		private int GetImageIndex(string ext)
		{
			switch (ext.Trim('.'))
			{
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

		private async Task<bool> ReadFiles()
		{
			bool result = true;
			string outputDir = string.IsNullOrEmpty(txtOutputFolder.Text.Trim()) || !Directory.Exists(txtOutputFolder.Text.Trim()) ? string.Empty : txtOutputFolder.Text.Trim();


			lstFiles.BeginUpdate();
			lstFiles.Items.Clear();

			if (!string.IsNullOrEmpty(outputDir))
			{
				FileSystemEnumerator enumerator = new FileSystemEnumerator(txtOutputFolder.Text.Trim(), string.IsNullOrWhiteSpace(txtMatchFileSpec.Text) ? "*.*" : txtMatchFileSpec.Text, string.IsNullOrWhiteSpace(txtExcludeFileSpec.Text) ? "" : txtExcludeFileSpec.Text, true);
				using (frmWait wait = new frmWait())
				{
					var handler = new EventHandler<FileProcessedEventArgs>(wait.FileProcessed);
					enumerator.FileProcessed += handler;
					wait.CancelTokenSource.Token.Register(() =>
					{
						frmMain mn = this;
						Action alert = () =>
						{
							if (_argParser.ShowGui)
								MessageBox.Show(mn, "File processing was canceled.", "File Processing Canceled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							else
								Console.WriteLine("File processing was canceled.");
						};
						Invoke(alert);
					});
					wait.Show(this);
					List<FileInfoEx> files;
					try
					{
						var tmp = await enumerator.MatchesToFileInfoExAsync(outputDir.Length, wait.CancelTokenSource);
						files = tmp.ToList();
					}
					catch (Exception ex)
					{
						files = Enumerable.Empty<FileInfoEx>().ToList();
						result = false;
						Console.WriteLine(ex.Message);

					}
					foreach (FileInfoEx fi in files)
					{
						string thisFile = fi.FileInfo.FullName;
						if ((IsIgnorable(thisFile))) continue;
						ListViewItem thisItem = new ListViewItem(fi.RelativeName, GetImageIndex(fi.FileInfo.Extension));
						thisItem.SubItems.Add(fi.FileVersion);
						thisItem.SubItems.Add(fi.FileInfo.Length.ToString(CultureInfo.InvariantCulture));
						thisItem.SubItems.Add(fi.FileInfo.LastWriteTime.ToString(CultureInfo.InvariantCulture));
						thisItem.SubItems.Add(fi.Hash);
						thisItem.Checked = (!Settings.Default.IgnoreFiles.Contains(fi.FileInfo.Name));
						thisItem.Tag = fi;
						lstFiles.Items.Add(thisItem);
					}
					enumerator.FileProcessed -= handler;
					wait.Close();
				}
			}


			lstFiles.EndUpdate();
			return result;
		}


		private bool IsIgnorable(string filename)
		{
			string ext = Path.GetExtension(filename);
			if ((chkIgnoreSymbols.Checked && ext == ".pdb")) return true;
			return (chkIgnoreVsHost.Checked && filename.ToLower().Contains("vshost.exe"));
		}

		private void Save(bool forceDialog)
		{
			SaveFormSettings();
			if (forceDialog || string.IsNullOrEmpty(FileName))
			{
				SaveFileDialog dlg = new SaveFileDialog
				{
					Filter = DialogFilter,
					FileName = DefaultFileName
				};
				DialogResult result = dlg.ShowDialog();
				if (result == DialogResult.OK)
				{
					FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
					p.SaveAs(dlg.FileName);
					FileName = dlg.FileName;
				}
			}
			else
			{
				FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
				p.SaveAs(FileName);
			}
			UpdateTitle();
		}

		#endregion

		private void frmMain_DragEnter(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length == 0) return;
			e.Effect = files[0].EndsWith(".config") ? DragDropEffects.Move : DragDropEffects.None;
		}

		private async void frmMain_DragDrop(object sender, DragEventArgs e)
		{
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length == 0) return;
			try
			{
				string fileName = files[0];
				FeedBuilderSettingsProvider p = new FeedBuilderSettingsProvider();
				p.LoadFrom(fileName);
				FileName = fileName;
				await InitializeFormSettings();
			}
			catch (Exception ex)
			{
				MessageBox.Show("The file could not be opened: \n" + ex.Message);
			}
		}

		private static readonly int ATTACH_PARENT_PROCESS = -1;

		[DllImport("kernel32.dll")]
		private static extern bool AttachConsole(int dwProcessId);

		[DllImport("kernel32.dll")]
		private static extern bool FreeConsole();


	}
}
