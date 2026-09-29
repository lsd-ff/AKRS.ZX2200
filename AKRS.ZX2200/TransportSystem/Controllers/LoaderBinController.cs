using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Enums;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Modules;
using DevExpress.XtraEditors;
using log4net.Core;

namespace AKRS.ZX2200.TransportSystem.Controllers
{
    /// <summary>
    /// 上料仓控制器
    /// </summary>
    public class LoaderBinController
    {
        /// <summary>
        /// 上料模组
        /// </summary>
        private LoaderBinModule loaderBinModule = new LoaderBinModule();

        /// <summary>
        /// 上料仓程式
        /// </summary>
        private LoaderBinProgram loaderBinProgram => TransportProgram.GetInstance().LoaderBinProgram;

        /// <summary>
        /// 是否感应到料盒A
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinAOnTable()
        {
            return this.loaderBinModule.IsBinAOnTable();
        }

        /// <summary>
        /// 是否感应到料盒B
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinBOnTable()
        {
            return this.loaderBinModule.IsBinBOnTable();
        }

        /// <summary>
        /// 移动Y轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveAxisY(double pos)
        {
             return this.loaderBinModule.MoveAxisY(pos);
        }

        /// <summary>
        /// 移动Z轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveAxisZ(double pos)
        {
           return this.loaderBinModule.MoveAxisZ(pos);
        }

        /// <summary>
        /// 移动推杆到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns>结果</returns>
        public ExcuteResult MovePushRod(double pos)
        {
            ExcuteResult ret = this.loaderBinModule.MovePushRod(pos);

           if (ret != ExcuteResult.Success)
           {
               throw new Exception("上料推杆移动异常！");
           }

           return ret;
        }

