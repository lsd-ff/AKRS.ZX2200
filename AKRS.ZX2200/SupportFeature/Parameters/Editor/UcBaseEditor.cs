using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.SupportFeature.Parameters.Models;
using DevExpress.XtraEditors;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Parameters.Editor;

/// <summary>
/// 参数编辑器基类
/// </summary>
public partial class UcBaseEditor : XtraUserControl, IParamEditor
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public UcBaseEditor()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// 获取或设置编辑器的容器控件。
    /// </summary>
    /// <remarks>
    /// 此属性继承自 <see cref="IParamEditor.EditorContainer"/>。
    /// </remarks>
    public Control EditorContainer { get; set; }

    /// <summary>
    /// 参数名称
    /// </summary>
    /// <remarks>
    /// 此属性继承自 <see cref="IParamEditor.ParamName"/>。
    /// </remarks>
    public string ParamName { get; set; }

    /// <summary>
    /// 是否是值类型或字符串类型集合
    /// </summary>
    /// <remarks>
    /// 此属性继承自 <see cref="IParamEditor.IsValueTypeCollection"/>。
    /// </remarks>
    public bool IsValueTypeCollection { get; set; } = false;

    /// <summary>
    /// 集合元素索引
    /// </summary>
    public int ElementIndex { get; set; } = -1;

    /// <summary>
    /// 元素的值
    /// </summary>
    public object ElementValue { get; set; }

    /// <summary>
    /// 参数所属类的属性信息
    /// </summary>
    /// <remarks>
    /// 此属性继承自 <see cref="IParamEditor.PropertyInfo"/>。
    /// </remarks>
    public PropertyInfo[] PropertyInfo { get; set; }

    /// <summary>
    /// 参数实例/所属类实例
    /// </summary>
    /// <remarks>
    /// 此属性继承自 <see cref="IParamEditor.Instance"/>。
    /// </remarks>
    public object Instance { get; set; }

    /// <summary>
    /// 参数实例/所属类实例
    /// </summary>
    /// <remarks>
    /// 此方法继承自 <see cref="IParamEditor.ShowEditor()"/>。
    /// </remarks>
    public void ShowEditor()
    {
        if (this.EditorContainer == null)
        {
            return;
        }

        this.EditorContainer.Controls.Clear();
        this.Dock = DockStyle.Fill;

        try
        {
            switch (this.IsValueTypeCollection)
            {
                case true when this.Instance != null:
                    this.ElementValue = ((IList)this.Instance)[this.ElementIndex];
                    this.GetChildControls<BaseEdit>()[0].Binding(this, nameof(this.ElementValue));
                    break;
                case false when this.Instance != null:
                    this.Binding();
                    break;
            }

            StringBuilder sb = new StringBuilder(this.ParamName);
            // if (this.ParamNodeModel.MaxValue != 0)
            // {
            //     sb.Append("-");
            //     sb.Append($"[{this.ParamNodeModel.MinValue}~{this.ParamNodeModel.MaxValue}]");
            // }

            if (!string.IsNullOrEmpty(this.ParamNodeModel.Unit))
            {
                sb.Append("-");
                sb.Append(this.ParamNodeModel.Unit);
            }

            this.GetChildControls<GroupControl>()[0].Text = sb.ToString();
            this.GetChildControls<SpinEdit>().ForEach(e =>
            {
                e.Properties.MaxValue = (decimal)this.ParamNodeModel.MaxValue;
                e.Properties.MinValue = (decimal)this.ParamNodeModel.MinValue;
            });
        }
        catch (Exception e)
        {
            AKRSXtraMessageBox.Show(e.Message, "参数编辑器异常提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        this.Parent = this.EditorContainer;
        this.SubscribeEditValueChanged();
    }

    /// <summary>
    /// 绑定数据源
    /// </summary>
    protected virtual void Binding()
    {
    }

    /// <summary>
    /// 隐藏编辑器
    /// </summary>
    /// <remarks>
    /// 此方法继承自 <see cref="IParamEditor.HideEditor()"/>。
    /// </remarks>
    public void HideEditor()
    {
        this.EditorContainer.Controls.Clear();
        this.UnSubscribeEditValueChanged();
    }

    /// <summary>
    /// 订阅编辑器值更改事件
    /// </summary>
    private void SubscribeEditValueChanged()
    {
        this.GetChildControls<BaseEdit>().ToList().ForEach(c => { c.EditValueChanged += this.BaseEditValueChanged; });
    }

    /// <summary>
    /// 取消订阅编辑器值更改事件
    /// </summary>
    private void UnSubscribeEditValueChanged()
    {
        this.GetChildControls<BaseEdit>().ToList().ForEach(c => { c.EditValueChanged -= this.BaseEditValueChanged; });
    }

    /// <summary>
    /// 编辑器值更改事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BaseEditValueChanged(object sender, EventArgs e)
    {
        this.ValueChanged?.Invoke(((BaseEdit)sender).EditValue);
    }

    /// <summary>
    /// 隐藏编辑器
    /// </summary>
    /// <remarks>
    /// 此事件继承自 <see cref="IParamEditor.ValueChanged"/>。
    /// </remarks>
    public event Action<object> ValueChanged;

    /// <summary>
    /// 参数节点模型
    /// </summary>
    /// <remarks>
    /// 此属性继承自 <see cref="IParamEditor.ParamNodeModel"/>。
    /// </remarks>
    public ParameterDetailModel ParamNodeModel { get; set; }
}