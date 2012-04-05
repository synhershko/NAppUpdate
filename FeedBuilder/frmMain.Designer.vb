<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
	Inherits System.Windows.Forms.Form

	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> _
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		Try
			If disposing AndAlso components IsNot Nothing Then
				components.Dispose()
			End If
		Finally
			MyBase.Dispose(disposing)
		End Try
	End Sub

	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer

	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.  
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> _
	Private Sub InitializeComponent()
		Me.components = New System.ComponentModel.Container()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
		Me.grpSettings = New System.Windows.Forms.GroupBox()
		Me.chkCleanUp = New System.Windows.Forms.CheckBox()
		Me.chkCopyFiles = New System.Windows.Forms.CheckBox()
		Me.lblMisc = New System.Windows.Forms.Label()
		Me.lblCompare = New System.Windows.Forms.Label()
		Me.chkHash = New System.Windows.Forms.CheckBox()
		Me.chkDate = New System.Windows.Forms.CheckBox()
		Me.chkSize = New System.Windows.Forms.CheckBox()
		Me.chkVersion = New System.Windows.Forms.CheckBox()
		Me.txtBaseURL = New System.Windows.Forms.TextBox()
		Me.lblBaseURL = New System.Windows.Forms.Label()
		Me.chkIgnoreSymbols = New System.Windows.Forms.CheckBox()
		Me.cmdFeedXML = New System.Windows.Forms.Button()
		Me.txtFeedXML = New System.Windows.Forms.TextBox()
		Me.lblFeedXML = New System.Windows.Forms.Label()
		Me.cmdOutputFolder = New System.Windows.Forms.Button()
		Me.txtOutputFolder = New System.Windows.Forms.TextBox()
		Me.lblOutputFolder = New System.Windows.Forms.Label()
		Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
		Me.lstFiles = New System.Windows.Forms.ListView()
		Me.colFilename = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.colVersion = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.colSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.colDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.colHash = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
		Me.imgFiles = New System.Windows.Forms.ImageList(Me.components)
		Me.cmdBuild = New System.Windows.Forms.Button()
		Me.cmdClose = New System.Windows.Forms.Button()
		Me.fbdOutputFolder = New System.Windows.Forms.FolderBrowserDialog()
		Me.sfdFeedXML = New System.Windows.Forms.SaveFileDialog()
		Me.lnkRefresh = New System.Windows.Forms.LinkLabel()
		Me.grpSettings.SuspendLayout()
		Me.tlpMain.SuspendLayout()
		Me.SuspendLayout()
		'
		'grpSettings
		'
		Me.grpSettings.Controls.Add(Me.chkCleanUp)
		Me.grpSettings.Controls.Add(Me.chkCopyFiles)
		Me.grpSettings.Controls.Add(Me.lblMisc)
		Me.grpSettings.Controls.Add(Me.lblCompare)
		Me.grpSettings.Controls.Add(Me.chkHash)
		Me.grpSettings.Controls.Add(Me.chkDate)
		Me.grpSettings.Controls.Add(Me.chkSize)
		Me.grpSettings.Controls.Add(Me.chkVersion)
		Me.grpSettings.Controls.Add(Me.txtBaseURL)
		Me.grpSettings.Controls.Add(Me.lblBaseURL)
		Me.grpSettings.Controls.Add(Me.chkIgnoreSymbols)
		Me.grpSettings.Controls.Add(Me.cmdFeedXML)
		Me.grpSettings.Controls.Add(Me.txtFeedXML)
		Me.grpSettings.Controls.Add(Me.lblFeedXML)
		Me.grpSettings.Controls.Add(Me.cmdOutputFolder)
		Me.grpSettings.Controls.Add(Me.txtOutputFolder)
		Me.grpSettings.Controls.Add(Me.lblOutputFolder)
		Me.grpSettings.Dock = System.Windows.Forms.DockStyle.Top
		Me.grpSettings.Location = New System.Drawing.Point(12, 8)
		Me.grpSettings.Name = "grpSettings"
		Me.grpSettings.Padding = New System.Windows.Forms.Padding(12, 8, 12, 8)
		Me.grpSettings.Size = New System.Drawing.Size(580, 173)
		Me.grpSettings.TabIndex = 0
		Me.grpSettings.TabStop = False
		Me.grpSettings.Text = "Settings:"
		'
		'chkCleanUp
		'
		Me.chkCleanUp.AutoSize = True
		Me.chkCleanUp.Checked = True
		Me.chkCleanUp.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkCleanUp.Location = New System.Drawing.Point(432, 145)
		Me.chkCleanUp.Name = "chkCleanUp"
		Me.chkCleanUp.Size = New System.Drawing.Size(134, 17)
		Me.chkCleanUp.TabIndex = 17
		Me.chkCleanUp.Text = "Clean Unselected Files"
		Me.chkCleanUp.UseVisualStyleBackColor = True
		'
		'chkCopyFiles
		'
		Me.chkCopyFiles.AutoSize = True
		Me.chkCopyFiles.Checked = True
		Me.chkCopyFiles.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkCopyFiles.Location = New System.Drawing.Point(285, 145)
		Me.chkCopyFiles.Name = "chkCopyFiles"
		Me.chkCopyFiles.Size = New System.Drawing.Size(141, 17)
		Me.chkCopyFiles.TabIndex = 16
		Me.chkCopyFiles.Text = "Copy Files with NauXML"
		Me.chkCopyFiles.UseVisualStyleBackColor = True
		'
		'lblMisc
		'
		Me.lblMisc.AutoSize = True
		Me.lblMisc.Location = New System.Drawing.Point(15, 146)
		Me.lblMisc.Name = "lblMisc"
		Me.lblMisc.Size = New System.Drawing.Size(32, 13)
		Me.lblMisc.TabIndex = 15
		Me.lblMisc.Text = "Misc:"
		'
		'lblCompare
		'
		Me.lblCompare.AutoSize = True
		Me.lblCompare.Location = New System.Drawing.Point(15, 118)
		Me.lblCompare.Name = "lblCompare"
		Me.lblCompare.Size = New System.Drawing.Size(52, 13)
		Me.lblCompare.TabIndex = 14
		Me.lblCompare.Text = "Compare:"
		'
		'chkHash
		'
		Me.chkHash.AutoSize = True
		Me.chkHash.Checked = True
		Me.chkHash.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkHash.Location = New System.Drawing.Point(320, 117)
		Me.chkHash.Name = "chkHash"
		Me.chkHash.Size = New System.Drawing.Size(51, 17)
		Me.chkHash.TabIndex = 13
		Me.chkHash.Text = "Hash"
		Me.chkHash.UseVisualStyleBackColor = True
		'
		'chkDate
		'
		Me.chkDate.AutoSize = True
		Me.chkDate.Location = New System.Drawing.Point(265, 117)
		Me.chkDate.Name = "chkDate"
		Me.chkDate.Size = New System.Drawing.Size(49, 17)
		Me.chkDate.TabIndex = 12
		Me.chkDate.Text = "Date"
		Me.chkDate.UseVisualStyleBackColor = True
		'
		'chkSize
		'
		Me.chkSize.AutoSize = True
		Me.chkSize.Location = New System.Drawing.Point(213, 117)
		Me.chkSize.Name = "chkSize"
		Me.chkSize.Size = New System.Drawing.Size(46, 17)
		Me.chkSize.TabIndex = 11
		Me.chkSize.Text = "Size"
		Me.chkSize.UseVisualStyleBackColor = True
		'
		'chkVersion
		'
		Me.chkVersion.AutoSize = True
		Me.chkVersion.Checked = True
		Me.chkVersion.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkVersion.Location = New System.Drawing.Point(146, 117)
		Me.chkVersion.Margin = New System.Windows.Forms.Padding(3, 3, 3, 8)
		Me.chkVersion.Name = "chkVersion"
		Me.chkVersion.Size = New System.Drawing.Size(61, 17)
		Me.chkVersion.TabIndex = 10
		Me.chkVersion.Text = "Version"
		Me.chkVersion.UseVisualStyleBackColor = True
		'
		'txtBaseURL
		'
		Me.txtBaseURL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtBaseURL.Location = New System.Drawing.Point(146, 86)
		Me.txtBaseURL.Margin = New System.Windows.Forms.Padding(3, 3, 3, 8)
		Me.txtBaseURL.Name = "txtBaseURL"
		Me.txtBaseURL.Size = New System.Drawing.Size(387, 20)
		Me.txtBaseURL.TabIndex = 9
		'
		'lblBaseURL
		'
		Me.lblBaseURL.AutoSize = True
		Me.lblBaseURL.Location = New System.Drawing.Point(15, 89)
		Me.lblBaseURL.Name = "lblBaseURL"
		Me.lblBaseURL.Size = New System.Drawing.Size(59, 13)
		Me.lblBaseURL.TabIndex = 8
		Me.lblBaseURL.Text = "Base URL:"
		'
		'chkIgnoreSymbols
		'
		Me.chkIgnoreSymbols.AutoSize = True
		Me.chkIgnoreSymbols.Checked = True
		Me.chkIgnoreSymbols.CheckState = System.Windows.Forms.CheckState.Checked
		Me.chkIgnoreSymbols.Location = New System.Drawing.Point(146, 145)
		Me.chkIgnoreSymbols.Name = "chkIgnoreSymbols"
		Me.chkIgnoreSymbols.Size = New System.Drawing.Size(133, 17)
		Me.chkIgnoreSymbols.TabIndex = 7
		Me.chkIgnoreSymbols.Text = "Ignore Debug Symbols"
		Me.chkIgnoreSymbols.UseVisualStyleBackColor = True
		'
		'cmdFeedXML
		'
		Me.cmdFeedXML.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdFeedXML.Location = New System.Drawing.Point(539, 53)
		Me.cmdFeedXML.Name = "cmdFeedXML"
		Me.cmdFeedXML.Size = New System.Drawing.Size(26, 23)
		Me.cmdFeedXML.TabIndex = 5
		Me.cmdFeedXML.Text = "..."
		Me.cmdFeedXML.UseVisualStyleBackColor = True
		'
		'txtFeedXML
		'
		Me.txtFeedXML.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtFeedXML.BackColor = System.Drawing.Color.White
		Me.txtFeedXML.Location = New System.Drawing.Point(146, 55)
		Me.txtFeedXML.Margin = New System.Windows.Forms.Padding(3, 3, 3, 8)
		Me.txtFeedXML.Name = "txtFeedXML"
		Me.txtFeedXML.ReadOnly = True
		Me.txtFeedXML.Size = New System.Drawing.Size(387, 20)
		Me.txtFeedXML.TabIndex = 4
		'
		'lblFeedXML
		'
		Me.lblFeedXML.AutoSize = True
		Me.lblFeedXML.Location = New System.Drawing.Point(15, 58)
		Me.lblFeedXML.Name = "lblFeedXML"
		Me.lblFeedXML.Size = New System.Drawing.Size(98, 13)
		Me.lblFeedXML.TabIndex = 3
		Me.lblFeedXML.Text = "Feed NauXML File:"
		'
		'cmdOutputFolder
		'
		Me.cmdOutputFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdOutputFolder.Location = New System.Drawing.Point(539, 22)
		Me.cmdOutputFolder.Name = "cmdOutputFolder"
		Me.cmdOutputFolder.Size = New System.Drawing.Size(26, 23)
		Me.cmdOutputFolder.TabIndex = 2
		Me.cmdOutputFolder.Text = "..."
		Me.cmdOutputFolder.UseVisualStyleBackColor = True
		'
		'txtOutputFolder
		'
		Me.txtOutputFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
				  Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.txtOutputFolder.BackColor = System.Drawing.Color.White
		Me.txtOutputFolder.Location = New System.Drawing.Point(146, 24)
		Me.txtOutputFolder.Margin = New System.Windows.Forms.Padding(3, 3, 3, 8)
		Me.txtOutputFolder.Name = "txtOutputFolder"
		Me.txtOutputFolder.ReadOnly = True
		Me.txtOutputFolder.Size = New System.Drawing.Size(387, 20)
		Me.txtOutputFolder.TabIndex = 1
		'
		'lblOutputFolder
		'
		Me.lblOutputFolder.AutoSize = True
		Me.lblOutputFolder.Location = New System.Drawing.Point(15, 27)
		Me.lblOutputFolder.Name = "lblOutputFolder"
		Me.lblOutputFolder.Size = New System.Drawing.Size(110, 13)
		Me.lblOutputFolder.TabIndex = 0
		Me.lblOutputFolder.Text = "Project Output Folder:"
		'
		'tlpMain
		'
		Me.tlpMain.ColumnCount = 3
		Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
		Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
		Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
		Me.tlpMain.Controls.Add(Me.lstFiles, 0, 1)
		Me.tlpMain.Controls.Add(Me.cmdBuild, 2, 2)
		Me.tlpMain.Controls.Add(Me.cmdClose, 0, 2)
		Me.tlpMain.Controls.Add(Me.lnkRefresh, 1, 2)
		Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tlpMain.Location = New System.Drawing.Point(12, 181)
		Me.tlpMain.Name = "tlpMain"
		Me.tlpMain.RowCount = 3
		Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12.0!))
		Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
		Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
		Me.tlpMain.Size = New System.Drawing.Size(580, 209)
		Me.tlpMain.TabIndex = 1
		'
		'lstFiles
		'
		Me.lstFiles.CheckBoxes = True
		Me.lstFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFilename, Me.colVersion, Me.colSize, Me.colDate, Me.colHash})
		Me.tlpMain.SetColumnSpan(Me.lstFiles, 3)
		Me.lstFiles.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lstFiles.Location = New System.Drawing.Point(0, 12)
		Me.lstFiles.Margin = New System.Windows.Forms.Padding(0)
		Me.lstFiles.Name = "lstFiles"
		Me.lstFiles.Size = New System.Drawing.Size(580, 162)
		Me.lstFiles.SmallImageList = Me.imgFiles
		Me.lstFiles.TabIndex = 0
		Me.lstFiles.UseCompatibleStateImageBehavior = False
		Me.lstFiles.View = System.Windows.Forms.View.Details
		'
		'colFilename
		'
		Me.colFilename.Text = "Filename"
		Me.colFilename.Width = 200
		'
		'colVersion
		'
		Me.colVersion.Text = "Version"
		Me.colVersion.Width = 80
		'
		'colSize
		'
		Me.colSize.Text = "Size"
		Me.colSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.colSize.Width = 80
		'
		'colDate
		'
		Me.colDate.Text = "Date"
		Me.colDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me.colDate.Width = 120
		'
		'colHash
		'
		Me.colHash.Text = "Hash"
		Me.colHash.Width = 300
		'
		'imgFiles
		'
		Me.imgFiles.ImageStream = CType(resources.GetObject("imgFiles.ImageStream"), System.Windows.Forms.ImageListStreamer)
		Me.imgFiles.TransparentColor = System.Drawing.Color.Transparent
		Me.imgFiles.Images.SetKeyName(0, "file_extension_other.png")
		Me.imgFiles.Images.SetKeyName(1, "file_extension_bmp.png")
		Me.imgFiles.Images.SetKeyName(2, "file_extension_dll.png")
		Me.imgFiles.Images.SetKeyName(3, "file_extension_doc.png")
		Me.imgFiles.Images.SetKeyName(4, "file_extension_exe.png")
		Me.imgFiles.Images.SetKeyName(5, "file_extension_htm.png")
		Me.imgFiles.Images.SetKeyName(6, "file_extension_jpg.png")
		Me.imgFiles.Images.SetKeyName(7, "file_extension_pdf.png")
		Me.imgFiles.Images.SetKeyName(8, "file_extension_png.png")
		Me.imgFiles.Images.SetKeyName(9, "file_extension_txt.png")
		Me.imgFiles.Images.SetKeyName(10, "file_extension_wav.png")
		Me.imgFiles.Images.SetKeyName(11, "file_extension_wmv.png")
		Me.imgFiles.Images.SetKeyName(12, "file_extension_zip.png")
		'
		'cmdBuild
		'
		Me.cmdBuild.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
		Me.cmdBuild.Location = New System.Drawing.Point(505, 186)
		Me.cmdBuild.Margin = New System.Windows.Forms.Padding(0)
		Me.cmdBuild.Name = "cmdBuild"
		Me.cmdBuild.Size = New System.Drawing.Size(75, 23)
		Me.cmdBuild.TabIndex = 2
		Me.cmdBuild.Text = "Build"
		Me.cmdBuild.UseVisualStyleBackColor = True
		'
		'cmdClose
		'
		Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
		Me.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.cmdClose.Location = New System.Drawing.Point(0, 186)
		Me.cmdClose.Margin = New System.Windows.Forms.Padding(0)
		Me.cmdClose.Name = "cmdClose"
		Me.cmdClose.Size = New System.Drawing.Size(75, 23)
		Me.cmdClose.TabIndex = 1
		Me.cmdClose.Text = "Close"
		Me.cmdClose.UseVisualStyleBackColor = True
		'
		'fbdOutputFolder
		'
		Me.fbdOutputFolder.Description = "Select your projects output folder:"
		'
		'sfdFeedXML
		'
		Me.sfdFeedXML.DefaultExt = "xml"
		Me.sfdFeedXML.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
		Me.sfdFeedXML.Title = "Select the location to save your NauXML file:"
		'
		'lnkRefresh
		'
		Me.lnkRefresh.Dock = System.Windows.Forms.DockStyle.Bottom
		Me.lnkRefresh.Location = New System.Drawing.Point(78, 186)
		Me.lnkRefresh.Name = "lnkRefresh"
		Me.lnkRefresh.Size = New System.Drawing.Size(424, 23)
		Me.lnkRefresh.TabIndex = 3
		Me.lnkRefresh.TabStop = True
		Me.lnkRefresh.Text = "Refresh"
		Me.lnkRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'frmMain
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.Color.White
		Me.CancelButton = Me.cmdClose
		Me.ClientSize = New System.Drawing.Size(604, 402)
		Me.Controls.Add(Me.tlpMain)
		Me.Controls.Add(Me.grpSettings)
		Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(620, 440)
		Me.Name = "frmMain"
		Me.Padding = New System.Windows.Forms.Padding(12, 8, 12, 12)
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Feed Builder"
		Me.grpSettings.ResumeLayout(False)
		Me.grpSettings.PerformLayout()
		Me.tlpMain.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents grpSettings As System.Windows.Forms.GroupBox
	Friend WithEvents cmdOutputFolder As System.Windows.Forms.Button
	Friend WithEvents txtOutputFolder As System.Windows.Forms.TextBox
	Friend WithEvents lblOutputFolder As System.Windows.Forms.Label
	Friend WithEvents cmdFeedXML As System.Windows.Forms.Button
	Friend WithEvents txtFeedXML As System.Windows.Forms.TextBox
	Friend WithEvents lblFeedXML As System.Windows.Forms.Label
	Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
	Friend WithEvents lstFiles As System.Windows.Forms.ListView
	Friend WithEvents fbdOutputFolder As System.Windows.Forms.FolderBrowserDialog
	Friend WithEvents sfdFeedXML As System.Windows.Forms.SaveFileDialog
	Friend WithEvents cmdBuild As System.Windows.Forms.Button
	Friend WithEvents cmdClose As System.Windows.Forms.Button
	Friend WithEvents colFilename As System.Windows.Forms.ColumnHeader
	Friend WithEvents colSize As System.Windows.Forms.ColumnHeader
	Friend WithEvents colDate As System.Windows.Forms.ColumnHeader
	Friend WithEvents colHash As System.Windows.Forms.ColumnHeader
	Friend WithEvents imgFiles As System.Windows.Forms.ImageList
	Friend WithEvents chkIgnoreSymbols As System.Windows.Forms.CheckBox
	Friend WithEvents colVersion As System.Windows.Forms.ColumnHeader
	Friend WithEvents txtBaseURL As System.Windows.Forms.TextBox
	Friend WithEvents lblBaseURL As System.Windows.Forms.Label
	Friend WithEvents lblCompare As System.Windows.Forms.Label
	Friend WithEvents chkHash As System.Windows.Forms.CheckBox
	Friend WithEvents chkDate As System.Windows.Forms.CheckBox
	Friend WithEvents chkSize As System.Windows.Forms.CheckBox
	Friend WithEvents chkVersion As System.Windows.Forms.CheckBox
	Friend WithEvents lblMisc As System.Windows.Forms.Label
	Friend WithEvents chkCopyFiles As System.Windows.Forms.CheckBox
	Friend WithEvents chkCleanUp As System.Windows.Forms.CheckBox
	Friend WithEvents lnkRefresh As System.Windows.Forms.LinkLabel

End Class
