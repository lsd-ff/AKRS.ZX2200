using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.Infrastructure.Controls.Common;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;

using DataAnalysis.Acquisition;

using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using Galaxy2.PR.Resipository;
using BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using BondSystem.Modules;
using Infrastructure.Utils;
using ch.etel.edi.dsa.v40;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using global::GlobalCalibration.GlobalCalibration;
using LanguageExt;
using DevExpress.XtraBars;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting.Native;
using AKRS.Galaxy2.UserManager;
using System.Text;
using DevExpress.Office.Utils;
using AKRS.ZX2200.Infrastructure.Controls.Currency;

/// <summary>
/// 二维补偿实验窗体
/// </summary>
public partial class FrmGlobalCalibrationEx : XtraForm
{
    /// <summary>
    /// 全局标定
    /// </summary>
    private GlobalCalibrationDomainEx GlobalCalibrationDomain => GlobalCalibrationDomainEx.GetInstance();

    /// <summary>
    /// system2控制器
    /// </summary>
    private System2Controller system2Controller = new();

    /// <summary>
    /// Bond模组
    /// </summary>
    private readonly BondModule bondModule = new();

    /// <summary>
    /// 是否编辑过该界面
    /// </summary>
    private bool isEdited = false;

    /// <summary>
    /// 标定窗体
    /// </summary>
    public FrmGlobalCalibrationEx()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// 获取轴的坐标
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">封装参数</param>
    private async void BtGetStartPos_Click(object sender, EventArgs e)
    {
        //if (this.ChkSimRun.Checked)
        //{
        //    await this.ShowOverlayAsync("正在移动焊头使当前Mark到视野中心,请稍等...", () =>
        //    {
        //        Thread.Sleep(3000);
        //    });
        
        //    return;
        //}

        this.MoveToCameraCenter();
        this.SpStartX.EditValue = this.bondModule.BondAxisX.GetRealPosition();
        this.SpStartY.EditValue = this.bondModule.BondAxisY.GetRealPosition();
        this.SpStartZ.EditValue = this.bondModule.BondHead.AxisZ.GetRealPosition();
        this.Save();
    }

    /// <summary>
    /// 制作PR
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">封装参数</param>
    private void BtEditPR_Click(object sender, EventArgs e)
    {
        string name = this.TxPrName.Text;

        PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
        BaseVisionEntity visionEntity = null;
        if (prEntity != null)
        {
            visionEntity = prEntity;
        }
        else
        {
            prEntity = new(name);
            prEntity.Alg.AlgBeLong = AlgBeLongEnum.Substrate;


            visionEntity = prEntity;

            VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
        }

        prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(CameraTypeEnum.BondCamera));

        FrmPREditor editor = new((PREntity)visionEntity, false);

        editor.ShowDialog();

