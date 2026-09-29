using System.Collections.Generic;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using DevExpress.Office.Utils;
using Newtonsoft.Json;

namespace AKRS.ZX2200.DispenseSystem.Models.Programs
{
    /// <summary>
    /// 点胶图形参数
    /// </summary>
    public class EpoxyApplicationProgram
    {
        /// <summary>
        /// 点胶参数的名称
        /// </summary>
        public List<string> EpoxyApplicationNames { get; set; } = new List<string>();   

        /// <summary>
        /// 点胶图形
        /// </summary>
        [JsonIgnore]
        public List<EpoxyApplication> EpoxyApplications => this.Find(this.EpoxyApplicationNames);

        /// <summary>
        /// 根据名称是寻找对应的图形
        /// </summary>
        /// <param name="epoxyApplicationNames">图形名称</param>
        /// <returns>图形集</returns>
        private List<EpoxyApplication> Find(List<string> epoxyApplicationNames)
        {
            List<EpoxyApplication> epoxyApplications = new List<EpoxyApplication>();

            if (epoxyApplicationNames != null)
            {
                for (int i = 0; i < epoxyApplicationNames.Count; i++)
                {
                    EpoxyApplication epoxyApplication =
                        (EpoxyApplication)EpoxyApplicationRepository.GetInstance().Find(epoxyApplicationNames[i]);
                    if (epoxyApplication != null)
                    {
                        epoxyApplications.Add(epoxyApplication);
                    }
                    else
                    {
                        // 如果没有找到，说明库里面已经不存在了，直接删除
                        epoxyApplicationNames.RemoveAt(i);
                    }
                }
            }

            return epoxyApplications;
        }
    }
}
