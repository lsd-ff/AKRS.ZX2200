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
    public partial class RetrieveEpoxyPatternDialogBox : Form
    {
        string _strRepositoryPath;
        string _strSelectedFilePath;

        public RetrieveEpoxyPatternDialogBox()
        {
            InitializeComponent();

            _strRepositoryPath = "";
            _strSelectedFilePath = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(_strRepositoryPath))
                throw new Exception("repository path is not set");

            string strSearchPattern = PatternFile.ConstructFileNameByPatternName("*");
            string[] strFilePathes = Directory.GetFiles(_strRepositoryPath, strSearchPattern);

            string[] strPatternNames = new string[strFilePathes.Length];
            for (int i = 0; i < strFilePathes.Length; i++)
                strPatternNames[i] = Path.GetFileNameWithoutExtension(strFilePathes[i]);

            this.listBox1.DataSource = strPatternNames;

            this.buttonXConfirm.Enabled = (strPatternNames.Length > 0);
        }

        public string RepositoryPath
        {
            get { return _strRepositoryPath; }
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

            _strRepositoryPath = strRepositoryPath;
        }

        
        public string SelectedFilePath
        {
            get { return _strSelectedFilePath; }
        }

        private void buttonXConfirm_Click(object sender, EventArgs e)
        {
            string strSelectedPatternName = (string)this.listBox1.SelectedValue;
            if (strSelectedPatternName != null)
            {
                string strFileName = PatternFile.ConstructFileNameByPatternName(strSelectedPatternName);
                _strSelectedFilePath = Path.Combine(_strRepositoryPath, strFileName);

                DialogResult = DialogResult.OK;
            }
        }
    }
}
