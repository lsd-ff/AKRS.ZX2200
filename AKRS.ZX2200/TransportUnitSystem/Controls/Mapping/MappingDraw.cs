using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    using System;

    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.TransportUnitSystem.Service;

    /// <summary>
    /// 图像处理的方法
    /// </summary>
    public class MappingDraw
    {
        /// <summary>
        /// 绿色笔刷
        /// </summary>
        private readonly SolidBrush solidBrushLine = new SolidBrush(Color.Lime);

        /// <summary>
        /// 天蓝笔刷
        /// </summary>
        private readonly SolidBrush solidBrushCyan = new SolidBrush(Color.Cyan);

        /// <summary>
        /// 红色笔刷
        /// </summary>
        private readonly SolidBrush solidBrushCrimson = new SolidBrush(Color.Crimson);

        /// <summary>
        /// 灰色笔刷
        /// </summary>
        private readonly SolidBrush solidBrushDarkDark = new SolidBrush(System.Drawing.SystemColors.ControlDarkDark);

        /// <summary>
        /// 其他配置的对象
        /// </summary>
        private OtherConfig OtherConfig => ProductConfiguration.GetInstance().OtherConfig;

        /// <summary>
        /// 配置文件的集合
        /// </summary>
        private List<MatterProductInformation> matterProductStateList;

        /// <summary>
        /// 配置文件的集合
        /// </summary>
        private List<MatterProductInformationMin>[][] matterProductStateListMax;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matterProductStateList">配置集合</param>
        public MappingDraw(List<MatterProductInformation> matterProductStateList)
        {
            this.matterProductStateList = matterProductStateList;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="matterProductStateList">配置集合</param>
        public MappingDraw(List<MatterProductInformationMin>[][] matterProductStateList)
        {
            this.matterProductStateListMax = matterProductStateList;
        }

        /// <summary>
        /// 绘制选项框
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="g">绘画对象</param>
        /// <param name="list">全部集合</param>
        /// <param name="selectList">被选中的集合</param>
        /// <param name="panelControl">显示的控件</param>
        public void DrawEntities<T>(Graphics g, List<T> list, List<T> selectList, PanelControl panelControl) where T : BaseMatter
        {
            this.DrawingFrame(g, panelControl);
            this.DrawingSub(g, list, panelControl);
            this.DrawSelectSub(g, selectList);
        }

        /// <summary>
        /// 绘制传输单元
        /// </summary>
        /// <param name="g">画笔</param>
        /// <param name="panelControl">控件</param>
        public void DrawingFrame(Graphics g, PanelControl panelControl)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;

            // 重置坐标系
            g.ResetTransform();

            // 清空
            g.Clear(panelControl.BackColor);

            // 先画出TU的形状
            Pen pen = new Pen(Color.Black);

            int width = panelControl.Width;

            int height = panelControl.Height;

            g.DrawRectangle(pen, 10, 10, width - 20, height - 20);
        }

        /// <summary>
        /// 绘制基板
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="g">绘画对象</param>
        /// <param name="list">集合</param>
        /// <param name="panelControl">控件</param>
        public void DrawingSub<T>(Graphics g, List<T> list,PanelControl panelControl) where T : BaseMatter
        {
            // 画笔
            Pen pen = new Pen(Color.Black);

            TuService.PointFNormalization(
                list,
                panelControl.Width - 100,
                panelControl.Height - 80);

            for (int i = 0; i < list.Count; i++)
            {
                this.DrawBaseEntity(g, list[i], pen);
            }
        }

        /// <summary>
        /// 绘制被选中的基板
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="g">绘画对象</param>
        /// <param name="list">结合</param>
        public void DrawSelectSub<T>(Graphics g, List<T> list) where T : BaseMatter
        {
            // 画笔
            Pen pen = new Pen(Color.Blue);

            foreach (T t in list)
            {
                this.DrawBaseEntityFrame(g, t, pen);
            }
        }

        /// <summary>
        /// 绘制图形选项框
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="baseEntity">实体</param>
        /// <param name="pen">笔</param>
        public void DrawBaseEntityFrame(Graphics g, BaseMatter baseEntity, Pen pen)
        {
            g.DrawRectangle(
                pen,
                new Rectangle(
                    baseEntity.Rectangle.X - 5,
                    baseEntity.Rectangle.Y - 5,
                    baseEntity.Rectangle.Width + 10,
                    baseEntity.Rectangle.Height + 10));
        }


        /// <summary>
        /// 绘制取消图形选项框
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="baseEntity">实体</param>
        /// <param name="color">颜色</param>
        public void DrawBaseEntityCancelFrame(Graphics g, BaseMatter baseEntity,Color color)
        {
            Pen pen = new Pen(color);
            g.DrawRectangle(
                pen,
                new Rectangle(
                    baseEntity.Rectangle.X - 5,
                    baseEntity.Rectangle.Y - 5,
                    baseEntity.Rectangle.Width + 10,
                    baseEntity.Rectangle.Height + 10));
        }

        /// <summary>
        /// 绘制图形
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="baseEntity">实体</param>
        /// <param name="pen">笔</param>
        public void DrawBaseEntity(Graphics g, BaseMatter baseEntity, Pen pen)
        {
            if (baseEntity.MatterProductState == MatterProductState.Enable)
            {
                g.FillRectangle(this.solidBrushLine, baseEntity.Rectangle);
            }
            else if(baseEntity.MatterProductState == MatterProductState.EnableInSystem1)
            {
                g.FillRectangle(this.solidBrushCyan, baseEntity.Rectangle);
            }
            else if (baseEntity.MatterProductState == MatterProductState.EnableInSystem2)
            {
                g.FillRectangle(this.solidBrushCrimson, baseEntity.Rectangle);
            }
            else if (baseEntity.MatterProductState == MatterProductState.Disable)
            {
                g.FillRectangle(this.solidBrushDarkDark, baseEntity.Rectangle);
            }

            g.DrawRectangle(pen, baseEntity.Rectangle);

            Font font = new Font("宋体", 10F);
            Brush brush = Brushes.Black;

            if (baseEntity is BondPosition)
            {
                g.DrawString(baseEntity.Name, font, brush, baseEntity.Rectangle.X, baseEntity.Rectangle.Y + 5);
            }
            else
            {
                g.DrawString(baseEntity.Index.ToString(), font, brush, baseEntity.Rectangle.X, baseEntity.Rectangle.Y + 5);
            }
        }


        /// <summary>
        /// 鼠标按下选择实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="e">事件</param>
        /// <param name="g">参数</param>
        /// <param name="selectList">选择的集合</param>
        /// <param name="allList">所有的集合</param>
        /// <param name="color">背景颜色</param>
        /// <returns>值</returns>
        public T SelectEntity<T>(MouseEventArgs e, Graphics g, List<T> selectList, List<T> allList,Color color) where T : BaseMatter
        {
            if (e.Button == MouseButtons.Left)
            {
                T entity = null;

                // 判断是否在选中的集合中
                for (int i = 0; i < selectList.Count; i++)
                {
                    if (TuService.PointInRectangles(e.Location, selectList[i].Rectangle))
                    {
                        this.DrawBaseEntityCancelFrame(g, selectList[i],color);
                        entity = selectList[i];
                        selectList.Remove(selectList[i]);

                        return entity;
                    }
                }

                foreach (T baseEntity in allList)
                {
                    if (TuService.PointInRectangles(e.Location, baseEntity.Rectangle))
                    {
                        selectList.Add(baseEntity);
                        entity = baseEntity;
                    }
                }

                this.DrawSelectSub(g, selectList);
                return entity;
            }

            return null;
        }

        /// <summary>
        /// 选择选中的对象
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="startPoint">开始点位</param>
        /// <param name="endPointF">结束点位</param>
        /// <param name="allEntity">所有实体</param>
        /// <param name="selectEntity">被选择的实体</param>
        public void ChooseEntityByRectangle<T>(PointF startPoint, PointF endPointF, List<T> allEntity, List<T> selectEntity) where T : BaseMatter
        {
             selectEntity.Clear();
            foreach (BaseMatter baseEntity in allEntity)
            {
                if (baseEntity.Rectangle.X > startPoint.X - baseEntity.Rectangle.Width 
                    && baseEntity.Rectangle.X < endPointF.X 
                    && baseEntity.Rectangle.Y > startPoint.Y - baseEntity.Rectangle.Height
                    && baseEntity.Rectangle.Y < endPointF.Y)
                {
                    selectEntity.Add((T)baseEntity);
                }
                else if (baseEntity.Rectangle.X < startPoint.X - baseEntity.Rectangle.Width
                         && baseEntity.Rectangle.X > endPointF.X
                         && baseEntity.Rectangle.Y < startPoint.Y - baseEntity.Rectangle.Height
                         && baseEntity.Rectangle.Y > endPointF.Y)
                {
                    selectEntity.Add((T)baseEntity);
                }
            }
        }

        /// <summary>
        /// 改变实体状态
        /// </summary>
        /// <param name="subLists">sub的集合</param>
        /// <param name="moduleList">module的集合</param>
        /// <param name="bpNames">焊点的名称</param>
        /// <param name="matterProduct">状态</param>
        public void ChangeMatterProduct(List<int> subLists, List<int> moduleList, List<string> bpNames, MatterProductState matterProduct)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.LagerNumber)
            {
                this.matterProductStateListMax = ProductConfiguration.GetInstance().OtherConfig.MatterProductInfoMax;

                for (int i = 0; i < subLists.Count; i++)
                {
                    for (int j = 0; j < moduleList.Count; j++)
                    {
                        for (int k = 0; k < bpNames.Count; k++)
                        {
                            this.matterProductStateListMax[subLists[i] - 1][moduleList[j] - 1].RemoveAll(it => it.BondPositionName == bpNames[k]);
                            this.matterProductStateListMax[subLists[i] - 1][moduleList[j] - 1].Add(new MatterProductInformationMin(bpNames[k], matterProduct));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < subLists.Count; i++)
                {
                    for (int j = 0; j < moduleList.Count; j++)
                    {
                        for (int k = 0; k < bpNames.Count; k++)
                        {
                            this.matterProductStateList.RemoveAll(
                                item => item.SubstrateIndex == subLists[i] && item.ModuleIndex == moduleList[j]
                                                                 && item.BondPositionName == bpNames[k]);

                            if (matterProduct != MatterProductState.Enable)
                            {
                                this.matterProductStateList.Add(new MatterProductInformation(subLists[i], moduleList[j], bpNames[k], matterProduct));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 打开选中的实体
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="select">选中的集合</param>
        /// <param name="allList">所有的集合</param>
        public void TurnOnEntities<T>(List<T> select, List<int> allList) where T : BaseMatter
        {
            foreach (T baseEntity in select)
            {
                if (allList.Exists(it => it == baseEntity.Index))
                {
                    allList.RemoveAt(baseEntity.Index);
                }
            }
        }

        /// <summary>
        /// 刷新选项框
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="pnMapping">绘画框</param>
        /// <param name="startPointF">开始点</param>
        /// <param name="endPointF">结束点</param>
        /// <param name="allList">所有点</param>
        /// <param name="seleList">选中的点</param>
        public void MouseMove<T>(XtraTabControl pnMapping, PointF startPointF, PointF endPointF,List<T> allList,List<T> seleList) where T : BaseMatter
        {
            // 刷新重绘选择框
            pnMapping.Refresh();

            // 计算距离
            PointF distance = new PointF(startPointF.X - endPointF.X, startPointF.Y - endPointF.Y);

            // 获取当前位置
            Point screenPoint = Control.MousePosition;

            // 矩形区域
            Rectangle rectangleToPn = new Rectangle(screenPoint.X, screenPoint.Y, (int)distance.X, (int)distance.Y);

            // 相对于屏幕的可逆框架
            ControlPaint.DrawReversibleFrame(rectangleToPn, Color.Black, FrameStyle.Thick);

            // 判断哪些点在矩形框中
            this.ChooseEntityByRectangle(
                startPointF,
                endPointF,
                allList,
                seleList);
        }

        /// <summary>
        /// 选项框是否生效
        /// </summary>
        /// <param name="startPointF">起始点</param>
        /// <param name="enPointF">结束点</param>
        /// <returns>结果</returns>
        public bool IsChooseEnable(PointF startPointF, PointF enPointF)
        {
            double distance = Math.Sqrt(
                (startPointF.X - enPointF.X) * (startPointF.X - enPointF.X)
                + (startPointF.Y - enPointF.Y) * (startPointF.Y - enPointF.Y));

            if (distance > 100)
            {
                return true;
            }

            return false;
        }
    }
}
