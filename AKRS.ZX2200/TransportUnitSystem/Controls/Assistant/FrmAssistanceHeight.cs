using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;

    /// <summary>
    /// 示教高度
    /// </summary>
    public partial class FrmAssistanceHeight : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 测高
        /// </summary>
        /// <param name="coordinate">坐标系</param>
        /// <param name="name">名称</param>
        public FrmAssistanceHeight(GeneralCoordinateSystem coordinate, string name)
        {
            this.InitializeComponent();
            this.coordinate = coordinate;
            this.matterName = name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        private string matterName;

        /// <summary>
        /// 传输坐标系
        /// </summary>
        private GeneralCoordinateSystem coordinate;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 测高类型
        /// </summary>
        private MultipleHeightMeasurementType type;

        /// <summary>
        /// 提示信息
        /// </summary>
        private string message1;

        /// <summary>
        /// 高度
        /// </summary>
        public double MatterHeight { get; set; } = 0;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            if (this.type == MultipleHeightMeasurementType.TouchDown)
            {
                this.message1 = $"Teach-in height measurement {this.matterName}\r\n"
                                + $"Position the touchdown tool approx 5 mm above {this.matterName}\r\n"
                                + $"to teach-in the XY position for the height measurement";
            }
            else
            {
                this.message1 = $"Teach-in height measurement {this.matterName}\r\n"
                                + $"Move to the point with the substrate camera\r\n"
                                + $"Where the height measurement will be carried out";
            }

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 移动到安全位置

                    // 拉原点1
                    new AssistantConfig(
                        index: 0,
                        descritpion: this.message1,
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                        },
                       doneAction: () =>
                       {
                           (ExcuteResult result, double height) =
                               TUAssistantHelper.AssistantMeasureHeight(this.type);

                           if (result == ExcuteResult.Success)
                           {
                               if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                               {
                                   this.MatterHeight = this.coordinate
                                       .G0PosToSelf(new AKRSPoint3D(0, 0, height)).Z;
                               }

                               this.DialogResult = DialogResult.OK;
                           }
                       }),
                };

            ucGuideMove = new UcGuideMove("FrmAssistanceHeight");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl(0);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
           // this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAssistanceHeight_Load(object sender, EventArgs e)
        {
            // 如果没有开启全部测高则返回
            if (!ProductConfiguration.GetInstance().TransportUnitConfig.AssistantAllHeight)
            {
                this.DialogResult = DialogResult.OK;
                return;
            }

            this.Text = this.matterName + " " + " Measure Height";

            // 选择测高的方式
            FrmChooseMeasureHeightType frmChooseMeasureHeightType = new FrmChooseMeasureHeightType();
            frmChooseMeasureHeightType.ShowDialog();

            this.type = frmChooseMeasureHeightType.Type;

            this.InitControl();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            this.assistantConfigList[0].DoneAction();
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.AutoFocus(sender, this);
        }

        /// <summary>
        /// 编辑视觉模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditPr_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPrInSystem2("框架示教找中心模板");
        }

        /// <summary>
        /// 移动到中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void MoveToCenter_Click(object sender, EventArgs e)
        {
            (bool success, AKRSPoint3D point) result = TUAssistantHelper.AssistantPR("框架示教找中心模板");

            if (result.success)
            {
                TUAssistantHelper.MoveToPos(result.point - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);
            }
            else
            {
                AKRSXtraMessageBox.Show("定位失败，请重置制作模板");
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAssistanceHeight_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }
    }
}