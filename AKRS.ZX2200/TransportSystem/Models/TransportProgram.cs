using System;
using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.Product;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportUnitSystem;

namespace AKRS.ZX2200.TransportSystem.Models
{
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 逻辑流道（流道程式）
    /// </summary>
    public class TransportProgram : Singleton<TransportProgram>
    {
        #region 程式

        /// <summary>
        /// 上料载台
        /// </summary>
        public LoadingSubSectionProgram LoadingSubSectionProgram = new LoadingSubSectionProgram();

        /// <summary>
        /// 点胶载台
        /// </summary>
        public DispenseSubSectionProgram DispenseSubSectionProgram = new DispenseSubSectionProgram();

        /// <summary>
        /// Bond载台
        /// </summary>
        public BondSubSectionProgram BondSubSectionProgram = new BondSubSectionProgram();

        /// <summary>
        /// 等待下料载台
        /// </summary>
        public WaitingUnloadSubSectionProgram WaitingUnloadSubSectionProgram = new WaitingUnloadSubSectionProgram();

        /// <summary>
        /// 下料载台
        /// </summary>
       public UnloadingSubSectionProgram UnloadingSubSectionProgram = new UnloadingSubSectionProgram();

        /// <summary>
        /// 上料仓
        /// </summary>
        public LoaderBinProgram LoaderBinProgram = new LoaderBinProgram();

        /// <summary>
        /// 下料仓
        /// </summary>
        public UnLoaderBinProgram UnLoaderBinProgram = new UnLoaderBinProgram();

        #endregion
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static TransportProgram()
        {
            FilePath = ZX2200PathConfig.TransportSystemProgramFilePath;
        }

        /// <summary>
        /// 初始化ProductConfiguration
        /// </summary>
        public void InitTransportSystemProgram()
        {
            FilePath = ZX2200PathConfig.TransportSystemProgramFilePath;
            instance = null;
        }

        /// <summary>
        /// 流道示教状态
        /// </summary>
        public AssistantState LoaderAssistantState { get; set; } = new AssistantState() { Name = "Loader" };

        /// <summary>
        /// 流道示教状态
        /// </summary>
        public AssistantState UnLoaderAssistantState { get; set; } = new AssistantState() { Name = "UnLoader" };

        /// <summary>
        /// 刷新TransportUnit
        /// </summary>
        public void RefreshTransportUnit()
        {
            if (this.DispenseSubSectionProgram.TransportUnit != null)
            {
                this.DispenseSubSectionProgram.TransportUnit.Refresh();
            }

            if (this.BondSubSectionProgram.TransportUnit != null)
            {
                this.BondSubSectionProgram.TransportUnit.Refresh();
            }

            if (this.WaitingUnloadSubSectionProgram.TransportUnit != null)
            {
                this.WaitingUnloadSubSectionProgram.TransportUnit.Refresh();
            }
        }

        /// <summary>
        /// 所有流道程式列表
        /// </summary>
        /// <returns>流道程式列表</returns>
        public List<BaseSubSectionProgram> GetSubSectionPrograms()
        {
            return new List<BaseSubSectionProgram>()
                       {
                           this.LoadingSubSectionProgram,
                           this.DispenseSubSectionProgram,
                           this.BondSubSectionProgram,
                           this.WaitingUnloadSubSectionProgram,
                           this.UnloadingSubSectionProgram
                       };
        }

        /// <summary>
        /// 获取上料仓的示教状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetLoaderAssistantStates()
        {
            return new List<AssistantState>() { this.LoaderAssistantState };
        }

        /// <summary>
        /// 获取下料仓的示教状态
        /// </summary>
        /// <returns>return</returns>
        public List<AssistantState> GetUnLoaderAssistantStates()
        {
            return new List<AssistantState>() { this.UnLoaderAssistantState };
        }

        /// <summary>
        /// 是否有程式为空
        /// </summary>
        /// <returns>结果</returns>
        public bool IsSettingNull()
        {
            // 是否存在为空的程式
            if (this.LoadingSubSectionProgram.IsSettingNull())
            {
                return false;
            }

            // 是否存在为空的程式
            if (this.DispenseSubSectionProgram.IsSettingNull())
            {
                return false;
            }

            // 是否存在为空的程式
            if (this.BondSubSectionProgram.IsSettingNull())
            {
                return false;
            }

            // 是否存在为空的程式
            if (this.WaitingUnloadSubSectionProgram.IsSettingNull())
            {
                return false;
            }

            // 是否存在为空的程式
            if (this.UnloadingSubSectionProgram.IsSettingNull())
            {
                return false;
            }

            return true;
        }
    }
}
