using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace AKRS.Galaxy2.Dispense
{
    public partial class SaveEpoxyPatternDialogBox : Form
    {
        string _strDirPath;

        public SaveEpoxyPatternDialogBox()
        {
            InitializeComponent();

            _strDirPath = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.textBoxPatternName.Clear();
            this.textBoxPatternName.Focus();
        }

        private void buttonXConfirm_Click(object sender, EventArgs e)
        {
            string strMessageBoxMessage = null;

            string strText = this.textBoxPatternName.Text;
            if (strText.Length == 0)
            {
                strMessageBoxMessage = "名称不能是空";
                goto LABEL_FAILED;
            }

            string strName = strText.Trim();
            if (strName.Length == 0)
            {
                strMessageBoxMessage = "名称不能是空白";
                goto LABEL_FAILED;
            }

            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
            foreach (char ch in invalidFileNameChars)
            {
                if (strName.Contains(ch))
                {
                    strMessageBoxMessage = String.Format("名称中发现一个非法字符\"{0}\"", ch);
                    goto LABEL_FAILED;
                }
            }

            if (!String.IsNullOrEmpty(_strDirPath))
            {
                string strFilePath = Path.Combine(_strDirPath, strName);
                if (File.Exists(strFilePath))
                {
                    strMessageBoxMessage = "文件已经存在";
                    goto LABEL_FAILED;
                }
            }

            //if (!strName.EndsWith(PatternFile.PATTERN_FILE_EXTENSION))
            //    strName = PatternFile.ConstructFileNameByPatternName(strName);

            this.textBoxPatternName.Text = strName;
            DialogResult = DialogResult.OK;
            return;

LABEL_FAILED:
            this.textBoxPatternName.SelectAll();
            this.textBoxPatternName.Focus();

            Debug.Assert(strMessageBoxMessage != null);
            string caption = Text;
            MessageBox.Show(this, strMessageBoxMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }    

        public string DirPath
        {
            get { return _strDirPath; }
            set { SetDirPath(value); }
        }

        public void SetDirPath(string strDirPath)
        {
            if (strDirPath == null)
                throw new ArgumentNullException(nameof(strDirPath));

            if (strDirPath == "")
                throw new ArgumentException("cannot be an empty string", nameof(strDirPath));

            if (!Directory.Exists(strDirPath))
            {
                Directory.CreateDirectory(strDirPath);
            }

            _strDirPath = strDirPath;
        }

        public string PatternName
        {
            get { return this.textBoxPatternName.Text; }
        }

        public string GetPatternFilePath()
        {
            if (String.IsNullOrEmpty(_strDirPath))
                throw new Exception("dir path is not set");

            if (String.IsNullOrEmpty(PatternName))
                throw new Exception("file name is not set");

            string strPatternFileName = PatternName + PatternFile.PATTERN_FILE_EXTENSION;
            string strPatternFilePath = Path.Combine(_strDirPath, strPatternFileName);

            return strPatternFilePath;
        }
    }
}
