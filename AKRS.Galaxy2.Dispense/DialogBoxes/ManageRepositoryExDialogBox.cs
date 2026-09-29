using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AKRS.Galaxy2.Dispense;

namespace AKRS.Galaxy2.Dispense
{
    public partial class ManageRepositoryExDialogBox : Form
    {
        string _strPatternRepositoryPath;

        public ManageRepositoryExDialogBox()
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
            {
                string strPatternName = Path.GetFileNameWithoutExtension(strFilePathes[i]);
                strPatternNames[i] = strPatternName;

                string strSnapshootFileName = "Snapshoot_" + strPatternName + ".bmp";
                string strSnapshootFilePath = Path.Combine(_strPatternRepositoryPath, strSnapshootFileName);

                try
                {
                    string strTmpImageFileName = "tmp_" + DateTime.Now.Ticks.ToString() + ".bmp";
                    string strTmpImageFilePath = Path.Combine(_strPatternRepositoryPath, strTmpImageFileName);
                    File.Copy(strSnapshootFilePath, strTmpImageFilePath, true);

                    this.imageList1.Images.Add(new Bitmap(strTmpImageFilePath));

                    this.listView1.Items.Add(strPatternName, this.imageList1.Images.Count - 1);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }

            //this.listBox1.DataSource = strPatternNames;
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
            if (this.listView1.SelectedItems.Count > 0)
            {
                var selectedItem = this.listView1.SelectedItems[0];
                string strPatternName = selectedItem.Text;
                //string strPatternName = (string)this.listBox1.SelectedValue;
                if (strPatternName != null)
                {
                    string strFileName = PatternFile.ConstructFileNameByPatternName(strPatternName);
                    string strFilePath = Path.Combine(_strPatternRepositoryPath, strFileName);
                    File.Delete(strFilePath);

                    string strImageName = strPatternName + ".bmp";
                    string strImagePath = Path.Combine(_strPatternRepositoryPath, strImageName);
                    File.Delete(strImagePath);


                    //string[] strPatternNames = (string[])this.listBox1.DataSource;
                    //List<string> strPatternNameList = new List<string>(strPatternNames);
                    //strPatternNameList.Remove(strPatternName);
                    //this.listBox1.DataSource = strPatternNameList.ToArray();

                    this.imageList1.Images.RemoveAt(selectedItem.ImageIndex);
                    this.listView1.Items.Remove(selectedItem);
                }
            }
        }

        private void buttonXDeleteAllItems_Click(object sender, EventArgs e)
        {
            // Creates the directory if necessary.
            Directory.CreateDirectory(_strPatternRepositoryPath);

            this.listView1.Items.Clear();

            foreach (Image image in this.imageList1.Images)
            {
                image.Dispose();
            }

            this.imageList1.Images.Clear();


            string strSearchPattern = PatternFile.ConstructFileNameByPatternName("*");
            string[] strFilePathes = Directory.GetFiles(_strPatternRepositoryPath, strSearchPattern);
            foreach (string strFilePath in strFilePathes)
            {
                File.Delete(strFilePath);

                string strPatternName = Path.GetFileNameWithoutExtension(strFilePath);
                try
                {                    
                    string strImageFileName = strPatternName + ".bmp";
                    string strImageFilePath = Path.Combine(_strPatternRepositoryPath, strImageFileName);
                    File.Delete(strImageFilePath);
                }
                catch
                { }

                try
                {
                    string strSnapshootFileName = "Snapshoot_" + strPatternName + ".bmp";
                    string strSnapshootFilePath = Path.Combine(_strPatternRepositoryPath, strSnapshootFileName);
                    File.Delete(strSnapshootFilePath);
                }
                catch
                { }
            }

            //this.listBox1.DataSource = null;
        }

        private void buttonXExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            DeleteTemporaryFiles();
        }


        private void DeleteTemporaryFiles()
        {
            string[] strFilePathes = Directory.GetFiles(_strPatternRepositoryPath, "tmp_*.bmp");
            foreach (string strFilePath in strFilePathes)
            {
                try
                {
                    File.Delete(strFilePath);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }            
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            DeleteTemporaryFiles();
        }
    }
}
