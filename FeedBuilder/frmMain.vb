Imports System.IO
Imports System.Xml

Public Class frmMain

	Private Sub frmMain_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
		If Not String.IsNullOrEmpty(My.Settings.OutputFolder) AndAlso Directory.Exists(My.Settings.OutputFolder) Then txtOutputFolder.Text = My.Settings.OutputFolder
		If Not String.IsNullOrEmpty(My.Settings.FeedXML) AndAlso Directory.Exists(Path.GetDirectoryName(My.Settings.FeedXML)) Then txtFeedXML.Text = My.Settings.FeedXML
		If Not String.IsNullOrEmpty(My.Settings.BaseURL) Then txtBaseURL.Text = My.Settings.BaseURL

		chkVersion.Checked = My.Settings.CompareVersion
		chkSize.Checked = My.Settings.CompareSize
		chkDate.Checked = My.Settings.CompareDate
		chkHash.Checked = My.Settings.CompareHash

		chkIgnoreSymbols.Checked = My.Settings.IgnoreDebugSymbols
		chkCopyFiles.Checked = My.Settings.CopyFiles
		chkCleanUp.Checked = My.Settings.CleanUp

		If My.Settings.IgnoreFiles Is Nothing Then My.Settings.IgnoreFiles = New Collections.Specialized.StringCollection
		ReadFiles()
	End Sub

	Private Sub frmMain_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		If Not String.IsNullOrEmpty(txtOutputFolder.Text.Trim) AndAlso Directory.Exists(txtOutputFolder.Text.Trim) Then My.Settings.OutputFolder = txtOutputFolder.Text.Trim
		If Not String.IsNullOrEmpty(txtFeedXML.Text.Trim) AndAlso Directory.Exists(Path.GetDirectoryName(txtFeedXML.Text.Trim)) Then My.Settings.FeedXML = txtFeedXML.Text.Trim
		If Not String.IsNullOrEmpty(txtBaseURL.Text.Trim) Then My.Settings.BaseURL = txtBaseURL.Text.Trim

		My.Settings.CompareVersion = chkVersion.Checked
		My.Settings.CompareSize = chkSize.Checked
		My.Settings.CompareDate = chkDate.Checked
		My.Settings.CompareHash = chkHash.Checked

		My.Settings.IgnoreDebugSymbols = chkIgnoreSymbols.Checked
		My.Settings.CopyFiles = chkCopyFiles.Checked
		My.Settings.CleanUp = chkCleanUp.Checked

		With My.Settings.IgnoreFiles
			.Clear()
			For Each thisItem As ListViewItem In lstFiles.Items
				If Not thisItem.Checked Then .Add(thisItem.Text)
			Next
		End With
	End Sub

#Region " Options "
	Private Sub cmdOutputFolder_Click(sender As System.Object, e As System.EventArgs) Handles cmdOutputFolder.Click
		fbdOutputFolder.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
		If fbdOutputFolder.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
			txtOutputFolder.Text = fbdOutputFolder.SelectedPath
			ReadFiles()
		End If
	End Sub

	Private Sub cmdFeedXML_Click(sender As System.Object, e As System.EventArgs) Handles cmdFeedXML.Click
		sfdFeedXML.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
		If sfdFeedXML.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then txtFeedXML.Text = sfdFeedXML.FileName
	End Sub

	Private Sub chkIgnoreSymbols_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkIgnoreSymbols.CheckedChanged
		ReadFiles()
	End Sub
#End Region
#Region " Misc "
	Private Function GetImageIndex(ext As String) As Integer
		Select Case ext.Trim("."c)
			Case "bmp"
				Return 1
			Case "dll"
				Return 2
			Case "doc", "docx"
				Return 3
			Case "exe"
				Return 4
			Case "htm", "html"
				Return 5
			Case "jpg", "jpeg"
				Return 6
			Case "pdf"
				Return 7
			Case "png"
				Return 8
			Case "txt"
				Return 9
			Case "wav", "mp3"
				Return 10
			Case "wmv"
				Return 11
			Case "xls", "xlsx"
				Return 12
			Case "zip"
				Return 13
			Case Else
				Return 0
		End Select
	End Function

	Private Sub cmdClose_Click(sender As System.Object, e As System.EventArgs) Handles cmdClose.Click
		Me.Close()
	End Sub
