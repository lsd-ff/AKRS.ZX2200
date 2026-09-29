namespace AKRS.ZX2200.SupportFeature.Consumables
{
    using System;
    using System.Collections.Generic;

    using AKRS.ZX2200.Consumables;
    using DevExpress.XtraRichEdit.Import.OpenXml;

    /// <summary>
    /// 耗材类
    /// </summary>
    public partial class FrmConsumables : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 耗材
        /// </summary>
        public FrmConsumables()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 耗材
        /// </summary>
        /// <param name="ppTools">吸嘴工具</param>
        /// <param name="needles">顶针</param>
        /// <param name="dispensers">点胶</param>
        public FrmConsumables(List<FrequencyConsumables> ppTools, List<FrequencyConsumables> needles, List<TimeConsumable> dispensers, TimeConsumable flux)
        {
            this.InitializeComponent();
            this.ppToolConsumables = ppTools;
            this.needleConsumables = needles;
            this.dispenserConsumables = dispensers;
            this.fluxConsumable=flux;
        }

        /// <summary>
        /// 吸嘴集合
        /// </summary>
        private readonly List<FrequencyConsumables> ppToolConsumables = new List<FrequencyConsumables>();

        /// <summary>
        /// 顶针集合
        /// </summary>
        private readonly List<FrequencyConsumables> needleConsumables = new List<FrequencyConsumables>();

        /// <summary>
        /// 点胶头
        /// </summary>
        private readonly List<TimeConsumable> dispenserConsumables = new List<TimeConsumable>();

        /// <summary>
        /// 助焊剂
        /// </summary>
        private readonly TimeConsumable fluxConsumable = new TimeConsumable();


        /// <summary>
        /// 吸嘴控件
        /// </summary>
        private readonly List<UcConsumables> ucPpToolConsumables = new List<UcConsumables>();

        /// <summary>
        /// 顶针控件
        /// </summary>
        private readonly List<UcConsumables> ucNeedleConsumables = new List<UcConsumables>();

        /// <summary>
        /// 点胶控件
        /// </summary>
        private readonly List<UcTimeConsumable> dispenserConsumablesList = new List<UcTimeConsumable>();

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmConsumables_Load(object sender, EventArgs e)
        {
            this.BingingData();
            this.InitData();
        }

        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BingingData()
        {
            this.ucPpToolConsumables.Add(this.ucConsumables1);
            this.ucPpToolConsumables.Add(this.ucConsumables2);
            this.ucPpToolConsumables.Add(this.ucConsumables3);
            this.ucPpToolConsumables.Add(this.ucConsumables4);
            this.ucPpToolConsumables.Add(this.ucConsumables5);
            this.ucPpToolConsumables.Add(this.ucConsumables6);
            this.ucPpToolConsumables.Add(this.ucConsumables7);

            this.ucNeedleConsumables.Add(this.ucConsumables8);
            this.ucNeedleConsumables.Add(this.ucConsumables9);
            this.ucNeedleConsumables.Add(this.ucConsumables10);
            this.ucNeedleConsumables.Add(this.ucConsumables11);
            this.ucNeedleConsumables.Add(this.ucConsumables12);

            this.dispenserConsumablesList.Add(this.ucTimeConsumable1);

            this.dispenserConsumablesList.Add(this.ucTimeConsumable2);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitData()
        {
            for (int i = 0; i < this.ppToolConsumables.Count; i++)
            {
                this.ucPpToolConsumables[i].Init(this.ppToolConsumables[i]);
            }

            for (int i = 0; i < this.needleConsumables.Count; i++)
            {
                this.ucNeedleConsumables[i].Init(this.needleConsumables[i]);
            }

            for (int i = 0; i < this.dispenserConsumables.Count; i++)
            {
                this.dispenserConsumablesList[i].Init(this.dispenserConsumables[i]);
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            foreach (UcConsumables ucConsumable in this.ucNeedleConsumables)
            {
                ucConsumable.RefreshData();
            }

            foreach (UcConsumables ucConsumable in this.ucPpToolConsumables)
            {
                ucConsumable.RefreshData();
            }

            foreach (UcTimeConsumable ucTimeConsumable in this.dispenserConsumablesList)
            {
                ucTimeConsumable.RefreshData();
            }
        }
    }
}