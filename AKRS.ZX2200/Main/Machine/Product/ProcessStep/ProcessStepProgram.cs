namespace AKRS.ZX2200.Main.Machine.Product.ProcessStep
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// ProcessStep程式
    /// </summary>
    public class ProcessStepProgram : Singleton<ProcessStepProgram>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ProcessStepProgram()
        {
            Singleton<ProcessStepProgram>.FilePath = ZX2200PathConfig.ProcessStepProgramFilePath;
        }

        /// <summary>
        /// 当前配方的ProcessStepList
        /// </summary>
        public List<Product.ProcessStep.ProcessStep> ProcessStepList = new List<Product.ProcessStep.ProcessStep>();

        /// <summary>
        /// 系统1排序方式
        /// </summary>
        public ProcessingStrategyEnum S1ProcessingStrategy { get; set; }

        /// <summary>
        /// 系统2排序方式
        /// </summary>
        public ProcessingStrategyEnum S2ProcessingStrategy { get; set; }

        /// <summary>
        /// 系统1动作节点排序模式
        /// </summary>
        public ActionNodesSortModeEnum ActionNodesSortModeInS1 { get; set; } = ActionNodesSortModeEnum.Normal;

        /// <summary>
        /// 系统2动作节点排序模式
        /// </summary>
        public ActionNodesSortModeEnum ActionNodesSortModeInS2 { get; set; } = ActionNodesSortModeEnum.Normal;

        /// <summary>
        /// 获取所有 step 名称
        /// </summary>
        /// <returns>step Name 集合</returns>
        public List<string> GetAllProcessStepNames()
        {
            return ProcessStepProgram.GetInstance().ProcessStepList.Select(a => a.Name).ToList();
        }

        /// <summary>
        /// 获取System1 step 名称
        /// </summary>
        /// <returns>step Name 集合</returns>
        public List<string> GetS1ProcessStepNames()
        {
            // 名称集合
            List<string> stepList = new List<string>();


            foreach (var item in this.ProcessStepList)
            {
                if (item.IsEnableInSystem1)
                {
                    stepList.Add(item.Name);
                }
            }

            return stepList;
        }

        /// <summary>
        /// 获取System2 step 名称
        /// </summary>
        /// <returns>step Name 集合</returns>
        public List<string> GetS2ProcessStepNames()
        {
            // 名称集合
            List<string> stepList = new List<string>();

            if (this.ProcessStepList.Count == 0)
            {
                MessageBox.Show("错误");
            }

            foreach (var item in this.ProcessStepList)
            {
                if (item.IsEnableInSystem2)
                {
                    stepList.Add(item.Name);
                }
            }

            return stepList;
        }

        /// <summary>
        /// 获取System2 step 
        /// </summary>
        /// <returns>step Name 集合</returns>
        public List<Product.ProcessStep.ProcessStep> GetS1ProcessStep()
        {
            // 名称集合
            List<Product.ProcessStep.ProcessStep> stepList = new List<Product.ProcessStep.ProcessStep>();

            foreach (var item in this.ProcessStepList)
            {
                if (item.IsEnableInSystem1)
                {
                    stepList.Add(item);
                }
            }

            return stepList;
        }

        /// <summary>
        /// 获取System2 要做的焊点名称
        /// </summary>
        /// <returns> Name 集合</returns>
        public List<string> GetS2BondPositionNames()
        {
            // 名称集合
            List<string> nameList = new List<string>();

            foreach (var item in this.GetS2ProcessStep())
            {
                if (item.IsEnableInSystem2)
                {
                    nameList.Add(item.BondPositionName);
                }
            }

            return nameList;
        }

        /// <summary>
        /// 获取System1 要做的焊点名称
        /// </summary>
        /// <returns> Name 集合</returns>
        public List<string> GetS1BondPositionNames()
        {
            // 名称集合
            List<string> nameList = new List<string>();

            foreach (var item in this.GetS1ProcessStep())
            {
                if (item.IsEnableInSystem1)
                {
                    nameList.Add(item.BondPositionName);
                }
            }

            return nameList;
        }

        /// <summary>
        /// 获取System2 step 
        /// </summary>
        /// <returns>step Name 集合</returns>
        public List<Product.ProcessStep.ProcessStep> GetS2ProcessStep()
        {
            // 名称集合
            List<Product.ProcessStep.ProcessStep> stepList = new List<Product.ProcessStep.ProcessStep>();

            foreach (var item in this.ProcessStepList)
            {
                if (item.IsEnableInSystem2)
                {
                    stepList.Add(item);
                }
            }

            return stepList;
        }

        /// <summary>
        /// 获取最大序号
        /// </summary>
        /// <returns>最大序号</returns>
        public int GetMaxOrder()
        {
            if (ProcessStepProgram.GetInstance().ProcessStepList.Any())
            {
                return ProcessStepProgram.GetInstance().ProcessStepList.Max(a => a.Order);
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="processStep">对象</param>
        public void Add(Product.ProcessStep.ProcessStep processStep)
        {
            this.ProcessStepList.Add(processStep);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="processStep">对象</param>
        public void Remove(Product.ProcessStep.ProcessStep processStep)
        {
            this.ProcessStepList.Remove(processStep);
        }

        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="name">数据配置实体名称</param>
        /// <returns>数据配置实体</returns>
        public Product.ProcessStep.ProcessStep Find(string name)
        {
            Product.ProcessStep.ProcessStep processStep = this.ProcessStepList.Find(bi => bi.Name == name);

            return processStep;
        }

        /// <summary>
        /// 初始化ProcessStep程式
        /// </summary>
        public void InitProcessStepProgram()
        {
            Singleton<ProcessStepProgram>.FilePath = ZX2200PathConfig.ProcessStepProgramFilePath;
            instance = null;
        }
    }
}
