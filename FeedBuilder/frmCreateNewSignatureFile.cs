using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace FeedBuilder
{
    public partial class frmCreateNewSignatureFile : Form
    {
        public frmCreateNewSignatureFile()
        {
            InitializeComponent();
        }

        private void cmdNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdYes_Click(object sender, EventArgs e)
        {
            var defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            folderBrowserDialog1.SelectedPath = defaultPath;
            if (folderBrowserDialog1.ShowDialog(this) != DialogResult.OK) this.Close();
            string selectedPath = folderBrowserDialog1.SelectedPath;
            CreateSignatureFilesAndStoreTo(selectedPath);
            this.Close();
        }

        public string SelectedPublicKeyPath { get; private set; }

        private void CreateSignatureFilesAndStoreTo(string selectedPath)
        {
            var privFP = selectedPath + Path.DirectorySeparatorChar + "private.txt";
            var pubFP = selectedPath + Path.DirectorySeparatorChar + "public.txt";

            if (File.Exists(privFP) ||
                File.Exists(pubFP))
            {
                MessageBox.Show("There is already files private.txt and/or public.txt in the directory. Please select another directory or delete the existing files first.");
                return;
            }

            var p = new CspParameters();
            p.Flags = CspProviderFlags.NoPrompt | CspProviderFlags.UseArchivableKey;

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(2048, p);
            provider.PersistKeyInCsp = false;

            string privKey = provider.ToXmlString(true);
            string pubKey = provider.ToXmlString(false);

            using (var file = File.Open(privFP, FileMode.CreateNew))
            {
                using(StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(privKey);
                }
            }
            using (var file = File.Open(pubFP, FileMode.CreateNew))
            {
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(pubKey);
                }
            }

            SelectedPublicKeyPath = pubFP;

            MessageBox.Show("key created successfully");
        }
    }
}
