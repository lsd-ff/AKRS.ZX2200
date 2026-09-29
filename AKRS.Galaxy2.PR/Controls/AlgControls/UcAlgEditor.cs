using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Aop.Aspects;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.Algs;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using BranchModule_STDCs;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Customization;
using GeometryCreateCs;
using ImageSourceModuleCs;
using IMVSBlobFindModuCs;
using IMVSCaliperCornerModuCs;
using IMVSCaliperEdgeModuCs;
using IMVSCircleFindModuCs;
using IMVSContourMatchModuCs;
using IMVSFastFeatureMatchModuCs;
using IMVSFixtureModuCs;
using IMVSGrayMatchModuCs;
using IMVSHPFeatureMatchModuCs;
using IMVSLineFindModuCs;
using IMVSMarkFindModuCs;
using IMVSQuadrangleFindModuCs;
using IMVSRectFindModuCs;
using ShellModuleCs;
using VM.Core;
using VMControls.Interface;
using VMControls.Winform.Release;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IMVSEdgeFlawInspModuCs;
using HalconDotNet;
using VM.PlatformSDKCS;
using IMVS2dBcrModuCs;

namespace AKRS.Galaxy2.PR.Controls.AlgControls
{
    /// <summary>
    /// 模版设置
    /// </summary>
    public partial class UcAlgEditor : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// BaseAlg对象
        /// </summary>
        private BaseAlg baseAlg;

        /// <summary>
        /// 参数配置界面控件
        /// </summary>
        private ParamsControl paramsControl;

        /// <summary>
        /// 图像渲染界面
        /// </summary>
        private RenderControl renderControl;

        /// <summary>
        /// 流程控制界面
        /// </summary>
        private ViewformControl viewform;

        /// <summary>
        /// 实时刷新图像标志
        /// </summary>
        private bool isRefreshImg;

        /// <summary>
        /// 粗定位模块名称
        /// </summary>
        private string crudeModuleName;

        /// <summary>
        /// 精定位/执行模块名称
        /// </summary>
        private string exactModuleName;

        /// <summary>
        /// 定位输出结果
        /// </summary>
        private MatchResult matchResult = new MatchResult();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="alg">算法流程</param>
        public UcAlgEditor(BaseAlg alg)
        {
            InitializeComponent();

            this.baseAlg = alg;

            paramsControl = new ParamsControl { Dock = DockStyle.Fill };

            this.renderControl = new RenderControl { Dock = DockStyle.Fill };

            viewform = new ViewformControl { Dock = DockStyle.Fill };

            LueAlgType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AlgFlowTypeEnum>().ToList();

            this.LueAlgType.EditValue = this.baseAlg.AlgFlowType;

            this.renderControl.ModuleSource = this.baseAlg.GetVmProcedure();

            RefreshUI();

            isRefreshImg = true;
        }

        /// <summary>
        /// 传入实时图像
        /// </summary>
        /// <param name="bitmap">图片</param>
        [HandleExceptionAspect("传入实时图像失败", LogCategory.PR)]
        public void RefreshImg(Bitmap bitmap)
        {

           

                if (isRefreshImg&&bitmap != null)
                {
                    this.baseAlg.SetImage(bitmap);
                    matchResult = new MatchResult();
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    this.baseAlg.FindModel();
                    stopwatch.Stop();
                    Console.WriteLine("Findmodel耗时:" + stopwatch.ElapsedMilliseconds);
                    if (this.baseAlg.MatchResults.Count() != 0 && this.baseAlg.AlgFlowType != AlgFlowTypeEnum.EpoxyDetectAlg &&
                        this.baseAlg.AlgFlowType != AlgFlowTypeEnum.HighPreModelAlgWithBlob && this.baseAlg.AlgFlowType != AlgFlowTypeEnum.InkDotDetectAlg &&
                        this.baseAlg.AlgFlowType != AlgFlowTypeEnum.HighPreModelAlgWithCorner
                        && this.baseAlg.AlgFlowType != AlgFlowTypeEnum.QrCodeDetectAlg)
                    {
                        matchResult = (MatchResult)this.baseAlg.MatchResults[0];
                    }

                    this.renderControl.AddShape();
                    paramsControl.AddShape();

                }
            
           
        }

        /// <summary>
        /// 获取算法BaseAlg
        /// </summary>
        /// <returns>
        /// The <see cref="BaseAlg"/>.
        /// </returns>
        [HandleExceptionAspect("获取算法BaseAlg失败", LogCategory.PR)]
        public BaseAlg GetBaseAlg()
        {
            return this.baseAlg;
        }