#End Region

	Private Sub ReadFiles()
		If Not String.IsNullOrEmpty(txtOutputFolder.Text.Trim) AndAlso Directory.Exists(txtOutputFolder.Text.Trim) Then
			Dim thisItem As ListViewItem
			Dim thisInfo As FileInfoEx

			lstFiles.BeginUpdate()
			lstFiles.Items.Clear()
			For Each thisFile As String In Directory.EnumerateFiles(txtOutputFolder.Text.Trim)
				If ((chkIgnoreSymbols.Checked And Path.GetExtension(thisFile) <> ".pdb") Or Not chkIgnoreSymbols.Checked) Then
					thisInfo = New FileInfoEx(thisFile)
					With thisInfo
						thisItem = New ListViewItem(.FileInfo.Name, GetImageIndex(.FileInfo.Extension))
						thisItem.SubItems.Add(.FileVersion)
						thisItem.SubItems.Add(.FileInfo.Length.ToString)
						thisItem.SubItems.Add(.FileInfo.LastWriteTime.ToString)
						thisItem.SubItems.Add(.Hash)
						thisItem.Checked = (Not My.Settings.IgnoreFiles.Contains(.FileInfo.Name))
					End With
					thisItem.Tag = thisInfo
					lstFiles.Items.Add(thisItem)
				End If
			Next
			lstFiles.EndUpdate()
		End If
	End Sub

	Private Sub cmdBuild_Click(sender As System.Object, e As System.EventArgs) Handles cmdBuild.Click
		Dim doc As New XmlDocument
		Dim dec As XmlDeclaration = doc.CreateXmlDeclaration("1.0", "utf-8", Nothing)
		Dim feed, tasks, task, conds, cond As XmlElement
		Dim pastFirst As Boolean

		doc.AppendChild(dec)
		feed = doc.CreateElement("Feed")
		If Not String.IsNullOrEmpty(txtBaseURL.Text.Trim) Then feed.SetAttribute("BaseUrl", txtBaseURL.Text.Trim)
		doc.AppendChild(feed)

		tasks = doc.CreateElement("Tasks")

		For Each thisItem As ListViewItem In lstFiles.Items
			Dim destFile As String = Path.Combine(Path.GetDirectoryName(txtFeedXML.Text.Trim), thisItem.Text)

			If thisItem.Checked Then
				pastFirst = False

				With CType(thisItem.Tag, FileInfoEx)
					task = doc.CreateElement("FileUpdateTask")
					task.SetAttribute("localPath", .FileInfo.Name)

					conds = doc.CreateElement("Conditions")

					'Version
					If chkVersion.Checked Then
						cond = doc.CreateElement("FileVersionCondition")
						cond.SetAttribute("what", "below")
						cond.SetAttribute("version", .FileVersion)
						conds.AppendChild(cond)
						pastFirst = True
					End If

					'Size
					If chkSize.Checked Then
						cond = doc.CreateElement("FileSizeCondition")
						If pastFirst Then cond.SetAttribute("type", "or")
						cond.SetAttribute("what", "is")
						cond.SetAttribute("size", .FileInfo.Length.ToString)
						conds.AppendChild(cond)
						pastFirst = True
					End If

					'Date
					If chkDate.Checked Then
						cond = doc.CreateElement("FileVersionCondition")
						If pastFirst Then cond.SetAttribute("type", "or")
						cond.SetAttribute("what", "below")
						cond.SetAttribute("timestamp", .FileInfo.LastWriteTime.ToFileTime.ToString)
						conds.AppendChild(cond)
						pastFirst = True
					End If

					'Hash
					If chkHash.Checked Then
						cond = doc.CreateElement("FileChecksumCondition")
						If pastFirst Then cond.SetAttribute("type", "or")
						cond.SetAttribute("checksumType", "sha256")
						cond.SetAttribute("checksum", .Hash)
						conds.AppendChild(cond)
					End If

					task.AppendChild(conds)
					tasks.AppendChild(task)

					If chkCopyFiles.Checked Then
						Try
							If File.Exists(destFile) Then File.Delete(destFile)
							File.Copy(.FileInfo.FullName, destFile)
						Catch ex As IOException
						End Try
					End If
				End With
			Else
				Try
					If chkCleanUp.Checked And File.Exists(destFile) Then File.Delete(destFile)
				Catch ex As IOException
				End Try
			End If
		Next
		feed.AppendChild(tasks)
		doc.Save(txtFeedXML.Text.Trim)
		With New Process
			.StartInfo.UseShellExecute = True
			.StartInfo.FileName = Path.GetDirectoryName(txtFeedXML.Text.Trim)
			.Start()
		End With
	End Sub

	Private Sub chkCopyFiles_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkCopyFiles.CheckedChanged
		chkCleanUp.Enabled = chkCopyFiles.Checked
		If Not chkCopyFiles.Checked Then chkCleanUp.Checked = False
	End Sub

	Private Sub lnkRefresh_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkRefresh.LinkClicked
		ReadFiles()
	End Sub
End Class