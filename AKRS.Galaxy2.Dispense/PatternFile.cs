using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using AKRS.Base;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// XML内部格式参考patternFile.xml文件。
    /// </remarks>
    public static class PatternFile
    {
        public const string PATTERN_FILE_EXTENSION = ".ep";

        public static string ConstructFileNameByPatternName(string strPatternName)
        {
            return strPatternName + PATTERN_FILE_EXTENSION;
        }

        public static void ClearTmpFiles(string strPatternRepositoryPath)
        {
            if (strPatternRepositoryPath == null)
                throw new ArgumentNullException(nameof(strPatternRepositoryPath));

            Directory.CreateDirectory(strPatternRepositoryPath);

            string[] strTmpFilePathes = Directory.GetFiles(strPatternRepositoryPath, "tmp_*.bmp");
            foreach (string strTmpFilePath in strTmpFilePathes)
            {
                try
                {
                    File.Delete(strTmpFilePath);
                }
                catch
                { }
            }
        }

        public static void SaveFile(string strFilePath, EPEData epeData)
        {
            if (epeData.BackgroundImageInfo.Image != null)
            {
                string strPatternName = Path.GetFileNameWithoutExtension(strFilePath);
                string strImageFileName = strPatternName + ".bmp";

                string strDirPath = Path.GetDirectoryName(strFilePath);
                string strImageFilePath = Path.Combine(strDirPath, strImageFileName);
                             
                epeData.BackgroundImageInfo.Image.Save(strImageFilePath);
            }
            
            using (var sxml = new SXml())
            {
                sxml.SetRootTagName("EPEData");

                sxml.SetAttrDouble("MicronPerPixel", "value", epeData.MicronPerPixel);

                List<SXmlTagAttribute> attrList = new List<SXmlTagAttribute>();

                attrList.Add(new SXmlTagAttribute("width", epeData.DieWidth));
                attrList.Add(new SXmlTagAttribute("height", epeData.DieHeight));
                sxml.SetAttributes("DieSize", attrList.ToArray());

                attrList.Clear();
                attrList.Add(new SXmlTagAttribute("m11", epeData.BackgroundImageInfo.MatrixElements[0]));
                attrList.Add(new SXmlTagAttribute("m12", epeData.BackgroundImageInfo.MatrixElements[1]));
                attrList.Add(new SXmlTagAttribute("m21", epeData.BackgroundImageInfo.MatrixElements[2]));
                attrList.Add(new SXmlTagAttribute("m22", epeData.BackgroundImageInfo.MatrixElements[3]));
                attrList.Add(new SXmlTagAttribute("dx", epeData.BackgroundImageInfo.MatrixElements[4]));
                attrList.Add(new SXmlTagAttribute("dy", epeData.BackgroundImageInfo.MatrixElements[5]));
                sxml.SetAttributes("Image", attrList.ToArray());

                attrList.Clear();
                AddInfo(attrList, epeData.DefaultInfo);
                sxml.SetAttributes("DefaultInfo", attrList.ToArray());

                int iCurveIndex = 0;
                foreach (EPEPointData[] epePoints in epeData.EpePointList)
                {
                    string strCurveTagPath = String.Format($"Curves.Curve[{iCurveIndex}]");

                    int iSegmentIndex = 0;
                    foreach (EPEPointData epePoint in epePoints)
                    {
                        string strSegmentTagPath = String.Format($"{strCurveTagPath}.Segment[{iSegmentIndex}]");

                        attrList.Clear();
                        attrList.Add(new SXmlTagAttribute("x", epePoint.Point.X));
                        attrList.Add(new SXmlTagAttribute("y", epePoint.Point.Y));
                        AddInfo(attrList, epePoint.Info);
                        sxml.SetAttributes(strSegmentTagPath, attrList.ToArray());

                        iSegmentIndex++;
                    }

                    iCurveIndex++;
                }

                sxml.SaveAs(strFilePath);
            }
        }

        public static EPEData ReadFile(string strFilePath)
        {
            EPEData epeData = null;

            try
            {
                Bitmap image;
                {
                    string strPatternName = Path.GetFileNameWithoutExtension(strFilePath);
                    string strImageFileName = strPatternName + ".bmp";

                    string strDirPath = Path.GetDirectoryName(strFilePath);
                    string strImageFilePath = Path.Combine(strDirPath, strImageFileName);

                    string strTmpImageFileName = "tmp_" + DateTime.Now.Ticks.ToString() + ".bmp";
                    string strTmpImageFilePath = Path.Combine(strDirPath, strTmpImageFileName);
                    File.Copy(strImageFilePath, strTmpImageFilePath, true);
                    image = new Bitmap(strTmpImageFilePath);
                }

                using (SXml sxml = new SXml(strFilePath, Encoding.UTF8))
                {
                    float micronPerPixel = (float)sxml.GetAttrDouble("MicronPerPixel", "value");

                    var attributes = sxml.GetAttributesEx("DieSize");
                    float dieWidth = float.Parse(attributes.GetAttribute("width"));
                    float dieHeight = float.Parse(attributes.GetAttribute("height"));

                    attributes = sxml.GetAttributesEx("Image");
                    float m11 = float.Parse(attributes.GetAttribute("m11"));
                    float m12 = float.Parse(attributes.GetAttribute("m12"));
                    float m21 = float.Parse(attributes.GetAttribute("m21"));
                    float m22 = float.Parse(attributes.GetAttribute("m22"));
                    float dx = float.Parse(attributes.GetAttribute("dx"));
                    float dy = float.Parse(attributes.GetAttribute("dy"));

                    attributes = sxml.GetAttributesEx("DefaultInfo");
                    EndpointSetting defaultInfo = RetrieveInfo(attributes);

                    List<EPEPointData[]> list;
                    if (sxml.DoesTagExist("Curves"))
                    {
                        int iCurveCount = sxml.GetSpecifiedChildTagCount("Curves", "Curve");
                        list = new List<EPEPointData[]>(iCurveCount);

                        for (int iCurveIndex = 0; iCurveIndex < iCurveCount; iCurveIndex++)
                        {
                            string strCurveTagPath = String.Format($"Curves.Curve[{iCurveIndex}]");

                            int iSegmentCount = sxml.GetSpecifiedChildTagCount(strCurveTagPath, "Segment");
                            EPEPointData[] epePoints = new EPEPointData[iSegmentCount];
                            for (int iSegmentIndex = 0; iSegmentIndex < iSegmentCount; iSegmentIndex++)
                            {
                                string strSegmentTagPath = String.Format($"{strCurveTagPath}.Segment[{iSegmentIndex}]");
                                attributes = sxml.GetAttributesEx(strSegmentTagPath);

                                // 240703改，数据转为int会报错
                                // int x = (int)attributes.GetAttributeInt64("x");
                                // int y = (int)attributes.GetAttributeInt64("y");

                                string stringX = attributes.GetAttribute("x");
                                string stringY = attributes.GetAttribute("y");
                                float x = float.Parse(stringX);
                                float y = float.Parse(stringY);
                                EndpointSetting info = RetrieveInfo(attributes);

                                epePoints[iSegmentIndex] = new EPEPointData(x, y, info);
                            }

                            list.Add(epePoints);
                        }
                    }
                    else
                    {
                        list = new List<EPEPointData[]>();
                    }

                    //epeData = new EPEData(micronPerPixel, dieWidth, dieHeight, list, image, imageOffsetX, imageOffsetY, imageRotationDegree);
                    epeData = new EPEData(micronPerPixel, dieWidth, dieHeight, list, image, new float[] { m11, m12, m21, m22, dx, dy });
                    epeData.SetDefaultValues(defaultInfo);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                epeData = null;
            }

            return epeData;
        }

        private static void AddInfo(List<SXmlTagAttribute> attrList, EndpointSetting info)
        {
            Debug.Assert(attrList != null);

            attrList.Add(new SXmlTagAttribute("AltitudeCompensation", info.AltitudeCompensation));
            attrList.Add(new SXmlTagAttribute("MaxSpeed", info.MaxSpeed));
            attrList.Add(new SXmlTagAttribute("endSpeed", info.EndSpeed));
            attrList.Add(new SXmlTagAttribute("Acc", info.Acc));
            attrList.Add(new SXmlTagAttribute("Time", info.Time));
            attrList.Add(new SXmlTagAttribute("dummied", info.Dummied));
            attrList.Add(new SXmlTagAttribute("preDelay", info.PreDelay));
            attrList.Add(new SXmlTagAttribute("postDelay", info.PostDelay));
        }

        private static EndpointSetting RetrieveInfo(SxmlTagAttributes attributes)
        {
            int iAltitudeCompensation = attributes.GetAttributeInt("AltitudeCompensation");
            int iMaxSpeed = attributes.GetAttributeInt("MaxSpeed");
            int iEndSpeed = attributes.GetAttributeInt("endSpeed");
            double acc = Double.Parse(attributes.GetAttribute("Acc"));
            int iTime = attributes.GetAttributeInt("Time");
            bool dummied = attributes.GetAttributeBoolean("dummied");
            int iPreDelay = attributes.GetAttributeInt("preDelay");
            int iPostDelay = attributes.GetAttributeInt("postDelay");
            return new EndpointSetting(iMaxSpeed, iEndSpeed, acc, iAltitudeCompensation, iTime, dummied, iPreDelay, iPostDelay);
        }
    }
}
