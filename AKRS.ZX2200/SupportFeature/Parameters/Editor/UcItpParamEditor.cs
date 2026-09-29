using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.SupportFeature.Parameters.Editor;

/// <summary>
/// 插补参数编辑器
/// </summary>
[ParamEditor(typeof(ItpParam))]
public partial class UcItpParamEditor : UcBaseEditor
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public UcItpParamEditor()
    {
        this.InitializeComponent();
    }

    protected override void Binding()
    {
        this.BindingControl();
    }

    private void BindingControl()
    {
        if (this.Instance is not ItpParam entity)
        {
            return;
        }

        this.spinEdit7.Binding(entity.TargetPoint, nameof(AKRSPoint3D.X));
        this.spinEdit8.Binding(entity.TargetPoint, nameof(AKRSPoint3D.Y));
        this.spinEdit9.Binding(entity.TargetPoint, nameof(AKRSPoint3D.Z));
        this.spinEdit1.Binding(entity, nameof(ItpParam.Speed));
        this.spinEdit2.Binding(entity, nameof(ItpParam.AccTime));
        this.spinEdit3.Binding(entity, nameof(ItpParam.JerkTime));
        this.spinEdit4.Binding(entity, nameof(ItpParam.AheadTime));
        this.spinEdit5.Binding(entity, nameof(ItpParam.AheadRadiusRatio));
    }
}