        /// <summary>
        /// 上料推杆缩回
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MovePushRodHome()
        {
            try
            {
                // 去这个位置是防止触发限位
                ExcuteResult ret = this.loaderBinModule.MovePushRod(2);

                if (ret != ExcuteResult.Success)
                {
                    throw new Exception("上料推杆缩回异常！");
                }

                return ret;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"上料推杆缩回异常！", e, LogCategory.Global);
                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 推杆推料 不等待到位
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult PushTabletToDispenseNoWait()
        {
            try
            {
                double pos = this.loaderBinProgram.LoaderBin.PushPos;

                return this.loaderBinModule.SendMovePushRodCommand(pos);
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"上料推杆推出运行故障", e, LogCategory.Global);
                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 推杆是否到位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsPushRodInPosition()
        {
            try
            {
                bool ret = this.loaderBinModule.IsPushRodInPosition();
                return ret;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"判断推杆是否到位失败", e, LogCategory.Global);
                return false;
            }
        }

        /// <summary>
        /// 推杆推料
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult PushTabletToDispense()
        {
            try
            {
                // 正限
                double pLimit = this.loaderBinModule.LoaderPushRod.AxisSetPara.PLimit;

                ExcuteResult ret = this.loaderBinModule.SendMovePushRodCommand(this.loaderBinProgram.LoaderBin.PushPos);

                if (ret != ExcuteResult.Success)
                {
                    throw new Exception("上料推杆推出异常！");
                }

                return ret;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"上料推杆推出异常！", e, LogCategory.Global);
                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 获取当前料片序号
        /// </summary>
        /// <returns>序号</returns>
        public int GetCurPlaceLayer()
        {
            return this.loaderBinProgram.CurPlaceLayer;
        }

        /// <summary>
        /// 获取当前料片序号
        /// </summary>
        /// <param name="num">序号</param>
        public void SetCurPlaceLayer(int num)
        {
             this.loaderBinProgram.CurPlaceLayer = num;
        }

        /// <summary>
        /// 当前放置层+1
        /// </summary>
        public void AddCurPlaceLayer()
        {
            this.loaderBinProgram.CurPlaceLayer += 1;
        }

        /// <summary>
        /// 重置当前料片序号
        /// </summary>
        public void ResetCurPlaceLayer()
        {
            this.loaderBinProgram.CurPlaceLayer = 1;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 重置上料仓
        /// </summary>
        public void ResetLoader()
        {
            this.loaderBinProgram.CurPlaceLayer = 1;
            this.loaderBinProgram.CurBinType = BinTypeEnum.BinA;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 获取当前料盒
        /// </summary>
        /// <returns>枚举</returns>
        public BinTypeEnum GetCurBinType()
        {
            return this.loaderBinProgram.CurBinType;
        }

        /// <summary>
        /// Z轴移动到上一料片
        /// </summary>
        public void MoveToLastTablet()
        {
            this.MoveToTabletLevel((int)this.loaderBinProgram.LoaderBin.LayerNum);
            this.loaderBinProgram.CurPlaceLayer = (int)this.loaderBinProgram.LoaderBin.LayerNum;

            //this.MoveToTabletLevel(this.loaderBinProgram.CurPlaceLayer - 1);
            //this.loaderBinProgram.CurPlaceLayer--;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 移动到指定料片
        /// </summary>
        /// <param name="num">料片序号</param>
        /// <returns>结果</returns>
        public ExcuteResult MoveToTabletLevel(int num)
        {
            if (!this.IsPushRodSafe())
            {
                // 不安全就推杆缩回
                this.MovePushRodHome();
            }

            if (this.IsOutOfTabletLimit(num))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"料片数量超限，上料仓移动失败!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                throw new Exception("料片数量超限，上料仓移动失败!");
            }

            double pos = this.loaderBinProgram.LoaderBin.FirstTabletLevel
                         + this.loaderBinProgram.LoaderBin.TabletPitch * (num - 1);

           return this.loaderBinModule.MoveAxisZ(pos);
        }

        /// <summary>
        /// 切换下一料盒
        /// </summary>
        public void MoveToNextBin()
        {
            if (!this.IsPushRodSafe())
            {
                // 不安全就推杆缩回
                this.MovePushRodHome();
            }

            if (this.loaderBinProgram.CurBinType == BinTypeEnum.BinA)
            {
                double pos = this.loaderBinProgram.LoaderBin.BinBPosY;
                this.loaderBinModule.MoveAxisY(pos);
                this.loaderBinProgram.CurBinType = BinTypeEnum.BinB;
            }
            else
            {
                double pos = this.loaderBinProgram.LoaderBin.BinAPosY;
                this.loaderBinModule.MoveAxisY(pos);
                this.loaderBinProgram.CurBinType = BinTypeEnum.BinA;
            }

            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 移动到当前料盒
        /// </summary>
        public void MoveToCurrentBin()
        {
            if (!this.IsPushRodSafe())
            {
                // 不安全就推杆缩回
                this.MovePushRodHome();
            }

            if (this.loaderBinProgram.CurBinType == BinTypeEnum.BinA)
            {
                double pos = this.loaderBinProgram.LoaderBin.BinAPosY;
                this.loaderBinModule.MoveAxisY(pos);
            }
            else
            {
                double pos = this.loaderBinProgram.LoaderBin.BinBPosY;
                this.loaderBinModule.MoveAxisY(pos);
            }
        }

        /// <summary>
        /// 移动到料盒A
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MoveToBinA()
        {
            double pos = this.loaderBinProgram.LoaderBin.BinAPosY;
            return this.loaderBinModule.MoveAxisY(pos);
        }

        /// <summary>
        /// 移动到料盒B
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MoveToBinB()
        {
            double pos = this.loaderBinProgram.LoaderBin.BinBPosY;
            return this.loaderBinModule.MoveAxisY(pos);
        }

        /// <summary>
        /// 料片序号是否超出上限
        /// </summary>
        /// <returns>结果</returns>
        public bool IsOutOfTabletLimit()
        {
            return this.loaderBinProgram.CurPlaceLayer > this.loaderBinProgram.LoaderBin.LayerNum
                   || this.loaderBinProgram.CurPlaceLayer <= 0;
        }

        /// <summary>
        /// 料片序号是否超出上限
        /// </summary>
        /// <param name="num">序号</param>
        /// <returns>结果</returns>
        public bool IsOutOfTabletLimit(int num)
        {
            return num > this.loaderBinProgram.LoaderBin.LayerNum
                   || num <= 0;
        }

        /// <summary>
        /// 检查料盒是否存在
        /// </summary>
        /// <returns>结果</returns>
        public bool IsCurrentBinOnTable()
        {
            if (this.loaderBinProgram.CurBinType == BinTypeEnum.BinA)
            {
                return this.loaderBinModule.IsBinAOnTable();
            }
            else
            {
                return this.loaderBinModule.IsBinBOnTable();
            }
        }

        /// <summary>
        /// 上料仓是否空了
        /// </summary>
        /// <returns>结果</returns>
        public bool IsLoaderEmpty()
        {
            // 当前是料盒B且料片超出范围
            bool ret1 = this.IsOutOfTabletLimit();
            bool ret2 = this.loaderBinProgram.CurBinType == BinTypeEnum.BinB;
            return ret1 && ret2;
        }

        /// <summary>
        /// 推杆是否安全
        /// </summary>
        /// <returns>结果</returns>
        public bool IsPushRodSafe()
        {
            // 判断是否在0位
            return Math.Abs(this.loaderBinModule.LoaderPushRod.GetRealPosition()) < 2.5;
        }

        /// <summary>
        /// 获取X的位置
        /// </summary>
        /// <returns>X轴的位置</returns>
        public double GetPushPos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return 1.0;
            }

            return this.loaderBinModule.LoaderPushRod.GetRealPosition();
        }

        /// <summary>
        /// 获取Y的位置
        /// </summary>
        /// <returns>X轴的位置</returns>
        public double GetYPos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return 1.0;
            }

            return this.loaderBinModule.LoaderTableAxisY.GetRealPosition();
        }

        /// <summary>
        /// 获取Z的位置
        /// </summary>
        /// <returns>X轴的位置</returns>
        public double GetZPos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return 1.0;
            }

            return this.loaderBinModule.LoaderTableAxisZ.GetRealPosition();
        }

