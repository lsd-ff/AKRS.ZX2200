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
    /// 下料仓控制器
    /// </summary>
    public class UnLoaderBinController
    {
        /// <summary>
        /// 下料模组
        /// </summary>
        private UnloaderBinModule unloaderBinModule = new UnloaderBinModule();

        /// <summary>
        /// 下料仓程式
        /// </summary>
        private UnLoaderBinProgram unloaderBinProgram => TransportProgram.GetInstance().UnLoaderBinProgram;

        /// <summary>
        /// 是否感应到料盒A
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinAOnTable()
        {
            return this.unloaderBinModule.IsBinAOnTable();
        }

        /// <summary>
        /// 是否感应到料盒B
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool IsBinBOnTable()
        {
            return this.unloaderBinModule.IsBinBOnTable();
        }

        /// <summary>
        /// 移动Y轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveAxisY(double pos)
        {
            this.unloaderBinModule.MoveAxisY(pos);
        }

        /// <summary>
        /// 移动Z轴到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveAxisZ(double pos)
        {
            this.unloaderBinModule.MoveAxisZ(pos);
        }

        /// <summary>
        /// 移动推杆到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MovePushRod(double pos)
        {
            ExcuteResult ret = this.unloaderBinModule.MovePushRod(pos);

            if (ret != ExcuteResult.Success)
            {
                throw new Exception("下料推杆移动异常！");
            }
        }

        /// <summary>
        /// 移动推杆缩回
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult MovePushRodHome()
        {
            try
            {
                // 去这个位置是防止触发限位
                ExcuteResult ret = this.unloaderBinModule.MovePushRod(4);

                if (ret != ExcuteResult.Success)
                {
                    throw new Exception("下料推杆缩回异常！");
                }

                return ret;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"下料推杆缩回运行故障", e, LogCategory.Global);
                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 推杆推料
        /// </summary>
        /// <returns>结果</returns>
        public ExcuteResult PushTabletToUnloader()
        {
            try
            {
                ExcuteResult ret = this.unloaderBinModule.MovePushRod(this.unloaderBinProgram.UnloaderBin.PushPos);

                if (ret != ExcuteResult.Success)
                {
                    throw new Exception("下料推杆推出异常！");
                }

                return ret;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"下料推杆推出异常", e, LogCategory.Global);
                return ExcuteResult.Exception;
            }
        }

        /// <summary>
        /// 获取当前料片序号
        /// </summary>
        /// <returns>序号</returns>
        public int GetCurPlaceLayer()
        {
            return this.unloaderBinProgram.CurPlaceLayer;
        }

        /// <summary>
        /// 设置当前放置层
        /// </summary>
        /// <param name="num">序号</param>
        public void SetCurPlaceLayer(int num)
        {
            this.unloaderBinProgram.CurPlaceLayer = num;
        }

        /// <summary>
        /// 当前放置层+1
        /// </summary>
        public void AddCurPlaceLayer()
        {
            this.unloaderBinProgram.CurPlaceLayer += 1;
        }

        /// <summary>
        /// 重置当前放置层
        /// </summary>
        public void ResetCurPlaceLayer()
        {
            this.unloaderBinProgram.CurPlaceLayer = 1;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 重置下料仓
        /// </summary>
        public void ResetUnloader()
        {
            this.unloaderBinProgram.CurPlaceLayer = 1;
            this.unloaderBinProgram.CurBinType = BinTypeEnum.BinA;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 获取当前料盒
        /// </summary>
        /// <returns>枚举</returns>
        public BinTypeEnum GetCurBinType()
        {
            return this.unloaderBinProgram.CurBinType;
        }

        /// <summary>
        /// Z轴移动到最后的料片
        /// </summary>
        public void MoveToLastTablet()
        {
            //this.MoveToTabletLevel(this.unloaderBinProgram.CurPlaceLayer - 1);

            this.MoveToTabletLevel((int)this.unloaderBinProgram.UnloaderBin.LayerNum);
            this.unloaderBinProgram.CurPlaceLayer = (int)this.unloaderBinProgram.UnloaderBin.LayerNum;

            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 移动到指定料片
        /// </summary>
        /// <param name="num">料片序号</param>
        public void MoveToTabletLevel(int num)
        {
            if (!this.IsPushRodSafe())
            {
                // 不安全就推杆缩回
                this.MovePushRodHome();
            }

            if (this.IsOutOfTabletLimit(num))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"料片数量超限，下料仓移动失败!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                throw new Exception("料片数量超限，下料仓移动失败!");
            }

            double pos = this.unloaderBinProgram.UnloaderBin.FirstTabletLevel
                         + this.unloaderBinProgram.UnloaderBin.TabletPitch * (num - 1);
            this.unloaderBinModule.MoveAxisZ(pos);
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

            if (this.unloaderBinProgram.CurBinType == BinTypeEnum.BinA)
            {
                double pos = this.unloaderBinProgram.UnloaderBin.BinBPosY;
                this.unloaderBinModule.MoveAxisY(pos);
                this.unloaderBinProgram.CurBinType = BinTypeEnum.BinB;
            }
            else
            {
                double pos = this.unloaderBinProgram.UnloaderBin.BinAPosY;
                this.unloaderBinModule.MoveAxisY(pos);
                this.unloaderBinProgram.CurBinType = BinTypeEnum.BinA;
            }

            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 移动到当前料盒
        /// </summary>
        public void MoveToCurrentBin()
        {
            if (this.unloaderBinProgram.CurBinType == BinTypeEnum.BinA)
            {
                double pos = this.unloaderBinProgram.UnloaderBin.BinAPosY;
                this.unloaderBinModule.MoveAxisY(pos);
            }
            else
            {
                double pos = this.unloaderBinProgram.UnloaderBin.BinBPosY;
                this.unloaderBinModule.MoveAxisY(pos);
            }
        }

        /// <summary>
        /// 移动到料盒A
        /// </summary>
        public void MoveToBinA()
        {
            double pos = this.unloaderBinProgram.UnloaderBin.BinAPosY;
            this.unloaderBinModule.MoveAxisY(pos);
        }

        /// <summary>
        /// 移动到料盒B
        /// </summary>
        public void MoveToBinB()
        {
            double pos = this.unloaderBinProgram.UnloaderBin.BinBPosY;
            this.unloaderBinModule.MoveAxisY(pos);
            this.unloaderBinProgram.CurBinType= BinTypeEnum.BinB;
        }

        /// <summary>
        /// 料片序号是否超出上限
        /// </summary>
        /// <returns>结果</returns>
        public bool IsOutOfTabletLimit()
        {
            return this.unloaderBinProgram.CurPlaceLayer > this.unloaderBinProgram.UnloaderBin.LayerNum
                   || this.unloaderBinProgram.CurPlaceLayer <= 0;
        }

        /// <summary>
        /// 移动到安全位
        /// </summary>
        public void MoveToSafePos()
        {
            this.MovePushRodHome();
        }

        /// <summary>
        /// 检查料盒是否存在
        /// </summary>
        /// <returns>结果</returns>
        public bool IsCurrentBinOnTable()
        {
            if (this.unloaderBinProgram.CurBinType == BinTypeEnum.BinA)
            {
                return this.unloaderBinModule.IsBinAOnTable();
            }
            else
            {
                return this.unloaderBinModule.IsBinBOnTable();
            }
        }

        /// <summary>
        /// 上料仓是否空了
        /// </summary>
        /// <returns>结果</returns>
        public bool IsUnloaderFull()
        {
            // 当前是料盒B且料片超出范围
            bool ret1 = this.IsOutOfTabletLimit();
            bool ret2 = this.unloaderBinProgram.CurBinType == BinTypeEnum.BinB;
            return ret1 && ret2;
        }

        /// <summary>
        /// 推杆是否安全
        /// </summary>
        /// <returns>结果</returns>
        public bool IsPushRodSafe()
        {
            return Math.Abs(this.unloaderBinModule.UnLoaderPushRod.GetRealPosition()) < 5;
        }

        /// <summary>
        /// 料片序号是否超出上限
        /// </summary>
        /// <param name="num">序号</param>
        /// <returns>结果</returns>
        public bool IsOutOfTabletLimit(int num)
        {
            return num > this.unloaderBinProgram.UnloaderBin.LayerNum
                   || num <= 0;
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

            return this.unloaderBinModule.UnLoaderPushRod.GetRealPosition();
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

            return this.unloaderBinModule.UnLoaderTableAxisY.GetRealPosition();
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

            return this.unloaderBinModule.UnLoaderTableAxisZ.GetRealPosition();
        }

        /// <summary>
        /// 移动到当前放置层
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
                    this.MoveToTabletLevel(this.unloaderBinProgram.CurPlaceLayer);
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
                        // 报警下料仓空了
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"下料仓已经满料! 请选择怎么处理!
                              选择
                              换仓: 换另一个料盒然后继续上料
                              停止: 设备停止工作",
                            $"下料仓报警",
                            new[] { "换仓", "停止" },
                            new[] { DialogResult.OK, DialogResult.Abort },
                            AlarmLevel.FirstLevel);

                        switch (dr)
                        {
                            case DialogResult.OK:
                                this.ResetUnloader();
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
                    $@"下料仓  {this.GetCurBinType()} 没有感应到料盒! 请检查料盒!
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
                if (!this.IsOutOfTabletLimit(this.unloaderBinProgram.CurPlaceLayer + 1))
                {
                    // 移动YZ轴
                    this.MoveToCurrentBin();

                    this.MoveToTabletLevel(this.unloaderBinProgram.CurPlaceLayer + 1);
                    this.unloaderBinProgram.CurPlaceLayer++;
                    TransportProgram.GetInstance().Save();
                }
                else
                {
                    if (this.GetCurBinType() == BinTypeEnum.BinA)
                    {
                        // 切换B料盒
                        this.unloaderBinProgram.CurBinType = BinTypeEnum.BinB;
                        this.SetCurPlaceLayer(0);
                        goto Reload;
                    }
                    else
                    {
                        // 报警下料仓空了
                        DialogResult dr = AKRSMessageBoxExt.Show(
                            @"下料仓已经满料! 请选择怎么处理!
                              选择
                              换仓: 换另一个料盒然后继续上料
                              停止: 设备停止工作",
                            $"下料仓报警",
                            new[] { "换仓", "停止" },
                            new[] { DialogResult.OK, DialogResult.Abort },
                            AlarmLevel.FirstLevel);

                        switch (dr)
                        {
                            case DialogResult.OK:
                               
                                // 重置
                                this.ResetUnloader();

                                // 移动YZ轴
                                this.MoveToCurrentBin();
                                this.MoveToTabletLevel(this.unloaderBinProgram.CurPlaceLayer );

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
                    $@"下料仓  {this.GetCurBinType()} 没有感应到料盒! 请检查料盒!
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
    }
}
