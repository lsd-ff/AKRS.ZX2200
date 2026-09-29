using System;
using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Models;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection
{
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using DevExpress.PivotGrid.OLAP.AdoWrappers;
    using log4net.Core;
    using MathNet.Numerics;
    using OfficeOpenXml;

    /// <summary>
    /// 焊后检测
    /// </summary>
    [Serializable]
    public class PostBondInspection : BaseDsSetting
    {
        /// <summary>
        /// 视觉检测配置
        /// </summary>
        public AdjustConfig VisionConfig { get; set; } = new AdjustConfig();

        /// <summary>
        /// 当前检测的数量
        /// </summary>
        [JsonIgnore]
        public int CurrentDefectNumber { get; set; } = 0;

        #region 类型枚举

        /// <summary>
        /// 视觉搜索点数量枚举
        /// </summary>
        [TreeProgramListArgs("芯片搜索点数量")]
        public MeasurePointNumberEnum MeasurePointNumber { get; set; }

        /// <summary>
        /// 视觉搜索点数量枚举
        /// </summary>
        [TreeProgramListArgs("参考点数量")]
        public MeasurePointNumberEnum ReferencePointNumber { get; set; }

        /// <summary>
        /// 焊后检测模式枚举
        /// </summary>
        [TreeProgramListArgs("焊后检测模式")]
        public PostBondInspectionModeEnum PostBondInspectionMode { get; set; }

        /// <summary>
        /// 焊后应用系统枚举（胶检或者贴片）
        /// </summary>
        public PostBondApplicationSystemEnum ApplicationSystem { get; set; }

        #endregion

        #region 标准值

        /// <summary>
        /// 焊后标准距离偏差
        /// </summary>
        [TreeProgramListArgs("标准距离X", "Mark到贴片位的距离", "mm")]
        public double PostBondDistanceX { get; set; }

        /// <summary>
        /// 焊后标准距离偏差
        /// </summary>
        [TreeProgramListArgs("标准距离Y", "Mark到贴片位的距离", "mm")]
        public double PostBondDistanceY { get; set; }

        /// <summary>
        /// 焊后标准距离偏差
        /// </summary>
        [TreeProgramListArgs("标准角度", "Mark到贴片位的距离", "度")]
        public double PostBondDistanceAngle { get; set; }

        /// <summary>
        /// 焊后检测点胶示教数量
        /// </summary>
        public double PostBondEpoxyNum { get; set; }

        /// <summary>
        /// 焊后检测点胶示教胶量
        /// </summary>
        public List<double> PostBondEpoxyArea { get; set; }

        /// <summary>
        /// 焊后检测点胶示教胶量
        /// </summary>
        public List<double> PostBondEpoxyCenterX { get; set; }

        /// <summary>
        /// 焊后检测点胶示教胶量
        /// </summary>
        public List<double> PostBondEpoxyCenterY { get; set; }

        #endregion

        #region 阈值参数

        /// <summary>
        /// 允许的焊后胶量最小值
        /// </summary>
        [TreeProgramListArgs("允许的焊后胶量最小值", "阈值", "百分比")]
        public double TolerantEpoxyMin { get; set; } = 0.8;

        /// <summary>
        /// 允许的焊后胶量最大值
        /// </summary>
        [TreeProgramListArgs("允许的焊后胶量最大值", "阈值", "百分比")]
        public double TolerantEpoxyMax { get; set; } = 1.2;

        /// <summary>
        /// X方向的阈值
        /// </summary>
        [TreeProgramListArgs("X方向的阈值", "阈值", "mm")]
        public double LimitX { get; set; } = 0.1;

        /// <summary>
        /// Y方向的阈值
        /// </summary>
        [TreeProgramListArgs("Y方向的阈值", "阈值", "mm")]
        public double LimitY { get; set; } = 0.1;

        /// <summary>
        /// 角度阈值
        /// </summary>
        [TreeProgramListArgs("角度阈值", "阈值", "度")]
        public double LimitAngle { get; set; } = 0.5;

        #endregion

        #region BLT

        /// <summary>
        /// 芯片周围测高点位(相对于焊点)
        /// </summary>
        public List<AKRSPoint3D> ComponentAroundMeasureHeightPosList { get; set; } = new List<AKRSPoint3D>();

        /// <summary>
        /// 芯片表面测高点位(相对于焊点)
        /// </summary>
        public List<AKRSPoint3D> ComponentSurfaceMeasureHeightPosList { get; set; } = new List<AKRSPoint3D>();

        /// <summary>
        /// 标准厚度
        /// </summary>
        [TreeProgramListArgs("标准厚度", "BLT检测", "mm")]
        public double BLTStandard { get; set; }

        /// <summary>
        /// 最大BLT偏移量
        /// </summary>
        [TreeProgramListArgs("最大BLT偏移量", "BLT检测", "mm")]
        public double MaxBLTOffset { get; set; } = 0.1;

        /// <summary>
        /// 芯片表面最大坡度
        /// </summary>
        [TreeProgramListArgs("芯片表面最大坡度", "BLT检测", "mm")]
        public double MaxSurfaceSlope { get; set; } = 0.1;

        /// <summary>
        /// 焊点回看时的灯光
        /// </summary>
        public Dictionary<string, int> BLTVisionLightValue { get; set; } = new Dictionary<string, int>();

        /// <summary>
        /// 相机曝光
        /// </summary>
        [TreeProgramListArgs("相机曝光", "BLT检测")]
        public double BLTVisionExposure { get; set; } = 500;

        /// <summary>
        /// 相机增益
        /// </summary>
        [TreeProgramListArgs("相机增益", "BLT检测")]
        public double BLTVisionGain { get; set; } = 1;

        /// <summary>
        /// 相机gamma
        /// </summary>
        [TreeProgramListArgs("相机伽马", "BLT检测")]
        public double BLTVisionGamma { get; set; } = 40;

        #endregion

        /// <summary>
        /// 补胶的名称
        /// </summary>
        public string ReDispenseName { get; set; } = "Null";

        /// <summary>
        /// 焊后补偿
        /// </summary>
        [TreeProgramListArgs("开启焊后补偿", "焊后补偿", true)]
        public bool IsBondPostCompensation { get; set; } = false;

        /// <summary>
        /// 焊后补偿数量
        /// </summary>
        [TreeProgramListArgs("焊后补偿参考次数", "焊后补偿", true)]
        public double BondPostCompensationNumber { get; set; } = 10;

        /// <summary>
        /// 补偿百分比
        /// </summary>
        [TreeProgramListArgs("焊后补偿百分比", "焊后补偿", true)]
        public int BondPostCompensationRation { get; set; } = 50;

        /// <summary>
        /// 检测频率
        /// </summary>
        [TreeProgramListArgs("检测频率", "焊后补偿", true)]
        public int DefectFrequency { get; set; } = 10;

        /// <summary>
        /// 检测频率
        /// </summary>
        [TreeProgramListArgs("抽检是否开启", "焊后补偿", true)]
        public bool IsDefectFrequencyOpen { get; set; } = false;

        /// <summary>
        /// 是否为预点胶版检测
        /// </summary>
        public bool IsPreDispenseCheck { get; set; } = false;

        /// <summary>
        /// 是否为圆形
        /// </summary>
        public bool IsCircle { get; set; } = false;

        /// <summary>
        /// 是否为背崩检测
        /// </summary>
        public bool IsDetectBacksideCrack { get; set; } = false;
        
        /// <summary>
        /// 焊后检测
        /// </summary>
        /// <param name="name">名称</param>
        public PostBondInspection(string name)
        {
            this.ResetAssistantStates();
            this.Name = name;
            this.VisionConfig.P1PRName = this.Name + "1";
            this.VisionConfig.P2PRName = this.Name + "2";
            this.VisionConfig.P1ReferName = this.Name + "Refer" + "1";
            this.VisionConfig.P2ReferName = this.Name + "Refer" + "2";
        }

        /// <summary>
        /// 焊后检测
        /// </summary>
        public PostBondInspection()
        {
            this.ResetAssistantStates();
        }

        /// <summary>
        /// PostBond
        /// </summary>
        public AssistantState PostBond { get; set; }

        /// <summary>
        /// 背崩检测
        /// </summary>
        public AssistantState BacksideCrackDetection { get; set; }

        /// <summary>
        /// PostBond是否示教完成
        /// </summary>
        [JsonIgnore]
        public bool IsAssistantSucceed => this.GetAssistantResult();

        /// <summary>
        /// PostBond是否示教完成
        /// </summary>
        /// <returns>return</returns>
        private bool GetAssistantResult()
        {
            List<AssistantState> list = this.GetAssistantStates();
            foreach (var item in list)
            {
                if (item == null)
                {
                    return false;
                }

                if (item.State == AssistantStateEnum.UnAble)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取PostBond所有示教的状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetAssistantStates()
        {
            List<AssistantState> list = new List<AssistantState>();
            list.Add(this.PostBond);
            list.Add(this.BacksideCrackDetection);

            // todo:
            //list.Add(this.BLTCheck);
            return list;
        }

        /// <summary>
        /// 初始化PostBond示教状态
        /// </summary>
        public void ResetAssistantStates()
        {
            this.PostBond = new AssistantState() { Name = "Post-bond", State = AssistantStateEnum.UnAble };
            this.BacksideCrackDetection = new AssistantState() { Name = "BacksideCrackDetection", State = AssistantStateEnum.ForBidden };

            // todo:
            //this.BLTCheck= new AssistantState() { Name = "BLTCheck", State = AssistantStateEnum.ForBidden };
        }


        /// <summary>
        /// 胶量检测结果处理
        /// </summary>
        /// <param name="baseAlgResults">定位结果</param>
        /// <returns>结果</returns>
        public ExcuteResult PostBondEpoxyCheck(List<BaseAlgResult> baseAlgResults,BondPosition bondPosition,PRHardware pRHardware)
        {
            List<double> avgX = new List<double>();
            List<double> avgY = new List<double>();
            List<double> avgArea = new List<double>();

            string alarmMessage = string.Empty;

            if (baseAlgResults == null)
            {
                alarmMessage = this.Name + "：胶量检测未找到识别点";
                return this.PostBondCheckHand(this.VisionConfig.P1PRName, alarmMessage, pRHardware);
            }
            else if (baseAlgResults.Count != (int)this.PostBondEpoxyNum)
            {
                alarmMessage = this.Name + "：胶量检测识别到的个数不对";
                return this.PostBondCheckHand(this.VisionConfig.P1PRName, alarmMessage, pRHardware);
            }
            else
            {
                for (int i = 0; i < baseAlgResults.Count; i++)
                {
                    BlobResult blobResult = (BlobResult)baseAlgResults[i];
                    MatchResult matchResult = new MatchResult(blobResult.CenterX, blobResult.CenterY, blobResult.Area);

                    // 定位结果和示教位置之间的差值
                    AKRSPoint3D pointResult = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem
                        .ForwardConvertCoordinate(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));

                    double offsetX = pointResult.X - this.PostBondEpoxyCenterX[i] - this.PostBondDistanceX;

                    double offsetY = pointResult.Y - this.PostBondEpoxyCenterY[i] - -this.PostBondDistanceY;

                    // 计算圆形面积
                    double cameraRatio = System2Domain.GetInstance().BondModuleController.GetCameraRatio();

                    double newArea = Math.Round(blobResult.Area * cameraRatio * cameraRatio, 4);
                    double oldArea = Math.Round(this.PostBondEpoxyArea[i] * cameraRatio * cameraRatio, 4);

                    double limitArea = 0;

                    if (!this.IsCircle)
                    {
                        // 面积判断
                        limitArea = Math.Round(newArea / oldArea, 2);
                        if (limitArea > this.TolerantEpoxyMax
                            || limitArea < this.TolerantEpoxyMin)
                        {
                            alarmMessage +=
                                $"胶量检测点 {i + 1} 的面积的检测最大比例为{this.TolerantEpoxyMax},"
                                + $"最小比例为{this.TolerantEpoxyMin},当前值为{limitArea}\r\n"
                                + $"当前面积为{newArea}mm²，标准面积为{oldArea}mm²";
                            break;
                        }
                    }
                    else
                    {
                        double newRadius = Math.Round(Math.Sqrt(newArea / Math.PI), 4);
                        double oldRadius = Math.Round(Math.Sqrt(oldArea / Math.PI), 4);

                        limitArea = Math.Round(newArea / oldArea, 2);

                        if (limitArea > this.TolerantEpoxyMax
                            || limitArea < this.TolerantEpoxyMin)
                        {
                            alarmMessage +=
                               $"胶量检测点 {i + 1} 半径的检测最大比例为{this.TolerantEpoxyMax},"
                               + $"最小比例为{this.TolerantEpoxyMin},当前值为{limitArea}\r\n"
                               + $"当前半径为{newRadius}mm²，标准半径为{oldRadius}mm²";
                            break;
                        }
                    }

                    avgX.Add(offsetX);
                    avgY.Add(offsetY);
                    avgArea.Add(limitArea);

                    // X方向判断
                    if (Math.Abs(offsetX) > this.LimitX)
                    {
                        alarmMessage =
                            $"胶量检测点 {i + 1} 的X方向的检测阈值为{this.LimitX}\r\n,当前值为{offsetX}\r\n";
                        break;
                    }

                    // Y方向判断
                    if (Math.Abs(offsetY) > this.LimitY)
                    {
                        alarmMessage +=
                            $"胶量检测点 {i + 1} 的Y方向的检测阈值为{this.LimitY}\r\n,当前值为{offsetY}\r\n";
                        break;
                    }
                }
            }

            if (alarmMessage != string.Empty)
            {
                return this.PostBondCheckHand(this.VisionConfig.P1PRName, alarmMessage, pRHardware);
            }

            if (System2Configuration.GetInstance().IsExportPostBondEpoxyCheckData)
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\设备功能测试\\{this.Name}-胶量检测数据.xlsx"));
                    ExcelWorksheet worksheet;
                    if (package.Workbook.Worksheets.Exists(a => a.Name == "sheet"))
                    {
                        worksheet = package.Workbook.Worksheets.ToList().Find(a => a.Name == "sheet");
                    }
                    else
                    {
                        worksheet = package.Workbook.Worksheets.Add("sheet");
                    }

                    worksheet.Cells[1, 1].Value = "时间";
                    worksheet.Cells[1, 2].Value = "名称";
                    worksheet.Cells[1, 3].Value = "TU名称";

                    worksheet.Cells[1, 4].Value = "基板号";
                    worksheet.Cells[1, 5].Value = "基岛号";
                    worksheet.Cells[1, 6].Value = "焊点名称";

                    worksheet.Cells[1, 7].Value = "X";
                    worksheet.Cells[1, 8].Value = "Y";
                    worksheet.Cells[1, 9].Value = "面积";


                    int rowNum = worksheet.Rows.Count();

                    worksheet.Cells[rowNum + 1, 1].Value = DateTime.Now.ToString();
                    worksheet.Cells[rowNum + 1, 2].Value = this.Name;
                    worksheet.Cells[rowNum + 1, 3].Value = bondPosition.TuName;
                    worksheet.Cells[rowNum + 1, 4].Value = bondPosition.SubstrateNum;

                    worksheet.Cells[rowNum + 1, 5].Value = bondPosition.ModuleNum;
                    worksheet.Cells[rowNum + 1, 6].Value = bondPosition.Name;

                    worksheet.Cells[rowNum + 1, 7].Value = avgX.Average() * 1000.0;
                    worksheet.Cells[rowNum + 1, 8].Value = avgY.Average() * 1000.0;
                    worksheet.Cells[rowNum + 1, 9].Value = avgArea.Average();

                    package.Save();
                }
                catch (Exception e)
                {
                    LogHelper.Post(Level.Error, $"胶量检测数打印失败！", e, LogCategory.Bond);

                    AKRSMessageBoxExt.Show("胶量检测数据打印失败,请检查数据文件是否被打开！", "异常", new string[] { "确认" }, new DialogResult[] { DialogResult.OK });
                }
            }

            //DefectStatisticsEntity defect = new DefectStatisticsEntity(
            //        DateTime.Now,
            //        this.Name,
            //        bondPosition.TuName,
            //        bondPosition.SubstrateNum,
            //        bondPosition.ModuleNum,
            //        bondPosition.Name,
            //        avgX.Average() * 1000.0,
            //        avgY.Average() * 1000.0,
            //        0,
            //        avgArea.Average(),
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0,
            //        0);

            //StatisticsDomain.GetInstance().AddDefectResult(defect);

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 焊后检测结果处理
        /// </summary>
        /// <param name="prName">PR名称</param>
        /// <param name="point4D">定位结果</param>
        /// <returns>结果</returns>
        public ExcuteResult PostBondCheck(string prName, AKRSPoint4D point4D,PRHardware pRHardware)
        {

            double distanceX = Math.Round(point4D.X - this.PostBondDistanceX, 3);

            double distanceY = Math.Round(point4D.Y - this.PostBondDistanceY, 3);

            double distanceAngle = Math.Round(point4D.T - this.PostBondDistanceAngle /*- BondPosition.CoordinateSystem.DegreeInG0()*/, 3);

            string alarmMessage = string.Empty;


            // X方向判断
            if (Math.Abs(distanceX) > this.LimitX)
            {
                alarmMessage =
                    $"{this.Name} X方向的检测阈值为{this.LimitX},当前值为{distanceX}\r\n";
            }

            // Y方向判断
            if (Math.Abs(distanceY) > this.LimitY)
            {
                alarmMessage +=
                    $"{this.Name}  Y方向的检测阈值为 {this.LimitY} ,当前值为 {distanceY}\r\n";
            }

            // 角度判断
            if (Math.Abs(distanceAngle) > this.LimitAngle)
            {
                alarmMessage +=
                    $"{this.Name}  角度检测阈值为 {this.LimitAngle} ,当前值为 {distanceAngle}\r\n";
            }

            if (alarmMessage != string.Empty)
            {
                return this.PostBondCheckHand(prName, alarmMessage, pRHardware);
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 检测失败后的处理方式
        /// </summary>
        /// <param name="prName">胶检</param>
        /// <param name="alarmMessage">信息</param>
        /// <returns>结果</returns>
        private ExcuteResult PostBondCheckHand(string prName, string alarmMessage,PRHardware pRHardware)
        {
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);

            if (pREntity == null)
            {
                throw new Exception($"找不到名为{prName}的PR!");
            }

            (DialogResult dialogResult, BaseAlgResult match) result = UcMainSystem.VisionAlarmLockFunc(
                pREntity,
                prName + $"{this.ApplicationSystem.ToString()}失败\r\n" + "原因：" + alarmMessage,
                $"{this.ApplicationSystem.ToString()}失败\r\n", pRHardware);
            if (result.dialogResult == DialogResult.OK)
            {
                return ExcuteResult.Success;
            }
            else if (result.dialogResult == DialogResult.Ignore)
            {
                return ExcuteResult.Fail;
            }
            else if (result.dialogResult == DialogResult.Abort)
            {
                return ExcuteResult.Abort;
            }
            else if (result.dialogResult == DialogResult.Retry)
            {
                return ExcuteResult.Retry;
            }
            else
            {
                throw new Exception("检测未知错误");
            }
        }

        /// <summary>
        /// 是否需要检测
        /// </summary>
        /// <returns>结果</returns>
        public bool IsNeedDefect()
        {
            if (!this.IsDefectFrequencyOpen)
            {
                return true;
            }

            if (this.CurrentDefectNumber >= this.DefectFrequency)
            {
                return true;
            }

            this.CurrentDefectNumber++;
            return false;
        }

        /// <summary>
        /// 完成检测
        /// </summary>
        public void FinishedDefect()
        {
            this.CurrentDefectNumber = 0;
        }

        /// <summary>
        /// 完成检测
        /// </summary>
        public void InitDefectCount()
        {
            this.CurrentDefectNumber = this.DefectFrequency;
        }
    }
}
