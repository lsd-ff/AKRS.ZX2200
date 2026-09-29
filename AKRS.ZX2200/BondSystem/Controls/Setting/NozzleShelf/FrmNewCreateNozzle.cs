using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.Models.Enums;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.NozzleShelf
{
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using System.IO;

    /// <summary>
    /// 新建工具窗体
    /// </summary>
    public partial class FrmNewCreateNozzle : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 吸嘴
        /// </summary>
        public Nozzle NewNozzle { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNewCreateNozzle()
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
            LueToolShape.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<NozzleShapeEnum>();
            this.LueToolShape.EditValue = NozzleShapeEnum.Round;

            LueToolSize.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<NozzleSizeEnum>();
            LueToolSize.EditValue = NozzleSizeEnum.Small;

            List<Nozzle> nozeList = NozzleRepository.GetInstance().GetDataSourceByCurrentRecipe().ToList();

            nozeList.Insert(0, new Nozzle() { Name = "No Reference" });

            this.GcTool.DataSource = nozeList;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TxtName.Text))
            {
                return;
            }

            if (this.LueToolSize.EditValue == null)
            {
                return;
            }

            Nozzle nozzle = this.GvTool.GetFocusedRow() as Nozzle;

            if (nozzle == null || nozzle.Name == "No Reference") 
            {
                nozzle = new Nozzle(this.TxtName.Text);
            }
            
            NewNozzle = JsonFormatHelper<Nozzle>.DeepGenericCopy<Nozzle>(nozzle);
            NewNozzle.Name = this.TxtName.Text;
            NewNozzle.NozzleSize = (NozzleSizeEnum)this.LueToolSize.EditValue;
            NewNozzle.NozzleShape = (NozzleShapeEnum)this.LueToolShape.EditValue;
            NewNozzle.CreateTime = DateTime.Now;

            NewNozzle.AddBelongRecipeIds();

            NozzleRepository.GetInstance().Add(NewNozzle);
            
            NozzleRepository.GetInstance().Save();

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
        /// 输入文本改变事件(防呆)
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TxtName_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtName.Text))
            {
                BtnGenerate.Enabled = false;
                return;
            }

            //if (this.TxtName.Text.Length > 15)
            //{
            //    BtnGenerate.Enabled = false;
            //    return;
            //}

            List<Nozzle> nozeList = NozzleRepository.GetInstance().BaseDsSettingList;

            foreach (Nozzle item in nozeList)
            {
                if (this.TxtName.Text == item.Name) 
                {
                    BtnGenerate.Enabled = false;
                    return;
                }
            }

            if (this.TxtName.Text == "BMC" || this.TxtName.Text == "TouchDown")
            {
                BtnGenerate.Enabled = false;
                return;
            }

            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                if (this.TxtName.Text.Contains(rInvalidChar.ToString()))
                {
                    BtnGenerate.Enabled = false;
                    return;
                }
            }

            BtnGenerate.Enabled = true;
        }
    }
}