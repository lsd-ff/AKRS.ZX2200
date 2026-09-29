namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using System.Drawing;
    using System.Linq;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.Product;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.Enums;
    using DevExpress.XtraBars;

    /// <summary>
    /// 根据设备状态，改变不同的状态
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// 设备状态
        /// </summary>
        private MachineStateEnum machineState = MachineStateEnum.Operate;

        /// <summary>
        /// 权限管理
        /// </summary>
        private RoleEnum role = RoleEnum.Operator;

        /// <summary>
        /// 当设备变成运行状态时，UI的状态
        /// </summary>
        private void ChangeRunningUiState()
        {
            this.BtLogon.Enabled = true;
            this.BtLoadProduct.Enabled = false;
            this.BtInitializeTS.Enabled = false;
            this.BtStart.Enabled = false;
            this.BtContinue.Enabled = false;
            this.BtOffsetAdd.Enabled = false;
           
            this.BtEmptyIndex.Enabled = true;
            this.BtDataPage.Enabled = false;
            this.BtVideoWindow.Enabled = true;
            this.BtLampRegulation.Enabled = false;
            this.BtManipulator.Enabled = false;
       
            this.BtLock.Enabled = false;
            this.BtMoveToBp.Enabled = false;
            this.BtVisionLastBp.Enabled = true;
            this.BtVisionLastDispense.Enabled = true;
            this.BtShowWaferMap.Enabled = false;
            this.BtSlotState.Enabled = true;
            this.BtRemoveAlarm.Enabled = true;
            this.BarBtLoad.Enabled = false;
            this.BarBtRecipeManage.Enabled = false;
            this.BarBtShutDown.Enabled = false;
            this.BarBtStart.Enabled = false;
            this.BarBtContinue.Enabled = false;
            this.BarBtProductionMode.Enabled = false;
            this.BtTUMapping.Enabled = false;
            this.barButtonItem31.Enabled = false;
            this.BtNewProduct.Enabled = false;
            this.BtEditProduct.Enabled = false;
            this.BsiToolReference.Enabled = false;
            this.BsiToolOptimization.Enabled = false;
            this.BtComponentPositionSearch.Enabled = false;
            this.BsiParameters.Enabled = false;
            this.BsiRecognition.Enabled = false;
            this.BtProductInformation.Enabled = false;

            this.BarBtManipulator.Enabled = false;
            this.BarItemVideo.Enabled = false;
            this.BarBtToolBank.Enabled = false;
            this.BarBtComponentHanding.Enabled = false;
            this.BtDispenseManual.Enabled = false;
            this.BarBtMeasureTool.Enabled = false;
            this.BarBtVacuum.Enabled = true;
            this.BarBtS2Dispense.Enabled = false;
            this.BarBtBondForceMeasure.Enabled = false;

            this.BarBtInitialize.Enabled = false;
            this.BarBtInitializeTS.Enabled = false;
            this.BtEmergencyStop.Enabled = false;
            this.BtCustomerSetup.Enabled = false;
            this.BartSubItemAutoCalibration.Enabled = false;
            this.BtnBrightnessSetting.Enabled = false;
            this.barButtonItem14.Enabled = false;
            this.BtMachineCoordinate.Enabled = false;
            this.BarBtSignalDisplay.Enabled = false;


            this.BarBtBMCTest.Enabled = false;
            this.BarBtUplookSimulateBondTest.Enabled = false;
            this.BarBtUpLookMarkTest.Enabled = false;
            this.barSubItem2.Enabled = false;
            this.BtnUserManager.Enabled = false;

            this.BtCancel.Enabled = true;
            this.BarBtCancel.Enabled = true;
            this.BtContinue.Enabled = false;
            this.BarBtContinue.Enabled = false;
            this.BtPause.Enabled = true;
            this.BarBtPause.Enabled = true;
            this.BtCurrentRecipePRList.Enabled = false;
            this.BarBtLogon.Enabled = false;
            this.BtSlotState.Enabled = true;

            this.BarBtTransportSystem.Enabled = false;
            this.BtShowWaferMap.Enabled = true;
            this.BtInitialize.Enabled = false;
            this.BarHeatingSettings.Enabled = false;

            this.barBtnHardWareList.Enabled = false;
            this.barBtnHardwareConfig.Enabled = false;
            this.BarAuthorization.Enabled = false;
            this.BtSoftWareInfo.Enabled = false;
            this.barButtonItem8.Enabled = false;
            this.BtnAlarmHistoryQuery.Enabled = false;
            this.BtUnLock.Enabled = false;
            this.BtTemperatureMarkAdjust.Enabled = false;
            this.BarPickUpMarkAdjust.Enabled = false;
            this.BarAssistantRuler.Enabled = false;
            this.barBtnLightCalib.Enabled = false;
            this.BtGlobalCalibration.Enabled = false;
            this.barBtnWaferCameraCalib.Enabled = false;
            this.ribbonPageGroup44.Enabled = false;
            this.BarBtTransportSystemState.Enabled = false;
            this.BarBtLoaderManual.Enabled = false;
            this.BtnAutoSlideFlux.Enabled = false;
            this.BtClearBpOffSet.Enabled = false;
            this.BarBtnFileCleanConfig.Enabled = false;
            this.BarBondCompensate.Enabled = false;

            // 连续上料时的判断
            if (TransportProvider.ContinuousFeeding == false)
            {
                this.BtEmptyIndex.ItemAppearance.Normal.BackColor = Color.Red;
            }
            else
            {
                this.BtEmptyIndex.ItemAppearance.Normal.BackColor = Color.LightGreen;
            }
        }

        /// <summary>
        /// 当设备变成停止状态时，UI的状态
        /// </summary>
        private void ChangeStopUiState()
        {
            this.BtLogon.Enabled = true;
            this.BtLoadProduct.Enabled = true;
            this.BtInitializeTS.Enabled = true;
            this.BtStart.Enabled = true;
            this.BtContinue.Enabled = true;
            this.BtOffsetAdd.Enabled = true;
           
            this.BtEmptyIndex.Enabled = true;
            this.BtDataPage.Enabled = true;
            this.BtVideoWindow.Enabled = true;
            this.BtLampRegulation.Enabled = true;
            this.BtManipulator.Enabled = true;
           
            this.BtLock.Enabled = true;
            this.BtMoveToBp.Enabled = true;
            this.BtVisionLastBp.Enabled = false;
            this.BtVisionLastDispense.Enabled = false;
            this.BtShowWaferMap.Enabled = true;
            this.BtSlotState.Enabled = true;
            this.BtRemoveAlarm.Enabled = true;
            this.BarBtLoad.Enabled = true;
            this.BarBtRecipeManage.Enabled = true;
            this.BarBtShutDown.Enabled = true;
            this.BarBtStart.Enabled = true;
            this.BarBtContinue.Enabled = true;
            this.BarBtProductionMode.Enabled = true;
            this.BtTUMapping.Enabled = true;
            this.barButtonItem31.Enabled = true;
            this.BtNewProduct.Enabled = true;
            this.BtEditProduct.Enabled = true;
            this.BsiToolReference.Enabled = true;
            this.BsiToolOptimization.Enabled = true;
            this.BtComponentPositionSearch.Enabled = true;
            this.BsiParameters.Enabled = true;
            this.BsiRecognition.Enabled = true;
            this.BtProductInformation.Enabled = true;

            this.BarBtManipulator.Enabled = true;
            this.BarItemVideo.Enabled = true;
            this.BarBtToolBank.Enabled = true;
            this.BarBtComponentHanding.Enabled = true;
            this.BtDispenseManual.Enabled = true;
            this.BarBtMeasureTool.Enabled = true;
            this.BarBtVacuum.Enabled = true;
            this.BarBtS2Dispense.Enabled = true;
            this.BarBtBondForceMeasure.Enabled = true;

            this.BarBtInitialize.Enabled = true;
            this.BarBtInitializeTS.Enabled = true;
            this.BtEmergencyStop.Enabled = true;
            this.BtCustomerSetup.Enabled = true;
            this.BartSubItemAutoCalibration.Enabled = true;
            this.BtnBrightnessSetting.Enabled = true;
            this.barButtonItem14.Enabled = true;
            this.BtMachineCoordinate.Enabled = true;
            this.BarBtSignalDisplay.Enabled = true;

            this.BarBtBMCTest.Enabled = true;
            this.BarBtUplookSimulateBondTest.Enabled = true;
            this.BarBtUpLookMarkTest.Enabled = true;
            this.barSubItem2.Enabled = true;
            this.BtnUserManager.Enabled = true;

            this.BtCancel.Enabled = false;
            this.BarBtCancel.Enabled = false;
            this.BtContinue.Enabled = false;
            this.BarBtContinue.Enabled = false;
            this.BtPause.Enabled = false;
            this.BarBtPause.Enabled = false;
            this.BtCurrentRecipePRList.Enabled = true;
            this.BarBtLogon.Enabled = true;
            this.BarBtTransportSystem.Enabled = true;
            this.BtSlotState.Enabled = true;
            this.BtInitialize.Enabled = true;


            this.barBtnHardWareList.Enabled = true;
            this.barBtnHardwareConfig.Enabled = true;
            this.BarAuthorization.Enabled = true;
            this.BtSoftWareInfo.Enabled = true;
            this.barButtonItem8.Enabled = true;
            this.BtnAlarmHistoryQuery.Enabled = true;
            this.BtUnLock.Enabled = true;
            this.BtTemperatureMarkAdjust.Enabled = true;
            this.BarPickUpMarkAdjust.Enabled = true;
            this.BarAssistantRuler.Enabled = true;
            this.barBtnLightCalib.Enabled = true;
            this.BtGlobalCalibration.Enabled = true;
            this.barBtnWaferCameraCalib.Enabled = true;
            this.ribbonPageGroup44.Enabled = true;
            this.BarBtTransportSystemState.Enabled = true;
            this.BarBtLoaderManual.Enabled = true;
            this.BtnAutoSlideFlux.Enabled = true;
            this.BarHeatingSettings.Enabled = true;
            this.BtClearBpOffSet.Enabled = true;
            this.BarBtnFileCleanConfig.Enabled = true;
            this.BarBondCompensate.Enabled = true;

            this.BtnAutoSlideFlux.Enabled = MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured;
        }

        /// <summary>
        /// 当设备变成暂停状态时，UI的状态
        /// </summary>
        private void ChangePauseUiState()
        {
            this.BtLogon.Enabled = true;
            this.BtLoadProduct.Enabled = false;
            this.BtInitializeTS.Enabled = false;
            this.BtStart.Enabled = false;
            this.BtContinue.Enabled = false;
            this.BtOffsetAdd.Enabled = false;
          
            this.BtEmptyIndex.Enabled = true;
            this.BtDataPage.Enabled = true;
            this.BtVideoWindow.Enabled = true;
            this.BtLampRegulation.Enabled = false;
         
            this.BtLock.Enabled = false;
            this.BtMoveToBp.Enabled = false;
            this.BtVisionLastBp.Enabled = false;
            this.BtVisionLastDispense.Enabled = false;
            this.BtShowWaferMap.Enabled = false;
            this.BtSlotState.Enabled = true;
            this.BtRemoveAlarm.Enabled = true;
            this.BarBtLoad.Enabled = false;
            this.BarBtRecipeManage.Enabled = false;
            this.BarBtShutDown.Enabled = false;
            this.BarBtStart.Enabled = false;
            this.BarBtContinue.Enabled = false;
            this.BarBtProductionMode.Enabled = false;
            this.BtTUMapping.Enabled = false;
            this.barButtonItem31.Enabled = false;
            this.BtNewProduct.Enabled = false;
            this.BtEditProduct.Enabled = false;
            this.BsiToolReference.Enabled = false;
            this.BsiToolOptimization.Enabled = false;
            this.BtComponentPositionSearch.Enabled = false;
            this.BsiParameters.Enabled = false;
            this.BsiRecognition.Enabled = false;
            this.BtProductInformation.Enabled = false;

            this.BarItemVideo.Enabled = false;
            this.BarBtToolBank.Enabled = false;
            this.BarBtComponentHanding.Enabled = false;
            this.BtDispenseManual.Enabled = false;
            this.BarBtMeasureTool.Enabled = false;
            this.BarBtVacuum.Enabled = true;
            this.BarBtS2Dispense.Enabled = false;
            this.BarBtBondForceMeasure.Enabled = false;

            this.BarBtInitialize.Enabled = false;
            this.BarBtInitializeTS.Enabled = false;
            this.BtEmergencyStop.Enabled = false;
            this.BtCustomerSetup.Enabled = false;
            this.BartSubItemAutoCalibration.Enabled = false;
            this.BtnBrightnessSetting.Enabled = false;
            this.barButtonItem14.Enabled = false;
            this.BtMachineCoordinate.Enabled = false;
            this.BarBtSignalDisplay.Enabled = false;


            this.BarBtBMCTest.Enabled = false;
            this.BarBtUplookSimulateBondTest.Enabled = false;
            this.BarBtUpLookMarkTest.Enabled = false;
            this.barSubItem2.Enabled = false;
            this.BtnUserManager.Enabled = false;

            this.BtCancel.Enabled = true;
            this.BarBtCancel.Enabled = true;
            this.BtContinue.Enabled = true;
            this.BarBtContinue.Enabled = true;
            this.BtPause.Enabled = false;
            this.BarBtPause.Enabled = false;

            this.BtManipulator.Enabled = true;

            this.BtCurrentRecipePRList.Enabled = false;

            this.BarBtLogon.Enabled = false;

            this.BarBtTransportSystem.Enabled = false;

            this.BtSlotState.Enabled = true;

            this.BtShowWaferMap.Enabled = true;
            this.BarBtSignalDisplay.Enabled = true;

            this.BtOffsetAdd.Enabled = true;

            this.BtEmptyIndex.Enabled = true;
            this.BtInitialize.Enabled = false;

            this.BarHeatingSettings.Enabled = false;

            this.barBtnHardWareList.Enabled = false;
            this.barBtnHardwareConfig.Enabled = false;
            this.BarAuthorization.Enabled = false;
            this.BtSoftWareInfo.Enabled = false;
            this.barButtonItem8.Enabled = false;
            this.BtnAlarmHistoryQuery.Enabled = false;
            this.BtUnLock.Enabled = false;
            this.BtTemperatureMarkAdjust.Enabled = false;
            this.BarPickUpMarkAdjust.Enabled = false;
            this.BarAssistantRuler.Enabled = false;
            this.barBtnLightCalib.Enabled = false;
            this.BtGlobalCalibration.Enabled = false;
            this.barBtnWaferCameraCalib.Enabled = false;
            this.ribbonPageGroup44.Enabled = false;
            this.BarBtTransportSystemState.Enabled = false;
            this.BarBtLoaderManual.Enabled = false;
            this.BtnAutoSlideFlux.Enabled = false;
            this.BarHeatingSettings.Enabled = false;
            this.BtClearBpOffSet.Enabled = false;
            this.BarBtnFileCleanConfig.Enabled = false;
            this.BarBondCompensate.Enabled = false;
        }

        /// <summary>
        /// 当设备变成单步状态时，UI的状态
        /// </summary>
        private void ChangeSingleStepUiState()
        {
            this.BarSwitchSingleStep.Checked =
                MachineStateModel.GetInstance().IsSingleStepWork;
        }

        /// <summary>
        /// 根据设备改变
        /// </summary>
        public void ChangeUiByState()
        {
            if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Stop 
                && this.Enabled == false
                && this.LbTip.Text == "设备正在停止，请稍后" )
            {
                if (Machine.GetInstance().IsAllowTaskClosed())
                {
                    this.Enabled = true;
                    this.LbTip.Visible = false;
                }
                else
                {
                    return; 
                }
               
            }

            // 如果当前状态和之前记忆的状态不同，改变UI状态
            if (this.machineState != MachineStateModel.GetInstance().MachineState)
            {
                this.machineState = MachineStateModel.GetInstance().MachineState;
                if (this.machineState == MachineStateEnum.Working)
                {
                    this.ChangeRunningUiState();
                }
                else if (this.machineState == MachineStateEnum.Pause)
                {
                    this.ChangePauseUiState();
                }
                else if (this.machineState == MachineStateEnum.Stop)
                {
                    this.ChangeStopUiState();           
                }
            }

            this.ChangeSingleStepUiState();
            this.ChangeContinuousFeedingUiState();
        }

        /// <summary>
        /// 流道持续上料状态改变
        /// </summary>
        private void ChangeContinuousFeedingUiState()
        {
            this.BtEmptyIndex.ItemAppearance.Normal.BackColor = TransportProvider.ContinuousFeeding? Color.LightGreen : Color.Red;
        }


        /// <summary>
        /// 根据设备配置改变
        /// </summary>
        public void ChangeUiByConfiguration()
        {
            this.BtnSystem1Enable.Visible = MachineHardwareConfiguration.GetInstance().IsSystem1Configrated;

            this.BtUpTu.Visible = MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin;
            //TransportDomain.GetInstance().LoaderTask.Enable = this.BtUpTu.Visible;

            this.BtDownTu.Visible = MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin;
            //TransportDomain.GetInstance().UnloaderTask.Enable = this.BtDownTu.Visible;

            // this.BarHeatingSettings.Enabled = MachineHardwareConfiguration.GetInstance().IsTransportHeatConfigured;

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                this.BarItemCmdSystemType.EditValue = "System 1";
            }
            else 
            {
                this.BarItemCmdSystemType.EditValue = "System 2";
            }

            if (Machine.GetInstance().IsWorking() &&
              MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem2)
            {
                this.BtVisionLastBp.ItemAppearance.Normal.BackColor = Color.Red;
            }
            else
            {
                this.BtVisionLastBp.ItemAppearance.Normal.BackColor = Color.Green;
            }

            if (Machine.GetInstance().IsWorking() &&
              MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem1)
            {
                this.BtVisionLastDispense.ItemAppearance.Normal.BackColor = Color.Red;
            }
            else
            {
                this.BtVisionLastDispense.ItemAppearance.Normal.BackColor = Color.Green;
            }

            if (!MachineHardwareConfiguration.GetInstance().IsTransportHeatConfigured)
            {
                this.BarHeatingSettings.Visibility = BarItemVisibility.Never;
            }
            else
            {
                this.BarHeatingSettings.Visibility = BarItemVisibility.Always;
            }
            this.InitByConfiguration();
        }

        /// <summary>
        /// 屏蔽系统1
        /// </summary>
        private void InitByConfiguration()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.BtDispenseManual.Visibility = BarItemVisibility.Never;
                this.BarItemCmdSystemType.Visibility = BarItemVisibility.Never;
                this.BtnSystem1Enable.Visible = false;
            }

            if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense && !MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool)
            {
                this.BarBtS2Dispense.Visibility = BarItemVisibility.Never;
            }
            else
            {
                this.BarBtS2Dispense.Visibility = BarItemVisibility.Always;
            }

            if (MachineSoftwareConfiguration.GetInstance().IsDetectingEncryption
                && MachineSoftwareConfiguration.GetInstance().IsPromptEncryption)
            {
                this.BarAuthorization.Visibility = BarItemVisibility.Always;
            }
            else
            {
                this.BarAuthorization.Visibility = BarItemVisibility.Never;
            }
            
        }
    }
}
