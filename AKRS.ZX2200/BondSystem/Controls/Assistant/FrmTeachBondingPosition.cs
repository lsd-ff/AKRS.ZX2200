using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using Accord.IO;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 焊点示教窗体
    /// </summary>
    public partial class FrmTeachBondingPosition : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="singleBondPositionConfig">焊点配置</param>
        /// <param name="coordinate">坐标系</param>
        public FrmTeachBondingPosition(SingleBondPositionConfig singleBondPositionConfig, GeneralCoordinateSystem coordinate)
        {
            this.singleBondPositionConfig = singleBondPositionConfig;
            this.coordinateSystem = coordinate;
            this.InitializeComponent();
            this.InitControl();
            this.Disposed += (o, e) =>
            {
                ucGuideMove1.Dispose();
            };
        }

        /// <summary>
        /// 坐标系
        /// </summary>
        private readonly GeneralCoordinateSystem coordinateSystem;

        /// <summary>
        /// 拉角度定位点1
        /// </summary>
        private AKRSPoint3D degreePoint1;

        /// <summary>
        /// 拉角度定位点2
        /// </summary>
        private AKRSPoint3D degreePoint2;

        /// <summary>
        /// 真实角度
        /// </summary>
        public double RealDegree { get; set; }

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 点击Start传进来的焊点配置
        /// </summary>
        private readonly SingleBondPositionConfig singleBondPositionConfig;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 确定焊点中心走的点位
        /// </summary>
        private readonly List<AKRSPoint3D> bondPositionList = new List<AKRSPoint3D>();

        /// <summary>
        ///  焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmTeachBondingPosition");

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.PnlControl.Controls.Add(ucGuideMove1);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    //// 测流道角度第一步
                    //new AssistantConfig(
                    //    index: 0,
                    //    descritpion: $"Step 1/6:确定焊点角度：移动轴使相机中心对准焊点左上角",
                    //    isShowTitle: true,
                    //    isShowBack: false,
                    //    isShowNext: true,
                    //    isShowDone: false,
                    //    backAction: () =>
                    //    {
                    //    },
                    //    nextAction: () =>
                    //        {
                    //            this.TileBarTeach.SelectedItem = this.TbiRotaryPosition2;

                    //            // 记录第一点位置
                    //            this.degreePoint1 = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                    //        },
                    //   doneAction: () =>
                    //   {
                    //   }),
                 
                    //// 测流道角度第二步
                    //new AssistantConfig(
                    //    index: 1,
                    //    descritpion: $"Step 2/6:确定焊点角度：移动轴使相机中心对准焊点右上角",
                    //    isShowTitle: true,
                    //    isShowBack: true,
                    //    isShowNext: true,
                    //    isShowDone: false,
                    //    backAction: () =>
                    //    {
                    //        this.TileBarTeach.SelectedItem = this.TbiRotaryPosition1;
                    //    },
                    //    nextAction: () =>
                    //    {
                    //         this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin1;

                    //         // 记录第二点位置
                    //        this.degreePoint2 = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);

                    //        // 两点距离过短报警（0.1mm）
                    //        if (Math.Abs(this.degreePoint2.X - this.degreePoint1.X) < 0.1) 
                    //        {
                    //            DialogResult dialog = AKRSXtraMessageBox.Show(
                    //                $"两点距离过短，至少 0.10 mm.\r\nOK：从头开始示教\r\nCancel:结束示教.",
                    //                "Warn",
                    //                MessageBoxButtons.OKCancel,
                    //                MessageBoxIcon.Warning);

                    //            if (dialog == DialogResult.OK)
                    //            {
                    //                // 返回到第一步
                    //                // 因为点next会自动加1，所以这里是-1
                    //                this.stepIndex = -1;
                    //                this.TileBarTeach.SelectedItem = this.TbiRotaryPosition1;
                    //                return;
                    //            }
                    //            else
                    //            {
                    //                // 清除数据
                    //                this.bondPositionList.Clear();
                    //                this.Close();
                    //            }
                    //        }

                    //        this.RealDegree = Math.Atan(
                    //            (this.degreePoint1.Y - this.degreePoint2.Y) / (this.degreePoint1.X
                    //                                                           - this.degreePoint2.X));

                    //        // 两点距离过短报警（0.1mm）
                    //        if (Math.Abs(this.RealDegree) > 1) 
                    //        {
                    //            DialogResult dialog = AKRSXtraMessageBox.Show(
                    //                $"两点距离过短，至少 0.10 mm.\r\nOK：从头开始示教\r\nCancel:结束示教.",
                    //                "Warn",
                    //                MessageBoxButtons.OKCancel,
                    //                MessageBoxIcon.Warning);

                    //            if (dialog == DialogResult.OK)
                    //            {
                    //                // 返回到第一步
                    //                // 因为点next会自动加1，所以这里是-1
                    //                this.stepIndex = -1;
                    //                this.TileBarTeach.SelectedItem = this.TbiRotaryPosition1;
                    //                return;
                    //            }
                    //            else
                    //            {
                    //                // 清除数据
                    //                this.bondPositionList.Clear();
                    //                this.Close();
                    //            }
                    //        }

                    //        this.singleBondPositionConfig.ElementCoordinate.Degree = this.RealDegree;
                    //    },
                    //    doneAction: () =>
                    //    {
                    //    }),

                    // 测焊点贴片位置第一步
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"Step 1/4: 确定原点 : 将焊点对准相机中心.\r\n如果是1点确定原点就直接将焊点中心对准相机中心点击“确定”,\r\n如果是多点确定焊点原点就将焊点左上角对准相机中心点击“下一步”。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                            {
                                //this.TileBarTeach.SelectedItem = this.TbiRotaryPosition2;
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin2;

                                // 获取第一个点的坐标
                                bondPositionList.Add(TUAssistantHelper.GetPosBySystem(this.coordinateSystem));
                            },
                        doneAction: () =>
                            {
                                // 保存焊点贴片位(相对于基岛)
                                this.singleBondPositionConfig.ElementCoordinate.Point = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);;
                            }),

                    // 测焊点贴片位置第二步
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"Step 2/4: 确定原点:如果是两点确定焊点原点就将焊点右下角对准相机中心点击“确定”，如果是四点确定焊点原点就将焊点右上角对准相机中心点击“下一步”。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin1;
                                bondPositionList.RemoveAt(0);
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin3;

                                // 获取第二个点的坐标
                                bondPositionList.Add(TUAssistantHelper.GetPosBySystem(this.coordinateSystem));
                            },
                        doneAction: () =>
                            {
                                bondPositionList.Add(TUAssistantHelper.GetPosBySystem(this.coordinateSystem));

                                // 保存焊点示教位
                                this.singleBondPositionConfig.ElementCoordinate.Point = (bondPositionList[0] + bondPositionList[1]) / 2;
                            }),

                    // 测焊点贴片位置第三步
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"Step 3/4:确定原点:将焊点右下角对准相机中心。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin2;
                                bondPositionList.RemoveAt(1);
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin4;

                                bondPositionList.Add(TUAssistantHelper.GetPosBySystem(this.coordinateSystem));
                            },
                        doneAction: () =>
                            {
                            }),

                    // 测焊点贴片位置第四步
                    new AssistantConfig(
                        index: 4,
                        descritpion: $"Step 4/4:确定原点:将焊点左下角对准相机中心。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin3;
                                bondPositionList.RemoveAt(2);
                            },
                        nextAction: () =>
                            {
                            },
                        doneAction: () =>
                            {
                                bondPositionList.Add(TUAssistantHelper.GetPosBySystem(this.coordinateSystem));

                                AKRSPoint3D point = (bondPositionList[0] + bondPositionList[1]
                                                                         + this.bondPositionList[2]
                                                                         + this.bondPositionList[3]) / 4;

                                // 保存焊点贴片位(视觉位)
                                this.singleBondPositionConfig.ElementCoordinate.Point.X = point.X;
                                this.singleBondPositionConfig.ElementCoordinate.Point.Y = point.Y;
                            }),
                };


            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin1;

            // 绑定模组
            this.ucGuideMove1.ChangeModuleName("固晶模组", true);
        }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUIControl(this.stepIndex);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUIControl(this.stepIndex);
        }

        /// <summary>
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();

            // 获取当前相机的视觉硬件参数，后续看焊点时会使用
            System2Domain.GetInstance().SaveVisionHardwareParameter(this.singleBondPositionConfig);
            this.Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.singleBondPositionConfig.TeachBondingPosition.State = AssistantStateEnum.Able;
            ProductConfiguration.GetInstance().Save();

            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.UpdateTransportUnit();
        }

        /// <summary>
        /// 相机自动对焦按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.bondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否结束示教?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                // 清除数据
                this.bondPositionList.Clear();
                this.Close();
                this.Dispose();
            }
        }

    }
}