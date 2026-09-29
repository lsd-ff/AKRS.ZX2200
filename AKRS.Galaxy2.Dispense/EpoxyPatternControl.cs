using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 编辑画胶样式的控件。
    /// </summary>
    public partial class EpoxyPatternControl : XtraUserControl
    {
        #region Static members.

        public const int INVALID_CURVE_INDEX = -1;
        public const int INVALID_SEGMENT_INDEX = -1;
        public const int INVALID_ENDPOINT_INDEX = -1;
        const char CHAR_ESC = (char)0x001B;

        /*
         * CMI is short for CONTEXT_MENU_ITEM
         */
        const string CMI_BASIC_ACTION_ROTATE_BACKGROUND_IMAGE = "basic_action_rotate_background_image";
        const string CMI_BASIC_ACTION_TRANSLATE_BACKGROUND_IMAGE = "basic_action_translate_background_image";
        const string CMI_BASIC_ACTION_RESET_BACKGROUND_IMAGE = "basic_action_reset_background_image";

        const string CMI_SEPARATOR_BETWEEN_BASIC_AND_CURVE = "separator_between_basic_and_curve";

        const string CMI_CURVE_ACTION_ADD_CURVE = "action_add_curve";

        const string CMI_CURVE_ACTION_APPEND_POINT = "action_append_point";
        const string CMI_CURVE_ACTION_INSERT_POINT = "action_insert_point";
        const string CMI_CURVE_ACTION_MOVE_POINT = "action_move_point";
        const string CMI_CURVE_ACTION_ERASE_POINT = "action_erase_point";

        const string CMI_SEPARATOR_EMBEDDED_IN_CURVE_ACTIONS = "separator_embedded_in_curve_actions";

        const string CMI_CURVE_ACTION_TRANSLATE_CURVE = "translate_curve";
        const string CMI_CURVE_ACTION_SCALE_CURVE = "scale_curve";
        const string CMI_CURVE_ACTION_ROTATE_CURVE = "rotate_curve";

        const string CMI_SEPARATOR_BEFORE_EXIT_AND_CANCEL = "separator_before_exit_and_cancel";
        const string CMI_EXIT_ACTION = "exit_action";
        const string CMI_CANCEL = "cancel";

        static readonly string[] s_basicActionNames;
        static readonly string[] s_curveActionNames;

        static readonly Dictionary<ActionEnum, string> s_actionNameDict;

        static EpoxyPatternControl()
        {
            Type type = typeof(EpoxyPatternControl);

            BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;
            FieldInfo[] fieldInfoArray = type.GetFields(bindingFlags);

            List<string> curveActionNameList = new List<string>(fieldInfoArray.Length);
            List<string> basicActionNameList = new List<string>(fieldInfoArray.Length);

            foreach (FieldInfo fieldInfo in fieldInfoArray)
            {
                if (fieldInfo.FieldType == typeof(string))
                {
                    if (fieldInfo.Name.StartsWith("CMI_CURVE_ACTION"))
                    {
                        var fieldVal = fieldInfo.GetValue(null);
                        curveActionNameList.Add((string)fieldVal);
                    }
                    else if (fieldInfo.Name.StartsWith("CMI_BASIC_ACTION"))
                    {
                        var fieldVal = fieldInfo.GetValue(null);
                        basicActionNameList.Add((string)fieldVal);
                    }
                }
            }

            s_curveActionNames = curveActionNameList.ToArray();
            s_basicActionNames = basicActionNameList.ToArray();

            s_actionNameDict = new Dictionary<ActionEnum, string>();

            s_actionNameDict.Add(ActionEnum.RotateBackgroundImage, CMI_BASIC_ACTION_ROTATE_BACKGROUND_IMAGE);
            s_actionNameDict.Add(ActionEnum.TranslateBackgroundImage, CMI_BASIC_ACTION_TRANSLATE_BACKGROUND_IMAGE);

            s_actionNameDict.Add(ActionEnum.PaintCurve, CMI_CURVE_ACTION_ADD_CURVE);
            s_actionNameDict.Add(ActionEnum.AppendPoint, CMI_CURVE_ACTION_APPEND_POINT);
            s_actionNameDict.Add(ActionEnum.InsertPoint, CMI_CURVE_ACTION_INSERT_POINT);
            s_actionNameDict.Add(ActionEnum.MovePoint, CMI_CURVE_ACTION_MOVE_POINT);
            s_actionNameDict.Add(ActionEnum.DeletePoint, CMI_CURVE_ACTION_ERASE_POINT);

            s_actionNameDict.Add(ActionEnum.TranslateCurve, CMI_CURVE_ACTION_TRANSLATE_CURVE);
            s_actionNameDict.Add(ActionEnum.RotateCurve, CMI_CURVE_ACTION_ROTATE_CURVE);
            s_actionNameDict.Add(ActionEnum.ScaleCurve, CMI_CURVE_ACTION_SCALE_CURVE);
        }

        #endregion

        #region Events.

        /// <summary>
        /// 晶片尺寸改变了。
        /// </summary>
        public event EventHandler<DieSizeChangedEventArgs> DieSizeChanged;

        /// <summary>
        /// 追加了新的端点。
        /// </summary>
        public event EventHandler<EndpointAppendedEventArgs> EndpointAppended;

        /// <summary>
        /// 插入了新的端点。
        /// </summary>
        public event EventHandler<EndpointInsertedEventArgs> EndpointInserted;

        /// <summary>
        /// 端点被删除了。
        /// </summary>
        public event EventHandler<EndpointDeletedEventArgs> EndpointDeleted;

        /// <summary>
        /// 端点的位置改变了。
        /// </summary>
        public event EventHandler<EndpointChangedEventArgs> EndpointChanged;

        /// <summary>
        /// 鼠标位置改变了。
        /// </summary>
        public event EventHandler<MousePointChangedEventArgs> MousePointChanged;

        /// <summary>
        /// 比例发生了改变。
        /// </summary>
        public event EventHandler<ScaleChangedEventArgs> ScaleChanged;

        /// <summary>
        /// 新的曲线被添加了。
        /// </summary>
        public event EventHandler<CurveAddedEventArgs> CurveAdded;

        /// <summary>
        /// 曲线被删除了。
        /// </summary>
        public event EventHandler<CurveDeletedEventArgs> CurveDeleted;

        /// <summary>
        /// 当前曲线改变了，变成了其他曲线或者不再有当前曲线。
        /// </summary>
        public event EventHandler<CurrentCurveIndexChangedEventArgs> CurrentCurveIndexChanged;

        /// <summary>
        /// 动作改变了。
        /// </summary>
        public event EventHandler<ActionChangedEventArgs> ActionChanged;

        #endregion

        #region Fields.

        /// <summary>
        /// 背景图片的信息。
        /// </summary>
        private BackgroundImageInfo backgroundImageInfo;

        /// <summary>
        /// 背景图片的变换矩阵。
        /// </summary>
        private Matrix backgroundImageMatrix;

        /// <summary>
        /// 背景位图，用于长期保存背景图片、坐标轴和网格线。
        /// </summary>
        private Bitmap backgroundBitmap;

        /// <summary>
        /// 晶片的尺寸。
        /// </summary>
        private SizeF dieSize;

        /// <summary>
        /// 端点矩形框的尺寸。
        /// </summary>
        private Size endpointRectSize;

        /// <summary>
        /// 坐标系原点在控件上的位置。
        /// </summary>
        private Point2D coordinateSystemOrigin;

        /// <summary>
        /// 网格线的画笔。
        /// </summary>
        private Pen gridLinePen;

        /// <summary>
        /// 坐标轴的画笔。
        /// </summary>
        private Pen coordinateAxisPen;

        /// <summary>
        /// 坐标延长线的画笔。
        /// </summary>
        private Pen coordinateLinePen;

        /// <summary>
        /// 坐标值的画刷。
        /// </summary>
        private Brush coordinateValueBrush;

        /// <summary>
        /// 当前曲线的画图工具集。
        /// </summary>
        private PaintingToolSet currentCurvePaintingToolSet;

        /// <summary>
        /// 其他曲线的画图工具集。
        /// </summary>
        private PaintingToolSet otherCurvePaintingToolSet;

        /// <summary>
        /// 曲线列表。
        /// </summary>
        public List<Curve> CurveList;

        /// <summary>
        /// 当前曲线。
        /// </summary>
        private int iCurrentCurveIndex;

        /// <summary>
        /// 当前的动作。
        /// </summary>
        private ActionEnum actionEnum;

        /// <summary>
        /// 动作和处理器字典。
        /// </summary>
        private Dictionary<ActionEnum, ActionHandler> handlerDict;

        /// <summary>
        /// 高亮时用的画图工具。
        /// </summary>
        private PaintingToolSet highlightPaintingToolSet;

        /// <summary>
        /// 高亮的端点。
        /// </summary>
        private int iHighlightedEndPointIndex;

        /// <summary>
        /// 高亮的线段。
        /// </summary>
        private int iHighlightedSegmentIndex;

        /// <summary>
        /// 缺省的端点信息。
        /// </summary>
        private EndpointSetting defaultEndPointInfo;

        /// <summary>
        /// 图形内容的缩放比。
        /// </summary>
        private float scale;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public EpoxyPatternControl()
        {
            this.backgroundImageInfo = new BackgroundImageInfo();

            this.scale = 1.0F;

            this.defaultEndPointInfo = new EndpointSetting();

            this.endpointRectSize = new Size(10, 10);

            this.gridLinePen = new Pen(Color.Gray, 0);
            this.gridLinePen.DashStyle = DashStyle.Dash;
            this.gridLinePen.DashPattern = new float[] { 3, 3 };

            this.coordinateAxisPen = new Pen(Color.Black, 0);
            this.coordinateAxisPen.DashStyle = DashStyle.Dot;
            this.coordinateAxisPen.DashPattern = new float[] { 10, 3 };

            this.otherCurvePaintingToolSet = new PaintingToolSet(Color.Blue);
            this.currentCurvePaintingToolSet = new PaintingToolSet(Color.Red);

            this.backgroundBitmap = null;
            //_bmBackground will be initialized in OnSizeChanged() invoked by InitializeComponent().

            this.dieSize = new SizeF(0, 0);
            this.coordinateSystemOrigin = new Point2D();
            // _logicalCoordinateSystemOrigin will be initialized in InitBackgroundBitmap().

            this.actionEnum = ActionEnum.None;

            this.CurveList = new List<Curve>();
            this.iCurrentCurveIndex = INVALID_CURVE_INDEX;

            this.highlightPaintingToolSet = new PaintingToolSet(Color.Red, 3);

            this.coordinateLinePen = new Pen(Color.Black);
            this.coordinateLinePen.DashStyle = DashStyle.Dot;
            this.coordinateValueBrush = new SolidBrush(Color.Red);

            this.backgroundImageInfo = new BackgroundImageInfo();            

            //SetStyle(ControlStyles.Selectable, true);

            this.handlerDict = new Dictionary<ActionEnum, ActionHandler>();
            InitActionHandlerDict();

            this.backgroundImageMatrix = new Matrix();

            InitializeComponent();

            InitializeContextMenuItemNames();
        }

        private void InitActionHandlerDict()
        {
            Debug.Assert(this.handlerDict != null);
            Debug.Assert(this.handlerDict.Count == 0);

            this.handlerDict.Add(ActionEnum.None, new NullHandler(this));

            this.handlerDict.Add(ActionEnum.TranslateBackgroundImage, new TranslateBackgroundImageHandler(this));
            this.handlerDict.Add(ActionEnum.RotateBackgroundImage, new RotateBackgroundImageHandler(this));

            this.handlerDict.Add(ActionEnum.SelectObject, new SelectObjectHandler(this));

            this.handlerDict.Add(ActionEnum.MovePoint, new MovePointHandler(this));
            this.handlerDict.Add(ActionEnum.AppendPoint, new AppendPointHandler(this));

            this.handlerDict.Add(ActionEnum.InsertPoint, new InsertPointHandler(this));
            this.handlerDict.Add(ActionEnum.DeletePoint, new ErasePointHandler(this));
            this.handlerDict.Add(ActionEnum.PaintCurve, new PaintCurveHandler(this));

            this.handlerDict.Add(ActionEnum.ScaleCurve, new ScaleCurveHandler(this));
            this.handlerDict.Add(ActionEnum.RotateCurve, new RotateCurveHandler(this));
            this.handlerDict.Add(ActionEnum.TranslateCurve, new TranslateCurveHandler(this));
        }

        private void InitializeContextMenuItemNames()
        {
            foreach (ToolStripItem item in this.contextMenuStrip1.Items)
            {
                string str = item.Tag as string;
                if (str != null)
                {
                    string strName = str.Trim();
                    item.Name = strName;
                }
            }
        }

        public void SetAttachable(bool bAttachable)
        {
            this.handlerDict[this.actionEnum].Attachable = bAttachable;
        }

        public SizeF DieSize
        {
            get { return this.dieSize; }
        }

        public void SetDieSize(float dieWidth, float dieHeight)
        {
            SizeF newDieSize = new SizeF(dieWidth, dieHeight);
            var eventArgs = new DieSizeChangedEventArgs(newDieSize, this.dieSize);
            this.dieSize = newDieSize;

            RebuildBackgroundBitmap();

            OnDieSizeChanged(eventArgs);
            Invalidate();
        }

        public void SetHighlightingColor(Color color)
        {
            this.highlightPaintingToolSet.SetColor(color);
        }

        public ActionEnum ActionEnum
        {
            get { return this.actionEnum; }
        }

        /// <summary>
        /// 设置动作枚举
        /// </summary>
        /// <param name="newActionEnum">动作枚举</param>
        public void SetAction(ActionEnum newActionEnum)
        {
            if (this.actionEnum != newActionEnum)
            {
                if (this.handlerDict.ContainsKey(newActionEnum))
                {
                    var eventArgs = new ActionChangedEventArgs(this.actionEnum, newActionEnum);
                    OnActionChanging(eventArgs);

                    this.actionEnum = newActionEnum;
                    this.iHighlightedSegmentIndex = INVALID_SEGMENT_INDEX;
                    this.iHighlightedEndPointIndex = INVALID_ENDPOINT_INDEX;
                    Cursor = Cursors.Default;

                    this.handlerDict[this.actionEnum].SetMode();

                    OnActionChanged(eventArgs);
                }
            }
        }

        internal void ResetAction()
        {
            if (this.actionEnum != ActionEnum.None)
            {
                ClearHighlightedEndpoint();
                ClearHighlightedSegment();
                SetAction(ActionEnum.None);
            }
        }

        internal int HighlightedEndpointIndex
        {
            get { return this.iHighlightedEndPointIndex; }
            set { this.iHighlightedEndPointIndex = value; }
        }

        internal void ClearHighlightedEndpoint()
        {
            this.iHighlightedEndPointIndex = INVALID_ENDPOINT_INDEX;
        }

        internal int HighlightedSegmentIndex
        {
            get { return this.iHighlightedSegmentIndex; }
            set
            {
                if (this.iHighlightedSegmentIndex != value)
                {
                    this.iHighlightedSegmentIndex = value;
                    Invalidate();
                }
            }
        }

        internal void ClearHighlightedSegment()
        {
            this.iHighlightedSegmentIndex = INVALID_SEGMENT_INDEX;
        }

        /// <summary>
        /// 获取或设置当前曲线。
        /// </summary>
        internal int CurrentCurveIndex
        {
            get { return this.iCurrentCurveIndex; }
            set
            {
                if (value >= 0 && value < this.CurveList.Count)
                    SetCurrentCurveIndex(value);
            }
        }

        internal bool SetCurrentCurveIndex(int iCurrentCurveIndex)
        {
            if (iCurrentCurveIndex < 0 || this.CurveList.Count <= iCurrentCurveIndex)
                throw new ArgumentOutOfRangeException(nameof(iCurrentCurveIndex));

            //if (actionEnum == ActionEnum.None)
            {
                if (this.iCurrentCurveIndex != iCurrentCurveIndex)
                {
                    var eventArgs = new CurrentCurveIndexChangedEventArgs(iOldIndex: this.iCurrentCurveIndex, iNewIndex: iCurrentCurveIndex);

                    this.iCurrentCurveIndex = iCurrentCurveIndex;
                    OnCurrentCurveIndexChanged(eventArgs);

                    Invalidate();

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取当前曲线的引用。
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">没有当前曲线。</exception>
        internal Curve GetCurrentCurve()
        {
            if (this.iCurrentCurveIndex == INVALID_CURVE_INDEX)
                throw new NoCurrentCurveException();

            return GetCurve(this.iCurrentCurveIndex);
        }

        /// <summary>
        /// 获取指定的曲线的引用。
        /// </summary>
        /// <param name="iCurveIndex"></param>
        /// <returns></returns>
        internal Curve GetCurve(int iCurveIndex)
        {
            if (iCurveIndex < 0 || this.CurveList.Count <= iCurveIndex)
                throw new ArgumentOutOfRangeException(nameof(iCurveIndex));

            return this.CurveList[iCurveIndex];
        }

        /// <summary>
        /// 获取曲线的数量。
        /// </summary>
        internal int CurveCount => this.CurveList.Count;


        /// <summary>
        /// 移除当前曲线。
        /// </summary>
        internal void RemoveCurrentCurve()
        {
            if (this.iCurrentCurveIndex == INVALID_CURVE_INDEX)
            {
                throw new NoCurrentCurveException();
            }

            RemoveCurve(this.iCurrentCurveIndex);
        }

        /// <summary>
        /// 移除指定的曲线。
        /// </summary>
        /// <param name="iCurveIndex"></param>
        internal void RemoveCurve(int iCurveIndex)
        {
            if (iCurveIndex < 0 || iCurveIndex >= this.CurveList.Count)
                throw new IndexOutOfRangeException(nameof(iCurveIndex));

            var eventArgs = new CurveDeletedEventArgs(iCurveIndex, this.CurveList[iCurveIndex]);

            this.CurveList.RemoveAt(iCurveIndex);
            OnCurveDeleted(eventArgs);

            if (iCurveIndex == this.iCurrentCurveIndex)
            {
                int iOldCurrentIndex = this.iCurrentCurveIndex;
                if (this.CurveList.Count == 0)
                {
                    this.iCurrentCurveIndex = INVALID_CURVE_INDEX;
                }
                else
                {
                    int iMaxIndex = this.CurveList.Count - 1;
                    this.iCurrentCurveIndex = Math.Max(0, Math.Min(this.iCurrentCurveIndex - 1, iMaxIndex));
                }

                OnCurrentCurveIndexChanged(new CurrentCurveIndexChangedEventArgs(iOldCurrentIndex, this.iCurrentCurveIndex));
                Invalidate();
            }
        }

        /// <summary>
        /// 移除所有曲线。
        /// </summary>
        internal void RemoveAllCurves()
        {
            if (this.CurveList.Count > 0)
            {
                var eventArgs = new CurrentCurveIndexChangedEventArgs(this.iCurrentCurveIndex, INVALID_CURVE_INDEX);

                this.iCurrentCurveIndex = INVALID_CURVE_INDEX;

                OnCurrentCurveIndexChanged(eventArgs);

                Curve[] curveArray = this.CurveList.ToArray();
                this.CurveList.Clear();

                int iCurveIndex = 0;
                foreach (Curve curve in curveArray)
                {
                    OnCurveDeleted(new CurveDeletedEventArgs(iCurveIndex, curve));
                    iCurveIndex++;
                }

                Invalidate();
            }
        }

        /// <summary>
        /// 隐藏指定的曲线。
        /// </summary>
        /// <param name="iCurveIndex"></param>
        internal void HideCurve(int iCurveIndex)
        {
            if (iCurveIndex < 0 || iCurveIndex >= this.CurveList.Count)
                throw new IndexOutOfRangeException(nameof(iCurveIndex));

            Curve curve = this.CurveList[iCurveIndex];
            if (curve.Visible)
            {
                curve.Visible = false;

                Invalidate();
            }
        }

        /// <summary>
        /// 隐藏指定的曲线。
        /// </summary>
        /// <param name="curve"></param>
        internal void HideCurve(Curve curve)
        {
            if (curve == null)
                throw new ArgumentNullException(nameof(Curve));

            if (curve.Visible)
            {
                curve.Visible = false;

                Invalidate();
            }
        }

        /// <summary>
        /// 隐藏所有曲线。
        /// </summary>
        /// <returns>被隐藏的曲线的数量，原来就隐藏的曲线不计数，这个值在0~CurveCount之间。</returns>
        internal int HideAllCurves()
        {
            bool[] curveVisibilityArray;
            return HideAllCurves(out curveVisibilityArray);
        }

        /// <summary>
        /// 隐藏所有曲线。
        /// </summary>
        /// <param name="curveVisibilityArray">反映每条曲线原来的情况，true表示该曲线原来是显示的，false表示曲线原来就是隐藏的。</param>
        /// <returns>被隐藏的曲线的数量，原来就隐藏的曲线不计数，这个值在0~CurveCount之间。</returns>
        internal int HideAllCurves(out bool[] curveVisibilityArray)
        {
            curveVisibilityArray = null;
            
            int iCount = 0;
            if (this.CurveList.Count > 0)
            {
                List<bool> list = new List<bool>(this.CurveList.Count);

                foreach (Curve curve in this.CurveList)
                {
                    if (curve.Visible)
                    {
                        curve.Visible = false;
                        iCount++;
                        list.Add(true);
                    }
                    else
                    {
                        list.Add(false);
                    }
                }

                curveVisibilityArray = list.ToArray();

                if (iCount > 0)
                    Invalidate();
            }
            
            return iCount;
        }

        /// <summary>
        /// 显示所有的曲线。
        /// </summary>
        internal void ShowAllCurves()
        {
            if (this.CurveList.Count > 0)
            {
                int iCount = 0;
                foreach (Curve curve in this.CurveList)
                {
                    if (!curve.Visible)
                    {
                        curve.Visible = true;
                        iCount++;
                    }
                }

                if (iCount > 0)
                {
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 显示指定的曲线。
        /// </summary>
        /// <param name="iCurveIndex"></param>
        internal void ShowCurve(int iCurveIndex)
        {
            if (iCurveIndex < 0 || this.CurveList.Count <= iCurveIndex)
                throw new ArgumentOutOfRangeException(nameof(iCurveIndex));

            Curve curve = this.CurveList[iCurveIndex];
            if (!curve.Visible)
            {
                curve.Visible = true;

                Invalidate();
            }
        }

        /// <summary>
        /// 按照参数的意思选择性地显示和隐藏曲线。
        /// </summary>
        /// <param name="curveVisibilityArray"></param>
        internal void ShowCurves(bool[] curveVisibilityArray)
        {
            if (curveVisibilityArray == null)
                throw new ArgumentNullException(nameof(curveVisibilityArray));

            if (curveVisibilityArray.Length != this.CurveList.Count)
                throw new ArgumentException("curve count conflict");

            if (this.CurveList.Count > 0)
            {
                int iCount = 0;
                int i = 0;
                foreach (Curve curve in this.CurveList)
                {
                    bool bVisible = curveVisibilityArray[i];
                    if (curve.Visible != bVisible)
                    {
                        curve.Visible = bVisible;
                        iCount++;
                    }

                    i++;
                }

                if (iCount > 0)
                {
                    Invalidate();
                }
            }
        }

        #region Transformation.

        /// <summary>
        /// 平移当前曲线。
        /// </summary>
        /// <param name="xIncrement"></param>
        /// <param name="yIncrement"></param>
        public void TranslateCurrentCurve(float xIncrement, float yIncrement)
        {
            if (this.iCurrentCurveIndex == INVALID_CURVE_INDEX)
                throw new NoCurrentCurveException();

            Curve curve = this.CurveList[this.iCurrentCurveIndex];
            curve.Translate(xIncrement, yIncrement);

            UpdateAllCurves();
            Invalidate();
        }

        /// <summary>
        /// 旋转当前曲线。
        /// </summary>
        /// <param name="rotationCenter"></param>
        /// <param name="degree"></param>
        /// <param name="additionalPoints"></param>
        public void RotateCurrentCurve(Point2D rotationCenter, float degree, ref Point2D[] additionalPoints)
        {
            if (this.iCurrentCurveIndex == INVALID_CURVE_INDEX)
                throw new NoCurrentCurveException();

            Curve curve = this.CurveList[this.iCurrentCurveIndex];
            curve.Rotate(rotationCenter, degree, ref additionalPoints);

            UpdateAllCurves();
            Invalidate();
        }

        /// <summary>
        /// 缩放当前曲线。
        /// </summary>
        /// <param name="fixedPoint"></param>
        /// <param name="xScale"></param>
        /// <param name="yScale"></param>
        /// <param name="additionalPoints"></param>
        /// <exception cref="InvalidOperationException">缩放失败。</exception>
        public void ScaleCurrentCurve(Point2D fixedPoint, float xScale, float yScale, ref Point2D[] additionalPoints)
        {
            if (this.iCurrentCurveIndex == INVALID_CURVE_INDEX)
                throw new NoCurrentCurveException();

            Curve curve = this.CurveList[this.iCurrentCurveIndex];

            curve.Scale(fixedPoint, xScale, yScale, ref additionalPoints);  // throws InvalidOperationException.

            UpdateAllCurves();
            Invalidate();
        }

        #endregion

        /// <summary>
        /// 获取 端点矩形框的宽度（单位是像素）。
        /// </summary>
        internal float EndpointRectWidth
        {
            get { return this.endpointRectSize.Width; }
        }

        /// <summary>
        /// 获取端点矩形框的高度（单位是像素）。
        /// </summary>
        internal float EndpointRectHeight
        {
            get { return this.endpointRectSize.Height; }
        }

        /// <summary>
        /// 设置图形。
        /// </summary>
        /// <param name="data"></param>
        public void SetFigure(List<EPEPointData[]> data)
        {
            this.CurveList = new List<Curve>(data.Count);

            var createEndpoint = new CreateEndpoint(this.CreateEndpoint);

            foreach (EPEPointData[] epePoints in data)
            {
                Curve curve = new Curve(epePoints, createEndpoint);
                this.CurveList.Add(curve);
            }

            if (this.CurveList.Count > 0)
            {
                this.iCurrentCurveIndex = 0;
            }
            else
            {
                this.iCurrentCurveIndex = INVALID_CURVE_INDEX;
            }
        }

        /// <summary>
        /// 获取线段数组的点
        /// </summary>
        /// <returns>线段集合</returns>
        public List<EPEPointData[]> GetFigure()
        {
            List<EPEPointData[]> list = new List<EPEPointData[]>(this.CurveList.Count);

            foreach (Curve curve in this.CurveList)
            {
                EPEPointData[] points = curve.GetEPEPoints();
                if (points.Length >= 2)
                {
                    EPEPointData[] array = new EPEPointData[points.Length];
                    Array.Copy(points, array, array.Length);
                    list.Add(array);
                }
            }

            return list;
        }

        /// <summary>
        /// 数据
        /// </summary>
        /// <param name="defaultInfo"></param>
        public void SetDefaultSegmentEndPointInfo(EndpointSetting defaultInfo)
        {
            this.defaultEndPointInfo = defaultInfo;
        }

        public EndpointSetting DefaultEndPointInfo
        {
            get { return this.defaultEndPointInfo; }
        }

        /// <summary>
        /// 重建背景位图。
        /// </summary>
        private void RebuildBackgroundBitmap()
        {
            if (this.backgroundBitmap != null)
            {
                if (this.backgroundBitmap.Width != Width || this.backgroundBitmap.Height != Height)
                {
                    this.backgroundBitmap.Dispose();
                    this.backgroundBitmap = null;

                    if (Width > 10 && Height > 10)
                    {
                        this.backgroundBitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                    }
                }
            }
            else
            {
                if (Width > 10 && Height > 10)
                {
                    try
                    {
                        this.backgroundBitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        if (Debugger.IsAttached)
                            Debugger.Break();
                    }
                }
            }

            if (Width > 10 && Height > 10 && this.backgroundBitmap != null)
            {
                Point2D newOrigin = InitBackgroundBitmap();
                SetLogicalCoordinateSystemOrigin(newOrigin);
            }
        }

        public Point2D LogicalCoordinateSystemOrigin
        {
            get { return this.coordinateSystemOrigin; }
            private set { SetLogicalCoordinateSystemOrigin(value); }
        }

        private void SetLogicalCoordinateSystemOrigin(Point2D newOrigin)
        {
            if (this.coordinateSystemOrigin != newOrigin)
            {
                var eventArgs = new LogicalCoordinateSystemOriginChangedEventArgs(this.coordinateSystemOrigin, newOrigin);
                this.coordinateSystemOrigin = newOrigin;
                
                OnLogicalCoordinateSystemOriginChanged(eventArgs);
            }
        }

        protected virtual void OnLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
            if (this.handlerDict != null)
            {
                this.handlerDict[this.actionEnum].HandleLogicalCoordinateSystemOriginChanged(eventArgs);
            }
        }

        /// <summary>
        /// 获取或设置 背景图片的信息。
        /// </summary>
        public BackgroundImageInfo BackgroundImageInfo
        {
            get
            {
                this.backgroundImageInfo.MatrixElements = this.backgroundImageMatrix.Elements;
                return new BackgroundImageInfo(this.backgroundImageInfo);
            }
            set
            {
                this.backgroundImageInfo.Set(value);

                float m11 = this.backgroundImageInfo.MatrixElements[0];
                float m12 = this.backgroundImageInfo.MatrixElements[1];
                float m21 = this.backgroundImageInfo.MatrixElements[2];
                float m22 = this.backgroundImageInfo.MatrixElements[3];
                float dx = this.backgroundImageInfo.MatrixElements[4];
                float dy = this.backgroundImageInfo.MatrixElements[5];
                this.backgroundImageMatrix = new Matrix(m11, m12, m21, m22, dx, dy);

                if (this.backgroundImageInfo.Image != null)
                {
                    RebuildBackgroundBitmap();
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 旋转背景图片指定的角度。
        /// </summary>
        /// <param name="angleDelta">角度增量，单位是角度。</param>
        public void RotateBackgroundImage(float angleDelta)
        {
            this.backgroundImageMatrix.RotateAt(angleDelta, (PointF)this.coordinateSystemOrigin, MatrixOrder.Append);

            if (this.backgroundImageInfo.Image != null)
            {
                RebuildBackgroundBitmap();
                Invalidate();
            }
        }

        /// <summary>
        /// 平移背景图片。
        /// </summary>
        /// <param name="offsetX">x平移</param>
        /// <param name="offsetY">y平移</param>
        public void TranslateBackgroundImage(float offsetX, float offsetY)
        {
            this.backgroundImageMatrix.Translate(offsetX, offsetY, MatrixOrder.Append);

            if (this.backgroundImageInfo.Image != null)
            {
                RebuildBackgroundBitmap();
                Invalidate();
            }
        }

        /// <summary>
        /// 获取缩放比。
        /// </summary>
        /// <returns>缩放比列</returns>
        public float GetScale()
        {
            return this.scale;
        }

        /// <summary>
        /// 设置缩放比。
        /// </summary>
        /// <param name="scale">缩放比列</param>
        /// <returns>新的缩放比。</returns>
        public float SetScale(float scale)
        {
            if (scale >= 1 && scale != this.scale)
            {
                var eventArgs = new ScaleChangedEventArgs(oldValue: this.scale, newValue: scale);

                this.scale = scale;   

                OnScaleChanged(eventArgs);                
            }

            return this.scale;
        }

        /// <summary>
        /// 初始化背景位图。
        /// </summary>
        /// <returns>中心点</returns>
        private Point2D InitBackgroundBitmap()
        {
            using (Graphics graphics = Graphics.FromImage(this.backgroundBitmap))
            {
                graphics.Clear(Color.White);

                Point2D bitmapCenter = new Point2D(this.backgroundBitmap.Width / 2F, this.backgroundBitmap.Height / 2F);

                InitBackgroundBitmap_BackgroundImage(graphics, bitmapCenter);

                InitBackgroundBitmap_Grids(graphics, bitmapCenter);
                InitBackgroundBitmap_CoordinateAxies(graphics, bitmapCenter);

                InitBackgroundBitmap_ChipRectangle(graphics, bitmapCenter);

                return bitmapCenter;
            }
        }

        private void InitBackgroundBitmap_BackgroundImage(Graphics graphics, Point2D bitmapCenter)
        {
            if (this.backgroundImageInfo.Image != null)
            {
                SizeF imageSize = this.backgroundImageInfo.Image.Size;


                Matrix tmpMatrix = new Matrix();
                tmpMatrix.Scale(this.scale, this.scale, MatrixOrder.Append);
                float offsetX = bitmapCenter.X - imageSize.Width / 2F * this.scale;
                float offsetY = bitmapCenter.Y - imageSize.Height / 2F * this.scale;
                tmpMatrix.Translate(offsetX, offsetY, MatrixOrder.Append);               

                tmpMatrix.Multiply(this.backgroundImageMatrix, MatrixOrder.Append);

                GraphicsState state = graphics.Save();
                graphics.Transform = tmpMatrix;
                graphics.DrawImage(this.backgroundImageInfo.Image, 0, 0);
                graphics.Restore(state);
            }
        }


        private void InitBackgroundBitmap_Grids(Graphics graphics, Point2D center)
        {
            float blockWidth = 50 * this.scale;
            float blockHeight = blockWidth;
            int iBlockSize = (int)(this.backgroundBitmap.Width / blockWidth);

            {
                PointF pt1 = new PointF(0, 0);
                PointF pt2 = new PointF(0, this.backgroundBitmap.Height);

                for (float x = center.X - blockWidth; x >= 0; x -= blockWidth)
                {
                    pt1.X = pt2.X = x;
                    graphics.DrawLine(this.gridLinePen, pt1, pt2);
                }

                for (float x = center.X + blockWidth; x < this.backgroundBitmap.Width; x += blockWidth)
                {
                    pt1.X = pt2.X = x;
                    graphics.DrawLine(this.gridLinePen, pt1, pt2);
                }
            }

            {
                PointF pt1 = new Point(0, 0);
                PointF pt2 = new Point(this.backgroundBitmap.Width, 0);

                for (float y = center.Y - blockHeight; y >= 0; y -= blockHeight)
                {
                    pt1.Y = pt2.Y = y;
                    graphics.DrawLine(this.gridLinePen, pt1, pt2);
                }

                for (float y = center.Y + blockHeight; y < this.backgroundBitmap.Height; y += blockHeight)
                {
                    pt1.Y = pt2.Y = y;
                    graphics.DrawLine(this.gridLinePen, pt1, pt2);
                }
            }
        }

        private void InitBackgroundBitmap_CoordinateAxies(Graphics graphics, Point2D center)
        {
            {
                // y-axis.
                PointF pt1 = new PointF(center.X, 0);
                PointF pt2 = new PointF(center.X, this.backgroundBitmap.Height);
                graphics.DrawLine(this.coordinateAxisPen, pt1, pt2);
            }

            {
                // x-axis.
                PointF pt1 = new PointF(0, center.Y);
                PointF pt2 = new PointF(this.backgroundBitmap.Width, center.Y);
                graphics.DrawLine(this.coordinateAxisPen, pt1, pt2);
            }
        }

        private void InitBackgroundBitmap_ChipRectangle(Graphics graphics, Point2D center)
        {
            if (this.dieSize.Width > 0 && this.dieSize.Height > 0)
            {
                float halfWidth = (this.dieSize.Width / 2F) * this.scale;
                float halfHeight = (this.dieSize.Height / 2F) * this.scale;

                PointF[] points = new PointF[4];
                points[0] = (PointF)center.Translate(-1 * halfWidth, -1 * halfHeight);
                points[1] = (PointF)center.Translate(halfWidth, -1 * halfHeight);
                points[2] = (PointF)center.Translate(halfWidth, halfHeight);
                points[3] = (PointF)center.Translate(-1 * halfWidth, halfHeight);

                graphics.DrawPolygon(Pens.Red, points);
            }
        }

        /// <summary>
        /// 添加新的曲线。
        /// </summary>
        /// <returns>新添加的曲线的Index。</returns>
        internal int AddCurve()
        {
            Curve curve = new Curve();
            this.CurveList.Add(curve);
            this.iCurrentCurveIndex = this.CurveList.Count - 1;

            return this.CurveList.Count - 1;
        }

        protected virtual void OnDieSizeChanged(DieSizeChangedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var handler = DieSizeChanged;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="eventArgs">参数</param>
        /// <exception cref="ArgumentNullException">空指针异常</exception>
        internal void OnCurveAdded(CurveAddedEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            var handler = CurveAdded;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        protected void OnCurrentCurveIndexChanged(CurrentCurveIndexChangedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var handler = CurrentCurveIndexChanged;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        protected void OnCurveDeleted(CurveDeletedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var handler = CurveDeleted;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        internal void OnEndpointAppended(EndpointAppendedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var handler = EndpointAppended;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        internal void OnEndpointInserted(EndpointInsertedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var handler = EndpointInserted;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        internal void OnEndpointChanged(EndpointChangedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var handler = EndpointChanged;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        internal void OnMousePointChanged(MousePointChangedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            var handler = MousePointChanged;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// 删除点
        /// </summary>
        /// <param name="eventArgs">参数</param>
        /// <exception cref="ArgumentNullException">空指针异常</exception>
        internal void OnEndpointErased(EndpointDeletedEventArgs eventArgs)
        {
            if (eventArgs == null)
            {
                throw new ArgumentNullException(nameof(eventArgs));
            }

            var handler = EndpointDeleted;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        protected void OnScaleChanged(ScaleChangedEventArgs eventArgs)
        {
            if (eventArgs == null)
                throw new ArgumentNullException(nameof(eventArgs));

            RebuildBackgroundBitmap();
            UpdateAllCurves();

            this.handlerDict[this.actionEnum].HandleScaleChanged(eventArgs);

            var handler = ScaleChanged;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }            

            Invalidate();
        }
       
        /// <summary>
        /// Paint 事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            // 画背景和曲线。
            PaintBackgroundImageAndCurves(graphics);

            // 调用已注册的事件处理函数。
            this.handlerDict[this.actionEnum].HandlePaintEvent(graphics);
        }

        /// <summary>
        /// 绘制背景和曲线
        /// </summary>
        /// <param name="graphics">画布</param>
        private void PaintBackgroundImageAndCurves(Graphics graphics)
        {
            //
            // (1) 画背景图。
            //
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.DrawImage(this.backgroundBitmap, 0, 0);

            GraphicsState state = graphics.Save();

            //
            // (2) 画出所有曲线。
            //
            if (this.CurveList.Count > 0)
            {
                //
                // 先画出其它曲线。
                // 
                for (int i = 0; i < this.CurveList.Count; i++)
                {
                    if (i != this.iCurrentCurveIndex)
                    {
                        this.CurveList[i].Paint(graphics, this.otherCurvePaintingToolSet);
                    }
                }

                //
                // 画当前曲线，并高亮所选线段。
                // 
                Debug.Assert(this.iCurrentCurveIndex >= 0);

                Curve currentCurve = this.CurveList[this.iCurrentCurveIndex];

                Debug.Assert(currentCurve != null);

                currentCurve.Paint(
                    graphics, 
                    this.currentCurvePaintingToolSet,
                    this.iHighlightedSegmentIndex,
                    this.iHighlightedEndPointIndex,
                    this.highlightPaintingToolSet);
            }

            graphics.Restore(state);
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.handlerDict[this.actionEnum].HandleMouseDownEvent(e);
        }

        /// <summary>
        /// 鼠标弹起
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            this.handlerDict[this.actionEnum].HandleMouseUpEvent(e);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
                                
            this.handlerDict[this.actionEnum].HandleMouseMoveEvent(e);

            Point2D mousePixelPoint = (Point2D)e.Location;
            Point2D mouseLogicalPoint = ConvertToLogicalPoint(mousePixelPoint);
            OnMousePointChanged(new MousePointChangedEventArgs(mouseLogicalPoint, mousePixelPoint));
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this.handlerDict[this.actionEnum].HandleMouseEnterEvent(e);
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this.handlerDict[this.actionEnum].HandleMouseLeaveEvent(e);
        }

        /// <summary>
        /// 鼠标滚轮滚动
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0)
            {
                ZoomOutEx();
            }
            else
            {
                ZoomInEx();                
            }            
        }

        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            this.handlerDict[this.actionEnum].HandleKeyDownEvent(e);
        }

        /// <summary>
        /// 键盘弹起
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            this.handlerDict[this.actionEnum].HandleKeyUpEvent(e);
        }

        /// <summary>
        /// 键盘按住
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);

            if (e.KeyChar == CHAR_ESC)
            {
                this.handlerDict[this.actionEnum].HandleExitActionRequest(e);
            }
            else
            {
                this.handlerDict[this.actionEnum].HandleKeyPressEvent(e);
            }
        }

        /// <summary>
        /// 尺寸稿件
        /// </summary>
        /// <param name="eventArgs">参数</param>
        protected override void OnSizeChanged(EventArgs eventArgs)
        {
            base.OnSizeChanged(eventArgs);

            RebuildBackgroundBitmap();
             UpdateAllCurves();

            Invalidate();
        }

        /// <summary>
        /// 动作枚举修改时
        /// </summary>
        /// <param name="eventArgs">参数</param>
        protected virtual void OnActionChanging(ActionChangedEventArgs eventArgs)
        {
            if (eventArgs.OldActionEnum == ActionEnum.PaintCurve)
            {
                if (this.CurveList.Count > 0)
                {
                    Curve currentCurve = this.CurveList[this.iCurrentCurveIndex];
                    if (currentCurve.EndpointCount < 2)
                    {
                        RemoveCurve(this.iCurrentCurveIndex);
                    }
                }
            }

            var handler = ActionChanged;
            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }

            Invalidate();
        }

        /// <summary>
        /// 动作枚举修改后
        /// </summary>
        /// <param name="eventArgs">参数</param>
        protected virtual void OnActionChanged(ActionChangedEventArgs eventArgs)
        {
            var handler = ActionChanged;

            if (handler != null)
            {
                handler.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// 更新所有曲线。
        /// </summary>
        private void UpdateAllCurves()
        {
            var createEndpoint = new CreateEndpoint(this.CreateEndpoint);

            foreach (Curve curve in this.CurveList)
            {
                curve.UpdateAllEndpoints(createEndpoint);
            }
        }

        /// <summary>
        /// 将逻辑尺寸转换成像素尺寸。
        /// </summary>
        /// <param name="logicalSize">逻辑尺寸</param>
        /// <returns>像素尺寸</returns>
        internal SizeF ConvertToPixelSize(SizeF logicalSize)
        {
            return new SizeF(logicalSize.Width / this.scale, logicalSize.Height / this.scale);
        }

        /// <summary>
        /// 将像素尺寸转换成逻辑尺寸。
        /// </summary>
        /// <param name="pixelLength">像素尺寸</param>
        /// <returns>逻辑尺寸</returns>
        internal SizeF ConvertToLogicalSize(SizeF pixelSize)
        {
            return new SizeF(pixelSize.Width * this.scale, pixelSize.Height * this.scale);
        }


        /// <summary>
        /// 将像素点转换成逻辑点。
        /// </summary>
        /// <param name="pixelPoint"></param>
        /// <returns>转换成的逻辑点。</returns>
        internal Point2D ConvertToLogicalPoint(Point2D pixelPoint)
        {           
            float x = (pixelPoint.X - this.coordinateSystemOrigin.X) / this.scale;
            float y = (this.coordinateSystemOrigin.Y - pixelPoint.Y) / this.scale;
            return new Point2D(x, y);
        }

        /// <summary>
        /// 将逻辑点转换成像素点。
        /// </summary>
        /// <param name="logicalPoint">逻辑点</param>
        /// <returns>转换成的像素点。</returns>
        internal Point2D ConvertToPixelPoint(Point2D logicalPoint)
        {
            return CreateRealPoint(logicalPoint);
        }

        /// <summary>
        /// 根据当前环境（比例值和中心的位置）计算出实际点，即逻辑坐标转换为实际坐标。
        /// </summary>
        /// <param name="logicalPoint">逻辑点，即端点的逻辑位置。</param>
        /// <returns>实际点。</returns>
        internal Point2D CreateRealPoint(Point2D logicalPoint)
        {
            float x = this.coordinateSystemOrigin.X + logicalPoint.X * this.scale;
            float y = this.coordinateSystemOrigin.Y - logicalPoint.Y * this.scale;

            return new Point2D(x, y);
        }

        /// <summary>
        /// 创建真实点和端点矩形框，单位都是像素。
        /// </summary>
        /// <param name="logicalPoint"></param>
        /// <param name="realPoint"></param>
        /// <param name="realRectangle"></param>
        internal void CreateRealPointAndRealRectangle(Point2D logicalPoint, out Point2D realPoint, out RectangleF realRectangle)
        {
            float realPointX = this.coordinateSystemOrigin.X + logicalPoint.X * this.scale;
            float realPointY = this.coordinateSystemOrigin.Y - logicalPoint.Y * this.scale;
            realPoint = new Point2D(realPointX, realPointY);

            float rectX = realPointX - this.endpointRectSize.Width / 2;
            float rectY = realPointY - this.endpointRectSize.Height / 2;
            realRectangle = new RectangleF(new PointF(rectX, rectY), this.endpointRectSize);
        }

        /// <summary>
        /// 创建端点。
        /// </summary>
        /// <param name="logicalPoint"></param>
        /// <returns>新建的端点。</returns>
        internal Endpoint CreateEndpoint(Point2D logicalPoint)
        {
            Point2D realPoint;
            RectangleF realRect;
            CreateRealPointAndRealRectangle(logicalPoint, out realPoint, out realRect);

            return new Endpoint(logicalPoint, realPoint, realRect);
        }



        #region Painting.

        ///// <summary>
        ///// 画末端有实心箭头的虚线段。
        ///// </summary>
        ///// <param name="graphics"></param>
        ///// <param name="initialPixelPoint"></param>
        ///// <param name="terminalPixelPoint"></param>
        //internal void FillArrowDashLineSegment(Graphics graphics, Point2D initialPixelPoint, Point2D terminalPixelPoint)
        //{
        //    GraphicsState state = graphics.Save();
        //    graphics.SmoothingMode = SmoothingMode.HighQuality;
        //    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        //    Utility.FillVector(graphics, initialPixelPoint, terminalPixelPoint, _dashPen, _highlightPaintTool.Brush);

        //    graphics.Restore(state);
        //}

        /// <summary>
        /// 高亮曲线上的指定端点。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="iEndpointIndex"></param>
        internal void HighlightCurveEndpoint(Graphics graphics, Curve curve, int iEndpointIndex)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            if (curve == null)
                throw new ArgumentNullException(nameof(curve));
            
            curve.HighlightEndpoint(graphics, iEndpointIndex, this.highlightPaintingToolSet.Brush);
        }

        /// <summary>
        /// 高亮当前曲线上的指定线段。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="iSegmentIndex"></param>
        internal void HighlightLineSegment(Graphics graphics, Curve curve, int iSegmentIndex)
        {
            HighlightLineSegment(graphics, curve, iSegmentIndex, this.highlightPaintingToolSet);
        }

        /// <summary>
        /// 高亮曲线上的指定线段。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="iSegmentIndex"></param>
        /// <param name="highlightPaintTool"></param>
        internal void HighlightLineSegment(Graphics graphics, Curve curve, int iSegmentIndex, PaintingToolSet highlightPaintTool)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            if (curve == null)
                throw new ArgumentNullException(nameof(curve));

            if (highlightPaintTool == null)
                throw new ArgumentNullException(nameof(highlightPaintTool));

            curve.HighlightSegment(graphics, iSegmentIndex, highlightPaintTool);
        }

        /// <summary>
        /// 为指定的端点画坐标线。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="endpoint"></param>
        internal void PaintCoordinateLines(Graphics graphics, Endpoint endpoint)
        {
            PaintCoordinateLines(graphics, endpoint.LogicalPoint, endpoint.RealPoint);
        }

        /// <summary>
        /// 为指定的端点画坐标线。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="logicalPoint"></param>
        /// <param name="pixelPoint"></param>
        internal void PaintCoordinateLines(Graphics graphics, Point2D logicalPoint, Point2D pixelPoint)
        {
            PointF leftPoint = new PointF(0, pixelPoint.Y);
			PointF rightPoint = new PointF(Width, pixelPoint.Y);
			graphics.DrawLine(this.coordinateLinePen, leftPoint, rightPoint);

			PointF topPoint = new PointF(pixelPoint.X, 0);
			PointF bottomPoint = new PointF(pixelPoint.X, Height);
			graphics.DrawLine(this.coordinateLinePen, topPoint, bottomPoint);

			graphics.DrawString(logicalPoint.Y.ToString(), Font, this.coordinateValueBrush, leftPoint);
			graphics.DrawString(logicalPoint.X.ToString(), Font, this.coordinateValueBrush, topPoint);
        }

        #endregion

        #region Zoom.

        /// <summary>
        /// 放大或缩小。
        /// </summary>
        /// <param name="delta"></param>
        public void Zoom(float delta)
        {
            if (delta > 0)
            {
                if (this.scale < 16)
                {
                    SetScale(this.scale + delta);
                }
            }
            else if (delta < 0)
            {
                SetScale(this.scale + delta);
            }
            else
            { }
        }

        /// <summary>
        /// 放大0.5。
        /// </summary>
        public void ZoomIn()
        {
            Zoom(0.5F);
        }

        /// <summary>
        /// 缩小0.5。
        /// </summary>
        public void ZoomOut()
        {
            Zoom(-0.5F);
        }

        /// <summary>
        /// 放大0.1。
        /// </summary>
        public void ZoomInEx()
        {
            Zoom(0.1F);
        }

        /// <summary>
        /// 缩小0.1。
        /// </summary>
        public void ZoomOutEx()
        {
            Zoom(-0.1F);
        }

        #endregion

        #region Context Menu.

        private void contextMenuStrip1_MouseLeave(object sender, EventArgs e)
        {
            ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
            contextMenuStrip.Hide();
        }

        /// <summary>
        /// 右击菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip contextMenuStrip = (ContextMenuStrip)sender;
            ToolStripItemCollection itemCollection = contextMenuStrip.Items;

            int iVisibleActionItemCount = 0;
            // actions
            {
                if (this.actionEnum == ActionEnum.None)
                {
                    // Show all basic actions.
                    foreach (string strName in s_basicActionNames)
                    {
                        var item = (ToolStripMenuItem)itemCollection[strName];
                        item.Visible = true;
                        item.Enabled = true;
                        item.Checked = false;
                        iVisibleActionItemCount++;
                    }

                    if (CurrentCurveIndex == INVALID_CURVE_INDEX)
                    {
                        // Hide all curve actions.
                        foreach (string strName in s_curveActionNames)
                        {
                            var item = itemCollection[strName];
                            item.Visible = false;                         
                        }

                        // Show "add curve" always.
                        {
                            var item = (ToolStripMenuItem)itemCollection[CMI_CURVE_ACTION_ADD_CURVE];
                            item.Visible = true;
                            item.Enabled = true;
                            item.Checked = false;
                            iVisibleActionItemCount++;
                        }

                        // Show the separator between basic actions and curve actions.
                        {
                            itemCollection[CMI_SEPARATOR_BETWEEN_BASIC_AND_CURVE].Visible = true;
                        }

                        // Hide the separator embedded in curve actions.
                        {
                            itemCollection[CMI_SEPARATOR_EMBEDDED_IN_CURVE_ACTIONS].Visible = false;
                        }
                    }
                    else
                    {
                        // Show all curve actions.
                        foreach (string strName in s_curveActionNames)
                        {
                            var item = (ToolStripMenuItem)itemCollection[strName];
                            item.Visible = true;
                            item.Enabled = true;
                            item.Checked = false;
                            iVisibleActionItemCount++;
                        }
                    }
                }
                else
                {
                    if (this.actionEnum == ActionEnum.RotateBackgroundImage || this.actionEnum == ActionEnum.TranslateBackgroundImage)
                    {
                        foreach (string strBasicActionName in s_basicActionNames)
                        {
                            var menuItem = (ToolStripMenuItem)itemCollection[strBasicActionName];
                            menuItem.Visible = true;
                            menuItem.Enabled = true;
                            menuItem.Checked = false;
                            iVisibleActionItemCount++;
                        }

                        {
                            string strActionName = s_actionNameDict[this.actionEnum];
                            var menuItem = (ToolStripMenuItem)itemCollection[strActionName];
                            menuItem.Visible = true;
                            menuItem.Checked = true;
                            menuItem.Enabled = false;
                            iVisibleActionItemCount++;
                        }

                        foreach (string strCurveActionName in s_curveActionNames)
                        {
                            var menuItem = (ToolStripMenuItem)itemCollection[strCurveActionName];
                            menuItem.Visible = false;
                        }
                    }
                    else
                    {
                        // Hide all basic actions.
                        foreach (string strBasicActionName in s_basicActionNames)
                        {
                            var menuItem = (ToolStripMenuItem)itemCollection[strBasicActionName];
                            menuItem.Visible = false;
                        }

                        // Hide the separator between basic actions and curve actions.
                        {
                            itemCollection[CMI_SEPARATOR_BETWEEN_BASIC_AND_CURVE].Visible = false;
                        }

                        // Show all curve actions.
                        foreach (string strCurveActionName in s_curveActionNames)
                        {
                            var menuItem = (ToolStripMenuItem)itemCollection[strCurveActionName];
                            menuItem.Visible = true;
                            menuItem.Enabled = true;
                            menuItem.Checked = false;
                            iVisibleActionItemCount++;
                        }

                        // Show the separator embedded in the curve actions.
                        {
                            itemCollection[CMI_SEPARATOR_EMBEDDED_IN_CURVE_ACTIONS].Visible = true;
                        }

                        // Disable and check the current curve actionEnum.
                        {
                            string strActionName = s_actionNameDict[this.actionEnum];
                            var menuItem = (ToolStripMenuItem)itemCollection[strActionName];
                            menuItem.Visible = true;
                            menuItem.Checked = true;
                            menuItem.Enabled = false;
                        }
                    }
                }
            }

            // "exit actionEnum"
            {
                if (this.actionEnum == ActionEnum.None)
                {
                    itemCollection[CMI_EXIT_ACTION].Visible = false;
                }
                else
                {
                    itemCollection[CMI_EXIT_ACTION].Visible = true;
                    itemCollection[CMI_EXIT_ACTION].Enabled = true;
                }
            }

            // "cancel"
            {
                ToolStripItem tsmiCancel = itemCollection[CMI_CANCEL];
                tsmiCancel.Visible = true;

                this.toolStripSeparatorBeforeExitAndCancel.Visible = iVisibleActionItemCount > 0;
            }
        }

        /// <summary>
        /// 平移背景图片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiTranslateBackgroundImage_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.TranslateBackgroundImage);
        }

        /// <summary>
        /// 旋转背景图片  少用
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiRotateBackgroundImage_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.RotateBackgroundImage);
        }

        /// <summary>
        /// 重置背景图片 
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiResetBackgroundImageAngle_Click(object sender, EventArgs e)
        {
            this.backgroundImageMatrix.Reset();

            //_backgroundImageInfo.OffsetX = 0;
            //_backgroundImageInfo.OffsetY = 0;
            //_backgroundImageInfo.RotationDegree = 0;

            if (this.backgroundImageInfo.Image != null)
            {
                RebuildBackgroundBitmap();
                Invalidate();
            }

            SetAction(ActionEnum.None);
        }

        private void tsmiResetBackgroundImagePosition_Click(object sender, EventArgs e)
        {
            this.backgroundImageMatrix.Reset();

            /*
            _backgroundImageInfo.OffsetX = 0;
            _backgroundImageInfo.OffsetY = 0;
            _backgroundImageInfo.RotationDegree = 0;
            */

            if (this.backgroundImageInfo.Image != null)
            {
                RebuildBackgroundBitmap();
                Invalidate();
            }

            SetAction(ActionEnum.None);
        }

        /// <summary>
        /// 结束动作
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiExitAction_Click(object sender, EventArgs e)
        {
            this.handlerDict[this.actionEnum].HandleExitActionRequest(EventArgs.Empty);
        }

        /// <summary>
        /// 移动点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiMovePoint_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.MovePoint);
        }

        /// <summary>
        /// 插入点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiInsertPoint_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.InsertPoint);
        }

        /// <summary>
        /// 追加点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiAppendPoint_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.AppendPoint);
        }

        /// <summary>
        /// 删除点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiErasePoint_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.DeletePoint);
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiAddCurve_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.PaintCurve);
        }

        /// <summary>
        /// 缩放曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiScaleCurve_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.ScaleCurve);
        }

        /// <summary>
        /// 平移曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiTranslateCurve_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.TranslateCurve);
        }

        /// <summary>
        /// 旋转曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiRotateCurve_Click(object sender, EventArgs e)
        {
            SetAction(ActionEnum.RotateCurve);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void tsmiCancel_Click(object sender, EventArgs e)
        { }

        #endregion

        public void SaveSnapshoot(string strFilePath)
        {
            if (strFilePath == null)
                throw new ArgumentNullException(nameof(strFilePath));

            //Bitmap bitmap = new Bitmap(Width, Height);
            int iBitmapWidth = (int)(this.dieSize.Width);
            int iBitmapHeight = (int)(this.dieSize.Height);
            Bitmap bitmap = new Bitmap(iBitmapWidth, iBitmapHeight);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                float dx = -1 * (this.backgroundBitmap.Width - iBitmapWidth) / 2F;
                float dy = -1 * (this.backgroundBitmap.Height - iBitmapHeight) / 2F;
                graphics.TranslateTransform(dx, dy);
                PaintBackgroundImageAndCurves(graphics);
            }

            bitmap.Save(strFilePath);
            bitmap.Dispose();
        }


    }
}
