namespace NAppUpdate.Updater
{
	partial class ConsoleForm
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
			if (disposing && (components != null))
			{
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
			this.rtbConsole = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// rtbConsole
			// 
			this.rtbConsole.BackColor = System.Drawing.Color.Black;
			this.rtbConsole.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtbConsole.Font = new System.Drawing.Font("Consolas", 10.5F);
			this.rtbConsole.ForeColor = System.Drawing.Color.White;
			this.rtbConsole.Location = new System.Drawing.Point(0, 0);
			this.rtbConsole.Name = "rtbConsole";
			this.rtbConsole.ReadOnly = true;
			this.rtbConsole.Size = new System.Drawing.Size(659, 302);
			this.rtbConsole.TabIndex = 0;
			this.rtbConsole.Text = "Microsoft Windows [Version 6.1.7601]\nCopyright (c) 2009 Microsoft Corportaion. Al" +
	"l rights reserved.\n\nC:\\Windows\\system32>";
			// 
			// ConsoleForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(659, 302);
			this.Controls.Add(this.rtbConsole);
			this.KeyPreview = true;
			this.Name = "ConsoleForm";
			this.Text = "Application Updater";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConsoleForm_FormClosed);
			this.Load += new System.EventHandler(this.ConsoleForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox rtbConsole;
	}
}
