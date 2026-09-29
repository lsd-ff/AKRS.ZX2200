using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controls.Assistant;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.TransportSystem.Models;
using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using DevExpress.CodeParser;
    using log4net.Core;
    using System;
    using System.Drawing;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Module = AKRS.ZX2200.TransportUnitSystem.Module.Matter.Module;

    /// <summary>
    /// 示教帮助类
    /// </summary>
    public static class TUAssistantHelper
    {
        /// <summary>
        /// 页面制作pR
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public static (bool isSuccesss, AKRSPoint3D point) AssistantPR(string name)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return (true, new AKRSPoint3D());
            }

            DispenseVisionController dispenseVisionController = new DispenseVisionController();

            // 执行定位
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                MatchResult result = (MatchResult)dispenseVisionController.DispenseVision(null, name);

                if (result == null)
                {
                    return (false, null);
                }

                AKRSPoint3D point = System1Domain.GetInstance().DispenseController.ConvertVisionResultInG0(
                    result,
                    System1Domain.GetInstance().DispenseController.GetAxisPos());

                AKRSPoint3D g0Pos = System1Domain.GetInstance().DispenseController.GetG0PosFromVision(point);

                return (true, g0Pos);
            }
            else
            {
                MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(null, name, name);

                if (result == null)
                {
                    return (false, null);
                }

                // 将值转化成G(0)
                AKRSPoint3D point = System2Module.GetInstance().BondModule.ConvertPixelToG0Pos(
                    System2Module.GetInstance().BondModule.Get3DRealPosition(),
                    result);

                point = point + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                return (true, point);
            }
        }

        /// <summary>
        /// 页面制作pR
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public static (bool isSuccesss, AKRSPoint3D point, double angle) AssistantPRAndAngle(string name)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return (true, new AKRSPoint3D(), 0);
            }

            DispenseVisionController dispenseVisionController = new DispenseVisionController();

            // 执行定位
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                MatchResult result = (MatchResult)dispenseVisionController.DispenseVision(null, name);

                if (result == null)
                {
                    return (false, null, 0);
                }

                AKRSPoint3D point = System1Domain.GetInstance().DispenseController.ConvertVisionResultInG0(
                    result,
                    System1Domain.GetInstance().DispenseController.GetAxisPos());

                AKRSPoint3D g0Pos = System1Domain.GetInstance().DispenseController.GetG0PosFromVision(point);

                return (true, g0Pos, result.Angle);
            }
            else
            {
                MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(null, name, name);

                if (result == null)
                {
                    return (false, null, 0);
                }

                // 将值转化成G(0)
                AKRSPoint3D point = System2Module.GetInstance().BondModule.ConvertPixelToG0Pos(
                    System2Module.GetInstance().BondModule.Get3DRealPosition(),
                    result);

                point = point + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                return (true, point, result.Angle);
            }
        }


        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="cameraType">相机类型</param>
        /// <param name="algFlowTypeEnum">模板类型</param>
        public static void EditPr(string name, CameraTypeEnum cameraType = CameraTypeEnum.BondCamera, AlgFlowTypeEnum algFlowTypeEnum = AlgFlowTypeEnum.XldModelAlg, AlgBeLongEnum algBeLong = AlgBeLongEnum.Substrate)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);

                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);

                prEntity.SetAlgFlowType(algFlowTypeEnum);

                prEntity.Alg.AlgBeLong = algBeLong;

                // 模板新创建时，默认使用当前的参数
                if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                {
                    System2Domain.GetInstance().System2Controller.SetDefaultHardwareParameter(prEntity, cameraType);
                }
                else
                {
                    System1Domain.GetInstance().DispenseVisionController.SetDefaultHardwareParameter(prEntity);
                }

            }

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(cameraType));
            }
            else
            {
                prEntity.SetHardware(System1Domain.GetInstance().DispenseVisionController.GetHardware());
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="cameraType">相机类型</param>
        /// <param name="algFlowTypeEnum">模板类型</param>
        public static void EditPrInSystem2(string name, CameraTypeEnum cameraType = CameraTypeEnum.BondCamera, AlgFlowTypeEnum algFlowTypeEnum = AlgFlowTypeEnum.XldModelAlg, AlgBeLongEnum algBeLong = AlgBeLongEnum.Substrate)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);

                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);

                prEntity.SetAlgFlowType(algFlowTypeEnum);

                prEntity.Alg.AlgBeLong = algBeLong;

                System2Domain.GetInstance().System2Controller.SetDefaultHardwareParameter(prEntity, cameraType);
            }

            prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(cameraType));

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="cameraType">相机类型</param>
        public static void EditPrInSystem1(string name)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);

                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Substrate;


                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            prEntity.SetHardware(System1Domain.GetInstance().DispenseVisionController.GetHardware());

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }


        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="point3D">点位</param>
        public static void MoveToPos(AKRSPoint3D point3D)
        {
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                System2Domain.GetInstance().BondModuleController.MoveToG0Pos(point3D);
            }
            else
            {
                System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(point3D);
            }
        }

        /// <summary>
        /// 在当前位置进行测高
        /// 外界在调用此方法之前需要保证已经到达测高的位置
        /// 返回的结果为G0中的结果
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="closeCy">是否收回点胶针</param>
        /// <returns>结果</returns>
        public static (ExcuteResult, double) AssistantMeasureHeight(
            MultipleHeightMeasurementType type,
            bool closeCy = true)
        {
            ExcuteResult result;
            double height;

            DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return (ExcuteResult.Success, 0);
            }

            // 执行测高
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                if (type == MultipleHeightMeasurementType.TouchDown)
                {
                    return dispenseMeasureHeightController.DispenserHeightMeasurementG0(
                        System1MeasHeightToolEnum.HeightSensor,
                        null,
                        double.NaN,
                        5,
                        closeCy);
                }
                else
                {
                    AKRSPoint3D point3DPos = System1Domain.GetInstance().DispenseController.GetG0Pos();
                    AKRSPoint3D point3D = System1Domain.GetInstance().DispenseController.GetG0PosFromVision(point3DPos);

                    point3D.Z += 2;
                    (result, height) = dispenseMeasureHeightController.DispenserHeightMeasurementG0(
                        System1MeasHeightToolEnum.HeightSensor,
                        point3D,
                        double.NaN,
                        5,
                        closeCy);

                    System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(point3DPos);

                    return (ExcuteResult.Success, height);
                }
            }
            else
            {
                if (type == MultipleHeightMeasurementType.TouchDown)
                {
                    // AKRSPoint3D currentPoint3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

                    double left = System2Module.GetInstance().BondModule.BondHead.AxisZ.GetRealPosition();
                    (result, height) = System2Domain.GetInstance().BondHeadController.MeasureHeight(
                        left,
                        HeightMeasurementFunctionEnum.WithTDSensor);

                    // 转到G0
                    height = System2Module.GetInstance().BondModule.ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, height))
                        .Z;

                    System2Domain.GetInstance().BondHeadController.MoveBondZToSafePos();

                    // System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(currentPoint3D);
                    return (result, height);
                }
                else
                {
                    // 获取当前位置，测高完成之后，回到当前位置
                    AKRSPoint3D currentPoint3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

                    // 移动到视觉所看到的位置
                    System2Domain.GetInstance().BondModuleController.MoveToVisionPos(5);
                    double left = System2Module.GetInstance().BondModule.BondHead.AxisZ.GetRealPosition();
                    (result, height) = System2Domain.GetInstance().BondHeadController.MeasureHeight(
                        left,
                        HeightMeasurementFunctionEnum.WithTDSensor);

                    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(currentPoint3D);

                    // 转到G0
                    height = System2Module.GetInstance().BondModule.ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, height))
                        .Z;
                    return (result, height);
                }
            }
        }

        /// <summary>
        /// 判断当前载台上是否存在着料
        /// </summary>
        /// <returns>结果</returns>
        public static TransportUnit JudgeTuExist()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return new TransportUnit("示教");
            }

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit == null)
                {
                    AKRSXtraMessageBox.Show("固晶载台记忆中没有产品，如果有产品请重新搜索轨道");
                }
                else
                {
                    if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit
                        .TransportUnitInfo.IsProduct)
                    {
                        DialogResult dialogResult = AKRSXtraMessageBox.Show(
                            "载台上的产品已经有生产记录，如果继续将会清空记录",
                            "警告",
                            MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            return null;
                        }
                    }

                    TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit =
                        new TransportUnit(CurrentMachineSystemEnum.System2);
                }

                return TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;
            }
            else if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                if (TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit == null)
                {
                    AKRSXtraMessageBox.Show("点胶载台记忆中没有产品，如果有产品请重新搜索轨道");
                }
                else
                {
                    if (TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit
                        .TransportUnitInfo.IsProduct)
                    {
                        DialogResult dialogResult = AKRSXtraMessageBox.Show(
                            "载台上的产品已经有生产记录，如果继续将会清空记录",
                            "警告",
                            MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.No)
                        {
                            return null;
                        }
                    }

                    TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit =
                        new TransportUnit(CurrentMachineSystemEnum.System1);
                }

                return TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit;
            }

            return null;
        }

        /// <summary>
        /// 根据系统返回差值
        /// </summary>
        /// <param name="generalCoordinateSystem">坐标系</param>
        /// <param name="type">类型</param>
        /// <returns>结果</returns>
        public static AKRSPoint3D GetPosBySystem(
            GeneralCoordinateSystem generalCoordinateSystem,
            MultipleHeightMeasurementType type = MultipleHeightMeasurementType.SubstrateCamera)
        {
            AKRSPoint3D point3D;

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return System1Domain.GetInstance().DispenseController.GetG0Pos();
            }

            // 系统2
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                point3D = System2Module.GetInstance().BondModule.GetG0RealPosition();

                // 如果是相机的话，需要加上相机和焊头的差值
                if (type == MultipleHeightMeasurementType.SubstrateCamera)
                {
                    point3D = point3D + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                }
            }
            else
            {
                point3D = System1Domain.GetInstance().DispenseController.GetG0Pos();

                // 如果是相机的话，需要加上相机和测高针的差值
                if (type == MultipleHeightMeasurementType.SubstrateCamera)
                {
                    point3D = System1Domain.GetInstance().DispenseController.GetG0PosFromVision(point3D);
                }
            }

            if (generalCoordinateSystem != null)
            {
                point3D = generalCoordinateSystem.G0PosToSelf(point3D);
            }

            return point3D;
        }

        /// <summary>
        /// 根据实体获取配置
        /// </summary>
        /// <param name="entityTypeEnum">实体类型</param>
        /// <returns>实体的配置文件</returns>
        public static BaseConfig SetConfig(EntityTypeEnum entityTypeEnum)
        {
            if (entityTypeEnum == EntityTypeEnum.TransportUnit)
            {
                return ProductConfiguration.GetInstance().TransportUnitConfig;
            }
            else if (entityTypeEnum == EntityTypeEnum.Substrate)
            {
                return ProductConfiguration.GetInstance().SubstrateConfig;
            }
            else if (entityTypeEnum == EntityTypeEnum.Module)
            {
                return ProductConfiguration.GetInstance().ModuleConfig;
            }

            return null;
        }

        /// <summary>
        /// 选择在哪个实体里面做流程
        /// </summary>
        /// <param name="entityTypeEnum">实体类型</param>
        /// <param name="singleBondPositionConfig">焊点配置对象</param>
        /// <returns>实体的坐标系</returns>
        public static GeneralCoordinateSystem SetCoordinateSystem(
            EntityTypeEnum entityTypeEnum,
            SingleBondPositionConfig singleBondPositionConfig)
        {
            if (entityTypeEnum == EntityTypeEnum.TransportUnit)
            {
                if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
                {
                    return TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit.CoordinateSystem;
                }
                else
                {
                    return TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit.CoordinateSystem;
                }
            }

            FrmModuleSelect frmModuleSelect = new FrmModuleSelect(entityTypeEnum);
            if (frmModuleSelect.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (entityTypeEnum == EntityTypeEnum.Substrate)
                {
                    return frmModuleSelect.CurrentSubstrate.CoordinateSystem;
                }
                else if (entityTypeEnum == EntityTypeEnum.Module)
                {
                    return frmModuleSelect.CurrentModule.CoordinateSystem;
                }
            }
            else
            {
                return null;
            }

            return null;
        }

        /// <summary>
        /// 更新坐标系
        /// </summary>
        /// <param name="entityTypeEnum">实体类型</param>
        /// <param name="degree">角度</param>
        public static void UpdateDegree(EntityTypeEnum entityTypeEnum, double degree)
        {
            if (entityTypeEnum == EntityTypeEnum.TransportUnit)
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.ElementCoordinate.Degree = degree;
            }
            else if (entityTypeEnum == EntityTypeEnum.Substrate)
            {
                foreach (ElementCoordinate item in
                         ProductConfiguration.GetInstance().SubstrateConfig.ElementCoordinates)
                {
                    item.Degree = degree;
                }
            }
            else if (entityTypeEnum == EntityTypeEnum.Module)
            {
                foreach (ElementCoordinate item in ProductConfiguration.GetInstance().ModuleConfig.ElementCoordinates)
                {
                    item.Degree = degree;
                }
            }
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">按钮</param>
        /// <param name="form">窗体</param>
        public static void AutoFocus(object sender, Form form)
        {
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                System1Domain.GetInstance().DispenseController.AutoFocusAssistance(sender, form);
            }
            else
            {
                System2Domain.GetInstance().BondHeadController.AutoFocusAssistance(
                    BondSystem.Models.Enums.CameraTypeEnum.BondCamera,
                    sender,
                    form);
            }
        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <param name="tileBar">选项框</param>
        public static void SetColor(TileBar tileBar)
        {
            tileBar.AppearanceItem.Normal.BackColor = System.Drawing.Color.Gray;
            tileBar.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.Gray;
            tileBar.AppearanceItem.Normal.Options.UseBackColor = true;
            tileBar.AppearanceItem.Selected.BackColor = System.Drawing.Color.LightGreen;
            tileBar.AppearanceItem.Selected.BackColor2 = System.Drawing.Color.LightGreen;
            tileBar.AppearanceItem.Selected.Options.UseBackColor = true;
            tileBar.Dock = System.Windows.Forms.DockStyle.Top;
            tileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileBar.SelectedItemChanged += SelectedItemChanged;
            SelectedItemChanged(tileBar, new TileItemEventArgs());
        }

        /// <summary>
        /// 当选项发生改变的时候
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        public static void SelectedItemChanged(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            TileBar tileBar = (TileBar)sender;

            // 获取当前选中bar的索引
            TileItem currentItem = tileBar.SelectedItem;

            if (currentItem == null)
            {
                return;
            }
        }
        
        /// <summary>
        /// 系统2产品定位
        /// </summary>
        /// <param name="baseEntity">实体</param>
        /// <returns>结果</returns>
        public static bool System2VisionByPos(BaseMatter baseEntity)
        {
            TransportUnit transportUnit = TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit;

            if (baseEntity is Substrate)
            {
                Substrate substrate = (Substrate)baseEntity;

                if (!System2Domain.GetInstance().System2MatterVision(transportUnit, false))
                {
                    return false;
                }

                if (!System2Domain.GetInstance().System2MatterVision(substrate, false))
                {
                    return false;
                }
            }
            else if (baseEntity is Module)
            {
                Module module = (Module)baseEntity;

                if (!System2Domain.GetInstance().System2MatterVision(transportUnit, false))
                {
                    return false;
                }

                Substrate substrate = transportUnit.Substrates.Find(it => it.Index == module.SubstrateNum);

                if (!System2Domain.GetInstance().System2MatterVision(substrate, false))
                {
                    return false;
                }

                if (!System2Domain.GetInstance().System2MatterVision(module, false))
                {
                    return false;
                }
            }
            else if (baseEntity is BondPosition)
            {
                BondPosition bondPosition = (BondPosition)baseEntity;

                if (!System2Domain.GetInstance().System2MatterVision(transportUnit, false))
                {
                    return false;
                }

                Substrate substrate = transportUnit.Substrates.Find(it => it.Index == bondPosition.SubstrateNum);

                if (!System2Domain.GetInstance().System2MatterVision(substrate, false))
                {
                    return false;
                }

                Module module = substrate.Modules.Find(it => it.Index == bondPosition.ModuleNum);

                if (!System2Domain.GetInstance().System2MatterVision(module, false))
                {
                    return false;
                }

                if (!System2Domain.GetInstance().System2MatterVision(bondPosition, false))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 系统1产品定位
        /// </summary>
        /// <param name="baseEntity">实体</param>
        /// <returns>结果</returns>
        public static bool System1VisionByPos(BaseMatter baseEntity)
        {
            TransportUnit transportUnit = TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit;

            if (baseEntity is Substrate)
            {
                Substrate substrate = (Substrate)baseEntity;

                if (!System1Domain.GetInstance().System1MatterVision(transportUnit, false))
                {
                    return false;
                }

                if (!System1Domain.GetInstance().System1MatterVision(substrate, false))
                {
                    return false;
                }
            }
            else if (baseEntity is Module)
            {
                Module module = (Module)baseEntity;

                if (!System1Domain.GetInstance().System1MatterVision(transportUnit, false))
                {
                    return false;
                }

                Substrate substrate = transportUnit.Substrates.Find(it => it.Index == module.SubstrateNum);

                if (!System1Domain.GetInstance().System1MatterVision(substrate, false))
                {
                    return false;
                }

                if (!System1Domain.GetInstance().System1MatterVision(module, false))
                {
                    return false;
                }
            }
            else if (baseEntity is BondPosition)
            {
                BondPosition bondPosition = (BondPosition)baseEntity;

                if (!System1Domain.GetInstance().System1MatterVision(transportUnit, false))
                {
                    return false;
                }

                Substrate substrate = transportUnit.Substrates.Find(it => it.Index == bondPosition.SubstrateNum);

                if (!System1Domain.GetInstance().System1MatterVision(substrate, false))
                {
                    return false;
                }

                Module module = substrate.Modules.Find(it => it.Index == bondPosition.ModuleNum);

                if (!System1Domain.GetInstance().System1MatterVision(module, false))
                {
                    return false;
                }

                if (!System1Domain.GetInstance().System1MatterVision(bondPosition, false))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
