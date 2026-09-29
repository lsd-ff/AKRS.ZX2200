namespace AKRS.ZX2200.Infrastructure.Utils
{
    using System;
    using System.Collections.Generic;

    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.GlobalCalibration;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using ch.etel.edi.dsa.v40;

    using DevExpress.XtraEditors;

    /// <summary>
    /// ETel直接驱动底层的静态类
    /// </summary>
    public static class ETelDrive
    {
        /// <summary>
        /// 将补偿Mapping直接注入到ETel驱动器里面去
        /// </summary>
        /// <param name="axisX">X轴</param>
        /// <param name="axisY">Y轴</param>
        /// <param name="fileName">mapping图的路径</param>
        public static void DownloadMapping(Axis axisX, Axis axisY, string fileName)
        {
            try
            {
                // 获取ETel轴的两个驱动器对象
                DsaDrive moveDriveX = ((ETELAxis)axisX.AxisDrive).GetDrive();
                DsaDrive moveDriveY = ((ETELAxis)axisY.AxisDrive).GetDrive();

                DsaDrive[] dsaDrives = new DsaDrive[] { moveDriveX, moveDriveY };

                DsaDriveGroup grp = new DsaDriveGroup(dsaDrives);

                string path = PathConfig.DeviceDirPath + "\\" + fileName + ".txt";
                
                grp.stageMappingDownload(path);
            }
            catch (DsaException exc)
            {
                AKRSXtraMessageBox.Show("全局补偿下载到驱动器失败" + exc.Message);
            }
        }

        /// <summary>
        /// 将单轴的补偿注入到ETel驱动器里面去
        /// </summary>
        /// <param name="axis">X轴</param>
        /// <param name="fileName">mapping图的路径</param>
        public static void DownloadMapping(Axis axis, string fileName)
        {
            try
            {
                string path = PathConfig.DeviceDirPath + "\\" + fileName + ".txt";

                // 获取ETel轴的两个驱动器对象
                DsaDrive moveDrive = ((ETELAxis)axis.AxisDrive).GetDrive();

                // 下载到驱动器里面去
                moveDrive.scaleMappingDownload(path, Dsa.SCALE_MAPPING_MODE_ZERO_EDGE);
            }
            catch (DsaException exc)
            {
                AKRSXtraMessageBox.Show("全局补偿下载到驱动器失败" + exc.Message);
            }
        }

        /// <summary>
        /// 保存Mapping数据
        /// </summary>
        /// <param name="mappingDataX">X轴的数据</param>
        /// <param name="mappingDataY">Y轴的数据</param>
        /// <param name="fileName">文件名称</param>
        public static void SaveMappingData(List<double> mappingDataX, List<double> mappingDataY, string fileName)
        {
            #region 添加原有格式
            GlobalCalibrationDomain globalCalibrationDomain = GlobalCalibrationDomain.GetInstance();
            List<string> list = new List<string>()
                                    {
                                        "[Header]",
                                        "[[General info]]",
                                        "Data type,Stage error mapping",
                                        "Data format version,1.00A",
                                        "Software name,ComET Stage mapping tool",
                                        "Software version,2.10B",
                                        "Date,10/11/2009",
                                        "Time,13:01:48",
                                        "Operator,MMH",
                                        "Customer,My 2nd custormer",
                                        "Project,Zeus mapping",
                                        "Description,XY stage error mapping",
                                        "",
                                        "[[Controller info]]",
                                        "Controller name list,EA-P2M-300-07/15A-0000-01, EA-P2M-300-07/15A-0000-01",
                                        "Controller firmware list,3.21A,3.21A",
                                        "Controller address list,0,1",
                                        "Controller SN,219684730, 219684730",
                                        "Controller type list,0,0",
                                        "Controller status,0x20290000,0x20290000",
                                        "",
                                        "[[Stage error mapping configuration]]",
                                        "Mapping version,1.01A",
                                        "Machine type,Zeus",
                                        "Machine SN,12345",
                                        "Correction mode,positive",
                                        "Axis,X,0",
                                        "Axis,Y,1",
                                        "",
                                        "[[[Configuration 1]]]",
                                        "Corrected axis,X",
                                        "Correction dimensions,2",
                                        "Source axes,X,Y",
                                        "Source registers,ML17,ML17",
                                        "Correction table,table 1",
                                        $"Origin,X,{globalCalibrationDomain.StartPoint3D.X / 1000.0}",
                                        $"Origin,Y,{globalCalibrationDomain.StartPoint3D.Y / 1000.0}",
                                        "",
                                        "[[[Configuration 2]]]",
                                        "Corrected axis,Y",
                                        "Correction dimensions,2",
                                        "Source axes,X,Y",
                                        "Source registers,ML17,ML17",
                                        "Correction table,table 2",
                                        $"Origin,X,{globalCalibrationDomain.StartPoint3D.X / 1000.0}",
                                        $"Origin,Y,{globalCalibrationDomain.StartPoint3D.Y / 1000.0}",
                                        ""
                                    };

            #endregion

            list.Add("[Data]");
            list.Add("[[Info table 1]]");
            list.Add($"Step size,{globalCalibrationDomain.ColumnSpacing}.0e-3,{globalCalibrationDomain.RowSpacing}.0e-3");
            list.Add($"Table size,{globalCalibrationDomain.ColumnCount},{globalCalibrationDomain.RowCount}");
            list.Add("Data unit factor,-3");
            list.Add("");
            list.Add("[[Data table 1]]");
            string data = string.Empty;
            for (int i = 0; i < mappingDataX.Count; i++)
            {
                data += mappingDataX[i].ToString("0.0000");

                if ((i + 1) % globalCalibrationDomain.ColumnCount == 0)
                {
                    list.Add(data);
                    data = string.Empty;
                }
                else
                {
                    data += ",";
                }
            }

            list.Add("");

            list.Add("[[Info table 2]]");
            list.Add($"Step size,{globalCalibrationDomain.ColumnSpacing}.0e-3,{globalCalibrationDomain.RowSpacing}.0e-3");
            list.Add($"Table size,{globalCalibrationDomain.ColumnCount},{globalCalibrationDomain.RowCount}");
            list.Add("Data unit factor,-3");

            list.Add("");
            list.Add("[[Data table 2]]");
            string data2 = string.Empty;
            for (int i = 0; i < mappingDataY.Count; i++)
            {
                data2 += mappingDataY[i].ToString("0.0000");

                if ((i + 1) % globalCalibrationDomain.ColumnCount == 0)
                {
                    list.Add(data2);
                    data2 = string.Empty;
                }
                else
                {
                    data2 += ",";
                }
            }

            list.Add("");
            list.Add("[End Of File]");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                       PathConfig.DeviceDirPath + "//" + fileName + ".txt",
                       false))
            {
                foreach (string line in list)
                {
                    file.WriteLine(line);
                }
            }
        }

        /// <summary>
        /// 保存Mapping数据(单轴)
        /// </summary>
        /// <param name="axisName">轴名称</param>
        /// <param name="axisIndex">轴索引</param>
        /// <param name="start">开始点位</param>
        /// <param name="mappingData">数据的集合X</param>
        /// <param name="fileName">名称</param>
        public static void SaveMappingDataX(string axisName, int axisIndex,double start, List<double> mappingData, string fileName)
        {
            try
            {
                #region 添加Etel要求的格式

                List<string> list = new List<string>()
                                    {
                                        "[Header]",
                                        "[[General info]]",
                                        "Data type,Scale error mapping",
                                        "Data format version,1.00A",
                                        "Software name,ComET Scale mapping tool",
                                        "Software version,4.52B",
                                        "Date,10/11/2009",
                                        "Time,13:01:48",
                                        "Operator,MMH",
                                        "Customer,My 2nd custormer",
                                        "Project,Zeus mapping",
                                        $"Description,{axisName} scale error mapping",
                                        "",
                                        "[[Controller info]]",
                                        "Controller name list,EA-P2M-300-07/15A-0000-01, EA-P2M-300-07/15A-0000-01",
                                        "Controller firmware list,3.52A,3.52A",
                                        "Controller address list,0,1",
                                        "Controller SN, 219684730, 219684730",
                                        "Controller status,0x20290000,0x20290000",
                                        "",
                                        "[[Stage error mapping configuration]]",
                                        "Mapping version,1.01A",
                                        "Machine type,Zeus",
                                        "Machine SN,12345",
                                        "Correction mode,positive",
                                        $"Axis,{axisName},{axisIndex}",
                                        "",
                                        "[[[Configuration 1]]]",
                                        $"Corrected axis,{axisName}",
                                        $"Source axes,{axisName}",
                                        "Source registers,ML19",
                                        $"Source cyclic stroke,{axisName},1.0",
                                        "Correction table,table 1",
                                        $"Origin,{axisName},{start / 1000.0}",
                                    };

                #endregion

                list.Add("[Data]");
                list.Add("[[Info table 1]]");
                list.Add("Step size,3.0e-3");
                list.Add($"Table size,{mappingData.Count}");
                list.Add("Data unit factor,-3");
                list.Add("[[Data table 1]]");
                string data = string.Empty;
                for (int i = 0; i < mappingData.Count; i++)
                {
                    data += mappingData[i].ToString("0.0000");
                    if (i != mappingData.Count - 1)
                    {
                        data += ",";
                    }
                }

                list.Add(data);

                list.Add("[End Of File]");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                           PathConfig.DeviceDirPath + "\\" + fileName + ".txt",
                           false))
                {
                    foreach (string line in list)
                    {
                        file.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"{axisName} 单轴补偿失败" + e.ToString());
            }
        }


        /// <summary>
        /// 开启2维补偿
        /// </summary>
        /// <param name="axisX">X轴</param>
        /// <param name="axisY">Y轴</param>
        /// <param name="fileName">文件名称</param>
        public static void Open2DCompensate(Axis axisX, Axis axisY, string fileName)
        {
            // 下载补偿文件
            ETelDrive.DownloadMapping(axisX, axisY, GlobalCalibrationDomain.GetInstance().TextName);

            GlobalCalibrationDomain.GetInstance().Save();

            DsaDrive moveDriveX = ((ETELAxis)axisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)axisY.AxisDrive).GetDrive();

            DsaDrive[] dsaDrives = new DsaDrive[] { moveDriveX, moveDriveY };

            DsaDriveGroup grp = new DsaDriveGroup(dsaDrives);

            grp.stageMappingActivate();
        }

        /// <summary>
        /// 关闭2维补偿
        /// </summary>
        /// <param name="axisX">轴X</param>
        /// <param name="axisY">轴Y</param>
        public static void Close2DCompensate(Axis axisX, Axis axisY)
        {
            DsaDrive moveDriveX = ((ETELAxis)axisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)axisY.AxisDrive).GetDrive();

            DsaDrive[] dsaDrives = new DsaDrive[] { moveDriveX, moveDriveY };

            DsaDriveGroup grp = new DsaDriveGroup(dsaDrives);

            moveDriveX.scaleMappingDeactivate();
            moveDriveY.scaleMappingDeactivate();

            if (grp.stageMappingIsActivated())
            {
                grp.stageMappingDeactivate();
            }
        }
    }
}
