using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraTab;
using Newtonsoft.Json.Linq;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraGrid.Columns;

namespace AKRS.ZX2200.Localization
{
    public static class FormLocalizer
    {
        private static readonly Dictionary<Type, string> _defaultPropertyMap = new Dictionary<Type, string>();

        static FormLocalizer()
        {
            InitializeDefaultPropertyMap();
        }

        /// <summary>
        /// 初始化控件类型的默认文本属性映射
        /// </summary>
        private static void InitializeDefaultPropertyMap()
        {
            // WinForm 控件默认属性
            _defaultPropertyMap[typeof(Form)] = "Text";
            _defaultPropertyMap[typeof(Button)] = "Text";
            _defaultPropertyMap[typeof(Label)] = "Text";
            _defaultPropertyMap[typeof(CheckBox)] = "Text";
            _defaultPropertyMap[typeof(RadioButton)] = "Text";
            _defaultPropertyMap[typeof(GroupBox)] = "Text";
            _defaultPropertyMap[typeof(TabPage)] = "Text";
            _defaultPropertyMap[typeof(DataGridViewTextBoxColumn)] = "HeaderText";
            _defaultPropertyMap[typeof(DataGridViewCheckBoxColumn)] = "HeaderText";

            // DevExpress 控件默认属性
            _defaultPropertyMap[typeof(BarButtonItem)] = "Caption";
            _defaultPropertyMap[typeof(BarStaticItem)] = "Caption";
            _defaultPropertyMap[typeof(BarEditItem)] = "Caption";
            _defaultPropertyMap[typeof(BarSubItem)] = "Caption";
            _defaultPropertyMap[typeof(RibbonPage)] = "Text";
            _defaultPropertyMap[typeof(RibbonPageGroup)] = "Text";
            _defaultPropertyMap[typeof(SimpleButton)] = "Text";
            _defaultPropertyMap[typeof(LabelControl)] = "Text";
            _defaultPropertyMap[typeof(CheckEdit)] = "Text";
            _defaultPropertyMap[typeof(RadioGroup)] = "Text";
            _defaultPropertyMap[typeof(GroupControl)] = "Text";
            _defaultPropertyMap[typeof(XtraTabPage)] = "Text";
            _defaultPropertyMap[typeof(TileBarItem)] = "Text";
            _defaultPropertyMap[typeof(GridColumn)] = "Caption";
        }

        /// <summary>
        /// 本地化窗体
        /// </summary>
        /// <param name="form">要本地化的窗体</param>
        public static void LocalizeForm(Control form)
        {
            if (form == null)
                return;

            string formName = form.GetType().Name;

            try
            {
                LocalizationCache.Instance.Initialize(formName);
                (form as ILanguage)?.GetUI();

                if (!LocalizationManager.HasFormTranslation(formName))
                {
                   // System.Diagnostics.Debug.WriteLine($"窗体 {formName} 无翻译文件，跳过本地化");
                    return;
                }

                LocalizeFormByTranslationKeys(form, formName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"窗体本地化失败: {formName}, 错误: {ex.Message}");
            }

           // System.Diagnostics.Debug.WriteLine($"窗体本地化完成: {formName}");
        }

        /// <summary>
        /// 本地化
        /// </summary>
        private static void LocalizeFormByTranslationKeys(Control form, string formName)
        {
            var formTranslations = LocalizationCache.Instance._formCache.ContainsKey(formName)
                ? LocalizationCache.Instance._formCache[formName]
                : null;

            if (formTranslations == null)
                return;

            foreach (var kvp in formTranslations)
            {
                string controlKey = kvp.Key;
                JToken translationValue = kvp.Value;

                if (translationValue is JObject translationObj)
                {
                    object component = FindComponentByKey(form, controlKey);
                    if (component != null)
                    {
                        ApplyTranslationToComponent(component, translationObj, controlKey);
                    }
                }
            }
        }

