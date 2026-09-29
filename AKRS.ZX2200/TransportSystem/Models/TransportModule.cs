using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.TransportSystem.Modules;

namespace AKRS.ZX2200.TransportSystem.Models
{
    using AKRS.ZX2200.TransportSystem.Controllers;
    using System.Threading.Tasks;

    /// <summary>
    /// 物理流道（流道模组）
    /// </summary>
    public class TransportModule : SingletonNoSave<TransportModule> 
    {
        #region 硬件模组

        /// <summary>
        /// 上料载台
        /// </summary>
        internal LoadingSubSectionModule LoadingSubSectionModule = new LoadingSubSectionModule(); 

        /// <summary>
        /// 点胶载台
        /// </summary>
        internal DispenseSubSectionModule DispenseSubSectionModule = new DispenseSubSectionModule();

        /// <summary>
        /// Bond载台
        /// </summary>
        internal BondSubSectionModule BondSubSectionModule = new BondSubSectionModule();

        /// <summary>
        /// 等待下料载台
        /// </summary>
        internal WaitingUnloadSubSectionModule WaitingUnloadSubSectionModule = new WaitingUnloadSubSectionModule();

        /// <summary>
        /// 下料载台
        /// </summary>
        internal UnloadingSubSectionModule UnloadingSubSectionModule = new UnloadingSubSectionModule();

        /// <summary>
        /// 上料仓
        /// </summary>
        internal LoaderBinModule LoaderBinModule = new LoaderBinModule();

        /// <summary>
        /// 下料仓
        /// </summary>
        internal UnloaderBinModule UnloaderBinModule = new UnloaderBinModule();

        /// <summary>
        /// 固晶大载台
        /// </summary>
        internal BondMaxSubSectionModule BondMaxSubSectionModule = new BondMaxSubSectionModule();

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            Task task1 = Task.Factory.StartNew(() => { this.LoadingSubSectionModule.StopMove(); this.LoadingSubSectionModule.BeltInit(); });
            Task task2 = Task.Factory.StartNew(() => { this.DispenseSubSectionModule.StopMove(); this.DispenseSubSectionModule.BeltInit(); });
            Task task3 = Task.Factory.StartNew(() => { this.BondSubSectionModule.StopMove(); this.BondSubSectionModule.BeltInit(); });
            Task task4 = Task.Factory.StartNew(() => { this.WaitingUnloadSubSectionModule.StopMove(); this.WaitingUnloadSubSectionModule.BeltInit(); });
            Task task5 = Task.Factory.StartNew(() => { this.UnloadingSubSectionModule.StopMove(); this.UnloadingSubSectionModule.BeltInit(); });

            Task.WaitAll(task1, task2, task3, task4, task5);
        }
    }
}
