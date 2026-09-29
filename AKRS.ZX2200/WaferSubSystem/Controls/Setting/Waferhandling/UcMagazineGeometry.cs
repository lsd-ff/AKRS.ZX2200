using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.Waferhandling
{
    using AKRS.ZX2200.WaferSubSystem.Services;
    using System.Threading;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// MagazineBox硬件编辑
    /// </summary>
    public partial class UcMagazineGeometry : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 刷新magazineBox数据委托
        /// </summary>
        public static Action RefreshMagazineBoxDataAction;

        /// <summary>
        /// 上晶圆程式
        /// </summary>
        private WaferSystemDomain WaferSystemDomain => WaferSystemDomain.GetInstance() ?? new WaferSystemDomain();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcMagazineGeometry()
        {
            this.InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= this.Timer1_Tick;
                    this.timer1.Dispose();
                };

            RefreshMagazineBoxDataAction += this.RefreshMagazineBoxData;
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.RefreshCmbItems();
            this.RefreshMagazineBoxData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshMagazineBoxData()
        {
            if (!MagazineBoxConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == WaferSystemProgram.GetInstance().MagazineProgram.Name))
            {
                WaferSystemProgram.GetInstance().MagazineProgram.Name = string.Empty;
                WaferSystemProgram.GetInstance().Save();
                Thread.Sleep(100);
            }
            else
            {
                this.RgPushStatus.SelectedIndex = this.PushStatusToInt(WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Push);
                this.RgDirection.SelectedIndex = (int)WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Direction;
                this.SpOffset.EditValue = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Offset;
                if (!MagazineBoxGeoConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeoName))
                {
                    WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeoName = string.Empty;
                    MagazineBoxGeoConfigRepository.GetInstance().Save();
                    Thread.Sleep(100);

                    this.CmbMagazineBoxGeoRepository.Text = string.Empty;
                }
                else
                {
                    this.CmbMagazineBoxGeoRepository.Text = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeoName;

                    this.SpSlotPitch.Text = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.SlotPitch.ToString();
                    this.SpPositionOfLowestSlot.Text = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.PositionOfLowestSlot.ToString();
                    this.TbLayerCount.Value = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.LayerCount;
                    this.RgWaferSize.SelectedIndex = (int)WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize;
                    this.SpMagazineSizeX.Text = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.MagazineSize.X.ToString();
                    this.SpMagazineSizeY.Text = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.MagazineSize.Y.ToString();
                    this.SpMagazineSizeZ.Text = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.MagazineSize.Z.ToString();
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshMagazineBoxGeoData()
        {
            string name = this.CmbMagazineBoxGeoRepository.Text;
            if (name != string.Empty)
            {
                MagazineBoxGeoConfig temp = MagazineBoxGeoConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == name);
                this.SpSlotPitch.Text = temp.SlotPitch.ToString();
                this.SpPositionOfLowestSlot.Text = temp.PositionOfLowestSlot.ToString();
                this.TbLayerCount.Value = temp.LayerCount;
                this.RgWaferSize.SelectedIndex = (int)temp.WaferSize;
                this.SpMagazineSizeX.Text = temp.MagazineSize.X.ToString();
                this.SpMagazineSizeY.Text = temp.MagazineSize.Y.ToString();
                this.SpMagazineSizeZ.Text = temp.MagazineSize.Z.ToString();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveData()
        {
            WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeoName = this.CmbMagazineBoxGeoRepository.Text;
            MagazineBoxConfigRepository.GetInstance().Save();

            WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Push = this.PushStatus();
            WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Direction = (MagazineDirectionEnum)this.RgDirection.SelectedIndex;
            WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.Offset = (double)this.SpOffset.Value;
            WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeoName = this.CmbMagazineBoxGeoRepository.Text;

            if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeoName != string.Empty)
            {
                WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.SlotPitch = (double)this.SpSlotPitch.Value;
                WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.PositionOfLowestSlot = (double)this.SpPositionOfLowestSlot.Value;
                WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.LayerCount = this.TbLayerCount.Value;
                WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize = (WaferSizeEnum)this.RgWaferSize.SelectedIndex;
                WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.MagazineSize = new AKRSPoint3D()
                                                                      {
                                                                          X = (double)this.SpMagazineSizeX.Value,
                                                                          Y = (double)this.SpMagazineSizeY.Value,
                                                                          Z = (double)this.SpMagazineSizeZ.Value
                                                                      };
            }

            MagazineBoxConfigRepository.GetInstance().Save();
            MagazineBoxGeoConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbMagazineBoxGeoRepository_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<MagazineBoxGeoConfig> frmRepository = new FrmRepository<MagazineBoxGeoConfig>(MagazineBoxGeoConfigRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    MagazineBoxGeoConfig temp = (MagazineBoxGeoConfig)frmRepository.DsSetting;

                    if (temp != null)
                    {
                        ((ComboBoxEdit)sender).Text = temp.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbMagazineBoxGeoRepository_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshMagazineBoxGeoData();
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            this.CmbMagazineBoxGeoRepository.Properties.Items.Clear();
            List<string> nameList = MagazineBoxGeoConfigRepository.GetInstance().Filter(string.Empty, false).Select(a => a.Name).ToList();
            this.CmbMagazineBoxGeoRepository.Properties.Items.AddRange(nameList);
            if (nameList.Count == 0)
            {
                this.CmbMagazineBoxGeoRepository.Text = string.Empty;
            }
        }

        /// <summary>
        /// 层数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void SpLayerCount_EditValueChanged(object sender, EventArgs e)
        {
            this.TbLayerCount.EditValue = this.SpLayerCount.EditValue;
        }

        /// <summary>
        /// 层数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void TbLayerCount_EditValueChanged(object sender, EventArgs e)
        {
            this.SpLayerCount.EditValue = this.TbLayerCount.EditValue;
        }

        /// <summary>
        /// 推杆
        /// </summary>
        /// <returns>推杆状态</returns>
        private bool PushStatus()
        {
            if (this.RgPushStatus.SelectedIndex == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 推杆
        /// </summary>
        /// <param name="isPush">是否使用推杆</param>
        /// <returns>推杆状态</returns>
        private int PushStatusToInt(bool isPush)
        {
            if (isPush)
            {
                return 0;
            }

            return 1;
        }

        /// <summary>
        /// 置True
        /// </summary>
        private void SetEnabledWithTrue()
        {
            this.GcWaferSize.Enabled = true;
            this.GcNumberOfSlots.Enabled = true;
            this.GcSlotPitch.Enabled = true;
            this.GcPositionOfLowestSlot.Enabled = true;
            this.GcMagazineBoxSize.Enabled = true;
        }

        /// <summary>
        /// 置False
        /// </summary>
        private void SetEnabledWithFalse()
        {
            this.GcWaferSize.Enabled = false;
            this.GcNumberOfSlots.Enabled = false;
            this.GcSlotPitch.Enabled = false;
            this.GcPositionOfLowestSlot.Enabled = false;
            this.GcMagazineBoxSize.Enabled = false;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            string name = this.CmbMagazineBoxGeoRepository.Text;
            if (name != string.Empty)
            {
                this.SetEnabledWithTrue();
            }
            else
            {
                this.SetEnabledWithFalse();
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            this.SaveData();
        }
    }
}
