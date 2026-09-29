using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Akrs.ChangeJson
{
    public partial class UcChangeNameSpace : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 对象命名空间列表
        /// </summary>
        public List<Data> DllList = new List<Data>();

        /// <summary>
        /// Json列表
        /// </summary>
        public List<Data> JsonList = new List<Data>();

        /// <summary>
        /// 对象命名空间列表
        /// </summary>
        public List<ClassNameSpace> ClassNameSpaceList = new List<ClassNameSpace>();    

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcChangeNameSpace()
        {
            InitializeComponent();
        }

        private string filePath;

        /// <summary>
        /// 打开Dll 路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtOpenDllDir_Click(object sender, EventArgs e)
        {
            if (this.xtraFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.DllList.Clear();
                 filePath = this.xtraFolderBrowserDialog1.SelectedPath;
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }
                else
                {
                    this.TeDllDir.Text = filePath;
                    DllList = Directory.GetFiles(filePath, "AKRS.ZX2200.exe").ToList().Select(a => new Data() { Name = a }).ToList();
                  
                }

                this.GcDll.DataSource = DllList;
            }
        }

        /// <summary>
        /// 反射获取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtReflectObject_Click(object sender, EventArgs e)
        {
            this.ClassNameSpaceList.Clear();
            int [] selectedIndexs = this.GvDll.GetSelectedRows();
            foreach (int i in selectedIndexs)
            {
                Data dll = this.GvDll.GetRow(i) as Data;

                // 假设你的DLL名为"MyLibrary.dll"
                string dllName = dll.Name;

                // 加载DLL
                Assembly.LoadFrom("AKRS.Galaxy2.Infrastructure.dll");
                Assembly myAssembly = Assembly.LoadFrom(dllName);

                // 获取DLL中所有类型
                Type[] types = myAssembly.GetTypes();

                // 遍历所有类型，并筛选出类
                foreach (Type type in types)
                {
                    if (type.IsClass)
                    {
                        this.ClassNameSpaceList.Add(new ClassNameSpace(type.Name, type.Namespace));
                    }
                }
            }

            this.GcClassNameSpace.DataSource = this.ClassNameSpaceList;
        }

        private void BtReadJson_Click(object sender, EventArgs e)
        {
            if (this.xtraFolderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.JsonList.Clear();
                filePath = this.xtraFolderBrowserDialog1.SelectedPath;
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }
                else
                {
                    this.TeJsonDir.Text = filePath;
                    JsonList = Directory.GetFiles(filePath, "*.json", SearchOption.AllDirectories).ToList().Select(a => new Data() { Name = a }).ToList();
                }

                this.GcJson.DataSource = JsonList;
            }
        }

        /// <summary>
        /// 更新JSon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtUpdateJson_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                foreach (Data json in this.JsonList)
                {
                    // 1. 读取Json文本
                    string content = File.ReadAllText(json.Name);

                    Regex regex = new Regex("type\": \"(?<NameSpace>.+)\\.(?<ClassName>[^\\.]+), AKRS.ZX2200");

                    MatchCollection mc = regex.Matches(content);

                    foreach (Match macth in mc)
                    {
                        if (macth.Success)
                        {
                            string nameSpace = macth.Groups["NameSpace"].Value;
                            string className = macth.Groups["ClassName"].Value;

                            // 根据ClassName 去找新的命名空间
                            ClassNameSpace newClassNameSpace = ClassNameSpaceList.Find(a => a.ClassName == className);
                            if (newClassNameSpace == null)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.memoEdit1.AppendText(
                                        $"{json.Name}  未能找到{className} 的命名空间----------------------------------------");
                                }));
                            }
                            else
                            {
                                this.Invoke(new Action(() =>
                                {
                                    this.memoEdit1.AppendText(
                                        $"{json.Name} - {className} : {nameSpace} ---> {newClassNameSpace.NameSpace}");
                                }));

                                content = content.Replace(nameSpace + "." + className + ", AKRS.ZX2200\"",  newClassNameSpace.NameSpace + "." + className + ", AKRS.ZX2200\"");
                            }
                        }
                    }

                    string filePath = json.Name; // 替换为你的文件路径

                    // 写入并覆盖文件
                    File.WriteAllText(filePath, content);
                }
            });
        }
    }
}
