using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;

namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    using System.IO;
    using System.Linq;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;
    using DevExpress.XtraRichEdit.Model.History;
    using OfficeOpenXml;

    /// <summary>
    /// 固高力控测试
    /// </summary>
    public partial class FrmGTForceTest1 : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmGTForceTest1()
        {
            InitializeComponent();
            this.InitForm();
            ModbusService.GetInstance().ConnectManometer();
            ModbusService.GetInstance().ConnectBondhead();
        }

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();


        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        ///  焊头
        /// </summary>

        private BondHead bondHead = new BondHead();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        ///  下压前是否清零
        /// </summary>
        private bool isZeroBondhead = false;

        /// <summary>
        ///  界面初始化
        /// </summary>
        private void InitForm()
        {
            this.CmbForceType.SelectedIndex = 0;
            this.bondHeadController.OpenBondHeadVaccum();

            this.RefreshData();
            this.timer3.Interval = 100;
            this.SpSmallForceCalibrateInitialVal.Value = (decimal)ForceConfig.GetInstance().SmallForceCalibrateInitialIncrement;
            this.SpLargeForceCalibrateInitialVal.Value = (decimal)ForceConfig.GetInstance().LargeForceCalibrateInitialIncrement;
            this.SpLVDTMaxInput.Value = (decimal)ForceConfig.GetInstance().LVDTMaxInputForce;

            this.txt_APos.Text= ForceConfig.GetInstance().ForceControlDebugAPos.ToString();
            this.txt_BPos.Text = ForceConfig.GetInstance().ForceControlDebugBPos.ToString();
            this.txt_CPos.Text = ForceConfig.GetInstance().ForceControlDebugCPos.ToString();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshData()
        {
            this.GcForceConfig.DataSource = this.isSmallForce
                                                ? ForceConfig.GetInstance()?.SmallForceConfigItemList
                                                : ForceConfig.GetInstance()?.LargeForceConfigItemList;

            this.bondHeadController.ChangeChannelAndSetForceControlPara(this.isSmallForce);

            this.GcForceConfig.Refresh();
            this.GvForceConfig.RefreshData();
        }

        public delegate void mydet();
        // short[] data = new short[512];
        //public delegate void RegisterCallbackFunction(const,  data);

        const short CORE = 1;
        short axis, testtry1, Homemethod;
        bool[] en = new bool[32];
        bool test4flag = false, home2flag = false, home3flag = false;
        int pos, encpos1;
        uint clk;
        double vel, prfpos, encpos;
        GTN.mc.TTrapPrm trap;
        GTN.mc.TJogPrm jog;
        GTN.mc.TPosCompareModeEx mode;
        GTN.mc.TPosComparePsoPrm psoPrm;

        //GTN.mc.TTrapPrm trapx;
        //GTN.mc.TTrapPrm trapy;
        short sProbePrm;        // 探针参数
        int iRiseValue1, iRiseValue2, iFallValue1, iFallValue2; // 探针捕获值
        //int iLastValue;
        ushort ECatiooffset, Ecatsize;
        byte Ecatiovalue;
        public Thread _th;

        public mydet _mydet;

        private Task recordTask;

        private bool isRecord = false;

        /// <summary>
        /// 打印的数据
        /// </summary>
        private List<(DateTime time, double targetForceVal, double forceVal)> printSource = new();

        /// <summary>
        ///  是否小力
        /// </summary>
        private bool isSmallForce = false;

        /// <summary>
        /// 退出力控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            double APos = Convert.ToDouble(txt_APos.Text);
            bondHeadController.GTForceControlReset(APos, 10);
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            //读取压力状态
            short rtn, CORE, PRESS_AXIS, sts;

            //获取轴状态
            rtn = GTN.mc.GTN_GetSts(2, AXIS, out int Axistate, 1, out uint CLOCK);
            //获取压力传感器状态
            long pressSts;
            rtn = GTN.mc.GTN_GetPressStatus(2, AXIS, out int Pressstate);

            //获取轴规划位置
            GTN.mc.GTN_GetPrfPos(2, AXIS, out double planValue, 1, out UInt32 pClock);

            //获取轴的实际位置
            GTN.mc.GTN_GetEncPos(2, AXIS, out double targetValue, 1, out UInt32 targetClock);

            //或者跟随误差
            GTN.mc.GTN_GetAxisErrorStatusLink(2, AXIS, out short pLinkAxis, out short pLinkMode, out short pCcount);

            //获取力矩轴的规划压力
            GTN.mc.GTN_GetPrfPress(2, AXIS, out double pPlanValue, out double pValueFilter);

            //获取力矩轴的实际压力
            GTN.mc.GTN_GetAtlPress(2, AXIS, out double pTarggetValue);

            //或者压力传感器模拟量的值
            short dacValue;
            rtn = GTN.mc.GTN_GetDac(1, 1, out short mpValue, 5, out uint p1Clock);

            //模拟量模块输入值
            //double adcValueF[4];
            rtn = GTN.mc.GTN_GetAuAdc(2, 1, out double pValue, 1, out UInt32 p2Clock);

            rtn = GTN.mc.GTN_GetAuAdc(2, 7, out double pValue1, 1, out UInt32 p3Clock);

            txt_pressStatus.Text = Pressstate.ToString();
            txt_axisStatus.Text = Axistate.ToString();

            txt_press.Text = this.isSmallForce ? "Null" : this.bondHeadController.GetBondForceCurrentVal(isSmallForce).ToString();

            txt_CaliTablePress.Text = this.system2Controller.GetCalibrateTableForceValue().ToString(); ;

            TxtLvdt.Text = this.isSmallForce
                               ? this.bondHeadController.GetBondForceCurrentVal(isSmallForce).ToString()
                               : "Null";
        }

        /// <summary>
        /// WatchOff
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            short rtn;
            short core = 1;

            rtn = GTN.mc.GTN_WatchOff(1);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core1 GTN_WatchOff" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_PrintWatch(1, "watchdataCore1.txt", 0, 0);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core1 GTN_PrintWatch" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_WatchOff(2);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core2 GTN_WatchOff" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_PrintWatch(2, "watchdataC  `ore2.txt", 0, 0);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core2 GTN_PrintWatch" + rtn.ToString());
            }

            isRecord = false;
        }

        /// <summary>
        /// Watch  ON
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            short rtn;
            short core = 1;

            rtn = GTN.mc.GTN_LoadWatchConfig(1, "watchTimerCore1.ini");
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core1GTN_LoadWatchConfig" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_LoadWatchConfig(2, "watchTimerCore2.ini");
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core2GTN_LoadWatchConfig" + rtn.ToString());
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            ForceConfig.GetInstance().SmallForceCalibrateInitialIncrement =
                (double)this.SpSmallForceCalibrateInitialVal.Value;

            ForceConfig.GetInstance().LVDTMaxInputForce = (double)this.SpLVDTMaxInput.Value;

            ForceConfig.GetInstance().ForceControlDebugAPos = Convert.ToDouble(this.txt_APos.Text);
            ForceConfig.GetInstance().ForceControlDebugBPos = Convert.ToDouble(this.txt_BPos.Text);
            ForceConfig.GetInstance().ForceControlDebugCPos = Convert.ToDouble(this.txt_CPos.Text);
            ForceConfig.GetInstance().ForceControlDebugBToCDistance =
                ForceConfig.GetInstance().ForceControlDebugBPos - ForceConfig.GetInstance().ForceControlDebugCPos;

            // 排序
            ForceConfig.GetInstance().Order();
            this.RefreshData();

            // 大力标定初始增量取力控配置中大力配置项的最小下限值
            ForceConfig.GetInstance().LargeForceCalibrateInitialIncrement = ForceConfig.GetInstance()
                .LargeForceConfigItemList.Select(it => it.ForceLowerLimit).Min();

            ForceConfig.GetInstance().Save();
            AKRSXtraMessageBox.Show("保存成功！");
        }

        /// <summary>
        /// 移动到标定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveToCaliPos_Click(object sender, EventArgs e)
        {
            AKRSPoint3D pos = BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos;

            this.bondModuleController.MoveToG0Pos(pos.X,pos.Y);
        }

        private void BtnChange_Click(object sender, EventArgs e)
        {
            // 判定是否小力
            double incrementForce = Convert.ToDouble(this.txt_pressTarget.Text)
                                    - this.bondHeadController.GetBondForceCurrentVal(this.isSmallForce);

            double angle = this.bondHeadController.GetAxisTRealPos();

            txt_ActualForce.Text = ForceCalibrationService
                .ForceIncrementToActualForce(incrementForce, this.isSmallForce, angle).ToString();
        }

        /// <summary>
        ///  新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ForceConfigItem forceConfigItem = new ForceConfigItem();

            FrmNewForceConfigItem frmNewForceConfigItem = new FrmNewForceConfigItem(forceConfigItem);

            // 获取显示器屏幕宽度,高度
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;
            frmNewForceConfigItem.Location = new Point(xWidth - 500, 250);
            frmNewForceConfigItem.StartPosition = FormStartPosition.Manual;
            if (frmNewForceConfigItem.ShowDialog() == DialogResult.OK)
            {
                if (this.isSmallForce)
                {
                    ForceConfig.GetInstance().SmallForceConfigItemList.Add(forceConfigItem);
                }
                else
                {
                    ForceConfig.GetInstance().LargeForceConfigItemList.Add(forceConfigItem);
                }

                this.RefreshData();
            }

            ForceConfig.GetInstance().Save();
        }

        /// <summary>
        /// 力控曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnForceCurve_Click(object sender, EventArgs e)
        {
            UcMainSystem.ShowForceRealTimeCurve();
        }

        /// <summary>
        ///  力控标定数据清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnForceCaliDataClean_Click(object sender, EventArgs e)
        {
            DialogResult dia = AKRSMessageBoxExt.Show(
                "确定清空旧的力控标定数据？",
                "提示",
                new string[] { "是", "否" },
                new DialogResult[] { DialogResult.Yes, DialogResult.No },
                AlarmLevel.SecondLevel);

            if (dia == DialogResult.Yes)
            {
              ForceCalibrationData.GetInstance().LargeForceRelationList.Clear();
              ForceCalibrationData.GetInstance().SmallForceRelationList.Clear();
              ForceCalibrationData.GetInstance().Save();
            }
        }

        /// <summary>
        /// LVDT曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLVDTCurve_Click(object sender, EventArgs e)
        {
            UcMainSystem.ShowLVDTRealTimeCurve();
        }

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            ForceConfigItem configItem = (ForceConfigItem)this.GvForceConfig.GetFocusedRow();

            if (configItem == null)
            {
                return;
            }

            DialogResult dia = AKRSXtraMessageBox.Show("确定删除？", "提示", MessageBoxButtons.OKCancel);

            if (dia == DialogResult.Cancel) 
            {
                    return;
            }

            // 移除
            if (this.isSmallForce)
            {
                ForceConfig.GetInstance().SmallForceConfigItemList.Remove(configItem);
            }
            else
            {
                ForceConfig.GetInstance().LargeForceConfigItemList.Remove(configItem);
            }

            ForceConfig.GetInstance().Save();

            this.RefreshData();
        }

        private void FrmGTForceTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer3.Stop();
            this.timer3.Tick -= timer3_Tick;
            this.timer3.Dispose();
        }

        /// <summary>
        /// 力类型下拉框改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbForceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.isSmallForce = this.CmbForceType.SelectedIndex == 1;

            this.RefreshData();
        }

        private Stopwatch sp = new Stopwatch();

        /// <summary>
        /// 力控下压集成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            double APos = Convert.ToDouble(txt_APos.Text);
            double BPos = Convert.ToDouble(txt_BPos.Text);
            double CPos = Convert.ToDouble(txt_CPos.Text);
            double force = Convert.ToDouble(txt_pressTarget.Text);

            ForceConfig.GetInstance().ForceControlDebugBToCDistance = BPos - CPos;

            int delay = (int)this.SpForceDelay.Value;
            int cycle = (int)this.SpTimes.Value;
            this.isSmallForce = this.CmbForceType.SelectedIndex == 1;
            this.isZeroBondhead = this.ChkZeroBondhead.Checked;

            // 慢速距离
            double slowTravelDistance = 0;

            isRecord = true;

            Task.Run(() =>
            {
                for (int i = 0; i < cycle; i++)
                {
                    if (this.isZeroBondhead)
                    {
                        ModbusService.GetInstance().ResetBondhead();
                    }

                    double bondHeadInitialVal = this.bondHeadController.GetBondForceCurrentVal(this.isSmallForce);

                    double caliTableInitialVal = this.system2Controller.GetCalibrateTableForceValue();

                    if (Math.Abs(caliTableInitialVal) > 2)
                    {
                        ModbusService.GetInstance().ResetManometer();
                        caliTableInitialVal = this.system2Controller.GetCalibrateTableForceValue();
                    }

                    // 快速运动到B点
                    this.bondHeadController.MoveAxisZ(BPos);

                    this.StartForceRealTimeCurve();

                    FrmLVDTRealTimeCurve.ReadLVDT(true);

                    this.sp.Restart();

                    // 切换通道并设置压力参数
                    this.bondHeadController.ChangeChannelAndSetForceControlPara(this.isSmallForce);

                    // 开启力控
                    this.bondHeadController.GTForceControlSet(force, BPos, slowTravelDistance, this.isSmallForce);

                    double forceSetTime= this.sp.ElapsedMilliseconds;

                    this.BeginInvoke(() =>
                    {
                        this.SpTime.EditValue = forceSetTime;
                    });

                    Thread.Sleep(delay);

                    double curZPos = this.bondHeadController.GetAxisZRealPos();

                    double curTPos = this.bondHeadController.GetAxisTRealPos();

                    double touchBondHeadVal = this.bondHeadController.GetBondForceCurrentVal(this.isSmallForce);

                    double touchForce = this.system2Controller.GetCalibrateTableForceValue() - caliTableInitialVal;

                    this.BeginInvoke(() => { this.txt_ActualForce.Text = touchForce.ToString(); });

                    double preLiftPos = this.bondHeadController.GetAxisZRealPos() + 2;

                    // 力控上抬
                    this.bondHeadController.GTForceControlReset(APos, 200);

                    this.StopForceRealTimeCurve();
                    FrmLVDTRealTimeCurve.ReadLVDT(false);

                    //this.bondHeadController.MoveAxisZ(APos);

                    double bondHeadValAfterUp = this.bondHeadController.GetBondForceCurrentVal(this.isSmallForce);

                    //this.SaveForceControlData(
                    //    force,
                    //    curTPos,
                    //    bondHeadInitialVal,
                    //    curZPos,
                    //    touchBondHeadVal,
                    //    touchForce,
                    //    bondHeadValAfterUp,
                    //    forceSetTime);
                }

                // 开始读焊头力
                UcMainSystem.ActiveReadBondForce(false);
            });
        }
        short AXIS = 3;

        GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();


        /// <summary>
        /// 力控曲线开始读取
        /// </summary>
        private void StartForceRealTimeCurve()
        {
            FrmForceRealTimeCurve.ForceReadTiming = ForceReadTimingEnum.TestOnCaliTable;

            // 开始读焊头力
            UcMainSystem.ActiveReadBondForce(true);
        }

        /// <summary>
        /// 力控曲线结束读取
        /// </summary>
        private void StopForceRealTimeCurve()
        {
            // 开始读焊头力
            UcMainSystem.ActiveReadBondForce(false);
        }


        /// <summary>
        /// 轴停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStartMove_Click(object sender, EventArgs e)
        {
            short rtn;
            for (short core = 1; core <= 2; core++)
            {
                for (short axis = 1; axis <= 12; axis++)
                {
                    rtn = GTN.mc.GTN_Stop(core, 1 << (axis - 1), 0);
                }
                rtn = GTN.mc.GTN_StopCommandList(core, 1, 0, ref listInfo);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inputForce"></param>
        /// <param name="angle"></param>
        /// <param name="beforeTouchForce"></param>
        /// <param name="touchForce"></param>
        /// <param name="riseForce"></param>
        private void SaveForceControlData(double inputForce, double angle, double beforeTouchForce, double zPos, double touchBondheadForce, double touchForce, double riseForce, double forceSetTime)
        {
            try
            {
                lock (this)
                {
                    // 添加一个工作表
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + "焊头稳定性测试数据" + DateTime.Now.ToString("yyMMdd")
                            + ".xlsx"));

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                                   ? excelPackage.Workbook.Worksheets[0]
                                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

                    // 设置列宽
                    for (int i = 1; i <= 10; i++)
                    {
                        worksheet.Column(i).Width = 15;
                    }

                    // 添加标题行
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "Time";

                        worksheet.Cells[1, 2].Value = "输入模拟量";

                        worksheet.Cells[1, 3].Value = "角度";

                        worksheet.Cells[1, 4].Value = "未接触前力g";

                        worksheet.Cells[1, 5].Value = "接触时的Z轴坐标";

                        worksheet.Cells[1, 6].Value = "接触时焊头的力g";

                        worksheet.Cells[1, 7].Value = "接触时平台压力传感器的力g";

                        worksheet.Cells[1, 8].Value = "抬起后力g";

                        worksheet.Cells[1, 9].Value = "力控下压到力控抬起时间(ms)";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value = inputForce;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = angle;

                    worksheet.Cells[lastUsedRow + 1, 4].Value = beforeTouchForce;


                    worksheet.Cells[lastUsedRow + 1, 5].Value = pos;

                    worksheet.Cells[lastUsedRow + 1, 6].Value = touchBondheadForce;
                    worksheet.Cells[lastUsedRow + 1, 7].Value = touchForce;

                    worksheet.Cells[lastUsedRow + 1, 8].Value = riseForce;

                    worksheet.Cells[lastUsedRow + 1, 9].Value = forceSetTime;

                    excelPackage.Save();
                }
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"文件已被打开，保存定位数据失败!\r\n Retry:关掉文件重新保存\r\nCancel:不保存数据",
                    "报警",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.Retry)
                {
                    SaveForceControlData(inputForce, angle, beforeTouchForce, pos, touchBondheadForce, touchForce, riseForce, forceSetTime);
                }
            }
        }
    }
}