using System;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Infrastructure.Service;

public static class DevControlService
{
    /// <summary>
    /// 获取控件的double类型属性值
    /// </summary>
    /// <param name="edit"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static double GetDouble(this BaseEdit edit)
    {
        try
        {
            return Convert.ToDouble(edit.EditValue);
        }
        catch (Exception e)
        {
            throw new Exception($"获取控件{edit.Name} double类型属性值时发生异常:{e.Message}");
        }
    }

    /// <summary>
    /// 尝试获取控件的double类型属性值，失败时返回0
    /// </summary>
    /// <param name="edit"></param>
    /// <returns></returns>
    public static double TryGetDouble(this BaseEdit edit)
    {
        try
        {
            return Convert.ToDouble(edit.EditValue);
        }
        catch (Exception)
        {
            return 0;
        }
    }

    /// <summary>
    /// 获取控件的int类型属性值
    /// </summary>
    /// <param name="edit"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static int GetInt32(this BaseEdit edit)
    {
        try
        {
            return Convert.ToInt32(edit.EditValue);
        }
        catch (Exception e)
        {
            throw new Exception($"获取控件{edit.Name} double类型属性值时发生异常:{e.Message}");
        }
    }

    /// <summary>
    /// 尝试获取控件的int类型属性值，失败时返回0
    /// </summary>
    /// <param name="edit"></param>
    /// <returns></returns>
    public static int TryGetInt32(this BaseEdit edit)
    {
        try
        {
            return Convert.ToInt32(edit.EditValue);
        }
        catch (Exception)
        {
            return 0;
        }
    }
}