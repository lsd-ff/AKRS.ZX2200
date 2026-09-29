using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using log4net.Core;

namespace AKRS.Galaxy2.PR.Models.TemplateConfigs
{
    /// <summary>
    /// PR模型 用于存储PR的设置的模板和参数等
    /// </summary>
    [Serializable]
    public class PRTemplateConfig : BaseTemplateConfig, IDisposable
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public PRTemplateConfig()
        {
        }

        /// <summary>
        /// 无参构造方法
        /// </summary>
        /// <param name="name">名称</param>
        public PRTemplateConfig(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 采图时刻 用于检查PR的模板是否有变动
        /// </summary>
        public DateTime GrabImageTime { get; set; }

        /// <summary>
        /// 离线原图
        /// </summary>
        [XmlIgnore]
        private Bitmap offLineBitmap = null;

        /// <summary>
        /// 离线原图
        /// </summary>
        [XmlIgnore]
        public Bitmap OffLineBitmap
        {
            get => this.offLineBitmap;
            set
            {
                this.offLineBitmap?.Dispose();
                this.offLineBitmap = value;
            }
        }

        /// <summary>
        /// 释放PR模型所有图片资源
        /// </summary>
        public void DisposeAllBitmap()
        {
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            this.DisposeAllBitmap();
        }

        /// <summary>
        /// 移除PR模板信息
        /// </summary>
        public void RemovePicture()
        {
   
        }

        /// <summary>
        /// 初始化PRAction里的配置
        /// </summary>
        public void InitConfig()
        {
           
        }
    }
}
