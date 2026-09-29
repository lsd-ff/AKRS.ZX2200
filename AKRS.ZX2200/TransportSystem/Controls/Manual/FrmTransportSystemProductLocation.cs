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

namespace AKRS.ZX2200.TransportSystem.Controls.Manual
{
    using System.Threading;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.Enums;
    using AKRS.ZX2200.TransportSystem.Models.Programs;

    using DevExpress.Utils;

    /// <summary>
    /// 产品在流道载台上的位置记录
    /// </summary>
    public partial class FrmTransportSystemProductLocation : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 单例
        /// </summary>
        private static FrmTransportSystemProductLocation instance = null;

        /// <summary>
        /// 单例锁，用于线程安全
        /// </summary>
        private static object Lock = new object();

        /// <summary>
        /// 实现窗口单例
        /// </summary>
        /// <returns>单例</returns>
        public static FrmTransportSystemProductLocation GetInstance()
        {
            if (instance == null)
            {
                lock (Lock)
                {
                    if (instance == null)
                    {
                        instance = new FrmTransportSystemProductLocation();

                        // 窗口关闭后将对象引用置为空
                        instance.FormClosed += (sender, e) =>
                            {
                                instance = null;
                            };
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// TransportProgram实例
        /// </summary>
        TransportProgram TransportProgram => TransportDomain.GetInstance().TransportProgram;


        /// <summary>
        /// 初始化
        /// </summary>
        private FrmTransportSystemProductLocation()
        {
            this.InitializeComponent();

            // 默认所有载台都无料
            this.TileBarLoadingSubSection.AppearanceItem.Normal.BackColor = Color.LightSkyBlue;
            this.TileBarDispenseSubSection.AppearanceItem.Normal.BackColor = Color.LightSkyBlue;
            this.TileBarBondSubSection.AppearanceItem.Normal.BackColor = Color.LightSkyBlue;
            this.TileBarWaitingUnloadSubSection.AppearanceItem.Normal.BackColor = Color.LightSkyBlue;
            this.TileBarUnloadingSubSection.AppearanceItem.Normal.BackColor = Color.LightSkyBlue;
        }

        /// <summary>
        /// UI显示产品位置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // 设备状态
            this.LbMachineStatus.Text = MachineStateModel.GetInstance().MachineState.ToString();

            // 是否不再上料
            if (TransportProvider.ContinuousFeeding)
            {
                this.LbIsEmptyIndexOn.Text = "On";
            }
            else
            {
                this.LbIsEmptyIndexOn.Text = "Off";
            }

            // 产品在载台的位置记录
            this.DisplayStatus(
                this.TransportProgram.LoadingSubSectionProgram.SubSectionState,
                this.TileBarLoadingSubSection.AppearanceItem.Normal.BackColor,
                this.TileBarLoadingSubSection);

            this.DisplayStatus(
                this.TransportProgram.DispenseSubSectionProgram.SubSectionState,
                this.TileBarDispenseSubSection.AppearanceItem.Normal.BackColor,
                this.TileBarDispenseSubSection);

            this.DisplayStatus(
                this.TransportProgram.BondSubSectionProgram.SubSectionState,
                this.TileBarBondSubSection.AppearanceItem.Normal.BackColor,
                this.TileBarBondSubSection);

            this.DisplayStatus(
                this.TransportProgram.WaitingUnloadSubSectionProgram.SubSectionState,
                this.TileBarWaitingUnloadSubSection.AppearanceItem.Normal.BackColor,
                this.TileBarWaitingUnloadSubSection);

            this.DisplayStatus(
                this.TransportProgram.UnloadingSubSectionProgram.SubSectionState,
                this.TileBarUnloadingSubSection.AppearanceItem.Normal.BackColor,
                this.TileBarUnloadingSubSection);
        }

        /// <summary>
        /// 点击鼠标右键显示快捷菜单
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmTransportSystemProductLocation_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        /// <summary>
        /// 记录流道产品位置
        /// </summary>
        /// <param name="subSectionState">流道状态</param>
        /// <param name="backColor">UI控件背景颜色</param>
        /// <param name="tileItem">UI控件显示的内容</param>
        private void DisplayStatus(SubSectionStateEnum subSectionState, Color backColor, TileItem tileItem)
        {
            switch (subSectionState)
            {
                case SubSectionStateEnum.NoMaterial:
                    backColor = Color.LightSkyBlue;
                    tileItem.Text = "NoMaterial";
                    break;

                //case SubSectionStateEnum.Transfering:
                //    backColor = Color.Green;
                //    tileItem.Text = "Transfering";
                    break;

                case SubSectionStateEnum.HasMaterial:
                    backColor = Color.DarkOrange;
                    tileItem.Text = "HasMaterial";
                    break;

                //case SubSectionStateEnum.Alarm:
                //    backColor = Color.Red;
                //    tileItem.Text = "Alarm";
                //    break;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmTransportSystemProductLocation_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.timer1.Tick -= timer1_Tick;
            this.timer1.Dispose();
            this.timer1 = null;
        }
    }
}