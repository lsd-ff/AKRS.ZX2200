#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/5/10 19:18:46
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.UserManager.Models;

namespace AKRS.Galaxy2.MachineSupport
{
    /// <summary>
    /// 描述：运行时环境参数
    /// </summary>
    public class RuntimeProvider
    {
        /// <summary>
        /// 当前登录账号
        /// </summary>
        public static User CurrentLoginUser { get; set; }

        /// <summary>
        /// 全局线程运行标志 ,你可以在系统关闭时 将此值设置为false , 那么系统里所有的循环线程将会终止
        /// </summary>
        public static volatile bool ThreadFlag = true;

        /// <summary>
        /// 系统类型
        /// </summary>
        public static SystemTypeEnum SystemType = SystemTypeEnum.System2;
    }

    /// <summary>
    /// 系统类型
    /// </summary>
    public enum SystemTypeEnum
    {
        /// <summary>
        /// System1
        /// </summary>
        System1,
        
        /// <summary>
        /// System2
        /// </summary>
        System2
    }
}
