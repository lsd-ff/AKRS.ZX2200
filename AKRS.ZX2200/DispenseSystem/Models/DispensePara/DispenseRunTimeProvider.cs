using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Models;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Models.DispensePara
{
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using DevExpress.XtraEditors;
    using LanguageExt;

    /// <summary>
    /// 点胶运行时的参数
    /// </summary>
    public static class DispenseRunTimeProvider
    {
        /// <summary>
        /// 是否为第一次点胶
        /// </summary>
        public static bool IsFirstDispense { get; set; } = true;

        ///// <summary>
        ///// 克隆的PR列表
        ///// </summary>
        //public static List<PREntity> DispensePREntityList { get; set; } = new List<PREntity>();

        /// <summary>
        /// 计时器
        /// </summary>
        public static Stopwatch System1Stopwatch = new Stopwatch();

        ///// <summary>
        ///// 点胶端获取的PR 是一个深拷贝，防止和bond那边使用同一个PR对象造成资源竞争
        ///// </summary>
        ///// <param name="pREntity">要拷贝的对象</param>
        ///// <returns>PR 深拷贝出的副本</returns>
        //public static PREntity GetDispensePREntity(PREntity pREntity)
        //{
        //    if (pREntity == null)
        //    {
        //        return null;
        //    }

        //    PREntity pr = DispensePREntityList.Find(a => a.GetName() == pREntity.GetName());
        //    if (pr == null)
        //    {
        //        // Copy
        //        pr = pREntity.Clone(pREntity.GetName());
        //        pr.SetHardware(new DispenseVisionController().GetHardware());
        //        DispensePREntityList.Add(pr);
        //    }
        //    else
        //    {
        //        // 如果没拷贝过，或者PR对象做了修改需要重新拷贝。 
        //        if (pREntity.IsSystem1Copy == false)
        //        {
        //            // Copy
        //            pr = pREntity.Clone(pREntity.GetName());
        //            pr.SetHardware(new DispenseVisionController().GetHardware());
        //            pREntity.IsSystem1Copy = true;

        //            // 移除旧的，添加新的
        //            DispensePREntityList.RemoveAll(n => n.GetName() == pREntity.GetName());
        //            DispensePREntityList.Add(pr);
        //        }
        //    }

        //    return pr;
        //}

        /// <summary>
        /// 加载点胶视觉模板
        /// </summary>
        public static void LoadDispensePr()
        {
            //if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            //{
            //    return;
            //}
            
            //try
            //{
            //    DispensePREntityList.Clear();
            //    LogHelper.Post(Level.Info, $"点胶模板清除完成", LogCategory.PR, ViewType.InFileAndUI);

            //    List<BaseVisionEntity> list = VisionEntityRepository.GetInstance().PRVisionList;
            //    foreach (BaseVisionEntity entity in list)
            //    {
            //        if (entity is PREntity)
            //        {
            //            PREntity pREntity = entity as PREntity;
            //            if (pREntity.Alg.AlgBeLong == AlgBeLongEnum.Substrate)
            //            {
            //                LogHelper.Post(Level.Info, $"点胶模板复制模板{pREntity.GetName()}", LogCategory.PR, ViewType.InFileAndUI);
            //                // Copy
            //                PREntity pr = pREntity.Clone(pREntity.GetName());
            //                pr.SetHardware(new DispenseVisionController().GetHardware());
            //                pREntity.IsSystem1Copy = true;

            //                // 移除旧的，添加新的
            //                DispensePREntityList.RemoveAll(n => n.GetName() == pREntity.GetName());
            //                DispensePREntityList.Add(pr);

            //                LogHelper.Post(Level.Info, $"点胶模板复制模板{pREntity.GetName()}复制完成", LogCategory.PR, ViewType.InFileAndUI);
            //            }
            //        }
            //    }

            //    LogHelper.Post(Level.Info, $"点胶模板加载完成", LogCategory.PR, ViewType.InFileAndUI);
            //}
            //catch(Exception ex)
            //{
            //    AKRSXtraMessageBox.Show("点胶模板复制模板出现异常");
            //    LogHelper.Post(Level.Info, $"点胶模板复制模板出现异常", ex, LogCategory.PR, ViewType.InFileAndUI);
            //    throw ex;
            //}
        }

        /// <summary>
        /// 时间记录
        /// </summary>
        /// <param name="item">动作信息</param>
        /// <param name="subItem">动作信息</param>
        public static void RecordTime(string item, string subItem)
        {
            LogHelper.Post(Level.Info,
                    $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.f")}: {item} - {subItem} 耗时：{System1Stopwatch.ElapsedMilliseconds} ms"
                    , LogCategory.Dispense,ViewType.InFileAndUI);

            System1Stopwatch.Restart();
        }
    }
}
