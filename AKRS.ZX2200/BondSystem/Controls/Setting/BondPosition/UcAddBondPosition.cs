using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.BondPosition
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using DevExpress.ExpressApp.Utils;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using System.IO;
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 新增焊点界面
    /// </summary>
    public partial class UcAddBondPosition : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refreshAction">刷新父节点委托</param>
        public UcAddBondPosition(Action refreshAction)
        {
            this.refreshparentTreeAction = refreshAction;
            this.InitializeComponent();
            this.GvBondPosition.CellValueChanged += this.GvBondPosition_CellValueChanged;
            this.GvBondPosition.RowCellClick += this.GvBondPosition_RowCellClick;
            this.RefreshControl();
            if (ProductDomain.GetInstance().ProductConfig.TransportUnitConfig.IsOppositeSex)
            {
                this.Enabled = false;
            }
        }

        /// <summary>
        /// 焊点配置集
        /// </summary>
        private List<SingleBondPositionConfig> bondPositionConfigList;

        /// <summary>
        /// 刷新父节点委托
        /// </summary>
        private Action refreshparentTreeAction;

        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshControl()
        {
            this.bondPositionConfigList = ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList;
            this.GcBondPosition.DataSource = this.bondPositionConfigList;
            this.GcBondPosition.Refresh();
            this.GvBondPosition.RefreshData();

            this.refreshparentTreeAction();
        }

        /// <summary>
        /// 新增焊点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string defaultName = "焊点" + (this.bondPositionConfigList.Length() + 1).ToString();

        ReNameLabel:

            string createName = XtraInputBox.Show(
                "请输入新焊点的名称！",
                "焊点名称",
                defaultName);

            if (string.IsNullOrEmpty(createName))
            {
                DialogResult ret = AKRSXtraMessageBox.Show(
                    "名称不能为空!\r\n请再次输入!",
                    "Warn",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (ret == DialogResult.OK)
                {
                    goto ReNameLabel;
                }
                else
                {
                    return;
                }
            }

            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                if (createName.Contains(rInvalidChar.ToString()))
                {
                    AKRSXtraMessageBox.Show($"焊点名存在非法字符: {rInvalidChar.ToString()}！", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
            }

            if (createName.Length > 15)
            {
                DialogResult ret = AKRSXtraMessageBox.Show(
                    "焊点名称长度超过限制!\r\n请重新输入!",
                    "Warn",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (ret == DialogResult.OK)
                {
                    defaultName = createName;
                    goto ReNameLabel;
                }
                else
                {
                    return;
                }
            }

            // 获取焊点名集合
            List<string> nameList = new List<string>();

            foreach (var bondPositionConfig in this.bondPositionConfigList)
            {
                nameList.Add(bondPositionConfig.Name);
            }

            // 检查名称是否冲突
            if (nameList.Contains(createName))
            {
                DialogResult ret = AKRSXtraMessageBox.Show(
                    "名称与已有焊点名称重复!",
                    "Warn",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (ret == DialogResult.OK)
                {
                    goto ReNameLabel;
                }
                else
                {
                    return;
                }
            }

            SingleBondPositionConfig singleBondPositionConfig = new SingleBondPositionConfig() { Name = createName };

            if (!MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                singleBondPositionConfig.MeasureHeightInSystem2 = true;
                singleBondPositionConfig.BondPositionMeasureHeight.State = Infrastructure.Models.Enums.AssistantStateEnum.Able;
                singleBondPositionConfig.RefreshMeasureHeightPoint();
            }

            // 添加焊点配置
            bondPositionConfigList.Add(singleBondPositionConfig);

            // 保存数据
            ProductConfiguration.GetInstance().Save();

            this.RefreshControl();
        }

        /// <summary>
        /// 删除选中的焊点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // 获取当前选中的焊点配置
            SingleBondPositionConfig bondPositionConfig = (SingleBondPositionConfig)this.GvBondPosition.GetFocusedRow();

            // 检查是否为空
            if (bondPositionConfig == null)
            {
                AKRSXtraMessageBox.Show($"请先选中一个焊点！", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult res = AKRSXtraMessageBox.Show($"是否确定移除此焊点：{bondPositionConfig.Name}?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (res == DialogResult.OK)
            {
                // 删除焊点配置
                this.bondPositionConfigList.Remove(bondPositionConfig);

                // 保存数据
                ProductConfiguration.GetInstance().Save();
            }

            if (TransportDomain.GetInstance().TransportController.IsExistTu)
            {
                TransportDomain.GetInstance().TransportController.InitializeTS();
            }

            this.RefreshControl();
        }

        /// <summary>
        /// 复制选中的焊点,这里新建的是焊点配置类，不是焊点对象，不允许复制！
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            string defaultName = "焊点" + (this.bondPositionConfigList.Length() + 1).ToString();

            // 获取当前选中的焊点配置
            SingleBondPositionConfig bondPositionConfig = (SingleBondPositionConfig)this.GvBondPosition.GetFocusedRow();

            // 检查是否为空
            if (bondPositionConfig == null)
            {
                AKRSXtraMessageBox.Show(
                    $"请先选择需要复制的",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

        ReNameLabel:
            string createName = XtraInputBox.Show(
                "Please input the new name",
                "Bonding position name",
                defaultName);

            // 获取焊点名集合
            List<string> nameList = new List<string>();

            foreach (var item in this.bondPositionConfigList)
            {
                nameList.Add(item.Name);
            }

            // 若名称不冲突就新建焊点
            if (nameList.Contains(createName))
            {
                AKRSXtraMessageBox.Show("Name conflict! Please  rename bonding position!");
                goto ReNameLabel;
            }

            // 深拷贝
            SingleBondPositionConfig newSingleBondPositionConfig = JsonFormatHelper<SingleBondPositionConfig>.DeepGenericCopy<SingleBondPositionConfig>(bondPositionConfig);

            // 添加焊点配置
            bondPositionConfigList.Add(newSingleBondPositionConfig);

            // 保存数据
            ProductConfiguration.GetInstance().Save();

            this.RefreshControl();
        }

        /// <summary>
        /// 焊点数据发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void GvBondPosition_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            string oldValue = (string)this.GvBondPosition.GetRowCellValue(e.RowHandle, e.Column);
            string newName = (string)e.Value;

            DialogResult dialogResult = AKRSXtraMessageBox.Show($"焊点名称{oldValue} => {newName},是否确认该操作", "提示", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.OK)
            {
                // 保存数据
                ProductConfiguration.GetInstance().Save();

                this.RefreshControl();
            }
            else
            {
                this.RefreshControl();
            }
        }


        /// <summary>
        /// 焊点数据发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void GvBondPosition_RowCellClick(object sender, RowCellClickEventArgs e)
        {
           
        }

        /// <summary>
        /// 改变焊点的Pr名称
        /// </summary>
        /// <param name="oldName">原来的名称</param>
        /// <param name="newName">新的名称</param>
        private void ChangePrName(string oldName, string newName)
        {
            List<string> list = new List<string>() { "Mark1", "Mark2", "Mark3", "Mark4" };

            foreach (string mark in list)
            {
                PREntity.CopyPREntity(
                    MachineConfigContext.GetInstance().RecipeName + oldName + mark,
                    MachineConfigContext.GetInstance().RecipeName + newName + mark);
                PREntity.DeletePREntity(MachineConfigContext.GetInstance().RecipeName + oldName + mark);
            }
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void GcBondPosition_DoubleClick(object sender, EventArgs e)
        {
            // 获取当前选中的焊点配置
            SingleBondPositionConfig bondPositionConfig = (SingleBondPositionConfig)this.GvBondPosition.GetFocusedRow();

            if (bondPositionConfig == null)
            {
                return;
            }

            // 获取当前名称
            string defaultName = bondPositionConfig.Name;
            string createName = XtraInputBox.Show("请输入修改后焊点的名称！", "焊点名称", defaultName, MessageBoxButtons.YesNo);

            if (string.IsNullOrEmpty(createName))
            {
                return;
            }

            if (createName == defaultName)
            {
                return;
            }

            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                if (createName.Contains(rInvalidChar.ToString()))
                {
                    AKRSXtraMessageBox.Show($"焊点名存在非法字符: {rInvalidChar.ToString()}！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
            }

            if (createName.Length > 15)
            {
                AKRSXtraMessageBox.Show(
                    "焊点名称长度超过限制!",
                    "提示");
                return;
            }

            if (this.bondPositionConfigList.Exists(it => it.Name == createName))
            {
                AKRSXtraMessageBox.Show(
                    "焊点名称已存在，请更换名称!",
                    "提示");
                return;
            }

            // 刷新相关的数据
            ProductConfiguration.GetInstance().OtherConfig.ChangeBpName(bondPositionConfig.Name, createName);
            ProcessDomain.GetInstance().ChangeBpName(bondPositionConfig.Name, createName);
            this.ChangePrName(bondPositionConfig.Name, createName);
            if (TransportDomain.GetInstance().TransportController.IsExistTu)
            {
                TransportDomain.GetInstance().TransportController.InitializeTS();
            }
            bondPositionConfig.Name = createName;

            // 保存数据
            ProductConfiguration.GetInstance().Save();

            this.RefreshControl();
        }
    }
}
