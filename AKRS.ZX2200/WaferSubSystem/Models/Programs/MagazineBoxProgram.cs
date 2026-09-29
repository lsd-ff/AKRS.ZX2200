using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Programs
{
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;

    /// <summary>
    /// 描述：料架程式
    /// </summary>
    public class MagazineBoxProgram
    {
        /// <summary>
        /// 当前Recipe 使用的料架名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 当前使用的料架硬件配置
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("MagazineBoxSetting", "MagazineBoxProgram")]
        public MagazineBoxConfig MagazineBoxConfig => MagazineBoxConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.Name);
    }
}
