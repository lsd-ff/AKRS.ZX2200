namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram
{
    using System.Collections.Generic;
    using System.Windows.Forms;

    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;

    using DevExpress.XtraTreeList;

    /// <summary>
    /// Configuration,用于显示系统2的Tool配置
    /// </summary>
    public partial class UcConfiguration2 : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UcConfiguration2()
        {
            this.InitializeComponent();
            this.RefreshControl();
        }

        /// <summary>
        /// 当前使用的吸嘴架
        /// </summary>
        private NozzleShelf nozzleShelf => System2Domain.GetInstance().BondProgram.NozzleShelfProgram.NozzleShelf;

        /// <summary>
        /// 当前使用顶针架，测试用
        /// </summary>
        private EjectionBankConfig ejectionBankConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.EjectionBankProgram.CurrentBankConfig;

        /// <summary>
        /// 声明树组件
        /// </summary>
        private TreeList treeList = new TreeList()
        {
            KeyFieldName = "Id",
            ParentFieldName = "ParentId",
            OptionsView = { ShowCaption = false, ShowIndicator = false, ShowHorzLines = false, ShowVertLines = false },
            OptionsBehavior = { Editable = false },
            RowHeight = 20,
            Dock = DockStyle.Fill
        };

        /// <summary>
        /// 树组件数据源
        /// </summary>
        private List<Node> list = new List<Node>();

        /// <summary>
        /// 当前父Id
        /// </summary>
        private int selectNodeParentIndex;

        /// <summary>
        /// 当前子Id
        /// </summary>
        private int selectNodeChildrenIndex;

        /// <summary>
        /// 页面初始化
        /// </summary>
        public void RefreshControl()
        {
            // GridControl绑定数据源
            this.GcPPTool.DataSource = this.nozzleShelf?.NozzleShelfSlots;
            this.GcPPTool.RefreshDataSource();

            this.GcESTool.DataSource = this.ejectionBankConfig?.EjectionBankSlots;
            this.GcESTool.RefreshDataSource();

            // 初始化树组件
            this.InitializeTreeComponent();

            // 控件赋值
            this.LbDispenserName.Text = System1Domain.GetInstance().System1Program.DispenserProgram.DispenserName;
            this.LbEpoxyMaterialName.Text = System1Domain.GetInstance().System1Program.EpoxyMaterialProgram.EpoxyMaterialName;
            this.LbFlipToolName.Text = WaferSystemProgram.GetInstance().FlipModuleProgram.FlipToolName;
        }

        /// <summary>
        /// 初始化树组件
        /// </summary>
        private void InitializeTreeComponent()
        {
            this.RefreshTrees();

            // 绑定TreeList
            this.treeList.DataSource = this.list;
            this.treeList.Columns[0].Caption = @"Allocations";
            this.treeList.Columns[0].OptionsColumn.AllowSort = false;
            this.treeList.Columns[0].OptionsFilter.AllowFilter = false;
            this.treeList.ExpandAll();
            this.treeList.FocusedNodeChanged += this.TreeList1_FocusedNodeChanged;
        }

        /// <summary>
        /// 树结构聚焦事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TreeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            Node node = this.list[e.Node.Id];
            this.selectNodeParentIndex = node.ParentId;
            this.selectNodeChildrenIndex = node.Id - node.ParentId;
        }

        /// <summary>
        /// 刷新树结构
        /// </summary>
        public void RefreshTrees()
        {
            MagazineAllocationsConfig allocationsTemp = WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig;

            if (allocationsTemp == null)
            {
                allocationsTemp = new MagazineAllocationsConfig();
            }

            this.list.Clear();
            int index = 0;
            int parentIndex = 0;

            for (int i = 0; i < allocationsTemp.MaxUseLayerCount; i++)
            {
                parentIndex = index;
                if (allocationsTemp.TabletArray[i] is WaferTablet)
                {
                    WaferTablet wt = (WaferTablet)allocationsTemp.TabletArray[i];
                    this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}: {wt.Name}" });
                    index++;
                }
                else if (allocationsTemp.TabletArray[i] is AdapterTablet)
                {
                    AdapterTablet ad = (AdapterTablet)allocationsTemp.TabletArray[i];
                    this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}: {ad.Name}" });
                    index++;

                    for (int j = 0; j < ad.AdapterSetting.MaxUseWaffleCount; j++)
                    {
                        this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"{index} - {ad.AdapterSetting.WaffleArray[j].Name}" });
                        index++;
                    }
                }
                else if (allocationsTemp.TabletArray[i] is NullTablet)
                {
                    this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}:" });
                    index++;
                }
            }

            this.GcWaferMagazine.Controls.Clear();
            this.GcWaferMagazine.Refresh();
            this.GcWaferMagazine.Controls.Add(this.treeList);
        }
    }
}
