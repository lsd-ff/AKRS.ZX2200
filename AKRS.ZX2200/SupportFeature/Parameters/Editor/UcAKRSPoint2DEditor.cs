using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.SupportFeature.Parameters.Editor;

[ParamEditor(typeof(AKRSPoint2D))]
public partial class UcAKRSPoint2DEditor : UcBaseEditor
{
    public UcAKRSPoint2DEditor()
    {
        this.InitializeComponent();
    }


    protected override void Binding()
    {
        if (this.Instance == null)
        {
            return;
        }

        try
        {
            this.spinEdit1.Binding(this.Instance, this.PropertyInfo[0].Name);
            this.spinEdit2.Binding(this.Instance, this.PropertyInfo[1].Name);
        }
        catch (Exception e)
        {
            return;
        }
    }
}