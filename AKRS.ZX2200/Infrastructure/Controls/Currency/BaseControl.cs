namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using System.Windows.Forms;

    using DevExpress.Utils.Menu;
    using DevExpress.XtraEditors;
    using DevExpress.XtraGrid;

    /// <summary>
    /// 模块基类
    /// </summary>
    public class BaseControl : XtraUserControl
    {
        /// <summary>
        /// StartGroup
        /// </summary>
        public DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer StartGroup;

        /// <summary>
        /// 等待框
        /// </summary>
        public virtual bool AllowWaitDialog => true;

        /// <summary>
        /// 模块标题
        /// </summary>
        public virtual string ModuleCaption => string.Empty;

        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="manager">菜单接口</param>
        /// <param name="data">data</param>
        internal virtual void InitModule(IDXMenuManager manager, object data)
        {
            this.SetMenuManager(this.Controls, manager);
        }

        /// <summary>
        /// 菜单管理设定
        /// </summary>
        /// <param name="controlCollection">控件集合</param>
        /// <param name="manager">菜单</param>
        private void SetMenuManager(ControlCollection controlCollection, IDXMenuManager manager)
        {
            foreach (Control ctrl in controlCollection)
            {
                if (ctrl is GridControl grid)
                {
                    grid.MenuManager = manager;
                    break;
                }

                if (ctrl is BaseEdit edit)
                {
                    edit.MenuManager = manager;
                    break;
                }

                this.SetMenuManager(ctrl.Controls, manager);
            }
        }
    }
}
