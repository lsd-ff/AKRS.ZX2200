using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.CalibSystem
{
    using System.IO;

    internal class LogOnePoint : BaseLog
    {
        /// <summary>
        /// 存储路径
        /// </summary>
        public override string FilePath { get; set; }

        /// <summary>
        /// 次数,每个区域测几次
        /// </summary>
        public override int Number { get; set; } = 1;

        /// <summary>
        /// 记录次数
        /// </summary>
        public static int Count { get; set; } = 1;

        /// <summary>
        /// 模具区域
        /// </summary>
        public static int die { get; set; } = 1;

        /// <summary>
        /// 存放次数的容器，用于增加模具区域
        /// </summary>
        public List<int> NumberList = new List<int>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="design">模式</param>
        /// <param name="number">次数</param>
        public LogOnePoint(string filePath, int number)
        {
            this.FilePath = filePath;
            this.Number = number;
            if (File.Exists(FilePath) == false)
            {
                File.AppendAllText(this.FilePath, "die/区域\t\tdie次数\t\ta1_x\t\ta1_y\n");
            }
        }

        public void Write(double data1, double data2)
        {
            // 把die次数放到list中
            for (int i = 1; i <= Number; i++)
            {
                this.NumberList.Add(i);
            }

            if (LogOnePoint.Count > this.Number)
            {
                LogOnePoint.Count = 1;
            }

            File.AppendAllText(
                FilePath,
                die.ToString() + "\t\t" + this.NumberList[LogOnePoint.Count - 1].ToString() + "\t\t" + data1.ToString() + "\t\t"
                + data2.ToString() + "\n");

            // 如果一个区域达到测试次数，则记录下一个区域
            if (LogOnePoint.Count / this.Number == 1)
            {
                die++;
            }

            LogOnePoint.Count++;
        }
    }

    internal class LogBondXY : BaseLog
    {
        /// <summary>
        /// 存储路径
        /// </summary>
        public override string FilePath { get; set; }

        /// <summary>
        /// 次数,每个区域测几次
        /// </summary>
        public override int Number { get; set; } = 1;

        /// <summary>
        /// 记录次数
        /// </summary>
        public static int Count { get; set; } = 1;

        /// <summary>
        /// 模具区域
        /// </summary>
        public static int die { get; set; } = 1;

        /// <summary>
        /// 存放次数的容器，用于增加模具区域
        /// </summary>
        public List<int> NumberList = new List<int>();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="design">模式</param>
        /// <param name="number">次数</param>
        public LogBondXY(string filePath, int number)
        {
            this.FilePath = filePath;
            this.Number = number;
            if (File.Exists(FilePath) == false)
            {
                File.AppendAllText(this.FilePath, "die/区域\t速度\tdie次数\tA1_x\tA1_y\tB1_x\tB1_y\tA2_x\tB2_y\ta1_x\ta1_y\tb1_x\tb1_y\ta2_x\ta2_y\n");
            }
        }

        /// <summary>
        /// 把数据追加到文件中
        /// </summary>
        /// <param name="data1"></param>
        /// <param name="data2"></param>
        /// <param name="data3"></param>
        /// <param name="data4"></param>
        /// <param name="data5"></param>
        /// <param name="data6"></param>
        /// <param name="data7"></param>
        /// <param name="data8"></param>
        /// <param name="data9"></param>
        /// <param name="data10"></param>
        /// <param name="data11"></param>
        /// <param name="data12"></param>
        public void Write(double data1, double data2, double data3, double data4, double data5, double data6, double data7, double data8, double data9, double data10, double data11, double data12)
        {
            // 把die次数放到list中
            for (int i = 1; i <= Number; i++)
            {
                this.NumberList.Add(i);
            }

            if (LogBondXY.Count > this.Number)
            {
                LogBondXY.Count = 1;
            }

            File.AppendAllText(
                FilePath,
                die.ToString() + "\t\t" + this.NumberList[LogBondXY.Count - 1].ToString() + "\t" + data1.ToString() + "\t"
                + data2.ToString() + "\t" + data3.ToString() + "\t" + data4.ToString() + "\t" + data5.ToString() + "\t" + data6.ToString() + "\t"
                + data7.ToString() + "\t" + data8.ToString() + "\t" + data9.ToString() + "\t" + data10.ToString() + "\t" + data11.ToString() + "\t"
                + data12.ToString() + "\n");

            // 如果一个区域达到测试次数，则记录下一个区域
            if (LogBondXY.Count / this.Number == 1)
            {
                die++;
            }

            LogBondXY.Count++;
        }
    }
}
