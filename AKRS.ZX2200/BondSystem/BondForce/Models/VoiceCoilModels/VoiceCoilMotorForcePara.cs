using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.BondForce.Models.VoiceCoilModels
{
    /// <summary>
    /// 音圈电机力控参数
    /// </summary>
    [Serializable]
    public class VoiceCoilMotorForceCalibrationPara
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static VoiceCoilMotorForceCalibrationPara instance;

        /// <summary>
        /// 单例模式
        /// </summary>
        /// <returns>单例</returns>
        public static VoiceCoilMotorForceCalibrationPara GetInstance()
        {
            if (instance == null)
            {
                if (DirAndFileHelper.IsExistFile(FilePath))
                {
                    try
                    {
                        instance = JsonFormatHelper<VoiceCoilMotorForceCalibrationPara>.ReadGenericObject(FilePath);
                    }
                    catch (Exception ex)
                    {
                        AKRSXtraMessageBox.Show(@"读取MachineConfig.json出错：" + ex.Message, @"错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    instance = new VoiceCoilMotorForceCalibrationPara();

                    // 初始化数据
                    if (instance.ForceTorqueList.Count == 0)
                    {
                        for (int torque = -170; torque <= 110; torque = torque + 10)
                        {
                            instance.ForceTorqueList.Add(new ForceTorque() { Torque = torque, Force = 0.0 });
                        }
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// 力控标定数据存储路径
        /// </summary>
        public static string FilePath => System.IO.Path.Combine(PathConfig.DeviceDirPath, "VoiceCoilCalibrationStorageFilePath", "VoiceCoilCalibrationStorageFilePath.json");

        /// <summary>
        /// 补偿值
        /// </summary>
        public double CompensationValue { get; set; } = -0.5;

        /// <summary>
        /// 力和转矩对象存储
        /// </summary>
        public List<ForceTorque> ForceTorqueList { get; set; } = new List<ForceTorque>();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="forceCalibrationPara">标定</param>
        /// <param name="filePath">文件路径</param>
        public void Save(VoiceCoilMotorForceCalibrationPara forceCalibrationPara, string filePath)
        {
            JsonFormatHelper<VoiceCoilMotorForceCalibrationPara>.SaveGenericObject(forceCalibrationPara, filePath);
        }
    }
}
