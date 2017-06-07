Imports System.Runtime.InteropServices
Imports System.Drawing

''' <summary>
''' Class that retrieves icons from file paths or extensions
''' </summary>
''' <remarks></remarks>
Public Class FileIcon
	Private Sub New()
	End Sub
	Private Declare Auto Function DestroyIcon Lib "user32" (ByVal hIcon As Integer) As Integer

	Private Declare Auto Function SHGetFileInfo Lib "shell32.dll" (
	ByVal pszPath As String,
	ByVal dwFileAttributes As Int32,
	ByRef psfi As SHFILEINFO,
	ByVal cbFileInfo As Int32,
	ByVal uFlags As Int32
) As IntPtr

	<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto, Pack:=1)>
	Private Structure SHFILEINFO
		Private Const MAX_PATH As Int32 = 260
		Public hIcon As IntPtr
		Public iIcon As Int32
		Public dwAttributes As Int32
		<MarshalAs(UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)>
		Public szDisplayName As String
		<MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
		Public szTypeName As String
		Public Sub New(ByVal B As Boolean)
			hIcon = IntPtr.Zero
			iIcon = 0
			dwAttributes = 0
			szDisplayName = vbNullString
			szTypeName = vbNullString
		End Sub
	End Structure


	Private Declare Auto Function SHGetFileInfo Lib "shell32.dll" _
			(ByVal pszPath As String,
			 ByVal dwFileAttributes As Integer,
			 ByRef psfi As SHFILEINFO,
			 ByVal cbFileInfo As Integer,
			 ByVal uFlags As SHGFI) As IntPtr


	<Flags()>
	Public Enum SHGFI As Int32

		SHGFI_ICON = &H100
		SHGFI_DISPLAYNAME = &H200
		SHGFI_TYPENAME = &H400
		SHGFI_ATTRIBUTES = &H800
		SHGFI_ICONLOCATION = &H1000
		SHGFI_EXETYPE = &H2000
		SHGFI_SYSICONINDEX = &H4000
		SHGFI_LINKOVERLAY = &H8000
		SHGFI_SELECTED = &H10000
		SHGFI_ATTR_SPECIFIED = &H20000
		SHGFI_LARGEICON = &H0
		SHGFI_SMALLICON = &H1
		SHGFI_OPENICON = &H2
		SHGFI_SHELLICONSIZE = &H4
		SHGFI_PIDL = &H8
		SHGFI_USEFILEATTRIBUTES = &H10
		SHGFI_ADDOVERLAYS = &H20
		SHGFI_OVERLAYINDEX = &H40
	End Enum

	Public Enum enmIconSize
		Small
		Large
	End Enum
	''' <summary>
	''' Gets an icon from a file path or extension.
	''' </summary>
	''' <param name="Extension">Path of file or extension.  Must contain a ".".</param>
	''' <param name="IconSize">Whether you want a large(32x32) or small(16x16) sized icon.</param>
	''' <param name="FileDescr">A string that will be set with the short file type description.</param>
	''' <returns>A bitmap object with the filetype icon associated with the file or extension.</returns>
	''' <remarks></remarks>
	Public Shared Function GetIconAsImageFromExtension(ByVal Extension As String, ByVal IconSize As enmIconSize, ByRef FileDescr As String) As Bitmap
		If Not Extension.Contains(".") Then Throw New System.ArgumentException("Extension must contain a ""."" to retrieve the icon from the filetype.")
		Dim shinfo As SHFILEINFO
		shinfo = New SHFILEINFO(True)
		Dim hImg As IntPtr
		'shinfo.szTypeName = Extension
		Dim baseFlags As SHGFI = SHGFI.SHGFI_TYPENAME Or SHGFI.SHGFI_DISPLAYNAME Or SHGFI.SHGFI_ICON
		If Not IO.File.Exists(Extension) Then
			baseFlags = baseFlags Or SHGFI.SHGFI_USEFILEATTRIBUTES
		End If

		If IconSize = enmIconSize.Small Then
			hImg = SHGetFileInfo(Extension, 256, shinfo,
								Marshal.SizeOf(shinfo),
								baseFlags Or SHGFI.SHGFI_SMALLICON)
		Else
			hImg = SHGetFileInfo(Extension, 256, shinfo,
									Marshal.SizeOf(shinfo),
								  baseFlags Or SHGFI.SHGFI_LARGEICON) 'Or SHGFI.SHGFI_LARGEICON)
		End If
		FileDescr = shinfo.szTypeName
		Using MyIcon As Icon = CType(Icon.FromHandle(shinfo.hIcon).Clone, Icon)



			DestroyIcon(shinfo.iIcon)
			'If IO.File.Exists(Extension) Then Return Icon.ExtractAssociatedIcon(Extension).ToBitmap
			Return MyIcon.ToBitmap()

		End Using


	End Function

End Class
''' <summary>
''' Used in the <see cref="FileIconListCreator"></see> to return information on all files or extensions requested.
''' </summary>
''' <remarks></remarks>
Public Class OSFileInfo
	Private _FileName As String
	Private _Icon As Bitmap
	Private _FileTypeDescription As String
	''' <summary>
	''' Returns the file name or extension used to get the icon.
	''' </summary>
	''' <value>The file name or extension used to get the icon.</value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property FileName() As String
		Get
			Return _FileName
		End Get
	End Property
	''' <summary>
	''' Returns the short file type description of the file or extension. 
	''' </summary>
	''' <value>The short file type description of the file or extension. </value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property FileTypeDescription() As String
		Get
			Return _FileTypeDescription
		End Get
	End Property
	Public ReadOnly Property Icon() As Bitmap
		Get
			Return _Icon
		End Get
	End Property

	Public Sub New(ByVal FileName As String, ByVal TypeDesc As String, ByVal Icon As Bitmap)
		_Icon = Icon
		_FileTypeDescription = TypeDesc
		_FileName = FileName
	End Sub
End Class
