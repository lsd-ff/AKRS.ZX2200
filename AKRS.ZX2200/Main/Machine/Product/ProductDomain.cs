namespace AKRS.ZX2200.Main.Machine.Product
{
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using DevExpress.XtraEditors;

    using Newtonsoft.Json;

    /// <summary>
    /// 产品域，用来存储产品相关的参数和类
    /// </summary>
    public class ProductDomain : Singleton<ProductDomain>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ProductDomain()
        {
            FilePath = ZX2200PathConfig.ProductDomainFilePath;
        }

        /// <summary>
        /// 产品配置
        /// </summary>
        [JsonIgnore]
        public ProductConfiguration ProductConfig => ProductConfiguration.GetInstance();

        ///// <summary>
        ///// ProcessStep程式
        ///// </summary>
        //[JsonIgnore]
        //public ProcessStepProgram ProcessStepProgram => ProcessStepProgram.GetInstance();
    }
}