        /// <summary>
        ///  根据PR类型，设置tab 的可见性
        /// </summary>
        [HandleExceptionAspect("设置tab可见性失败", LogCategory.PR)]
        public void RefreshUI()
        {
            for (int i = 0; i < TabSetPR.TabPages.Count(); i++)
            {
                TabSetPR.TabPages[i].PageVisible = false;
            }

            switch (this.baseAlg.AlgFlowType)
            {
                case AlgFlowTypeEnum.FastModelAlg:
                    LueFastCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueFastCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseFastCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseFastCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.快速匹配2";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.快速匹配3";
                    }
                    TabSetPR.TabPages[0].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.HighPreModelAlg:
                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配3";
                    TabSetPR.TabPages[1].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.HighPreModelAlgWithRefer:

                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + "高精度匹配1";


                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";

                    TabSetPR.TabPages[2].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.HighPreModelAlgWithBlob:

                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + "高精度匹配1";


                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.Blob分析1";

                    TabSetPR.TabPages[3].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.NccModelAlg:
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.灰度匹配1";
                    TabSetPR.TabPages[4].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.XldModelAlg:
                    LueXldCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueXldCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseXldCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    this.ChkIsUseXldCrudeAngle.Checked = this.baseAlg.IsUseCrudeLocateAngle;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseXldCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配2";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配3";
                    }
                    TabSetPR.TabPages[5].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.EpoxyDetectAlg:
                    LueEpoxyCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueEpoxyCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseEpoxyCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    ChkIsUseTotalArea.Checked = this.baseAlg.IsOutputTotal;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseEpoxyCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析2";
                    }
                    TabSetPR.TabPages[6].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.PostBondDetectAlg:
                    LuePostCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    LuePostExactPR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LuePostCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    this.LuePostExactPR.EditValue = this.baseAlg.ExactLocateType;
                    ChkIsUsePostCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUsePostCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "2";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "3";
                    }
                    TabSetPR.TabPages[7].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.InkDotDetectAlg:
                    LueInkCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueInkCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseInkCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseInkCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析2";
                    }
                    TabSetPR.TabPages[8].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.CornerDetectAlg:
                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
                    TabSetPR.TabPages[9].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.PreventReverseAlg:

                    LueReverseCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueReverseCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseReverseCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    LueReverseExactPR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueReverseExactPR.EditValue = this.baseAlg.ExactLocateType;

                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseReverseCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "2";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "3";
                    }

                    TabSetPR.TabPages[10].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.PreventWrongAlg:

                    LueWrongCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueWrongCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseWrongCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;                  
                    LueWrongExactPR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueWrongExactPR.EditValue = this.baseAlg.ExactLocateType;

                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseWrongCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "2";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "3";
                    }
                    TabSetPR.TabPages[11].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.RectangleDetectAlg:
                    LueRecCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueRecCrudePR.EditValue = this.baseAlg.CrudeLocateType;

                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.四边形查找1";
                    TabSetPR.TabPages[12].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.CircleFindAlg:
                    LueCircleCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueCircleCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseCircleCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    ChkIsOutputMidpoint.Checked = this.baseAlg.IsOutputMidpoint;
                    if (ChkIsUseCircleCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找1";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找2";
                    }
                    TabSetPR.TabPages[13].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.FocusMeasureAlg:
                    break;

                case AlgFlowTypeEnum.CenterSearchAlg:
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
                    TabSetPR.TabPages[14].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.CrossSearchAlg:
                    LueCrossCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueCrossCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseCrossCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    TabSetPR.TabPages[15].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.FiducialFindAlg:
                    LueFiducialCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueFiducialCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseFiducialCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseFiducialCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.图形定位1";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.图形定位2";
                    }
                    TabSetPR.TabPages[16].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.LineDetectAlg:

                    LueLineCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueLineCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    ChkIsUseLineCrudePR.Checked = this.baseAlg.IsUseCrudeLocate;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    if (ChkIsUseLineCrudePR.Checked)
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找1";
                    }
                    else
                    {
                        exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找2";
                    }
                    TabSetPR.TabPages[17].PageVisible = true;
                    break;
                case AlgFlowTypeEnum.DistanceMeasureAlg:                    
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配1";

                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";

                    TabSetPR.TabPages[18].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.RectangleSecondDetectAlg:
                    LueRecSecondCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueRecSecondCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    this.ChkIsUseRecLength.Checked=this.baseAlg.IsUseRecLength;

                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.矩形检测1";
                    TabSetPR.TabPages[19].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.SymmetricModeleAlg:
                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配1";
                    TabSetPR.TabPages[20].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.HighPreModelAlgWithCorner:
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配1";
                    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.组合模块1.Blob分析1";
                    TabSetPR.TabPages[21].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.HybridPositionAlg:
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配1";
                    TabSetPR.TabPages[22].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.FcUplookModelAlg:
                    LueFcUplookCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LueFcUplookCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    TabSetPR.TabPages[23].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.FcEdgeBreakDetectAlg:                    
                    TabSetPR.TabPages[24].PageVisible = true;
                    break;
                case AlgFlowTypeEnum.SurfaceDetectAlg:
                    TabSetPR.TabPages[25].PageVisible = true;

                    this.spThreshold.Value = this.baseAlg.Threshold;
                    this.spArea.Value = this.baseAlg.Area;

                    break;
                case AlgFlowTypeEnum.EdgeBreakDetectAlg:
                    TabSetPR.TabPages[26].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.QrCodeDetectAlg:
                    TabSetPR.TabPages[27].PageVisible = true;
                    break;

                case AlgFlowTypeEnum.CircleAndLineAlg:
                    LuecirclelineCrudePR.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LocateTypeEnum>().ToList();
                    this.LuecirclelineCrudePR.EditValue = this.baseAlg.CrudeLocateType;
                    crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
                    TabSetPR.TabPages[28].PageVisible = true;
                    break;

                default:
                    break;
            }


        }

        /// <summary>
        /// 根据定位类型，绑定对应模块
        /// </summary>
        /// <param name="locateTypeName">定位类型名称</param>
        /// <param name="moduleName">流程.模块名</param>        
        public void BindingModuleSource(string locateTypeName, string moduleName)
        {
            IVmModule vmModule = null;
            switch (Enum.Parse(typeof(LocateTypeEnum), locateTypeName))
            {
                case LocateTypeEnum.FastModel:
                    vmModule = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[moduleName];
                    paramsControl.ModuleSource = vmModule;
                    break;

                case LocateTypeEnum.HighPreModel:
                    vmModule = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[moduleName];
                    paramsControl.ModuleSource = vmModule;
                    break;

                case LocateTypeEnum.NccModel:
                    vmModule = (IMVSGrayMatchModuVACs.IMVSGrayMatchModuVATool)VmSolution.Instance[moduleName];
                    paramsControl.ModuleSource = vmModule;
                    break;

                case LocateTypeEnum.XldModel:
                    vmModule = (IMVSContourMatchModuTool)VmSolution.Instance[moduleName];
                    paramsControl.ModuleSource = vmModule;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 初始化控件参数
        /// </summary>
        /// <param name="locateTypeName"></param>
        /// <param name="moduleName"></param>
        /// <param name="SpAngle"></param>
        /// <param name="SpScore"></param>
        public void InitModuleParam(LocateTypeEnum locateTypeEnum, string moduleName, SpinEdit SpAngle, SpinEdit SpScore)
        {
            switch (locateTypeEnum)
            {
                case LocateTypeEnum.FastModel:
                    IMVSFastFeatureMatchModuTool FvmModule = (IMVSFastFeatureMatchModuTool)VmSolution.Instance[moduleName];
                    SpAngle.Value = (decimal)FvmModule.ModuParams.AngleEnd;
                    SpScore.Value = (decimal)FvmModule.ModuParams.MinScore;
                    break;

                case LocateTypeEnum.HighPreModel:
                    IMVSHPFeatureMatchModuTool HvmModule = (IMVSHPFeatureMatchModuTool)VmSolution.Instance[moduleName];
                    SpAngle.Value = (decimal)HvmModule.ModuParams.AngleEnd;
                    SpScore.Value = (decimal)HvmModule.ModuParams.MinScore;
                    break;

                case LocateTypeEnum.NccModel:
                    IMVSGrayMatchModuVACs.IMVSGrayMatchModuVATool NvmModule = (IMVSGrayMatchModuVACs.IMVSGrayMatchModuVATool)VmSolution.Instance[moduleName];
                    SpAngle.Value = (decimal)NvmModule.ModuParams.AngleEnd;
                    SpScore.Value = (decimal)NvmModule.ModuParams.MinScore;
                    break;

                case LocateTypeEnum.XldModel:
                    IMVSContourMatchModuTool XvmModule = (IMVSContourMatchModuTool)VmSolution.Instance[moduleName];
                    SpAngle.Value = (decimal)XvmModule.ModuParams.AngleEnd;
                    SpScore.Value = (decimal)XvmModule.ModuParams.MinScore;
                    break;

                default:
                    break;
            }

        }

        /// <summary>
        /// 展示图像显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("切换图像显示界面失败", LogCategory.PR)]
        private void BtnShowImage_Click(object sender, EventArgs e)
        {
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(this.renderControl);
        }

        /// <summary>
        /// 展示参数配置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("切换参数配置界面失败", LogCategory.PR)]
        private void BtnParamConfig_Click(object sender, EventArgs e)
        {
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 展示流程设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("切换流程设置界面失败", LogCategory.PR)]
        private void BtnShowProcedure_Click(object sender, EventArgs e)
        {
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(viewform);
        }

        /// <summary>
        /// 设置选定的PR模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("切换PR模型失败", LogCategory.PR)]
        private void BtnSetPRType_Click(object sender, EventArgs e)
        {

            // 切换模型时，停止实时图像
            isRefreshImg = false;

            string AlgPath = this.baseAlg.AlgSavePath;

            string oldName = this.baseAlg.VmProcedureName;

            DialogResult dialog = XtraMessageBox.Show(
             $"确认要删除原有PR，并设置新的PR？",
             "提示",
             MessageBoxButtons.OKCancel,
             MessageBoxIcon.Information);

            if (dialog == DialogResult.OK)
            {
                if (File.Exists(AlgPath + this.baseAlg.Name + ".prc"))
                {
                    DirAndFileHelper.DeleteFile(AlgPath + this.baseAlg.Name + ".prc");
                }
            }
            else
            {
                this.LueAlgType.EditValue = this.baseAlg.AlgFlowType;
                isRefreshImg = true;
                return;
            }

            this.baseAlg.AlgFlowType = (AlgFlowTypeEnum)Enum.Parse(typeof(AlgFlowTypeEnum), this.LueAlgType.EditValue.ToString());

            AlgBeLongEnum AlgBeLong = this.baseAlg.AlgBeLong;

            BaseAlg baseAlgTemp = AlgsFactory.Create(this.baseAlg.AlgFlowType, this.baseAlg.Name);

            this.baseAlg = baseAlgTemp;

            this.baseAlg.AlgFlowType = (AlgFlowTypeEnum)Enum.Parse(typeof(AlgFlowTypeEnum), this.LueAlgType.EditValue.ToString());

            this.baseAlg.InitVmProcedure();

            this.baseAlg.AlgBeLong = AlgBeLong;

            this.baseAlg.AlgSavePath = AlgPath;

            this.renderControl.ModuleSource = this.baseAlg.GetVmProcedure();

            RefreshUI();

            VmProcedure oldprocedure = (VmProcedure)VmSolution.Instance[oldName];
            if (oldprocedure != null) { oldprocedure.Dispose(); }

            isRefreshImg = true;
        }

        /// <summary>
        /// 快速匹配模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置快速匹配模型失败", LogCategory.PR)]
        private void BtnSetFastModel_Click(object sender, EventArgs e)
        {

            if (ChkIsUseFastCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.快速匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.快速匹配3";
            }

            IVmModule iMVSFastFeatureMatchModuTool =
                (IMVSFastFeatureMatchModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSFastFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 高精度模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置高精度匹配模型失败", LogCategory.PR)]
        private void BtnSetHighPreModel_Click(object sender, EventArgs e)
        {
            //if (ChkIsUseHighPreCrudePR.Checked)
            //{
            //    exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";
            //}
            //else
            //{
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配3";
            //}
            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 灰度匹配模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置灰度匹配模型失败", LogCategory.PR)]
        private void BtnSetNccModel_Click(object sender, EventArgs e)
        {
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.灰度匹配1";
            IVmModule iMVSGrayMatchModuVATool =
                (IMVSGrayMatchModuVACs.IMVSGrayMatchModuVATool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = iMVSGrayMatchModuVATool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 轮廓匹配模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置轮廓匹配模型失败", LogCategory.PR)]
        private void BtnSetXldModel_Click(object sender, EventArgs e)
        {
            if (ChkIsUseXldCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配3";
            }
            IMVSContourMatchModuTool iMVSContourMatchModuTool =
                (IMVSContourMatchModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSContourMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 胶量检测是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选胶量检测粗定位失败", LogCategory.PR)]
        private void ChkIsUseEpoxyCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseEpoxyCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.组合模块1.BLOB分析1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.组合模块2.BLOB分析1";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseEpoxyCrudePR.Checked;
            this.LueEpoxyCrudePR.Visible = ChkIsUseEpoxyCrudePR.Checked;
            this.BtnSetEpoxyCrudePR.Visible = ChkIsUseEpoxyCrudePR.Checked;

            //IMVSBlobFindModuTool iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[exactModuleName];
            //this.SpEpoxyLowThresh.Value = (decimal)iMVSBlobFindModuTool.ModuParams.LowThreshold;
            //this.SpEpoxyHighThresh.Value = (decimal)iMVSBlobFindModuTool.ModuParams.HightThreshold;
            //this.SpEpoxyAreaThresh.Value = (decimal)iMVSBlobFindModuTool.ModuParams.MinArea;
            //this.SpEpoxyCount.Value = (decimal)iMVSBlobFindModuTool.ModuParams.FindNum;
        }

        /// <summary>
        /// 胶量检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置胶量检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetEpoxyCrudePR_Click(object sender, EventArgs e)
        {
            
            string lueEpoxyCrudePRText = this.LueEpoxyCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueEpoxyCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueEpoxyCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 胶量检测参数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置胶量检测模型失败", LogCategory.PR)]
        private void BtnSetEpoxyBlob_Click(object sender, EventArgs e)
        {

            if (ChkIsUseEpoxyCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析2";
            }
            IVmModule iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSBlobFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 焊后检测是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选焊后检测粗定位失败", LogCategory.PR)]
        private void ChkIsUsePostCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            this.baseAlg.IsUseCrudeLocate = ChkIsUsePostCrudePR.Checked;
            this.LuePostCrudePR.Visible = ChkIsUsePostCrudePR.Checked;
            this.BtnSetPostCrudePR.Visible = ChkIsUsePostCrudePR.Checked;

            if (ChkIsUsePostCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "3";
            }
            //InitModuleParam(this.baseAlg.ExactLocateType, exactModuleName, this.SpPostExactAngle, this.SpPostExactScore);
        }

        /// <summary>
        /// 焊后检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置焊后检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetPostCrudePR_Click(object sender, EventArgs e)
        {
            string luePostCrudePRText = this.LuePostCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), luePostCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(luePostCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 焊后检测的精定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置焊后检测精定位模型失败", LogCategory.PR)]
        private void BtnSetPostExactPR_Click(object sender, EventArgs e)
        {
            string luePostExactPRText = this.LuePostExactPR.EditValue.ToString();
            this.baseAlg.ExactLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), luePostExactPRText);
            if (ChkIsUsePostCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "3";
            }
            BindingModuleSource(luePostExactPRText, exactModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 墨点检测是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选墨点检测粗定位失败", LogCategory.PR)]
        private void ChkIsUseInkCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseInkCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析2";
            }

            this.baseAlg.IsUseCrudeLocate = ChkIsUseInkCrudePR.Checked;
            this.LueInkCrudePR.Visible = ChkIsUseInkCrudePR.Checked;
            this.BtnSetInkCrudePR.Visible = ChkIsUseInkCrudePR.Checked;

            //IMVSBlobFindModuTool MVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[exactModuleName];
            //this.SpInkHighThresh.Value = (decimal)MVSBlobFindModuTool.ModuParams.HightThreshold;
            //this.SpInkLowThresh.Value = (decimal)MVSBlobFindModuTool.ModuParams.LowThreshold;
            //this.SpInkAreaThresh.Value = (decimal)MVSBlobFindModuTool.ModuParams.MinArea;
        }

        /// <summary>
        /// 墨点检测参数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置墨点检测参数失败", LogCategory.PR)]
        private void BtnSetInkBlob_Click(object sender, EventArgs e)
        {
            if (ChkIsUseInkCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析2";
            }
            IVmModule iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSBlobFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置输入图像
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置传入图像失败", LogCategory.PR)]
        private void BtnSetImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "BMP文件|*.bmp*|JPEG文件|*.jpg*|TIFF文件|*.tiff*";
            ofd.RestoreDirectory = true;
            ofd.FilterIndex = 1;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string imageDir = ofd.FileName;
                Bitmap bmp = new Bitmap(imageDir);
                this.baseAlg.SetImage(bmp);
            }
        }

        /// <summary>
        /// 执行流程
        /// </summary>
        /// <param name="sender">事件源</param>     
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("执行检测流程失败", LogCategory.PR)]
        private void BtnRun_Click(object sender, EventArgs e)
        {
            this.baseAlg.FindModel();
            List<BaseAlgResult> MatchResults = this.baseAlg.MatchResults;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(this.renderControl);
            this.renderControl.AddShape();
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("窗体初始化失败", LogCategory.PR)]
        private void FrmSetPRModel_Load(object sender, EventArgs e)
        {
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(this.renderControl);
        }

        /// <summary>
        /// 墨点检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置墨点检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetInkCrudePR_Click(object sender, EventArgs e)
        {
            string lueInkCrudePRText = this.LueInkCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueInkCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueInkCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 胶量检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置胶量检测区域失败", LogCategory.PR)]
        private void BtnSetEpoxyRegion_Click(object sender, EventArgs e)
        {
            //paramsControl.isedit = isEdit;
            //string moduleName = "";
            //if (ChkIsUseEpoxyCrudePR.Checked)
            //{
            //    moduleName = $"{this.baseAlg.GetVmProcedure().FullName}.几何创建1";
            //}
            //else
            //{
            //    moduleName = $"{this.baseAlg.GetVmProcedure().FullName}.几何创建2";
            //}
            //IVmModule geometryCreateTool = (GeometryCreateTool)VmSolution.Instance[moduleName];
            //paramsControl.ModuleSource = geometryCreateTool;
            //PnlVmControl.Controls.Clear();
            //PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 矩形检测的粗定位模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置矩形检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetRecCrudePR_Click(object sender, EventArgs e)
        {
            string lueRecCrudePRText = this.LueRecCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueRecCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueRecCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 矩形检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置矩形检测区域失败", LogCategory.PR)]
        private void BtnSetRecDetect_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.四边形查找1";
            IVmModule iMVSRectFindModuTool = (IMVSQuadrangleFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSRectFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 圆查找是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选圆查找粗定位失败", LogCategory.PR)]
        private void ChkIsUseCircleCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseCircleCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找2";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseCircleCrudePR.Checked;
            this.LueCircleCrudePR.Visible = ChkIsUseCircleCrudePR.Checked;
            this.BtnSetCircleCrudePR.Visible = ChkIsUseCircleCrudePR.Checked;

            //IMVSCircleFindModuTool iMVSCircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[exactModuleName];
            //this.SpCircleInnerRadius.Value = (decimal)iMVSCircleFindModuTool.ModuParams.MinRadius;
            //this.SpCircleOuterRadius.Value = (decimal)iMVSCircleFindModuTool.ModuParams.MaxRadius;
            //this.SpCircleRejectNum.Value = (decimal)iMVSCircleFindModuTool.ModuParams.RejectNum;
            //this.SpCircleEdgeWidth.Value = (decimal)iMVSCircleFindModuTool.ModuParams.EdgeWidth;
            //this.SpCircleEdgeThresh.Value = (decimal)iMVSCircleFindModuTool.ModuParams.EdgeThresh;
            //this.LueCircleEdgePolarity.EditValue = (EdgePolarityEnum)Enum.Parse(typeof(EdgePolarityEnum), iMVSCircleFindModuTool.ModuParams.EdgePolarity.ToString());
        }

        /// <summary>
        /// 圆查找的粗定位模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置圆查找粗定位模型失败", LogCategory.PR)]
        private void BtnSetCircleCrudePR_Click(object sender, EventArgs e)
        {
            string lueCircleCrudePRText = this.LueCircleCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueCircleCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueCircleCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }


        /// <summary>
        /// 圆查找区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置圆查找区域失败", LogCategory.PR)]
        private void BtnSetCircleDetect_Click(object sender, EventArgs e)
        {
            if (ChkIsUseCircleCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找2";
            }
            IVmModule iMVSCircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCircleFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 快速检测是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选快速检测粗定位失败", LogCategory.PR)]
        private void ChkIsUseFastCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseFastCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.快速匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.快速匹配3";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseFastCrudePR.Checked;
            this.LueFastCrudePR.Visible = ChkIsUseFastCrudePR.Checked;
            this.BtnSetFastCrudePR.Visible = ChkIsUseFastCrudePR.Checked;

            //this.GcFastCrudeParam.Visible = ChkIsUseFastCrudePR.Checked;
            //InitModuleParam(LocateTypeEnum.FastModel, exactModuleName, this.SpFastExactAngle, this.SpFastExactScore);
        }

        /// <summary>
        /// 快速检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置快速检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetFastCrudePR_Click(object sender, EventArgs e)
        {
            string lueFastCrudePRText = this.LueFastCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueFastCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueFastCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 高精度检测是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选高精度检测粗定位失败", LogCategory.PR)]
        private void ChkIsUseHighPreCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseHighPreCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配3";
            }
            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[exactModuleName];
            this.baseAlg.IsUseCrudeLocate = ChkIsUseHighPreCrudePR.Checked;
            this.LueHighPreCrudePR.Visible = ChkIsUseHighPreCrudePR.Checked;
            this.BtnSetHighPreCrudePR.Visible = ChkIsUseHighPreCrudePR.Checked;
            //InitModuleParam(LocateTypeEnum.HighPreModel, exactModuleName, this.SpHighPreExactAngle, this.SpHighPreExactScore);
        }

        /// <summary>
        /// 高精度检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置高精度检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetHighPreCrudePR_Click(object sender, EventArgs e)
        {
            string lueHighPreCrudePRText = this.LueHighPreCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueHighPreCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueHighPreCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 轮廓检测是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选轮廓检测粗定位失败", LogCategory.PR)]
        private void ChkIsUseXldCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            this.baseAlg.IsUseCrudeLocate = ChkIsUseXldCrudePR.Checked;
            this.LueXldCrudePR.Visible = ChkIsUseXldCrudePR.Checked;
            this.BtnSetXldCrudePR.Visible = ChkIsUseXldCrudePR.Checked;

            if (ChkIsUseXldCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配3";
            }
            // InitModuleParam(LocateTypeEnum.XldModel, exactModuleName, this.SpXldExactAngle, this.SpXldExactScore);
        }

        /// <summary>
        /// 轮廓检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置轮廓检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetXldCrudePR_Click(object sender, EventArgs e)
        {
            string lueXldCrudePRText = this.LueXldCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueXldCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueXldCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 墨点检测的检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置墨点检测区域失败", LogCategory.PR)]
        private void BtnSetInkRegion_Click(object sender, EventArgs e)
        {
            string moduleName = "";
            if (ChkIsUseInkCrudePR.Checked)
            {
                moduleName = $"{this.baseAlg.GetVmProcedure().FullName}.几何创建1";
            }
            else
            {
                moduleName = $"{this.baseAlg.GetVmProcedure().FullName}.几何创建2";
            }
            IVmModule geometryCreateTool = (GeometryCreateTool)VmSolution.Instance[moduleName];
            paramsControl.ModuleSource = geometryCreateTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 中心查找区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置中心查找区域失败", LogCategory.PR)]
        private void BtnSetCenterReg_Click(object sender, EventArgs e)
        {
            IVmModule iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = iMVSBlobFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 交点查找是否使用粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置交点查找是否使用粗定位失败", LogCategory.PR)]
        private void ChkIsUseCrossCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseCrossCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘交点1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘交点2";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseCrossCrudePR.Checked;
            this.LueCrossCrudePR.Visible = ChkIsUseCrossCrudePR.Checked;
            this.BtnSetCrossCrudePR.Visible = ChkIsUseCrossCrudePR.Checked;

            //IMVSCaliperCornerModuTool iMVSCaliperCornerModuTool = (IMVSCaliperCornerModuTool)VmSolution.Instance[exactModuleName];
            //this.SpCrossEdgeStrength.Value = (decimal)iMVSCaliperCornerModuTool.ModuParams.EdgeStrength;
            //this.SpCrossKernelSize.Value = (decimal)iMVSCaliperCornerModuTool.ModuParams.KernelSize;
            //this.SpCrossProjectLen.Value = (decimal)iMVSCaliperCornerModuTool.ModuParams.ProjectLen;
            //this.SpCrossCaliperNum.Value = (decimal)iMVSCaliperCornerModuTool.ModuParams.CaliperNum;
            //this.LueCrossEdge0Polarity.EditValue = (EdgePolarityEnum)Enum.Parse(typeof(EdgePolarityEnum), iMVSCaliperCornerModuTool.ModuParams.Edge0Polarity.ToString());
            //this.LueCrossEdge0Type.EditValue = (EdgeTypeEnum)Enum.Parse(typeof(EdgeTypeEnum), iMVSCaliperCornerModuTool.ModuParams.Edge0Type.ToString());
            //this.LueCrossEdge1Polarity.EditValue = (EdgePolarityEnum)Enum.Parse(typeof(EdgePolarityEnum), iMVSCaliperCornerModuTool.ModuParams.Edge1Polarity.ToString());
            //this.LueCrossEdge1Type.EditValue = (EdgeTypeEnum)Enum.Parse(typeof(EdgeTypeEnum), iMVSCaliperCornerModuTool.ModuParams.Edge1Type.ToString());
        }

        /// <summary>
        /// 交点查找粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置交点查找粗定位模型失败", LogCategory.PR)]
        private void BtnSetCrossCrudePR_Click(object sender, EventArgs e)
        {
            string lueCrossCrudePRText = this.LueCrossCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueCrossCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueCrossCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 交点查找区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置交点查找区域失败", LogCategory.PR)]
        private void BtnCrossAusEdge_Click(object sender, EventArgs e)
        {
            if (ChkIsUseCrossCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘交点1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘交点2";
            }
            IMVSCaliperCornerModuTool iMVSCaliperCornerModuTool = (IMVSCaliperCornerModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCaliperCornerModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 基准查找是否使用粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置基准查找是否使用粗定位失败", LogCategory.PR)]
        private void ChkIsUseFiducialCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseFiducialCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.图形定位1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.图形定位2";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseFiducialCrudePR.Checked;
            this.LueFiducialCrudePR.Visible = ChkIsUseFiducialCrudePR.Checked;
            this.BtnSetFiducialCrudePR.Visible = ChkIsUseFiducialCrudePR.Checked;

            //IMVSMarkFindModuTool iMVSMarkFindModuTool = (IMVSMarkFindModuTool)VmSolution.Instance[exactModuleName];
            //this.SpFiducialExactAngle.Value = (decimal)iMVSMarkFindModuTool.ModuParams.AngleEnd;
            //this.SpFiducialExactScore.Value = (decimal)iMVSMarkFindModuTool.ModuParams.MinScore;
        }

        /// <summary>
        /// 基准查找的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置基准查找粗定位模板失败", LogCategory.PR)]
        private void BtnSetFiducialCrudePR_Click(object sender, EventArgs e)
        {
            string lueFiducialCrudePRText = this.LueFiducialCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueFiducialCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueFiducialCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 基准查找区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置基准查找区域失败", LogCategory.PR)]
        private void BtnSetFiducialReg_Click(object sender, EventArgs e)
        {
            IMVSMarkFindModuTool iMVSMarkFindModuTool = (IMVSMarkFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSMarkFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 线检测是否使用粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置线检测是否使用粗定位失败", LogCategory.PR)]
        private void ChkIsUseLineCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseLineCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找2";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseLineCrudePR.Checked;
            this.LueLineCrudePR.Visible = ChkIsUseLineCrudePR.Checked;
            this.BtnSetLineCrudePR.Visible = ChkIsUseLineCrudePR.Checked;

            //IMVSCaliperEdgeModuTool IMVSCaliperEdgeModuTool = (IMVSCaliperEdgeModuTool)VmSolution.Instance[exactModuleName];
            //this.LueLineEdgePolarity.EditValue = (EdgePolarityEnum)Enum.Parse(typeof(EdgePolarityEnum), IMVSCaliperEdgeModuTool.ModuParams.EdgePolarity.ToString());
            //this.LueLineFindMode.EditValue = (EdgeTypeEnum)Enum.Parse(typeof(EdgeTypeEnum), IMVSCaliperEdgeModuTool.ModuParams.FindMode.ToString());
            //this.SpLineContrastTH.Value = (decimal)IMVSCaliperEdgeModuTool.ModuParams.ContrastTH;
            //this.SpLineHalfKernelSize.Value = (decimal)IMVSCaliperEdgeModuTool.ModuParams.HalfKernelSize;
        }

        /// <summary>
        /// 线检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置线检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetLineCrudePR_Click(object sender, EventArgs e)
        {
            string lueLineCrudePRText = this.LueLineCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueLineCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueLineCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 线检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置线检测区域失败", LogCategory.PR)]
        private void BtnSetLineReg_Click(object sender, EventArgs e)
        {
            if (ChkIsUseLineCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找1";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找2";
            }
            IMVSLineFindModuTool IMVSCaliperEdgeModuTool = (IMVSLineFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = IMVSCaliperEdgeModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 创建基准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("设置焊后检测基准", LogCategory.PR)]
        private void BtnPostReferencePoint_Click(object sender, EventArgs e)
        {
            string ShellModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.脚本3";
            ShellModuleTool shellModuleTool = (ShellModuleTool)VmSolution.Instance[ShellModuleName];

            float[] Centerx = shellModuleTool.ModuResult.GetOutputFloat("outX").pFloatVal;
            float[] Centery = shellModuleTool.ModuResult.GetOutputFloat("outY").pFloatVal;

            this.baseAlg.ReferencePoint = new Infrastructure.CommonModel.AKRSPoint2D();
            this.baseAlg.ReferencePoint.X = Centerx[0];
            this.baseAlg.ReferencePoint.Y = Centery[0];
        }

        /// <summary>
        /// 带粗定位高精度检测的粗定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置带粗定位高精度检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetHighPreWithReferCrudePR_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配1";

            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 带粗定位高精度模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置带粗定位高精度匹配模型失败", LogCategory.PR)]
        private void BtnSetHighPreModelWithRefer_Click(object sender, EventArgs e)
        {

            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";

            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 带墨点检测的高精度模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置带墨点检测的高精度模型失败", LogCategory.PR)]
        private void BtnSetHighPreWithBlobCrudePR_Click(object sender, EventArgs e)
        {
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配1";

            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 带墨点检测的高精度Blob
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置带墨点检测的高精度Blob失败", LogCategory.PR)]
        private void BtnSetHighPreModelWithBlob_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
            IVmModule iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSBlobFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置距离测量的高精度模型1
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置距离测量的高精度模型1失败", LogCategory.PR)]
        private void BtnSetDistanceCrudePR_Click(object sender, EventArgs e)
        {

            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配1";

            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置距离测量的高精度模型2
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置距离测量的高精度模型2失败", LogCategory.PR)]
        private void BtnSetDistanceCircleReg_Click(object sender, EventArgs e)
        {

            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";

            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        private void BtnSetDistanceRecReg_Click(object sender, EventArgs e)
        {

        }

        private void LueAlgType_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 优化后矩形检测的粗定位模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置优化后矩形检测粗定位模型失败", LogCategory.PR)]
        private void BtnSetRecSecondPR_Click(object sender, EventArgs e)
        {
            string lueRecCrudePRText = this.LueRecSecondCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueRecCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueRecCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 优化后矩形检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置优化后矩形检测区域失败", LogCategory.PR)]
        private void BtnSetRecSecondDetect_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.矩形检测1";
            IVmModule iMVSRectFindModuTool = (IMVSRectFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSRectFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        private void LueRecCrudePR_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 优化后矩形检测直线查找区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置优化后矩形检测直线查找区域失败", LogCategory.PR)]
        private void BtnSetLineDetect_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找1";
            IVmModule iMVSLineFindModuTool = (IMVSLineFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSLineFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置对称模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置高精度匹配模型失败", LogCategory.PR)]
        private void BtnSetSymModel_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配1";

            IVmModule HPFeatureMatchModuTool =
                (IMVSHPFeatureMatchModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置缺角检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置缺角检测区域失败", LogCategory.PR)]
        private void BtnSetCornerBlob_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
            IVmModule iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSBlobFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 保存对称模型结果
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("保存对称模型结果失败", LogCategory.PR)]
        private void BtnSavepara_Click(object sender, EventArgs e)
        {
            if (matchResult.CenterX != 1224)
            {
                this.baseAlg.SymmetricCenterX = matchResult.CenterX;
                this.baseAlg.SymmetricCenterY = matchResult.CenterY;
            }
            else
            {
                DialogResult dialog = XtraMessageBox.Show(
                      $"保存失败，检查是否开启实时图像显示",
                      "提示",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Information);
            }

        }

        /// <summary>
        /// 设置对称模型1边缘
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置对称模型1边缘失败", LogCategory.PR)]
        private void BtnSetSymLineModel1_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘交点1";
            IMVSCaliperCornerModuTool iMVSCaliperCornerModuTool = (IMVSCaliperCornerModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCaliperCornerModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置对称模型2边缘
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置对称模型2边缘失败", LogCategory.PR)]
        private void BtnSetSymLineModel2_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘交点2";
            IMVSCaliperCornerModuTool iMVSCaliperCornerModuTool = (IMVSCaliperCornerModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCaliperCornerModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 防反检测的精定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置防反检测精定位模型失败", LogCategory.PR)]
        private void BtnSetReverseModel_Click(object sender, EventArgs e)
        {
            string lueReverseExactPRText = this.LueReverseExactPR.EditValue.ToString();
            this.baseAlg.ExactLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueReverseExactPRText);
            if (ChkIsUseReverseCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "3";
            }
            BindingModuleSource(lueReverseExactPRText, exactModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 防错检测的精定位模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置防错检测精定位模型失败", LogCategory.PR)]
        private void BtnSetWrongModel_Click(object sender, EventArgs e)
        {
            string lueWrongExactPRText = this.LueWrongExactPR.EditValue.ToString();
            this.baseAlg.ExactLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueWrongExactPRText);
            if (ChkIsUseWrongCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.ExactLocateType) + "3";
            }
            BindingModuleSource(lueWrongExactPRText, exactModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }


        /// <summary>
        /// 防错是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选防错粗定位失败", LogCategory.PR)]
        private void ChkIsUseWrongCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseWrongCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配3";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseWrongCrudePR.Checked;
            this.LueWrongCrudePR.Visible = ChkIsUseWrongCrudePR.Checked;
            this.BtnSetWrongCrudePR.Visible = ChkIsUseWrongCrudePR.Checked;
        }

        /// <summary>
        /// 防反是否需要粗定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选防反粗定位失败", LogCategory.PR)]
        private void ChkIsUseReverseCrudePR_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsUseReverseCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.高精度匹配3";
            }
            this.baseAlg.IsUseCrudeLocate = ChkIsUseReverseCrudePR.Checked;
            this.LueReverseCrudePR.Visible = ChkIsUseReverseCrudePR.Checked;
            this.BtnSetReverseCrudePR.Visible = ChkIsUseReverseCrudePR.Checked;
        }

        /// <summary>
        /// 防错的粗定位模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置防错粗定位模型失败", LogCategory.PR)]
        private void BtnSetWrongCrudePR_Click(object sender, EventArgs e)
        {
            string lueWrongCrudePRText = this.LueWrongCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueWrongCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueWrongCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 防反的粗定位模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置防反粗定位模型失败", LogCategory.PR)]
        private void BtnSetReverseCrudePR_Click(object sender, EventArgs e)
        {
            string lueReverseCrudePRText = this.LueReverseCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueReverseCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueReverseCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 胶量检测是否输出总值
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选胶量检测输出总值失败", LogCategory.PR)]
        private void ChkIsUseTotalArea_CheckedChanged(object sender, EventArgs e)
        {
            this.baseAlg.IsOutputTotal = ChkIsUseTotalArea.Checked;
        }





        /// <summary>
        /// 设置带缺角检测轮廓匹配的缺角参数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置带缺角检测轮廓匹配的缺角参数失败", LogCategory.PR)]
        private void BtnSetHighPreWithCornerBlob_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘模型缺陷检测1";
            IVmModule iMVSBlobFindModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSBlobFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }


        /// <summary>
        /// 设置带缺角检测轮廓匹配模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置带缺角检测轮廓匹配模型失败", LogCategory.PR)]
        private void BtnSetHighPreWithCornerCrudePR_Click(object sender, EventArgs e)
        {
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配1";

            IVmModule HPFeatureMatchModuTool =
                (IMVSContourMatchModuTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = HPFeatureMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        private void LuePostExactPR_EditValueChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 圆检测是否输出圆弧中点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选圆检测输出中点失败", LogCategory.PR)]
        private void ChkIsOutputMidpoint_CheckedChanged(object sender, EventArgs e)
        {
            this.baseAlg.IsOutputMidpoint = ChkIsOutputMidpoint.Checked;
        }



        /// <summary>
        /// 设置组合定位匹配模型
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置组合定位匹配模型失败", LogCategory.PR)]
        private void BtnSetHybridPositionPR_Click(object sender, EventArgs e)
        {
            
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配1";
            IMVSContourMatchModuTool iMVSContourMatchModuTool =
                (IMVSContourMatchModuTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = iMVSContourMatchModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置组合定位直线1
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置组合定位直线1失败", LogCategory.PR)]
        private void BtnSetHybridPositionLine1_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找1";
            IVmModule iMVSLineFindModuTool = (IMVSLineFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSLineFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置组合定位直线2
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置组合定位直线2失败", LogCategory.PR)]
        private void BtnSetHybridPositionLine2_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.直线查找2";
            IVmModule iMVSLineFindModuTool = (IMVSLineFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSLineFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// FC上视定位模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置FC上视的定位模板失败", LogCategory.PR)]
        private void BtnSetFcUplookPR_Click(object sender, EventArgs e)
        {

            string lueCrudePRText = this.LueFcUplookCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置FC bump球检测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置FC bump球检测失败", LogCategory.PR)]
        private void BtnSetFcUplookBump_Click(object sender, EventArgs e)
        {

            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.BLOB分析1";
            IVmModule iMVSBlobFindModuTool = (IMVSBlobFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSBlobFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);

        }

        /// <summary>
        /// 设置FC缺角检测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置FC 缺角检测失败", LogCategory.PR)]
        private void BtnSetFcUplookCorner_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘模型缺陷检测1";
            IVmModule iMVSEdgeFlawModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSEdgeFlawModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置FC崩边检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置FC崩边检测区域失败", LogCategory.PR)]
        private void BtnSetEdgeRegion_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.边缘模型缺陷检测1";
            IVmModule iMVSEdgeFlawModuTool = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSEdgeFlawModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 创建表面缺陷检测区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("创建表面缺陷检测区域失败", LogCategory.PR)]
        private void btnCreateDetectRegion_Click(object sender, EventArgs e)
        {
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + "几何创建1";
            IVmModule vmModule = (GeometryCreateCs.GeometryCreateTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = vmModule;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        public Bitmap bmp;

        /// <summary>
        /// 保存缺陷检测图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("保存缺陷检测图像失败", LogCategory.PR)]
        private void btnSaveModelImage_Click(object sender, EventArgs e)
        {
            GeometryCreateTool geometryCreateTool = (GeometryCreateCs.GeometryCreateTool)VmSolution.Instance[crudeModuleName];

            string CrudeModuleName1 = $"{this.baseAlg.GetVmProcedure().FullName}." + "位置修正1";
            IMVSFixtureModuTool fixtureModuTool = (IMVSFixtureModuTool)VmSolution.Instance[CrudeModuleName1];
            fixtureModuTool.ModuParams.DoFixtureInit();

            List<RectBox> rectbox = geometryCreateTool.ModuResult.ListOutputResultBox;

            if (rectbox.Count == 0)
            {
                MessageBox.Show("请绘制检测区域!");
            }
            else
            {
                HObject image = AKRS.Galaxy2.PR.Models.Services.ImageHelp.BitmapToImg(this.bmp);

                HOperatorSet.GenRectangle2(out HObject rectangle, rectbox[0].CenterPoint.Y, rectbox[0].CenterPoint.X, new HTuple(-rectbox[0].Angle).TupleRad(), rectbox[0].BoxWidth / 2f, rectbox[0].BoxHeight / 2f);
                HOperatorSet.ReduceDomain(image, rectangle, out HObject imageReduced);

                string modelImagePath = this.baseAlg.AlgSavePath + this.baseAlg.Name;

                // 保存模板图像
                HOperatorSet.WriteImage(imageReduced, "bmp", 0, modelImagePath);
                MessageBox.Show("模板图像保存成功.");
            }
        }

        /// <summary>
        /// 设置表面检测的缺陷阈值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("设置表面检测的缺陷阈值失败", LogCategory.PR)]
        private void spThreshold_EditValueChanged(object sender, EventArgs e)
        {
            this.baseAlg.Threshold = Convert.ToInt32(this.spThreshold.Value);
        }

        /// <summary>
        /// 设置表面检测的缺陷面积
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("设置表面检测的缺陷面积失败", LogCategory.PR)]
        private void spArea_EditValueChanged(object sender, EventArgs e)
        {
            this.baseAlg.Area = Convert.ToInt32(this.spArea.Value);
        }

        /// <summary>
        /// 设置崩边检测边缘
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HandleExceptionAspect("设置崩边检测边缘失败", LogCategory.PR)]
        private void btnCreateEdge_Click(object sender, EventArgs e)
        {
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + "边缘模型缺陷检测1";
            IVmModule vmModule = (IMVSEdgeFlawInspModuTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = vmModule;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置二维码检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置二维码检测区域失败", LogCategory.PR)]
        private void BtnSetQrCodeRegion_Click(object sender, EventArgs e)
        {
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + "二维码识别1";
            IVmModule vmModule = (IMVS2dBcrModuTool)VmSolution.Instance[crudeModuleName];
            paramsControl.ModuleSource = vmModule;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 矩形检测2是否输出长宽尺寸
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选矩形检测2输出长宽尺寸失败", LogCategory.PR)]
        private void ChkIsUseRecLength_CheckedChanged(object sender, EventArgs e)
        {
            this.baseAlg.IsUseRecLength = ChkIsUseRecLength.Checked;
        }

        /// <summary>
        /// 轮廓检测是否需要使用粗定位角度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("勾选轮廓检测粗定位角度失败", LogCategory.PR)]
        private void ChkIsUseXldCrudeAngle_CheckedChanged(object sender, EventArgs e)
        {
            this.baseAlg.IsUseCrudeLocateAngle = this.ChkIsUseXldCrudeAngle.Checked;

            if (ChkIsUseXldCrudePR.Checked)
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配2";
            }
            else
            {
                exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.轮廓匹配3";
            }
            // InitModuleParam(LocateTypeEnum.XldModel, exactModuleName, this.SpXldExactAngle, this.SpXldExactScore);
        }

        /// <summary>
        /// 圆心和角度的粗定位模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置圆心和角度粗定位模型失败", LogCategory.PR)]

        private void BtnSetcirclelinePR_Click(object sender, EventArgs e)
        {
            string lueCrudePRText = this.LuecirclelineCrudePR.EditValue.ToString();
            this.baseAlg.CrudeLocateType = (LocateTypeEnum)Enum.Parse(typeof(LocateTypeEnum), lueCrudePRText);
            crudeModuleName = $"{this.baseAlg.GetVmProcedure().FullName}." + EnumHelper.GetDescription(this.baseAlg.CrudeLocateType) + "1";
            BindingModuleSource(lueCrudePRText, crudeModuleName);
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置圆1检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置圆1检测区域失败", LogCategory.PR)]
        private void BtnSetCircle1Detect_Click(object sender, EventArgs e)
        {
            
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找1";
            
            IVmModule iMVSCircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCircleFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置圆2检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置圆2检测区域失败", LogCategory.PR)]
        private void BtnSetCircle2Detect_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找2";

            IVmModule iMVSCircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCircleFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置圆3检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置圆3检测区域失败", LogCategory.PR)]
        private void BtnSetCircle3Detect_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找3";

            IVmModule iMVSCircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCircleFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }

        /// <summary>
        /// 设置圆4检测区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        [HandleExceptionAspect("设置圆4检测区域失败", LogCategory.PR)]
        private void BtnSetCircle4Detect_Click(object sender, EventArgs e)
        {
            exactModuleName = $"{this.baseAlg.GetVmProcedure().FullName}.圆查找4";

            IVmModule iMVSCircleFindModuTool = (IMVSCircleFindModuTool)VmSolution.Instance[exactModuleName];
            paramsControl.ModuleSource = iMVSCircleFindModuTool;
            PnlVmControl.Controls.Clear();
            PnlVmControl.Controls.Add(paramsControl);
        }
    }
}
