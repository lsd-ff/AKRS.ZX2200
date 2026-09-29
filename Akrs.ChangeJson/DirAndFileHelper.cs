#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：DESKTOP-8U1CRVN
 * 公司名称：
 * 命名空间：AKRS.Galaxy2.Infrastructure.Helper
 * 唯一标识：1f4af9b3-2b7d-4927-a349-df056b3467f2
 * 文件名：DirAndFileHelper
 * 当前用户域：DESKTOP-8U1CRVN
 * 
 * 创建时间：2022/4/2 13:52:32
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akrs.ChangeJson
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    public static class DirAndFileHelper
    {
       

        #region 目录操作

        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="dir">目录的绝对路径</param>
        /// <returns>是否存在</returns>
        public static bool IsExistDirectory(string dir)
        {
            return Directory.Exists(dir);
        }

        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param>  
        /// <returns>文件列表</returns>
        public static string[] GetFileNames(string dir)
        {
            // 获取文件列表
            return Directory.GetFiles(dir);
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>文件名称列表</returns>
        public static string[] GetFileNames(string dir, string searchPattern, bool isSearchChild)
        {
            SearchOption option = isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.GetFiles(dir, searchPattern, option);
        }

        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param>   
        /// <returns>子目录列表</returns>
        public static string[] GetDirectories(string dir)
        {
            return Directory.GetDirectories(dir);
        }

        /// <summary>
        /// 获取目录信息
        /// </summary>
        /// <param name="dir">目录路径</param>
        /// <returns>目录信息列表</returns>
        public static List<DirectoryInfo> GetDirectoryInfos(string dir)
        {
            return Directory.GetDirectories(dir).Select(a => new DirectoryInfo(a)).ToList();
        }

        /// <summary>
        /// 获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>目录列表</returns>
        public static string[] GetDirectories(string dir, string searchPattern, bool isSearchChild)
        {
            SearchOption option = isSearchChild ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.GetDirectories(dir, searchPattern, option);
        }

        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param> 
        /// <returns>是否为空</returns>
        public static bool IsEmptyDirectory(string dir)
        {
            // 判断是否存在文件
            string[] fileNames = GetFileNames(dir);
            if (fileNames.Length > 0)
            {
                return false;
            }

            // 判断是否存在文件夹
            string[] directoryNames = GetDirectories(dir);
            if (directoryNames.Length > 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 文件有没有被占用
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>true表示正在使用,false没有使用</returns>
        public static bool IsFileInUse(string fileName)
        {
            bool inUse;

            if (!File.Exists(fileName))
            {
                return false;
            }

            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                inUse = false;
            }
            catch
            {
                inUse = true;
            }
            finally
            {
                fs?.Close();
            }

            // true表示正在使用,false没有使用
            return inUse;
        }


        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        /// <returns>是否包含文件</returns>
        public static bool ContainFile(string dir, string searchPattern)
        {
            return ContainFile(dir, searchPattern, false);
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        /// <returns>是否包含文件</returns>
        public static bool ContainFile(string dir, string searchPattern, bool isSearchChild)
        {
            // 获取指定的文件列表
            string[] fileNames = GetFileNames(dir, searchPattern, isSearchChild);

            return fileNames.Length > 0;
        }

        /// <summary>
        /// 如果目录不存在，创建它。
        /// </summary>
        /// <param name="dir">要创建的目录路径包括目录名</param>
        public static void CreateDirectory(string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
                return;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// 在当前目录下创建目录。如希望在C:\xx\xxx 目录下创建子目录 aa\aaa\
        /// 调用该方法后会创建目录C:\xx\xxx\aa\aaa\
        /// </summary>
        /// <param name="orign">当前目录</param>
        /// <param name="newPath">新目录</param>
        public static void CreateDirectory(string orign, string newPath)
        {
            Directory.SetCurrentDirectory(orign);
            Directory.CreateDirectory(newPath);
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        /// <param name="recursive">是否递归删除子目录和文件</param>
        public static void DeleteDirectory(string dir, bool recursive = false)
        {
            if (string.IsNullOrWhiteSpace(dir))
                return;

            if (Directory.Exists(dir))
                Directory.Delete(dir, recursive);
        }

        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="dir">指定目录的绝对路径</param>
        public static void ClearDirectory(string dir)
        {
            if (IsExistDirectory(dir))
            {
                // 删除目录中所有的文件
                string[] fileNames = GetFileNames(dir);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }

                // 删除目录中所有的子目录
                string[] directoryNames = GetDirectories(dir);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }

        /// <summary>
        /// 复制文件夹(递归)
        /// </summary>
        /// <param name="source">源文件夹路径</param>
        /// <param name="target">目标文件夹路径</param>
        public static void CopyDirectory(string source, string target)
        {
            if (!Directory.Exists(source))
                return;

            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);

            // 获取所有子目录
            string[] directories = Directory.GetDirectories(source);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyDirectory(d, target + d.Substring(d.LastIndexOf(Path.DirectorySeparatorChar)));
                }
            }

            string[] files = Directory.GetFiles(source);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, target + s.Substring(s.LastIndexOf(Path.DirectorySeparatorChar)), true);
                }
            }
        }

        /// <summary>
        /// 获取目录大小
        /// </summary>
        /// <param name="dir">目录的绝对路径</param>
        /// <returns>目录大小</returns>
        public static long GetDirectorySize(string dir)
        {
            if (!Directory.Exists(dir))
                return 0;

            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dir);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }

            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectorySize(dis[i].FullName);
                }
            }

            return len;
        }
        #endregion

        #region 文件操作

        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="fileFullName">文件的绝对路径</param>    
        /// <returns>是否存在</returns>
        public static bool IsExistFile(string fileFullName)
        {
            return File.Exists(fileFullName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileFullName">要删除的文件路径和名称</param>
        public static void DeleteFile(string fileFullName)
        {
            if (File.Exists(fileFullName))
                File.Delete(fileFullName);
        }

        /// <summary>
        /// 移动文件(剪切--粘贴)
        /// </summary>
        /// <param name="source">要移动的文件的路径及全名(包括后缀)</param>
        /// <param name="target">文件移动到新的位置,并指定新的文件名</param>
        public static void MoveFile(string source, string target)
        {
            if (string.IsNullOrWhiteSpace(target)) return;
            CreateDirectory(target);

            if (File.Exists(source))
                File.Move(source, target);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="source">要复制的文件的路径已经全名(包括后缀)</param>
        /// <param name="target">目标位置,并指定新的文件名</param>
        /// <param name="overwrite">指示是否允许覆盖同名文件</param>
        public static void CopyFile(string source, string target, bool overwrite = true)
        {
            if (string.IsNullOrWhiteSpace(target))
                return;

            // 获取文件父目录
            string path = GetFilePath(target);
            CreateDirectory(path);

            if (File.Exists(source))
                File.Copy(source, target, overwrite);
        }

        #region 文本文件读写
        /// <summary>
        /// 读取指定文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns>文本内容</returns>
        public static string ReadTextFile(string path, string fileName)
        {
            return ReadTextFile(Path.Combine(path, fileName));
        }

        /// <summary>
        /// 读取指定文本文件
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <returns>文本内容</returns>
        public static string ReadTextFile(string fileFullName)
        {
            string text = null;

            // 假如不存在txt文件，那算球了.
            if (File.Exists(fileFullName))
            {
                using (StreamReader r = new StreamReader(fileFullName))
                {
                    text = r.ReadToEnd();
                }
            }

            return text;
        }

        /// <summary>
        /// 读取指定文本文件，得到所有行
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <returns>文本文件的所有行集合</returns>
        public static List<string> ReadTextFileAllLines(string path, string fileName)
        {
            return ReadTextFileAllLines(Path.Combine(path, fileName));
        }

        /// <summary>
        /// 读取指定文本文件，得到所有行
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <returns>文件行文本集合</returns>
        public static List<string> ReadTextFileAllLines(string fileFullName)
        {
            List<string> lines = new List<string>();

            // 假如不存在txt文件，那算球了.
            if (File.Exists(fileFullName))
            {
                using (StreamReader r = new StreamReader(fileFullName))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        line = line.Trim();
                        if (line.Length > 0)
                            lines.Add(line);
                    }
                }
            }

            return lines;
        }

        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="text">一行文本内容</param>
        public static void WriteTextFile(string path, string fileName, string text)
        {
            WriteTextFile(Path.Combine(path, fileName), text);
        }

        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <param name="text">一行文本内容</param>
        public static void WriteTextFile(string fileFullName, string text)
        {
            string parentPath = Path.GetDirectoryName(fileFullName);
            if (!Directory.Exists(parentPath))
            {
                // 假如不存在目录，创建它.
                Directory.CreateDirectory(parentPath);
            }

            using (FileStream fs = new FileStream(fileFullName, FileMode.OpenOrCreate))
            {
                using (StreamWriter w = new StreamWriter(fs))
                {
                    w.Write(text);
                }
            }
        }

        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="text">文本内容，多行</param>
        public static void WriteTextFileAllLines(string path, string fileName, List<string> text)
        {
            WriteTextFileAllLines(Path.Combine(path, fileName), text);
        }

        /// <summary>
        /// 写文本文件
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <param name="text">文本内容，多行</param>
        public static void WriteTextFileAllLines(string fileFullName, List<string> text)
        {
            string parentPath = Path.GetDirectoryName(fileFullName);
            if (!Directory.Exists(parentPath))
            {
                // 假如不存在目录，创建它.
                Directory.CreateDirectory(parentPath);
            }

            using (FileStream fs = new FileStream(fileFullName, FileMode.OpenOrCreate))
            {
                using (StreamWriter w = new StreamWriter(fs))
                {
                    text.ForEach(w.WriteLine);
                }
            }
        }

        /// <summary>
        /// 在文本文件末尾追加文本。如果文件不存在，创建文件。
        /// </summary>
        /// <param name="fileFullName">文件路径</param>
        /// <param name="text">文本内容</param>
        public static void AppendTextFile(string fileFullName, string text)
        {
            File.AppendAllText(fileFullName, text);
        }

        #endregion

        /// <summary>
        /// 获取文件名。如 XXX.XML
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <returns>文件名</returns>
        public static string GetFileName(string fileFullName)
        {
            return Path.GetFileName(fileFullName);
        }

        /// <summary>
        /// 获取文件名，不含扩展名。如 XXX
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <returns>文件名，不含扩展名</returns>
        public static string GetFileNameWithoutExtension(string fileFullName)
        {
            return Path.GetFileNameWithoutExtension(fileFullName);
        }

        /// <summary>
        /// 获取文件扩展名，包含句点。 如.XML 
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <returns>文件扩展名</returns>
        public static string GetFileExtension(string fileFullName)
        {
            return Path.GetExtension(fileFullName);
        }

        /// <summary>
        /// 获取文件所在目录。如 C:\xx\xxx\
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <returns>文件路径</returns>
        public static string GetFilePath(string fileFullName)
        {
            return Path.GetDirectoryName(fileFullName);
        }

        /// <summary>
        /// 获取已存在文件大小
        /// </summary>
        /// <param name="fileFullName">文件全名</param>
        /// <returns>文件大小</returns>
        public static long GetFileSize(string fileFullName)
        {
            return File.Exists(fileFullName) ? new FileInfo(fileFullName).Length : 0;
        }

        #endregion
    }
}
