using AKRS.Galaxy2.PR.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Localization
{
    public class LocalizationCache
    {
        private static readonly Lazy<LocalizationCache> _instance = new Lazy<LocalizationCache>(() => new LocalizationCache());
        public static LocalizationCache Instance => _instance.Value;

        public readonly Dictionary<string, JObject> _formCache;
        public readonly Dictionary<string, JObject> _commonCache;
        public readonly Dictionary<string, JObject> _messageCache; //用于报警、提示等信息的翻译缓存
        private readonly HashSet<string> _loadedForms;
        private readonly HashSet<string> _nonExistentForms;
        private readonly HashSet<string> _loadedMessages;
        private readonly HashSet<string> _nonExistentMessages;

        private readonly string _translationBasePath;

        private LocalizationCache()
        {
            _formCache = new Dictionary<string, JObject>();
            _commonCache = new Dictionary<string, JObject>();
            _messageCache = new Dictionary<string, JObject>();
            _loadedForms = new HashSet<string>();
            _nonExistentForms = new HashSet<string>();
            _loadedMessages = new HashSet<string>();
            _nonExistentMessages = new HashSet<string>();
            
            _translationBasePath = GetTranslationBasePath();
        }

        /// <summary>
        /// 初始化缓存，可指定首次需要加载的窗体
        /// </summary>
        /// <param name="initialFormNames">首次需要加载的窗体名称列表</param>
        public void Initialize(params string[] initialFormNames)
        {
            // System.Diagnostics.Debug.WriteLine($"开始初始化翻译缓存，路径: {_translationBasePath}");

            // 1. 始终加载通用翻译
            LoadCommonTranslations();

            // 2. 加载指定的窗体翻译
            if (initialFormNames != null)
            {
                foreach (string formName in initialFormNames)
                {
                    if (!string.IsNullOrEmpty(formName))
                    {
                        LoadFormTranslation(formName);
                    }
                }
            }

            // System.Diagnostics.Debug.WriteLine($"翻译缓存初始化完成: 窗体缓存={_formCache.Count}, 通用缓存={_commonCache.Count}");
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        public void ClearCache()
        {
            _formCache.Clear();
            _commonCache.Clear();
            _messageCache.Clear();
            _loadedForms.Clear();
            _nonExistentForms.Clear();
            System.Diagnostics.Debug.WriteLine("翻译缓存已清理");
        }

        /// <summary>
        /// 检查窗体是否有翻译文件
        /// </summary>
        public bool HasFormTranslation(string formName)
        {
            if (string.IsNullOrEmpty(formName))
                return false;

            // 如果已经确认不存在，直接返回false
            if (_nonExistentForms.Contains(formName))
                return false;

            // 如果已经加载过，说明存在
            if (_loadedForms.Contains(formName))
                return true;

            // 检查文件是否存在
            string filePath = Path.Combine(_translationBasePath, "Forms", $"{formName}.json");
            bool exists = File.Exists(filePath);
            
            if (!exists)
            {
                _nonExistentForms.Add(formName);
            }

            return exists;
        }

        /// <summary>
        /// 获取窗体翻译
        /// </summary>
        public string GetFormTranslation(string formName, string key, string language)
        {
            if (string.IsNullOrEmpty(formName) || string.IsNullOrEmpty(key))
                return null;

            // 按需加载窗体翻译
            EnsureFormLoaded(formName);

            if (_formCache.TryGetValue(formName, out JObject formTranslations))
            {
                return GetTranslationFromJObject(formTranslations, key, language);
            }

            return null;
        }

        /// <summary>
        /// 获取通用翻译
        /// </summary>
        public string GetCommonTranslation(string category, string key, string language)
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(key))
                return null;

            if (_commonCache.TryGetValue(category, out JObject commonTranslations))
            {
                return GetTranslationFromJObject(commonTranslations, key, language);
            }

            return null;
        }

        /// <summary>
        /// 获取Message(提示、报警等)翻译
        /// </summary>
        public string GetMessageTranslation(string category, string key, string language)
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(key))
                return null;

            // 按需加载Message翻译
            EnsureMessageLoaded(category);

            if (_messageCache.TryGetValue(category, out JObject messageTranslations))
            {
                return GetTranslationFromJObject(messageTranslations, key, language);
            }

            return null;
        }

        /// <summary>
        /// 获取翻译数据 - 只查询窗体翻译，不做降级处理
        /// </summary>
        public TranslationData GetTranslationData(string formName, string key, string language)
        {
            TranslationData data = new TranslationData();

            // 只从窗体翻译获取，查询不到就返回空数据
            JObject translationObj = GetTranslationObject(formName, key);
            
            if (translationObj != null)
            {
                // 获取文本 - 只查询指定语言
                string translatedText = GetValueFromJObject(translationObj, language);
                if (!string.IsNullOrEmpty(translatedText))
                {
                    data.Text = translatedText;
                }
                // 查询不到就保持为空
                
                // 获取ToolTipText
                if (translationObj["ToolTipText"] is JObject toolTipObj)
                {
                    string toolTipText = GetValueFromJObject(toolTipObj, language);
                    if (!string.IsNullOrEmpty(toolTipText))
                    {
                        data.ToolTipText = toolTipText;
                    }
                }

                // 获取Properties
                if (translationObj["Properties"] is JToken propertiesToken)
                {
                    data.Properties = propertiesToken.ToString();
                }
            }
            // 查询不到translationObj就返回空的TranslationData

            return data;
        }

        /// <summary>
        /// 获取翻译文件基础路径
        /// </summary>
        private string GetTranslationBasePath()
        {
            string projectPath = GetProjectRootPath();
            if (!string.IsNullOrEmpty(projectPath))
            {
                string devPath = Path.Combine(projectPath, "Localization", "Data");
                if (Directory.Exists(devPath))
                {
                    System.Diagnostics.Debug.WriteLine($"使用项目翻译路径: {devPath}");
                    return devPath;
                }
            }

            string binPath = Path.Combine(Application.StartupPath, "Localization", "Data");
            System.Diagnostics.Debug.WriteLine($"使用发布翻译路径: {binPath}");
            return binPath;
        }

        /// <summary>
        /// 获取项目根目录路径
        /// </summary>
        private string GetProjectRootPath()
        {
            try
            {
                string currentPath = Application.StartupPath;
                DirectoryInfo dir = new DirectoryInfo(currentPath);

                // 向上查找，寻找包含.csproj或.sln文件的目录
                while (dir != null && dir.Parent != null)
                {
                    if (dir.GetFiles("*.csproj").Length > 0 || dir.GetFiles("*.sln").Length > 0)
                    {
                        // System.Diagnostics.Debug.WriteLine($"找到项目根目录: {dir.FullName}");
                        return dir.FullName;
                    }
                    dir = dir.Parent;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找项目根目录失败: {ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 确保窗体翻译已加载
        /// </summary>
        private void EnsureFormLoaded(string formName)
        {
            if (!_loadedForms.Contains(formName) && !_nonExistentForms.Contains(formName))
            {
                LoadFormTranslation(formName);
            }
        }

        /// <summary>
        /// 加载窗体翻译
        /// </summary>
        private void LoadFormTranslation(string formName)
        {
            try
            {
                string filePath = Path.Combine(_translationBasePath, "Forms", $"{formName}.json");
                
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath, Encoding.UTF8);
                    JObject translations = JObject.Parse(content);
                    
                    _formCache[formName] = translations;
                    _loadedForms.Add(formName);
                    
                    // System.Diagnostics.Debug.WriteLine($"已加载窗体翻译: {formName} ({translations.Count} 项)");
                }
                else
                {
                    _nonExistentForms.Add(formName);
                    System.Diagnostics.Debug.WriteLine($"窗体翻译文件不存在: {formName}");
                }
            }
            catch (Exception ex)
            {
                _nonExistentForms.Add(formName);
                System.Diagnostics.Debug.WriteLine($"加载窗体翻译失败: {formName}, 错误: {ex.Message}");
            }
        }
        /// <summary>
        /// 确保Message翻译已加载
        /// </summary>
        private void EnsureMessageLoaded(string messageName)
        {
            if (!_loadedMessages.Contains(messageName) && !_nonExistentMessages.Contains(messageName))
            {
                LoadMessageTranslation(messageName);
            }
        }

        /// <summary>
        /// 加载Message翻译
        /// </summary>
        private void LoadMessageTranslation(string messageName)
        {
            try
            {
                string filePath = Path.Combine(_translationBasePath, "Messages", $"{messageName}.json");
                
                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath, Encoding.UTF8);
                    JObject translations = JObject.Parse(content);
                    
                    _messageCache[messageName] = translations;
                    _loadedMessages.Add(messageName);
                    
                    // System.Diagnostics.Debug.WriteLine($"已加载Message翻译: {messageName} ({translations.Count} 项)");
                }
                else
                {
                    _nonExistentMessages.Add(messageName);
                    System.Diagnostics.Debug.WriteLine($"Message翻译文件不存在: {messageName}");
                }
            }
            catch (Exception ex)
            {
                _nonExistentMessages.Add(messageName);
                System.Diagnostics.Debug.WriteLine($"加载Message翻译失败: {messageName}, 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 加载通用翻译
        /// </summary>
        private void LoadCommonTranslations()
        {
            string[] commonFiles = { "Common_UI", "Common_Messages" };
            
            foreach (string fileName in commonFiles)
            {
                try
                {
                    string filePath = Path.Combine(_translationBasePath, "Common",$"{fileName}.json");
                    
                    if (File.Exists(filePath))
                    {
                        string content = File.ReadAllText(filePath, Encoding.UTF8);
                        JObject translations = JObject.Parse(content);
                        
                        _commonCache[fileName] = translations;
                        // System.Diagnostics.Debug.WriteLine($"已加载通用翻译: {fileName} ({translations.Count} 项)");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"通用翻译文件不存在: {fileName}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"加载通用翻译失败: {fileName}, 错误: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// 从JObject中获取翻译
        /// </summary>
        private string GetTranslationFromJObject(JObject obj, string key, string language)
        {
            if (obj?[key] is JObject translationObj)
            {
                return GetValueFromJObject(translationObj, language);
            }
            return null;
        }

        /// <summary>
        /// 从翻译对象中获取指定语言的值 - 只查询指定语言，不做降级处理
        /// </summary>
        private string GetValueFromJObject(JObject obj, string language)
        {
            if (obj == null || string.IsNullOrEmpty(language))
                return null;

            // 只返回指定语言的翻译
            if (obj[language]?.ToString() is string value && !string.IsNullOrEmpty(value))
            {
                return value;
            }

            // 查询不到返回null，不做任何处理
            return null;
        }

        /// <summary>
        /// 获取翻译对象
        /// </summary>
        private JObject GetTranslationObject(string formName, string key)
        {
            EnsureFormLoaded(formName);
            
            if (_formCache.TryGetValue(formName, out JObject formTranslations))
            {
                return formTranslations[key] as JObject;
            }
            
            return null;
        }

        /// <summary>
        /// 获取通用翻译对象
        /// </summary>
        private JObject GetCommonTranslationObject(string category, string key)
        {
            if (_commonCache.TryGetValue(category, out JObject commonTranslations))
            {
                return commonTranslations[key] as JObject;
            }
            
            return null;
        }
    }
}



