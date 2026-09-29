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
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.SupportFeature.Parameters.Editor;

[ParamEditor(typeof(bool))]
public partial class UcBooleanEditor : UcBaseEditor
{
    public UcBooleanEditor()
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
            this.toggleSwitch1.Binding(this.Instance, this.PropertyInfo[0].Name);
        }
        catch (Exception e)
        {
            return;
        }
    }
}