using System;
using System.Threading;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Modules;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    ///  刮胶盘控制器
    /// </summary>
    public class SlideFluxerController
    {
        /// <summary>
        ///  刮胶盘
        /// </summary>
        private SlideFluxer slideFluxer = new SlideFluxer();

        /// <summary>
        ///  刮胶程式
        /// </summary>
        private SlideFluxerProgram slideFluxerProgram => BondProgram.GetInstance().SlideFluxerProgram;

        /// <summary>
        /// 气缸伸出
        /// </summary>
        public void SlideFluxerOutNoWait()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.slideFluxer.SlideFluxerOut();
        }


        /// <summary>
        /// 气缸缩回
        /// </summary>
        public void SlideFluxerHomeNoWait()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.slideFluxer.SlideFluxerHome();
        }

        /// <summary>
        /// 气缸缩回
        /// </summary>
        public void SlideFluxerHomeWaitArrive()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.slideFluxer.SlideFluxerHome();

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (this.IsSlideFluxerAtNLimit())
                {
                    return;
                }

                if ((DateTime.Now - startTime) > TimeSpan.FromSeconds(10))
                {
                    throw new Exception("刮胶盘气缸缩回超时！");
                }
            }
        }

        /// <summary>
        /// 气缸伸出
        /// </summary>
        public void SlideFluxerOutWaitArrive()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.slideFluxer.SlideFluxerOut();

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (this.IsSlideFluxerAtPLimit())
                {
                    return;
                }

                if ((DateTime.Now - startTime) > TimeSpan.FromSeconds(10))
                {
                    throw new Exception("刮胶盘气缸缩回超时！");
                }
            }
        }

        /// <summary>
        /// 气缸是否在负限位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsSlideFluxerAtNLimit()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return false;
            }

            return this.slideFluxer.IsSlideFluxerAtNLimit();
        }


        /// <summary>
        /// 气缸是否在正限位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsSlideFluxerAtPLimit()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return false;
            }


            return this.slideFluxer.IsSlideFluxerAtPLimit();
        }

        /// <summary>
        /// 刮胶
        /// </summary>
        /// <param name="delay">延时</param>
        public void SlideFlux(int delay)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.SlideFluxerOutWaitArrive();

            Thread.Sleep(delay);

            this.SlideFluxerHomeWaitArrive();
        }

        /// <summary>
        /// 刮胶
        /// </summary>
        public void SlideFlux()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.SlideFluxerOutWaitArrive();

            Thread.Sleep(this.slideFluxerProgram.SlideDelay);

            this.SlideFluxerHomeWaitArrive();
        }
    }
}
