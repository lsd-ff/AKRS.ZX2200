using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AKRS.Galaxy2.Dispense;

namespace AKRS.Galaxy2.Dispense
{
    public partial class ManageRepositoryDialogBox : Form
    {
        string _strPatternRepositoryPath;

        public ManageRepositoryDialogBox()
        {
            InitializeComponent();

            _strPatternRepositoryPath = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (String.IsNullOrEmpty(_strPatternRepositoryPath))
                throw new Exception("pattern repository path has not been set");

            string strSearchPattern = PatternFile.ConstructFileNameByPatternName("*");
            string[] strFilePathes = Directory.GetFiles(_strPatternRepositoryPath, strSearchPattern);

            string[] strPatternNames = new string[strFilePathes.Length];
            for (int i = 0; i < strFilePathes.Length; i++)
                strPatternNames[i] = Path.GetFileNameWithoutExtension(strFilePathes[i]);

            this.listBox1.DataSource = strPatternNames;
        }

        public string PatternRepository
        {
            get { return _strPatternRepositoryPath; }
        }

        public void SetPatternRepository(string strPatternRepositoryPath)
        {
            if (strPatternRepositoryPath == null)
                throw new ArgumentNullException(nameof(strPatternRepositoryPath));

            if (strPatternRepositoryPath.Length == 0)
                throw new ArgumentException("cannot be an empty string", nameof(strPatternRepositoryPath));

            // Creates the directory if necessary.
            Directory.CreateDirectory(strPatternRepositoryPath);

            _strPatternRepositoryPath = strPatternRepositoryPath;
        }

        private void ManageRepositoryDialogBox_Load(object sender, EventArgs e)
        {

        }

        private void buttonXDeleteSelectedItems_Click(object sender, EventArgs e)
        {
            string strPatternName = (string)this.listBox1.SelectedValue;
            if (strPatternName != null)
            {
                string strFileName = PatternFile.ConstructFileNameByPatternName(strPatternName);
                string strFilePath = Path.Combine(_strPatternRepositoryPath, strFileName);
                File.Delete(strFilePath);

                string strImageName = strPatternName + ".bmp";
                string strImagePath = Path.Combine(_strPatternRepositoryPath, strImageName);
                File.Delete(strImagePath);


                string[] strPatternNames = (string[])this.listBox1.DataSource;
                List<string> strPatternNameList = new List<string>(strPatternNames);
                strPatternNameList.Remove(strPatternName);
                this.listBox1.DataSource = strPatternNameList.ToArray();
            }
        }

        private void buttonXDeleteAllItems_Click(object sender, EventArgs e)
        {
            // Creates the directory if necessary.
            Directory.CreateDirectory(_strPatternRepositoryPath);

            string strSearchPattern = PatternFile.ConstructFileNameByPatternName("*");
            string[] strFilePathes = Directory.GetFiles(_strPatternRepositoryPath, strSearchPattern);
            foreach (string strFilePath in strFilePathes)
            {
                File.Delete(strFilePath);

                string strPatternName = Path.GetFileNameWithoutExtension(strFilePath);
                string strImageFileName = strPatternName + ".bmp";
                string strImageFilePath = Path.Combine(_strPatternRepositoryPath, strImageFileName);
                File.Delete(strImageFilePath);
            }

            this.listBox1.DataSource = null;
        }

        private void buttonXExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }


    }
}
