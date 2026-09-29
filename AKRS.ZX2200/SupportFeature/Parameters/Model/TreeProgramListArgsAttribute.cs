namespace AKRS.ZX2200.SupportFeature.Parameters.Model
{
    using System;

    using AKRS.ZX2200.Infrastructure.Models.Enums;

    /// <summary>
    /// 注解类：对象中的注解封装
    /// </summary>
    public class TreeProgramListArgsAttribute : Attribute
    {
        //注：集合支持Point3D和Point2D,子元素的显示内容需要使用Description注解指定
        //构造器不够可以加，注意重载

        /// <summary>
        /// 显示文本,默认为属性名
        /// </summary>
        public string ShowText { get; set; } = null;

        /// <summary>
        /// 是否有父节点，默认没有
        /// </summary>
        public string ParentNode { get; set; } = null;

        /// <summary>
        /// 焦点选择，修改属性时的提示文字，默认没有,除bool和enum的类型都能显示
        /// </summary>
        public string InfoText { get; set; } = null;

        /// <summary>
        /// 传入的值类型最小值，在float double int属性启用功能
        /// </summary>
        public double MinValue { get; set; } = double.MinValue;

        /// <summary>
        /// 传入的值类型最大值，在float double int属性启用功能
        /// </summary>
        public double MaxValue { get; set; } = double.MaxValue;

        /// <summary>
        /// 是否允许用户修改，Point3D集合和Point2D集合会影响所有所属元素
        /// </summary>
        public bool AllowUserChange { get; set; } = true;

        /// <summary>
        /// 属性禁止在哪个界面显示，默认都显示
        /// </summary>
        public TreeGroupChildNodesEnum[] VisibleFlaseEnum { get; set; } = null;

        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; } = string.Empty;

        /// <summary>
        /// 角色等级
        /// </summary>
        public int RoleLevel { get; set; } = 0;

        /// <summary>
        /// 可视性，显示的文本，父节点，提示信息，数值类型的最小值，数值类型的最大值
        /// </summary>
        /// <param name="visibile"></param>
        /// <param name="text"></param>
        /// <param name="parentnode"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, string parentNode, string infoText, double min, double max, TreeGroupChildNodesEnum[] VisibleFalse, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.ShowText = text;
            this.ParentNode = parentNode;
            this.InfoText = infoText;
            this.MinValue = min;
            this.MaxValue = max;
            this.VisibleFlaseEnum = VisibleFalse;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }


        /// <summary>
        /// 可视性，显示的文本，父节点，提示信息，数值类型的最小值，数值类型的最大值
        /// </summary>
        /// <param name="visibile"></param>
        /// <param name="text"></param>
        /// <param name="parentnode"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string showText, string parentNode, double min, double max, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.ShowText = showText;
            this.ParentNode = parentNode;
            this.MinValue = min;
            this.MaxValue = max;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }

        /// <summary>
        /// 可视性，显示的文本，父节点，提示信息
        /// </summary>
        /// <param name="visibile"></param>
        /// <param name="text"></param>
        /// <param name="parentnode"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, string parentNode, string infoText, TreeGroupChildNodesEnum[] VisibleFalse, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.ShowText = text;
            this.ParentNode = parentNode;
            this.InfoText = infoText;
            this.VisibleFlaseEnum = VisibleFalse;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }

        /// <summary>
        /// 显示的文本，父节点
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parentnode"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, string parentNode, double min, double max, TreeGroupChildNodesEnum[] VisibleFalse, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.ShowText = text;
            this.ParentNode = parentNode;
            this.MinValue = min;
            this.MaxValue = max;
            this.VisibleFlaseEnum = VisibleFalse;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }

        /// <summary>
        /// 显示的文本，父节点
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parentnode"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, string parentNode, TreeGroupChildNodesEnum[] VisibleFalse, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.ShowText = text;
            this.ParentNode = parentNode;
            this.VisibleFlaseEnum = VisibleFalse;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }


        /// <summary>
        /// 显示的文本，父节点
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parentnode"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, string parentNode = null, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.ShowText = text;
            this.ParentNode = parentNode;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }

        /// <summary>
        /// 显示的文本，父节点
        /// </summary>
        /// <param name="text"></param>
        /// <param name="parentnode"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, string parentNode, bool AllowUserChange, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.AllowUserChange = AllowUserChange;
            this.ShowText = text;
            this.ParentNode = parentNode;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }

        /// <summary>
        /// 显示的文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, TreeGroupChildNodesEnum[] VisibleFalse, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.ShowText = text;
            this.VisibleFlaseEnum = VisibleFalse;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }

        ///// <summary>
        ///// 显示的文本
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="roleLevel">roleLevel</param>
        //public TreeProgramListArgsAttribute(string text, string unitName = "", RoleEnum role = RoleEnum.Null)
        //{
        //    this.ShowText = text;
        //    this.UnitName = unitName;
        //    this.RoleLevel = (int)role;
        //}

        /// <summary>
        /// 显示的文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="roleLevel">roleLevel</param>
        public TreeProgramListArgsAttribute(string text, bool AllowUserChange, TreeGroupChildNodesEnum[] VisibleFalse, string unitName = "", RoleEnum role = RoleEnum.Engineer)
        {
            this.AllowUserChange = AllowUserChange;
            this.ShowText = text;
            this.VisibleFlaseEnum = VisibleFalse;
            this.UnitName = unitName;
            this.RoleLevel = (int)role;
        }

        /// <summary>
        /// 默认
        /// </summary>
        public TreeProgramListArgsAttribute()
        {
        }
    }
}
