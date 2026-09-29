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
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.SupportFeature.Parameters.Editor;

[ParamEditor(typeof(AKRSPoint3D))]
public partial class UcAKRSPoint3DEditor : UcBaseEditor
{
    public UcAKRSPoint3DEditor()
    {
        this.InitializeComponent();
    }


    protected override void Binding()
    {
        if (this.Instance == null)
        {
            return;
        }

        this.panelControl1.Visible = this.ParamNodeModel.BindingConfigs.Count != 0;

        try
        {
            this.spinEdit1.Binding(this.Instance, nameof(AKRSPoint3D.X));
            this.spinEdit2.Binding(this.Instance, nameof(AKRSPoint3D.Y));
            this.spinEdit3.Binding(this.Instance, nameof(AKRSPoint3D.Z));
        }
        catch (Exception e)
        {
            return;
        }
    }

    private void simpleButton1_Click(object sender, EventArgs e)
    {
        List<ParamAxesConfig> bindingConfig = this.ParamNodeModel.BindingConfigs;

        foreach (ParamAxesConfig config in bindingConfig)
        {
            Axis axis = HardwareRepositoryService.GetHardware<Axis>(config.AxisName);
            if (axis == null)
            {
                continue;
            }

            string propertyName = config.PropertyName;
            PropertyInfo propertyInfo = this.PropertyInfo.FirstOrDefault(p => p.Name == propertyName);
            if (propertyInfo == null)
            {
                continue;
            }

            double pos = axis.GetRealPosition();
            propertyInfo.SetValue(this.Instance, pos);
        }
    }
}