#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/10/13 19:21:13
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

namespace Akrs.ChangeJson
{
    /// <summary>
    /// 描述：
    /// </summary>
    public class ClassNameSpace
    {
        public string ClassName { get; set; }

        public string NameSpace { get; set; }

        public ClassNameSpace(string className, string nameSpace)
        {
            this.ClassName = className; 
            this.NameSpace = nameSpace;
        }
    }
}
