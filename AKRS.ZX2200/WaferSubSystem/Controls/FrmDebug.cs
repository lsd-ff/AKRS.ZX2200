using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach;
using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.MagazineTeach;

namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.LeadShine.E5032;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.WM;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.CalibSystem.GlobalCalibration;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Experiment.Test;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.WaferHandlingTeach;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.Office.Utils;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraTreeList;
    using OfficeOpenXml;

    public partial class FrmDebug : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        public FrmDebug()
        {
            this.InitializeComponent();
            this.Init();
        }        

        /// <summary>
        /// BtnStart
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (!MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    Machine.GetInstance().Stop();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    if (WaferSystemDomain.GetInstance().CheckIsReady(true))
                    {
                        WaferSystemDomain.GetInstance().WaferSubSystemTask.Start();
                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();
                        btn.Appearance.BackColor = Color.Yellow;
                    }
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //Block.GetInstance().SetCurrentCarrier(CarrierConfigRepository.GetInstance().BaseDsSettingList.Find(a => a.Name == "wt-1"));
            //Block.GetInstance().LoadMap("wt-1", true);
            //SignalPool.GetInstance().IsLoadWaferMap.Set();
            return;

            //FrmComponentMap frmComponentMap = new FrmComponentMap(CarrierConfigRepository.GetInstance().BaseDsSettingList.Find(a=>a.Name == "wt-1"));
            //frmComponentMap.ShowDialog();

            //return;

            //if (!MachineStateModel.GetInstance().IsOffLineWork)
            //{
            //    return;
            //}

            //SimpleButton btn = sender as SimpleButton;
            //try
            //{
            //    btn.Enabled = false;
            //    btn.Appearance.BackColor = Color.Yellow;

            //    Block.GetInstance().LoadMap(Static.CurrentWaferMapName);
            //    SignalPool.GetInstance().IsLoadWaferMap.Set();
            //}
            //catch (Exception exception)
            //{
            //    AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    btn.Appearance.BackColor = default;
            //    btn.Enabled = true;
            //}
        }
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (!MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            this.ShowForm<FrmParamManager>();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //Dictionary<AKRSPoint2D, AKRSPoint2D> resultZWithXY = new Dictionary<AKRSPoint2D, AKRSPoint2D>();
            //for (int i = 0; i < GlobalCalibrationDomain.GetInstance().BondPosX.Count; i++)
            //{
            //    resultZWithXY.Add(new AKRSPoint2D(GlobalCalibrationDomain.GetInstance().BondPosX[i], GlobalCalibrationDomain.GetInstance().BondPosY[i]), new AKRSPoint2D(GlobalCalibrationDomain.GetInstance().BondOffsetX[i], GlobalCalibrationDomain.GetInstance().BondOffsetY[i]));
            //}

            //GlobalCalibrationDomain.GetInstance().ResultZWithXY = resultZWithXY;
            //GlobalCalibrationDomain.GetInstance().Save();
        }

        private void Init()
        {
            this.simpleButton2.Text = "参数";
        }
    }
}
