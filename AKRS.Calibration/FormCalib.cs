using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using DevExpress.XtraPrinting.Native;

using GlobalVariableModuleCs;

using VM.Core;
using VM.PlatformSDKCS;

using PointF = System.Drawing.PointF;

namespace AKRS.Calibration
{
    using AKRS.Calibration.Controls;

    /// <summary>
    /// 标定测试窗体
    /// </summary>
    public partial class FormCalib : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 窗体单例
        /// </summary>
        private static FormCalib formCalib;

        /// <summary>
        /// 创建单例
        /// </summary>
        /// <returns>窗体实例</returns>
        public static FormCalib CreateInstrance()
        {
            if (formCalib == null  || formCalib.IsDisposed)
            {
                formCalib = new FormCalib();
            }
            return formCalib;
        }

        /// <summary>
        /// The control for display picture
        /// </summary>
        private RenderControl renderControl;

        /// <summary>
        /// The control for configure parameter
        /// </summary>
        private MainViewControl mainViewControl;

        /// <summary>
        /// Solution Path
        /// </summary>
        private string currentSolutionPath = string.Empty;

        /// <summary>
        /// Process List
        /// </summary>
        private List<VmProcedure> processList;

        /// <summary>
        /// isSolutionLoad = false, indicates that the solution is not loaded
        /// </summary>
        private bool solutionIsLoaded = false;

        /// <summary>
        /// 计时器
        /// </summary>
        private readonly System.Windows.Forms.Timer loadSolutionIndicateTimer = new System.Windows.Forms.Timer();

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormCalib()
        {
            this.InitializeComponent();
            this.renderControl = new RenderControl();

            // mainViewControl = new MainViewControl();

            // mainViewControl.Lock();
            this.renderControl.Dock = DockStyle.Fill;

            // mainViewControl.Dock = DockStyle.Fill;
            this.BtnRender.BackColor = Color.Orange;
            this.BtnConfig.BackColor = Color.Gray;
            this.loadSolutionIndicateTimer.Interval = 300;
            this.loadSolutionIndicateTimer.Tick += this.LoadSolutionIndicateTimer_Tick;
            this.InitSolutiion();

            // mainViewControl.single("1下相机12点");
            VmSolution.OnProcessStatusStartEvent += this.VmSolution_OnProcessStatusStartEvent;   // Registration callback for the start of the procedure continuous run
            VmSolution.OnProcessStatusStopEvent += this.VmSolution_OnProcessStatusStopEvent; // Registration callback for the stop of the procedure continuous run
        }

