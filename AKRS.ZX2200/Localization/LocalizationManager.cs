using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Localization
{
    using System.ComponentModel;

    using AKRS.Galaxy2.Infrastructure.Helper;

    public static class LocalizationManager
    {
        private static readonly object _lockObject = new object();
        private static bool _isInitialized = false;

        /// <summary>
        /// 初始化国际化管理器，可指定首次需要加载的窗体
        /// </summary>
        /// <param name="initialFormNames">首次需要加载的窗体名称列表，如果为空则只加载通用翻译</param>
        public static void Initialize(params string[] initialFormNames)
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                {
                    // 初始化缓存并加载指定的窗体翻译
                    LocalizationCache.Instance.Initialize(initialFormNames);
                    
                    System.Diagnostics.Debug.WriteLine($"LocalizationManager 已初始化，当前语言: {LanguageSettings.GetInstance().Language.GetDescription()}");
                                 
                    _isInitialized = true;
                }
            }
        }

        /// <summary>
        /// 确保已初始化
        /// </summary>
        private static void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                Initialize(); // 使用默认初始化
            }
        }

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="language">新语言代码</param>
        public static void ChangeLanguage(LanguageEnum language)
        {
            if (LanguageSettings.GetInstance().Language != language)
            {
                LanguageSettings.GetInstance().Language = language;
                LanguageSettings.GetInstance().Save();
            }
        }

        /// <summary>
        /// 获取UI文本 - 精确查找窗体翻译
        /// </summary>
        public static string GetUI(string formName, string controlName)
        {
            EnsureInitialized();
            return GetFormTranslation(formName, controlName);
        }

        /// <summary>
        /// 获取消息文本 - 精确查找消息翻译
        /// </summary>
        public static string GetMessage(string category, string messageKey)
        {
            EnsureInitialized();
            return GetCategoryTranslation(category, messageKey);
        }

        /// <summary>
        /// 获取窗体翻译 - 只查询窗体JSON数据
        /// </summary>
        private static string GetFormTranslation(string formName, string key)
        {
            if (string.IsNullOrEmpty(formName) || string.IsNullOrEmpty(key))
                return null;

            try
            {
                string translation = LocalizationCache.Instance.GetFormTranslation(formName, key, LanguageSettings.GetInstance().Language.ToString());
                return translation; // 查询不到返回null，不做任何处理
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取窗体翻译失败 - 窗体: {formName}, 键: {key}, 错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取分类翻译 - 只查询指定分类的JSON数据
        /// </summary>
        private static string GetCategoryTranslation(string category, string key)
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(key))
                return null;

            try
            {
                string translation = LocalizationCache.Instance.GetMessageTranslation(category, key, LanguageSettings.GetInstance().Language.ToString());
                return translation; // 查询不到返回null，不做任何处理
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取分类翻译失败 - 分类: {category}, 键: {key}, 错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 获取翻译数据
        /// </summary>
        public static TranslationData GetTranslationData(string formName, string controlName)
        {
            EnsureInitialized();
            return LocalizationCache.Instance.GetTranslationData(formName, controlName, LanguageSettings.GetInstance().Language.ToString());
        }

        /// <summary>
        /// 检查窗体是否有翻译文件
        /// </summary>
        public static bool HasFormTranslation(string formName)
        {
            EnsureInitialized();
            return LocalizationCache.Instance.HasFormTranslation(formName);
        }


    }

    /// <summary>
    /// 翻译数据结构
    /// </summary>
    public class TranslationData
    {
        public string Text { get; set; }
        public string ToolTipText { get; set; }
        public string Properties { get; set; }

        public TranslationData()
        {
            Text = string.Empty;
            ToolTipText = string.Empty;
            Properties = string.Empty;
        }
    }

    public enum LanguageEnum
    {
        [Description("中文")]
        Chinese = 0,

        [Description("英文")]
        English = 1,
    }

    public interface ILanguage
    {
        void GetUI();
    }
}
