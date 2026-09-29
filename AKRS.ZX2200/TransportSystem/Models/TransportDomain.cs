#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 11:28:50
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using AKRS.Galaxy2.Component.Simple.MessageBox;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Tasks;
using DevExpress.XtraEditors;
using log4net.Core;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportSystem.Models
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using DevExpress.DataAccess.Wizard.Views;
    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;

    /// <summary>
    /// 描述：基板流道域
    /// 包含物理流道和逻辑流道（流道程式）
    /// </summary>
    public class TransportDomain : SingletonNoSave<TransportDomain>
    {
        /// <summary>
        /// 流道程式
        /// </summary>
        public TransportProgram TransportProgram => TransportProgram.GetInstance();

        /// <summary>
        /// load section 控制器
        /// </summary>
        public TransportController TransportController = new TransportController();

        /// <summary>
        /// 流道线程
        /// </summary>
        public TransportTask transportTask = new TransportTask();

        /// <summary>
        /// 上料仓线程
        /// </summary>
        public LoaderTask LoaderTask = new LoaderTask();

        /// <summary>
        /// 下料仓线程
        /// </summary>
        public UnloaderTask UnloaderTask = new UnloaderTask();

        /// <summary>
        /// 启动基板流道任务
        /// </summary>
        public void StartTask()
        {
            try
            {
                if (!MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
                {
                    return;
                }

                TransportModule.GetInstance().Init();

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                {
                    this.transportTask.StartDryRun();
                }
                else
                {
                    // 启动前检查
                    if (this.TransportController.CheckCarrierBeforeLoaderMove() == false)
                    {
                        Machine.GetInstance().Stop();
                        return;
                    }

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online)
                    {
                        this.TransportController.LoadingSubSectionController.SetLoadingTableNeedTabletSignal(false);
                    }


                    if (TransportDevicePara.GetInstance().IsReverseSearch)
                    {
                        // 2. 刷新流道的TransportUnit
                        TransportProgram.RefreshTransportUnit();
                    }

                    // 4. Check 流道看有没有料 并和记忆做匹配
                    this.TransportController.MapMaterial();

                    // 3、开启上下料
                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        this.LoaderTask.Start();
                        this.UnloaderTask.Start();
                    }

                    // 产品位置记录显示
                    // FrmTransportSystemProductLocation.GetInstance().Show();
                    // 3. 流道线程开启
                    this.transportTask.Start();
                }                
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流道线程异常", ex, LogCategory.Transport);
                AKRSXtraMessageBox.Show(ex.Message, "Transport task error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ///// <summary>
        ///// 流道是否准备好
        ///// </summary>
        ///// <returns></returns>
        //public bool IsTranportSystemReady()
        //{
        //    if (this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState != SubSectionStateEnum.NoMaterial
        //        && TransportProgram.GetInstance().WaitingUnloadSubSectionProgram.TransportUnit == null)
        //    {
        //    }
        //}
    }
}
