namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.LeadShine.E5032;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.MeasureHeight;
    using AKRS.Galaxy2.MeasureHeight.IOMeasureHeight;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    using DevExpress.XtraEditors;

    public partial class FrmTest : DevExpress.XtraEditors.XtraForm
    {
        public FrmTest()
        {
            this.InitializeComponent();
        }

        private void BtTestMeasureHeight_Click(object sender, EventArgs e)
        {
            int value = 0;
            // 关闭探针
            short ret = LTDMC.nmc_write_rxpdo_extra(0, 2, 4, 1, 0);
            Thread.Sleep(10);
            // 打开探针
            ret = LTDMC.nmc_write_rxpdo_extra(0, 2, 4, 1, 4352);
            Thread.Sleep(10);
            ret = LTDMC.nmc_read_txpdo_extra(0, 2, 4, 1, ref value);

            // 运动        
            Axis DispenseAxisZ = HardwareRepositoryService.GetHardware<Axis>("点胶Z");
            DispenseAxisZ.AbsoluteMove(0);
            DispenseAxisZ.AbsoluteMove(-34.72);

            ret = LTDMC.nmc_read_txpdo_extra(0, 2, 4, 1, ref value);
            // 判断低 9 位 如果为1 则执行成功  0 执行失败
            if (value != 0)
            {

                ret = LTDMC.nmc_read_txpdo_extra(0, 2, 5, 2, ref value);
                AKRSXtraMessageBox.Show(value.ToString() + " " + LTDMC.nmc_get_axis_io_in(0, 0).ToString());
            }
        }

        private void BtTestMeasureHeight2_Click(object sender, EventArgs e)
        {

            //// 设置搜索速度 这个不知道可不可变
            //double searchVel = 5;

            IOJudgeCondition condition = new IOJudgeCondition()
            {
                MeasureHeightSensorName = "点胶测高传感器",

                // 默认值
                IsCalcLVDT = false,

                // 默认值 不使用 lvdt
                IsUseLVDTJudgeState = false,

                // 当传感器 的状态为 TriggeIoValue 时 说明已经到达测高位置
                TriggeIoValue = true
            };

            // 测高实体
            MeasureHeightEntity measureHeightEntity = new MeasureHeightEntity()
            {
                AxisName = "点胶Z",
                SearchPosition = -32,
                LiftPosition = 0,
                SearchVel = 50,
                MeasureType = MeasureHeightTypeEnum.Probe,
                SearchDir = MoveDirection.Negative,
                SearchDistance = 3,
                TimeOutMillSeconds = 10000,
                IsWaitStopArrive = false,
                JudgeArrive = condition,

                // 探针地址
                ProbeAddress = 4,

                // 打开探针  的地址
                ProbeOpenValue = 4532,

                // 探针测出高度存储的地址
                ProbeValueAddress = 5
            };
  
            // 返回结果
            var ret = measureHeightEntity.DoWork();

            AKRSXtraMessageBox.Show(ret.HeightValue.ToString());
        }

        private void BtSaveUplookImage_Click(object sender, EventArgs e)
        {
            AKRSCamera camera = HardwareRepositoryService.GetHardware<AKRSCamera>("上视相机");
            Task.Run(() =>
            {
                for (int i = 0; i < 500; i++)
                {
                    Thread.Sleep(100);
                    string fileName = "D:\\小芯片图片\\3\\" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".bmp";
                    camera.SnapImage(false, false)?.Save(fileName);
                }
            });
        }

        /// <summary>
        /// 读取Bond Lvdt
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtReadBondLvdt_Click(object sender, EventArgs e)
        {
            Sensor lvdt = HardwareRepositoryService.GetHardware<Sensor>("LVDT");
            int value = lvdt.ReadTxPDO(0, 1);
            this.SpLvdt.EditValue = value;
        }

        private void BtMessageBox_Click(object sender, EventArgs e)
        {
            AKRSMessageBoxExt.Show(
                $"Refresh  mapping  time  out!\r\n",
                "Alarm",
                new string[] { "Retry", "Abort" },
                new DialogResult[] { DialogResult.Retry, DialogResult.Abort });
        }

        private BondHeadController bondHeadController = new BondHeadController();
        private bool tag = false;
        /// <summary>
        /// 读取真空值
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtReadBondVaccum_Click(object sender, EventArgs e)
        {
            if (this.BtReadBondVaccum.Text == "启动读取Bond真空值")
            {
                this.BtReadBondVaccum.Text = "停止读取Bond真空值";
                this.tag = true;

                Task.Run(() =>
                {
                    this.bondHeadController.OpenToolVaccum();
                    while (this.tag)
                    {
                        double value = this.bondHeadController.ReadVacuumValue();
                     
                        this.BeginInvoke(
                            new Action(() =>
                            {
                                this.SpBondVaccum.EditValue = value;
                            }));
                    }
                    this.bondHeadController.CloseToolVaccum();
                });
            }
            else
            {
                this.BtReadBondVaccum.Text = "启动读取Bond真空值";
                this.tag = false;
                this.bondHeadController.CloseToolVaccum();
            }
        }

        private void BtReadBondVacuumManul_Click(object sender, EventArgs e)
        {
            this.bondHeadController.CloseToolVaccum();
            this.bondHeadController.OpenToolBlowEle();
            this.bondHeadController.CloseToolBlowEle();
            this.bondHeadController.OpenToolVaccum();
            double value = this.bondHeadController.ReadVacuumValue();
            this.SpBondVaccum.EditValue = value;
            this.bondHeadController.CloseToolVaccum();

        }
    }
}