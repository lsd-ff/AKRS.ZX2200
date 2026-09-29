namespace AKRS.ZX2200.Infrastructure.Utils
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;


    using DevExpress.XtraGrid.Views.Grid;

    /// <summary>
    /// 这个类用于一些公用的初始化
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 根据系统去选择控件的硬件
        /// </summary>
        /// <param name="ucGuideMove">方向盘</param>
        /// <param name="system">系统</param>
        public static void ChangeUcMove(UcGuideMove ucGuideMove, CurrentMachineSystemEnum system)
        {
            // 不允许在这里创建， 因为没办法释放
            //if (ucGuideMove == null)
            //{
            //    ucGuideMove = new UcGuideMove();
            //}

            if (system == CurrentMachineSystemEnum.System1)
            {
                ucGuideMove.ChangeModuleName("点胶模组",false);
            }
            else
            {
                ucGuideMove.ChangeModuleName("固晶模组", false);
            }
        }

        /// <summary>
        /// 移动排序
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="tList">集合</param>
        /// <param name="currentT">移动项</param>
        /// <param name="direction">方向</param>
        private static void MoveItem<T>(List<T> tList, T currentT, Direction direction) where T : BaseOrderDsSetting
        {
            int targetOrder = currentT.Order;
            int lastOrNext = (direction == Direction.Up) ? -1 : 1;

            List<T> tTempList = tList.FindAll(t => t.Order == targetOrder + lastOrNext);

            tList.Iter(
                t =>
                {
                    t.Order = t.Order - lastOrNext;
                    currentT.Order = currentT.Order + lastOrNext;
                });
            tList.Sort();

            // 重新设置序号
            tList.ForEach(t => t.Order = tList.IndexOf(t));
        }

        ///// <summary>
        ///// 上下移
        ///// </summary>
        ///// <typeparam name="T">类型</typeparam>
        ///// <param name="direction">列表项移动的方向</param>
        ///// <param name="gv">GridView控件</param>
        //public static void UpOrDown<T>(Direction direction, GridView gv) where T : BaseOrderDsSetting
        //{
        //    T t = gv.GetFocusedRow() as T;
        //    if (t == null)
        //    {
        //        return;
        //    }

        //    CommonHelper.MoveItem(ProcessStepProgram.GetInstance().ProcessStepList.Cast<BaseOrderDsSetting>().ToList(), t, direction);

        //    List<ProcessStep> list = ProcessStepProgram.GetInstance().ProcessStepList;

        //    gv.GridControl.DataSource = new BindingList<T>(ProcessStepProgram.GetInstance().ProcessStepList.Cast<T>().ToList());
        //    gv.FocusedRowHandle = t.Order;
        //}

        ///// <summary>
        ///// 上下移
        ///// </summary>
        ///// <param name="direction">列表项移动的方向</param>
        ///// <param name="gv">GridView控件</param>
        //public static void UpOrDown(Direction direction, GridView gv) 
        //{
        //    ProcessStep curt = gv.GetFocusedRow() as ProcessStep;
        //    if (curt == null)
        //    {
        //        return;
        //    }

        //    int targetOrder = curt.Order;
        //    int lastOrNext = (direction == Direction.Up) ? -1 : 1;

        //    List<ProcessStep> tTempList = ProcessStepProgram.GetInstance().ProcessStepList.FindAll(t => t.Order == targetOrder + lastOrNext);

        //    ProcessStep curProcessStep =
        //        ProcessStepProgram.GetInstance().ProcessStepList.Find(t => t.Order == curt.Order);
        //    ProcessStep changeProcessStep = ProcessStepProgram.GetInstance().ProcessStepList
        //        .Find(t => t.Order == curt.Order + lastOrNext);

        //    curProcessStep.Order = targetOrder + lastOrNext;
        //    changeProcessStep.Order = changeProcessStep.Order - lastOrNext;

        //    ProcessStepProgram.GetInstance().ProcessStepList.Sort();

        //    //// 重新设置序号
        //    //ProcessStepProgram.GetInstance().ProcessStepList.ForEach(t => t.Order = ProcessStepProgram.GetInstance().ProcessStepList.IndexOf(t));

        //    List<ProcessStep> list = ProcessStepProgram.GetInstance().ProcessStepList;
        //    gv.GridControl.DataSource = ProcessStepProgram.GetInstance().ProcessStepList;
        //    gv.FocusedRowHandle = curt.Order;
        //}
    }
}
