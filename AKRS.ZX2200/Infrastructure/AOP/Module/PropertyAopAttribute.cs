using System;
using MethodBoundaryAspect.Fody.Attributes;

namespace AKRS.ZX2200.Infrastructure.AOP.Module
{
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Models;

    using log4net.Core;

    /// <summary>
    /// 属性的特性
    /// 标记之后会记录属性值的改变
    /// 所有方法命名禁止以get_或者set_开头
    /// </summary>
    public class PropertyAopAttribute : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 是否起用
        /// </summary>
        public static bool IsPropertyAopEnable { get; set; } = false;

        /// <summary>
        /// 名称
        /// </summary>
        /// <param name="name">属性展示的名称</param>
        public PropertyAopAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        private readonly string name;

        /// <summary>
        /// 值
        /// </summary>
        private object value;

        /// <summary>
        /// 在进入时发生的放生
        /// </summary>
        /// <param name="arg">方法的信息</param>
        public override void OnEntry(MethodExecutionArgs arg)
        {
            if (!IsPropertyAopEnable)
            {
                return;
            }

            string methodName = arg.Method.Name;

            // 判断是不是get方法
            if (methodName.StartsWith("get_"))
            {
                return;
            }

            if (arg.Instance == null)
            {
                return;
            }

            Regex regex = new Regex("set");

            methodName = regex.Replace(methodName, "get", 1);

            Type type = arg.Instance.GetType();

            MethodInfo method = type.GetMethod(methodName);

            if (method != null)
            {
               this.value = method.Invoke(arg.Instance, null);
            }
        }

        /// <summary>
        /// 在退出的时候发生
        /// </summary>
        /// <param name="arg">方法的信息</param>
        public override void OnExit(MethodExecutionArgs arg)
        {
            if (!IsPropertyAopEnable)
            {
                return;
            }

            string methodName = arg.Method.Name;

            // 判断是不是get方法
            if (methodName.StartsWith("get_"))
            {
                return;
            }

            object afterValue = arg.Arguments[0];

            if (this.value == null)
            {
                return;
            }

            if (afterValue.ToString() != this.value.ToString())
            {
                LogHelper.Post(
                    Level.Info,
                    $"{this.name} 的值改变： {this.value} => {afterValue}",
                    LogCategory.MainSoftWare,
                    ViewType.InFileAndUI);
            }
        }
    }
}
