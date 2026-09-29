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

namespace AKRS.ZX2200.Experiment.DispenseMeasureExperiment
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models;

    using System.IO;
    using System.Threading;

    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using LanguageExt.TypeClasses;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.Galaxy2.MachineSupport.Config;
    using OfficeOpenXml;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 测高针稳定性实验
    /// </summary>
    public partial class FrmDispenseMeasureHeightTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 点胶测高
        /// </summary>
        private readonly DispenseMeasureHeightController controller = new DispenseMeasureHeightController();

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmDispenseMeasureHeightTest()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 全局标定线程
        /// </summary>
        private Task measureHeightTestTask;

        /// <summary>
        /// 是否停止
        /// </summary>
        private bool isStop = false;

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStart_Click(object sender, EventArgs e)
        {
            if (this.measureHeightTestTask == null || this.measureHeightTestTask.IsCompleted)
            {
                this.isStop = false;
                this.measureHeightTestTask = new Task(this.MeasureHeightTest);
                this.measureHeightTestTask.Start();
            }
        }

        /// <summary>
        /// 测高
        /// </summary>
        public void MeasureHeightTest()
        {
            while(!isStop)
            {
                //System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(new AKRSPoint3D(65.7902, -257.4097, 87.1235));
                //this.MeasureHeightDispenser();

                System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(new AKRSPoint3D(84, -212, 101));
                //this.MeasureHeight(10);
                //this.MeasureHeight(30);
                //this.MeasureHeight(50);
                //this.MeasureHeight(70);
                this.MeasureHeight(90);
                this.MeasureHeight(120);
                this.MeasureHeight(150);
                //this.MeasureHeight(180);
                //this.MeasureHeight(200);
            }
        }

        public void DispenseMeasureHeight()
        {
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;
            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            foreach (Substrate substrate in transportUnit.Substrates)
            {
                foreach (Module module in substrate.Modules)
                {
                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        System1Domain.GetInstance().MatterHeightMeasurePoints(bondPosition);
                    }
                }
            }

            List<List<string>> resultList = new List<List<string>>();

            foreach (Substrate substrate in transportUnit.Substrates)
            {
                foreach (Module module in substrate.Modules)
                {
                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        resultList.Add(
                            new List<string>()
                                {
                                    bondPosition.Name, bondPosition.BondPositionInfo.MeasureHeightResult.ToString("f4")
                                });
                    }
                }
            }

            // 数据处理
            string path = FileHelper.CreateFileByDate("点胶测高");

            //FileHelper.SaveDoubleExcel(resultList, Path.Combine(path, $"点胶测高" + "时间" + DateTime.Now.ToFileTime().ToString() + ".xlsx"));
        }

        /// <summary>
        /// 测高
        /// </summary>
        /// <param name="speed">测高速度</param>
        private void MeasureHeight(double speed)
        {
            List<List<double>> resultList = new List<List<double>>();
            for (int i = 0; i < 500; i++)
            {
                if (this.isStop)
                {
                    return;
                }

                Thread.Sleep(1000);
                (ExcuteResult result, double height1) = this.controller.DispenserHeightMeasurementG0(
                    System1MeasHeightToolEnum.HeightSensor,
                    null,
                    double.NaN,
                    double.NaN,
                    true);

                if (result == ExcuteResult.Success)
                {
                    resultList.Add(new List<double>() { 1, height1 });
                }
                else
                {
                    resultList.Add(new List<double>() { -1, height1 });
                }
            }

            // 数据处理
            string path = FileHelper.CreateFileByDate("点胶测高");

            FileHelper.SaveDoubleExcel(resultList, Path.Combine(path, $"点胶测高速度{speed}"+"时间" +DateTime.Now.ToFileTime().ToString() + ".xlsx"));

            resultList.Clear();
        }

        /// <summary>
        /// 测高
        /// </summary>
        private void MeasureHeightDispenser()
        {
            List<List<double>> resultList = new List<List<double>>();
            for (int i = 0; i < 500; i++)
            {
                if (this.isStop)
                {
                    return;
                }

                Thread.Sleep(1000);
                (ExcuteResult result, double height1) = this.controller.DispenserHeightMeasurementG0(
                    System1MeasHeightToolEnum.Dispenser,
                    null,
                    double.NaN,
                    double.NaN,
                    false);

                if (result == ExcuteResult.Success)
                {
                    resultList.Add(new List<double>() { 1, height1 });
                }
                else
                {
                    resultList.Add(new List<double>() { -1, height1 });
                }
            }

            // 数据处理
            string path = FileHelper.CreateFileByDate("点胶测高");

            FileHelper.SaveDoubleExcel(resultList, Path.Combine(path, $"预点胶板测高" + "时间" + DateTime.Now.ToFileTime().ToString() + ".xlsx"));

            resultList.Clear();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            this.isStop = true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Task.Run(() => 
            {
            
            AKRSPoint3D visionPos = System2Domain.GetInstance().BondModuleController.ConvertMachineToG0Pos(BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);

            AKRSPoint3D visionPos2 = new AKRSPoint3D(-118, -238.676025, 106.506);

            AKRSPoint3D visionPos3 = new AKRSPoint3D(-76.237275, -185.247725, 106.506);

            AKRSPoint3D visionPos4 = new AKRSPoint3D(-57, -238.676025, 106.506);

            AKRSPoint3D visionPos5 = new AKRSPoint3D(-14.547975, -185.247725, 106.506);

            AKRSPoint3D visionPos6 = new AKRSPoint3D(4.693225, -238.676025, 106.506);

            AKRSPoint3D visionPos7 = new AKRSPoint3D(46.693225, -185.247725, 106.506);

            AKRSPoint3D visionPos8 = new AKRSPoint3D(66.628525, -238.676025, 106.506);

            AKRSPoint3D visionPos9 = new AKRSPoint3D(108.340825, -185.247725, 106.506);

            List<MatchResult> resultList = new List<MatchResult>();
            List<MatchResult> resultList2 = new List<MatchResult>();
            List<MatchResult> resultList3 = new List<MatchResult>();
            List<MatchResult> resultList4 = new List<MatchResult>();
            List<MatchResult> resultList5 = new List<MatchResult>();
            List<MatchResult> resultList6 = new List<MatchResult>();
            List<MatchResult> resultList7 = new List<MatchResult>();
            List<MatchResult> resultList8 = new List<MatchResult>();
            List<MatchResult> resultList9 = new List<MatchResult>();
            while (true)
            {
                for (int i = 0; i < 500; i++)
                {
                    if (this.isStop)
                    {
                        return;
                    }
                    
                    // 温漂定位
                    MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos,
                        "温漂定位",
                        "上视温漂测试Mark");
                   
                    // 温漂定位
                    MatchResult driftMatchResult2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos2,
                        "温漂定位",
                        "xu123ModuleMark1");

                    // 温漂定位
                    MatchResult driftMatchResult3 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos3,
                        "温漂定位",
                        "xu123ModuleMark1");


                    // 温漂定位
                    MatchResult 
                        driftMatchResult4 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos4,
                        "温漂定位",
                        "xu123ModuleMark1");

                    // 温漂定位
                    MatchResult driftMatchResult5 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos5,
                        "温漂定位",
                        "xu123ModuleMark1");


                    // 温漂定位
                    MatchResult driftMatchResult6 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos6,
                        "温漂定位",
                        "xu123ModuleMark1");


                    // 温漂定位
                    MatchResult driftMatchResult7 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos7,
                        "温漂定位",
                        "xu123ModuleMark1");

                    // 温漂定位
                    MatchResult driftMatchResult8 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos8,
                        "温漂定位",
                        "xu123ModuleMark1");

                    // 温漂定位
                    MatchResult driftMatchResult9 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos9,
                        "温漂定位",
                        "xu123ModuleMark1");

                    if (driftMatchResult != null && driftMatchResult2 != null && driftMatchResult3 != null && driftMatchResult4 != null && driftMatchResult5 != null && driftMatchResult6 != null && driftMatchResult7 != null && driftMatchResult8 != null && driftMatchResult9 != null)
                    {
                        resultList.Add(driftMatchResult);
                        resultList2.Add(driftMatchResult2);
                        resultList3.Add(driftMatchResult3);
                        resultList4.Add(driftMatchResult4);
                        resultList5.Add(driftMatchResult5);
                        resultList6.Add(driftMatchResult6);
                        resultList7.Add(driftMatchResult7);
                        resultList8.Add(driftMatchResult8);
                        resultList9.Add(driftMatchResult9);
                    }
                }

                this.SaveCorrectionData(resultList, resultList2,resultList3, resultList4, resultList5, resultList6, resultList7, resultList8, resultList9);

                resultList.Clear();
            }
            });
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="match">结果</param>
        private void SaveCorrectionData(List<MatchResult> match , List<MatchResult> match2, List<MatchResult> match3,List<MatchResult> match4, List<MatchResult> match5, List<MatchResult> match6, List<MatchResult> match7, List<MatchResult> match8, List<MatchResult> match9)
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(
                new FileInfo(
                    @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + DateTime.Now.ToFileTime()
                    + "--位置变化结果.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 7; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "温漂MarkX";

                worksheet.Cells[1, 2].Value = "温漂MarkY";

                worksheet.Cells[1, 3].Value = "BMCMarkX";

                worksheet.Cells[1, 4].Value = "BMCMarkY";

                worksheet.Cells[1, 5].Value = "time";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            for (int i = 0; i < match.Count; i++)
            {
                worksheet.Cells[lastUsedRow + 1 + i, 1].Value = match[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 2].Value = match[i]?.CenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 3].Value = match2[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 4].Value = match2[i]?.CenterY;
                worksheet.Cells[lastUsedRow + 1 + i, 5].Value = match3[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 6].Value = match3[i]?.CenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 7].Value = match4[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 8].Value = match4[i]?.CenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 9].Value = match5[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 10].Value = match5[i]?.CenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 11].Value = match6[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 12].Value = match6[i]?.CenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 13].Value = match7[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 14].Value = match7[i]?.CenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 15].Value = match8[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 16].Value = match8[i]?.CenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 17].Value = match9[i]?.CenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 18].Value = match9[i]?.CenterY;
            }

            excelPackage.Save();
        }
    }
}