using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 移动到焊点示教窗体,弃用
    /// </summary>
    public partial class FrmMoveToBondingPosition : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 点胶视觉模块控制器
        /// </summary>
        private DispenseVisionController dispenseVisionController = new DispenseVisionController();

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="singleBondPositionConfig">焊点配置</param>
        public FrmMoveToBondingPosition(SingleBondPositionConfig singleBondPositionConfig)
        {
            this.bondPosition = singleBondPositionConfig;
            this.InitializeComponent();
            this.Init();

             InitMovement();
        }

        /// <summary>
        /// 当前载具
        /// </summary>
        private TransportUnit TransportUnit => TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;


        private bool SubMove;

        /// <summary>
        /// 配置对象
        /// </summary>
        private ProductConfiguration ProductConfig => ProductConfiguration.GetInstance();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 基板号
        /// </summary>
        public Substrate CurrentSubstrate
        {
            get
            {
                return this.TransportUnit.Substrates.Find(it => it.Index == this.SpSubstrateNum.Value);
            }
        }

        /// <summary>
        /// 基岛号
        /// </summary>
        public Module CurrentModule
        {
            get
            {
                return this.CurrentSubstrate.Modules.Find(it => it.Index == this.SpModuleNum.Value);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.transportUnit = new TransportUnit(MachineStateModel.GetInstance().CurrentMachineSystem);

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

            this.SpModuleNum.Properties.MaxValue = this.ProductConfig.SubstrateConfig.Count;

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


            // 焊点名称集合
            List<string> bondPositionNameList = new List<string>();

            // 获取数据源
            foreach (var item in this.BondPositionConfigList)
            {
                bondPositionNameList.Add(item.Name);
            }

            this.CmbBondPositions.Properties.Items.Clear();
            this.CmbBondPositions.Properties.Items.AddRange(bondPositionNameList);
            this.CmbBondPositions.SelectedItem = this.bondPosition.Name;
        }

        /// <summary>
        /// 点击Start传进来的焊点对象
        /// </summary>
        private SingleBondPositionConfig bondPosition;

        /// <summary>
        /// 固晶模组
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 固晶域
        /// </summary>
        private System2Domain System2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 焊点配置集
        /// </summary>
        private List<SingleBondPositionConfig> BondPositionConfigList =>
            ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList;

        /// <summary>
        /// 是否第一次加载,防呆用
        /// </summary>
        private bool isFirstLoad = true;


        /// <summary>
        /// 当前传输单元/载具 
        /// </summary>
        private TransportUnit transportUnit;
        
        /// <summary>
        /// 初始运动
        /// </summary>
        private void InitMovement()
        {
            // 获取焊点所在基岛
            Module module = this.TransportUnit.GetModule(this.bondPosition.TeachSubstrateNum, this.bondPosition.TeachModuleNum);

            if (this.TransportUnit.GetBondPosition(this.bondPosition.TeachSubstrateNum, this.bondPosition.TeachModuleNum, this.bondPosition.Name) == null)
            {
                return;
            }

            // 获取焊点视觉位
            AKRSPoint3D visionPos = System2TUService.GetBondPositionVisionPos(this.bondPosition.TeachSubstrateNum, this.bondPosition.TeachModuleNum, this.bondPosition.Name);

            // 移动到视觉位
            this.bondModuleController.MoveSafeBondXYZ(visionPos);
        }

        /// <summary>
        /// 移动到焊点示教位
        /// </summary>
        private void MoveToTeachPos()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (!BondModule.IsReady())
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"CAUTION:Bond  axis is  null.\r\n" + "Ignore with  OK\r\n" + "End with  Cancel\r\n",
                    "Warn",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.Cancel)
                {
                    this.Close();
                }

                return;
            }

            // 获取示教贴片位
            AKRSPoint3D teachPos = System2TUService.GetBondPositionTeachPos(
                (int)this.bondPosition.TeachSubstrateNum,
                (int)this.bondPosition.TeachModuleNum,
                this.bondPosition.Name);

            // this.bondModuleController.MoveBondXY(teachPos.X, teachPos.Y);

            // 移动到示教贴片位
            this.bondModuleController.MoveSafeBondXYZ(teachPos);
        }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            this.Save();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            // 设置焊点编辑状态完成
            this.bondPosition.EditState = EditStateEnum.Completely;

            // 根据焊点配置集创建焊点
            ProductConfiguration.GetInstance().Save();
        }


        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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
        private void BtnMoveRight_Click(object sender, EventArgs e)
        {
            this.MoveToModule(MoveTypeEnum.Right);
        }

        /// <summary>
        /// 检查行列数是否超出载具规格（防呆用）
        /// </summary>
        private void CarrierBorderCheck()
        {
            // 左移防呆
            if (this.SpModuleColumn.Value >= ProductConfiguration.GetInstance().ModuleConfig.ColumnCount)
            {
                this.BtnModuelMoveLeft.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveLeft.Enabled = true;
            }

            // 右移防呆
            if (this.SpModuleColumn.Value <= 1)
            {
                this.BtnModuelMoveRight.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveRight.Enabled = true;
            }

            // 下移防呆
            if (this.SpModuleRow.Value >= ProductConfiguration.GetInstance().ModuleConfig.RowCount)
            {
                this.BtnModuelMoveDown.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveDown.Enabled = true;
            }

            // 上移防呆
            if (this.SpModuleRow.Value <= 1)
            {
                this.BtnModuelMoveUp.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveUp.Enabled = true;
            }

            if (ProductConfiguration.GetInstance().ModuleConfig.ColumnCount <= 1 && ProductConfiguration.GetInstance().ModuleConfig.RowCount <= 1)
            {
                this.GcSubstrate.Enabled = false;
            }
            else
            {
                this.GcSubstrate.Enabled = true;
            }
        }

        /// <summary>
        /// 检查行列数是否超出基板规格（防呆用）
        /// </summary>
        private void SubstrateBorderCheck()
        {
            // 左移防呆
            if (this.SpModuleColumn.Value >= ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount)
            {
                this.BtnModuelMoveLeft.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveLeft.Enabled = true;
            }

            // 右移防呆
            if (this.SpModuleColumn.Value <= 1)
            {
                this.BtnModuelMoveRight.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveRight.Enabled = true;
            }

            // 下移防呆
            if (this.SpModuleRow.Value >= ProductConfiguration.GetInstance().SubstrateConfig.RowCount)
            {
                this.BtnModuelMoveDown.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveDown.Enabled = true;
            }

            // 上移防呆
            if (this.SpModuleRow.Value <= 1)
            {
                this.BtnModuelMoveUp.Enabled = false;
            }
            else
            {
                this.BtnModuelMoveUp.Enabled = true;
            }

            if (ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount <= 1 && ProductConfiguration.GetInstance().SubstrateConfig.RowCount <= 1)
            {
                this.GcSubstrate.Enabled = false;
            }
            else
            {
                this.GcSubstrate.Enabled = true;
            }
        }

        /// <summary>
        /// Combobox下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbBondPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            System2Domain.BondHeadController.MoveBondZToSafePos();

            // 第一次加载窗体不触发以下事件
            if (this.isFirstLoad)
            {
                return;
            }

            // 判断焊点编辑状态
            if (this.TransportUnit.GetBondPosition(
                    (int)this.SpSubstrateNum.Value,
                    (int)this.SpModuleNum.Value,
                    this.CmbBondPositions.Text).EditState != EditStateEnum.Completely)
            {
                // 获取示教基岛
                AKRSPoint3D teachPos = System2TUService.GetModulePos(
                    (int)this.bondPosition.TeachSubstrateNum,
                    (int)this.bondPosition.TeachModuleNum);

                // 移动到示教基岛
                this.bondModuleController.MoveSafeBondXYZ(System2TUService.GetModulePos(
                    (int)this.bondPosition.TeachSubstrateNum,
                    (int)this.bondPosition.TeachModuleNum));
            }
            else
            {
                // 获取所选焊点示教贴片位
                AKRSPoint3D teachPos = System2TUService.GetBondPositionTeachPos(
                    (int)this.SpSubstrateNum.Value,
                    (int)this.SpModuleNum.Value,
                    this.CmbBondPositions.Text);

                // 移动到所选焊点示教贴片位
                this.bondModuleController.MoveSafeBondXYZ(teachPos);

                this.SubstrateBorderCheck();
            }
        }

        /// <summary>
        /// 基岛行数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpModuelRow_EditValueChanged(object sender, EventArgs e)
        {
            this.SetIndex();
        }

        /// <summary>
        /// 基岛列数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpModuelColumn_EditValueChanged(object sender, EventArgs e)
        {
            this.SetIndex();
        }

        /// <summary>
        /// 基板行数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSubstrateRow_EditValueChanged(object sender, EventArgs e)
        {
            this.SetIndex();
        }

        /// <summary>
        /// 基板列数改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSubstrateColumn_EditValueChanged(object sender, EventArgs e)
        {
            this.SetIndex();
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
        /// 左移一个基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSubstrateMoveLeft_Click(object sender, EventArgs e)
        {
            this.MoveToSubstrate(MoveTypeEnum.Left);
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
        /// 基岛号改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpModuleNum_EditValueChanged(object sender, EventArgs e)
        {
            int column = this.CurrentModule.ColumnIndex;
            int row = this.CurrentModule.RowIndex;
            this.SpModuleColumn.Value = column;
            this.SpModuleRow.Value = row;
        }

        /// <summary>
        /// 基板号改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSubstrateNum_EditValueChanged(object sender, EventArgs e)
        {
            int column = this.CurrentSubstrate.ColumnIndex;
            int row = this.CurrentSubstrate.RowIndex;
            this.SpSubstrateColumn.Value = column;
            this.SpSubstrateRow.Value = row;
        }

        #region 移动判定


        /// <summary>
        /// 移动到指定的位置
        /// </summary>
        public void MoveToPos()
        {
            AKRSPoint3D point3D = null;
            if (SubMove)
            {
                point3D = this.CurrentSubstrate.CoordinateSystem
                       .SelfPosToG0(new AKRSPoint3D());
            }
            else
            {
                point3D = this.CurrentModule.CoordinateSystem
                      .SelfPosToG0(new AKRSPoint3D());
            }
            // 获取位置
            point3D.Z = point3D.Z + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Z;
            point3D.X = point3D.X - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X;
            point3D.Y = point3D.Y - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y;

            //AKRSPoint3D point3D = this.CurrentSubstrate.CoordinateSystem
            //   .SelfPosToG0(new AKRSPoint3D());

            // 移动到指定的位置
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(point3D);
            }
            else
            {
                this.bondModuleController.MoveToG0Pos(point3D);
            }
        }

        /// <summary>
        /// 设置行列号
        /// </summary>
        public void SetSubRowColumn(int row, int column)
        {
            this.SpSubstrateColumn.Value = row;
            this.SpSubstrateRow.Value = column;
        }

        /// <summary>
        /// 设置行列号
        /// </summary>
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
                it => it.ColumnIndex == this.SpModuleColumn.Value && it.RowIndex == this.SpModuleNum.Value);

            // this.SpModuleNum.Value = module.Index;
        }

        /// <summary>
        /// 设置基板号，基岛号
        /// </summary>
        /// <param name="subIndex">基板号</param>
        /// <param name="moduleIndex">基岛号</param>
        public void SetModuleIndex(int subIndex, int moduleIndex)
        {
            this.SpSubstrateNum.EditValue = subIndex;
            this.SpModuleNum.EditValue = moduleIndex;
        }


        /// <summary>
        /// 移动到位置
        /// </summary>
        /// <param name="type">类型</param>
        private void MoveToSubstrate(MoveTypeEnum type)
        {
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
                    AKRSXtraMessageBox.Show($"{type.ToString()} exceeding the limit, can not move");
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
                    AKRSXtraMessageBox.Show($"{type.ToString()} exceeding the limit, can not move");
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

    }
}