        /// <summary>
        /// 移动到当前料片
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MoveToCurrentPlaceLayer()
        {
            Reload:

            // 检查当前料盒是否存在
            if (this.IsCurrentBinOnTable())
            {
                // 料盒有料
                if (!this.IsOutOfTabletLimit())
                {
                    // 移动YZ到准备位
                    this.MoveToCurrentBin();
                    this.MoveToTabletLevel(this.loaderBinProgram.CurPlaceLayer);
                }
                else
                {
                    if (this.GetCurBinType() == BinTypeEnum.BinA)
                    {
                        // 移动到下一料盒
                        this.MoveToNextBin();

                        // 重置料片序号
                        this.ResetCurPlaceLayer();

                        goto Reload;
                    }
                    else
                    {
                        // 报警上料仓空了
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"上料仓已经没有料! 请选择怎么处理!
                              选择
                              换仓: 换另一个料盒然后继续上料
                              停止: 设备停止工作",
                            $"上料仓报警",
                            new[] { "换仓", "停止" },
                            new[] { DialogResult.OK, DialogResult.Abort },
                            AlarmLevel.FirstLevel);

                        switch (dr)
                        {
                            case DialogResult.OK:

                                this.ResetLoader();
                                goto Reload;

                            case DialogResult.Abort:
                                return ExcuteResult.Abort;
                        }
                    }
                }
            }
            else
            {
                // 不存在就报警
                DialogResult dr = AKRSMessageBoxExt.Show(
                    $@"上料仓  {this.GetCurBinType()} 没有感应到料盒! 请检查料盒!
                              选择
                              重试: 传感器重新感应
                              忽略: 传感器误识别，换料仓继续工作
                              停止: 设备停止工作",
                    $"上料仓报警",
                    new[] { "重试", "忽略", "停止" },
                    new[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                    AlarmLevel.FirstLevel);

                switch (dr)
                {
                    case DialogResult.Retry:
                        goto Reload;

                    case DialogResult.Ignore:

                        // 移动到下一料盒
                        this.MoveToNextBin();

                        // 重置料片序号
                        this.ResetCurPlaceLayer();

                        goto Reload;

                    case DialogResult.Abort:
                        return ExcuteResult.Abort;
                }
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 移动到下一料片
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MoveToNextPlaceLayer()
        {
        Reload:

            // 检查当前料盒是否存在
            if (this.IsCurrentBinOnTable())
            {
                // 料盒有料
                if (!this.IsOutOfTabletLimit(this.loaderBinProgram.CurPlaceLayer + 1))
                {
                    // 移动YZ轴
                    this.MoveToCurrentBin();

                    this.MoveToTabletLevel(this.loaderBinProgram.CurPlaceLayer + 1);
                    this.loaderBinProgram.CurPlaceLayer++;
                    TransportProgram.GetInstance().Save();
                }
                else
                {
                    if (this.GetCurBinType() == BinTypeEnum.BinA)
                    {
                        // 切换B料盒
                        this.loaderBinProgram.CurBinType = BinTypeEnum.BinB;
                        this.SetCurPlaceLayer(0);
                        goto Reload;
                    }
                    else
                    {
                        // 报警上料仓空了
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"上料仓已经没有料! 请选择怎么处理!
                              选择
                              换仓: 换另一个料盒然后继续上料
                              停止: 设备停止工作",
                            $"上料仓报警",
                            new[] { "换仓", "停止" },
                            new[] { DialogResult.OK, DialogResult.Abort },
                            AlarmLevel.FirstLevel);

                        switch (dr)
                        {
                            case DialogResult.OK:

                                // 重置
                                this.ResetLoader();

                                // 移动YZ轴
                                this.MoveToCurrentBin();
                                this.MoveToTabletLevel(this.loaderBinProgram.CurPlaceLayer);

                                return ExcuteResult.Success;

                            case DialogResult.Abort:
                                return ExcuteResult.Abort;
                        }
                    }
                }
            }
            else
            {
                // 不存在就报警
                DialogResult dr = AKRSMessageBoxExt.Show(
                    $@"上料仓  {this.GetCurBinType()} 没有感应到料盒! 请检查料盒!
                              选择
                              重试: 传感器重新感应
                              忽略: 传感器误识别，换料仓继续工作
                              停止: 设备停止工作",
                    $"Loader Warning",
                    new[] { "重试", "忽略", "停止" },
                    new[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                    AlarmLevel.FirstLevel);

                switch (dr)
                {
                    case DialogResult.Retry:
                        goto Reload;

                    case DialogResult.Ignore:

                        // 移动到下一料盒
                        this.MoveToNextBin();

                        // 重置料片序号
                        this.ResetCurPlaceLayer();

                        goto Reload;

                    case DialogResult.Abort:
                        return ExcuteResult.Abort;
                }
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 推杆清错
        /// </summary>
        public void PushRodResetError()
        {
            this.loaderBinModule.PushRodResetError();
        }

        /// <summary>
        /// 推杆上使能
        /// </summary>
        public void PushRodPushRodServoOn()
        {
            this.loaderBinModule.PushRodServoOn();
        }
    }
}