        /// <summary>
        /// 根据控件键查找控件
        /// </summary>
        private static object FindComponentByKey(Control container, string controlKey)
        {
            try
            {
                // 支持层级路径：panel1.button1
                if (controlKey.Contains('.'))
                {
                    string[] path = controlKey.Split('.');
                    object current = container; // 修改点: 使用 object 类型

                    foreach (string name in path)
                    {
                        current = FindDirectChild(current, name);
                        if (current == null)
                        {
                            System.Diagnostics.Debug.WriteLine($"层级查找失败: 在 {path[0]} 中未找到 {name}");
                            break;
                        }
                    }

                    return current;
                }
                else
                {
                    return FindDirectChild(container, controlKey);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"查找控件失败: {controlKey}, 错误: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 查找直接子组件
        /// </summary>
        private static object FindDirectChild(object parent, string name)
        {
            if (parent == null || string.IsNullOrEmpty(name))
                return null;

            // 反射
            const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var field = parent.GetType().GetField(name, flags);
            if (field != null)
            {
                return field.GetValue(parent);
            }

            var property = parent.GetType().GetProperty(name, flags);
            if (property != null)
            {
                return property.GetValue(parent);
            }

            // DevExpress 特殊容器处理 (查找 Items 集合)
            if (parent is TileBar tileBar)
            {
                foreach (TileBarGroup group in tileBar.Groups)
                {
                    foreach (TileBarItem item in group.Items)
                    {
                        if (item.Name == name)
                            return item;
                    }
                }
            }
            if (parent is BarManager barManager)
            {
                foreach (BarItem item in barManager.Items)
                {
                    if (item.Name == name) return item;
                }
            }
            if (parent is RibbonControl ribbon)
            {
                foreach (BarItem item in ribbon.Items)
                {
                    if (item.Name == name) return item;
                }
            }

            // 备选
            if (parent is TreeView treeView)
            {
                foreach (TreeNode treeNode in treeView.Nodes)
                {
                    if (treeNode.Name == name) return treeNode;
                }
            }
            else if (parent is Control parentControl)
            {
                foreach (Control child in parentControl.Controls)
                {
                    if (child.Name == name) return child;
                }
            }

            return null;
        }

        /// <summary>
        /// 应用翻译到组件
        /// </summary>
        private static void ApplyTranslationToComponent(object component, JObject translationObj, string componentKey)
        {
            try
            {
                string currentLanguage = LanguageSettings.GetInstance().Language.ToString();

                // 优先应用 JSON 中指定的 "Property"
                string targetProperty = translationObj["Property"]?.ToString() ?? GetDefaultProperty(component.GetType());

                if (translationObj[currentLanguage] is JValue valueToken)
                {
                    string translatedText = valueToken.Value<string>();
                    if (!string.IsNullOrEmpty(translatedText))
                    {
                        SetComponentProperty(component, targetProperty, translatedText);
                        // System.Diagnostics.Debug.WriteLine($"本地化组件: {component.GetType().Name}.{componentKey} -> {targetProperty} = '{translatedText}'");
                    }
                }

                // 应用ToolTipText (适用于 BarItem 等)
                var toolTipProperty = component.GetType().GetProperty("Hint"); // DevExpress Hint 属性
                if (translationObj["ToolTipText"] is JObject toolTipObj && toolTipProperty != null)
                {
                    string toolTipText = GetTranslationValue(toolTipObj, currentLanguage);
                    if (!string.IsNullOrEmpty(toolTipText))
                    {
                        SetComponentProperty(component, "Hint", toolTipText);
                    }
                }

                // 应用其他属性
                if (translationObj["Properties"] is JToken propertiesToken)
                {
                    ApplyAdditionalProperties(component, propertiesToken, currentLanguage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"应用翻译失败: {componentKey}, 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 从翻译对象获取指定语言的值
        /// </summary>
        private static string GetTranslationValue(JObject translationObj, string language)
        {
            if (translationObj == null)
                return null;

            if (translationObj.TryGetValue(language, StringComparison.OrdinalIgnoreCase, out JToken token) && token is JValue value)
            {
                return value.Value<string>();
            }

            return null;
        }

        /// <summary>
        /// 应用额外属性
        /// </summary>
        private static void ApplyAdditionalProperties(object component, JToken propertiesToken, string currentLanguage)
        {
            if (propertiesToken is JObject propertiesObj)
            {
                foreach (var property in propertiesObj)
                {
                    string propertyName = property.Key;
                    if (property.Value is JObject propertyTranslationObj)
                    {
                        string propertyValue = GetTranslationValue(propertyTranslationObj, currentLanguage);
                        if (!string.IsNullOrEmpty(propertyValue))
                        {
                            SetComponentProperty(component, propertyName, propertyValue);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取控件的默认文本属性
        /// </summary>
        private static string GetDefaultProperty(Type componentType)
        {
            if (componentType == null)
                return "Text";

            if (_defaultPropertyMap.TryGetValue(componentType, out string property))
            {
                return property;
            }

            // 检查基类
            foreach (var kvp in _defaultPropertyMap)
            {
                if (kvp.Key.IsAssignableFrom(componentType))
                {
                    return kvp.Value;
                }
            }

            return "Text"; // 默认属性
        }

        /// <summary>
        /// 设置组件属性
        /// </summary>
        private static void SetComponentProperty(object component, string propertyName, object value)
        {
            if (component == null) return;
            try
            {
                PropertyInfo property = component.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (property != null && property.CanWrite)
                {
                    object convertedValue = ConvertValue(value, property.PropertyType);
                    property.SetValue(component, convertedValue);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"属性未找到或不可写: {component.GetType().Name}.{propertyName}");
                }
            }
            catch (Exception ex)
            {
                string componentName = component.GetType().GetProperty("Name")?.GetValue(component)?.ToString() ?? "Unknown";
                System.Diagnostics.Debug.WriteLine($"设置组件属性失败: {componentName}.{propertyName} = {value}, 错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 值类型转换
        /// </summary>
        private static object ConvertValue(object value, Type targetType)
        {
            if (value == null)
                return null;

            if (targetType.IsAssignableFrom(value.GetType()))
                return value;

            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// 遍历翻译项，根据key查找控件
        /// </summary>
        private static void LocalizeControlByTranslationKeys(Control container, string controlName)
        {
            var controlTranslations = LocalizationCache.Instance._formCache.ContainsKey(controlName)
                ? LocalizationCache.Instance._formCache[controlName]
                : null;

            if (controlTranslations == null)
                return;

            foreach (var kvp in controlTranslations)
            {
                string controlKey = kvp.Key;
                JToken translationValue = kvp.Value;

                if (translationValue is JObject translationObj)
                {
                    object component = FindComponentByKey(container, controlKey);
                    if (component != null)
                    {
                        ApplyTranslationToComponent(component, translationObj, controlKey);
                    }
                }
            }
        }
    }
}
