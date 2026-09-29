using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.DispenseSystem.Models.Programs;

namespace AKRS.ZX2200.DispenseSystem.Models
{
    using System.Collections.Generic;

    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using DevExpress.CodeParser;

    /// <summary>
    /// 点胶程式
    /// </summary>
    public class System1Program : Singleton<System1Program>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static System1Program()
        {
            System1Program.FilePath = ZX2200PathConfig.DispenseProgramPath;
        }

        /// <summary>
        /// 点胶头
        /// </summary>
        public DispenserProgram DispenserProgram { get; set; } = new DispenserProgram();

        /// <summary>
        /// 胶水
        /// </summary>
        public EpoxyMaterialProgram EpoxyMaterialProgram { get; set; } = new EpoxyMaterialProgram();

        /// <summary>
        /// 点胶图形
        /// </summary>
        public EpoxyApplicationProgram EpoxyApplicationProgram { get; set; } = new EpoxyApplicationProgram();

        /// <summary>
        /// 预点胶板
        /// </summary>
        public PreDispensePlateProgram PreDispensePlateProgram { get; set; } = new PreDispensePlateProgram();

        /// <summary>
        /// 其他程式
        /// </summary>
        public System1OtherProgram System1OtherProgram { get; set; } = new System1OtherProgram();

        /// <summary>
        /// 初始化点胶程式
        /// </summary>
        public void InitSystem1Program()
        {
            System1Program.FilePath = ZX2200PathConfig.DispenseProgramPath;
            instance = null;
        }

        /// <summary>
        /// 获取耗材的集合
        /// </summary>
        /// <returns>结果</returns>
        public List<TimeConsumable> GetEpoxyConsumable()
        {
            List<TimeConsumable> list = new List<TimeConsumable>();

            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                return list;
            }
            
            if (this.EpoxyMaterialProgram.EpoxyMaterial != null)
            {
                if (this.EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable == null)
                {
                    this.EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable = new TimeConsumable();
                }

                this.EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable.Name = this.EpoxyMaterialProgram.EpoxyMaterial.Name;
                list.Add(this.EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable);
            }

            return list;
        }
    }
}
