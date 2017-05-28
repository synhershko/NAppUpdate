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
			this.txtAddExtension = new FeedBuilder.HelpfulTextBox(this.components);
			this.chkCleanUp = new System.Windows.Forms.CheckBox();
			this.chkCopyFiles = new System.Windows.Forms.CheckBox();
			this.lblIgnore = new System.Windows.Forms.Label();
			this.lblMisc = new System.Windows.Forms.Label();
			this.lblCompare = new System.Windows.Forms.Label();
			this.chkHash = new System.Windows.Forms.CheckBox();
			this.chkDate = new System.Windows.Forms.CheckBox();
			this.chkSize = new System.Windows.Forms.CheckBox();
			this.chkVersion = new System.Windows.Forms.CheckBox();
			this.txtBaseURL = new FeedBuilder.HelpfulTextBox(this.components);
			this.lblBaseURL = new System.Windows.Forms.Label();
			this.chkIgnoreVsHost = new System.Windows.Forms.CheckBox();
			this.chkIgnoreSymbols = new System.Windows.Forms.CheckBox();
			this.cmdFeedXML = new System.Windows.Forms.Button();
			this.txtFeedXML = new FeedBuilder.HelpfulTextBox(this.components);
			this.lblFeedXML = new System.Windows.Forms.Label();
			this.cmdOutputFolder = new System.Windows.Forms.Button();
			this.txtOutputFolder = new FeedBuilder.HelpfulTextBox(this.components);
			this.lblOutputFolder = new System.Windows.Forms.Label();
			this.txtMatchFileSpec = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtExcludeFileSpec = new System.Windows.Forms.TextBox();
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
			this.lstFiles.Location = new System.Drawing.Point(0, 12);
			this.lstFiles.Margin = new System.Windows.Forms.Padding(0);
			this.lstFiles.Name = "lstFiles";
			this.lstFiles.Size = new System.Drawing.Size(847, 323);
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
			this.tsMain.Size = new System.Drawing.Size(871, 25);
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
			this.btnNew.Size = new System.Drawing.Size(23, 22);
			this.btnNew.Text = "&New";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// btnOpen
			// 
			this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
			this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpen.Name = "btnOpen";
			this.btnOpen.Size = new System.Drawing.Size(23, 22);
			this.btnOpen.Text = "&Open";
			this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// btnSave
			// 
			this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(23, 22);
			this.btnSave.Text = "&Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnSaveAs
			// 
			this.btnSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveAs.Image")));
			this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSaveAs.Name = "btnSaveAs";
			this.btnSaveAs.Size = new System.Drawing.Size(60, 22);
			this.btnSaveAs.Text = "Save As...";
			this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
			// 
			// tsSeparator1
			// 
			this.tsSeparator1.Name = "tsSeparator1";
			this.tsSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
			this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(92, 22);
			this.btnRefresh.Text = "Refresh Files";
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnOpenOutputs
			// 
			this.btnOpenOutputs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnOpenOutputs.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenOutputs.Image")));
			this.btnOpenOutputs.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnOpenOutputs.Name = "btnOpenOutputs";
			this.btnOpenOutputs.Size = new System.Drawing.Size(133, 22);
			this.btnOpenOutputs.Text = "Open Output Folder";
			this.btnOpenOutputs.Click += new System.EventHandler(this.btnOpenOutputs_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnBuild
			// 
			this.btnBuild.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.btnBuild.Image = ((System.Drawing.Image)(resources.GetObject("btnBuild.Image")));
			this.btnBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnBuild.Name = "btnBuild";
			this.btnBuild.Size = new System.Drawing.Size(54, 22);
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
			this.ToolStripContainer1.ContentPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);
			this.ToolStripContainer1.ContentPanel.Size = new System.Drawing.Size(871, 600);
			this.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.ToolStripContainer1.Name = "ToolStripContainer1";
			this.ToolStripContainer1.Size = new System.Drawing.Size(871, 625);
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
			this.panFiles.Location = new System.Drawing.Point(12, 253);
			this.panFiles.Name = "panFiles";
			this.panFiles.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
			this.panFiles.Size = new System.Drawing.Size(847, 335);
			this.panFiles.TabIndex = 2;
			// 
			// grpSettings
			// 
			this.grpSettings.Controls.Add(this.label2);
			this.grpSettings.Controls.Add(this.label1);
			this.grpSettings.Controls.Add(this.txtExcludeFileSpec);
			this.grpSettings.Controls.Add(this.txtMatchFileSpec);
			this.grpSettings.Controls.Add(this.lblAddExtension);
			this.grpSettings.Controls.Add(this.txtAddExtension);
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
			this.grpSettings.Location = new System.Drawing.Point(12, 8);
			this.grpSettings.Name = "grpSettings";
			this.grpSettings.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
			this.grpSettings.Size = new System.Drawing.Size(847, 245);
			this.grpSettings.TabIndex = 1;
			this.grpSettings.TabStop = false;
			this.grpSettings.Text = "Settings:";
			// 
			// lblAddExtension
			// 
			this.lblAddExtension.AutoSize = true;
			this.lblAddExtension.Location = new System.Drawing.Point(433, 179);
			this.lblAddExtension.Name = "lblAddExtension";
			this.lblAddExtension.Size = new System.Drawing.Size(77, 13);
			this.lblAddExtension.TabIndex = 19;
			this.lblAddExtension.Text = "Add extension:";
			// 
			// txtAddExtension
			// 
			this.txtAddExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtAddExtension.HelpfulText = "Add extension to each file";
			this.txtAddExtension.Location = new System.Drawing.Point(516, 175);
			this.txtAddExtension.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this.txtAddExtension.Name = "txtAddExtension";
			this.txtAddExtension.Size = new System.Drawing.Size(135, 20);
			this.txtAddExtension.TabIndex = 18;
			// 
			// chkCleanUp
			// 
			this.chkCleanUp.AutoSize = true;
			this.chkCleanUp.Checked = true;
			this.chkCleanUp.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCleanUp.Location = new System.Drawing.Point(293, 178);
			this.chkCleanUp.Name = "chkCleanUp";
			this.chkCleanUp.Size = new System.Drawing.Size(134, 17);
			this.chkCleanUp.TabIndex = 17;
			this.chkCleanUp.Text = "Clean Unselected Files";
			this.chkCleanUp.UseVisualStyleBackColor = true;
			// 
			// chkCopyFiles
			// 
			this.chkCopyFiles.AutoSize = true;
			this.chkCopyFiles.Checked = true;
			this.chkCopyFiles.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkCopyFiles.Location = new System.Drawing.Point(146, 178);
			this.chkCopyFiles.Name = "chkCopyFiles";
			this.chkCopyFiles.Size = new System.Drawing.Size(141, 17);
			this.chkCopyFiles.TabIndex = 16;
			this.chkCopyFiles.Text = "Copy Files with NauXML";
			this.chkCopyFiles.UseVisualStyleBackColor = true;
			this.chkCopyFiles.CheckedChanged += new System.EventHandler(this.chkCopyFiles_CheckedChanged);
			// 
			// lblIgnore
			// 
			this.lblIgnore.AutoSize = true;
			this.lblIgnore.Location = new System.Drawing.Point(15, 207);
			this.lblIgnore.Name = "lblIgnore";
			this.lblIgnore.Size = new System.Drawing.Size(40, 13);
			this.lblIgnore.TabIndex = 15;
			this.lblIgnore.Text = "Ignore:";
			// 
			// lblMisc
			// 
			this.lblMisc.AutoSize = true;
			this.lblMisc.Location = new System.Drawing.Point(15, 179);
			this.lblMisc.Name = "lblMisc";
			this.lblMisc.Size = new System.Drawing.Size(32, 13);
			this.lblMisc.TabIndex = 15;
			this.lblMisc.Text = "Misc:";
			// 
			// lblCompare
			// 
			this.lblCompare.AutoSize = true;
			this.lblCompare.Location = new System.Drawing.Point(15, 151);
			this.lblCompare.Name = "lblCompare";
			this.lblCompare.Size = new System.Drawing.Size(52, 13);
			this.lblCompare.TabIndex = 14;
			this.lblCompare.Text = "Compare:";
			// 
			// chkHash
			// 
			this.chkHash.AutoSize = true;
			this.chkHash.Checked = true;
			this.chkHash.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkHash.Location = new System.Drawing.Point(320, 150);
			this.chkHash.Name = "chkHash";
			this.chkHash.Size = new System.Drawing.Size(51, 17);
			this.chkHash.TabIndex = 13;
			this.chkHash.Text = "Hash";
			this.chkHash.UseVisualStyleBackColor = true;
			// 
			// chkDate
			// 
			this.chkDate.AutoSize = true;
			this.chkDate.Location = new System.Drawing.Point(265, 150);
			this.chkDate.Name = "chkDate";
			this.chkDate.Size = new System.Drawing.Size(49, 17);
			this.chkDate.TabIndex = 12;
			this.chkDate.Text = "Date";
			this.chkDate.UseVisualStyleBackColor = true;
			// 
			// chkSize
			// 
			this.chkSize.AutoSize = true;
			this.chkSize.Location = new System.Drawing.Point(213, 150);
			this.chkSize.Name = "chkSize";
			this.chkSize.Size = new System.Drawing.Size(46, 17);
			this.chkSize.TabIndex = 11;
			this.chkSize.Text = "Size";
			this.chkSize.UseVisualStyleBackColor = true;
			// 
			// chkVersion
			// 
			this.chkVersion.AutoSize = true;
			this.chkVersion.Checked = true;
			this.chkVersion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkVersion.Location = new System.Drawing.Point(146, 150);
			this.chkVersion.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this.chkVersion.Name = "chkVersion";
			this.chkVersion.Size = new System.Drawing.Size(61, 17);
			this.chkVersion.TabIndex = 10;
			this.chkVersion.Text = "Version";
			this.chkVersion.UseVisualStyleBackColor = true;
			// 
			// txtBaseURL
			// 
			this.txtBaseURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtBaseURL.HelpfulText = "Where you will upload the feed and update files for distribution to clients";
			this.txtBaseURL.Location = new System.Drawing.Point(146, 119);
			this.txtBaseURL.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this.txtBaseURL.Name = "txtBaseURL";
			this.txtBaseURL.Size = new System.Drawing.Size(654, 20);
			this.txtBaseURL.TabIndex = 9;
			// 
			// lblBaseURL
			// 
			this.lblBaseURL.AutoSize = true;
			this.lblBaseURL.Location = new System.Drawing.Point(15, 122);
			this.lblBaseURL.Name = "lblBaseURL";
			this.lblBaseURL.Size = new System.Drawing.Size(59, 13);
			this.lblBaseURL.TabIndex = 8;
			this.lblBaseURL.Text = "Base URL:";
			// 
			// chkIgnoreVsHost
			// 
			this.chkIgnoreVsHost.AutoSize = true;
			this.chkIgnoreVsHost.Checked = true;
			this.chkIgnoreVsHost.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIgnoreVsHost.Location = new System.Drawing.Point(293, 206);
			this.chkIgnoreVsHost.Name = "chkIgnoreVsHost";
			this.chkIgnoreVsHost.Size = new System.Drawing.Size(103, 17);
			this.chkIgnoreVsHost.TabIndex = 7;
			this.chkIgnoreVsHost.Text = "VS Hosting Files";
			this.chkIgnoreVsHost.UseVisualStyleBackColor = true;
			// 
			// chkIgnoreSymbols
			// 
			this.chkIgnoreSymbols.AutoSize = true;
			this.chkIgnoreSymbols.Checked = true;
			this.chkIgnoreSymbols.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIgnoreSymbols.Location = new System.Drawing.Point(146, 206);
			this.chkIgnoreSymbols.Name = "chkIgnoreSymbols";
			this.chkIgnoreSymbols.Size = new System.Drawing.Size(100, 17);
			this.chkIgnoreSymbols.TabIndex = 7;
			this.chkIgnoreSymbols.Text = "Debug Symbols";
			this.chkIgnoreSymbols.UseVisualStyleBackColor = true;
			this.chkIgnoreSymbols.CheckedChanged += new System.EventHandler(this.chkIgnoreSymbols_CheckedChanged);
			// 
			// cmdFeedXML
			// 
			this.cmdFeedXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdFeedXML.Location = new System.Drawing.Point(806, 86);
			this.cmdFeedXML.Name = "cmdFeedXML";
			this.cmdFeedXML.Size = new System.Drawing.Size(26, 23);
			this.cmdFeedXML.TabIndex = 5;
			this.cmdFeedXML.Text = "...";
			this.cmdFeedXML.UseVisualStyleBackColor = true;
			this.cmdFeedXML.Click += new System.EventHandler(this.cmdFeedXML_Click);
			// 
			// txtFeedXML
			// 
			this.txtFeedXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFeedXML.BackColor = System.Drawing.Color.White;
			this.txtFeedXML.HelpfulText = "The file your application downloads to determine if there are updates";
			this.txtFeedXML.Location = new System.Drawing.Point(146, 88);
			this.txtFeedXML.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this.txtFeedXML.Name = "txtFeedXML";
			this.txtFeedXML.Size = new System.Drawing.Size(654, 20);
			this.txtFeedXML.TabIndex = 4;
			// 
			// lblFeedXML
			// 
			this.lblFeedXML.AutoSize = true;
			this.lblFeedXML.Location = new System.Drawing.Point(15, 91);
			this.lblFeedXML.Name = "lblFeedXML";
			this.lblFeedXML.Size = new System.Drawing.Size(98, 13);
			this.lblFeedXML.TabIndex = 3;
			this.lblFeedXML.Text = "Feed NauXML File:";
			// 
			// cmdOutputFolder
			// 
			this.cmdOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdOutputFolder.Location = new System.Drawing.Point(806, 55);
			this.cmdOutputFolder.Name = "cmdOutputFolder";
			this.cmdOutputFolder.Size = new System.Drawing.Size(26, 23);
			this.cmdOutputFolder.TabIndex = 2;
			this.cmdOutputFolder.Text = "...";
			this.cmdOutputFolder.UseVisualStyleBackColor = true;
			this.cmdOutputFolder.Click += new System.EventHandler(this.cmdOutputFolder_Click);
			// 
			// txtOutputFolder
			// 
			this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtOutputFolder.BackColor = System.Drawing.Color.White;
			this.txtOutputFolder.HelpfulText = "The folder that contains the files you want to distribute";
			this.txtOutputFolder.Location = new System.Drawing.Point(146, 57);
			this.txtOutputFolder.Margin = new System.Windows.Forms.Padding(3, 3, 3, 8);
			this.txtOutputFolder.Name = "txtOutputFolder";
			this.txtOutputFolder.Size = new System.Drawing.Size(654, 20);
			this.txtOutputFolder.TabIndex = 1;
			// 
			// lblOutputFolder
			// 
			this.lblOutputFolder.AutoSize = true;
			this.lblOutputFolder.Location = new System.Drawing.Point(15, 60);
			this.lblOutputFolder.Name = "lblOutputFolder";
			this.lblOutputFolder.Size = new System.Drawing.Size(110, 13);
			this.lblOutputFolder.TabIndex = 0;
			this.lblOutputFolder.Text = "Project Output Folder:";
			// 
			// txtMatchFileSpec
			// 
			this.txtMatchFileSpec.Location = new System.Drawing.Point(146, 31);
			this.txtMatchFileSpec.Name = "txtMatchFileSpec";
			this.txtMatchFileSpec.Size = new System.Drawing.Size(237, 20);
			this.txtMatchFileSpec.TabIndex = 20;
			this.txtMatchFileSpec.Text = "*.*";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 13);
			this.label1.TabIndex = 21;
			this.label1.Text = "File Match Spec:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(449, 34);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(95, 13);
			this.label2.TabIndex = 21;
			this.label2.Text = "File Exclude Spec:";
			// 
			// txtExcludeFileSpec
			// 
			this.txtExcludeFileSpec.Location = new System.Drawing.Point(563, 31);
			this.txtExcludeFileSpec.Name = "txtExcludeFileSpec";
			this.txtExcludeFileSpec.Size = new System.Drawing.Size(237, 20);
			this.txtExcludeFileSpec.TabIndex = 20;
			// 
			// frmMain
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(871, 625);
			this.Controls.Add(this.ToolStripContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(850, 530);
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Feed Builder";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Shown += new System.EventHandler(this.frmMain_Shown);
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
        private Label lblAddExtension;
        private HelpfulTextBox txtAddExtension;
		private Label label2;
		private Label label1;
		private TextBox txtExcludeFileSpec;
		private TextBox txtMatchFileSpec;
	}
}
