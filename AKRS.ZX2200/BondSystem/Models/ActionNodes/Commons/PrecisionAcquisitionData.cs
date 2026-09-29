using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.Utils;
using DevExpress.DashboardCommon.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons
{
    public class PrecisionAcquisitionDataContainer : SingletonNoSave<PrecisionAcquisitionDataContainer>
    {
        public List<PrecisionAcquisitionData> DataList { get; set; } = new List<PrecisionAcquisitionData>();

        public PrecisionAcquisitionData Data { get; set; } = new PrecisionAcquisitionData();

        public void CreateDataObject() 
        {
            Data = new PrecisionAcquisitionData();
        }

        private int dataCount = 0;
        
        public void AddData()
        {
            dataCount++;

            DataList.Add(Data);
            this.CreateDataObject();
        }

        /// <summary>
        /// 保存到Excel
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveData(string fileName) 
        {
            // 100 行保存一次
            if (dataCount % 1000 == 0) 
            {
                // 保存
                FileHelper.Export2(DataList, fileName);
            }
        }
    }

    /// <summary>
    /// 精度分析的过程数据采集
    /// </summary>
    public class PrecisionAcquisitionData
    {
        public string DTime { get; set; } = DateTime.Now.ToString("dd HH:mm:ss");

        public int ColumnIndex { get; set; }

        
        public int RowIndex { get; set; }

        /**************************SubstrateCamerActionNode*************************/

        /// <summary>
        /// 放到中转台前芯片的角度 可能是焊后看出来的角度  温漂1 的激光干涉尺读数
        /// </summary>
        public double 放中转台前芯片的角度 { get; set; }

        /// <summary>
        /// 旋转带来的偏移量  温漂1 的定位结果
        /// </summary>
        public AKRSPoint2D 放中转台前的旋转偏移量 { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 放中转台位置  -- 温漂2 的定位结果
        /// </summary>
        public AKRSPoint4D 放中转台的位置 { get; set; } = new AKRSPoint4D();

        public double 移动到中转台后的激光干涉尺读数 { get; set; }


        public AKRSPoint3D 中转台拍照位置 { get; set; } = new AKRSPoint3D();

        public AKRSPoint4D 移动到中转台拍照位后的编码器读数 { get; set; }

        public double 移动到中转台拍照位后的干涉尺读数 { get; set; }

        public AKRSPoint4D 中转台定位结果 { get; set; } = new AKRSPoint4D();

        public AKRSPoint2D 取片焊头旋转后带来的偏移量 { get; set; } = new AKRSPoint2D();

        public AKRSPoint3D 实际取片位置 { get; set; } = new AKRSPoint3D();

        public AKRSPoint3D 中转台芯片偏移 { get; set; } = new AKRSPoint3D();

        public AKRSPoint4D 运动到的预取晶位置 { get; set; } = new AKRSPoint4D();

        public double 运动到预取晶位置的干涉尺读数 { get; set; }





        public AKRSPoint4D 固晶位置 { get; set; } = new AKRSPoint4D();
        public AKRSPoint4D 固晶编码器位置 { get; set; } = new AKRSPoint4D();
        public double 移动到固晶位置后的干涉尺读数 { get; set; }


        public AKRSPoint3D 焊后拍照位置G0 { get; set; } = new AKRSPoint3D();
        public double 移动到焊后位置后的干涉尺读数 { get; set; }

        public AKRSPoint4D 焊后定位拍照结果 { get; set; } = new AKRSPoint4D();
        public AKRSPoint4D 焊后定位结果 { get; set; } = new AKRSPoint4D();
    }
}