        /// <summary>
        /// 初始化方案
        /// </summary>
        public void InitSolutiion()
        {
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");
            }
            catch (Exception)
            {
                MessageBox.Show("方案初始化失败");
            }
        }

        /// <summary>
        /// Callback function for the start of the procedure continuous run
        /// </summary>
        /// <param name="statusInfo">状态</param>
        private void VmSolution_OnProcessStatusStartEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_START_CONTINUOUSLY_INFO statusInfo)
        {
            this.Invoke(new Action(() =>
            {
                if (statusInfo.nStatus == 0)
                {
                    string strMessage = null;

                    // buttonContiRun.Text = "Run Stop";

                    // Disable button
                    BtnSelect.Enabled = false;

                    // buttonRunOnce.Enabled = false;
                    BtnLoad.Enabled = false;
                    BtnSave.Enabled = false;

                    // comboProcedure.Enabled = false;
                    strMessage = "Start continuous run!";
                    AppendLog(strMessage);
                }
            }));
        }

        /// <summary>
        /// Callback function for the stop of the procedure continuous run
        /// </summary>
        /// <param name="statusInfo">状态</param>
        private void VmSolution_OnProcessStatusStopEvent(ImvsSdkDefine.IMVS_STATUS_PROCESS_STOP_INFO statusInfo)
        {
            if (this.IsHandleCreated)
            {
                this.Invoke(new Action(() =>
                {
                    if (statusInfo.nStopAction == 1)
                    {
                        string strMessage = null;

                        // buttonContiRun.Text = "Run Continuous";

                        // Enable button
                        BtnSelect.Enabled = true;

                        // buttonRunOnce.Enabled = true;
                        BtnLoad.Enabled = true;

                        // buttonSaveSolu.Enabled = true;

                        // comboProcedure.Enabled = true;
                        strMessage = "End Run!";
                        AppendLog(strMessage);
                    }
                }));
            }

        }

        /// <summary>
        /// 图像
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnRender_Click(object sender, EventArgs e)
        {
            string processName = this.comboProcedure.SelectedItem.ToString();
            VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;
            this.renderControl.ModuleSource = procedure;
            this.renderPanel.Controls.Clear();
            this.renderPanel.Controls.Add(this.renderControl);
            this.BtnRender.BackColor = Color.Orange;
            this.BtnConfig.BackColor = Color.Gray;
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnConfig_Click(object sender, EventArgs e)
        {
            this.renderPanel.Controls.Clear();
            this.renderPanel.Controls.Add(this.mainViewControl);
            BtnRender.BackColor = Color.Gray;
            BtnConfig.BackColor = Color.Orange;
        }

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "VM Sol File(*.sol)|*.sol";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    this.solutionIsLoaded = false;
                    this.currentSolutionPath = ofd.FileName;
                    this.loadSolutionIndicateTimer.Enabled = true;
                    this.AppendLog("The solution path is: " + this.currentSolutionPath);
                    MessageBox.Show(
                        "The solution path is: " + this.currentSolutionPath + "\nNext click the Load solution button!",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    this.currentSolutionPath = string.Empty;
                }
            }
            catch (Exception ex)
            {
                this.AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// Append Log
        /// </summary>
        /// <param name="message">信息</param>
        private void AppendLog(string message)
        {
            try
            {
                string timeStamp = DateTime.Now.ToString("yy-MM-dd HH:mm:ss-fff");
                if (this.listViewLog.Items.Count > 10000)
                {
                    this.listViewLog.Items.Clear();
                }

                if (this.listViewLog.InvokeRequired)
                {
                    this.listViewLog.BeginInvoke(new Action(() =>
                    {
                        listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, message }));
                    }));
                }
                else
                {
                    this.listViewLog.Items.Insert(0, new ListViewItem(new string[] { timeStamp, message }));
                }

                if (!Directory.Exists("./log"))
                {
                    Directory.CreateDirectory("./log");
                }
                using (FileStream fs = new FileStream("./log/LocateDemoCs.log", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(timeStamp + ":" + message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            this.loadSolutionIndicateTimer.Enabled = false;
            this.BtnLoad.BackColor = Color.Orange;
            this.BtnLoad.Enabled = false;

            // Disable button
            this.BtnSelect.Enabled = false;

            // buttonRunOnce.Enabled = false;
            // buttonContiRun.Enabled = false;
            this.BtnSave.Enabled = false;

            // comboProcedure.Enabled = false;
            this.BtnRender.Enabled = false;
            this.BtnConfig.Enabled = false;
            try
            {
                if (this.currentSolutionPath != string.Empty)
                {
                    VmSolution.Load(this.currentSolutionPath);
                    this.processList = this.GetCurrentSolProcedureList();
                    this.UpdateProcessComboBox(this.processList);
                    this.RegisterProcedureWorkEndCallback(this.processList);//Registration callback for the end of the procedure run
                    this.solutionIsLoaded = true;
                    this.renderControl.ModuleSource = this.processList[0];
                    this.AppendLog("Loading Solution succeeded!");
                    MessageBox.Show("Loading Solution succeeded!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                this.listViewLog.Items.Add(
                    new ListViewItem(new string[] { DateTime.Now.ToString("YY-MM-DD HH:mm:ss-fff"), ex.Message }));
            }
            finally
            {
                this.BtnLoad.BackColor = Color.DimGray;
                this.BtnLoad.Enabled = true;

                // Enable button
                this.BtnSelect.Enabled = true;

                // buttonRunOnce.Enabled = true;
                // buttonContiRun.Enabled = true;
                this.BtnSave.Enabled = true;

                // comboProcedure.Enabled = true;
                this.BtnRender.Enabled = true;
                this.BtnConfig.Enabled = true;
            }
        }

        /// <summary>
        /// Obtain all processes in the solution
        /// </summary>
        /// <returns>processList</returns>
        private List<VmProcedure> GetCurrentSolProcedureList()
        {
            List<VmProcedure> procedureList = new List<VmProcedure>();
            string processName = string.Empty;
            var processInfoList = VmSolution.Instance.GetAllProcedureList();
            for (int i = 0; i < processInfoList.nNum; i++)
            {
                processName = processInfoList.astProcessInfo[i].strProcessName;
                procedureList.Add((VmProcedure)VmSolution.Instance[processName]);
            }

            return procedureList;
        }

        /// <summary>
        /// Registration callback for the end of the procedure run
        /// </summary>
        /// <param name="lst">proList</param>
        public void RegisterProcedureWorkEndCallback(List<VmProcedure> lst)
        {
            try
            {
                foreach (var vmProcedure in this.processList)
                {
                    vmProcedure.OnWorkEndStatusCallBack += this.VmProcedure_OnWorkEndStatusCallBack;
                }
            }
            catch (Exception ex)
            {
                this.AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// Registration callback for the end of the procedure run
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void VmProcedure_OnWorkEndStatusCallBack(object sender, EventArgs e)
        {
            try
            {
                VmProcedure procedure = sender as VmProcedure;
                if (procedure != null)
                {
                    var outputList = procedure.ModuResult.GetAllOutputNameInfo();
                    bool outputConfigIsWrong = true;
                    foreach (var ioNameInfo in outputList)
                    {
                        if (ioNameInfo.Name == "out" &&
                            ioNameInfo.TypeName == IMVS_MODULE_BASE_DATA_TYPE.IMVS_GRAP_TYPE_STRING)
                        {
                            var result = procedure.ModuResult.GetOutputString(ioNameInfo.Name);
                            string resultStrValue = result.astStringVal[0].strValue;
                            this.AppendLog("Result: " + resultStrValue);

                            // UpdateResultListBox("Result: " + resultStrValue);
                            // UpdateLableState(resultStrValue);
                            outputConfigIsWrong = false;
                        }
                    }
                    if (outputConfigIsWrong)
                    {
                        this.AppendLog("The result argument (out) is not exit or is not string format！");
                    }
                    this.AppendLog("Time" + procedure.ProcessTime);
                }
            }
            catch (Exception ex)
            {
                this.AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.solutionIsLoaded)
                {
                    VmSolution.Save();
                    this.AppendLog("Succeeded to save solution!");
                }
            }
            catch (Exception ex)
            {
                this.AppendLog(ex.Message);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void LoadSolutionIndicateTimer_Tick(object sender, EventArgs e)
        {
            if (!this.solutionIsLoaded)
            {
                if (this.BtnLoad.BackColor == Color.DimGray)
                {
                    this.BtnLoad.BackColor = Color.Orange;
                }
                else
                {
                    this.BtnLoad.BackColor = Color.DimGray;
                }
            }
            if (this.solutionIsLoaded)
            {
                this.BtnLoad.BackColor = Color.DimGray;
            }
        }

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns>prolist</returns>
        public List<string> GetAllProcedureName()
        {
            this.processList = this.GetCurrentSolProcedureList();
            List<string> procedureName = new List<string>();

            for (int i = 0; i < this.processList.Count; i++)
            {
                procedureName.Add(this.processList[i].Name);
            }
            return procedureName;
        }


        /// <summary>
        /// UpdateProcessComboBox
        /// </summary>
        /// <param name="lst">prolist</param>
        private void UpdateProcessComboBox(List<VmProcedure> lst)
        {
            this.comboProcedure.Properties.Items.Clear();
            foreach (var vmProcedure in lst)
            {
                this.comboProcedure.Properties.Items.Add(vmProcedure.Name);
            }
            if (this.comboProcedure.Properties.Items.Count > 0)
            {
                this.comboProcedure.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.renderPanel.Controls.Clear();
            this.renderPanel.Controls.Add(this.mainViewControl);

            // renderPanel.Controls.Add(renderControl);
            // renderPanel.Controls.Remove(mainViewControl);
            this.processList = this.GetCurrentSolProcedureList();
            this.UpdateProcessComboBox(this.processList);
        }

        /// <summary>
        /// 选择流程
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnSelectPrc_Click(object sender, EventArgs e)
        {
            string processName = this.comboProcedure.SelectedItem.ToString();
            VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;
            this.renderControl.ModuleSource = procedure;
        }

        /// <summary>
        /// 运行一次
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnRunOnce_Click(object sender, EventArgs e)
        {
            string processName = this.comboProcedure.SelectedItem.ToString();
            VmProcedure procedure = VmSolution.Instance[processName] as VmProcedure;

            procedure.Run();
        }

        /// <summary>
        /// 点胶标定
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool DripCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");
                VmProcedure procedure = VmSolution.Instance["4点胶9点标定"] as VmProcedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);

                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                procedure.Run();

                FloatDataArray cameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                FloatDataArray transXArray = procedureResult.GetOutputFloat("TransX");
                FloatDataArray transYArray = procedureResult.GetOutputFloat("TransY");

                if (cameraScaleArray.nValueNum != 0)
                {
                    cameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                }
                if (transXArray.nValueNum != 0)
                {
                    transX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                }
                if (transYArray.nValueNum != 0)
                {
                    transY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                }

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Bond相机9点标定接口
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool BondCamCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");
                VmProcedure procedure = VmSolution.Instance["3上相机12点"] as VmProcedure;
                this.renderControl.ModuleSource = procedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);

                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                procedure.Run();

                // FloatDataArray CameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                // FloatDataArray TransXArray = procedureResult.GetOutputFloat("TransX");
                // FloatDataArray TransYArray = procedureResult.GetOutputFloat("TransY");

                // if (CameraScaleArray.nValueNum != 0)
                // {
                //     CameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                // }
                // if (TransXArray.nValueNum != 0)
                // {
                //    TransX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                // }
                // if (TransYArray.nValueNum != 0)
                // {
                //    TransY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                //// }

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 晶圆相机9点标定接口
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool WaferCamCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");

                VmProcedure procedure = VmSolution.Instance["5晶圆相机9点"] as VmProcedure;
                this.renderControl.ModuleSource = procedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);
                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                // FloatDataArray CameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                // FloatDataArray TransXArray = procedureResult.GetOutputFloat("TransX");
                //// FloatDataArray TransYArray = procedureResult.GetOutputFloat("TransY");

                procedure.Run();

                /*
                if (CameraScaleArray.nValueNum != 0)
                {
                    CameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                }
                if (TransXArray.nValueNum != 0)
                {
                    TransX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                }
                if (TransYArray.nValueNum != 0)
                {
                    TransY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                }
                */

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 上视相机9点标定接口
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool UpLookCamCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");

                VmProcedure procedure = VmSolution.Instance["1下相机12点"] as VmProcedure;
                this.renderControl.ModuleSource = procedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);

                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                procedure.Run();

                /*
                FloatDataArray CameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                FloatDataArray TransXArray = procedureResult.GetOutputFloat("TransX");
                FloatDataArray TransYArray = procedureResult.GetOutputFloat("TransY");

                if (CameraScaleArray.nValueNum != 0)
                {
                    CameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                }
                if (TransXArray.nValueNum != 0)
                {
                    TransX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                }
                if (TransYArray.nValueNum != 0)
                {
                    TransY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                }
                */

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FormCalib_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.loadSolutionIndicateTimer != null)
            {
                this.loadSolutionIndicateTimer.Stop();
                this.loadSolutionIndicateTimer.Tick -= this.LoadSolutionIndicateTimer_Tick;
                this.loadSolutionIndicateTimer.Dispose();
            }

        }
    }
}
