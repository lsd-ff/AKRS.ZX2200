using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Localization
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;

    [Serializable]
    public class LanguageSettings : Singleton<LanguageSettings>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static LanguageSettings()
        {
            Singleton<LanguageSettings>.FilePath = Path.Combine(Application.StartupPath, "language_settings.json");
        }

        public LanguageEnum Language { get; set; } = LanguageEnum.Chinese;
    }
}