        VisionEntityRepository.GetInstance().Save();
    }

    private void RefreshPermission()
    {
        bool isAdmin = UserManagerContext.GetInstance().UserCache.Account == "Admin";
        this.ChkSimRun.Enabled = isAdmin;
        this.BtnSimGenerateScanDatas.Enabled = isAdmin;
        this.GvCompensatorRegions.OptionsBehavior.Editable = isAdmin;
        this.BtnRemoveSelectedRegion.Enabled = isAdmin;
        this.BtnClearPoints.Enabled = isAdmin;
    }


    /// <summary>
    /// 窗体加载事件
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">封装参数</param>
    private void FrmGlobalCalibration_Load(object sender, EventArgs e)
    {
        this.ChkCaptureTd_CheckedChanged(null, null); // this.RefreshPermission();

        this.SpStartX.EditValue = this.GlobalCalibrationDomain.StartPoint3D.X;
        this.SpStartY.EditValue = this.GlobalCalibrationDomain.StartPoint3D.Y;
        this.SpStartZ.EditValue = this.GlobalCalibrationDomain.StartPoint3D.Z;

        this.SpColumnSpacing.Binding(this.GlobalCalibrationDomain,
            nameof(this.GlobalCalibrationDomain.ColumnSpacing));

        this.SpRowSpacing.Binding(this.GlobalCalibrationDomain, nameof(this.GlobalCalibrationDomain.RowSpacing));


        this.SpLeftCols.Binding(this.GlobalCalibrationDomain, nameof(this.GlobalCalibrationDomain.LeftColCount));
        this.SpRightCols.Binding(this.GlobalCalibrationDomain, nameof(this.GlobalCalibrationDomain.RightColCount));
        this.SpUpperRows.Binding(this.GlobalCalibrationDomain, nameof(this.GlobalCalibrationDomain.UpRowCount));
        this.SpLowerRows.Binding(this.GlobalCalibrationDomain, nameof(this.GlobalCalibrationDomain.DownRowCount));
        this.SpPrDelay.Binding(this.GlobalCalibrationDomain, nameof(this.GlobalCalibrationDomain.Delay));
        this.TxPrName.Binding(this.GlobalCalibrationDomain, nameof(this.GlobalCalibrationDomain.PrName));

        this.GcPreDefinedPoints.DataSource = this.GlobalCalibrationDomain.ScanData;
        this.GcCompersatorRegions.DataSource = this.GlobalCalibrationDomain.Compensator.Regions;

        this.GvPreDefinedPoints.OptionsBehavior.Editable = false;
        this.BtnPauseScan.Enabled = false;
        this.BtnStopScan.Enabled = false;

        this.GetChildControls<SimpleButton>().Iter(btn => { btn.Click += (s, e) => { this.isEdited = true; }; });

        this.GetChildControls<BaseEdit>().Iter(btn => { btn.EditValueChanged += (s, e) => { this.isEdited = true; }; });

        this.GvCompensatorRegions.DoubleClick += (o, args) =>
        {
            GridView view = this.GvCompensatorRegions;

            Point pt = view.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = view.CalcHitInfo(pt);


            if (!info.InRow && !info.InRowCell)
            {
                return;
            }

            int rowHandle = info.RowHandle;

            object rowData = view.GetRow(rowHandle);

            if (rowData is not RegionCompensator region)
            {
                return;
            }

            FrmCompensationRegionView editor = new(region);
            editor.ShowDialog();
            this.GlobalCalibrationDomain.Compensator.Regions[rowHandle] = region;
            this.GcCompersatorRegions.RefreshDataSource();
        };
    }

    /// <summary>
    /// 关闭事件
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">封装参数</param>
    private void FrmGlobalCalibration_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (this.BtnStartBoot.Enabled)
        {
            if (!this.isEdited)
            {
                return;
            }

            DialogResult dialogResult = AKRSXtraMessageBox.Show($"是否需要保存已经配置过的补偿信息", "提示", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                this.Save();
            }

            return;
        }

        e.Cancel = true;

        AKRSXtraMessageBox.Show($"当前正在运行，请先结束当前任务再关闭此页面", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    /// <summary>
    /// 移动到相机中心
    /// </summary>
    private void MoveToCameraCenter()
    {
        // 寻找Pr模板
        PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.GlobalCalibrationDomain.PrName);

        while (true)
        {
            Thread.Sleep(50);

            // 执行定位
            ExcuteResult p1Result = pREntity.DoWork();

            // 定位失败，直接返回
            if (p1Result != ExcuteResult.Success)
            {
                AKRSXtraMessageBox.Show("Vision fail");
                return;
            }

            // 定位结果
            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            if (Math.Abs(matchResult.CenterX - 1224) < 0.1 && Math.Abs(matchResult.CenterY - 1024) < 0.1)
            {
                return;
            }

            AKRSPoint3D pos = this.bondModule.ConvertPixelToG0Pos(this.bondModule.Get3DRealPosition(), matchResult);
            this.bondModule.MoveToG0Pos(pos);
        }
    }

    /// <summary>
    /// 保存
    /// </summary>
    private void Save()
    {
        this.GlobalCalibrationDomain.StartPoint3D = new(
            (double)this.SpStartX.Value,
            (double)this.SpStartY.Value,
            (double)this.SpStartZ.Value);

        this.GlobalCalibrationDomain.ColumnSpacing = (double)this.SpColumnSpacing.Value;
        this.GlobalCalibrationDomain.RowSpacing = (double)this.SpRowSpacing.Value;
        this.GlobalCalibrationDomain.PrName = this.TxPrName.Text;
        this.GlobalCalibrationDomain.Delay = (int)this.SpPrDelay.Value;
        this.GlobalCalibrationDomain.Save();
    }

    /// <summary>
    /// 移动XY
    /// </summary>
    /// <param name="x">x轴</param>
    /// <param name="y">Y轴</param>
    private void MoveXY(double x, double y)
    {
        if (this.bondModule.BondAxisX.AxisDrive is ETELAxis)
        {
            // 设置群组
            DsaIpolGroup iGroup = System2Domain.GetInstance().BondModuleController.GetXYIpolGroup();


            // 开始插补
            iGroup.ipolBegin();

            // 设置为绝对坐标系 ，不设置绝对坐标系
            iGroup.ipolSetAbsMode(true, -1);

            iGroup.ipolTanVelocity(0.05);
            iGroup.ipolTanAcceleration(0.5);
            iGroup.ipolTanDeceleration(0.5);

            // 设置抖动时间，不知道什么意思
            iGroup.ipolTanJerkTime(0.01);

            iGroup.ipolLine(x / 1000.0, y / 1000.0);

            // 等待插补结束
            iGroup.ipolWaitMovement(100000);

            // 退出插补模式
            iGroup.ipolEnd();
        }
        else
        {
            this.bondModule.MoveBondXY(x, y);
        }
    }

    /// <summary>
    /// 获取Mark在机器坐标系的位置
    /// </summary>
    /// <param name="matchResult"></param>
    /// <returns></returns>
    private AKRSPoint2D GetAxesPosOfMark(MatchResult matchResult)
    {
        AKRSPoint3D point3D = this.bondModule.ConvertPixelToG0Pos(this.bondModule.Get3DRealPosition(), matchResult);
        var retPoint = this.bondModule.ConvertG0ToMachinePos(point3D);
        return new(retPoint.X, retPoint.Y);
    }

    /// <summary>
    /// 自动扫描预定义点位
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnAutoScanPreDefinePoints_Click(object sender, EventArgs e)
    {
        DialogResult dialogResult = AKRSXtraMessageBox.Show(
            $"即将开始自动扫描预定义点位(当前预定义点数16点)，请确认以下事项：{Environment.NewLine}1.Bond模组在视觉高度{Environment.NewLine}2.当前定位点位于中心或左下区域{Environment.NewLine}3.重新扫描会覆盖已生成的扫描点位列表",
            "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (dialogResult != DialogResult.Yes)
        {
            return;
        }

        List<GlobalCalibrationData> data = new();

        double columnSpacing = this.GlobalCalibrationDomain.ColumnSpacing;

        double rowSpacing = this.GlobalCalibrationDomain.RowSpacing;

        sender.UIActionAsync(() =>
        {
            AKRSPoint2D currentPos = this.bondModule.Get2DRealPosition();
            int rowDir = 1;
            for (int i = 0; i < 4; i++)
            {
                this.bondModule.MoveBondXY(currentPos.X, currentPos.Y + rowSpacing);
                for (int j = 0; j < 4; j++)
                {
                    this.bondModule.MoveBondXY(currentPos.X + columnSpacing * rowDir, currentPos.Y);
                    this.MoveToCameraCenter();
                    currentPos = this.bondModule.Get2DRealPosition();
                    data.Add(new()
                    {
                        WorldX = j * columnSpacing,
                        WorldY = i * rowSpacing,
                        AxisX = currentPos.X,
                        AxisY = currentPos.Y
                    });
                }

                rowDir *= -1;
            }

            this.BeginInvoke((MethodInvoker)(() =>
            {
                this.GlobalCalibrationDomain.ScanData.Clear();
                this.GlobalCalibrationDomain.ScanData = data;
                this.GlobalCalibrationDomain.Save();
                this.GcPreDefinedPoints.RefreshDataSource();
            }));
        });
    }

    /// <summary>
    /// 根据预定义点位生成扫描点列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnGeneratePoints_Click(object sender, EventArgs e)
    {
        if (this.GlobalCalibrationDomain.ScanData.Count < 3)
        {
            AKRSXtraMessageBox.Show($"生成前需确保预定义点位数量至少为3个", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 计算轴坐标到世界坐标的转换关系
        List<AKRSPoint2D> sourcePoints = new();
        List<AKRSPoint2D> targetPoints = new();
        foreach (GlobalCalibrationData data in this.GlobalCalibrationDomain.ScanData)
        {
            sourcePoints.Add(new(data.WorldX, data.WorldY));
            targetPoints.Add(new(data.AxisX, data.AxisY));
        }

        double[,] fromWorldToAxis = MathService.FitTransMatrix(sourcePoints, targetPoints);

        using FileStream fs = new FileStream($"D:\\2025二维标定数据采集\\标定片坐标系到轴坐标系传递矩阵.txt", mode: FileMode.OpenOrCreate);
        using StreamWriter sw = new StreamWriter(fs);
        int rows = fromWorldToAxis.GetLength(0);
        int cols = fromWorldToAxis.GetLength(1);

        sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}通过自定义点位构建:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                sw.Write(fromWorldToAxis[i, j]);
                sw.Write("\t");
            }

            sw.Write(Environment.NewLine);
        }


        int leftCols = Convert.ToInt32(this.SpLeftCols.EditValue);
        int rightCols = Convert.ToInt32(this.SpRightCols.EditValue);
        int upRows = Convert.ToInt32(this.SpUpperRows.EditValue);
        int downRows = Convert.ToInt32(this.SpLowerRows.EditValue);

        double rowSpace = Convert.ToDouble(this.SpRowSpacing.EditValue);
        double colSpace = Convert.ToDouble(this.SpRowSpacing.EditValue);

        // 计算轴坐标系下的步长
        // AKRSPoint2D calibBoardStep = new AKRSPoint2D(this.GlobalCalibrationDomain.ColumnSpacing, this.GlobalCalibrationDomain.RowSpacing);
        // var axisStep = MathService.TransformPoint(calibBoardStep, fromWorldToAxis);
        // this.GlobalCalibrationDomain.XStep = axisStep.X;
        // this.GlobalCalibrationDomain.YStep = axisStep.Y;

        List<GlobalCalibrationData> dataToFill = new();
        dataToFill.AddRange(this.GlobalCalibrationDomain.ScanData);

        // 第一象限
        List<GlobalCalibrationData> firstQuadrant = this.CalcAndFillQuadrant(dataToFill,
            upRows, rightCols, rowSpace, colSpace);
        dataToFill.AddRange(firstQuadrant);

        // 第二象限
        List<GlobalCalibrationData> secondQuadrant = this.CalcAndFillQuadrant(dataToFill,
            upRows, -leftCols, rowSpace, colSpace);
        dataToFill.AddRange(secondQuadrant);

        // 第三象限
        List<GlobalCalibrationData> thirdQuadrant = this.CalcAndFillQuadrant(dataToFill,
            -downRows, -leftCols, rowSpace, colSpace);
        dataToFill.AddRange(thirdQuadrant);

        // 第四象限
        List<GlobalCalibrationData> fourthQuadrant = this.CalcAndFillQuadrant(dataToFill,
            -downRows, rightCols, rowSpace, colSpace);
        dataToFill.AddRange(fourthQuadrant);

        dataToFill.Iter(d =>
        {
            AKRSPoint2D axisPoint = MathService.TransformPoint(new(d.WorldX, d.WorldY), fromWorldToAxis);
            d.AxisX = axisPoint.X;
            d.AxisY = axisPoint.Y;
        });

        this.GlobalCalibrationDomain.ScanData.Clear();
        this.GlobalCalibrationDomain.ScanData.AddRange(dataToFill);
        this.GlobalCalibrationDomain.Save();
        this.GcPreDefinedPoints.RefreshDataSource();
    }

    /// <summary>
    /// 计算并填充每个象限数据
    /// </summary>
    /// <param name="sourceList"></param>
    /// <param name="rowCount"></param>
    /// <param name="colCount"></param>
    private List<GlobalCalibrationData> CalcAndFillQuadrant(List<GlobalCalibrationData> sourceList, int rowCount,
        int colCount, double rowSpace, double colSpace)
    {
        if (rowCount == 0 || colCount == 0)
        {
            throw new ArgumentOutOfRangeException($"{nameof(colCount)} / {nameof(rowCount)}", "行/列数不能为0");
        }

        // 根据rowCount、colCount判断象限
        int quadrant = 0;
        if (rowCount > 0 && colCount > 0)
        {
            quadrant = 1;
        }
        else if (colCount < 0 && rowCount > 0)
        {
            quadrant = 2;
        }
        else if (colCount < 0 && rowCount < 0)
        {
            quadrant = 3;
        }
        else
        {
            quadrant = 4;
        }

        int colSign = quadrant is 1 or 4 ? 1 : -1;
        int rowSign = quadrant is 1 or 2 ? 1 : -1;
        List<GlobalCalibrationData> dataToAdd = new();
        for (int i = 0; i < Math.Abs(rowCount) + 1; i++)
        {
            for (int j = 0; j < Math.Abs(colCount) + 1; j++)
            {
                double worldX = j * colSpace * colSign;
                double worldY = i * rowSpace * rowSign;
                if (sourceList.Exists(d => MathService.DoubleEqual(d.WorldX, worldX) && MathService.DoubleEqual(d.WorldY, worldY)))
                {
                    continue;
                }

                dataToAdd.Add(new() { WorldX = worldX, WorldY = worldY });
            }
        }

        return dataToAdd;
    }

    private void BtnSortScanPoints_Click(object sender, EventArgs e)
    {
        List<GlobalCalibrationData> sortedData = GlobalCalibrationData.Sort(this.GlobalCalibrationDomain.ScanData);
        this.GlobalCalibrationDomain.ScanData.Clear();
        this.GlobalCalibrationDomain.ScanData.AddRange(sortedData);
        this.GcPreDefinedPoints.RefreshDataSource();
    }

    private CancellationTokenSource cts;
    private ManualResetEvent pauseSignal;

    /// <summary>
    /// 启动选择的启动项
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnStartBoot_Click(object sender, EventArgs e)
    {
        if (this.ComBoxBootItems.SelectedIndex == -1)
        {
            AKRSXtraMessageBox.Show($"请先选择启动项！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
        }

        bool simulation = this.ChkSimRun.Checked;
        sender.UIActionAsync(async () =>
        {
            this.pauseSignal = new(true);
            this.cts = new();

            this.Invoke((MethodInvoker)(() =>
            {
                this.BtnPauseScan.Enabled = true;
                this.BtnPauseScan.Text = "暂停";
                this.BtnStopScan.Enabled = true;
                this.ComBoxBootItems.Enabled = false;
                this.ChkSimRun.Enabled = false;
                this.GcCapTDConfig.Enabled = false;
            }));
   
            try
            {
                if (this.ComBoxBootItems.SelectedIndex == 0)
                {
                    await this.ScanPoints(simulation, this.cts.Token).ConfigureAwait(false);
                }
                else
                {
                    await this.ScanPointsWithCompensation(simulation, this.cts.Token).ConfigureAwait(false);
                }
            }
            finally
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    this.BtnPauseScan.Enabled = false;
                    this.BtnStopScan.Enabled = false;
                    this.ComBoxBootItems.Enabled = true;
                    this.ChkSimRun.Enabled = true;
                    this.GcCapTDConfig.Enabled = true;
                    this.GcPreDefinedPoints.RefreshDataSource();
                }));
            }

            this.RunToCompleteEvent?.BeginInvoke(sender, e, null, null);
        });
    }

    private event Action<object, EventArgs> RunToCompleteEvent;

    /// <summary>
    /// 选中指定数据行
    /// </summary>
    /// <param name="data"></param>
    private void RefreshCurrentRow(GlobalCalibrationData data)
    {
        if (this.InvokeRequired)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                this.RefreshCurrentRow(data);
            }));
        }
        else
        {
            GridView gridView = this.GvPreDefinedPoints;
            int index = this.GlobalCalibrationDomain.ScanData.IndexOf(data);
            int rowHandle = gridView.GetRowHandle(index);
            gridView.FocusedRowHandle = rowHandle;
            gridView.SelectRow(rowHandle);
            gridView.MakeRowVisible(rowHandle, false);
        }
    }

    /// <summary>
    /// 扫描所有点
    /// </summary>
    /// <param name="simRunFlag"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private Task ScanPoints(bool simRunFlag, CancellationToken ct)
    {
        if (simRunFlag)
        {
            return Task.Run(async () =>
            {
                string fileName = $"D:\\2025二维标定数据采集\\新数据采集{DateTime.Now:HHmmss}";
                IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"二维标定新数据采集{DateTime.Now:HHmmss}")
                    .ConfigureFileOutput($"{fileName}.json");

                await Task.Run(async () =>
                {
                    int count = 0;

                    foreach (GlobalCalibrationData data in this.GlobalCalibrationDomain.ScanData)
                    {
                        this.pauseSignal.WaitOne();
                        if (this.cts.IsCancellationRequested)
                        {
                            break;
                        }

                        this.RefreshCurrentRow(data);

                        count++;
                        await Task.Delay(this.GlobalCalibrationDomain.Delay);

                        Random random = new();

                        // 计算Mark在轴坐标系中的位置
                        data.ScanAxisX = data.AxisX + random.NextDouble() * 0.004 - 0.002;
                        data.ScanAxisY = data.AxisY + random.NextDouble() * 0.004 - 0.002;

                        dataLog.AddData("轴指令位置", new { X = data.AxisX, Y = data.AxisY });
                        dataLog.AddData("Mark轴坐标", new { X = data.ScanAxisX, Y = data.ScanAxisY });
                        dataLog.CommitData();

                        if (count % 10 == 0)
                        {
                            await dataLog.FlushAsync();
                        }
                    }

                    this.GlobalCalibrationDomain.Save();
                }, this.cts.Token).ConfigureAwait(false);
            }, ct);
        }
        else
        {
            return Task.Run(async () =>
            {
                string fileName = $"D:\\2025二维标定数据采集\\新数据采集{DateTime.Now:HHmmss}";
                IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"二维标定新数据采集{DateTime.Now:HHmmss}")
                    .ConfigureFileOutput($"{fileName}.json");

                Stopwatch captureTime = this.ChkCaptureTd.Checked && this.CmbTDCaptureConfig.SelectedIndex == 1 ? new Stopwatch() : null;
                int captureInterval = this.SpTdCapCycleInterval.TryGetInt32();

                // 寻找Pr模板
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                    .Find(this.GlobalCalibrationDomain.PrName);


                double startPosX = Convert.ToDouble(this.SpStartX.EditValue);
                double startPosY = Convert.ToDouble(this.SpStartY.EditValue);
                double startPosZ = Convert.ToDouble(this.SpStartZ.EditValue);

                // 确保处于视觉高度
                System2Domain.GetInstance().BondModuleController
                    .MoveSafeBondXYZ(new(startPosX, startPosY, startPosZ));

                try
                {
                    await Task.Run(async () =>
                    {
                        int count = 0;
                        captureTime?.Start();
                        (MatchResult matchResult, AKRSPoint2D axisPos) tdMarkResult = default;
                        foreach (GlobalCalibrationData data in this.GlobalCalibrationDomain.ScanData)
                        {
                            this.pauseSignal.WaitOne();

                            if (this.cts.IsCancellationRequested)
                            {
                                break;
                            }

                            bool isUpLookMarkCaptured = false;
                            
                            if (this.ChkCaptureTd.Checked)
                            {
                                if (count == 0)
                                {
                                    tdMarkResult = this.VisionTpMark();
                                    isUpLookMarkCaptured = true;
                                }
                                else if (this.CmbTDCaptureConfig.SelectedIndex == 0)
                                {
                                    if (count % captureInterval == 0)
                                    {
                                        tdMarkResult = this.VisionTpMark();
                                        isUpLookMarkCaptured = true;
                                    }
                                }
                                else if (this.CmbTDCaptureConfig.SelectedIndex == 1)
                                {
                                    if (captureTime != null && captureTime.Elapsed >= this.TsTDCapture.TimeSpan)
                                    {
                                        tdMarkResult = this.VisionTpMark();
                                        captureTime = Stopwatch.StartNew();
                                        isUpLookMarkCaptured = true;
                                    }
                                }
                            }

                            this.RefreshCurrentRow(data);
                            if (isUpLookMarkCaptured)
                            {
                                if (pREntity != null)
                                {
                                    pREntity.SetHardware(this.bondModule.GetHardware());
                                    pREntity.SetLight();
                                }

                                System2Domain.GetInstance().BondModuleController
                                    .MoveSafeBondXYZ(new(data.AxisX, data.AxisY, startPosZ));
                            }
                            else
                            {
                                this.MoveXY(data.AxisX, data.AxisY);
                            }
                            
                            await Task.Delay(this.GlobalCalibrationDomain.Delay);

                            // 执行定位
                            ExcuteResult p1Result = pREntity.DoWork(this.bondModule.GetHardware(),false);

                            // 定位失败，直接返回
                            if (p1Result == ExcuteResult.Success)
                            {
                                // 定位结果
                                MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                                // 计算Mark在轴坐标系中的位置
                                AKRSPoint2D markPosInAxis = this.GetAxesPosOfMark(matchResult);
                                data.ScanAxisX = markPosInAxis.X;
                                data.ScanAxisY = markPosInAxis.Y;


                                AKRSPoint2D encoder = this.bondModule.Get2DRealPosition();
                                AKRSPoint2D cmdPos = new AKRSPoint2D(data.AxisX, data.AxisY);
                                dataLog.AddData("标定板Mark坐标", new { X = data.WorldX, Y = data.WorldY });
                                dataLog.AddData("轴指令位置", cmdPos.Export());
                                dataLog.AddData("轴编码器位置", encoder.Export());
                                dataLog.AddData("Mark像素坐标", matchResult.Export());
                                dataLog.AddData("Mark轴坐标", markPosInAxis.Export());
                                dataLog.AddData("Offset", (markPosInAxis - encoder).Export());
                                if (this.ChkCaptureTd.Checked)
                                {
                                    dataLog.AddData("温漂Mark拍摄结果", tdMarkResult.matchResult.Export());
                                    dataLog.AddData("温漂Mark轴坐标", tdMarkResult.axisPos.Export());
                                    dataLog.AddData("温漂Mark拍摄坐标",
                                        BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1.Export());
                                }

                                if (this.ChkUsingPredictor.Checked)
                                {
                                    AKRSPoint3D pos = BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1;
                                    AKRSPoint2D predictOffset =
                                        this.Predict(new(pos.X, pos.Y), tdMarkResult.axisPos, cmdPos);
                                    dataLog.AddData("预测补偿值um", predictOffset.Export());
                                }

                                dataLog.CommitData();
                                count++;
                            }
                            else
                            {
                                throw new("定位失败，请检查标定板是否有问题");
                            }

                            if (count % 10 == 0)
                            {
                                await dataLog.FlushAsync();
                            }
                        }

                        this.GlobalCalibrationDomain.Save();
                    }, this.cts.Token).ConfigureAwait(false);
                }
                finally
                {
                    await dataLog.FlushAsync();
                }
            }, ct);
        }
    }

    /// <summary>
    /// 温漂标定片定位
    /// </summary>
    private (MatchResult matchResult, AKRSPoint2D axisPos) VisionTpMark()
    {
        AKRSPoint3D visionPos = System2Domain.GetInstance().BondModuleController.ConvertMachineToG0Pos(BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);

        // 温漂定位
        MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
            visionPos,
            "温漂定位",
            "上视温漂测试Mark");

        AKRSPoint2D axisPos = this.GetAxesPosOfMark(driftMatchResult);

        return (driftMatchResult, axisPos);
    }

    /// <summary>
    /// 带补偿扫描所有点，需要提前扫描过一遍所有点
    /// </summary>
    /// <param name="simRunFlag"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    private Task ScanPointsWithCompensation(bool simRunFlag, CancellationToken ct)
    {
        if (simRunFlag)
        {
            return Task.Run(async () =>
            {
                string fileName = $"D:\\2025二维标定数据采集\\新数据采集带补偿{DateTime.Now:HHmmss}";
                IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"二维标定新数据采集带补偿{DateTime.Now:HHmmss}")
                    .ConfigureFileOutput($"{fileName}.json");

                BilinearCompensator compensator = this.GlobalCalibrationDomain.Compensator;
                await Task.Run(async () =>
                {
                    int count = 0;

                    foreach (GlobalCalibrationData data in this.GlobalCalibrationDomain.ScanData)
                    {
                        this.pauseSignal.WaitOne();
                        if (this.cts.IsCancellationRequested)
                        {
                            break;
                        }

                        this.RefreshCurrentRow(data);

                        count++;
                        await Task.Delay(this.GlobalCalibrationDomain.Delay);

                        AKRSPoint2D compensation = compensator.Interpolate(data.AxisX, data.AxisY);

                        dataLog.AddData("实际扫描位置",
                            new { X = data.AxisX + compensation.X, Y = data.AxisY + compensation.Y });
                        dataLog.AddData("补偿值", compensation.Export());
                        AKRSPoint2D markAxisPos = data.ScanAxisX == 0 && data.ScanAxisY == 0
                            ? new(data.AxisX + compensation.X, data.AxisY + compensation.Y)
                            : new AKRSPoint2D(data.ScanAxisX, data.ScanAxisY);
                        dataLog.AddData("Mark轴坐标", markAxisPos.Export());
                        dataLog.CommitData();

                        if (count % 10 == 0)
                        {
                            await dataLog.FlushAsync();
                        }
                    }
                }, this.cts.Token).ConfigureAwait(false);
            }, ct);
        }
        else
        {
            return Task.Run(async () =>
            {
                string fileName = $"D:\\2025二维标定数据采集\\新数据采集带补偿{DateTime.Now:HHmmss}";
                IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"二维标定新数据采集带补偿{DateTime.Now:HHmmss}")
                    .ConfigureFileOutput($"{fileName}.json");

                Stopwatch captureTime = this.ChkCaptureTd.Checked && this.CmbTDCaptureConfig.SelectedIndex == 1 ? new Stopwatch() : null;
                int captureInterval = this.SpTdCapCycleInterval.TryGetInt32();

                BilinearCompensator compensator = this.GlobalCalibrationDomain.Compensator;

                // 寻找Pr模板
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                    .Find(this.GlobalCalibrationDomain.PrName);

                double startPosX = Convert.ToDouble(this.SpStartX.EditValue);
                double startPosY = Convert.ToDouble(this.SpStartY.EditValue);
                double startPosZ = Convert.ToDouble(this.SpStartZ.EditValue);

                // 确保处于视觉高度
                System2Domain.GetInstance().BondModuleController
                    .MoveSafeBondXYZ(new(startPosX, startPosY, startPosZ));

                try
                {
                    await Task.Run(async () =>
                    {
                        int count = 0;
                        captureTime?.Start();
                        (MatchResult matchResult, AKRSPoint2D axisPos) tdMarkResult = default;
                        foreach (GlobalCalibrationData data in this.GlobalCalibrationDomain.ScanData)
                        {
                            this.pauseSignal.WaitOne();

                            if (this.cts.IsCancellationRequested)
                            {
                                break;
                            }

                            bool isUpLookMarkCaptured = false;

                            if (this.ChkCaptureTd.Checked)
                            {
                                if (count == 0)
                                {
                                    tdMarkResult = this.VisionTpMark();
                                    isUpLookMarkCaptured = true;
                                }
                                else if (this.CmbTDCaptureConfig.SelectedIndex == 0)
                                {
                                    if (count % captureInterval == 0)
                                    {
                                        tdMarkResult = this.VisionTpMark();
                                        isUpLookMarkCaptured = true;
                                    }
                                }
                                else if (this.CmbTDCaptureConfig.SelectedIndex == 1)
                                {
                                    if (captureTime != null && captureTime.Elapsed >= this.TsTDCapture.TimeSpan)
                                    {
                                        tdMarkResult = this.VisionTpMark();
                                        captureTime = Stopwatch.StartNew();
                                        isUpLookMarkCaptured = true;
                                    }
                                }
                            }

                            this.RefreshCurrentRow(data);

                            AKRSPoint2D compensation = compensator.Interpolate(data.AxisX, data.AxisY);

                            if (isUpLookMarkCaptured)
                            {
                                if (pREntity != null)
                                {
                                    pREntity.SetHardware(this.bondModule.GetHardware());
                                    pREntity.SetLight();
                                }
                                System2Domain.GetInstance().BondModuleController
                                    .MoveSafeBondXYZ(new(compensation.X + data.AxisX, compensation.Y + data.AxisY, startPosZ));
                            }
                            else
                            {
                                this.MoveXY(compensation.X + data.AxisX, compensation.Y + data.AxisY);
                            }
                          

                            await Task.Delay(this.GlobalCalibrationDomain.Delay);

                            // 执行定位
                            ExcuteResult p1Result = pREntity.DoWork(this.bondModule.GetHardware(),false);

                            // 定位失败，直接返回
                            if (p1Result == ExcuteResult.Success)
                            {
                                // 定位结果
                                MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                                // 计算Mark在轴坐标系中的位置
                                AKRSPoint2D markPosInAxis = this.GetAxesPosOfMark(matchResult);
                                data.ScanAxisX = markPosInAxis.X;
                                data.ScanAxisY = markPosInAxis.Y;

                                AKRSPoint2D encoder = this.bondModule.Get2DRealPosition();
                                dataLog.AddData("轴指令位置", new { X = data.AxisX, Y = data.AxisY });
                                dataLog.AddData("补偿值", compensation.Export());
                                dataLog.AddData("轴编码器位置", encoder.Export());
                                dataLog.AddData("Mark像素坐标", matchResult.Export());
                                dataLog.AddData("Mark轴坐标", markPosInAxis.Export());
                                dataLog.AddData("OffsetWithCompensation", (markPosInAxis - encoder).Export());
                                if (this.ChkCaptureTd.Checked)
                                {
                                    dataLog.AddData("温漂Mark拍摄结果", tdMarkResult.matchResult.Export());
                                    dataLog.AddData("温漂Mark轴坐标", tdMarkResult.axisPos.Export());
                                    dataLog.AddData("温漂Mark拍摄坐标",
                                        BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1.Export());
                                }

                                dataLog.CommitData();
                                count++;
                            }
                            else
                            {
                                throw new("定位失败，请检查标定板是否有问题");
                            }

                            if (count % 10 == 0)
                            {
                                await dataLog.FlushAsync();
                            }
                        }

                    }, this.cts.Token).ConfigureAwait(false);
                }
                finally
                {
                    await dataLog.FlushAsync();
                }
            }, ct);
        }
    }

    /// <summary>
    /// 暂停/继续扫描
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnPauseScan_Click(object sender, EventArgs e)
    {
        bool currentStatus = this.pauseSignal.WaitOne(1);
        if (currentStatus)
        {
            this.pauseSignal.Reset();
            this.BtnPauseScan.Text = "继续";
        }
        else
        {
            this.pauseSignal.Set();
            this.BtnPauseScan.Text = "暂停";
        }
    }

    /// <summary>
    /// 生成补偿区域
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnGenerateRegion_Click(object sender, EventArgs e)
    {
        if (!this.GlobalCalibrationDomain.ScanCompletedFlag)
        {
            AKRSXtraMessageBox.Show($"请先完成当前补偿列表扫描动作！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using FrmNewCompensationRegion frm = new(this.GlobalCalibrationDomain.Compensator);
        DialogResult dialogResult = frm.ShowDialog();
        if (dialogResult != DialogResult.OK)
        {
            return;
        }

        string regionName = frm.RegionName;
        int priority = frm.Priority;

        BilinearCompensationRegion newRegion = new(
            this.GlobalCalibrationDomain.ScanData);
        this.GlobalCalibrationDomain.Compensator.AddRegion(regionName, newRegion, priority);

        this.GcCompersatorRegions.DataSource = null;
        this.GcCompersatorRegions.DataSource = this.GlobalCalibrationDomain.Compensator.Regions;
        this.GcCompersatorRegions.RefreshDataSource();
    }

    /// <summary>
    /// 删除选中的补偿区域
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnRemoveSelectedRegion_Click(object sender, EventArgs e)
    {
        if (this.GlobalCalibrationDomain.Compensator.Regions.Count == 0)
        {
            AKRSXtraMessageBox.Show($"当前没有补偿区域");
            return;
        }

        if (this.GvCompensatorRegions.GetFocusedRow() is not RegionCompensator region)
        {
            return;
        }

        DialogResult dialogResult = AKRSXtraMessageBox.Show($"确认删除补偿区域[{region.RegionName}]?", "提示",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (dialogResult != DialogResult.Yes)
        {
            return;
        }

        this.GlobalCalibrationDomain.Compensator.RemoveRegion(region);

        this.GcCompersatorRegions.DataSource = null;
        this.GcCompersatorRegions.DataSource = this.GlobalCalibrationDomain.Compensator.Regions;
        this.GcCompersatorRegions.RefreshDataSource();
    }

    /// <summary>
    /// 排序补偿区域
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSortRegions_Click(object sender, EventArgs e)
    {
        this.GlobalCalibrationDomain.Compensator.Sort();

        this.GcCompersatorRegions.DataSource = null;
        this.GcCompersatorRegions.DataSource = this.GlobalCalibrationDomain.Compensator.Regions;
        this.GcCompersatorRegions.RefreshDataSource();
    }

    /// <summary>
    /// 模拟生成扫描数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnSimGenerateScanDatas_Click(object sender, EventArgs e)
    {
        DialogResult dialogResult = AKRSXtraMessageBox.Show($"根据当前配置信息生成新的模拟扫描点位会覆盖当前所有扫描点，是否继续?", "提示",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (dialogResult != DialogResult.Yes)
        {
            return;
        }

        List<GlobalCalibrationData> data = this.GlobalCalibrationDomain.ScanData;
        data.Clear();
        this.GcCompersatorRegions.RefreshDataSource();
        double colSpace = this.GlobalCalibrationDomain.ColumnSpacing;
        double rowSpace = this.GlobalCalibrationDomain.RowSpacing;

        this.GlobalCalibrationDomain.StartPoint3D = new(
            (double)this.SpStartX.Value,
            (double)this.SpStartY.Value,
            (double)this.SpStartZ.Value);

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                data.Add(new()
                {
                    WorldX = j * colSpace,
                    WorldY = i * rowSpace,
                    AxisX = this.GlobalCalibrationDomain.StartPoint3D.X + j * colSpace,
                    AxisY = this.GlobalCalibrationDomain.StartPoint3D.Y + i * rowSpace
                });
            }
        }

        this.BtnGeneratePoints_Click(null, null);
    }

    /// <summary>
    /// 点击移动到预定义点位按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BarBtnMoveTo_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (this.GvPreDefinedPoints.GetFocusedRow() is not GlobalCalibrationData data)
        {
            return;
        }

        ControlService.UIActionAsync(this.BarBtnMoveTo, () =>
        {
            if (this.ChkSimRun.Checked)
            {
                Thread.Sleep(1000);
                return;
            }

            System2Domain.GetInstance().BondModuleController.MoveSafeBondXYZ(new(data.AxisX, data.AxisY,
                this.GlobalCalibrationDomain.StartPoint3D.Z));
        });
    }

    /// <summary>
    /// 点击移动到相机中心按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnMoveToCamCenter_Click(object sender, EventArgs e)
    {
        sender.UIActionAsync(() =>
        {
            if (this.ChkSimRun.Checked)
            {
                Thread.Sleep(1000);
                return;
            }

            this.MoveToCameraCenter();
        });
    }

    /// <summary>
    /// 右键点击预定义点位表格
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GvPreDefinedPoints_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right)
        {
            return;
        }

        GridView view = sender as GridView;
        GridHitInfo hitInfo = view.CalcHitInfo(e.Location);
        if (!hitInfo.InRow)
        {
            return;
        }

        view.FocusedRowHandle = hitInfo.RowHandle;
        this.PopMenuGcPreDefinedPoints.ShowPopup(MousePosition);
    }

    /// <summary>
    /// 添加到位置配置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnAddToPosConfig_Click(object sender, EventArgs e)
    {
        double worldPosX = this.SpCalibMarkPosX.TryGetDouble();
        double worldPosY = this.SpCalibMarkPosY.TryGetDouble();

        double colSpace = this.SpColumnSpacing.TryGetDouble();
        double rowSpace = this.SpRowSpacing.TryGetDouble();

        if (worldPosX % colSpace != 0 || worldPosY % rowSpace != 0)
        {
            AKRSXtraMessageBox.Show(
                $"当前配置的点位({worldPosX},{worldPosY})不符合标定板Mark间距配置{Environment.NewLine}当前列间距{colSpace},标定片Mark的X坐标应为{colSpace}的整数倍；Y坐标同理",
                "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            return;
        }

        AKRSPoint2D axisPos = this.ChkSimRun.Checked
            ? new(this.SpStartX.TryGetDouble(), this.SpStartY.TryGetDouble())
            : this.bondModule.Get2DRealPosition();
        DialogResult dialogResult = AKRSXtraMessageBox.Show(
            $"确认添加标定片坐标系点位({worldPosX},{worldPosY})以及当前轴位置({axisPos})为一组对应点?", "提示",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (dialogResult != DialogResult.Yes)
        {
            return;
        }

        List<GlobalCalibrationData> sourceList = this.GlobalCalibrationDomain.ScanData;
        if (sourceList.Exists(d => MathService.DoubleEqual(d.WorldX, worldPosX) && MathService.DoubleEqual(d.WorldY, worldPosY)))
        {
            dialogResult = AKRSXtraMessageBox.Show($"已存在标定片坐标系点位为({worldPosX},{worldPosY})的点，是否重新写入?", "提示",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            sourceList.RemoveAll(d => MathService.DoubleEqual(d.WorldX, worldPosX) && MathService.DoubleEqual(d.WorldY, worldPosY));
        }

        GlobalCalibrationData data = new()
        {
            WorldX = worldPosX,
            WorldY = worldPosY,
            AxisX = axisPos.X,
            AxisY = axisPos.Y
        };

        sourceList.Add(data);

        this.GcPreDefinedPoints.RefreshDataSource();
    }

    /// <summary>
    /// 删除选中的预定义点位
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BarBtnDeleteSelectedPoint_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (this.GvPreDefinedPoints.GetFocusedRow() is not GlobalCalibrationData data)
        {
            return;
        }

        DialogResult dialogResult = AKRSXtraMessageBox.Show($"确定删除标定片坐标点({data.WorldX},{data.WorldY})?", "提示",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (dialogResult != DialogResult.Yes)
        {
            return;
        }

        this.GlobalCalibrationDomain.ScanData.Remove(data);
        this.GcPreDefinedPoints.RefreshDataSource();
    }

    /// <summary>
    /// 清空预定义点位
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnClearPoints_Click(object sender, EventArgs e)
    {
        DialogResult dialogResult =
            AKRSXtraMessageBox.Show($"确认清空当前配置的所有点位?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (dialogResult != DialogResult.Yes)
        {
            return;
        }

        this.GlobalCalibrationDomain.ScanData.Clear();
        this.GcPreDefinedPoints.RefreshDataSource();
    }

    /// <summary>
    /// 停止
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnStopScan_Click(object sender, EventArgs e)
    {
        this.pauseSignal.Set();
        this.cts?.Cancel();
    }

    /// <summary>
    /// 导入扫描列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnImportScanList_Click(object sender, EventArgs e)
    {
        XtraOpenFileDialog ofd = new XtraOpenFileDialog();
        ofd.Title = "导入扫描列表文件";
        ofd.Filter = "Json文件|*.json";
        ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        ofd.RestoreDirectory = true;
        ofd.CheckFileExists = true;
        ofd.CheckPathExists = true;
        DialogResult dialogResult = ofd.ShowDialog();
        if (dialogResult != DialogResult.OK)
        {
            return;
        }

        try
        {
            string fileName = ofd.FileName;
            List<GlobalCalibrationData> scanList = JsonFormatHelper<GlobalCalibrationData>.ReadGenericList(fileName);
            if (scanList == null)
            {
                AKRSXtraMessageBox.Show($"导入文件格式不正确，请检查文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 先保存原有配置后再加载
            this.GlobalCalibrationDomain.Save();
            this.GlobalCalibrationDomain.ScanData.Clear();
            this.GlobalCalibrationDomain.ScanData.AddRange(scanList);

            this.GcPreDefinedPoints.RefreshDataSource();
        }
        catch (Exception exception)
        {
            AKRSXtraMessageBox.Show($"加载扫描列表时出错：{exception.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// 导出扫描列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnExportScanList_Click(object sender, EventArgs e)
    {
        XtraSaveFileDialog sfd = new XtraSaveFileDialog();
        sfd.Title = "导出扫描列表到文件";
        sfd.Filter = "Json文件|*.json";
        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        sfd.FileName = $"ScanList_{DateTime.Now:yyyyMMdd_HHmmss}.json";
        sfd.RestoreDirectory = true;
        sfd.OverwritePrompt = true;
        sfd.CreatePrompt = true;
        DialogResult dialogResult = sfd.ShowDialog();
        if (dialogResult != DialogResult.OK)
        {
            return;
        }

        try
        {
            // 直接保存原始配置
            string fileName = sfd.FileName;
            JsonFormatHelper<GlobalCalibrationData>.SaveGenericList(this.GlobalCalibrationDomain.ScanData, fileName);

            // 保存可转excel的文件
            string formattedFileName = $"Formatted_{fileName}";
            IDataLog dataLog = DataLogManager.Instance.CreateDataLog("ScanList").ConfigureFileOutput(formattedFileName);
            this.GlobalCalibrationDomain.ScanData.Iter(d => { dataLog.AddData("扫描列表", d.Export()); });
        }
        catch (Exception exception)
        {
            AKRSXtraMessageBox.Show($"导出扫描点位列表出错：{exception.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    /// <summary>
    /// 采集温漂Mark配置项
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CmbTDCaptureConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.TsTDCapture.Enabled = this.CmbTDCaptureConfig.SelectedIndex == 1;
        this.SpTdCapCycleInterval.Enabled = this.CmbTDCaptureConfig.SelectedIndex == 0;
    }

    /// <summary>
    /// 是否采集温漂Mark
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChkCaptureTd_CheckedChanged(object sender, EventArgs e)
    {
        this.CmbTDCaptureConfig.Enabled = this.ChkCaptureTd.Checked;
        if (this.ChkCaptureTd.Checked)
        {
            return;
        }

        this.TsTDCapture.Enabled = false;
        this.SpTdCapCycleInterval.Enabled = false;
    }

    /// <summary>
    /// 基于扫描结果重新映射点位
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BtnGeneratePointsByScanPoints_Click(object sender, EventArgs e)
    {
        if (this.GlobalCalibrationDomain.ScanData.Count < 3)
        {
            AKRSXtraMessageBox.Show($"生成前需确保预定义点位数量至少为3个", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!this.GlobalCalibrationDomain.ScanCompletedFlag)
        {
            AKRSXtraMessageBox.Show($"当前点位列表未扫描完成", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 计算轴坐标到世界坐标的转换关系
        List<AKRSPoint2D> sourcePoints = new();
        List<AKRSPoint2D> targetPoints = new();
        foreach (GlobalCalibrationData data in this.GlobalCalibrationDomain.ScanData)
        {
            sourcePoints.Add(new(data.WorldX, data.WorldY));
            targetPoints.Add(new(data.ScanAxisX, data.ScanAxisY));
        }

        double[,] fromWorldToAxis = MathService.FitTransMatrix(sourcePoints, targetPoints);

        using FileStream fs = new FileStream($"D:\\2025二维标定数据采集\\标定片坐标系到轴坐标系传递矩阵.txt", mode: FileMode.OpenOrCreate);
        using StreamWriter sw = new StreamWriter(fs);
        int rows = fromWorldToAxis.GetLength(0);
        int cols = fromWorldToAxis.GetLength(1);

        sw.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}通过扫描点位构建:");
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                sw.Write(fromWorldToAxis[i, j]);
                sw.Write("\t");
            }

            sw.Write(Environment.NewLine);
        }

        int leftCols = Convert.ToInt32(this.SpLeftCols.EditValue);
        int rightCols = Convert.ToInt32(this.SpRightCols.EditValue);
        int upRows = Convert.ToInt32(this.SpUpperRows.EditValue);
        int downRows = Convert.ToInt32(this.SpLowerRows.EditValue);

        double rowSpace = Convert.ToDouble(this.SpRowSpacing.EditValue);
        double colSpace = Convert.ToDouble(this.SpRowSpacing.EditValue);

        // 计算轴坐标系下的步长
        // AKRSPoint2D calibBoardStep = new AKRSPoint2D(this.GlobalCalibrationDomain.ColumnSpacing, this.GlobalCalibrationDomain.RowSpacing);
        // var axisStep = MathService.TransformPoint(calibBoardStep, fromWorldToAxis);
        // this.GlobalCalibrationDomain.XStep = axisStep.X;
        // this.GlobalCalibrationDomain.YStep = axisStep.Y;

        List<GlobalCalibrationData> dataToFill = new();
        dataToFill.AddRange(this.GlobalCalibrationDomain.ScanData);

        // 第一象限
        List<GlobalCalibrationData> firstQuadrant = this.CalcAndFillQuadrant(dataToFill,
            upRows, rightCols, rowSpace, colSpace);
        dataToFill.AddRange(firstQuadrant);

        // 第二象限
        List<GlobalCalibrationData> secondQuadrant = this.CalcAndFillQuadrant(dataToFill,
            upRows, -leftCols, rowSpace, colSpace);
        dataToFill.AddRange(secondQuadrant);

        // 第三象限
        List<GlobalCalibrationData> thirdQuadrant = this.CalcAndFillQuadrant(dataToFill,
            -downRows, -leftCols, rowSpace, colSpace);
        dataToFill.AddRange(thirdQuadrant);

        // 第四象限
        List<GlobalCalibrationData> fourthQuadrant = this.CalcAndFillQuadrant(dataToFill,
            -downRows, rightCols, rowSpace, colSpace);
        dataToFill.AddRange(fourthQuadrant);

        dataToFill.Iter(d =>
        {
            AKRSPoint2D axisPoint = MathService.TransformPoint(new(d.WorldX, d.WorldY), fromWorldToAxis);
            d.AxisX = axisPoint.X;
            d.AxisY = axisPoint.Y;
        });

        this.GlobalCalibrationDomain.ScanData.Clear();
        this.GlobalCalibrationDomain.ScanData.AddRange(dataToFill);
        this.GlobalCalibrationDomain.Save();
        this.GcPreDefinedPoints.RefreshDataSource();
    }

    /// <summary>
    /// 循环运行
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChkCycleRun_CheckedChanged(object sender, EventArgs e)
    {
        if (this.ChkCycleRun.Checked)
        {
            this.RunToCompleteEvent += FrmGlobalCalibrationEx_RunToCompleteEvent;
        }
        else
        {
            this.RunToCompleteEvent -= FrmGlobalCalibrationEx_RunToCompleteEvent;
        }
    }

    /// <summary>
    /// 循环运行完成事件
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void FrmGlobalCalibrationEx_RunToCompleteEvent(object arg1, EventArgs arg2)
    {
        Task.Run(async () => 
        {
            await Task.Delay(500).ConfigureAwait(false);

            this.Invoke(
                (MethodInvoker)(() =>
                {
                    this.BtnStartBoot_Click(arg1, arg2);
                }));
        });
    }

    private CompensationPredictor predictor;

    /// <summary>
    /// 是否启用补偿预测
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void ChkUsingPredictor_CheckedChanged(object sender, EventArgs e)
    {
        if (this.ChkUsingPredictor.Checked)
        {
            try
            {
                //await this.ShowOverlayAsync("正在加载模型,请稍等...", () =>
                //{
                //    string modelPath = "compensation_model.onnx";
                //    string scalerXPath = "scaler_x.json";
                //    string scalerYPath = "scaler_y.json";

                  
                //    this.predictor = new CompensationPredictor(modelPath, scalerXPath, scalerYPath);
                //}).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show($"模型加载异常：{exception.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        else
        {
            this.predictor.Dispose();
            this.predictor = null;
        }
    }

    /// <summary>
    /// 预测
    /// </summary>
    /// <param name="tdMarkShotPos">温漂Mark拍摄位置</param>
    /// <param name="tdMarkAxisPos">温漂Mark当前位置</param>
    /// <param name="axisPos">目标轴位置</param>
    /// <returns>补偿值</returns>
    private AKRSPoint2D Predict(AKRSPoint2D tdMarkShotPos, AKRSPoint2D tdMarkAxisPos, AKRSPoint2D axisPos)
    {
        if (this.predictor == null)
        {
            return new(0, 0);
        }

        PointF tdOffset = ((tdMarkAxisPos - tdMarkShotPos) * 1000).ToPointF();
        PointF inputAxisPos = (axisPos * 1000).ToPointF();
        float[] input = new float[] { tdOffset.X, tdOffset.Y, inputAxisPos.X, inputAxisPos.Y };

        float[] output = this.predictor.Predict(input);
        return new(output[0], output[1]);
    }
}