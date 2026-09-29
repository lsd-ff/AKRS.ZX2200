using System;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using System.Collections;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.CalibSystem.GlobalCalibration;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Infrastructure.Service;

/// <summary>
/// 导出服务
/// </summary>
public static class ExportService
{
    /// <summary>
    /// 导出到Excel
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="source">集合</param>
    /// <param name="filePath">文件名</param>
    /// <param name="sheetName">工作表名</param>
    public static void ExportToXlsx<T>(this IEnumerable<T> source, string filePath, string sheetName = "sheet")
    {
        if (source == null || !source.Any())
        {
            throw new ArgumentException("源集合为空或无元素。", nameof(source));
        }

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using ExcelPackage package = new(new FileInfo(filePath));

        T[] enumerable = source as T[] ?? source.ToArray();
        string[] headers;
        Func<T, object[]> getValuesFunc;

        if (typeof(T).IsValueType || typeof(T) == typeof(string))
        {
            headers = new[] { "Value" };
            getValuesFunc = item => new object[] { item };
        }
        else if (typeof(T).GetInterface(nameof(IEnumerable)) != null && typeof(T) != typeof(string))
        {
            headers = new[] { "Index", "Value" };
            getValuesFunc = item =>
            {
                IEnumerable collection = item as IEnumerable ?? new object[] { item };
                return collection.Cast<object>().Select((value, index) => new object[] { index, value })
                    .SelectMany(x => x).ToArray();
            };
        }
        else if (typeof(T).Name.StartsWith("ValueTuple"))
        {
            FieldInfo[] tupleFields = typeof(T).GetFields();
            headers = tupleFields.Select(f => f.Name).ToArray();
            getValuesFunc = item => tupleFields.Select(f => f.GetValue(item)).ToArray();
        }
        else
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            headers = properties.Select(p => p.Name).ToArray();
            getValuesFunc = item => properties.Select(p => p.GetValue(item)).ToArray();
        }

        string formattedSheetName = $"{sheetName}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}";
        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(formattedSheetName);

        try
        {
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
            }

            for (int row = 0; row < enumerable.Length; row++)
            {
                object[] values = getValuesFunc(enumerable[row]);
                for (int col = 0; col < values.Length; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = values[col] is DateTime dateTimeValue
                        ? dateTimeValue.ToString("yyyy-MM-dd HH:mm:ss.fff")
                        : values[col];
                }
            }

            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            package.Save();
        }
        catch (Exception e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show($"文件：{filePath}可能已被打开，请关闭后重试！", "错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

            if (dialog == DialogResult.Retry)
            {
                ExportToXlsx<T>(source, filePath, sheetName);
            }
        }
    }

    /// <summary>
    /// 格式化导出使用
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static object Export(this AKRSPoint2D pos) => new { X = pos.X, Y = pos.Y };

    /// <summary>
    /// 格式化导出使用
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static object Export(this AKRSPoint3D pos) => new { X = pos.X, Y = pos.Y, Z = pos.Z };

    /// <summary>
    /// 格式化导出使用
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static object Export(this AKRSPoint4D pos) => new { X = pos.X, Y = pos.Y, Z = pos.Z, T = pos.T };

    /// <summary>
    /// 格式化导出使用
    /// </summary>
    /// <param name="matchResult"></param>
    /// <returns></returns>
    public static object Export(this MatchResult matchResult) => new { X = matchResult.CenterX, Y = matchResult.CenterY, Angle = matchResult.Angle };

    public static object Export(this GlobalCalibrationData data) => new
    {
        世界坐标X = data.WorldX, 
        世界坐标Y = data.WorldY, 
        轴坐标X = data.AxisX, 
        轴坐标Y = data.AxisY, 
        扫描位置X = data.ScanAxisX,
        扫描位置Y = data.ScanAxisY, 
        扫描偏差X = data.OffsetX, 
        扫描偏差Y = data.OffsetY
    };

    /// <summary>
    /// 确保目录存在，如果不存在则创建
    /// </summary>
    /// <param name="path">目录路径</param>
    /// <returns>是否成功创建（或已存在）</returns>
    public static bool EnsureDirectoryExists(string path)
    {
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true; // 新创建
            }
            return true; // 已存在
        }
        catch (Exception ex)
        {
            // 可以根据需要记录日志
            Console.WriteLine($"创建目录失败: {ex.Message}");
            return false;
        }
    }
}