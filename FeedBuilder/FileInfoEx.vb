Imports System.IO

Public Class FileInfoEx
	Private myFileInfo As FileInfo
	Private myFileVersion, myHash As String

	Public ReadOnly Property FileInfo As FileInfo
		Get
			Return myFileInfo
		End Get
	End Property

	Public ReadOnly Property FileVersion As String
		Get
			Return myFileVersion
		End Get
	End Property

	Public ReadOnly Property Hash As String
		Get
			Return myHash
		End Get
	End Property

	Public Sub New(fileName As String)
		myFileInfo = New FileInfo(fileName)
		myFileVersion = FileVersionInfo.GetVersionInfo(fileName).FileVersion
		myHash = NAppUpdate.Framework.Utils.FileChecksum.GetSHA256Checksum(fileName)
	End Sub
End Class
