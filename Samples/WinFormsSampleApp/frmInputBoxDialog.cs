using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsSampleApp
{
	public class frmInputBoxDialog : Form
	{
		#region Windows Contols and Constructor

		private Label lblPrompt;
		private Button btnOK;
		private Button btnCancel;
		private TextBox txtInput;

		///<summary>
		///  Required designer variable.
		///</summary>
		private readonly Container components = null;

		public frmInputBoxDialog()
		{
			InitializeComponent();
		}

		#endregion

		#region Dispose

		///<summary>
		///  Clean up any resources being used.
		///</summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing) if (components != null) components.Dispose();
			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code

		///<summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		///</summary>
		private void InitializeComponent()
		{
			this.lblPrompt = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtInput = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// lblPrompt
			// 
			this.lblPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPrompt.BackColor = System.Drawing.SystemColors.Control;
			this.lblPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblPrompt.Location = new System.Drawing.Point(12, 9);
			this.lblPrompt.Name = "lblPrompt";
			this.lblPrompt.Size = new System.Drawing.Size(302, 82);
			this.lblPrompt.TabIndex = 3;
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(326, 24);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(64, 24);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(326, 56);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(64, 24);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.Click += new System.EventHandler(this.button1_Click);
			// 
			// txtInput
			// 
			this.txtInput.Location = new System.Drawing.Point(8, 100);
			this.txtInput.Name = "txtInput";
			this.txtInput.Size = new System.Drawing.Size(379, 20);
			this.txtInput.TabIndex = 0;
			// 
			// InputBoxDialog
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(398, 128);
			this.Controls.Add(this.txtInput);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblPrompt);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputBoxDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "InputBox";
			this.Load += new System.EventHandler(this.InputBox_Load);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		#region Private Variables

		private string formCaption = string.Empty;
		private string formPrompt = string.Empty;
		private string inputResponse = string.Empty;
		private string defaultValue = string.Empty;

		#endregion

		#region Public Properties

		public string FormCaption
		{
			get { return formCaption; }
			set { formCaption = value; }
		}

		// property FormCaption
		public string FormPrompt
		{
			get { return formPrompt; }
			set { formPrompt = value; }
		}

		// property FormPrompt
		public string InputResponse
		{
			get { return inputResponse; }
			set { inputResponse = value; }
		}

		// property InputResponse
		public string DefaultValue
		{
			get { return defaultValue; }
			set { defaultValue = value; }
		}

		// property DefaultValue

		#endregion

		#region Form and Control Events

		private void InputBox_Load(object sender, EventArgs e)
		{
			txtInput.Text = defaultValue;
			lblPrompt.Text = formPrompt;
			Text = formCaption;
			txtInput.SelectionStart = 0;
			txtInput.SelectionLength = txtInput.Text.Length;
			txtInput.Focus();
		}


		private void btnOK_Click(object sender, EventArgs e)
		{
			InputResponse = txtInput.Text;
			Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Close();
		}

		#endregion
	}
}