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

[ParamEditor(typeof(string))]
public partial class UcTextEditor : UcBaseEditor
{
    public UcTextEditor()
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
            this.memoEdit1.Binding(this.Instance, this.PropertyInfo[0].Name);
        }
        catch (Exception e)
        {
            return;
        }
    }
}