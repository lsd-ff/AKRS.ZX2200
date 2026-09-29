using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach
{
    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.SupportFeature.Consumables;

    /// <summary>
    /// 新建工具窗体
    /// </summary>
    public partial class FrmNewCreateEjection : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 吸嘴
        /// </summary>
        public EjectionConfig NewEjection { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNewCreateEjection()
        {
            InitializeComponent();

            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            // 控件绑定数据源
            LueToolType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<EjectionTypeEnum>();
            this.LueToolType.EditValue = EjectionTypeEnum.Standard;
            LueToolType.Enabled = false;


            List<EjectionConfig> ejectinoList = EjectionConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().ToList();

            ejectinoList.Insert(0, new EjectionConfig() { Name = "No Reference" });

            this.GcTool.DataSource = ejectinoList;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            EjectionConfig ejectino = this.GvTool.GetFocusedRow() as EjectionConfig;

            if (ejectino == null)
            {
                ejectino = new EjectionConfig();
                ejectino.Frequency = new FrequencyConsumables(this.TxtName.Text);
            }

            NewEjection = JsonFormatHelper<EjectionConfig>.DeepGenericCopy<EjectionConfig>(ejectino);
            NewEjection.Name = this.TxtName.Text;
            NewEjection.Frequency = new FrequencyConsumables(this.TxtName.Text);
            NewEjection.EjectionType = (EjectionTypeEnum)this.LueToolType.EditValue;

            NewEjection.AddBelongRecipeIds();

            EjectionConfigRepository.GetInstance().Add(NewEjection);
            
            EjectionConfigRepository.GetInstance().Save();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 判断字符串中是否存在特殊字符
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>return</returns>
        private bool IsExistSpecialChars(string str)
        {
            List<string> strList = new List<string> { "!", "@", "#", "￥", "%", "&", "*", "！", "$", "^", "（", "）", "(", ")", ".", "。", "/", "[", "]", "{", "}", "【", "】", "|", "\\", "?", "？", ",", "，", "《", "》", "<", ">" };
            foreach (var item in strList)
            {
                if (str.Contains(item))
                {
                    return true;
                }
            }
            
            return false;
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.BtnGenerate.Enabled = !this.IsExistSpecialChars(this.TxtName.Text) && !string.IsNullOrWhiteSpace(this.TxtName.Text) && !EjectionConfigRepository
                                           .GetInstance().BaseDsSettingList.Exists(a => a.Name == this.TxtName.Text);
        }

        private void FrmNewCreateEjection_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }
    }
}