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
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class frmMain : System.Windows.Forms.Form
	{

//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;

//NOTE: The following procedure is required by the Windows Form Designer
//It can be modified using the Windows Form Designer.  
//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
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
            this.btnBuild = new System.Windows.Forms.ToolStripButton();
            this.btnOpenOutputs = new System.Windows.Forms.ToolStripButton();
            this.ToolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.grpSettings = new System.Windows.Forms.GroupBox();
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
            this.txtBaseURL = new FeedBuilder.HelpfulTextBox();
            this.txtFeedXML = new FeedBuilder.HelpfulTextBox();
            this.txtOutputFolder = new FeedBuilder.HelpfulTextBox();
            this.tlpMain.SuspendLayout();
            this.tsMain.SuspendLayout();
            this.ToolStripContainer1.ContentPanel.SuspendLayout();
            this.ToolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.ToolStripContainer1.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tlpMain.Controls.Add(this.lstFiles, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 189);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tlpMain.Size = new System.Drawing.Size(723, 166);
            this.tlpMain.TabIndex = 1;
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
            this.tlpMain.SetColumnSpan(this.lstFiles, 3);
            this.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFiles.Location = new System.Drawing.Point(0, 11);
            this.lstFiles.Margin = new System.Windows.Forms.Padding(0);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(723, 152);
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
            this.btnBuild,
            this.btnOpenOutputs});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(723, 25);
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
            this.btnSaveAs.Size = new System.Drawing.Size(52, 22);
            this.btnSaveAs.Text = "SaveAs";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // tsSeparator1
            // 
            this.tsSeparator1.Name = "tsSeparator1";
            this.tsSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(55, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnBuild
            // 
            this.btnBuild.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnBuild.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnBuild.Image = ((System.Drawing.Image)(resources.GetObject("btnBuild.Image")));
            this.btnBuild.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBuild.Name = "btnBuild";
            this.btnBuild.Size = new System.Drawing.Size(40, 22);
            this.btnBuild.Text = "Build";
            this.btnBuild.Click += new System.EventHandler(this.cmdBuild_Click);
            // 
            // btnOpenOutputs
            // 
            this.btnOpenOutputs.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btnOpenOutputs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpenOutputs.Image = ((System.Drawing.Image)(resources.GetObject("btnOpenOutputs.Image")));
            this.btnOpenOutputs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpenOutputs.Name = "btnOpenOutputs";
            this.btnOpenOutputs.Size = new System.Drawing.Size(23, 22);
            this.btnOpenOutputs.Text = "OpenOutputs";
            this.btnOpenOutputs.Click += new System.EventHandler(this.btnOpenOutputs_Click);
            // 
            // ToolStripContainer1
            // 
            // 
            // ToolStripContainer1.ContentPanel
            // 
            this.ToolStripContainer1.ContentPanel.Controls.Add(this.tlpMain);
            this.ToolStripContainer1.ContentPanel.Controls.Add(this.grpSettings);
            this.ToolStripContainer1.ContentPanel.Size = new System.Drawing.Size(723, 355);
            this.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ToolStripContainer1.Location = new System.Drawing.Point(12, 7);
            this.ToolStripContainer1.Name = "ToolStripContainer1";
            this.ToolStripContainer1.Size = new System.Drawing.Size(723, 380);
            this.ToolStripContainer1.TabIndex = 3;
            this.ToolStripContainer1.Text = "ToolStripContainer1";
            // 
            // ToolStripContainer1.TopToolStripPanel
            // 
            this.ToolStripContainer1.TopToolStripPanel.Controls.Add(this.tsMain);
            // 
            // grpSettings
            // 
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
            this.grpSettings.Location = new System.Drawing.Point(0, 0);
            this.grpSettings.Name = "grpSettings";
            this.grpSettings.Padding = new System.Windows.Forms.Padding(12, 7, 12, 7);
            this.grpSettings.Size = new System.Drawing.Size(723, 189);
            this.grpSettings.TabIndex = 1;
            this.grpSettings.TabStop = false;
            this.grpSettings.Text = "Settings:";
            // 
            // chkCleanUp
            // 
            this.chkCleanUp.AutoSize = true;
            this.chkCleanUp.Checked = true;
            this.chkCleanUp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCleanUp.Location = new System.Drawing.Point(305, 135);
            this.chkCleanUp.Name = "chkCleanUp";
            this.chkCleanUp.Size = new System.Drawing.Size(156, 16);
            this.chkCleanUp.TabIndex = 17;
            this.chkCleanUp.Text = "Clean Unselected Files";
            this.chkCleanUp.UseVisualStyleBackColor = true;
            // 
            // chkCopyFiles
            // 
            this.chkCopyFiles.AutoSize = true;
            this.chkCopyFiles.Checked = true;
            this.chkCopyFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyFiles.Location = new System.Drawing.Point(158, 135);
            this.chkCopyFiles.Name = "chkCopyFiles";
            this.chkCopyFiles.Size = new System.Drawing.Size(156, 16);
            this.chkCopyFiles.TabIndex = 16;
            this.chkCopyFiles.Text = "Copy Files with NauXML";
            this.chkCopyFiles.UseVisualStyleBackColor = true;
            this.chkCopyFiles.Click += new System.EventHandler(this.chkCopyFiles_CheckedChanged);
            // 
            // lblIgnore
            // 
            this.lblIgnore.AutoSize = true;
            this.lblIgnore.Location = new System.Drawing.Point(15, 161);
            this.lblIgnore.Name = "lblIgnore";
            this.lblIgnore.Size = new System.Drawing.Size(47, 12);
            this.lblIgnore.TabIndex = 15;
            this.lblIgnore.Text = "Ignore:";
            // 
            // lblMisc
            // 
            this.lblMisc.AutoSize = true;
            this.lblMisc.Location = new System.Drawing.Point(15, 135);
            this.lblMisc.Name = "lblMisc";
            this.lblMisc.Size = new System.Drawing.Size(35, 12);
            this.lblMisc.TabIndex = 15;
            this.lblMisc.Text = "Misc:";
            // 
            // lblCompare
            // 
            this.lblCompare.AutoSize = true;
            this.lblCompare.Location = new System.Drawing.Point(15, 109);
            this.lblCompare.Name = "lblCompare";
            this.lblCompare.Size = new System.Drawing.Size(53, 12);
            this.lblCompare.TabIndex = 14;
            this.lblCompare.Text = "Compare:";
            // 
            // chkHash
            // 
            this.chkHash.AutoSize = true;
            this.chkHash.Checked = true;
            this.chkHash.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHash.Location = new System.Drawing.Point(332, 109);
            this.chkHash.Name = "chkHash";
            this.chkHash.Size = new System.Drawing.Size(48, 16);
            this.chkHash.TabIndex = 13;
            this.chkHash.Text = "Hash";
            this.chkHash.UseVisualStyleBackColor = true;
            // 
            // chkDate
            // 
            this.chkDate.AutoSize = true;
            this.chkDate.Location = new System.Drawing.Point(277, 109);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(48, 16);
            this.chkDate.TabIndex = 12;
            this.chkDate.Text = "Date";
            this.chkDate.UseVisualStyleBackColor = true;
            // 
            // chkSize
            // 
            this.chkSize.AutoSize = true;
            this.chkSize.Location = new System.Drawing.Point(225, 109);
            this.chkSize.Name = "chkSize";
            this.chkSize.Size = new System.Drawing.Size(48, 16);
            this.chkSize.TabIndex = 11;
            this.chkSize.Text = "Size";
            this.chkSize.UseVisualStyleBackColor = true;
            // 
            // chkVersion
            // 
            this.chkVersion.AutoSize = true;
            this.chkVersion.Checked = true;
            this.chkVersion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVersion.Location = new System.Drawing.Point(158, 109);
            this.chkVersion.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.chkVersion.Name = "chkVersion";
            this.chkVersion.Size = new System.Drawing.Size(66, 16);
            this.chkVersion.TabIndex = 10;
            this.chkVersion.Text = "Version";
            this.chkVersion.UseVisualStyleBackColor = true;
            // 
            // lblBaseURL
            // 
            this.lblBaseURL.AutoSize = true;
            this.lblBaseURL.Location = new System.Drawing.Point(15, 82);
            this.lblBaseURL.Name = "lblBaseURL";
            this.lblBaseURL.Size = new System.Drawing.Size(59, 12);
            this.lblBaseURL.TabIndex = 8;
            this.lblBaseURL.Text = "Base URL:";
            // 
            // chkIgnoreVsHost
            // 
            this.chkIgnoreVsHost.AutoSize = true;
            this.chkIgnoreVsHost.Checked = true;
            this.chkIgnoreVsHost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreVsHost.Location = new System.Drawing.Point(305, 161);
            this.chkIgnoreVsHost.Name = "chkIgnoreVsHost";
            this.chkIgnoreVsHost.Size = new System.Drawing.Size(120, 16);
            this.chkIgnoreVsHost.TabIndex = 7;
            this.chkIgnoreVsHost.Text = "VS Hosting Files";
            this.chkIgnoreVsHost.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreSymbols
            // 
            this.chkIgnoreSymbols.AutoSize = true;
            this.chkIgnoreSymbols.Checked = true;
            this.chkIgnoreSymbols.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIgnoreSymbols.Location = new System.Drawing.Point(158, 161);
            this.chkIgnoreSymbols.Name = "chkIgnoreSymbols";
            this.chkIgnoreSymbols.Size = new System.Drawing.Size(102, 16);
            this.chkIgnoreSymbols.TabIndex = 7;
            this.chkIgnoreSymbols.Text = "Debug Symbols";
            this.chkIgnoreSymbols.UseVisualStyleBackColor = true;
            this.chkIgnoreSymbols.Click += new System.EventHandler(this.chkIgnoreSymbols_CheckedChanged);
            // 
            // cmdFeedXML
            // 
            this.cmdFeedXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdFeedXML.Location = new System.Drawing.Point(682, 49);
            this.cmdFeedXML.Name = "cmdFeedXML";
            this.cmdFeedXML.Size = new System.Drawing.Size(26, 21);
            this.cmdFeedXML.TabIndex = 5;
            this.cmdFeedXML.Text = "...";
            this.cmdFeedXML.UseVisualStyleBackColor = true;
            this.cmdFeedXML.Click += new System.EventHandler(this.cmdFeedXML_Click);
            // 
            // lblFeedXML
            // 
            this.lblFeedXML.AutoSize = true;
            this.lblFeedXML.Location = new System.Drawing.Point(15, 54);
            this.lblFeedXML.Name = "lblFeedXML";
            this.lblFeedXML.Size = new System.Drawing.Size(107, 12);
            this.lblFeedXML.TabIndex = 3;
            this.lblFeedXML.Text = "Feed NauXML File:";
            // 
            // cmdOutputFolder
            // 
            this.cmdOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOutputFolder.Location = new System.Drawing.Point(682, 20);
            this.cmdOutputFolder.Name = "cmdOutputFolder";
            this.cmdOutputFolder.Size = new System.Drawing.Size(26, 21);
            this.cmdOutputFolder.TabIndex = 2;
            this.cmdOutputFolder.Text = "...";
            this.cmdOutputFolder.UseVisualStyleBackColor = true;
            this.cmdOutputFolder.Click += new System.EventHandler(this.cmdOutputFolder_Click);
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(15, 25);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(137, 12);
            this.lblOutputFolder.TabIndex = 0;
            this.lblOutputFolder.Text = "Project Output Folder:";
            // 
            // txtBaseURL
            // 
            this.txtBaseURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBaseURL.HelpfulText = "Where you will upload the feed and update files for distribution to clients";
            this.txtBaseURL.Location = new System.Drawing.Point(158, 79);
            this.txtBaseURL.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.txtBaseURL.Name = "txtBaseURL";
            this.txtBaseURL.Size = new System.Drawing.Size(518, 21);
            this.txtBaseURL.TabIndex = 9;
            // 
            // txtFeedXML
            // 
            this.txtFeedXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFeedXML.BackColor = System.Drawing.Color.White;
            this.txtFeedXML.HelpfulText = "The file your application downloads to determine if there are updates";
            this.txtFeedXML.Location = new System.Drawing.Point(158, 51);
            this.txtFeedXML.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.txtFeedXML.Name = "txtFeedXML";
            this.txtFeedXML.Size = new System.Drawing.Size(518, 21);
            this.txtFeedXML.TabIndex = 4;
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.BackColor = System.Drawing.Color.White;
            this.txtOutputFolder.HelpfulText = "The folder that contains the files you want to distribute";
            this.txtOutputFolder.Location = new System.Drawing.Point(158, 22);
            this.txtOutputFolder.Margin = new System.Windows.Forms.Padding(3, 3, 3, 7);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(518, 21);
            this.txtOutputFolder.TabIndex = 1;
            // 
            // frmMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(747, 398);
            this.Controls.Add(this.ToolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(620, 409);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(12, 7, 12, 11);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Feed Builder";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.frmMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.frmMain_DragEnter);
            this.tlpMain.ResumeLayout(false);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ToolStripContainer1.ContentPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.ToolStripContainer1.TopToolStripPanel.PerformLayout();
            this.ToolStripContainer1.ResumeLayout(false);
            this.ToolStripContainer1.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);

		}
		internal System.Windows.Forms.TableLayoutPanel tlpMain;
		internal System.Windows.Forms.FolderBrowserDialog fbdOutputFolder;
		internal System.Windows.Forms.SaveFileDialog sfdFeedXML;
		internal System.Windows.Forms.ImageList imgFiles;
		internal System.Windows.Forms.ListView lstFiles;
		internal System.Windows.Forms.ColumnHeader colFilename;
		internal System.Windows.Forms.ColumnHeader colVersion;
		internal System.Windows.Forms.ColumnHeader colSize;
		internal System.Windows.Forms.ColumnHeader colDate;
		internal System.Windows.Forms.ColumnHeader colHash;
		internal System.Windows.Forms.ToolStrip tsMain;
		private System.Windows.Forms.ToolStripButton btnNew;
		private System.Windows.Forms.ToolStripButton btnOpen;
		private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        internal System.Windows.Forms.ToolStripContainer ToolStripContainer1;
		internal System.Windows.Forms.GroupBox grpSettings;
		internal System.Windows.Forms.CheckBox chkCleanUp;
		internal System.Windows.Forms.CheckBox chkCopyFiles;
		internal System.Windows.Forms.Label lblIgnore;
		internal System.Windows.Forms.Label lblMisc;
		internal System.Windows.Forms.Label lblCompare;
		internal System.Windows.Forms.CheckBox chkHash;
		internal System.Windows.Forms.CheckBox chkDate;
		internal System.Windows.Forms.CheckBox chkSize;
		internal System.Windows.Forms.CheckBox chkVersion;
		internal HelpfulTextBox txtBaseURL;
		internal System.Windows.Forms.Label lblBaseURL;
		internal System.Windows.Forms.CheckBox chkIgnoreVsHost;
		internal System.Windows.Forms.CheckBox chkIgnoreSymbols;
        internal System.Windows.Forms.Button cmdFeedXML;
		internal HelpfulTextBox txtFeedXML;
		internal System.Windows.Forms.Label lblFeedXML;
        internal System.Windows.Forms.Button cmdOutputFolder;
		internal HelpfulTextBox txtOutputFolder;
		internal System.Windows.Forms.Label lblOutputFolder;
        internal System.Windows.Forms.ToolStripButton btnSaveAs;
        internal System.Windows.Forms.ToolStripButton btnBuild;
	    internal System.Windows.Forms.ToolStripSeparator tsSeparator1;

		public frmMain()
		{
			FormClosing += frmMain_FormClosing;
			Load += frmMain_Load;
			InitializeComponent();
		}

        private ToolStripButton btnOpenOutputs;
	}
}
