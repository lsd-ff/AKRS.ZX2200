using System;
using System.IO;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Recipe;
using Newtonsoft.Json;

namespace AKRS.Galaxy2.MachineSupport.Config
{
    /// <summary>
    /// 设备的基础配置类
    /// 单例
    /// </summary>
    public class MachineConfigContext
    {
        /// <summary>
        /// 锁
        /// </summary>
        [JsonIgnore]
        private static object locker = new object();

        /// <summary>
        /// Recipe ID
        /// </summary>
        public Guid RecipeID { get; set; } = Guid.Empty;

        /// <summary>
        /// Recipe
        /// </summary>
        [JsonIgnore]
        public Recipe.Recipe CurrentRecipe => RecipeRepository.GetInstance().GetRecipeByID(this.RecipeID);

        /// <summary>
        /// RecipeName 也是产品名称
        /// </summary>
        [JsonIgnore]
        public string RecipeName => RecipeRepository.GetInstance().GetRecipeByID(this.RecipeID)?.RecipeName;

        /// <summary>
        /// 设备的基础配置实例
        /// </summary>
        private static MachineConfigContext instance;

        /// <summary>
        /// 构造函数
        /// </summary>
        private MachineConfigContext()
        {
        }

        /// <summary>
        /// 单例的获取方法
        /// </summary>
        /// <returns>单例实例</returns>
        public static MachineConfigContext GetInstance()
        {
            if (instance == null)
            {
                if (DirAndFileHelper.IsExistFile(PathConfig.MachineConfigFileName))
                {
                    try
                    {
                        instance = JsonFormatHelper<MachineConfigContext>.ReadGenericObject(PathConfig.MachineConfigFileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(@"读取MachineConfig.json出错：" + ex.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    instance = new MachineConfigContext();
                }
            }

            return instance;
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void SaveMachineConfig()
        {
            JsonFormatHelper<MachineConfigContext>.SaveGenericObject(this, PathConfig.MachineConfigFileName);
        }
    }
}
