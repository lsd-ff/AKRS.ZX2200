using System;
using System.Windows.Forms;

namespace AKRS.Galaxy2.TeachTest
{
    using AKRS.ZX2200.TransportUnit.Controls.Assistant.CarrierTeach;
    using AKRS.ZX2200.TransportUnit.Controls.Assistant.ModuleTeach;
    using AKRS.ZX2200.TransportUnit.Controls.Assistant.SubstrateTeach;

    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmTUAdjustTeachTest());
        }
    }
}
