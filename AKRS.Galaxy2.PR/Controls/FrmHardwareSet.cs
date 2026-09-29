using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Component.Simple.CommonControls;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.Galaxy2.LogicHardware.Hardwares;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Models;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.PR.Models.Entities;

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    /// 硬件配置窗体
    /// </summary>
    public partial class FrmHardwareSet : XtraForm
    {
        /// <summary>
        /// 重构PR动作模型
        /// </summary>
        private PREntity prEntity;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prEntity">PR实体</param>
        public FrmHardwareSet(PREntity prEntity)
        {
            InitializeComponent();
            this.prEntity = prEntity;
            BsHardware.DataSource = HardwareRepositoryService.GetHardwareBindingSource();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitView()
        {
            // 获取模组信息
            List<string> modules = HardwareRepositoryService.GetGroupHardwares().Keys.ToList();
            List<string> groupNames = new List<string>(modules);
            groupNames.Insert(0, string.Empty);
            BsModule.DataSource = groupNames;
            groupNames=groupNames.Where(name => !string.IsNullOrEmpty(name)).ToList();
            CmbModule.Properties.Items.AddRange(groupNames);
           
            // 加载相机信息
            List<AKRSCamera> cameraList = HardwareRepositoryService.GetHardwaresByType<AKRSCamera>();
            BsCamera.DataSource = cameraList;

            if (prEntity != null)
            {
                //相机名字绑定
                LueCamera.Binding(prEntity, "CameraName");

                //轴绑定
                LueAxisX.Binding(prEntity, "AxisXName");
                LueAxisY.Binding(prEntity, "AxisYName");
                LueAxisZ.Binding(prEntity, "AxisZName");

                //相关光源绑定
                BsPRLight.DataSource = prEntity.PRLightList;

                //初始化模组下拉框
                CmbModule.SelectedIndex= 0;
                CmbModule.SelectedItem = groupNames[0];

                //加载轴信息
                if (HardwareRepositoryService.GetGroupHardwares().Keys.Contains(CmbModule.Text))
                {
                    List<HardwareBase> list = HardwareRepositoryService.GetGroupHardwares()[CmbModule.Text];
                    List<Axis> axisList = list.OfType<Axis>().ToList();
                    axisList.Insert(0, new Axis { HardwareName = "无" });
                    BsAxis.DataSource = axisList;
                }
            }
        }

        /// <summary>
        /// 模组切换，并将模组中的相关轴进行刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HardwareRepositoryService.GetGroupHardwares().Keys.Contains(CmbModule.Text))
            {
                List<HardwareBase> list = HardwareRepositoryService.GetGroupHardwares()[CmbModule.Text];
                List<Axis> axisList = list.OfType<Axis>().ToList();
                axisList.Insert(0, new Axis { HardwareName = "无" });
                BsAxis.DataSource = axisList;
            }
        }

        /// <summary>
        /// 点击确定，并处理勾选光源名字为空的异常
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="e">触发参数</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            string message = string.Empty;

            prEntity.PRLightList.ForEach(
                light =>
                    {
                        if (light.IsUse && string.IsNullOrEmpty(light.LightName))
                        {
                            message = $"如果[使用]处于勾选状态，请确保你已经选择了[光源]";
                        }
                    });

            if (!string.IsNullOrEmpty(message))
            {
                XtraMessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
         
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 点击取消
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="e">触发参数</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="e">触发参数</param>
        private void FormHardwareSet_Load(object sender, EventArgs e)
        {
            InitView();
        }

        /// <summary>
        /// 单元格双击
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="e">触发参数</param>
        private void GvPRLights_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                PRLight prLight = GvPRLights.GetFocusedRow() as PRLight;
                if (prLight == null) 
                    return;

                if (e.Column.FieldName == "LightName")
                {
                    FormHardwareSelect formHardwareSelect = new FormHardwareSelect(typeof(Light));
                    if (formHardwareSelect.ShowDialog() == DialogResult.OK)
                    {
                        HardwareDto hardware = formHardwareSelect.SelectedElement;
                        prLight.LightName = hardware.HardwareName;
                        formHardwareSelect.Dispose();
                    }
                }

                GvPRLights.RefreshData();
            }
        }
    }
}
