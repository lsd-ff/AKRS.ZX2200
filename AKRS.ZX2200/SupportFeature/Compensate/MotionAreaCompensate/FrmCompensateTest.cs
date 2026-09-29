using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.Infrastructure.Service;
using DevExpress.XtraEditors;
using LanguageExt.ClassInstances;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    public partial class FrmCompensateTest : DevExpress.XtraEditors.XtraForm
    {
        public FrmCompensateTest()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            AKRSPoint3D startPoint = new AKRSPoint3D();

            AKRSPoint3D endPoint = new AKRSPoint3D();

            AKRSPoint3D markStartPoint = new AKRSPoint3D();


            while (true)
            {
                List<List<double>> list = new List<List<double>>();

                for (int i = 0; i < 20; i++)
                {
                    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(
                        new AKRSPoint3D(markStartPoint.X + i * 3, markStartPoint.Y, markStartPoint.Z));

                    Thread.Sleep(1000);

                    // 视觉定位
                    // 执行定位
                    MatchResult baseAlg = (MatchResult)VisionService.Vision(
                        "测试",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "Test",
                        false);
                    list.Add(
                        new List<double>()
                            {
                                baseAlg.CenterX, baseAlg.CenterY, markStartPoint.X + i * 3, markStartPoint.Y
                            });
                }

                // 创建一个新的 Excel 包
                ExcelPackage excelPackage = new ExcelPackage();
                var workbook = excelPackage.Workbook;
                var worksheet1 = workbook.Worksheets.Add("Sheet1");

                // 写入数据
                for (int i = 1; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        worksheet1.Cells[i + 1, j + 1].Value = list[i][j];
                    }
                }

                string test = DateTime.Now.ToString("MM-dd-HH-mm-ss-fff");

                // 保存文件
                excelPackage.SaveAs(new System.IO.FileInfo($"E:\\温漂测试\\测试{test}.xlsx"));

                DateTime dateTime = DateTime.Now;

                while ((dateTime - DateTime.Now).TotalMinutes > 30)
                {
                    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(startPoint);
                    Thread.Sleep(100);
                    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(endPoint);
                    Thread.Sleep(100);
                }
            }
        }
    }
}