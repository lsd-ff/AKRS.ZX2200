using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.AOP.Module
{
    /// <summary>
    /// 系统2单步调试的时候执行的AOP
    /// </summary>
    public class S2SingleStepAop : OnMethodBoundaryAspect
    {
        /// <summary>
        /// 当进入时发生
        /// </summary>
        /// <param name="arg">方法信息</param>
        public override void OnEntry(MethodExecutionArgs arg)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                arg.FlowBehavior = FlowBehavior.Return;
            }
        }
    }
}
