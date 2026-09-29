using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    using System.Collections.ObjectModel;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 画胶原点
    /// </summary>
    public partial class EpoxyPatternForm : XtraForm
    {
        /// <summary>
        /// 曲线是否显示 列索引
        /// </summary>
        internal const int CURVE_COLLECTION_VISIBLE = 1;

        /// <summary>
        /// 曲线删除 列索引
        /// </summary>
        internal const int CURVE_COLLECTION_DELETE = 2;

        /// <summary>
        /// 速度的列索引
        /// </summary>
        internal const int SEGMENT_MAX_SPEED = 1;

        /// <summary>
        /// 末端的列索引
        /// </summary>
        internal const int SEGMENT_END_SPEED = 2;

        /// <summary>
        /// 加速度的列索引
        /// </summary>
        internal const int SEGMENT_ACC = 3;

        internal const int SEGMENT_TIME = 4;
        internal const int SEGMENT_DUMMIED = 5;
        internal const int SEGMENT_PREDELAY = 6;
        internal const int SEGMENT_POSTDELAY = 7;

        internal const int ENDPOINT_INDEX = 1;
        internal const int ENDPOINT_Z = 4;

        internal const int HEIGHT_MEASUREMENT = 5; //测高那一列


        internal static readonly Dictionary<ActionEnum, string> s_actionReadableNameDict;

        //---------------------------------------------------------------------------------

        /// <summary>
        /// 当前模板名称
        /// </summary>
        private string currentPatternName;

        /// <summary>
        /// 像素比
        /// </summary>
        private float micronPerPixel;

        /// <summary>
        /// 芯片宽度
        /// </summary>
        private float dieWidth;

        /// <summary>
        /// 芯片高度
        /// </summary>
        private float dieHeight;

        //---------------------------------------------------------------------------------

        /// <summary>
        /// 静态构造函数，只执行一次
        /// </summary>
        static EpoxyPatternForm()
        {
            s_actionReadableNameDict = new Dictionary<ActionEnum, string>
            {
                { ActionEnum.None, string.Empty },
                { ActionEnum.TranslateBackgroundImage, TextResource.ACTION_TRANSLATE_BACKGROUND_IMAGE },
                { ActionEnum.RotateBackgroundImage, TextResource.ACTION_ROTATE_BACKGROUND_IMAGE },
                { ActionEnum.SelectObject, TextResource.ACTION_SELECT_OBJECT },
                { ActionEnum.PaintCurve, TextResource.ACTION_PAINT_CURVE },
                { ActionEnum.InsertPoint, TextResource.ACTION_INSERT_POINT },
                { ActionEnum.DeletePoint, TextResource.ACTION_DELETE_POINT },
                { ActionEnum.AppendPoint, TextResource.ACTION_APPEND_POINT },
                { ActionEnum.MovePoint, TextResource.ACTION_MOVE_POINT },
                { ActionEnum.ScaleCurve, TextResource.ACTION_SCALE_CURVE },
                { ActionEnum.RotateCurve, TextResource.ACTION_ROTATE_CURVE },
                { ActionEnum.TranslateCurve, TextResource.ACTION_TRANSLATE_CURVE }
            };
        }

        /// <summary>
        /// 根据Action 获取指令名称
        /// </summary>
        /// <param name="actionEnum">actionEnum</param>
        /// <returns>名称</returns>
        public static string GetActionReadableName(ActionEnum actionEnum)
        {
            return s_actionReadableNameDict[actionEnum];
        }

        /// <summary>
        /// 版本
        /// </summary>
        /// <returns>版本号</returns>
        //private static string GetAssemblyVersion()
        //{
        //    Assembly assembly = Assembly.GetExecutingAssembly();
        //    Version version = assembly.GetName().Version;
        //    return $"{version.Major}.{version.Minor}.{version.Build}";
        //}

        /// <summary>
        /// 获取模版仓库的路径
        /// </summary>
        /// <returns>模版仓库的路径</returns>
        private static string GetPatternRepositoryPath()
        {
            // 需要修改
            Assembly assembly = Assembly.GetExecutingAssembly();
            string strAssemblyDirPath = Path.GetDirectoryName(assembly.Location);

            return Path.Combine(strAssemblyDirPath, "PatternRepository");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EpoxyPatternForm()
        {
            InitializeComponent();

            this.micronPerPixel = 50;
            this.dieWidth = 350;
            this.dieHeight = 300;

            this.currentPatternName = string.Empty;

            // 初始化按钮 给按钮的Tag赋值
            foreach (Control control in this.PnlCurveActions.Controls)
            {
                if (control is SimpleButton button)
                {
                    // PaintCurve, SelectObject ......
                    // 根据按钮的名称把 Tag 的属性赋为对应的枚举
                    button.Tag = (ActionEnum)Enum.Parse(typeof(ActionEnum), button.Name.Substring(2));
                }
            }
        }

        /// <summary>
        /// 初始化 GvSegmentEndpoints
        /// </summary>
        private void InitGvSegmentEndpoints()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("PointType", typeof(string));
            dt.Columns.Add("Index", typeof(int));
            dt.Columns.Add("X", typeof(double));
            dt.Columns.Add("Y", typeof(double));
            dt.Columns.Add("Z", typeof(double));
            dt.Columns.Add("HeightMeasurement", typeof(bool));
            this.GcSegmentEndpoints.DataSource = dt;

            DataRow row = dt.Rows.Add();
            row["PointType"] = "起点";
            row["Index"] = 1;
            row["X"] = double.NaN;
            row["Y"] = double.NaN;
            row["Z"] = double.NaN;

            row = dt.Rows.Add();
            row["PointType"] = "终点";
            row["Index"] = 2;
            row["X"] = double.NaN;
            row["Y"] = double.NaN;
            row["Z"] = double.NaN;
        }

        /// <summary>
        /// Load事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void EpoxyPatternForm_Load(object sender, EventArgs e)
        {
            // 转为像素尺寸
            float width = this.dieWidth / this.micronPerPixel;
            float height = this.dieHeight / this.micronPerPixel;

            // 设置芯片大小
            this.EpoxyPatternControl.SetDieSize(width, height);
            this.InitGvSegmentEndpoints();
            this.LbAction.Text = string.Empty;

            // this.dataGridViewCurveSegments.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
        }

        /// <summary>
        /// 刷新表格  待优化
        /// </summary>
        private void RefreshDataGridVeiws()
        {
            RefreshDataGridViewCurveCollection();
            RefreshDataGridViewCurveSegments();
        }

        /// <summary>
        /// 刷新曲线表格  待优化
        /// </summary>
        private void RefreshDataGridViewCurveCollection()
        {
            this.dataGridViewCurveCollection.SuspendLayout();

            this.dataGridViewCurveCollection.Rows.Clear();

            int iCurveIndex = 0;
            foreach (Curve curve in this.EpoxyPatternControl.CurveList)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.dataGridViewCurveCollection);

                row.Cells[0].Value = $"{iCurveIndex + 1}";
                row.Cells[1].Value = curve.Visible;

                this.dataGridViewCurveCollection.Rows.Add(row);
                iCurveIndex++;
            }

            this.dataGridViewCurveCollection.ResumeLayout();
        }

        /// <summary>
        /// 刷新表格  待优化
        /// </summary>
        private void RefreshDataGridViewCurveSegments()
        {
            this.dataGridViewCurveSegments.SuspendLayout();

            this.dataGridViewCurveSegments.Rows.Clear();

            if (this.EpoxyPatternControl.CurveCount > 0 && this.EpoxyPatternControl.CurrentCurveIndex != EpoxyPatternControl.INVALID_CURVE_INDEX)
            {
                Curve currentCurve = this.EpoxyPatternControl.GetCurrentCurve();

                // Debug.Assert(currentCurve.EndpointCount > 1);

                int iSegmentCount = currentCurve.EndpointCount - 1;
                if (iSegmentCount > 0)
                {
                    int iSegmentIndex = 0;
                    foreach (EndpointUnit endpointUnit in currentCurve.EndpointUnitList)
                    {
                        var row = CreateDataGridViewRowOfSegmentCollection(iSegmentIndex, endpointUnit.EndpointInfo);
                        this.dataGridViewCurveSegments.Rows.Add(row);

                        iSegmentIndex++;
                        if (iSegmentIndex >= iSegmentCount)
                        {
                            break;
                        }
                    }
                }
            }

            this.dataGridViewCurveSegments.ResumeLayout();
        }

        /// <summary>
        /// 刷新表格  待优化
        /// </summary>
        private void RefreshDataGridViewEndpointCollection()
        {
            this.dataGridViewCurveCollection.SuspendLayout();

            if (this.EpoxyPatternControl.CurveCount > 0)
            {
            }
            else
            {
                this.InitGvSegmentEndpoints();
            }

            this.dataGridViewCurveCollection.ResumeLayout();
        }

        /// <summary>
        /// 添加一行数据
        /// </summary>
        /// <param name="iSegmentInitialEndpointIndex">线段索引</param>
        /// <param name="segmentInitialEndpointInfo">线段数据</param>
        /// <returns>DataGridViewRow</returns>
        private DataGridViewRow CreateDataGridViewRowOfSegmentCollection(int iSegmentInitialEndpointIndex, EndpointSetting segmentInitialEndpointInfo)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(this.dataGridViewCurveSegments);

            row.SetValues(
                $"{iSegmentInitialEndpointIndex + 1}",  // INDEX
                segmentInitialEndpointInfo.MaxSpeed,    // MAX_SPEED
                segmentInitialEndpointInfo.EndSpeed,    // END_SPEED
                segmentInitialEndpointInfo.Acc,         // ACC
                segmentInitialEndpointInfo.Time,        // TIME
                segmentInitialEndpointInfo.Dummied,     // DUMMIED
                segmentInitialEndpointInfo.PreDelay,    // PREDELAY
                segmentInitialEndpointInfo.PostDelay);   // POSTDELAY

            return row;
        }

        #region DialogResult buttons.

        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 点击取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        #endregion
        /// <summary>
        /// 执行Action
        /// </summary>
        /// <param name="actionEnum">actionEnum</param>
        private void ExecuteAction(ActionEnum actionEnum)
        {
            this.dataGridViewCurveSegments.ClearSelection();

            this.EpoxyPatternControl.SetAction(actionEnum);
        }

        #region Curve actionEnum buttons.

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonAppendPointSegment_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.AppendPoint);
        }

        /// <summary>
        /// 插入点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonInsertPoint_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.InsertPoint);
        }

        /// <summary>
        /// 移动点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonMovePoint_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.MovePoint);
        }

        /// <summary>
        /// 删除点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonDeletePoint_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.DeletePoint);
        }

        /// <summary>
        /// 选择按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonSelectObject_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.None);
        }

        /// <summary>
        /// 添加线段
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAddCurve_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.PaintCurve);
        }

        /// <summary>
        /// 移动曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonTranslateCurve_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.TranslateCurve);
        }

        /// <summary>
        /// 移动曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonScaleCurve_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.ScaleCurve);
        }

        /// <summary>
        /// 旋转曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonRotateCurve_Click(object sender, EventArgs e)
        {
            ExecuteAction(ActionEnum.RotateCurve);
        }

        /// <summary>
        /// 吸附
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkXiFu_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            this.EpoxyPatternControl.SetAttachable(checkBox.Checked);
        }

        #endregion

        #region Curve collection buttons.

        private void EnableCurveCollectionButtons()
        {
            //this.flowLayoutPanelCurveCollectionActions.Enabled = true;
            //this.buttonRemoveAllCurves.Enabled = true;
            //this.buttonHideAllCurves.Enabled = true;
            //this.buttonShowAllCurves.Enabled = true;
        }

        private void DisableCurveCollectionButtons()
        {
            //this.flowLayoutPanelCurveCollectionActions.Enabled = false;
            //this.buttonRemoveAllCurves.Enabled = false;
            //this.buttonHideAllCurves.Enabled = false;
            //this.buttonShowAllCurves.Enabled = false;
        }

        /// <summary>
        /// 清空线段
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonRemoveAllCurves_Click(object sender, EventArgs e)
        {
            this.EpoxyPatternControl.SetAction(ActionEnum.None);
            this.EpoxyPatternControl.RemoveAllCurves();            
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        //private void buttonHideAllCurves_Click(object sender, EventArgs e)
        //{
        //    if (this.epoxyPatternControl.ActionEnum == ActionEnum.None)
        //    {
        //        this.epoxyPatternControl.HideAllCurves();

        //        foreach (DataGridViewRow row in this.dataGridViewCurveCollection.Rows)
        //        {
        //            row.Cells[1].Value = false;
        //        }
        //    }
        //    else
        //    {
        //        int iCurrentCurveIndex = this.epoxyPatternControl.CurrentCurveIndex;
        //        int iCurveCount = this.epoxyPatternControl.CurveCount;
        //        for (int iCurveIndex = 0; iCurveIndex < iCurveCount; iCurveIndex++)
        //        {
        //            if (iCurveIndex != iCurrentCurveIndex)
        //            {
        //                this.epoxyPatternControl.HideCurve(iCurveIndex);
        //                this.dataGridViewCurveCollection.Rows[iCurveIndex].Cells[1].Value = false;
        //            }
        //        }                
        //    }
        //}

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        //private void buttonShowAllCurves_Click(object sender, EventArgs e)
        //{
        //    this.epoxyPatternControl.ShowAllCurves();

        //    foreach (DataGridViewRow row in this.dataGridViewCurveCollection.Rows)
        //    {
        //        row.Cells[CURVE_COLLECTION_VISIBLE].Value = true;
        //    }
        //}

        #endregion

        #region EpoxyPatternControl's event handlers.

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void epoxyPatternControl_Load(object sender, EventArgs e)
        {
            RefreshDataGridVeiws();
        }

        /// <summary>
        /// ScaleChange
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_ScaleChanged(object sender, ScaleChangedEventArgs eventArgs)
        {
            this.LbScale.Text = $"{eventArgs.NewScaleValue * 100}%";
        }

        /// <summary>
        /// DieSize改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_DieSizeChanged(object sender, DieSizeChangedEventArgs eventArgs)
        {
            var epoxyPatternControl = (EpoxyPatternControl)sender;

            string strText = string.Format(TextResource.FORMAT_DIE_SIZE, eventArgs.NewSize.Width, eventArgs.NewSize.Height);
            this.LbDieSizeValue.Text = strText;
        }

        /// <summary>
        /// ActionEnum Change
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_ActionChanged(object sender, ActionChangedEventArgs eventArgs)
        {
            if (eventArgs.NewActionEnum != ActionEnum.None)
            {
                this.LbAction.ForeColor = Color.Red;
            }
            else
            {
                this.LbAction.ForeColor = Color.Black;
            }

            this.LbAction.Text = GetActionReadableName(eventArgs.NewActionEnum);

            switch (eventArgs.OldActionEnum)
            {
            case ActionEnum.PaintCurve:
            case ActionEnum.AppendPoint:
            case ActionEnum.DeletePoint:
            case ActionEnum.InsertPoint:
            case ActionEnum.MovePoint:
            case ActionEnum.TranslateCurve:
            case ActionEnum.ScaleCurve:
            case ActionEnum.RotateCurve:
                if (this.EpoxyPatternControl.CurveCount > 0)
                {
                    EnableCurveActions();
                }
                break;

            default:
                break;
            }

            if (eventArgs.NewActionEnum == ActionEnum.None)
            {
                this.BtSelectObject.Enabled = false;
            }
            else
            {
                this.BtSelectObject.Enabled = true;

                foreach (SimpleButton button in this.PnlCurveActions.Controls)
                {
                    if ((ActionEnum)button.Tag == eventArgs.NewActionEnum)
                    {
                        button.Enabled = false;
                        break;
                    }
                }
            }

            switch (eventArgs.NewActionEnum)
            {
            case ActionEnum.PaintCurve:

            case ActionEnum.AppendPoint:
            case ActionEnum.InsertPoint:
            case ActionEnum.MovePoint:
                this.ChkXiFu.Visible = true;
                this.ChkXiFu.Enabled = true;
                this.EpoxyPatternControl.SetAttachable(this.ChkXiFu.Checked);
                break;

            default:
                this.ChkXiFu.Visible = false;
                break;
            }
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void epoxyPatternControl_MouseEnter(object sender, EventArgs e)
        {
            Control control = sender as Control;
            if (control != null)
            {
                control.Focus();
            }
        }

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_EndpointAppended(object sender, EndpointAppendedEventArgs eventArgs)
        {
            Curve curve = this.EpoxyPatternControl.GetCurrentCurve();
            Debug.Assert(object.ReferenceEquals(curve, eventArgs.Curve));

            if (curve.SegmentCount > 0)
            {
                int iNewSegmentInitialEndpointIndex = eventArgs.EndpointIndex - 1;
                Debug.Assert(iNewSegmentInitialEndpointIndex >= 0);

                // EndpointSetting endpointInfo = curve.GetEndpointInfo(iNewSegmentInitialEndpointIndex);

                EndpointSetting endpointInfo = new EndpointSetting(10, 10, 100, 0, 0, false, 0, 0);
                    var row = CreateDataGridViewRowOfSegmentCollection(iNewSegmentInitialEndpointIndex, endpointInfo);

                this.dataGridViewCurveSegments.Rows.Add(row);
            }
        }

        /// <summary>
        /// 插入点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_EndpointInserted(object sender, EndpointInsertedEventArgs eventArgs)
        {
            RefreshDataGridViewCurveSegments();
        }

        /// <summary>
        /// 删除点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_PointErased(object sender, EndpointDeletedEventArgs eventArgs)
        {
            this.RefreshDataGridVeiws();

            Curve curve = this.EpoxyPatternControl.GetCurrentCurve();
            if (curve.EndpointCount == 0)
            {
                this.BtDeletePoint.PerformClick();
                EpoxyPatternControl.RemoveAllCurves();
            }
        }

        /// <summary>
        /// 点改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_EndpointChanged(object sender, EndpointChangedEventArgs eventArgs)
        {
            EpoxyPatternControl epoxyPatternControl = (EpoxyPatternControl)sender;

            Debug.Assert(ReferenceEquals(eventArgs.Curve, epoxyPatternControl.GetCurrentCurve()));
            
            int iInitialEndpointIndex = Convert.ToInt32(this.GvSegmentEndpoints.GetRowCellValue(0, this.GvSegmentEndpoints.Columns[1])) - 1;

            int rowIndex = -1;
            if (iInitialEndpointIndex == eventArgs.EndpointIndex)
            {
                rowIndex = 0;
            }
            else if (iInitialEndpointIndex + 1 == eventArgs.EndpointIndex)
            {
                rowIndex = 1;
            }

            if (rowIndex != -1)
            {
                this.GvSegmentEndpoints.SetRowCellValue(rowIndex, this.GvSegmentEndpoints.Columns[2], eventArgs.EndpointLogicalPoint.X);
                this.GvSegmentEndpoints.SetRowCellValue(rowIndex, this.GvSegmentEndpoints.Columns[3], eventArgs.EndpointLogicalPoint.Y);
            }
        }

        /// <summary>
        /// 鼠标点改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_MousePointChanged(object sender, MousePointChangedEventArgs eventArgs)
        {
            this.LbCoordinates.Text = eventArgs.MouseLogicalPoint.ToString();
        }

        /// <summary>
        /// 添加线条
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_CurveAdded(object sender, CurveAddedEventArgs eventArgs)
        {
            var epoxyPatternControl = (EpoxyPatternControl)sender;

            Debug.Assert(ReferenceEquals(eventArgs.Curve, epoxyPatternControl.GetCurrentCurve()));
            Debug.Assert(eventArgs.CurveIndex == epoxyPatternControl.CurrentCurveIndex);

            // Append new record into DataGridViewCurveCollection.
            var row = CreateDataGridViewRowOfCurveCollection(eventArgs.CurveIndex);
            this.dataGridViewCurveCollection.Rows.Add(row);

            row.Selected = true;

            // Clear DataGridViewSegmentCollection.

            this.dataGridViewCurveSegments.Rows.Clear();

            // Enable the curve collection buttons if possible.
            if (epoxyPatternControl.CurveCount == 1)
            {
                EnableCurveCollectionButtons();
            }
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="iCurveIndex">线段索引</param>
        private DataGridViewRow CreateDataGridViewRowOfCurveCollection(int iCurveIndex)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(this.dataGridViewCurveCollection);

            //row.Cells[CURVE_COLLECTION_INDEX].Value = $"{iCurveIndex + 1}";
            //row.Cells[CURVE_COLLECTION_VISIBLE].Value = true;

            row.SetValues(
                $"{iCurveIndex + 1}",   // index
                true);                  // visible

            return row;
        }

        /// <summary>
        /// 当前线段索引改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_CurrentCurveIndexChanged(object sender, CurrentCurveIndexChangedEventArgs eventArgs)
        {
            RefreshDataGridViewCurveSegments();
        }

        /// <summary>
        /// 线段删除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void epoxyPatternControl_CurveDeleted(object sender, CurveDeletedEventArgs eventArgs)
        {
            //DeleteDataGridViewRowOfCurveCollection(iRowIndex: eventArgs.CurveIndex);

            RefreshDataGridViewCurveCollection();
            RefreshDataGridViewCurveSegments();
            RefreshDataGridViewEndpointCollection();

            if (EpoxyPatternControl.CurveCount == 0)
            {
                DisableCurveActions();
                this.BtPaintCurve.Enabled = true;

                DisableCurveCollectionButtons();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        //private void DeleteDataGridViewRowOfCurveCollection(int iRowIndex)
        //{
        //    var rowCollection = this.dataGridViewCurveCollection.Rows;
        //    rowCollection.RemoveAt(iRowIndex);

        //    for (int i = iRowIndex; i < rowCollection.Count; i++)
        //    {
        //        rowCollection[i].Cells[CURVE_COLLECTION_INDEX].Value = i - 1;
        //    }

        //    int iCurveIndex = 0;
        //    foreach (DataGridViewRow row in this.dataGridViewCurveCollection.Rows)
        //    {
        //        if (iCurveIndex >= iRowIndex)
        //        {
        //            row.Cells[CURVE_COLLECTION_INDEX].Value = iCurveIndex;
        //        }

        //        iCurveIndex++;
        //    }
        //}

        /// <summary>
        /// 禁用按钮
        /// </summary>
        private void DisableCurveActions()
        {
            foreach (Control control in this.PnlCurveActions.Controls)
            {
                control.Enabled = false;
            }
        }

        /// <summary>
        /// 启用按钮
        /// </summary>
        private void EnableCurveActions()
        {
            foreach (Control control in this.PnlCurveActions.Controls)
            {
                control.Enabled = true;
            }
        }

        #endregion

        #region Event handlers of the curve collection DataGridView object.

        /// <summary>
        /// CellContentClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void dataGridViewCurveCollection_CellContentClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.RowIndex < 0)
                return;

            if (eventArgs.ColumnIndex == CURVE_COLLECTION_DELETE)
            {
                int iCurveIndex = eventArgs.RowIndex;
                if (iCurveIndex == this.EpoxyPatternControl.CurrentCurveIndex)
                {
                    this.EpoxyPatternControl.RemoveCurve(iCurveIndex);
                    ExecuteAction(ActionEnum.None);
                }
                else
                {
                    this.EpoxyPatternControl.RemoveCurve(iCurveIndex);
                }
            }
        }

        /// <summary>
        /// Value Changed
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void dataGridViewCurveCollection_CellValueChanged(object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.RowIndex < 0)
                return;

            DataGridView dataGridView = (DataGridView)sender;

            if (eventArgs.ColumnIndex == CURVE_COLLECTION_VISIBLE)
            {
                int iCurveIndex = eventArgs.RowIndex;

                if (iCurveIndex != this.EpoxyPatternControl.CurrentCurveIndex)
                {
                    // "visible" checkbox's value is changed.
                    DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dataGridView[CURVE_COLLECTION_VISIBLE, eventArgs.RowIndex];
                    bool bChecked = (bool)checkBoxCell.Value;
                    if (bChecked)
                    {
                        Console.WriteLine($"Show curve({iCurveIndex})");
                        this.EpoxyPatternControl.ShowCurve(iCurveIndex);
                    }
                    else
                    {
                        Console.WriteLine($"Hide curve({iCurveIndex})");
                        this.EpoxyPatternControl.HideCurve(iCurveIndex);
                    }
                }
            }
        }

        /// <summary>
        /// CellClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void dataGridViewCurveCollection_CellClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.RowIndex < 0)
                return;

            DataGridView dataGridView = (DataGridView)sender;

            if (eventArgs.ColumnIndex == CURVE_COLLECTION_VISIBLE)
            {
                //
                // "visible" checkbox is clicked.
                //
                DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dataGridView[eventArgs.ColumnIndex, eventArgs.RowIndex];
                bool bChecked = (bool)checkBoxCell.Value;
                checkBoxCell.Value = !bChecked;
            }
        }

        /// <summary>
        /// SelectionChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void dataGridViewCurveCollection_SelectionChanged(object sender, EventArgs eventArgs)
        {
            if (this.dataGridViewCurveCollection.SelectedRows.Count <= 0)
                return;

            var selectedRow = this.dataGridViewCurveCollection.SelectedRows[0];

            Console.WriteLine($"SetCurrentCurveIndex({selectedRow.Index})");
            if (this.EpoxyPatternControl.SetCurrentCurveIndex(selectedRow.Index))
            {
                ExecuteAction(ActionEnum.None);
            }
        }


        #endregion

        #region The event handlers of the curve segments DataGridView object.
        /// <summary>
        /// CellValueChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="eventArgs">参数封装</param>
        private void dataGridViewCurveSegments_CellValueChanged(object sender, DataGridViewCellEventArgs eventArgs)
        {
            if (eventArgs.RowIndex < 0)
                return;

            var dataGridView = (DataGridView)sender;
            DataGridViewRow row = dataGridView.Rows[eventArgs.RowIndex];

            int iSegmentIndex = eventArgs.RowIndex;
            Curve curve = this.EpoxyPatternControl.GetCurrentCurve();

            EndpointSetting endpointInfo = curve.GetEndpointInfo(iSegmentIndex);
            endpointInfo.MaxSpeed = int.Parse((string)row.Cells[SEGMENT_MAX_SPEED].FormattedValue);
            endpointInfo.EndSpeed = int.Parse((string)row.Cells[SEGMENT_END_SPEED].FormattedValue);
            endpointInfo.Acc = double.Parse((string)row.Cells[SEGMENT_ACC].FormattedValue);
            endpointInfo.Time = int.Parse((string)row.Cells[SEGMENT_TIME].FormattedValue);
            endpointInfo.Dummied = (bool)row.Cells[SEGMENT_DUMMIED].Value;
            endpointInfo.PreDelay = int.Parse((string)row.Cells[SEGMENT_PREDELAY].FormattedValue);
            endpointInfo.PostDelay = int.Parse((string)row.Cells[SEGMENT_POSTDELAY].FormattedValue);
            
            curve.ModifySegmentInfo(iSegmentIndex, endpointInfo);
            this.EpoxyPatternControl.Invalidate();
        }

        /// <summary>
        /// SelectionChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void dataGridViewSegmentCollection_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            if (dataGridView.SelectedRows.Count > 0)
            {
                Debug.Assert(dataGridView.SelectedRows.Count == 1);

                DataGridViewRow selectedRow = dataGridView.SelectedRows[0];

                this.EpoxyPatternControl.HighlightedSegmentIndex = selectedRow.Index;

                UpdateDataGridViewEndpointCollection(iSegmentIndex: selectedRow.Index);
            }
            else
            {
                Debug.Assert(dataGridView.SelectedRows.Count == 0);
            }
        }

      /// <summary>
      /// 更新
      /// </summary>
      /// <param name="iSegmentIndex">线段索引</param>
        private void UpdateDataGridViewEndpointCollection(int iSegmentIndex)
        {
            if (this.GvSegmentEndpoints.RowCount == 0)
            {
                this.InitGvSegmentEndpoints();
            }

            Debug.Assert(this.GvSegmentEndpoints.RowCount == 2);

            Curve curve = this.EpoxyPatternControl.GetCurrentCurve();

            UpdateDataGridViewRowOfEndpointCollection(0, curve, iSegmentIndex);
            UpdateDataGridViewRowOfEndpointCollection(1, curve, iSegmentIndex + 1);
        }

        /// <summary>
        /// SelectionChanged
        /// </summary>
        /// <param name="iDataGridViewRowIndex">索引</param>
        /// <param name="curve">曲线</param>
        /// <param name="iEndpointIndex">端点索引</param>
        private void UpdateDataGridViewRowOfEndpointCollection(int iDataGridViewRowIndex, Curve curve, int iEndpointIndex)
        {
            Debug.Assert(iDataGridViewRowIndex == 0 || iDataGridViewRowIndex == 1);

            Endpoint endpoint = curve.GetEndpoint(iEndpointIndex);
            EndpointSetting endpointInfo = curve.GetEndpointInfo(iEndpointIndex);
            
            this.GvSegmentEndpoints.SetRowCellValue(iDataGridViewRowIndex, this.GvSegmentEndpoints.Columns[1], $"{iEndpointIndex + 1}");
            this.GvSegmentEndpoints.SetRowCellValue(iDataGridViewRowIndex, this.GvSegmentEndpoints.Columns[2], endpoint.LogicalPoint.X);
            this.GvSegmentEndpoints.SetRowCellValue(iDataGridViewRowIndex, this.GvSegmentEndpoints.Columns[3], endpoint.LogicalPoint.Y);
            this.GvSegmentEndpoints.SetRowCellValue(iDataGridViewRowIndex, this.GvSegmentEndpoints.Columns[4], $"{endpointInfo.AltitudeCompensation}");
            this.GvSegmentEndpoints.SetRowCellValue(iDataGridViewRowIndex, this.GvSegmentEndpoints.Columns[5], endpointInfo.HeightMeasurement);
        }

        #endregion

        #region The event handlers of the segment's endpoint collection DataGridView object.
        /// <summary>
        /// SelectionChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void dataGridViewEndointCollection_CellValueChanged(object sender, DataGridViewCellEventArgs eventArgs)
        {
            DataGridView dataGridView = (DataGridView)sender;

            Curve curve = this.EpoxyPatternControl.GetCurrentCurve();

            Debug.Assert(dataGridView.SelectedRows.Count == 1);
            DataGridViewRow endpointCollectionSelectedRow = dataGridView.SelectedRows[0];
            DataGridViewCell endpointCollectionCell = endpointCollectionSelectedRow.Cells[ENDPOINT_INDEX];
            int iEndpointIndex = int.Parse((string)endpointCollectionCell.FormattedValue) - 1;

            EndpointSetting endpointInfo = curve.GetEndpointInfo(iEndpointIndex);
            {
                var cell = endpointCollectionSelectedRow.Cells[ENDPOINT_Z];
                endpointInfo.AltitudeCompensation = int.Parse((string)cell.FormattedValue);
            }

            {
                var cell = endpointCollectionSelectedRow.Cells[HEIGHT_MEASUREMENT];
                endpointInfo.HeightMeasurement = bool.Parse((string)cell.FormattedValue);
            }

            curve.ModifyEndpointInfo(iEndpointIndex, endpointInfo);
        }


        #endregion

        #region Pattern repository buttons.
        /// <summary>
        /// SelectionChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonManageRepository_Click(object sender, EventArgs e)
        {
            string patternRepositoryPath = GetPatternRepositoryPath();

            using (var dialogBox = new ManageRepositoryExDialogBox())
            {
                dialogBox.SetPatternRepository(patternRepositoryPath);
                dialogBox.ShowDialog(this);

                if (!string.IsNullOrEmpty(this.currentPatternName))
                {
                    string strFileName = PatternFile.ConstructFileNameByPatternName(this.currentPatternName);
                    string strFilePath = Path.Combine(patternRepositoryPath, strFileName);
                    if (!File.Exists(strFilePath))
                        this.currentPatternName = null;
                }
            }
        }

        /// <summary>
        /// SelectionChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonRetrievePatternFromRepository_Click(object sender, EventArgs e)
        {
            string patternRepositoryPath = GetPatternRepositoryPath();
            using (var dialogBox = new RetrieveEpoxyPatternExDialogBox())
            {
                dialogBox.SetRepositoryPath(patternRepositoryPath);

                if (dialogBox.ShowDialog(this) == DialogResult.OK)
                {
                    EPEData epeData = PatternFile.ReadFile(dialogBox.SelectedPatternFilePath);
                    if (epeData != null)
                    {
                        SetContent(epeData);

                        this.EpoxyPatternControl.Invalidate();
                        RefreshDataGridVeiws();
                        RefreshDataGridViewCurveCollection();
                        RefreshDataGridViewCurveSegments();

                        this.currentPatternName = Path.GetFileNameWithoutExtension(dialogBox.SelectedPatternFilePath);
                    }
                    else
                    {
                        MessageBox.Show(this, TextResource.MESSAGE_BOX_TEXT_RETRIEVE_PATTERN_FAILED, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// 添加模版
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void buttonAddPatternToRepository_Click(object sender, EventArgs e)
        {
            using (var dialogBox = new SaveEpoxyPatternDialogBox())
            {
                dialogBox.SetDirPath(EpoxyPatternForm.GetPatternRepositoryPath());

                if (dialogBox.ShowDialog(this) == DialogResult.OK)
                {
                    string strPatternFilePath = dialogBox.GetPatternFilePath();
                   
                    EPEData epeData = GetContent();
                    PatternFile.SaveFile(strPatternFilePath, epeData);

                    this.currentPatternName = Path.GetFileNameWithoutExtension(strPatternFilePath);

                    string strDirPath = Path.GetDirectoryName(strPatternFilePath);
                    string strPatternName = Path.GetFileNameWithoutExtension(strPatternFilePath);
                    string strSnapshootFileName = "Snapshoot_" + strPatternName + ".bmp";
                    string strSnapshootFilePath = Path.Combine(strDirPath, strSnapshootFileName);
                    this.EpoxyPatternControl.SaveSnapshoot(strSnapshootFilePath);
                }
            }
        }

        #endregion

        #region external interfaces.

        /// <summary>
        /// 设置Content
        /// </summary>
        /// <param name="epeData">epeData</param>
        public void SetContent(EPEData epeData)
        {
            SetContent(epeData, false);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="epeData">点胶数据</param>
        /// <param name="bHeightMeasurement">测高单选框是否可见</param>
        public void SetContent(EPEData epeData, bool bHeightMeasurement)
        {
            {
                float diePixelWidth = this.dieWidth / this.micronPerPixel;
                float diePixelHeight = this.dieHeight / this.micronPerPixel;
                this.EpoxyPatternControl.SetDieSize(diePixelWidth, diePixelHeight);

                this.EpoxyPatternControl.SetDefaultSegmentEndPointInfo(epeData.DefaultInfo);

                this.EpoxyPatternControl.SetFigure(epeData.EpePointList);
                this.EpoxyPatternControl.BackgroundImageInfo = epeData.BackgroundImageInfo;
            }

            {
                this.micronPerPixel = epeData.MicronPerPixel;
                this.LbUnitValue.Text = string.Format(TextResource.FORMAT_MICRON_PER_PIXEL, this.micronPerPixel);

                this.dieWidth = epeData.DieWidth;
                this.dieHeight = epeData.DieHeight;

                this.dataGridViewCurveSegments.ClearSelection();

                if (epeData.EpePointList.Count > 0)
                {
                    EnableCurveActions();
                    this.BtSelectObject.Enabled = false;

                    EnableCurveCollectionButtons();
                }
                else
                {
                    DisableCurveActions();
                    this.BtPaintCurve.Enabled = true;

                    DisableCurveCollectionButtons();
                }
            }

            this.GvSegmentEndpoints.Columns[5].Visible = bHeightMeasurement;
        }

        /// <summary>
        /// 获取此刻的图形内容。
        /// </summary>
        /// <returns>点胶数据</returns>
        public EPEData GetContent()
        {
            List<EPEPointData[]> list = this.EpoxyPatternControl.GetFigure();

            BackgroundImageInfo imageInfo = this.EpoxyPatternControl.BackgroundImageInfo;
            return new EPEData(this.micronPerPixel, this.dieWidth, this.dieHeight, list, imageInfo.Image, imageInfo.MatrixElements);
        }

        #endregion

        /// <summary>
        /// 统一参数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAutoParam_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewCurveSegments.CurrentCell == null)
            {
                MessageBox.Show("请先选中标准参数值");
                return;
            }

            int columnIndex = this.dataGridViewCurveSegments.CurrentCell.ColumnIndex;

            if (columnIndex == 0)
            {
                MessageBox.Show("请先选中标准参数值");
                return;
            }

            foreach (DataGridViewRow row in this.dataGridViewCurveSegments.Rows)
            {
                row.Cells[columnIndex].Value = this.dataGridViewCurveSegments.CurrentCell.Value;
            }

            this.RefreshDataGridViewCurveSegments();
        }

        /// <summary>
        /// 统一所有曲线参数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAutoAllParam_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewCurveSegments.CurrentCell == null)
            {
                MessageBox.Show("请先选中标准参数值");
                return;
            }

            int columnIndex = this.dataGridViewCurveSegments.CurrentCell.ColumnIndex;

            if (columnIndex == 0)
            {
                MessageBox.Show("请先选中标准参数值");
                return;
            }

            foreach (Curve curve in this.EpoxyPatternControl.CurveList)
            {
                foreach (EndpointUnit endpointUnit in curve.EndpointUnitList)
                {
                    int index = columnIndex;
                    string value = this.dataGridViewCurveSegments.CurrentCell.Value.ToString();

                    int maxSpeed = endpointUnit.EndpointInfo.MaxSpeed;
                    int endSpeed = endpointUnit.EndpointInfo.EndSpeed;
                    double acc = endpointUnit.EndpointInfo.Acc;
                    int time = endpointUnit.EndpointInfo.Time;
                    bool dummied = endpointUnit.EndpointInfo.Dummied;
                    int preDelay = endpointUnit.EndpointInfo.PreDelay;
                    int postDelay = endpointUnit.EndpointInfo.PostDelay;

                    switch (index)
                    {
                        case 1:
                            maxSpeed = int.Parse(value);
                            break;
                        case 2:
                            endSpeed = int.Parse(value);
                            break;
                        case 3:
                            acc = double.Parse(value);
                            break;
                        case 4:
                            time = int.Parse(value);
                            break;
                        case 5:
                            dummied = bool.Parse(value);
                            break;
                        case 6:
                            preDelay = int.Parse(value);
                            break;
                        case 7:
                            postDelay = int.Parse(value);
                            break;
                    }
                    
                    endpointUnit.EndpointInfo = new EndpointSetting(
                        maxSpeed,
                        endSpeed,
                        acc,
                        0,
                        time,
                        dummied,
                        preDelay,
                        postDelay);
                }
            }
            
            this.RefreshDataGridViewCurveSegments();
        }
    }
}