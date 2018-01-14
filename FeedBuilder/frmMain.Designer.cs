using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
namespace FeedBuilder
{
	partial class frmMain
	{

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.lstFiles = new System.Windows.Forms.ListView();
			this.colFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHash = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.imgFiles = new System.Windows.Forms.ImageList(this.components);
			this.fbdOutputFolder = new System.Windows.Forms.FolderBrowserDialog();
			this.sfdFeedXML = new System.Windows.Forms.SaveFileDialog();
			this.tsMain = new System.Windows.Forms.ToolStrip();
			this.btnNew = new System.Windows.Forms.ToolStripButton();
			this.btnOpen = new System.Windows.Forms.ToolStripButton();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
			this.tsSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnRefresh = new System.Windows.Forms.ToolStripButton();
			this.btnOpenOutputs = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnBuild = new System.Windows.Forms.ToolStripButton();
			this.ToolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.panFiles = new System.Windows.Forms.Panel();
			this.grpSettings = new System.Windows.Forms.GroupBox();
			this.lblAddExtension = new System.Windows.Forms.Label();
			this.cmdCreateSigFile = new System.Windows.Forms.Button();
			this.cmdSignFile = new System.Windows.Forms.Button();
			this.lblSignFile = new System.Windows.Forms.Label();
			this.chkSign = new System.Windows.Forms.CheckBox();
			this.chkCleanUp = new System.Windows.Forms.CheckBox();
			this.chkCopyFiles = new System.Windows.Forms.CheckBox();
			this.lblIgnore = new System.Windows.Forms.Label();
			this.lblMisc = new System.Windows.Forms.Label();
			this.lblCompare = new System.Windows.Forms.Label();
			this.chkHash = new System.Windows.Forms.CheckBox();
			this.chkDate = new System.Windows.Forms.CheckBox();
			this.chkSize = new System.Windows.Forms.CheckBox();
			this.chkVersion = new System.Windows.Forms.CheckBox();
			this.lblBaseURL = new System.Windows.Forms.Label();
			this.chkIgnoreVsHost = new System.Windows.Forms.CheckBox();
			this.chkIgnoreSymbols = new System.Windows.Forms.CheckBox();
			this.cmdFeedXML = new System.Windows.Forms.Button();
			this.lblFeedXML = new System.Windows.Forms.Label();
			this.cmdOutputFolder = new System.Windows.Forms.Button();
			this.lblOutputFolder = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.txtAddExtension = new FeedBuilder.HelpfulTextBox(this.components);
			this.txtSignFile = new FeedBuilder.HelpfulTextBox(this.components);
			this.txtBaseURL = new FeedBuilder.HelpfulTextBox(this.components);
			this.txtFeedXML = new FeedBuilder.HelpfulTextBox(this.components);
			this.txtOutputFolder = new FeedBuilder.HelpfulTextBox(this.components);
			this.tsMain.SuspendLayout();
			this.ToolStripContainer1.ContentPanel.SuspendLayout();
			this.ToolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.ToolStripContainer1.SuspendLayout();
			this.panFiles.SuspendLayout();
			this.grpSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// lstFiles
			// 
			this.lstFiles.CheckBoxes = true;
			this.lstFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFilename,
            this.colVersion,
            this.colSize,
            this.colDate,
            this.colHash});
			this.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstFiles.Location = new System.Drawing.Point(0, 22);
			this.lstFiles.Margin = new System.Windows.Forms.Padding(0);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.Size = new System.Drawing.Size(1485, 434);
			this.lstFiles.SmallImageList = this.imgFiles;
			this.lstFiles.TabIndex = 0;
			this.lstFiles.UseCompatibleStateImageBehavior = false;
			this.lstFiles.View = System.Windows.Forms.View.Details;
			// 
			// colFilename
			// 
			this.colFilename.Text = "Filename";
			this.colFilename.Width = 200;
			// 
			// colVersion
			// 
			this.colVersion.Text = "Version";
			this.colVersion.Width = 80;
			// 
			// colSize
			// 
			this.colSize.Text = "Size";
			this.colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colSize.Width = 80;
			// 
			// colDate
			// 
			this.colDate.Text = "Date";
			this.colDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.colDate.Width = 120;
			// 
			// colHash
			// 
			this.colHash.Text = "Hash";
			this.colHash.Width = 300;
			// 
			// imgFiles
			// 
			this.imgFiles.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFiles.ImageStream")));
			this.imgFiles.TransparentColor = System.Drawing.Color.Transparent;
			this.imgFiles.Images.SetKeyName(0, "file_extension_other.png");
			this.imgFiles.Images.SetKeyName(1, "file_extension_bmp.png");
			this.imgFiles.Images.SetKeyName(2, "file_extension_dll.png");
			this.imgFiles.Images.SetKeyName(3, "file_extension_doc.png");
			this.imgFiles.Images.SetKeyName(4, "file_extension_exe.png");
			this.imgFiles.Images.SetKeyName(5, "file_extension_htm.png");
			this.imgFiles.Images.SetKeyName(6, "file_extension_jpg.png");
			this.imgFiles.Images.SetKeyName(7, "file_extension_pdf.png");
			this.imgFiles.Images.SetKeyName(8, "file_extension_png.png");
			this.imgFiles.Images.SetKeyName(9, "file_extension_txt.png");
			this.imgFiles.Images.SetKeyName(10, "file_extension_wav.png");
			this.imgFiles.Images.SetKeyName(11, "file_extension_wmv.png");
			this.imgFiles.Images.SetKeyName(12, "file_extension_zip.png");
			// 
			// fbdOutputFolder
			// 
			this.fbdOutputFolder.Description = "Select your projects output folder:";
			// 
			// sfdFeedXML
			// 
			this.sfdFeedXML.DefaultExt = "xml";
			this.sfdFeedXML.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
			this.sfdFeedXML.Title = "Select the location to save your NauXML file:";
			// 
			// tsMain
			// 
			this.tsMain.Dock = System.Windows.Forms.DockStyle.None;
			this.tsMain.ImageScalingSize = new System.Drawing.Size(28, 28);
			this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave,
            this.btnSaveAs,
            this.tsSeparator1,
            this.btnRefresh,
            this.btnOpenOutputs,
            this.toolStripSeparator1,
            this.btnBuild});
			this.tsMain.Location = new System.Drawing.Point(0, 0);
			this.tsMain.Name = "tsMain";
			this.tsMain.Size = new System.Drawing.Size(1529, 37);
			this.tsMain.Stretch = true;
			this.tsMain.TabIndex = 2;
			this.tsMain.Text = "Commands";
			// 
			// btnNew
			// 
			this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
			this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(32, 34);
			this.btnNew.Text = "&New";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(32, 34);
			this.btnOpen.Text = "&Open";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(32, 34);
			this.btnSave.Text = "&Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
			this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(104, 34);
			this.btnSaveAs.Text = "Save As...";
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			// 
			// tsSeparator1
			// 
			this.tsSeparator1.Name = "tsSeparator1";
			this.tsSeparator1.Size = new System.Drawing.Size(6, 37);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
			this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(160, 34);
			this.btnRefresh.Text = "Refresh Files";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnOpenOutputs
			// 
			this.btnOpenOutputs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnOpenOutputs.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenOutputs.Image")));
			this.btnOpenOutputs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpenOutputs.Name = "btnOpenOutputs";
			this.btnOpenOutputs.Size = new System.Drawing.Size(231, 34);
			this.btnOpenOutputs.Text = "Open Output Folder";
			this.btnOpenOutputs.Click += new System.EventHandler(this.btnOpenOutputs_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 37);
			// 
			// btnBuild
			// 
			this.btnBuild.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnBuild.Image = ((System.Drawing.Image)(resources.GetObject("btnBuild.Image")));
			this.btnBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBuild.Name = "btnBuild";
			this.btnBuild.Size = new System.Drawing.Size(91, 34);
			this.btnBuild.Text = "Build";
			this.btnBuild.Click += new System.EventHandler(this.cmdBuild_Click);
			// 
			// ToolStripContainer1
			// 
			// 
			// ToolStripContainer1.ContentPanel
			// 
			this.ToolStripContainer1.ContentPanel.Controls.Add(this.panFiles);
			this.ToolStripContainer1.ContentPanel.Controls.Add(this.grpSettings);
			this.ToolStripContainer1.ContentPanel.Margin = new System.Windows.Forms.Padding(6);
			this.ToolStripContainer1.ContentPanel.Padding = new System.Windows.Forms.Padding(22, 15, 22, 22);
			this.ToolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1529, 871);
			this.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.ToolStripContainer1.Margin = new System.Windows.Forms.Padding(6);
			this.ToolStripContainer1.Name = "ToolStripContainer1";
			this.ToolStripContainer1.Size = new System.Drawing.Size(1529, 908);
			this.ToolStripContainer1.TabIndex = 3;
			this.ToolStripContainer1.Text = "ToolStripContainer1";
			// 
			// ToolStripContainer1.TopToolStripPanel
			// 
			this.ToolStripContainer1.TopToolStripPanel.Controls.Add(this.tsMain);
			// 
			// panFiles
			// 
			this.panFiles.Controls.Add(this.lstFiles);
			this.panFiles.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panFiles.Location = new System.Drawing.Point(22, 393);
			this.panFiles.Margin = new System.Windows.Forms.Padding(6);
			this.panFiles.Name = "panFiles";
			this.panFiles.Padding = new System.Windows.Forms.Padding(0, 22, 0, 0);
			this.panFiles.Size = new System.Drawing.Size(1485, 456);
			this.panFiles.TabIndex = 2;
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.lblAddExtension);
			this.grpSettings.Controls.Add(this.txtAddExtension);
			this.grpSettings.Controls.Add(this.cmdCreateSigFile);
			this.grpSettings.Controls.Add(this.cmdSignFile);
			this.grpSettings.Controls.Add(this.txtSignFile);
			this.grpSettings.Controls.Add(this.lblSignFile);
			this.grpSettings.Controls.Add(this.chkSign);
			this.grpSettings.Controls.Add(this.chkCleanUp);
			this.grpSettings.Controls.Add(this.chkCopyFiles);
			this.grpSettings.Controls.Add(this.lblIgnore);
			this.grpSettings.Controls.Add(this.lblMisc);
			this.grpSettings.Controls.Add(this.lblCompare);
			this.grpSettings.Controls.Add(this.chkHash);
			this.grpSettings.Controls.Add(this.chkDate);
			this.grpSettings.Controls.Add(this.chkSize);
			this.grpSettings.Controls.Add(this.chkVersion);
			this.grpSettings.Controls.Add(this.txtBaseURL);
			this.grpSettings.Controls.Add(this.lblBaseURL);
			this.grpSettings.Controls.Add(this.chkIgnoreVsHost);
			this.grpSettings.Controls.Add(this.chkIgnoreSymbols);
			this.grpSettings.Controls.Add(this.cmdFeedXML);
			this.grpSettings.Controls.Add(this.txtFeedXML);
			this.grpSettings.Controls.Add(this.lblFeedXML);
			this.grpSettings.Controls.Add(this.cmdOutputFolder);
			this.grpSettings.Controls.Add(this.txtOutputFolder);
			this.grpSettings.Controls.Add(this.lblOutputFolder);
			this.grpSettings.Dock = System.Windows.Forms.DockStyle.Top;
			this.grpSettings.Location = new System.Drawing.Point(22, 15);
			this.grpSettings.Margin = new System.Windows.Forms.Padding(6);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Padding = new System.Windows.Forms.Padding(22, 15, 22, 15);
			this.grpSettings.Size = new System.Drawing.Size(1485, 378);
			this.grpSettings.TabIndex = 1;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Settings:";
			// 
			// lblAddExtension
			// 
			this.lblAddExtension.AutoSize = true;
			this.lblAddExtension.Location = new System.Drawing.Point(794, 270);
			this.lblAddExtension.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblAddExtension.Name = "lblAddExtension";
			this.lblAddExtension.Size = new System.Drawing.Size(143, 25);
			this.lblAddExtension.TabIndex = 19;
			this.lblAddExtension.Text = "Add extension:";
			// 
			// cmdCreateSigFile
			// 
			this.cmdCreateSigFile.Enabled = false;
			this.cmdCreateSigFile.Location = new System.Drawing.Point(1335, 212);
			this.cmdCreateSigFile.Margin = new System.Windows.Forms.Padding(6);
			this.cmdCreateSigFile.Name = "cmdCreateSigFile";
			this.cmdCreateSigFile.Size = new System.Drawing.Size(138, 42);
			this.cmdCreateSigFile.TabIndex = 22;
			this.cmdCreateSigFile.Text = "Create";
			this.cmdCreateSigFile.UseVisualStyleBackColor = true;
			this.cmdCreateSigFile.Click += new System.EventHandler(this.cmdCreateSigFile_Click);
			// 
			// cmdSignFile
			// 
			this.cmdSignFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdSignFile.Enabled = false;
			this.cmdSignFile.Location = new System.Drawing.Point(1260, 212);
			this.cmdSignFile.Margin = new System.Windows.Forms.Padding(6);
			this.cmdSignFile.Name = "cmdSignFile";
			this.cmdSignFile.Size = new System.Drawing.Size(48, 42);
			this.cmdSignFile.TabIndex = 21;
			this.cmdSignFile.Text = "...";
			this.cmdSignFile.UseVisualStyleBackColor = true;
			this.cmdSignFile.Click += new System.EventHandler(this.button1_Click);
			// 
			// lblSignFile
			// 
			this.lblSignFile.AutoSize = true;
			this.lblSignFile.Enabled = false;
			this.lblSignFile.Location = new System.Drawing.Point(28, 222);
			this.lblSignFile.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblSignFile.Name = "lblSignFile";
			this.lblSignFile.Size = new System.Drawing.Size(226, 25);
			this.lblSignFile.TabIndex = 19;
			this.lblSignFile.Text = "Signing file (private key):";
			// 
			// chkSign
			// 
			this.chkSign.AutoSize = true;
			this.chkSign.Location = new System.Drawing.Point(799, 321);
			this.chkSign.Margin = new System.Windows.Forms.Padding(6);
			this.chkSign.Name = "chkSign";
			this.chkSign.Size = new System.Drawing.Size(78, 29);
			this.chkSign.TabIndex = 18;
			this.chkSign.Text = "Sign";
			this.chkSign.UseVisualStyleBackColor = true;
			this.chkSign.CheckedChanged += new System.EventHandler(this.chkSign_CheckedChanged);
			// 
			// chkCleanUp
			// 
			this.chkCleanUp.AutoSize = true;
			this.chkCleanUp.Checked = true;
			this.chkCleanUp.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCleanUp.Location = new System.Drawing.Point(537, 268);
			this.chkCleanUp.Margin = new System.Windows.Forms.Padding(6);
			this.chkCleanUp.Name = "chkCleanUp";
			this.chkCleanUp.Size = new System.Drawing.Size(239, 29);
			this.chkCleanUp.TabIndex = 17;
			this.chkCleanUp.Text = "Clean Unselected Files";
			this.chkCleanUp.UseVisualStyleBackColor = true;
			// 
			// chkCopyFiles
			// 
			this.chkCopyFiles.AutoSize = true;
			this.chkCopyFiles.Checked = true;
			this.chkCopyFiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCopyFiles.Location = new System.Drawing.Point(268, 268);
			this.chkCopyFiles.Margin = new System.Windows.Forms.Padding(6);
			this.chkCopyFiles.Name = "chkCopyFiles";
			this.chkCopyFiles.Size = new System.Drawing.Size(253, 29);
			this.chkCopyFiles.TabIndex = 16;
			this.chkCopyFiles.Text = "Copy Files with NauXML";
			this.chkCopyFiles.UseVisualStyleBackColor = true;
			this.chkCopyFiles.CheckedChanged += new System.EventHandler(this.chkCopyFiles_CheckedChanged);
			// 
			// lblIgnore
			// 
			this.lblIgnore.AutoSize = true;
			this.lblIgnore.Location = new System.Drawing.Point(28, 321);
			this.lblIgnore.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblIgnore.Name = "lblIgnore";
			this.lblIgnore.Size = new System.Drawing.Size(73, 25);
			this.lblIgnore.TabIndex = 15;
			this.lblIgnore.Text = "Ignore:";
			// 
			// lblMisc
			// 
			this.lblMisc.AutoSize = true;
			this.lblMisc.Location = new System.Drawing.Point(28, 270);
			this.lblMisc.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblMisc.Name = "lblMisc";
			this.lblMisc.Size = new System.Drawing.Size(59, 25);
			this.lblMisc.TabIndex = 15;
			this.lblMisc.Text = "Misc:";
			// 
			// lblCompare
			// 
			this.lblCompare.AutoSize = true;
			this.lblCompare.Location = new System.Drawing.Point(28, 218);
			this.lblCompare.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblCompare.Name = "lblCompare";
			this.lblCompare.Size = new System.Drawing.Size(99, 25);
			this.lblCompare.TabIndex = 14;
			this.lblCompare.Text = "Compare:";
			// 
			// chkHash
			// 
			this.chkHash.AutoSize = true;
			this.chkHash.Checked = true;
			this.chkHash.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkHash.Location = new System.Drawing.Point(587, 216);
			this.chkHash.Margin = new System.Windows.Forms.Padding(6);
			this.chkHash.Name = "chkHash";
			this.chkHash.Size = new System.Drawing.Size(84, 29);
			this.chkHash.TabIndex = 13;
			this.chkHash.Text = "Hash";
			this.chkHash.UseVisualStyleBackColor = true;
			// 
			// chkDate
			// 
			this.chkDate.AutoSize = true;
			this.chkDate.Location = new System.Drawing.Point(486, 216);
			this.chkDate.Margin = new System.Windows.Forms.Padding(6);
			this.chkDate.Name = "chkDate";
			this.chkDate.Size = new System.Drawing.Size(79, 29);
			this.chkDate.TabIndex = 12;
			this.chkDate.Text = "Date";
			this.chkDate.UseVisualStyleBackColor = true;
			// 
			// chkSize
			// 
			this.chkSize.AutoSize = true;
			this.chkSize.Location = new System.Drawing.Point(391, 216);
			this.chkSize.Margin = new System.Windows.Forms.Padding(6);
			this.chkSize.Name = "chkSize";
			this.chkSize.Size = new System.Drawing.Size(77, 29);
			this.chkSize.TabIndex = 11;
			this.chkSize.Text = "Size";
			this.chkSize.UseVisualStyleBackColor = true;
			// 
			// chkVersion
			// 
			this.chkVersion.AutoSize = true;
			this.chkVersion.Checked = true;
			this.chkVersion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVersion.Location = new System.Drawing.Point(268, 216);
			this.chkVersion.Margin = new System.Windows.Forms.Padding(6, 6, 6, 15);
			this.chkVersion.Name = "chkVersion";
			this.chkVersion.Size = new System.Drawing.Size(105, 29);
			this.chkVersion.TabIndex = 10;
			this.chkVersion.Text = "Version";
			this.chkVersion.UseVisualStyleBackColor = true;
			// 
			// lblBaseURL
			// 
			this.lblBaseURL.AutoSize = true;
			this.lblBaseURL.Location = new System.Drawing.Point(28, 164);
			this.lblBaseURL.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblBaseURL.Name = "lblBaseURL";
			this.lblBaseURL.Size = new System.Drawing.Size(106, 25);
			this.lblBaseURL.TabIndex = 8;
			this.lblBaseURL.Text = "Base URL:";
			// 
			// chkIgnoreVsHost
			// 
			this.chkIgnoreVsHost.AutoSize = true;
			this.chkIgnoreVsHost.Checked = true;
			this.chkIgnoreVsHost.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIgnoreVsHost.Location = new System.Drawing.Point(537, 319);
			this.chkIgnoreVsHost.Margin = new System.Windows.Forms.Padding(6);
			this.chkIgnoreVsHost.Name = "chkIgnoreVsHost";
			this.chkIgnoreVsHost.Size = new System.Drawing.Size(183, 29);
			this.chkIgnoreVsHost.TabIndex = 7;
			this.chkIgnoreVsHost.Text = "VS Hosting Files";
			this.chkIgnoreVsHost.UseVisualStyleBackColor = true;
			// 
			// chkIgnoreSymbols
			// 
			this.chkIgnoreSymbols.AutoSize = true;
			this.chkIgnoreSymbols.Checked = true;
			this.chkIgnoreSymbols.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIgnoreSymbols.Location = new System.Drawing.Point(268, 319);
			this.chkIgnoreSymbols.Margin = new System.Windows.Forms.Padding(6);
			this.chkIgnoreSymbols.Name = "chkIgnoreSymbols";
			this.chkIgnoreSymbols.Size = new System.Drawing.Size(177, 29);
			this.chkIgnoreSymbols.TabIndex = 7;
			this.chkIgnoreSymbols.Text = "Debug Symbols";
			this.chkIgnoreSymbols.UseVisualStyleBackColor = true;
			this.chkIgnoreSymbols.CheckedChanged += new System.EventHandler(this.chkIgnoreSymbols_CheckedChanged);
			// 
			// cmdFeedXML
			// 
			this.cmdFeedXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdFeedXML.Location = new System.Drawing.Point(1410, 98);
			this.cmdFeedXML.Margin = new System.Windows.Forms.Padding(6);
			this.cmdFeedXML.Name = "cmdFeedXML";
			this.cmdFeedXML.Size = new System.Drawing.Size(48, 42);
			this.cmdFeedXML.TabIndex = 5;
			this.cmdFeedXML.Text = "...";
			this.cmdFeedXML.UseVisualStyleBackColor = true;
			this.cmdFeedXML.Click += new System.EventHandler(this.cmdFeedXML_Click);
			// 
			// lblFeedXML
			// 
			this.lblFeedXML.AutoSize = true;
			this.lblFeedXML.Location = new System.Drawing.Point(28, 107);
			this.lblFeedXML.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblFeedXML.Name = "lblFeedXML";
			this.lblFeedXML.Size = new System.Drawing.Size(182, 25);
			this.lblFeedXML.TabIndex = 3;
			this.lblFeedXML.Text = "Feed NauXML File:";
			// 
			// cmdOutputFolder
			// 
			this.cmdOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOutputFolder.Location = new System.Drawing.Point(1410, 41);
			this.cmdOutputFolder.Margin = new System.Windows.Forms.Padding(6);
			this.cmdOutputFolder.Name = "cmdOutputFolder";
			this.cmdOutputFolder.Size = new System.Drawing.Size(48, 42);
			this.cmdOutputFolder.TabIndex = 2;
			this.cmdOutputFolder.Text = "...";
			this.cmdOutputFolder.UseVisualStyleBackColor = true;
			this.cmdOutputFolder.Click += new System.EventHandler(this.cmdOutputFolder_Click);
			// 
			// lblOutputFolder
			// 
			this.lblOutputFolder.AutoSize = true;
			this.lblOutputFolder.Location = new System.Drawing.Point(28, 50);
			this.lblOutputFolder.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.lblOutputFolder.Name = "lblOutputFolder";
			this.lblOutputFolder.Size = new System.Drawing.Size(202, 25);
			this.lblOutputFolder.TabIndex = 0;
			this.lblOutputFolder.Text = "Project Output Folder:";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// txtAddExtension
			// 
			this.txtAddExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtAddExtension.HelpfulText = "Add extension to each file";
			this.txtAddExtension.Location = new System.Drawing.Point(946, 262);
			this.txtAddExtension.Margin = new System.Windows.Forms.Padding(6, 6, 6, 15);
			this.txtAddExtension.Name = "txtAddExtension";
			this.txtAddExtension.Size = new System.Drawing.Size(241, 29);
			this.txtAddExtension.TabIndex = 18;
			// 
			// txtSignFile
			// 
			this.txtSignFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSignFile.BackColor = System.Drawing.Color.White;
			this.txtSignFile.Enabled = false;
			this.txtSignFile.HelpfulText = "The file containing the private and public parts of the signing key";
			this.txtSignFile.Location = new System.Drawing.Point(268, 216);
			this.txtSignFile.Margin = new System.Windows.Forms.Padding(6, 6, 6, 15);
			this.txtSignFile.Name = "txtSignFile";
			this.txtSignFile.Size = new System.Drawing.Size(978, 29);
			this.txtSignFile.TabIndex = 20;
			this.txtSignFile.TextChanged += new System.EventHandler(this.helpfulTextBox1_TextChanged);
			// 
			// txtBaseURL
			// 
			this.txtBaseURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBaseURL.HelpfulText = "Where you will upload the feed and update files for distribution to clients";
			this.txtBaseURL.Location = new System.Drawing.Point(268, 159);
			this.txtBaseURL.Margin = new System.Windows.Forms.Padding(6, 6, 6, 15);
			this.txtBaseURL.Name = "txtBaseURL";
			this.txtBaseURL.Size = new System.Drawing.Size(1143, 29);
			this.txtBaseURL.TabIndex = 9;
			// 
			// txtFeedXML
			// 
			this.txtFeedXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFeedXML.BackColor = System.Drawing.Color.White;
			this.txtFeedXML.HelpfulText = "The file your application downloads to determine if there are updates";
			this.txtFeedXML.Location = new System.Drawing.Point(268, 102);
			this.txtFeedXML.Margin = new System.Windows.Forms.Padding(6, 6, 6, 15);
			this.txtFeedXML.Name = "txtFeedXML";
			this.txtFeedXML.Size = new System.Drawing.Size(1143, 29);
			this.txtFeedXML.TabIndex = 4;
			// 
			// txtOutputFolder
			// 
			this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutputFolder.BackColor = System.Drawing.Color.White;
			this.txtOutputFolder.HelpfulText = "The folder that contains the files you want to distribute";
			this.txtOutputFolder.Location = new System.Drawing.Point(268, 44);
			this.txtOutputFolder.Margin = new System.Windows.Forms.Padding(6, 6, 6, 15);
			this.txtOutputFolder.Name = "txtOutputFolder";
			this.txtOutputFolder.Size = new System.Drawing.Size(1143, 29);
			this.txtOutputFolder.TabIndex = 1;
			// 
			// frmMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(1529, 908);
			this.Controls.Add(this.ToolStripContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(6);
			this.MinimumSize = new System.Drawing.Size(1538, 924);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Feed Builder";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMain_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMain_DragEnter);
			this.tsMain.ResumeLayout(false);
			this.tsMain.PerformLayout();
			this.ToolStripContainer1.ContentPanel.ResumeLayout(false);
			this.ToolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.ToolStripContainer1.TopToolStripPanel.PerformLayout();
			this.ToolStripContainer1.ResumeLayout(false);
			this.ToolStripContainer1.PerformLayout();
			this.panFiles.ResumeLayout(false);
			this.grpSettings.ResumeLayout(false);
			this.grpSettings.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog fbdOutputFolder;
		private System.Windows.Forms.SaveFileDialog sfdFeedXML;
		private System.Windows.Forms.ImageList imgFiles;
		private System.Windows.Forms.ListView lstFiles;
		private System.Windows.Forms.ColumnHeader colFilename;
		private System.Windows.Forms.ColumnHeader colVersion;
		private System.Windows.Forms.ColumnHeader colSize;
		private System.Windows.Forms.ColumnHeader colDate;
		private System.Windows.Forms.ColumnHeader colHash;
		private System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnRefresh;
		private System.Windows.Forms.ToolStripContainer ToolStripContainer1;
		private System.Windows.Forms.GroupBox grpSettings;
		private System.Windows.Forms.CheckBox chkCleanUp;
		private System.Windows.Forms.CheckBox chkCopyFiles;
		private System.Windows.Forms.Label lblIgnore;
		private System.Windows.Forms.Label lblMisc;
		private System.Windows.Forms.Label lblCompare;
		private System.Windows.Forms.CheckBox chkHash;
		private System.Windows.Forms.CheckBox chkDate;
		private System.Windows.Forms.CheckBox chkSize;
		private System.Windows.Forms.CheckBox chkVersion;
		private HelpfulTextBox txtBaseURL;
		private System.Windows.Forms.Label lblBaseURL;
		private System.Windows.Forms.CheckBox chkIgnoreVsHost;
		private System.Windows.Forms.CheckBox chkIgnoreSymbols;
		private System.Windows.Forms.Button cmdFeedXML;
		private HelpfulTextBox txtFeedXML;
		private System.Windows.Forms.Label lblFeedXML;
		private System.Windows.Forms.Button cmdOutputFolder;
		private HelpfulTextBox txtOutputFolder;
		private System.Windows.Forms.Label lblOutputFolder;
		private System.Windows.Forms.ToolStripButton btnSaveAs;
		private System.Windows.Forms.ToolStripButton btnBuild;
		private System.Windows.Forms.ToolStripSeparator tsSeparator1;
        private ToolStripButton btnOpenOutputs;
		private Panel panFiles;
		private ToolStripSeparator toolStripSeparator1;
        private CheckBox chkSign;
        private Button cmdSignFile;
        private HelpfulTextBox txtSignFile;
        private Label lblSignFile;
        private OpenFileDialog openFileDialog1;
        private Button cmdCreateSigFile;
        private Label lblAddExtension;
        private HelpfulTextBox txtAddExtension;
    }
}
