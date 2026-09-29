using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using System.ComponentModel;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 移动到指定基岛示教窗体
    /// Todo 这个界面被修改过。后续未验证，使用时小心
    /// </summary>
    public partial class FrmModuleSelect : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="singleBondPositionConfig">焊点配置</param>
        /// <param name="teachFormEnum">页面</param>
        public FrmModuleSelect(SingleBondPositionConfig singleBondPositionConfig, TeachFormEnum teachFormEnum)
        {
            this.bondPosition = singleBondPositionConfig;
            this.InitializeComponent();
            this.EntityType = EntityTypeEnum.BondPosition;
            this.Init();
            this.Text = singleBondPositionConfig.Name.ToString() + " Select";
            this.transportUnit = TUAssistantHelper.JudgeTuExist();
            this.teachForm = teachFormEnum;
        }

        /// <summary>
        /// 是否定位自己
        /// </summary>
        public bool IsVisionSelf { get; set; } = true;

        /// <summary>
        /// 示教页面
        /// </summary>
        private TeachFormEnum teachForm;

        /// <summary>
        /// 移动界面构造函数
        /// </summary>
        /// <param name="entityTypeEnum">移动层次</param>
        public FrmModuleSelect(EntityTypeEnum entityTypeEnum)
        {
            this.InitializeComponent();
            this.EntityType = entityTypeEnum;
            this.Text = entityTypeEnum.ToString() + " Select";
            this.Init();
            this.transportUnit = TUAssistantHelper.JudgeTuExist();
        }

        /// <summary>
        ///  移动层次
        /// </summary>
        private EntityTypeEnum EntityType { get; set; }

        /// <summary>
        /// 配置对象
        /// </summary>
        private ProductConfiguration ProductConfig => ProductConfiguration.GetInstance();

        /// <summary>
        /// 焊点配置集
        /// </summary>
        private List<SingleBondPositionConfig> BondPositionConfigList =>
            ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList;

        /// <summary>
        /// 点击Start传进来的焊点对象
        /// </summary>
        private SingleBondPositionConfig bondPosition;

        /// <summary>
        /// 点胶视觉控制器
        /// </summary>
        private DispenseVisionController dispenseVisionController = new DispenseVisionController();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 基板
        /// </summary>
        public Substrate CurrentSubstrate 
        {
            get
            {
                return this.transportUnit.Substrates.Find(it => it.Index == this.SpSubstrateNum.Value);
            }
        }

        /// <summary>
        /// 基岛
        /// </summary>
        public Module CurrentModule 
        {
            get
            {
                return this.CurrentSubstrate.Modules.Find(it => it.Index == this.SpModuleNum.Value);
            }
        }

        /// <summary>
        /// 基岛
        /// </summary>
        public BondPosition CurrentBondPosition
        {
            get
            {
                return this.CurrentModule.BondPositions.Find(
                    it => it.Name == this.CmbBondPositions.SelectedItem.ToString());
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                this.dispenseVisionController.InitLight(this.ucLight1, this.ucLight2);
            }
            else
            {
                // Bond光源配置
                this.ucLight1.Init("Bond点光", "Bond点光红光", "Bond点光绿光", "Bond电光蓝光");
                this.ucLight2.Init("Bond环光", "Bond点光环光", "Bond点光环光", "Bond环光蓝光");
            }

            this.SpSubstrateNum.Properties.MaxValue = this.ProductConfig.SubstrateConfig.Count;
            this.SpSubstrateColumn.Properties.MaxValue = this.ProductConfig.SubstrateConfig.ColumnCount;
            this.SpSubstrateRow.Properties.MaxValue = this.ProductConfig.SubstrateConfig.RowCount;

            this.SpModuleNum.Properties.MaxValue = this.ProductConfig.ModuleConfig.Count;
            this.SpModuleColumn.Properties.MaxValue = this.ProductConfig.ModuleConfig.ColumnCount;
            this.SpModuleRow.Properties.MaxValue = this.ProductConfig.ModuleConfig.RowCount;

            if (this.ProductConfig.SubstrateConfig.Multiplication != MultiplicationEnum.Matrix)
            {
                this.SpSubstrateRow.Visible = false;
                this.SpSubstrateColumn.Visible = false;
            }

            if (this.ProductConfig.ModuleConfig.Multiplication != MultiplicationEnum.Matrix)
            {
                this.SpModuleRow.Visible = false;
                this.SpModuleColumn.Visible = false;
            }

            if (this.EntityType == EntityTypeEnum.BondPosition)
            {
                // 焊点名称集合
                List<string> bondPositionNameList = new List<string>();

                // 获取数据源
                foreach (var item in this.BondPositionConfigList)
                {
                    bondPositionNameList.Add(item.Name);
                }

                this.CmbBondPositions.Properties.Items.Clear();
                this.CmbBondPositions.Properties.Items.AddRange(bondPositionNameList);
                if (this.bondPosition != null)
                {
                    this.CmbBondPositions.SelectedItem = this.bondPosition.Name;
                }
                else if (this.CmbBondPositions.Properties.Items.Count != 0)
                {
                    this.CmbBondPositions.SelectedIndex = 0;
                }
            }
            else
            {
                this.CmbBondPositions.Visible = false;
                this.CmbBondPositions.Enabled = false;
            }
        }

        /// <summary>
        /// 当前传输单元/载具 
        /// </summary>
        private TransportUnit transportUnit;


        /// <summary>
        /// 设置基板号，基岛号
        /// </summary>
        /// <param name="subIndex">基板号</param>
        /// <param name="moduleIndex">基岛号</param>
        public void SetModuleIndex(int subIndex,int moduleIndex)
        {
            this.SpSubstrateNum.EditValue = subIndex;
            this.SpModuleNum.EditValue = moduleIndex;
        }


        /// <summary>
        /// 移动到指定的位置
        /// </summary>
        public void MoveToPos()
        {
            AKRSPoint3D point3D = null;

            if (this.EntityType == EntityTypeEnum.Substrate) 
            {
                point3D = this.CurrentSubstrate.CoordinateSystem
                       .SelfPosToG0(new AKRSPoint3D());
            }
           else if (this.EntityType == EntityTypeEnum.Module)
            {
                point3D = this.CurrentModule.CoordinateSystem
                      .SelfPosToG0(new AKRSPoint3D());
            }
            else
            {
                BondPosition bp = this.CurrentModule.BondPositions.Find(it => it.Name == this.CmbBondPositions.Text);

                point3D = this.CurrentModule.CoordinateSystem.SelfPosToG0(bp.SingleBondPositionConfig.ElementCoordinate.Point);
            }

            //AKRSPoint3D point3D = this.CurrentSubstrate.CoordinateSystem
            //   .SelfPosToG0(new AKRSPoint3D());

            // 移动到指定的位置
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
               //  System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(point3D);

                AKRSPoint3D point3D1 = System1Domain.GetInstance().DispenseController.GetG0VisionPos(point3D);

                System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(point3D1);
            }
            else
            {
                // 获取位置
                point3D = point3D - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                this.bondModuleController.MoveToG0Pos(point3D);
            }
        }

        /// <summary>
        /// 设置行列号
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public void SetSubRowColumn(int row, int column)
        {
            this.SpSubstrateRow.Value = row;
            this.SpSubstrateColumn.Value = column;
        }


        /// <summary>
        /// 设置行列号
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        public void SetModuleRowColumn(int row, int column)
        {
            this.SpModuleRow.Value = row;
            this.SpModuleColumn.Value = column;
        }

        /// <summary>
        /// 设置索引
        /// </summary>
        public void SetIndex()
        {
            Substrate substrate = this.transportUnit.Substrates.Find(
                it => it.ColumnIndex == this.SpSubstrateColumn.Value && it.RowIndex == this.SpSubstrateRow.Value);

            this.SpSubstrateNum.Value = substrate.Index;

            Module module = this.CurrentSubstrate.Modules.Find(
                it => it.ColumnIndex == this.SpModuleColumn.Value && it.RowIndex == this.SpModuleRow.Value);

            this.SpModuleNum.Value = module.Index;
        }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                this.DialogResult = DialogResult.OK;
                return;
            }
            
            bool success = false;

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                if (this.EntityType == EntityTypeEnum.Substrate)
                {
                    if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateAdjust.State == AssistantStateEnum.Able && IsVisionSelf)
                    {
                        success = TUAssistantHelper.System2VisionByPos(this.CurrentSubstrate);
                    }
                    else
                    {
                        success = TUAssistantHelper.System2VisionByPos(this.transportUnit);
                    }
                   
                    if (!success)
                    {
                        this.DialogResult = DialogResult.Abort;
                        return;
                    }
                }
                else if (this.EntityType == EntityTypeEnum.Module)
                {
                    if (ProductConfiguration.GetInstance().TransportUnitConfig.ModuleAdjust.State == AssistantStateEnum.Able && IsVisionSelf)
                    {
                        success = TUAssistantHelper.System2VisionByPos(this.CurrentModule);
                    }
                    else
                    {
                        success = TUAssistantHelper.System2VisionByPos(this.CurrentSubstrate);
                    }

                    if (!success)
                    {
                        this.DialogResult = DialogResult.Abort;
                        return;
                    }
                }
                else
                {
                   SingleBondPositionConfig bondPositionConfig = ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList.Find(it => it.Name == this.CurrentBondPosition.Name);

                    if (bondPositionConfig.BondPositionAdjust.State == AssistantStateEnum.Able && IsVisionSelf)
                    {
                        success = TUAssistantHelper.System2VisionByPos(this.CurrentBondPosition);
                    }
                    else
                    {
                        success = TUAssistantHelper.System2VisionByPos(this.CurrentModule);
                    }

                    if (!success)
                    {
                        this.DialogResult = DialogResult.Abort;
                        return;
                    }
                }

            }
            else
            {
                if (this.EntityType == EntityTypeEnum.Substrate)
                {
                    if (!success)
                    {
                        this.DialogResult = DialogResult.Abort;
                        return;
                    }
                }
                else if (this.EntityType == EntityTypeEnum.Module)
                {
                    if (!success)
                    {
                        this.DialogResult = DialogResult.Abort;
                        return;
                    }
                }
                else
                {
                    if (!success)
                    {
                        this.DialogResult = DialogResult.Abort;
                        return;
                    }
                }
            }

            this.MoveToPos();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 移动到莫一个基板，由外界传入
        /// </summary>
        /// <param name="substrateIndex">基板号</param>
        /// <param name="moduleIndex">基岛号</param>
        public void MoveToModule(int substrateIndex, int moduleIndex)
        {
            this.SpSubstrateNum.Value = substrateIndex;
            this.SpModuleNum.Value = moduleIndex;
            this.MoveToPos();
        }

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;      
    }

        /// <summary>
        /// 左移一个基岛
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveLeft_Click(object sender, EventArgs e)
        {
            this.MoveToModule(MoveTypeEnum.Left);
        }

        /// <summary>
        /// 右移一个基岛
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveRight_Click_1(object sender, EventArgs e)
        {
            this.MoveToModule(MoveTypeEnum.Right);
        }

        /// <summary>
        /// 上移一个基岛
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            this.MoveToModule(MoveTypeEnum.Up);
        }

        /// <summary>
        /// 下移一个基岛
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            this.MoveToModule(MoveTypeEnum.Down);
        }

        /// <summary>
        /// 基板行数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSubstrateRow_EditValueChanged(object sender, EventArgs e)
        {
            this.SpSubstrateNum.EditValueChanged -= new System.EventHandler(this.SpSubstrateNum_EditValueChanged);
            this.SetIndex();
            this.SpSubstrateNum.EditValueChanged += new System.EventHandler(this.SpSubstrateNum_EditValueChanged);
        }

        /// <summary>
        /// 基板列数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSubstrateColumn_EditValueChanged(object sender, EventArgs e)
        {
            this.SpSubstrateNum.EditValueChanged -= new System.EventHandler(this.SpSubstrateNum_EditValueChanged);
            this.SetIndex();
            this.SpSubstrateNum.EditValueChanged += new System.EventHandler(this.SpSubstrateNum_EditValueChanged);
        }

        /// <summary>
        /// 上移一个基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSubstrateMoveUp_Click(object sender, EventArgs e)
        {
            this.MoveToSubstrate(MoveTypeEnum.Up);
        }

        /// <summary>
        /// 右移一个基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSubstrateMoveRight_Click(object sender, EventArgs e)
        {
            this.MoveToSubstrate(MoveTypeEnum.Right);
        }

        /// <summary>
        /// 下移一个基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSubstrateMoveDown_Click(object sender, EventArgs e)
        {
            this.MoveToSubstrate(MoveTypeEnum.Down);
        }

        /// <summary>
        /// 左移一个基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSubstrateMoveLeft_Click(object sender, EventArgs e)
        {
            this.MoveToSubstrate(MoveTypeEnum.Left);
        }

        /// <summary>
        /// 基岛列数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpModuleColumn_EditValueChanged(object sender, EventArgs e)
        {
            this.SpModuleNum.EditValueChanged -= new System.EventHandler(this.SpModuleNum_EditValueChanged);
            this.SetIndex();
            this.SpModuleNum.EditValueChanged += new System.EventHandler(this.SpModuleNum_EditValueChanged);
        }

        /// <summary>
        /// 基岛行数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpModuleRow_EditValueChanged(object sender, EventArgs e)
        {
            this.SpModuleNum.EditValueChanged -= new System.EventHandler(this.SpModuleNum_EditValueChanged);
            this.SetIndex();
            this.SpModuleNum.EditValueChanged += new System.EventHandler(this.SpModuleNum_EditValueChanged);
        }

        /// <summary>
        /// 基板序号改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSubstrateNum_EditValueChanged(object sender, EventArgs e)
        {
            this.SpSubstrateColumn.EditValueChanged -= new System.EventHandler(this.SpSubstrateColumn_EditValueChanged);
            this.SpSubstrateRow.EditValueChanged -= new System.EventHandler(this.SpSubstrateRow_EditValueChanged);

            int column = this.CurrentSubstrate.ColumnIndex;
            int row = this.CurrentSubstrate.RowIndex;
            this.SpSubstrateColumn.Value = column;
            this.SpSubstrateRow.Value = row;

            this.SpSubstrateColumn.EditValueChanged += new System.EventHandler(this.SpSubstrateColumn_EditValueChanged);
            this.SpSubstrateRow.EditValueChanged += new System.EventHandler(this.SpSubstrateRow_EditValueChanged);
        }

        /// <summary>
        /// 基岛序号改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpModuleNum_EditValueChanged(object sender, EventArgs e)
        {
            this.SpModuleRow.EditValueChanged -= new System.EventHandler(this.SpModuleRow_EditValueChanged);
            this.SpModuleColumn.EditValueChanged -= new System.EventHandler(this.SpModuleColumn_EditValueChanged);
            int column = this.CurrentModule.ColumnIndex;
            int row = this.CurrentModule.RowIndex;
            this.SpModuleColumn.Value = column;
            this.SpModuleRow.Value = row;
            this.SpModuleRow.EditValueChanged += new System.EventHandler(this.SpModuleRow_EditValueChanged);
            this.SpModuleColumn.EditValueChanged += new System.EventHandler(this.SpModuleColumn_EditValueChanged);
        }

        #region 移动判定


        /// <summary>
        /// 移动到位置
        /// </summary>
        /// <param name="type">类型</param>
        private void MoveToSubstrate(MoveTypeEnum type)
        {
            // 防呆
            if (this.EntityType == EntityTypeEnum.BondPosition && string.IsNullOrEmpty(this.CmbBondPositions.Text))
            {
                AKRSXtraMessageBox.Show(
                    "请先选择焊点!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (this.ProductConfig.SubstrateConfig.Multiplication != MultiplicationEnum.Matrix)
            {
                this.SpSubstrateNum.Value++;
            }
            else
            {
                int row = 0;
                int column = 0;
                if (type == MoveTypeEnum.Up)
                {
                    // 获取当前的行数
                    row = this.CurrentSubstrate.RowIndex + 1;
                    column = this.CurrentSubstrate.ColumnIndex;
                }
                else if (type == MoveTypeEnum.Down)
                {
                    // 获取当前的行数
                    row = this.CurrentSubstrate.RowIndex - 1;
                    column = this.CurrentSubstrate.ColumnIndex;
                }
                else if (type == MoveTypeEnum.Right)
                {
                    // 获取当前的行数
                    row = this.CurrentSubstrate.RowIndex;
                    column = this.CurrentSubstrate.ColumnIndex + 1;
                }
                else if (type == MoveTypeEnum.Left)
                {
                    // 获取当前的行数
                    row = this.CurrentSubstrate.RowIndex;
                    column = this.CurrentSubstrate.ColumnIndex - 1;
                }


                Substrate sub =
                    this.transportUnit.Substrates.Find(it => it.RowIndex == row && it.ColumnIndex == column);


                if (sub == null)
                {
                    AKRSXtraMessageBox.Show($"{type.ToString()} 超出范围限制，不能移动！");
                    return;
                }

                this.SetSubRowColumn(row, column);

                this.SpSubstrateNum.Value = sub.Index;
            }

            this.MoveToPos();
        }


        /// <summary>
        /// 移动到位置
        /// </summary>
        /// <param name="type">类型</param>
        private void MoveToModule(MoveTypeEnum type)
        {
            // 防呆
            if (this.EntityType == EntityTypeEnum.BondPosition && string.IsNullOrEmpty(this.CmbBondPositions.Text))
            {
                AKRSXtraMessageBox.Show(
                    "请先选择焊点!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (this.ProductConfig.ModuleConfig.Multiplication != MultiplicationEnum.Matrix)
            {
                this.SpSubstrateNum.Value++;
            }
            else
            {
                int row = 0;
                int column = 0;
                if (type == MoveTypeEnum.Up)
                {
                    // 获取当前的行数
                    row = this.CurrentModule.RowIndex + 1;
                    column = this.CurrentModule.ColumnIndex;
                }
                else if (type == MoveTypeEnum.Down)
                {
                    // 获取当前的行数
                    row = this.CurrentModule.RowIndex - 1;
                    column = this.CurrentModule.ColumnIndex;
                }
                else if (type == MoveTypeEnum.Right)
                {
                    // 获取当前的行数
                    row = this.CurrentModule.RowIndex;
                    column = this.CurrentModule.ColumnIndex + 1;
                }
                else if (type == MoveTypeEnum.Left)
                {
                    // 获取当前的行数
                    row = this.CurrentModule.RowIndex;
                    column = this.CurrentModule.ColumnIndex - 1;
                }

                this.SetModuleRowColumn(row, column);

                Module module =
                    this.CurrentSubstrate.Modules.Find(it => it.RowIndex == row && it.ColumnIndex == column);

                if (module == null)
                {
                    AKRSXtraMessageBox.Show($"{type.ToString()} 超出范围限制，不能移动！");
                    return;
                }

                this.SpModuleNum.Value = module.Index;
            }

            this.MoveToPos();
        }

        #endregion


        /// <summary>
        /// 移动类型
        /// </summary>
        public enum MoveTypeEnum
        {
            /// <summary>
            /// 上
            /// </summary>
            Up, 
            
            /// <summary>
            /// 下
            /// </summary>
            Down,

            /// <summary>
            /// 左
            /// </summary>
            Left,

            /// <summary>
            /// 右
            /// </summary>
            Right
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtMove_Click(object sender, EventArgs e)
        {
            // 防呆
            if (this.EntityType == EntityTypeEnum.BondPosition && string.IsNullOrEmpty(this.CmbBondPositions.Text))
            {
                AKRSXtraMessageBox.Show(
                    "请先选择焊点!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            this.MoveToPos();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            // 设置焊点编辑状态完成
            this.bondPosition.EditState = EditStateEnum.Completely;

            // 根据焊点配置集创建焊点
            if (this.teachForm == TeachFormEnum.MoveToBondingPosition)
            {
                this.bondPosition.MoveToBondingPosition.State = AssistantStateEnum.Able;
            }

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 界面加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmModuleSelect_Load(object sender, EventArgs e)
        {
            this.transportUnit = TUAssistantHelper.JudgeTuExist();
            if (this.transportUnit == null)
            {
                this.DialogResult = DialogResult.Abort;
            }
        }

        /// <summary>
        /// 选中焊点
        /// </summary>
        /// <param name="bondPositionName">焊点名称</param>
        public void SelectBondPosition(string bondPositionName)
        {
            if (bondPositionName == null)
            {
                return;
            }

            if (this.CmbBondPositions.Properties.Items.Contains(bondPositionName))
            {
                this.CmbBondPositions.SelectedItem = bondPositionName;
            }
        }

        /// <summary>
        /// 打开相机
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtRealTimeVision_Click(object sender, EventArgs e)
        {
            UcMainSystem.VmVisionShow();
            UcMainSystem.ChangeCameraVision(CameraEnum.BondCamera.GetDescription());
        }
    }

    /// <summary>
    /// 示教页面枚举
    /// </summary>
    public enum TeachFormEnum
    {
        /// <summary>
        /// TeachBondingPosition
        /// </summary>
        [Description("TeachBondingPosition")]
        TeachBondingPosition = 0,

        /// <summary>
        /// TeachBondingPosition
        /// </summary>
        [Description("TeachBondingPosition")]
        MoveToBondingPosition = 1,

        /// <summary>
        /// BondPositionAdjust
        /// </summary>
        [Description("BondPositionAdjust")]
        BondPositionAdjust = 3,

        /// <summary>
        /// BondPositionAdjust
        /// </summary>
        [Description("BondPositionAdjust")]
        BondPositionMeasureHeight = 4
    }
}