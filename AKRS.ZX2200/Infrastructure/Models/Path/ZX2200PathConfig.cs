#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/10/12 17:05:03
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

namespace AKRS.ZX2200.Infrastructure.Models.Path
{
    using System.IO;

    using AKRS.Galaxy2.MachineSupport.Config;

    /// <summary>
    /// 描述：路径设置
    /// </summary>
    public class ZX2200PathConfig : PathConfig
    {
        /// <summary>
        /// 设备
        /// </summary>
        public static string MachineFilePath => Path.Combine(PathConfig.RecipeDirPath, "Machine.json");

        /// <summary>
        /// 设备参数存储路径
        /// </summary>
        public static string MachineDeviceParaPath => Path.Combine(PathConfig.DeviceDirPath, "MachineDevicePara.json");

        #region 基板流道域
        /// <summary>
        /// InOutPutSettingRepositoryPath仓库存储路径
        /// </summary>
        public static string InOutPutSettingRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "InOutPutSettingRepositoryFilePath.json");

        /// <summary>
        /// TransportBeltSetting仓库存储路径
        /// </summary>
        public static string TransportBeltSettingRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "TransportBeltSettingRepository.json");

        /// <summary>
        /// TransportBeltSetting仓库存储路径
        /// </summary>
        public static string BondinsertSettingRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "BondinsertSettingRepository.json");

        /// <summary>
        /// 上料盒仓库存储路径
        /// </summary>
        public static string LoaderBinRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "LoaderBinRepository.json");

        /// <summary>
        /// 下料盒仓库存储路径
        /// </summary>
        public static string UnLoaderBinRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "UnLoaderBinRepository.json");

        /// <summary>
        /// 流道程式路径
        /// </summary>
        public static string TransportSystemProgramFilePath => Path.Combine(PathConfig.RecipeDirPath, "TransportSystemProgram.json");

        /// <summary>
        /// 基板程式存储路径
        /// </summary>
        // public static string SubstrateDomainFilePath => Path.Combine(PathConfig.RecipeDirPath, "SubstrateDomain.json");

        /// <summary>
        /// WafrSubDeviceParaPath
        /// </summary>
        public static string TransportDeviceParaPath => Path.Combine(PathConfig.DeviceDirPath, "TransportDeviceParaPath.json");

        #endregion

        #region 点胶域

        /// <summary>
        /// 点胶程式的存储路径
        /// </summary>
        public static string DispenseProgramPath => Path.Combine(PathConfig.RecipeDirPath, "DispenseProgram.json");

        /// <summary>
        /// 点胶设备文件的存储路径
        /// </summary>
        public static string DispenseDevicePath => Path.Combine(PathConfig.DeviceDirPath, "DispenseDevice.json");

        /// <summary>
        /// DispenserRepository仓库存储路径
        /// </summary>
        public static string DispenserRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "DispenserRepository.json");

        /// <summary>
        /// PatternRepository仓库存储路径
        /// </summary>
        public static string EpoxyApplicationRepositoryPath => Path.Combine(PathConfig.DeviceDirPath, "EpoxyApplicationRepository.json");

        /// <summary>
        /// Material仓库存储路径
        /// </summary>
        public static string EpoxyMaterialRepositoryPath => Path.Combine(PathConfig.DeviceDirPath, "EpoxyMaterialRepository.json");
        
        /// <summary>
        /// 点胶域参数
        /// </summary>
        //public static string DispenseCorrectProgramPath => Path.Combine(PathConfig.RecipeDirPath, "DispenseCorrectProgram.json");
        #endregion

        #region 固晶域

        /// <summary>
        /// 吸嘴架模板仓库存储路径
        /// </summary>
        public static string NozzleShelfRepositoryPath => Path.Combine(PathConfig.DeviceDirPath, "NozzleShelfRepositoryPath.json");

        /// <summary>
        /// 吸嘴模板仓库存储路径
        /// </summary>
        public static string NozzleRepositoryPath => Path.Combine(PathConfig.DeviceDirPath, "NozzleRepositoryPath.json");

        /// <summary>
        /// 固晶程式参数存储路径
        /// </summary>
        public static string BondRecipeParaPath => Path.Combine(PathConfig.DeviceDirPath, "BondRecipeParaPath.json");

        /// <summary>
        /// 固晶设备参数存储路径
        /// </summary>
        public static string BondDeviceParaPath => Path.Combine(PathConfig.DeviceDirPath, "BondDeviceParaPath.json");

        /// <summary>
        /// 力控配置对象存储路径（Json）
        /// </summary>
        public static string ForceConfig => Path.Combine(PathConfig.DeviceDirPath, "ForceConfig.json");

        /// <summary>
        /// Rtu连接配置对象存储路径
        /// </summary>
        public static string RtuConnectConfig => Path.Combine(PathConfig.DeviceDirPath, "RtuConnectConfig.json");

        /// <summary>
        /// 力控标定数据存储路径
        /// </summary>
        public static string ForceCalibrationStorageFilePath => Path.Combine(PathConfig.DeviceDirPath, "ForceCalibrationDataFilePath", "ForceCalibrationData.json");

        /// <summary>
        /// 固晶程式存储路径
        /// </summary>
        public static string BondProgramFilePath => Path.Combine(PathConfig.RecipeDirPath, "BondProgram.json");

        /// <summary>
        /// 产品配置存储路径
        /// </summary>
        public static string ProductDomainFilePath => Path.Combine(PathConfig.RecipeDirPath, "ProductDomain.json");

        /// <summary>
        /// ProcessStep程式
        /// </summary>
        public static string ProcessStepProgramFilePath => Path.Combine(PathConfig.RecipeDirPath, "ProcessStepProgram.json");

        /// <summary>
        /// 程式
        /// </summary>
        public static string ProcessProgramFilePath => Path.Combine(PathConfig.RecipeDirPath, "ProcessProgram.json");

        #endregion

        #region 上晶圆域

        /// <summary>
        /// WaferSubDevicePara
        /// </summary>
        public static string WaferSubDevicePara => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "WaferSubDevicePara.json");

        /// <summary>
        /// WaferSystemProgram
        /// </summary>
        public static string WaferSystemProgram => Path.Combine(PathConfig.RecipeDirPath, "WaferSystemProgram.json");

        /// <summary>
        /// WaferTableDeviceParas
        /// </summary>
        public static string MagazineBoxGeoRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "MagazineBoxGeoRepository.json");

        /// <summary>
        /// MagazineBoxPath
        /// </summary>
        public static string MagazineBoxRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "MagazineBoxRepository.json");

        /// <summary>
        /// MagazineBoxPath
        /// </summary>
        public static string CarrierRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "CarrierRepository.json");

        /// <summary>
        /// MagazineBoxPath
        /// </summary>
        public static string MagazineAllocationsRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "MagazineAllocationsRepository.json");      

        /// <summary>
        /// MagazineBoxPath
        /// </summary>
        public static string EjectionBankRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "EjectionBankRepository.json");

        /// <summary>
        /// 翻转工具库
        /// </summary>
        public static string FlipToolRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "FlipToolRepository.json");

        /// <summary>
        /// MagazineBoxPath
        /// </summary>
        public static string AdapterRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "AdapterRepository.json");

        /// <summary>
        /// MagazineBoxPath
        /// </summary>
        public static string EjectionRepository => Path.Combine(PathConfig.DeviceDirPath, "WaferSub", "EjectionRepository.json");

        #endregion

        #region 标定
        /// <summary>
        /// CalibratePara
        /// </summary>
        public static string CalibratePara => Path.Combine(PathConfig.DeviceDirPath, "CalibratePara.json");

        /// <summary>
        /// LightCalibratePara
        /// </summary>
        public static string LightCalibratePara => Path.Combine(PathConfig.DeviceDirPath, "LightCalibratePara.json");

        /// <summary>
        /// LightCalibratePara
        /// </summary>
        public static string WaferCameraAndEjectorCalibratePara => Path.Combine(PathConfig.DeviceDirPath, "WaferCameraAndEjectorCalibratePara.json");

        /// <summary>
        /// MachineCoordinateSystem
        /// </summary>
        public static string MachineCoordinateSystem => Path.Combine(PathConfig.DeviceDirPath, "MachineCoordinateSystem", "MachineCoordinateSystem.json");

        /// <summary>
        /// 标定流程
        /// </summary>
        public static string CalibrationProcess => Path.Combine(PathConfig.DeviceDirPath, "MachineCoordinateSystem", "CalibrationProcess.sol");

        /// <summary>
        /// 转换流程
        /// </summary>
        public static string TransformProcess => Path.Combine(PathConfig.DeviceDirPath, "MachineCoordinateSystem", "TransformProcess.sol");

        /// <summary>
        /// BCOffsetRelationCalibration
        /// </summary>
        public static string BCOffsetRelationCalibration => Path.Combine(PathConfig.DeviceDirPath, "MachineCoordinateSystem", "BCOffsetRelationCalibration.json");

        #endregion

        #region 补偿

        /// <summary>
        /// CalibratePara
        /// </summary>
        public static string Compensate => Path.Combine(PathConfig.DeviceDirPath, "Compensate.json");

        #endregion

        #region

        /// <summary> 通用
        /// ProcessStepProgram
        /// </summary>
        public static string ProcessStepPool => Path.Combine(PathConfig.DeviceDirPath, "ProcessStepPool.json");

        /// <summary>
        /// ProcessControler
        /// </summary>
        public static string ProcessControlerFilePath => Path.Combine(PathConfig.DeviceDirPath, "ProcessControler.json");

        /// <summary>
        /// ProcessControler
        /// </summary>
        public static string BondPositionRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "BondPositionRepository.json");

        /// <summary>
        /// PostBondInspectionRepository
        /// </summary>
        public static string PostBondInspectionRepositoryFilePath => Path.Combine(PathConfig.DeviceDirPath, "PostBondInspectionRepository.json");

        /// <summary>
        /// 设备硬件配置
        /// </summary>
        public static string MachineHardwareConfigurationPath => Path.Combine(PathConfig.DeviceDirPath, "MachineConfiguration.json");

        /// <summary>
        /// 设备软件配置
        /// </summary>
        public static string MachineSoftwareConfigurationPath => Path.Combine(PathConfig.DeviceDirPath, "MachineSoftwareConfiguration.json");

        /// <summary>
        /// 设备配置
        /// </summary>
        public static string StatisticsDomainPath => Path.Combine(PathConfig.RecipeDirPath, "StatisticsDomainPath.json");
        
        #endregion

        #region 检测
        /// <summary>
        /// BondLevelMeasure
        /// </summary>
        public static string BondLevelMeasureFilePath => Path.Combine(PathConfig.DeviceDirPath, "BondLevelMeasureFilePath.json");

        /// <summary>
        /// 系统2平面测量参数
        /// </summary>
        public static string System2FlatLevelMeasureFilePath => Path.Combine(PathConfig.DeviceDirPath, "System2FlatLevelMeasureFilePath.json");

        /// <summary>
        /// 系统1平面测量参数
        /// </summary>
        public static string System1FlatLevelMeasureFilePath => Path.Combine(PathConfig.DeviceDirPath, "System1FlatLevelMeasureFilePath.json");
        #endregion

        #region 全局标定

        /// <summary>
        /// 运动规划
        /// </summary>
        public static string MotionPlanDomainPath => Path.Combine(PathConfig.DeviceDirPath, "MotionPlanDomainPath.json");

        /// <summary>
        /// PatternRepository仓库存储路径
        /// </summary>
        public static string GlobalCalibrationDomainPath => Path.Combine(PathConfig.DeviceDirPath, "GlobalCalibrationDomain.json");

        /// <summary>
        /// PatternRepository仓库存储路径
        /// </summary>
        public static string BondCompensateCalibrationPath => Path.Combine(PathConfig.DeviceDirPath, "BondCompensateCalibration.json");

        /// <summary>
        /// PatternRepository仓库存储路径
        /// </summary>
        public static string GlobalCalibrationDomainPathEx => Path.Combine(PathConfig.DeviceDirPath, "GlobalCalibrationDomainEx.json");

        /// <summary>
        /// 贴片补偿路径
        /// </summary>
        public static string BondCompensatePath => Path.Combine(PathConfig.DeviceDirPath, "BondCompensate.json");

        /// <summary>
        /// 实时矫正
        /// </summary>
        public static string RealTimeCorrectionPath => Path.Combine(PathConfig.DeviceDirPath, "RealTimeCorrection.json");

        /// <summary>
        /// 贴片补偿路径
        /// </summary>
        public static string TransportUnitCompensatePath => Path.Combine(PathConfig.DeviceDirPath, "TransportUnitCompensate.json");

        /// <summary>
        /// 温漂补偿路径
        /// </summary>
        public static string TransportUnitDriftCompensateCSV => Path.Combine(PathConfig.DeviceDirPath, "温漂箱补偿值数据表.csv");

        /// <summary>
        /// 风冷参数存储路径
        /// </summary>
        public static string AirCooledControllerPath => Path.Combine(PathConfig.DeviceDirPath, "AirCooledController.json");


        #endregion
    }
}
