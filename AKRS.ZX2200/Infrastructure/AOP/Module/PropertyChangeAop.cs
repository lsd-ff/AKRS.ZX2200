using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.AOP.Module
{
    using System.Reflection;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.SupportFeature.Statistics;

    using Newtonsoft.Json;

    /// <summary>
    /// 基础跟踪类
    /// </summary>
    [Serializable]
    public abstract class PropertyChangeAop : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性更改事件
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 是否初始化完成
        /// </summary>
        public static bool InitSuccess { get; set; } = false;

        /// <summary>
        /// 属性更改记录
        /// </summary>
        /// <param name="ob">对象</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="before">开始</param>
        /// <param name="after">结束</param>
        public static void OnPropertyChangedEvent(object ob, string propertyName, object before, object after)
        {
            if (!InitSuccess)
            {
                return;
            }

            var property = ob.GetType().GetProperty(propertyName);
            TreeProgramListArgsAttribute attribute = (TreeProgramListArgsAttribute)property?.GetCustomAttribute(typeof(AKRS.ZX2200.SupportFeature.Parameters.Model.TreeProgramListArgsAttribute));
            string name = attribute?.ShowText;

            if (before == null || after == null)
            {
                return;
            }

            if (before.ToString() == string.Empty || after.ToString() == string.Empty)
            {
                return;
            }

            if (before.Equals(after))
            {
                return;
            }

            if (IsPoint3DEquals(before, after))
            {
                return;
            }

            if (name == null)
            {
                return;
            }

            string obName = (string)ob.GetType().GetProperty("Name")?.GetValue(ob, null);

            ParameterChangeLogEntity parameterChangeLogEntity = new ParameterChangeLogEntity(
                DateTime.Now,
                 RuntimeProvider.CurrentLoginUser?.Name,
                name,
                $"{obName} : {name}值由{before}变成{after}");

            StatisticsService.DbEntityBlockingCollection.Add(parameterChangeLogEntity);
        }

        /// <summary>
        /// 属性更改记录
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="before">开始</param>
        /// <param name="after">结束</param>
        protected virtual void OnPropertyChanged(string propertyName, object before, object after)
        {
            OnPropertyChangedEvent(this, propertyName, before, after);
        }
        
        /// <summary>
        /// 3D点位
        /// </summary>
        /// <param name="before">之前的</param>
        /// <param name="after">之后的</param>
        /// <returns>结果</returns>
        private static bool IsPoint3DEquals(object before, object after)
        {
            if (before is AKRSPoint3D point3DBefore && after is AKRSPoint3D point3DAfter)
            {
                if (point3DBefore.X == point3DAfter.X
                    && point3DBefore.Y == point3DAfter.Y
                    && point3DBefore.Z == point3DAfter.Z)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}
