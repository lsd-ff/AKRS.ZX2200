using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.MagazineTeach
{
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Carrier;

    /// <summary>
    /// 新建工具窗体
    /// </summary>
    public partial class FrmNewCreateComponent : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 吸嘴
        /// </summary>
        public BaseCarrierConfig NewCarrierConfig { get; set; }

        /// <summary>
        /// 是否是静态华夫盒
        /// </summary>
        private bool isStaticWafflePack;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isStaticWafflePack">是否是静态华夫盒</param>
        public FrmNewCreateComponent(bool isStaticWafflePack = false)
        {
            InitializeComponent();
            this.isStaticWafflePack = isStaticWafflePack;
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            // 控件绑定数据源
            LueCarrierType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<CarrierTypeEnum>();
            if (this.isStaticWafflePack)
            {
                this.LueCarrierType.EditValue = CarrierTypeEnum.StaticWaffle;
            }
            else
            {
                this.LueCarrierType.EditValue = CarrierTypeEnum.Wafer;
            }
            
            LuePackType.Enabled = false;

            LuePackType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<WaffleTypeEnum>();
            LuePackType.EditValue = WaffleTypeEnum.Wafflepack;

            //List<BaseCarrierConfig> carrierList = CarrierConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().ToList();

            //carrierList.Insert(0, new CarrierWithWaferConfig() { Name = "No Pattern" });

            //this.GcWafer.DataSource = carrierList;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            BaseCarrierConfig carrierConfig = this.GvWafer.GetFocusedRow() as BaseCarrierConfig;
            
            if ((CarrierTypeEnum)this.LueCarrierType.EditValue == CarrierTypeEnum.Wafer)
            {
                if (carrierConfig == null || carrierConfig.Name == "No Pattern")
                {
                    carrierConfig = new CarrierWithWaferConfig();
                }

                this.NewCarrierConfig = JsonFormatHelper<CarrierWithWaferConfig>.DeepGenericCopy<CarrierWithWaferConfig>(carrierConfig as CarrierWithWaferConfig);
                this.NewCarrierConfig.CarrierType = CarrierTypeEnum.Wafer;
            }
            else if ((CarrierTypeEnum)this.LueCarrierType.EditValue == CarrierTypeEnum.Waffle)
            {
                if (carrierConfig == null || carrierConfig.Name == "No Pattern")
                {
                    carrierConfig = new CarrierWithWaffleConfig();
                }

                this.NewCarrierConfig = JsonFormatHelper<CarrierWithWaffleConfig>.DeepGenericCopy<CarrierWithWaffleConfig>(carrierConfig as CarrierWithWaffleConfig);
                this.NewCarrierConfig.CarrierType = CarrierTypeEnum.Waffle;

                (this.NewCarrierConfig as CarrierWithWaffleConfig).WaffleType = (WaffleTypeEnum)this.LuePackType.EditValue;
            }
            else if ((CarrierTypeEnum)this.LueCarrierType.EditValue == CarrierTypeEnum.StaticWaffle)
            {
                if (carrierConfig == null || carrierConfig.Name == "No Pattern")
                {
                    carrierConfig = new CarrierWithStaticWaffleConfig();
                }

                this.NewCarrierConfig = JsonFormatHelper<CarrierWithStaticWaffleConfig>.DeepGenericCopy<CarrierWithStaticWaffleConfig>(carrierConfig as CarrierWithStaticWaffleConfig);
                this.NewCarrierConfig.CarrierType = CarrierTypeEnum.StaticWaffle;

                (this.NewCarrierConfig as CarrierWithStaticWaffleConfig).WaffleType = (WaffleTypeEnum)this.LuePackType.EditValue;
            }

            this.NewCarrierConfig.Name = this.TxtName.Text;
            this.NewCarrierConfig.CopyName = carrierConfig.Name;
            this.NewCarrierConfig.AddBelongRecipeIds();
            CarrierConfigRepository.GetInstance().Add(this.NewCarrierConfig);
            CarrierConfigRepository.GetInstance().Save();
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
        /// Carrier 类型改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LueCarrierType_EditValueChanged(object sender, EventArgs e)
        {
            if ((CarrierTypeEnum)this.LueCarrierType.EditValue == CarrierTypeEnum.Wafer)
            {
                List<BaseCarrierConfig> carrierList =
                    CarrierConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().Where( a => a.CarrierType == CarrierTypeEnum.Wafer).ToList();

                carrierList.Insert(0, new CarrierWithWaferConfig() { Name = "No Pattern" });

                this.GcWafer.DataSource = carrierList;
                this.LuePackType.Enabled = false;
            }
            else if ((CarrierTypeEnum)this.LueCarrierType.EditValue == CarrierTypeEnum.Waffle)
            {
                List<BaseCarrierConfig> carrierList =
                    CarrierConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().Where(a => a.CarrierType == CarrierTypeEnum.Waffle).ToList();

                carrierList.Insert(0, new CarrierWithWaffleConfig() { Name = "No Pattern" });

                this.GcWafer.DataSource = carrierList;
                this.LuePackType.Enabled = true;
            }
            else if ((CarrierTypeEnum)this.LueCarrierType.EditValue == CarrierTypeEnum.StaticWaffle)
            {
                List<BaseCarrierConfig> carrierList =
                    CarrierConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().Where(a => a.CarrierType == CarrierTypeEnum.StaticWaffle).ToList();

                carrierList.Insert(0, new CarrierWithStaticWaffleConfig() { Name = "No Pattern" });

                this.GcWafer.DataSource = carrierList;
                this.LuePackType.Enabled = true;
            }
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
            this.BtnGenerate.Enabled = !this.IsExistSpecialChars(this.TxtName.Text) && !string.IsNullOrWhiteSpace(this.TxtName.Text) && !CarrierConfigRepository
                                           .GetInstance().BaseDsSettingList.Exists(a => a.Name == this.TxtName.Text);
        }

        private void FrmNewCreateComponent_FormClosing(object sender, FormClosingEventArgs e)
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