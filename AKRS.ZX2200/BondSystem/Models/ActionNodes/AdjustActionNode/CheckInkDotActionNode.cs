namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.AdjustActionNode
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Models;

    using log4net.Core;

    using Newtonsoft.Json;

    /// <summary>
    /// 墨点检测,贴片应该没有，后续和工艺确认
    /// </summary>
    [Serializable]
    public class CheckInkDotActionNode : ActionNode
    {
        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            return ExcuteResult.Success;

            //try
            //{
            //    return ExcuteResult.Success;

            //    // 移动到安全高度
            //    System2Module.GetInstance().BondModule.MoveBondZToHomePos();

            //    // 移动到矫正后的墨点拍照位
            //    List<AKRSPoint3D> pos = new List<AKRSPoint3D>();
            //    pos.Add(this.CheckInkDotPos);
            //   // this.bondModuleController.MoveSafeBondXYZ(BondProgramSort.GetCorrectPos(pos, this)[0]);

            //    // 拍照到位停留
            //    DelayHelper.Delay(System2Domain.GetInstance().BondConfiguration.VisionDelay);

            //    // 获取PR实体
            //    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.PrName);

            //// 拍照
            //RetryVision:

            //    ExcuteResult excuteResultP1 = pREntity.DoWork();

            //    if (excuteResultP1 != ExcuteResult.Success)
            //    {
            //        DialogResult dialog = AKRSMessageBoxExt.Show(
            //            "焊点定位失败  点击 \r\n" + "重试：重新拍照并定位\r\n" + "终止：异常停止，退出自动工作\r\n" + "忽略：忽略此报警，继续下一步骤\r\n",
            //          "报警",
            //          new string[] { "重试", "终止", "忽略" },
            //          new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
            //          AlarmLevel.SecondLevel);

            //        switch (dialog)
            //        {
            //            case DialogResult.Retry:
            //                // 重新执行检测流程
            //                goto RetryVision;

            //            case DialogResult.Abort:
            //                // 返回终止 
            //                return ExcuteResult.Abort;

            //            case DialogResult.Ignore:
            //                // 忽略视觉定位结果，继续执行动作,将焊点定位结果置为相机中心


            //                break;
            //        }
            //    }
            //    else
            //    {
            //        // 定位成功

            //    }

            //    // 获取当前基岛，
            //    Module curIslandAcupoint = this.System2Domain.GetCurrentModule();

            //    // 拍照结果存到基岛(如果检测到墨点，整个基岛都不贴)
            //    if (true)
            //    {
            //        curIslandAcupoint.StateEnum = IslandAcupointStateEnum.Bad;
            //    }

            //    return ExcuteResult.Success;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Post(Level.Error, $"流程{this.Name}运行故障！", ex, LogCategory.Process);
            //    AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
            //    return ExcuteResult.Exception;
            //}
            //finally
            //{
            //    this.State = RunStateEnum.Stop;
            //    this.WorkStop?.Invoke();
            //}
        }

        #region 方法

        /// <summary>
        /// 完成动作的条件是否满足
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            return true;
        }

        /// <summary>
        /// 是否报警
        /// 这个一般是程序中出现空指针的时候会采用这个方法
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            return true;
        }

        #endregion
    }
}
