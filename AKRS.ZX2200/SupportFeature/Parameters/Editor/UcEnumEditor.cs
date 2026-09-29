using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.SupportFeature.Parameters.Editor;

[ParamEditor(typeof(Enum))]
public partial class UcEnumEditor : UcBaseEditor
{
    public UcEnumEditor()
    {
        this.InitializeComponent();
    }

    protected override void Binding()
    {
        Array items = this.PropertyInfo[0].GetValue(this.Instance).GetType().GetEnumValues();
        this.radioGroup1.Properties.Items.Clear();
        foreach (object item in items)
        {
            object enumValue = item;
            string description = ((Enum)item).GetDescription();
            this.radioGroup1.Properties.Items.Add(new DevExpress.XtraEditors.Controls.RadioGroupItem(enumValue, description));
        }

        this.radioGroup1.Binding(this.Instance, this.PropertyInfo[0].Name);
    }
}