using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
namespace FeedBuilder
{

	public partial class HelpfulTextBox : TextBox
	{

		#region "Win32 DLL Imports"


		private const int EM_SETCUEBANNER = 0x1501;
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, 		[MarshalAs(UnmanagedType.LPWStr)]
string lParam);

		#endregion


		private string _helpfulText;
		[Browsable(true)]
		[Category("Appearance")]
		[DefaultValue("")]
		[Description("The grayed text that appears in the box when the box contains no text and is not focused.")]
		[RefreshProperties(RefreshProperties.None)]
		public string HelpfulText {
			get { return _helpfulText; }
			set {
				_helpfulText = value;
				SetCue();
			}
		}

		public HelpfulTextBox()
		{
			InitializeComponent();
			//Me.SetStyle(ControlStyles.UserPaint, True)
		}

		/// <summary>
		/// Actually, the system cue only works for editable (i.e. not read-only) text boxes.
		/// </summary>
		/// <remarks></remarks>
		private void SetCue()
		{
			SendMessage(this.Handle, EM_SETCUEBANNER, 0, this.HelpfulText);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			base.OnPaint(e);
			//If Me.Focused And Not Me.ReadOnly Then Return
			//If String.IsNullOrEmpty(Me.HelpfulText) Then Return
			//If Not String.IsNullOrEmpty(Me.Text) Then Return
			//Dim grayBrush As New SolidBrush(SystemColors.GrayText)
			//e.Graphics.DrawString(Me.HelpfulText, Me.Font, grayBrush, Me.ClientRectangle)
		}

	}
}
namespace FeedBuilder
{

	#region "Windows Forms stuff"

	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class HelpfulTextBox : System.Windows.Forms.TextBox
	{

		//Control overrides dispose to clean up the component list.
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

		//Required by the Control Designer

		private System.ComponentModel.IContainer components;
		// NOTE: The following procedure is required by the Component Designer
		// It can be modified using the Component Designer.  Do not modify it
		// using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

	}
}

#endregion
