using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using ch.etel.edi.dmd.v40;
using ch.etel.edi.dsa.v40;
using DevExpress.XtraEditors;
using log4net.Core;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Modules
{
    /// <summary>
    /// 刮胶盘
    /// </summary>
    public class SlideFluxer
    {
        #region 硬件


        /// <summary>
        /// 固晶区挡料气缸
        /// </summary>
        internal Electric SlideFluxerCylinderElectric => HardwareRepositoryService.GetHardware<Electric>("刮胶盘气缸");

        /// <summary>
        /// 刮胶盘气缸正限位
        /// </summary>
        internal Sensor SlideFluxerCylinderPLimit => HardwareRepositoryService.GetHardware<Sensor>("刮胶盘正限位");

        /// <summary>
        /// 刮胶盘气缸负限位
        /// </summary>
        internal Sensor SlideFluxerCylinderNLimit => HardwareRepositoryService.GetHardware<Sensor>("刮胶盘负限位");

        #endregion

        /// <summary>
        /// 气缸伸出
        /// </summary>
        public void SlideFluxerOut()
        {
            this.SlideFluxerCylinderElectric.SetOutputValue(true);
        }


        /// <summary>
        /// 气缸缩回
        /// </summary>
        public void SlideFluxerHome()
        {
            this.SlideFluxerCylinderElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 刮胶盒是否在负限位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsSlideFluxerAtNLimit()
        {
            return this.SlideFluxerCylinderNLimit.GetInputValue();
        }


        /// <summary>
        /// 刮胶盒是否在正限位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsSlideFluxerAtPLimit()
        {
            return this.SlideFluxerCylinderPLimit.GetInputValue();
        }
    }
}
