using System;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet
{
    /// <summary>
    /// 空料片
    /// </summary>
    [Serializable]
    public class NullTablet : BaseTablet
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NullTablet()
        {
            this.TabletType = TabletTypeEnum.Null;
            this.Name = string.Empty;
        }
    }
}
