namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram
{
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;

    using DevExpress.XtraTreeList.Nodes;

    /// <summary>
	/// UcEditProductProgramming
	/// </summary>
	public partial class UcEditProductProgramming
    {
        /// <summary>
        /// SetComponentStateImage
        /// </summary>
        /// <param name="carrierConfig">carrierConfig</param>
        /// <param name="node">node</param>
        private void SetComponentStateImage(BaseCarrierConfig carrierConfig, TreeListNode node)
        {
            if (carrierConfig.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetComponentStateImage
        /// </summary>
        /// <param name="ejectionBankSlotConfig">ejectionBankSlotConfig</param>
        /// <param name="node">node</param>
        private void SetEjectStateImage(EjectionBankSlotConfig ejectionBankSlotConfig, TreeListNode node)
        {
            if (ejectionBankSlotConfig.EjectionConfig.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetMagazineBoxStateImage
        /// </summary>
        /// <param name="magazineBoxConfig">magazineBoxConfig</param>
        /// <param name="node">node</param>
        private void SetMagazineBoxStateImage(MagazineBoxConfig magazineBoxConfig, TreeListNode node)
        {
            if (magazineBoxConfig.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetTransportUnitStateImage
        /// </summary>
        /// <param name="transportUnitConfig">transportUnitConfig</param>
        /// <param name="node">node</param>
        private void SetTransportUnitStateImage(TransportUnitConfig transportUnitConfig, TreeListNode node)
        {
            if (transportUnitConfig.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetDispenserStateImage
        /// </summary>
        /// <param name="dispenser">dispenser</param>
        /// <param name="node">node</param>
        private void SetDispenserStateImage(Dispenser dispenser, TreeListNode node)
        {
            if (dispenser == null)
            {
                return;
            }

            if (dispenser.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// 设置翻转工具节点图片
        /// </summary>
        /// <param name="flipTool">翻转工具</param>
        /// <param name="node">节点</param>
        private void SetFlipToolStateImage(FlipTool flipTool, TreeListNode node)
        {
            if (flipTool.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }


        /// <summary>
        /// SetNozzleStateImage
        /// </summary>
        /// <param name="nozzleShelfSlot">nozzleShelfSlot</param>
        /// <param name="node">node</param>
        private void SetNozzleStateImage(NozzleShelfSlot nozzleShelfSlot, TreeListNode node)
        {
            if (nozzleShelfSlot.Nozzle.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetPostBondStateImage
        /// </summary>
        /// <param name="postBondInspection">postBondInspection</param>
        /// <param name="node">node</param>
        private void SetPostBondStateImage(PostBondInspection postBondInspection, TreeListNode node)
        {
            if (postBondInspection.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetBondPositionStateImage
        /// </summary>
        /// <param name="singleBondPositionConfig">singleBondPositionConfig</param>
        /// <param name="node">node</param>
        private void SetBondPositionStateImage(SingleBondPositionConfig singleBondPositionConfig, TreeListNode node)
        {
            if (singleBondPositionConfig.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetLoaderStateImage
        /// </summary>
        /// <param name="LoaderBin">LoaderBin</param>
        /// <param name="node">node</param>
        private void SetLoaderStateImage(LoaderBin loader, TreeListNode node)
        {
            if (loader.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }

        /// <summary>
        /// SetLoaderStateImage
        /// </summary>
        /// <param name="UnloaderBin">UnloaderBin</param>
        /// <param name="node">node</param>
        private void SetUnLoaderStateImage(UnloaderBin unLoader, TreeListNode node)
        {
            if (unLoader.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }
    }
}
