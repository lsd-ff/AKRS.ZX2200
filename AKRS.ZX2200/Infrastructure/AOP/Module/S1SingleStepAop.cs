using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.AOP.Module
{
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.DispenseSystem.Models;

    using DevExpress.XtraEditors;

    using log4net.Core;
    using MethodBoundaryAspect.Fody.Attributes;

    /// <summary>
    /// 单步调试的时候调用的AOP
    /// </summary>
    public class S1SingleStepAop : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 当进入时发生
        /// </summary>
        /// <param name="arg">方法信息</param>
        public override void OnEntry(MethodExecutionArgs arg)
        {
            // 单步工作
            if (!System1Domain.GetInstance().WaitSingleStep())
            {
                throw new Exception("...");
            }
        }
    }
}
