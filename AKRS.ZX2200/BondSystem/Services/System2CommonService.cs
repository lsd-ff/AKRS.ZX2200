using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.WaferSubSystem.Models;
using DevExpress.XtraEditors;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Services
{
    /// <summary>
    /// 系统2服务类（主要存放一些辅助性的功能）
    /// </summary>
    public static class System2CommonService
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 获取当前配方所有PR
        /// </summary>
        /// <returns>result</returns>
        public static List<string> GetCurrentRecipePREntityNameList()
        {
            try
            {
                List<string> list = new List<string>();
                if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig != null)
                {
                    list.AddRange(WaferSystemProgram.GetInstance().GetComponentCarriersPREntityNameList());
                }

                if (WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig != null)
                {
                    list.AddRange(WaferSystemProgram.GetInstance().GetEjectionPREntityNameList());
                }

                if (PostBondInspectionRepository.GetInstance() != null)
                {
                    list.AddRange(BondProgram.GetInstance().PostBondProgram.GetPostBondInspectionNames());
                }

                list.AddRange(BondProgram.GetInstance().NozzleShelfProgram.GetNozzlePRNameList());

                if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
                {
                    foreach (OppositeSexConfig oppositeSexConfig in ProductConfiguration.GetInstance().OppositeSexConfiguration.BaseConfigs)
                    {
                        list.AddRange(oppositeSexConfig.LocateConfig.GetPrNames());
                    }
                }
                else
                {
                    list.AddRange(ProductConfiguration.GetInstance().TransportUnitConfig.LocateConfig.GetPrNames());
                    list.AddRange(ProductConfiguration.GetInstance().TransportUnitConfig.LeftLocateConfig.GetPrNames());
                    list.AddRange(ProductConfiguration.GetInstance().TransportUnitConfig.RightLocateConfig.GetPrNames());
                    list.AddRange(ProductConfiguration.GetInstance().SubstrateConfig.LocateConfig.GetPrNames());
                    list.AddRange(ProductConfiguration.GetInstance().ModuleConfig.LocateConfig.GetPrNames());

                    foreach (SingleBondPositionConfig singleBondPositionConfig in ProductConfiguration.GetInstance()
                                 .BondPositionConfig.SingleBpPositionConfigList)
                    {
                        list.AddRange(singleBondPositionConfig.LocateConfig.GetPrNames());
                    }
                }

                // 中转台模板
                list.Add("IPTCenterCircle");

                return list;
            }
            catch (Exception ex) 
            {
                AKRSXtraMessageBox.Show("获取模板集合失败" + ex.Message);
                throw ex;
            }
           
        }

        /// <summary>
        ///  保存数据
        /// </summary>
        /// <param name="matchResult">定位结果</param>
        /// <param name="fileName">文件名</param>
        public static void SaveVisionResult(MatchResult matchResult, string fileName)
        {
            try
            {
                lock (locker)
                {
                    string path = @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + fileName
                                  + ".xlsx";

                    // 添加一个工作表
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + fileName + ".xlsx"));

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

                        worksheet.Cells[1, 2].Value = "CenterX";

                        worksheet.Cells[1, 3].Value = "CenterY";

                        worksheet.Cells[1, 4].Value = "Angle";

                        worksheet.Cells[1, 5].Value = "AxisPosX";

                        worksheet.Cells[1, 6].Value = "AxisPosY";

                        worksheet.Cells[1, 7].Value = "AxisPosZ";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value = matchResult?.CenterX;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = matchResult?.CenterY;

                    worksheet.Cells[lastUsedRow + 1, 4].Value = matchResult?.Angle;

                    worksheet.Cells[lastUsedRow + 1, 5].Value = System2Domain.GetInstance().BondModuleController.GetAxisXRealPos();

                    worksheet.Cells[lastUsedRow + 1, 6].Value = System2Domain.GetInstance().BondModuleController.GetAxisYRealPos();

                    worksheet.Cells[lastUsedRow + 1, 7].Value = System2Domain.GetInstance().BondHeadController.GetAxisZRealPos();

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
                    SaveVisionResult(matchResult, fileName);
                }

                //Process[] process = Process.GetProcessesByName("wps");
                //foreach (var item in process)
                //{
                //    item.Kill();
                //}

                //Thread.Sleep(100);

                //SaveVisionResult(matchResult, fileName);
            }
        }

        /// <summary>
        /// 杀死占用文件的进程
        /// </summary>
        /// <param name="fileName">被占用文件的完全限定名</param>
        public static void KillFileEmbezzlementProcess(string fileName)
        {
            var p = new Process();
            p.StartInfo.FileName = "D:\\Driver\\Toolkit\\handle64.exe";//handle应用程序的绝对路径,要根据操作系统选择对应版本
            p.StartInfo.Arguments = fileName;//文件名以参数形式传递
            p.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;//设定控制台输出编码格式,保持与当前进程编码一致
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
            p.WaitForExit();
            string str = p.StandardOutput.ReadToEnd();

            //针对Handle返回值进行正则匹配,满足匹配结果的pid进程都应该被杀掉
            MatchCollection matchs = Regex.Matches(str, @"pid:\s*(\d+)\s*type:");
            foreach (Match match in matchs)
            {
                if (match.Success)
                {
                    Process.GetProcessById(int.Parse(match.Groups[1].Value)).Kill();
                }
            }
        }

        /// <summary>
        ///  保存数据
        /// </summary>
        /// <param name="matchResult">定位结果</param>
        /// <param name="fileName">文件名</param>
        /// <param name="tuDegree">基板角度</param>
        /// <param name="angle">角度</param>
        public static void SaveVisionResult(MatchResult matchResult, string fileName, double tuDegree, double angle, MatchResult matchResult2)
        {
            try
            {
                lock (locker)
                {
                    // 添加一个工作表
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + fileName + ".xlsx"));

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

                        worksheet.Cells[1, 2].Value = "Last-CenterX";

                        worksheet.Cells[1, 3].Value = "Last-CenterY";

                        worksheet.Cells[1, 4].Value = "Last-Angle";

                        worksheet.Cells[1, 5].Value = "AxisPosX";

                        worksheet.Cells[1, 6].Value = "AxisPosY";

                        worksheet.Cells[1, 7].Value = "AxisPosZ";

                        worksheet.Cells[1, 8].Value = "TUDegree";

                        worksheet.Cells[1, 9].Value = "FirstVisionAngle";

                        worksheet.Cells[1, 10].Value = "Second-CenterX";

                        worksheet.Cells[1, 11].Value = "Second-CenterY";

                        worksheet.Cells[1, 12].Value = "Second-Angle";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value = matchResult?.CenterX;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = matchResult?.CenterY;

                    worksheet.Cells[lastUsedRow + 1, 4].Value = matchResult?.Angle;

                    worksheet.Cells[lastUsedRow + 1, 5].Value =
                        System2Domain.GetInstance().BondModuleController.GetAxisXRealPos();

                    worksheet.Cells[lastUsedRow + 1, 6].Value =
                        System2Domain.GetInstance().BondModuleController.GetAxisYRealPos();

                    worksheet.Cells[lastUsedRow + 1, 7].Value =
                        System2Domain.GetInstance().BondHeadController.GetAxisZRealPos();

                    worksheet.Cells[lastUsedRow + 1, 8].Value = tuDegree;

                    worksheet.Cells[lastUsedRow + 1, 9].Value = angle;

                    worksheet.Cells[lastUsedRow + 1, 10].Value =
                        matchResult2.CenterX;

                    worksheet.Cells[lastUsedRow + 1, 11].Value = matchResult2.CenterY;

                    worksheet.Cells[lastUsedRow + 1, 12].Value = matchResult2.Angle;

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
                    SaveVisionResult(matchResult, fileName, tuDegree, angle, matchResult2);
                }
            }
        }
    }
}
