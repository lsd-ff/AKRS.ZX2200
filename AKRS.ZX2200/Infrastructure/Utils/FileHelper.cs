namespace AKRS.ZX2200.Infrastructure.Utils
{
    using AKRS.Base.SimpleXml;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using DevExpress.DataAccess.DataFederation;
    using DevExpress.ExpressApp;
    using DevExpress.Office.Utils;
    using DevExpress.XtraEditors;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// 文件帮助类
    /// 主要包涵对文件的操作
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// 将结果存到Excel里面
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="path">路径</param>
        public static void SaveDoubleExcel(List<List<double>> data, string path)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // 创建文件
            ExcelPackage package = new ExcelPackage(path);

            bool a = Directory.Exists(path);


            bool b = File.Exists("D:");
            // 添加一个工作表
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0
                                           ? package.Workbook.Worksheets[0]
                                           : package.Workbook.Worksheets.Add(/*DateTime.Now +*/ "sheet");

            // 设置列宽
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    worksheet.Cells[i + 1, j + 1].Value = data[i][j];
                }
            }

            package.Save();
            package.Dispose();
        }

        /// <summary>
        /// 将结果存到Excel里面
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="path">路径</param>
        public static void SaveExcel(List<List<object>> data, string path)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // 创建文件
            ExcelPackage package = new ExcelPackage(path);

            // 添加一个工作表
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0
                                           ? package.Workbook.Worksheets[0]
                                           : package.Workbook.Worksheets.Add(/*DateTime.Now +*/ "sheet");

            // 设置列宽
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            for (int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[i].Count; j++)
                {
                    worksheet.Cells[i + 1, j + 1].Value = data[i][j];
                }
            }

            package.Save();
            package.Dispose();
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="rootPath">路径</param>
        /// <returns>结果</returns>
        public static string CreateFileByDate(string rootPath)
        {
            List<string> paths = new List<string>();

            string allPath = "D:" + "\\" + rootPath;

            if (!Directory.Exists(allPath))
            {
                Directory.CreateDirectory(allPath);
            }

            // 当前配方名称
            paths.Add(MachineConfigContext.GetInstance().CurrentRecipe.RecipeName);

            paths.Add(DateTime.Now.Year.ToString() + "年");
            paths.Add(DateTime.Now.Month.ToString() + "月");
            paths.Add(DateTime.Now.Day.ToString() + "日");
            foreach (string path in paths)
            {
                allPath = System.IO.Path.Combine(allPath, path);
                if (!Directory.Exists(allPath))
                {
                    Directory.CreateDirectory(allPath);
                }
            }

            return allPath;
        }

        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="excelPath">地址</param>
        /// <param name="data">数据</param>
        public static void AppendExcelData(string excelPath, List<object> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // 创建文件
            ExcelPackage package = new ExcelPackage(excelPath);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0
                                           ? package.Workbook.Worksheets[0]
                                           : package.Workbook.Worksheets.Add(/*DateTime.Now +*/ "sheet");


            for (int i = 1; i < worksheet.Cells.Rows; i++)
            {
                if (worksheet.Cells[i, 1].Value == null)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        worksheet.Cells[i, j + 1].Value = data[j].ToString();
                    }

                    break;
                }
            }

            package.Save();
            package.Dispose();
        }

        /// <summary>
        /// 追加数据
        /// </summary>
        /// <param name="excelPath">地址</param>
        /// <param name="data">数据</param>
        public static void AppendExcelData(string excelPath, List<List<object>> data)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // 创建文件
            ExcelPackage package = new ExcelPackage(excelPath);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0
                                           ? package.Workbook.Worksheets[0]
                                           : package.Workbook.Worksheets.Add(/*DateTime.Now +*/ "sheet");


            for (int i = 1; i < worksheet.Cells.Rows; i++)
            {
                if (worksheet.Cells[i, 1].Value == null)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        for (int k = 0; k < data[j].Count; k++)
                        {
                            worksheet.Cells[i + j, k + 1].Value = data[j][k].ToString();
                        }
                    }

                    break;
                }
            }

            package.Save();
            package.Dispose();
        }


        /// <summary>
        /// 保存到Excel文件中去
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="source">对象啊</param>
        /// <param name="saveFileName">地址</param>
        public static void Export<T>(List<T> source, string saveFileName)
        {
            while (true)
            {
                if (File.Exists(saveFileName))
                {
                    saveFileName += "副本";
                }
                else
                {
                    break;
                }
            }

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // 创建文件
            ExcelPackage package = new ExcelPackage(saveFileName);

            ExcelWorksheet worksheet = package.Workbook.Worksheets.Count > 0
                                           ? package.Workbook.Worksheets[0]
                                           : package.Workbook.Worksheets.Add(/*DateTime.Now +*/ "sheet");
            try
            {
                Export(1, source, worksheet);

                package.Save();
                package.Dispose();
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show("数据保存失败" + ex.Message);
            }
        }

        /// <summary>
        /// 保存到excel
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="start">表中开始的位置</param>
        /// <param name="source">对象集合</param>
        /// <param name="worksheet">表</param>
        public static void Export<T>(int start, List<T> source, ExcelWorksheet worksheet)
        {
            // 写入字段
            Type t = typeof(T);
            PropertyInfo[] infos = t.GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                // 如果是基础类型，直接写入值
                if (infos[i].PropertyType.IsPrimitive)
                {
                    worksheet.Cells[start, i + 1].Value = infos[i].Name;

                    for (int j = 0; j < source.Count; j++)
                    {
                        worksheet.Cells[1 + j, i + start].Value = infos[i].GetValue(source[j]);
                    }
                }
                else
                {
                    // 这一段应该用递归去实现，但是T类型无法强制转化，看看能不能改

                    //Type c = infos[i].PropertyType;
                    //List<object> list = new List<object>();
                    //for (int j = 0; j < source.Count; j++)
                    //{
                    //     object ob = infos[i].GetValue(source[j]);
                    //     list.Add(ob);
                    //     Export(i, list, worksheet);
                    //}


                    // 如果无法递归实现，只能找固定的层数，下面代码是实现找第二层
                    PropertyInfo[] infos2 = infos[i].GetValue(source[0]).GetType().GetProperties();

                    for (int j = 0; j < infos2.Length; j++)
                    {
                        worksheet.Cells[start, i + 1].Value = infos2[j].Name;

                        for (int k = 0; k < source.Count; k++)
                        {
                            object value = infos[i].GetValue(source[k]);

                            worksheet.Cells[1 + j, i + start].Value = infos2[j].GetValue(value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取一个对象的所有属性
        /// </summary>
        /// <param name="ob">对象名称</param>
        /// <returns>结果</returns>
        public static List<string> GetObjectAllProperties(Type type)
        {
            List<string> propertiesName = new List<string>();
            PropertyInfo[] infos = type.GetProperties();
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].PropertyType.IsPrimitive)
                {
                    propertiesName.Add(infos[i].Name);
                }
                else
                {
                    propertiesName.AddRange(GetObjectAllProperties(infos[i].PropertyType));
                }
            }

            return propertiesName;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inputForce"></param>
        /// <param name="angle"></param>
        /// <param name="beforeTouchForce"></param>
        /// <param name="touchForce"></param>
        /// <param name="riseForce"></param>
        public static void SaveForceControlData(List<(DateTime dateTime, double targetForce, double angle, double beforeTouchForce, double
                                                    posZAfter, double touchBondheadForce, double touchForce, double riseForce, double time,
                                                    double forceReference, double touchBondheadForceBefore, double touchForceBefore, double
                                                    posZBefore)> source)
        {
            try
            {
                // 添加一个工作表
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                ExcelPackage excelPackage = new ExcelPackage(
                    new FileInfo(
                        $"D:\\设备功能测试\\焊头力控稳定性测试数据{DateTime.Now:yyyy-MM-dd}.xlsx"));

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
                    worksheet.Cells[1, 1].Value = "时间";

                    worksheet.Cells[1, 2].Value = "目标力";

                    worksheet.Cells[1, 3].Value = "角度";

                    worksheet.Cells[1, 4].Value = "未接触前力g";

                    worksheet.Cells[1, 5].Value = "延时后的Z轴坐标";

                    worksheet.Cells[1, 6].Value = "延时后的LVDT值/焊头力";

                    worksheet.Cells[1, 7].Value = "延时后平台压力传感器的力g";

                    worksheet.Cells[1, 8].Value = "抬起后力g";

                    worksheet.Cells[1, 9].Value = "力控下压到力控抬起时间(ms)";

                    worksheet.Cells[1, 10].Value = "模拟量输入值";

                    worksheet.Cells[1, 11].Value = "延时前LVDT/焊头力";

                    worksheet.Cells[1, 12].Value = "延时前平台压力传感器的力g";

                    worksheet.Cells[1, 13].Value = "延时前的Z轴坐标";
                }

                int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                // 写入数据
                for (int i = 0; i < source.Count; i++)
                {
                    int row = lastUsedRow + 1;

                    worksheet.Cells[row + i, 1].Value = source[i].dateTime.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[row + i, 2].Value = source[i].targetForce;

                    worksheet.Cells[row + i, 3].Value = source[i].angle;

                    worksheet.Cells[row + i, 4].Value = source[i].beforeTouchForce;


                    worksheet.Cells[row + i, 5].Value = source[i].posZAfter;

                    worksheet.Cells[row + i, 6].Value = source[i].touchBondheadForce;
                    worksheet.Cells[row + i, 7].Value = source[i].touchForce;

                    worksheet.Cells[row + i, 8].Value = source[i].riseForce;

                    worksheet.Cells[row + i, 9].Value = source[i].time;
                    worksheet.Cells[row + i, 10].Value = source[i].forceReference;
                    worksheet.Cells[row + i, 11].Value = source[i].touchBondheadForceBefore;
                    worksheet.Cells[row + i, 12].Value = source[i].touchForceBefore;

                    worksheet.Cells[row + i, 13].Value = source[i].posZBefore;
                }

                excelPackage.Save();
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
                    SaveForceControlData(source);
                }
            }
        }

        /// <summary>
        /// 递归导出对象（支持嵌套对象，如：User → Address → City）
        /// </summary>
        /// <param name="rowStart">起始行</param>
        /// <param name="colStart">起始列</param>
        /// <param name="source">数据源</param>
        /// <param name="worksheet">工作表</param>
        /// <returns>导出后占用的最后一列索引（用于外层继续写入）</returns>
        public static int Export<T>(int rowStart, int colStart, List<T> source, ExcelWorksheet worksheet)
        {
            if (source == null || source.Count == 0)
                return colStart;

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int currentCol = colStart;

            foreach (PropertyInfo prop in properties)
            {
                Type propType = prop.PropertyType;

                // ✅ 基础类型 / 字符串 / 可空基础类型 → 直接导出
                if (IsSimpleType(propType))
                {
                    // 写表头
                    worksheet.Cells[rowStart, currentCol].Value = prop.Name;

                    // 写数据行
                    for (int i = 0; i < source.Count; i++)
                    {
                        object value = prop.GetValue(source[i]);
                        worksheet.Cells[rowStart + 1 + i, currentCol].Value = value;
                    }

                    currentCol++; // 列右移
                }
                // ✅ 复杂对象 → 递归导出
                else
                {
                    // 把当前属性的所有值取出来，变成 List<object> 传给递归
                    List<object> nestedList = new List<object>();
                    foreach (var item in source)
                    {
                        nestedList.Add(prop.GetValue(item));
                    }

                    // 递归导出，自动获取子对象占用的列数
                    currentCol = Export(rowStart, currentCol, nestedList, worksheet);
                }
            }

            return currentCol;
        }

        // 非泛型重载：嵌套List<object>专用，不再依赖T，从实例取真实类型
        private static int Export(int rowStart, int colStart, List<object> source, ExcelWorksheet worksheet)
        {
            if (source == null || source.Count == 0)
                return colStart;

            // 遍历找到第一个非null对象，获取真实实体类型
            Type realModelType = null;
            foreach (var obj in source)
            {
                if (obj != null)
                {
                    realModelType = obj.GetType();
                    break;
                }
            }
            // 全部为null则不输出任何列
            if (realModelType == null)
                return colStart;

            PropertyInfo[] props = realModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            int currentCol = colStart;

            foreach (var prop in props)
            {
                Type propType = prop.PropertyType;
                if (IsSimpleType(propType))
                {
                    worksheet.Cells[rowStart, currentCol].Value = prop.Name;
                    for (int i = 0; i < source.Count; i++)
                    {
                        var parent = source[i];
                        var cellVal = parent == null ? null : prop.GetValue(parent);
                        worksheet.Cells[rowStart + 1 + i, currentCol].Value = cellVal;
                    }
                    currentCol++;
                }
                else
                {
                    List<object> childList = new List<object>();
                    bool allNull = true;
                    foreach (var obj in source)
                    {
                        var child = obj == null ? null : prop.GetValue(obj);
                        childList.Add(child);
                        if (child != null) allNull = false;
                    }
                    if (!allNull)
                        currentCol = Export(rowStart, currentCol, childList, worksheet);
                }
            }
            return currentCol;
        }


        /// <summary>
        /// 判断是否为基础类型（可直接写入Excel）
        /// </summary>
        private static bool IsSimpleType(Type type)
        {
            // 处理可空类型 int? DateTime? 等
            Type underlyingType = Nullable.GetUnderlyingType(type) ?? type;

            return underlyingType.IsPrimitive ||
                   underlyingType.IsEnum ||
                   underlyingType == typeof(string) ||
                   underlyingType == typeof(decimal) ||
                   underlyingType == typeof(DateTime) ||
                   underlyingType == typeof(Guid);
        }

        public static void Export2<T>(List<T> source, string fileName) 
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var sheet = package.Workbook.Worksheets.Add("Sheet1");

                // 从第1行第1列开始导出
                Export(1, 1, source, sheet);

                package.SaveAs(new FileInfo(fileName));
            }
        }
    }
}
