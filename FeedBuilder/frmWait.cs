using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FeedBuilder
{
    public partial class frmWait : Form
    {
        public frmWait()
        {
            InitializeComponent();
        }

        private int fileCount = 0;
        /// <summary>
        /// Gets or sets the count of files processed.
        /// </summary>
        public int FileProcessedCount
        {
            get { return fileCount; }
            set
            {
                fileCount = value;
                lblFileCount.Text = string.Format("{0} files processed.", value);
            }
        }

        public void FileProcessed(object sender, FileProcessedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { FileProcessed(sender, e); });
                return;
            }
            else
                FileProcessedCount = e.FileProcesCount;
        }
    }
}
