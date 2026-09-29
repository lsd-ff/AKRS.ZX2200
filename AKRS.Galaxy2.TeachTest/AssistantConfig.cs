#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/17 14:35:10
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

namespace AKRS.ZX2200.Models.CommonModels
{
    /// <summary>
    /// 描述：Assistant 界面配置
    /// </summary>
    public class AssistantConfig
    {
        /// <summary>
        /// 步骤索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Descritpion { get; set; }

        /// <summary>
        /// 当前步骤是否显示
        /// </summary>
        public bool IsShowTitle { get; set; }

        /// <summary>
        /// 是否显示Back按钮
        /// </summary>
        public bool IsShowBack { get; set; }

        /// <summary>
        /// 是否显示 Next按钮
        /// </summary>
        public bool IsShowNext { get; set; }

        /// <summary>
        /// 是否显示 Done按钮
        /// </summary>
        public bool IsShowDone { get; set; }

        /// <summary>
        /// Back 按钮事件
        /// </summary>
        public Action BackAction { get; set; }

        /// <summary>
        /// Next 按钮事件
        /// </summary>
        public Action NextAction { get; set; }

        /// <summary>
        /// Done 按钮事件
        /// </summary>
        public Action DoneAction { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="descritpion">描述</param>
        /// <param name="isShowTitle">是否显示</param>
        /// <param name="isShowBack">是否显示Back按钮</param>
        /// <param name="isShowNext">是否显示Next按钮</param>
        /// <param name="isShowDone">是否显示Done按钮</param>
        /// <param name="backAction">next 按钮点击事件</param>
        /// <param name="nextAction">next 按钮点击事件</param>
        /// <param name="doneAction">done 按钮点击事件</param>
        public AssistantConfig(int index, string descritpion, bool isShowTitle, bool isShowBack, bool isShowNext, bool isShowDone, Action backAction, Action nextAction, Action doneAction)
        {
            this.Index = index;
            this.Descritpion = descritpion;
            this.IsShowTitle = isShowTitle;
            this.IsShowBack = isShowBack;
            this.IsShowNext = isShowNext;
            this.IsShowDone = isShowDone;
            this.BackAction = backAction;
            this.NextAction = nextAction;
            this.DoneAction = doneAction;
        }
    }
}
