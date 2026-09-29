using AKRS.Galaxy2.Component.Simple.MessageBox;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Controls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using DevExpress.Tutorials.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.VisionControl
{
    /*   版本更新规则
        *   版本号必须与AssemblyVersion保持一致
        *   XX.XX.XX.XX 分别对应 主版本号、次版本号、内部版本号和修订号。前面数字提升时后面数字归零
        *   当遇到软件更新内容无法兼顾以前版本时或者有重大功能增加、修改时，提升主版本号 
        *   次版本号为每周或者月固定更新一次，只涉及BUG修复和简单的功能增加
        *   内部版本号用于更改处理器、平台或编译器的情况，此项目不需考虑
        *   修订号用于功能急需上线和bug急需修复时，提升修订号。
        *   功能更新时在VersionInfos中添加对应的更改记录
        *  */

    /// <summary>
    /// 版本更新公告
    /// </summary>
    public static class VersionUpdateAnnouncement
    {
        /// <summary>
        /// 检查版本更新
        /// </summary>
        public static void CheckVersionUpdate()
        {
            // 当前设备版本
            Version machineVersion = new Version(MachineSoftwareConfiguration.GetInstance().CurrentVersionNumber);

            Version currentVersion = VersionInfos.Max(v => v.VersionNumber);

            // 获取软件最新版本号
            if (currentVersion != Assembly.GetExecutingAssembly().GetName().Version)
            {
                AKRSXtraMessageBox.Show("当前版本号与内置版本号不相同，请联系软件人员");
                return;
            }
            

            // 需要更新的版本集合
            List<VersionInfo> updates = VersionInfos.Where(v => v.VersionNumber > machineVersion).ToList();
            if (updates.Count > 0)
            {
                // 当前设备更新到最新版本
                MachineSoftwareConfiguration.GetInstance().LastVersionNumber = MachineSoftwareConfiguration.GetInstance().CurrentVersionNumber;
                MachineSoftwareConfiguration.GetInstance().CurrentVersionNumber = VersionInfos.Max(v => v.VersionNumber).ToString();
                MachineSoftwareConfiguration.GetInstance().LastVersionUpdataTime = DateTime.Now;
                MachineSoftwareConfiguration.GetInstance().Save();
                FrmVersionInfo frmVersionInfo = new FrmVersionInfo();
                frmVersionInfo.ShowDialog();
            }
        }

        /// <summary>
        /// 版本信息公告
        /// </summary>
        public static List<VersionInfo> VersionInfos = new List<VersionInfo>()
        {
        new VersionInfo()
            {
                VersionNumber = new Version("1.0.0.0"),
                UpdataTime = new DateTime(2026, 1, 1),
                UpdateContent = new List<string>()
                {
                    "1.新增软件版本控制模块",
                }
            },

        new VersionInfo()
            {
                VersionNumber = new Version("1.1.0.0"),
                UpdataTime = new DateTime(2026, 3, 24),
                UpdateContent = new List<string>()
                                    {
                                        "1.新增Bond避让静态华夫盒位置" ,
                                        "2.翻转吸嘴交接位置改为示教时的位置"
                                    }
            },
         new VersionInfo()
            {
                VersionNumber = new Version("1.2.0.0"),
                UpdataTime = new DateTime(2026, 4, 8),
                UpdateContent = new List<string>()
                                    {
                                        "1.新增Bond蘸胶" ,
                                        "2.修改统计界面查询功能",
                                        "3.软件启动和关闭时的等待画面",
                                        "4.其他优化"
                                    }
            },
         new VersionInfo()
             {
                 VersionNumber = new Version("1.3.0.0"),
                 UpdataTime = new DateTime(2026, 5, 28),
                 UpdateContent = new List<string>()
                                     {
                                         "1.修复点胶激光测高时界面卡死的问题" ,
                                         "2.增加用Bond相机搜晶功能，使用此功能要重新示教ULM运动点位",
                                     }
             },
         new VersionInfo()
             {
                 VersionNumber = new Version("1.4.0.0"),
                 UpdataTime = new DateTime(2026, 8, 12),
                 UpdateContent = new List<string>()
                                     {
                                         "1.更新底层库（Drive、LogicHardware、Infrastructure.dll）" ,
                                         "2.兼容左右两个中转台（如果设备安装了右中转台需在参数界面-机器-机器硬件配置中将右中转台配置为启用，并重新示教右中转台和ULM运动点位！）",
                                         "3.机器硬件配置增加Magazine是否配置选项，不配置则屏蔽晶圆夹和料盒模组"
                                     }
             },

         new VersionInfo()
             {
                 VersionNumber = new Version("1.5.0.0"),
                 UpdataTime = new DateTime(2026, 8, 24),
                 UpdateContent = new List<string>()
                                     {
                                         "1.PR项目Bug修复",
                                         "2.修复芯片编辑界面Timer无法释放的问题",
                                         "3.增加激光干涉尺预热功能，放在硬件初始化之前"
                                     }
             },
        };
    }
}
