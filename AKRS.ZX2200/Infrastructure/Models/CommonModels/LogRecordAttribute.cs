namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    using PostSharp.Aspects;
    using PostSharp.Serialization;

    /// <summary>
    /// log记录
    /// </summary>
    [PSerializable]
    [AttributeUsage(AttributeTargets.Method)]
    public class LogRecordMethodAttribute : OnMethodBoundaryAspect, IOnExceptionAspect
    {
        /// <summary>
        /// 进入时
        /// </summary>
        /// <param name="args">args</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            string logText = string.Format(
                "用户: 执行{0}.{1} 方法.",
                args.Method.DeclaringType.FullName,
                args.Method.Name);
            LogRecordDataHelper.InsertIntoTable(logText);
        }

        ///// <summary>
        ///// 离开时
        ///// </summary>
        ///// <param name="args">args</param>
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    base.OnExit(args);

        //    string logText = string.Format(
        //        "Leaving: {0}.{1} Method. The Result Is：{2}",
        //        args.Method.DeclaringType.FullName,
        //        args.Method.Name,
        //        args.ReturnValue);
        //    LogRecordDataHelper.InsertIntoTable(logText);
        //}

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="args">args</param>
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            base.OnException(args);

            string logText = string.Format(
                "用户: 执行{0}.{1} 方法. 抛异常：{2}",
                args.Method.DeclaringType.FullName,
                args.Method.Name,
                args.Exception.Message);
            LogRecordDataHelper.InsertIntoTable(logText);
        }
    }

    /// <summary>
    /// log记录
    /// </summary>
    [PSerializable]
    [AttributeUsage(AttributeTargets.Property)]
    public class LogRecordPropertyAttribute : OnMethodBoundaryAspect, IOnExceptionAspect
    {
        /// <summary>
        /// 进入时
        /// </summary>
        /// <param name="args">args</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            Arguments arguments = args.Arguments;
            StringBuilder sb = new StringBuilder();
            ParameterInfo[] parameters = args.Method.GetParameters();
            for (int i = 0; arguments != null && i < arguments.Count; i++)
            {
                sb.Append(parameters[i].Name + "=" + arguments[i] + string.Empty);
            }

            string logText = string.Format(
                "用户: 修改{0}.{1} 参数. 修改后的值：{2}",
                args.Method.DeclaringType.FullName,
                args.Method.Name,
                sb.ToString());
            LogRecordDataHelper.InsertIntoTable(logText);
        }

        ///// <summary>
        ///// 离开时
        ///// </summary>
        ///// <param name="args">args</param>
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    base.OnExit(args);

        //    string logText = string.Format(
        //        "Leaving: {0}.{1} Method. The Result Is：{2}",
        //        args.Method.DeclaringType.FullName,
        //        args.Method.Name,
        //        args.ReturnValue);
        //    LogRecordDataHelper.InsertIntoTable(logText);
        //}

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="args">args</param>
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            base.OnException(args);

            string logText = string.Format(
                "用户: 修改{0}.{1} 参数. 抛异常：{2}",
                args.Method.DeclaringType.FullName,
                args.Method.Name,
                args.Exception.Message);
            LogRecordDataHelper.InsertIntoTable(logText);
        }
    }

    /// <summary>
    /// log记录
    /// </summary>
    [PSerializable]
    public class LogRecordButtonAttribute : OnMethodBoundaryAspect, IOnExceptionAspect
    {
        /// <summary>
        /// 进入时
        /// </summary>
        /// <param name="args">args</param>
        public override void OnEntry(MethodExecutionArgs args)
        {
            base.OnEntry(args);

            Arguments arguments = args.Arguments;
            StringBuilder sb = new StringBuilder();
            ParameterInfo[] parameters = args.Method.GetParameters();
            for (int i = 0; arguments != null && i < arguments.Count; i++)
            {
                sb.Append(parameters[i].Name + "=" + arguments[i] + string.Empty);
            }

            string logText = string.Format(
                "用户: 点击{0}.{1} 按钮.",
                args.Method.DeclaringType.FullName,
                args.Method.Name);
            LogRecordDataHelper.InsertIntoTable(logText);
        }

        ///// <summary>
        ///// 离开时
        ///// </summary>
        ///// <param name="args">args</param>
        //public override void OnExit(MethodExecutionArgs args)
        //{
        //    base.OnExit(args);

        //    string logText = string.Format(
        //        "Leaving: {0}.{1} Method. The Result Is：{2}",
        //        args.Method.DeclaringType.FullName,
        //        args.Method.Name,
        //        args.ReturnValue);
        //    LogRecordDataHelper.InsertIntoTable(logText);
        //}

        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="args">args</param>
        public override void OnException(MethodExecutionArgs args)
        {
            args.FlowBehavior = FlowBehavior.Continue;
            base.OnException(args);

            string logText = string.Format(
                "用户: 点击{0}.{1} 按钮. 抛异常：{2}",
                args.Method.DeclaringType.FullName,
                args.Method.Name,
                args.Exception.Message);
            LogRecordDataHelper.InsertIntoTable(logText);
        }
    }

    /// <summary>
    /// log记录-数据帮助类
    /// </summary>
    public class LogRecordDataHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static string strConn = @"Server=.;Database=UserManager;Trusted_Connection=True;";

        /// <summary>
        /// 增加数据
        /// </summary>
        /// <param name="logText">保存文本</param>
        public static void InsertIntoTable(string logText)
        {
            using (SqlConnection con = new SqlConnection(strConn))
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    string sqlStr = "insert into LogRecord (Text,CreateTime) " +
                                    "values('" + logText + "','" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "')";
                    SqlCommand cmd = new SqlCommand(sqlStr, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    AKRSXtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 清除记录数据
        /// </summary>
        public static void ClearLogRecord()
        {
            using (SqlConnection con = new SqlConnection(strConn))
            {
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    string sqlStr = "truncate table LogRecord";
                    SqlCommand cmd = new SqlCommand(sqlStr, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    AKRSXtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 获取记录数据
        /// </summary>
        /// <returns>result</returns>
        public static DataTable GetLogRecord()
        {
            using (SqlConnection con = new SqlConnection(strConn))
            {
                DataTable dt = new DataTable();
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    string sqlStr = "select * from LogRecord order by CreateTime desc";
                    SqlCommand cmd = new SqlCommand(sqlStr, con);
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter msda = new SqlDataAdapter(cmd);
                    msda.Fill(dt);
                }
                catch (Exception ex)
                {
                    AKRSXtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                        con.Dispose();
                    }
                }

                return dt;
            }
        }
    }
}
