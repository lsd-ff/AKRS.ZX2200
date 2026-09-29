using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    public partial class RetrieveEpoxyPatternExDialogBox : Form
    {
        string _strPatternRepositoryPath;
        string _strSelectedPatternFilePath;

        public RetrieveEpoxyPatternExDialogBox()
        {
            InitializeComponent();

            _strPatternRepositoryPath = "";
            _strSelectedPatternFilePath = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(_strPatternRepositoryPath))
                throw new Exception("repository path is not set");

            string strSearchPattern = PatternFile.ConstructFileNameByPatternName("*");
            string[] strFilePathes = Directory.GetFiles(_strPatternRepositoryPath, strSearchPattern);

            string[] strPatternNames = new string[strFilePathes.Length];
            for (int i = 0; i < strFilePathes.Length; i++)
            {
                string strPatternName = Path.GetFileNameWithoutExtension(strFilePathes[i]);
                strPatternNames[i] = strPatternName;

                string strSnapshootFileName = "Snapshoot_" + strPatternName + ".bmp";
                string strSnapshootFilePath = Path.Combine(_strPatternRepositoryPath, strSnapshootFileName);

                try
                {
                    this.imageList1.Images.Add(new Bitmap(strSnapshootFilePath));

                    this.listView1.Items.Add(strPatternName, this.imageList1.Images.Count - 1);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
            //this.listBox1.DataSource = strPatternNames;

            

            this.buttonXConfirm.Enabled = (strPatternNames.Length > 0);
        }

        public string RepositoryPath
        {
            get { return _strPatternRepositoryPath; }
            set { SetRepositoryPath(value); }
        }

        public void SetRepositoryPath(string strRepositoryPath)
        {
            if (strRepositoryPath == null)
                throw new ArgumentNullException(nameof(strRepositoryPath));

            if (strRepositoryPath == "")
                throw new ArgumentException("cannot be an empty string", nameof(strRepositoryPath));

            // Creates the directory if necessary.
            Directory.CreateDirectory(strRepositoryPath);

            _strPatternRepositoryPath = strRepositoryPath;
        }

        
        public string SelectedPatternFilePath
        {
            get { return _strSelectedPatternFilePath; }
        }

        private void buttonXConfirm_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0)
            {
                string strPatternName = this.listView1.SelectedItems[0].Text;
                string strPatternFileName = PatternFile.ConstructFileNameByPatternName(strPatternName);
                _strSelectedPatternFilePath = Path.Combine(_strPatternRepositoryPath, strPatternFileName);

                DialogResult = DialogResult.OK;
            }

            DeleteTemporaryFiles();
        }

        
        private void DeleteTemporaryFiles()
        {
            try
            {
                string[] strFilePathes = Directory.GetFiles(_strPatternRepositoryPath, "tmp_*.bmp");
                foreach (string strFilePath in strFilePathes)
                {
                    File.Delete(strFilePath);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void buttonXCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            DeleteTemporaryFiles();
        }
    }
}
