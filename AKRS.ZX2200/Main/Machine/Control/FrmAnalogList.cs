using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Main.Machine.Control
{
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.WaferSubSystem.Modules;

    /// <summary>
    /// 模拟量列表窗体
    /// </summary>
    public partial class FrmAnalogList : XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmAnalogList()
        {
            InitializeComponent();
        }

        /// <summary>
        ///  模拟量列表
        /// </summary>
        private List<Sensor> list = new List<Sensor>();

        /// <summary>
        /// bond模组
        /// </summary>
        private BondModule bondModule => System2Module.GetInstance().BondModule;



        private void Init()
        {
            list.Clear();

            // 吸嘴漏晶检测
            this.list.Add(bondModule.BondHead.CheckVaccumSensor);

            // 焊头吸附检测
            this.list.Add(bondModule.BondHead.CheckToolSensor);

            // 顶针负压检测
            this.list.Add(WaferSubModule.GetInstance().Eject.EjectionVacuumDetectSensor);



        }
    }
}
