Imports System.Threading.Tasks
Imports System.Threading.Tasks.Schedulers
Imports System.Threading
Imports System.Drawing
''' <summary>
''' A class that will generate icons from files or filetypes.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class FileIconListCreator

	Private Sub New()

	End Sub

	Public Shared Async Function GenerateAsync(iconSize As FileIcon.enmIconSize, extensions As IEnumerable(Of String)) As Task(Of IEnumerable(Of OSFileInfo))
		Try
			Using ts As StaTaskScheduler = New StaTaskScheduler(1)
				Dim tsk = Task.Factory.StartNew(Of IEnumerable(Of OSFileInfo))(
			Function()
				Return GenerateIcons(iconSize, extensions).AsEnumerable
			End Function,
			CancellationToken.None,
			TaskCreationOptions.None,
			ts)
				Return Await tsk
			End Using
		Catch ex As Exception
			Return Enumerable.Empty(Of OSFileInfo)
		End Try
	End Function
	Private Shared Function GenerateIcons(iconSize As FileIcon.enmIconSize, extensions As IEnumerable(Of String)) As IEnumerable(Of OSFileInfo)
		Dim results As New List(Of OSFileInfo)
		For Each ext As String In extensions
			Dim fdesc As String = ""
			Dim ic As Bitmap = FileIcon.GetIconAsImageFromExtension(ext, iconSize, fdesc)
			ic.MakeTransparent()
			results.Add(New OSFileInfo(ext, fdesc, ic))
		Next
		Return results.AsEnumerable
	End Function

End Class

