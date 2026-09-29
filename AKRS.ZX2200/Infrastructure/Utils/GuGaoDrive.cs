using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.CalibSystem.GlobalCalibration;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GTN.mc;

namespace AKRS.ZX2200.Infrastructure.Utils
{
    using AKRS.Base;
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.Exceptions;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.Data.Linq.Helpers;
    using DevExpress.Utils.Extensions;
    using LanguageExt.ClassInstances.Pred;
    using log4net.Core;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// 固高驱动
    /// </summary>
    public static class GuGaoDrive
    {
        /// <summary>
        /// 连续插补
        /// </summary>
        /// <param name="interpolationParam">插补参数</param>

        public static void ContinueInterpolationMove(InterpolationParam interpolationParam)

        {
            short rtn = 0;

            // 获取核号

            //short coreNo = (short)this.CardNum;
            short coreNo = 2;
            short listNo = interpolationParam.ListNo;  //1;  // 指令流号
            short groupNo = (short)interpolationParam.GrpCrd; //1; // 组号
            GTN.mc.TListInfo listInfoNull = new GTN.mc.TListInfo();

            //listInfoNull.reserve1 = new short[2];

            //listInfoNull.reserve2 = new short[3];

            //listInfoNull.reserve3 = new double[4];



            // 指令流结构

            GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();



            // listInfo初始化

            listInfo.list = listNo;

            listInfo.modal = 0;

            listInfo.segNum = 0;

            //listInfo.reserve1 = new short[2];

            //listInfo.reserve2 = new short[3];

            //listInfo.reserve3 = new double[4];



            try

            {

                // 轴数             

                int axisCount = interpolationParam.AxisDrives.Length; //2;

                int[] axisNoArr = interpolationParam.AxisDrives.Select(a => a.AxisNum).ToArray();



                // short externalAxisCount = 0;



                // 声明结构体数组长度 脉冲当量 保存每个轴的脉冲当量

                GTN.mc.TProfileScale[] scale = new GTN.mc.TProfileScale[24];



                // 读取脉冲当量

                // GTN.mc.TProfileScale[] scaleRead = new GTN.mc.TProfileScale[24];

                // double[] gearRatio = new double[24];

                // double velMax, accMax, jerkMax;



                // 1、基本轴初始化

                // velMax = 2000;

                // accMax = 10000;

                // jerkMax = 1000000;



                for (int i = 0; i < axisCount; i++)

                {

                    scale[i].alpha = new double[4];

                    scale[i].beta = new double[4];

                    scale[i].reverse1 = new short[3];



                    // 如果这个个数设置成1，那后面的数组只要设置第一组就可以，设置成2，就设置两组。  有减速比的情况下 可能需要设置2组

                    scale[i].count = 1;   // 固定

                    scale[i].alpha[0] = 1;                 // 脉冲当量，alpha可以认为是mm的单位，beta是脉冲的单位。beta / alpha 默认值设置1 就可以了

                    scale[i].beta[0] = interpolationParam.AxisDrives[i].PulseEquivalent;    //10000;              // 设置规划器的脉冲当量 10000个脉冲1mm



                    //scale[i].alpha[1] = 1;

                    //scale[i].beta[1] = 1;



                    // 设置规划器的脉冲当量，毫米 对应的 脉冲数     卡号， 规划器号

                    rtn = GTN.mc.GTN_SetAxisScale(coreNo, (short)(i + 1), ref scale[i], ref listInfoNull);



                    //轴运动约束参数的结构体

                    GTN.mc.TAxisMotionConstraint[] axisMotionConstraint = new GTN.mc.TAxisMotionConstraint[24];

                    //rtn = GTN.mc.GTN_GetAxisScale(coreNo, Convert.ToInt16(i + 1), out scaleRead[i]);



                    //memset(&axisMotionConstraint[i], 0, sizeof(axisMotionConstraint[i]));

                    //设置轴运动约束参数，规划器运动的最大参数，GT_运动指令中的运动参数大于这些参数数，约束到该最大值

                axisMotionConstraint[i].reserve1 = new short[3];

                    axisMotionConstraint[i].reserve2 = new double[8];



                    //axisMotionConstraint[i].velMax = velMax;        // 单位：mm/s   或者 度/s

                    //axisMotionConstraint[i].accMax = accMax;        // 单位：mm/s^2 或者 度/s^2

                    //axisMotionConstraint[i].decMax = accMax;        // 单位：mm/s^2 或者 度/s^2

                    //axisMotionConstraint[i].jerkMax = jerkMax;      // 单位：mm/s^3 或者 度/s^3

                    //axisMotionConstraint[i].dvMax = 10;             // 单位：mm/s   或者 度/s   轴的最大速度跳变量



                    axisMotionConstraint[i].velMax = 500;        // 单位：mm/s   或者 度/s

                    axisMotionConstraint[i].accMax = 5000;        // 单位：mm/s^2 或者 度/s^2

                    axisMotionConstraint[i].decMax = 5000;        // 单位：mm/s^2 或者 度/s^2

                    axisMotionConstraint[i].jerkMax = 5000;      // 单位：mm/s^3 或者 度/s^3

                    axisMotionConstraint[i].dvMax = 10;             // 单位：mm/s   或者 度/s   轴的最大速度跳变量



                    rtn = GTN.mc.GTN_SetAxisMotionConstraint(coreNo, Convert.ToInt16(i + 1), ref axisMotionConstraint[i], ref listInfoNull);



                    rtn = GTN.mc.GTN_GetAxisMotionConstraint(coreNo, Convert.ToInt16(i + 1), out axisMotionConstraint[i]);



                }



                /*  看起来好像没什么用

                // 2、List初始化

                GTN.mc.TCommandListConfig commandListConfig = new GTN.mc.TCommandListConfig();



                // 静态模式只支持指令流空间大小的数据(4096段)，控制器不会自动清除指令流数据，只有调用GTN_ClearCommandListData时才会清空数据，

                // 动态模式下指令流数据执行后可以继续发送新的数据，保证指令流可以动态执行起来。

                commandListConfig.mode = GTN.mc.COMMAND_LIST_MODE_DYNAMIC;

                commandListConfig.elementSize = 128 * sizeof(double);               // 指令流里的每条指令byte个数最大值，

                commandListConfig.forwardSpace = 1000;                              // 指令流允许预存的最大段数

                commandListConfig.reverseSpace = 0;                                 // 指令流允许回退的最大段数



                // rtn = GTN.mc.GTN_SetCommandListConfig(coreNo, listTemp, ref commandListConfig);

                rtn = GTN.mc.GTN_GetCommandListConfig(coreNo, listNo, out commandListConfig);

                */



                // 将Group与List关联。按未关联，最多2个Group关联到同一个List中

                uint linkGroupMask;

                rtn += GTN.mc.GTN_GetCommandListLinkGroup(coreNo, listNo, out linkGroupMask);



                linkGroupMask |= Convert.ToUInt32(1 << (groupNo - 1));



                rtn += GTN.mc.GTN_SetCommandListLinkGroup(coreNo, listNo, linkGroupMask);



                //// 3、Group初始化

                rtn += GTN.mc.GTN_UngroupAllAxes(coreNo, groupNo, ref listInfoNull);

                rtn += GTN.mc.GTN_GroupDisable(coreNo, groupNo, ref listInfoNull);





                // 将轴号添加到Group中去

                //short AXIS_X, AXIS_Y;

                //AXIS_X = Convert.ToInt16(param.AxisX.AxisNum);

                //AXIS_Y = Convert.ToInt16(param.AxisY.AxisNum);



                //把轴添加到组中

                for (int i = 0; i < axisCount; i++)

                {

                    rtn += GTN.mc.GTN_AddAxisToGroup(coreNo, groupNo, (short)axisNoArr[i], (short)(i + 1), ref listInfoNull);

                    // rtn = GTN.mc.GTN_AddAxisToGroup(coreNo, groupNo, (short)axisNoArr[1], 2, ref listInfoNull);

                }



                // 设置运动学参数



                //GTN.mc.TKinematicTransformMultiAxis kinematicTransformMultiAxis = new GTN.mc.TKinematicTransformMultiAxis();

                //kinematicTransformMultiAxis.reserve = new short[3];

                //kinematicTransformMultiAxis.multiAxis.reserve1 = new short[3];

                //kinematicTransformMultiAxis.multiAxis.reserve2 = new Int32[10];

                //kinematicTransformMultiAxis.multiAxis.reserve3 = new double[42];



                //// memset(&kinematicTransform,0,sizeof(kinematicTransform));

                //// 标准正交三轴结构

                //kinematicTransformMultiAxis.type = GTN.mc.KIN_TYPE_ORTHOGONAL;

                //rtn = GTN.mc.GTN_SetGroupKinematicTransform(coreNo, groupTemp, ref kinematicTransformMultiAxis, ref listInfoNull);



                /*

                //设置group的笛卡尔坐标系运动约束参数。插补规划器的运动约束

                GTN.mc.TGroupMotionConstraint groupMotionConstraint = new GTN.mc.TGroupMotionConstraint();

                groupMotionConstraint.reserve = new double[10];

                //memset(&groupMotionConstraint,0,sizeof(groupMotionConstraint));

                rtn = GTN.mc.GTN_GetGroupMotionConstraint(coreNo, groupTemp, out groupMotionConstraint);

                groupMotionConstraint.velMax = velMax;          // 单位：mm/s   或者 度/s

                groupMotionConstraint.accMax = accMax;          // 单位：mm/s^2 或者 度/s^2

                groupMotionConstraint.decMax = accMax;          // 单位：mm/s^2 或者 度/s^2

                groupMotionConstraint.jerkMax = jerkMax;        // 单位：mm/s^3 或者 度/s^3

                rtn = GTN.mc.GTN_SetGroupMotionConstraint(coreNo, groupTemp, ref groupMotionConstraint, ref listInfoNull);

                */



                // 设置group的速度规划模式。目前只支持平滑模式PROFILE_MODE_SMOOTH

                GTN.mc.TVelProfileSmoothMode velProfileModeSmooth = new GTN.mc.TVelProfileSmoothMode();

                //memset(&velProfileMode, 0, sizeof(velProfileMode));

                velProfileModeSmooth.mode = GTN.mc.VEL_PROFILE_MODE_SMOOTH;

                //velProfileModeSmooth.reserve1 = new short[3];

                //velProfileModeSmooth.smooth.reserve = new double[18];



                velProfileModeSmooth.smooth.accTime = interpolationParam.AccAcc; //50;       // 加速度变化时间，类似Trap模式的smoothTime 当 accTime 为 0 时，速度规划曲线为 T 型曲线

                velProfileModeSmooth.smooth.k = 0;              // 形态系数,取值范围：[0,1000], 暂时不清楚是什么意思

                rtn += GTN.mc.GTN_SetGroupVelProfileMode(coreNo, groupNo, ref velProfileModeSmooth, ref listInfoNull);



                //rtn = GTN.mc.GTN_GetGroupVelProfileMode(coreNo, groupTemp, out velProfileModeSmooth);



                // 设置Group的停止参数

                short decelerationSmoothStop = 5; // 组的平滑停止减速度，取值范围：(0,...)，单位：mm/s^2。

                short jerkSmoothStop = 5;         // 停止减减速度，取值范围：(0,...)，单位：mm/s^3。

                short stopType = 0;                       // type：0：平滑停止	1：急停		2：异常停止

                //stopPrm.mode = 0;					// 0：停止；1：停止到段末

                //stopPrm.haltMode = 0;				// 0：暂停过程中不允许再次启动，1：暂停过程中允许再次启动



                GTN.mc.TGroupStopParameter stopPrm = new GTN.mc.TGroupStopParameter();

                //stopPrm.reserve1 = new short[4];

                //stopPrm.reserve2 = new double[5];

                stopPrm.deceleration = decelerationSmoothStop;      // 组的平滑停止减速度，取值范围：(0,...)，单位：mm/s^2。

                stopPrm.jerk = jerkSmoothStop;                      // 停止减减速度，取值范围：(0,...)，单位：mm/s^3。



                rtn += GTN.mc.GTN_SetGroupStopParameter(coreNo, groupNo, stopType, ref stopPrm, ref listInfoNull);

                // rtn = GTN.mc.GTN_GetGroupStopParameter(coreNo, groupTemp, stopType, out stopPrm);



                /*

                short decelerationQuickStop, jerkQuickStop;

                decelerationQuickStop = 5;

                jerkQuickStop = 5;

                GTN.mc.TGroupStopParameter abruptStopPrm = new GTN.mc.TGroupStopParameter();

                abruptStopPrm.reserve1 = new short[4];

                abruptStopPrm.reserve2 = new double[5];

                // memset(&abruptStopPrm, 0, sizeof(abruptStopPrm));

                stopType = 1;                           // type：0：平滑停止	1：急停		 2：异常停止

                                                        //abruptStopPrm.mode = 0;	    // 0：停止；1：停止到段末

                                                        // abruptStopPrm.haltMode = 0;	// 0：暂停过程中不允许再次启动，1：暂停过程中允许再次启动

                abruptStopPrm.deceleration = decelerationQuickStop; // 停止加速度，取值范围：(0,...)，单位：mm/s^2。

                abruptStopPrm.jerk = jerkQuickStop;// 停止加加速度，取值范围：(0,...)，单位：mm/s^3。。

                rtn = GTN.mc.GTN_SetGroupStopParameter(coreNo, groupTemp, stopType, ref abruptStopPrm, ref listInfoNull);

                rtn = GTN.mc.GTN_GetGroupStopParameter(coreNo, groupTemp, stopType, out abruptStopPrm);

                */



                //// 设置group运动最大速度倍率和加速度倍率

                //double maxOverride = Convert.ToDouble(textBox_groupMaxOverride.Text);

                //short overRideType;

                //overRideType = GTN.mc.OVERRIDE_TYPE_VEL;            // OVERRIDE_TYPE_VEL 速度倍率;OVERRIDE_TYPE_ACC 加速度倍率，此类型为保留值，暂不支持

                //rtn = GTN.mc.GTN_SetGroupMaxOverride(coreNo, groupTemp, overRideType, maxOverride, ref listInfoNull);

                //rtn = GTN.mc.GTN_GetGroupMaxOverride(coreNo, groupTemp, overRideType, out maxOverride);



                // 组轨迹前瞻结构体

                GTN.mc.TGroupBlendingParameterPathBlendingPrm groupBlendPathParam = new GTN.mc.TGroupBlendingParameterPathBlendingPrm();

                //groupBlendPathParam.pathBlendingPrm.reserve1 = new short[2];

                //groupBlendPathParam.pathBlendingPrm.reserve2 = new double[8];

                //groupBlendPathParam.reserve = new short[3];





                // 速度前瞻结构体

                GTN.mc.TGroupBlendingParameterVelPathBlendingPrm velblendPathParam = new GTN.mc.TGroupBlendingParameterVelPathBlendingPrm();

                //velblendPathParam.reserve = new short[3];



                //GTN.mc.TGroupBlendingParameter groupBlendPathParam = new GTN.mc.TGroupBlendingParameter();

                // memset(&groupBlendPathParam,0,sizeof(groupBlendPathParam));



                // 前瞻设置 结构体

                GTN.mc.TGroupLookAheadParameter groupAheadConfigParam = new GTN.mc.TGroupLookAheadParameter();





                short enableLa = 1;



                ////// blending 使能

                if (enableLa == 1)

                {

                    // 指令流累加

                    listInfo.segNum++;



                    // 打开 group 前瞻功能。

                    rtn = GTN.mc.GTN_GroupLookAheadEnable(coreNo, groupNo, ref listInfo);



                    groupAheadConfigParam.lookAheadNum = 100;



                    // 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低

                    groupAheadConfigParam.time = interpolationParam.AheadParam.Time;



                    //曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小

                    groupAheadConfigParam.radiusRatio = interpolationParam.AheadParam.RadiusRatio; ;



                    // 设置前瞻的参数

                    rtn = GTN.mc.GTN_SetGroupLookAheadParameter(coreNo, groupNo, ref groupAheadConfigParam, ref listInfo);

                }



                // 6、先停一下

                rtn += GTN.mc.GTN_StopCommandList(coreNo, groupNo, 1, ref listInfoNull);



                // 再清空一下指令

                rtn += GTN.mc.GTN_ClearCommandListData(coreNo, listNo, ref listInfoNull);



                // 使能Group

                rtn += GTN.mc.GTN_GroupEnable(coreNo, groupNo, ref listInfoNull);



                // 有什么区别

                // Group规划坐标系为PCS

                short profileCoord = GTN.mc.COORD_SYSTEM_PCS;



                // 设置 group 的运动规划坐标系。建立坐标系

                rtn += GTN.mc.GTN_SetGroupProfileCoordinateSystem(coreNo, groupNo, profileCoord, ref listInfoNull);





                // 数组处理,位置与方向  初始化给0  只有旋转轴会有方向

                double[] pos = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

                short[] dir = new short[8] { 0, 0, 0, 0, 0, 0, 0, 0 };



                // 工件坐标系

                short cmdCoord = GTN.mc.COORD_SYSTEM_PCS;

                short cmdOriMode = GTN.mc.ORI_MODE_NONE; // 无姿态描述。

                short configIndex = 0; // 未知



                // 设置 group 的插补指令位置描述参数。

                rtn += GTN.mc.GTN_SetGroupCommandPosDefine(coreNo, groupNo, cmdCoord, cmdOriMode, configIndex, ref listInfoNull);



                //short cmdCoordGet = GTN.mc.COORD_SYSTEM_PCS;

                //// 当指令位置的描述坐标系为笛卡尔坐标系时，需要指定姿态的描述模式。ORI_MODE_NONE：无姿态描述。

                //short cmdOriModeGet = GTN.mc.ORI_MODE_NONE;

                //short configIndexGet = 0;

                //rtn = GTN.mc.GTN_GetGroupCommandPosDefine(coreNo, groupTemp, out cmdCoordGet, out cmdOriModeGet, out configIndexGet);// 读取 group 的插补指令位置描述参数。

                //   



                GTN.mc.TGroupMoveParameter groupMovePrm = new GTN.mc.TGroupMoveParameter();



                // 平滑模式， 加加速时间在上面设置了

                // Group运动指令速度参数。



                listInfo.segNum++;

                listInfo.modal = 1;






                //--------------------------------------第一段插补运动--------------------------------------/

                GTN.mc.TDigitalOutputBit output = new GTN.mc.TDigitalOutputBit();

                foreach (SegmentConfig segment in interpolationParam.SegmentConfigs)
                {


                    if (Array.IndexOf(interpolationParam.SegmentConfigs, segment) != interpolationParam.SegmentConfigs.Length -1)
                    {
                        GTN.mc.TGroupBlendingParameterPathBlendingPrm blendPrmTemp = new GTN.mc.TGroupBlendingParameterPathBlendingPrm();//组轨迹前瞻结构体
                        GTN.mc.TGroupBlendingParameterVelPathBlendingPrm velblendPrmTemp = new GTN.mc.TGroupBlendingParameterVelPathBlendingPrm();//速度前瞻结构体
                        blendPrmTemp.pathBlendingPrm.reserve1 = new short[2];
                        blendPrmTemp.pathBlendingPrm.reserve2 = new double[8];

                        blendPrmTemp.reserve = new short[3];
                        velblendPrmTemp.reserve = new short[3];

                        ///////////////////////////////////////第一次轨迹BLENDING///////////////////////////////////////////////////////////////////////
                        //blending模式：0：轨迹blending，1：速度blending。轨迹结束，关闭轨迹前瞻
                        // BLEND_MODE_ARC:圆弧blending； BLEND_MODE_BIARC:双圆弧blending	
                        blendPrmTemp.mode = 0;
                        blendPrmTemp.pathBlendingPrm.blendType = Convert.ToInt16(GTN.mc.BLEND_MODE_ARC);       //GTN.mc.BLEND_MODE_ARC;
                                                                                                               // BLEND_PARA_TYPE_ERROR:轮廓误差，只对BLEND_MODE_ARC生效; BLEND_PARA_TYPE_RADIUS:过渡圆弧半径，只对BLEND_MODE_ARC生效；BLEND_PARA_TYPE_DISTANCE:距离
                        blendPrmTemp.pathBlendingPrm.prmType = Convert.ToInt16(2);// GTN.mc.BLEND_PARA_TYPE_RADIUS;       //过渡圆弧半径，只对BLEND_MODE_ARC生效
                                                                                                              //prmType为BLEND_PARA_TYPE_ERROR时，prm表示最大误差；prmType为BLEND_PARA_TYPE_RADIUS时，prm表示过渡的半径值；prmType为BLEND_PARA_TYPE_DISTANCE时，prm表示距离
                        blendPrmTemp.pathBlendingPrm.prm = Convert.ToDouble(3);                  // 1;           //prm表示过渡的半径值
                        blendPrmTemp.pathBlendingPrm.reserve2[0] = Convert.ToDouble(0.98);        //越小越远离
                        blendPrmTemp.pathBlendingPrm.minAngle = 0;                                                    //进行blending处理的最小角度，当轨迹矢量角度变化小于该角度时不进行处理。
                        blendPrmTemp.pathBlendingPrm.maxAngle = 180;                                                  //进行blending处理的最大角度，当轨迹矢量角度变化大于该角度时不进行处理。

                        listInfo.segNum++;//指令流加1

                        listInfo.modal = 1;

                        rtn = GTN.mc.GTN_SetGroupBlendingParameter(coreNo, groupNo, ref blendPrmTemp, ref listInfo);//设置 轨迹blending 参数。

                        /////////////////////////////////////////第一次设置速度blengding////////////////////////////////////////////////////////////////////////////////
                        listInfo.segNum++;                                            // VEL_BLEND_MODE_NEXT:过渡速度为下一段的速度； VEL_BLEND_MODE_LOW:过渡速度为
                        velblendPrmTemp.mode = 1;
                        velblendPrmTemp.velBlendingPrm.blendType = Convert.ToInt16(GTN.mc.VEL_BLEND_MODE_NEXT);// GTN.mc.VEL_BLEND_MODE_NEXT;
                        velblendPrmTemp.velBlendingPrm.percentMode = Convert.ToInt16(0); // 速度百分比模式，0：过渡速度为由blendType决定的速度，1：过渡速度为由blendType决定的速度*设置的百分比。
                        velblendPrmTemp.velBlendingPrm.percent = Convert.ToDouble(0);//0; // 速度度百分比，在percentMode为1时生效，范围：[0,1]。过渡速度为下一段速度的0.2倍

                        listInfo.segNum++;//指令流加1

                        listInfo.modal = 1;
                        rtn = GTN.mc.GTN_SetGroupBlendingParameter(coreNo, groupNo, ref velblendPrmTemp, ref listInfo);//设置 速度blending 参数。
                    }

                    listInfo.segNum++;//指令流加1

                    listInfo.modal = 0;

                    pos[0] = Convert.ToDouble(segment.Point.X);

                    pos[1] = Convert.ToDouble(segment.Point.Y);



                    if (axisCount >= 3)

                    {

                        pos[2] = Convert.ToDouble(segment.Point.Z);

                    }



                    if (axisCount >= 4)

                    {

                        pos[3] = Convert.ToDouble(segment.Point.T);

                    }



                    // 线段速度

                    groupMovePrm.velocity = segment.Velocity;   // 运动速度,mm/s

                    groupMovePrm.acceleration = segment.Acc;    // 运动加速度mm/s^2

                    groupMovePrm.deceleration = segment.Dec;    // 运动加速度mm/s^2



                    rtn += GTN.mc.GTN_MoveLinearAbsolute(coreNo, groupNo, ref pos[0], ref dir[0], ref groupMovePrm, ref listInfo); // 绝对位置模式的直线插补。

                }



                if (rtn != 0)

                {

                    throw new DriveMotionException(MotionControllerType.GT, rtn, "插补指令设置出错");

                }



                do

                {

                    rtn = GTN.mc.GTN_CommandListDataEnd(coreNo, listNo); // 指令流数据全部压入结束。



                } while (0 != rtn);



                // 启动指令流。

                rtn = GTN.mc.GTN_StartCommandList(coreNo, listNo, ref listInfo);



                if (rtn != 0)

                {

                    throw new DriveMotionException(MotionControllerType.GT, rtn, "插补指令设置出错");

                }



                #region 等待指令流结束

                //TCommandListStatus pStatus;

                //do

                //{

                //    rtn = GTN.mc.GTN_GetCommandListStatus(coreNo, listNo, out pStatus);

                //} while (pStatus.stopInfo != 10);



                #endregion



                #region 等待指令流结束 唐鹏修改



                TCommandListStatus pStatus;

                TGroupStatus groupStatus;

                Stopwatch sw = new Stopwatch();

                sw.Restart();

                do

                {

                    rtn = GTN.mc.GTN_GetCommandListStatus(coreNo, listNo, out pStatus);

                    rtn = GTN.mc.GTN_GetGroupStatus(coreNo, groupNo, out groupStatus);



                    if (sw.ElapsedMilliseconds > 30000)

                    {

                        throw new DriveMotionException(MotionControllerType.GT, rtn, "插补运动超时");

                    }

                }

                while ((1 == groupStatus.run) || (1 == pStatus.execute));



                #endregion

            }

            finally

            {

                // 移除

                rtn = GTN.mc.GTN_UngroupAllAxes(coreNo, groupNo, ref listInfoNull);

                rtn = GTN.mc.GTN_GroupDisable(coreNo, groupNo, ref listInfoNull);

            }

        }

        /// <summary>
        /// 关闭补偿
        /// </summary>
        public static void Close2DCompensate()
        {
            // 3、设置补偿相关参数
            short sRtn = 0;

            // 设置X轴补偿参数
            TCompensate2D comp2D1 = new TCompensate2D();

            // 获取X轴二维补偿参数
            sRtn += GTN_GetCompensate2D((short)2, 1, out comp2D1);

            // 使能补偿功能
            comp2D1.enable = 0;

            // 使用补偿表1中的补偿数据
            comp2D1.tableIndex = 1;

            // 查表所使用的X位置为规划位置
            comp2D1.axisType1 = MC_PROFILE;

            // 1轴作为二维补偿运动的X轴
            comp2D1.axisIndex1 = 1;

            // 查表所使用的Y位置为规划位置
            comp2D1.axisType2 = MC_PROFILE;

            // 2轴作为二维补偿运动的Y轴
            comp2D1.axisIndex2 = 2;

            // 设置补偿参数（补偿轴必须先使能）,补偿轴为X轴
            sRtn += GTN_SetCompensate2D((short)2, 1, ref comp2D1);

            // 设置Y轴补偿参数
            TCompensate2D comp2D2 = new TCompensate2D();
            sRtn += GTN_GetCompensate2D((short)2, 2, out comp2D2); // 获取Y轴二维补偿参数
            comp2D2.enable = 0; // 使能补偿功能
            comp2D2.tableIndex = 2; // 使用补偿表2中的补偿数据

            comp2D2.axisType1 = MC_PROFILE; // 查表所使用的X位置为规划位置
            comp2D2.axisIndex1 = 1; // 1轴作为二维补偿运动的X轴

            comp2D2.axisType2 = MC_PROFILE; // 查表所使用的Y位置为规划位置
            comp2D2.axisIndex2 = 2; // 轴作为二维补偿运动的Y轴
            sRtn += GTN_SetCompensate2D((short)2, 2, ref comp2D2); // 设置补偿参数（补偿轴必须先使能）,补偿轴为Y轴

            if (sRtn != 0) 
            {
                throw new Exception("关闭补偿失败");
            }
        }

        /// <summary>
        /// 开启补偿
        /// </summary>
        /// <param name="paramObj">数据对象</param>
        /// <param name="dataX">补偿X</param>
        /// <param name="dataY">补偿Y</param>
        /// <returns>结果</returns>
        public static bool Open2DCompensate(GTCalib2DParam paramObj, int[,] dataX, int[,] dataY)
        {
            GTCalib2DParam calib2DParam = (GTCalib2DParam)paramObj;

            // 3、设置补偿相关参数
            short sRtn = 0;

            // 补偿表1
            TCompensate2DTable tableX = new TCompensate2DTable();

            // 把X轴的补偿数据写入到补偿表1  默认就用 1  表  因为我们就需要一个2维补偿
            sRtn += GTN_GetCompensate2DTable((short)2, 1, out tableX, out short pExtendX); //获取二维补偿表

            // X方向3个数据
            tableX.count1 = calib2DParam.XCount;

            // Y方向2个数据
            tableX.count2 = calib2DParam.YCount;

            // 补偿起点X轴坐标
            tableX.posBegin1 = calib2DParam.PosBeginX;

            // 补偿起点Y轴坐标
            tableX.posBegin2 = calib2DParam.PosBeginY;

            // X轴补偿步长为50000
            tableX.step1 = calib2DParam.StepX;

            // Y轴补偿步长为50000
            tableX.step2 = calib2DParam.StepY;

            // 把X轴补偿量写入到补偿表1（扩展）
            sRtn += GTN_SetCompensate2DTable((short)2, 1, ref tableX, ref dataX[0, 0], 1);


            // 设置X轴补偿参数
            TCompensate2D comp2D1 = new TCompensate2D();

            // 获取X轴二维补偿参数
            sRtn += GTN_GetCompensate2D((short)2, calib2DParam.AxisXNum, out comp2D1);

            // 使能补偿功能
            comp2D1.enable = 1;

            // 使用补偿表1中的补偿数据
            comp2D1.tableIndex = 1;

            // 查表所使用的X位置为规划位置
            comp2D1.axisType1 = MC_PROFILE;
            // 1轴作为二维补偿运动的X轴
            comp2D1.axisIndex1 = calib2DParam.AxisXNum;

            // 查表所使用的Y位置为规划位置
            comp2D1.axisType2 = MC_PROFILE;

            // 2轴作为二维补偿运动的Y轴
            comp2D1.axisIndex2 = 2;

            // 设置补偿参数（补偿轴必须先使能）,补偿轴为X轴
            sRtn += GTN_SetCompensate2D((short)2, calib2DParam.AxisXNum, ref comp2D1);

            // 补偿表2
            TCompensate2DTable tableY = new TCompensate2DTable();

            // 把Y轴的补偿数据写入到补偿表2
            // 获取二维补偿表
            sRtn += GTN_GetCompensate2DTable((short)2, 2, out tableY, out short pExtendY);

            // X方向3个数据
            tableY.count1 = calib2DParam.XCount;

            // Y方向2个数据
            tableY.count2 = calib2DParam.YCount;

            // 补偿起点X轴坐标
            tableY.posBegin1 = calib2DParam.PosBeginX;

            // 补偿起点Y轴坐标
            tableY.posBegin2 = calib2DParam.PosBeginY;

            // X轴补偿步长为50000
            tableY.step1 = calib2DParam.StepX;

            // Y轴补偿步长为50000
            tableY.step2 = calib2DParam.StepY;

            // long[,] dataY = calib2DParam.DataY;

            // 把Y轴补偿量写入到补偿表2（不扩展）
            sRtn += GTN_SetCompensate2DTable((short)2, 2, ref tableY, ref dataY[0, 0], 1);

            // 设置Y轴补偿参数
            TCompensate2D comp2D2 = new TCompensate2D();
            sRtn += GTN_GetCompensate2D((short)2, calib2DParam.AxisYNum, out comp2D2); // 获取Y轴二维补偿参数
            comp2D2.enable = 1; // 使能补偿功能
            comp2D2.tableIndex = 2; // 使用补偿表2中的补偿数据

            comp2D2.axisType1 = MC_PROFILE; // 查表所使用的X位置为规划位置
            comp2D2.axisIndex1 = calib2DParam.AxisXNum; // 1轴作为二维补偿运动的X轴

            comp2D2.axisType2 = MC_PROFILE; // 查表所使用的Y位置为规划位置
            comp2D2.axisIndex2 = calib2DParam.AxisYNum; // 轴作为二维补偿运动的Y轴
            sRtn += GTN_SetCompensate2D((short)2, calib2DParam.AxisYNum, ref comp2D2); // 设置补偿参数（补偿轴必须先使能）,补偿轴为Y轴

            if (sRtn == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 开启补偿
        /// 先复位再开启
        /// </summary>
        /// <param name="pulseRatio">脉冲比</param>
        /// <returns>结果</returns>
        public static bool Open2DCompensate(int pulseRatio = 10000)
        {
            GTCalib2DParam calib2DParam = new GTCalib2DParam();

            calib2DParam.StepX = (int)GlobalCalibrationDomain.GetInstance().ColumnSpacing * pulseRatio;
            calib2DParam.StepY = (int)GlobalCalibrationDomain.GetInstance().RowSpacing * pulseRatio;

            calib2DParam.AxisXNum = 1;
            calib2DParam.AxisYNum = 2;

            calib2DParam.PosBeginX = (int)(GlobalCalibrationDomain.GetInstance().StartPoint3D.X * pulseRatio);

            calib2DParam.PosBeginY = (int)(GlobalCalibrationDomain.GetInstance().StartPoint3D.Y * pulseRatio);

            calib2DParam.XCount = (short)GlobalCalibrationDomain.GetInstance().ColumnCount;

            calib2DParam.YCount = (short)GlobalCalibrationDomain.GetInstance().RowCount;

            if (GlobalCalibrationDomain.GetInstance().AutoScale)
            {
                calib2DParam.XCount = (short)(GlobalCalibrationDomain.GetInstance().ColumnCount + 20);
                calib2DParam.YCount = (short)(GlobalCalibrationDomain.GetInstance().RowCount + 20);

                int[,] compensateX = SizeAutoExpend(GlobalCalibrationDomain.GetInstance().CompensateX, calib2DParam.XCount, calib2DParam.YCount);
                int[,] compensateY = SizeAutoExpend(GlobalCalibrationDomain.GetInstance().CompensateY, calib2DParam.XCount, calib2DParam.YCount);
                return Open2DCompensate(calib2DParam, compensateX, compensateY);
            }


            return Open2DCompensate(calib2DParam, GlobalCalibrationDomain.GetInstance().CompensateX, GlobalCalibrationDomain.GetInstance().CompensateY);
        }

        /// <summary>
        /// 自动拓展
        /// </summary>
        /// <param name="compensate">补偿矩阵</param>
        /// /// <param name="rowMax">行最大值</param>
        /// /// <param name="columnMax">列最大值</param>
        /// <returns>拓展之后的补偿</returns>
        public static int[,] SizeAutoExpend(int[,] compensate, int rowMax, int columnMax)
        {
            int[,] expendCompensate = new int[columnMax, rowMax];

            for (int i = 0; i < columnMax; i++)
            {
                for (int j = 0; j < rowMax; j++)
                {
                    if (j > compensate.GetLength(1) - 1 && i > compensate.GetLength(0) - 1)
                    {
                        expendCompensate[i, j] = compensate[compensate.GetLength(0) - 1, compensate.GetLength(1) - 1];
                    }
                    else if (j > compensate.GetLength(1) - 1)
                    {
                        // 如果列数大于原来的则用最后一个
                        expendCompensate[i, j] = compensate[i, compensate.GetLength(1) - 1];
                    }
                    else if (i > compensate.GetLength(0) - 1)
                    {
                        // 如果列数大于原来的则用最后一个
                        expendCompensate[i, j] = compensate[compensate.GetLength(0) - 1, j];
                    }
                    else
                    {
                        expendCompensate[i, j] = compensate[i, j];
                    }
                }
            }

            return expendCompensate;
        }

        [DllImport("gxn.dll")]
        public static extern short GTN_GroupStop(short core, short group, ref TListInfo pListInfo);

        /// <summary>
        /// 固高插补运动
        /// </summary>
        /// <param name="coreIndex">核号</param>
        /// <param name="param">画胶参数</param>
        /// <param name="ioIndex">输出索引</param>
        /// <returns>结果</returns>
        public static bool Interpolation(int coreIndex, EpoxyApplicationLocation[] param, int ioIndex)
        {
            try
            {
                #region 准备环境，核号根据自己的配置修改，其他不用管

                LogHelper.Post(Level.Info, $"画胶指令开始", LogCategory.Dispense, ViewType.InFileAndUI);

                // 执行结果
                short rtn = 0;

                // 核号(系统1为核1，系统为核2)
                short coreTemp = (short)coreIndex;

                // 指令流号
                short listTemp = 1;

                // 组号
                short groupTemp = 1;

                //// 指令流号
                //short listTemp = 2;

                //// 组号
                //short groupTemp = 2;

                // axisCount：是规划器号不是轴号
                short i, axisCount = 2, externalAxisCount = 0;

                GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();
                GTN.mc.TListInfo listInfoNull = new GTN.mc.TListInfo();

                // 声明结构体数组长度
                GTN.mc.TProfileScale[] scale = new GTN.mc.TProfileScale[24];
                GTN.mc.TProfileScale[] scaleRead = new GTN.mc.TProfileScale[24];
                GTN.mc.TAxisMotionConstraint[] axisMotionConstraint = new GTN.mc.TAxisMotionConstraint[24];
                double[] gearRatio = new double[24];
                #endregion

                #region 限制最大的运行参数（速度等），如果超过则按照这个最大的参数运行，不用修改

                double velMax = 800, accMax = 8000, jerkMax = 80000;

                listInfo.reserve1 = new short[2];
                listInfo.reserve2 = new short[3];
                listInfo.reserve3 = new double[4];
                listInfoNull.reserve1 = new short[2];
                listInfoNull.reserve2 = new short[3];
                listInfoNull.reserve3 = new double[4];

                // 1、基本轴初始化
                // 最大速度，加速度，超过就按照这个数值运动
                for (i = 0; i < axisCount + externalAxisCount; ++i)
                {
                    scale[i].alpha = new double[4];
                    scale[i].beta = new double[4];
                    scale[i].reverse1 = new short[3];

                    scale[i].count = 2;
                    scale[i].alpha[0] = 1; // 脉冲当量，alpha可以认为是mm的单位，beta是脉冲的单位。beta / alpha

                    // 脉冲当量，根据轴去设置
                    scale[i].beta[0] = 10000; // 10000个脉冲1mm


                    scale[i].alpha[1] = 1;
                    scale[i].beta[1] = 1;


                    // 设置脉冲当量，毫米 对应的 脉冲数
                    rtn = GTN.mc.GTN_SetAxisScale(coreTemp, Convert.ToInt16(i + 1), ref scale[i], ref listInfoNull);
                    rtn = GTN.mc.GTN_GetAxisScale(coreTemp, Convert.ToInt16(i + 1), out scaleRead[i]);

                    // 设置轴运动约束参数，规划器运动的最大参数，GT_运动指令中的运动参数大于这些参数数，约束到该最大值
                    //axisMotionConstraint[i].reserve1 = new short[3];
                    //axisMotionConstraint[i].reserve2 = new double[8];
                    //axisMotionConstraint[i].velMax = velMax; // 单位：mm/s   或者 度/s
                    //axisMotionConstraint[i].accMax = accMax; // 单位：mm/s^2 或者 度/s^2
                    //axisMotionConstraint[i].decMax = accMax; // 单位：mm/s^2 或者 度/s^2
                    //axisMotionConstraint[i].jerkMax = jerkMax; // 单位：mm/s^3 或者 度/s^3
                    //axisMotionConstraint[i].dvMax = 10; // 单位：mm/s   或者 度/s   轴的最大速度跳变量
                    //rtn = GTN.mc.GTN_SetAxisMotionConstraint(
                    //    coreTemp,
                    //    Convert.ToInt16(i + 1),
                    //    ref axisMotionConstraint[i],
                    //    ref listInfoNull);
                    //rtn = GTN.mc.GTN_GetAxisMotionConstraint(
                    //    coreTemp,
                    //    Convert.ToInt16(i + 1),
                    //    out axisMotionConstraint[i]);
                }

                #endregion

                #region 族群准备，不用管

                //// 2、List初始化
                GTN.mc.TCommandListConfig commandListConfig = new GTN.mc.TCommandListConfig();

                // 静态模式只支持指令流空间大小的数据(4096段)，控制器不会自动清除指令流数据，只有调用GTN_ClearCommandListData时才会清空数据，
                // 动态模式下指令流数据执行后可以继续发送新的数据，保证指令流可以动态执行起来。
                commandListConfig.mode = GTN.mc.COMMAND_LIST_MODE_DYNAMIC;
                commandListConfig.elementSize = 128 * sizeof(double); // 指令流里的每条指令byte个数最大值，
                commandListConfig.forwardSpace = 1000; // 指令流允许预存的最大段数
                commandListConfig.reverseSpace = 0; // 指令流允许回退的最大段数

                //rtn = GTN.mc.GTN_SetCommandListConfig(coreTemp, listTemp, ref commandListConfig);
                rtn = GTN.mc.GTN_GetCommandListConfig(coreTemp, listTemp, out commandListConfig);

                //// 将Group与List关联。按未关联，最多2个Group关联到同一个List中
                uint linkGroupMask;
                rtn = GTN.mc.GTN_GetCommandListLinkGroup(coreTemp, listTemp, out linkGroupMask);

                // linkGroupMask |= 1<<(groupTemp-1);
                linkGroupMask |= Convert.ToUInt32(1 << (groupTemp - 1));
                rtn = GTN.mc.GTN_SetCommandListLinkGroup(coreTemp, listTemp, linkGroupMask);

                //// 3、Group初始化
                rtn = GTN.mc.GTN_UngroupAllAxes(coreTemp, groupTemp, ref listInfoNull);
                rtn = GTN.mc.GTN_GroupDisable(coreTemp, groupTemp, ref listInfoNull);

                #endregion

                #region 将轴号添加到Group中去

                // 核的单独轴号
                short axisX = Convert.ToInt16(1), axisY = Convert.ToInt16(2);

                // 把轴添加到组中
                rtn = GTN.mc.GTN_AddAxisToGroup(coreTemp, groupTemp, axisX, 1, ref listInfoNull);
                rtn = GTN.mc.GTN_AddAxisToGroup(coreTemp, groupTemp, axisY, 2, ref listInfoNull);

                GTN.mc.TKinematicTransformMultiAxis kinematicTransformMultiAxis =
                    new GTN.mc.TKinematicTransformMultiAxis();
                kinematicTransformMultiAxis.reserve = new short[3];
                kinematicTransformMultiAxis.multiAxis.reserve1 = new short[3];
                kinematicTransformMultiAxis.multiAxis.reserve2 = new int[10];
                kinematicTransformMultiAxis.multiAxis.reserve3 = new double[42];

                kinematicTransformMultiAxis.type = GTN.mc.KIN_TYPE_ORTHOGONAL;
                rtn = GTN.mc.GTN_SetGroupKinematicTransform(
                    coreTemp,
                    groupTemp,
                    ref kinematicTransformMultiAxis,
                    ref listInfoNull);

                #endregion

                #region 组群设置

                // 设置group的笛卡尔坐标系运动约束参数。插补规划器的运动约束
                //GTN.mc.TGroupMotionConstraint groupMotionConstraint = new GTN.mc.TGroupMotionConstraint();
                //groupMotionConstraint.reserve = new double[10];

                //rtn = GTN.mc.GTN_GetGroupMotionConstraint(coreTemp, groupTemp, out groupMotionConstraint);
                //groupMotionConstraint.velMax = velMax; // 单位：mm/s   或者 度/s
                //groupMotionConstraint.accMax = accMax; // 单位：mm/s^2 或者 度/s^2
                //groupMotionConstraint.decMax = accMax; // 单位：mm/s^2 或者 度/s^2
                //groupMotionConstraint.jerkMax = jerkMax; // 单位：mm/s^3 或者 度/s^3
                //rtn = GTN.mc.GTN_SetGroupMotionConstraint(
                //    coreTemp,
                //    groupTemp,
                //    ref groupMotionConstraint,
                //    ref listInfoNull);

                #endregion

                #region 平滑设置，accTime越大，拐角越快

                // 设置group的速度规划模式。目前只支持平滑模式PROFILE_MODE_SMOOTH
                GTN.mc.TVelProfileSmoothMode velProfileModeSmooth = new GTN.mc.TVelProfileSmoothMode();
                velProfileModeSmooth.mode = GTN.mc.VEL_PROFILE_MODE_SMOOTH;
                velProfileModeSmooth.reserve1 = new short[3];
                velProfileModeSmooth.smooth.reserve = new double[18];
                velProfileModeSmooth.smooth.accTime = 30; // 加速度变化时间，类似Trap模式的smoothTime
                velProfileModeSmooth.smooth.k = 0; // 形态系数,保留
                rtn = GTN.mc.GTN_SetGroupVelProfileMode(
                    coreTemp,
                    groupTemp,
                    ref velProfileModeSmooth,
                    ref listInfoNull);
                rtn = GTN.mc.GTN_GetGroupVelProfileMode(coreTemp, groupTemp, out velProfileModeSmooth);

                #endregion

                #region 设置停止的平滑参数，不用管

                // stopType：0：平滑停止。1：急停。2：异常停止
                short stopType = 0;
                short decelerationSmoothStop = Convert.ToInt16(5);
                short jerkSmoothStop = Convert.ToInt16(5);
                GTN.mc.TGroupStopParameter stopPrm = new GTN.mc.TGroupStopParameter();
                stopPrm.reserve1 = new short[4];
                stopPrm.reserve2 = new double[5];
                stopPrm.deceleration = decelerationSmoothStop; // 停止加速度，取值范围：(0,...)，单位：mm/s^2。
                stopPrm.jerk = jerkSmoothStop; // 停止加加速度，取值范围：(0,...)，单位：mm/s^3。。
                rtn = GTN.mc.GTN_SetGroupStopParameter(coreTemp, groupTemp, stopType, ref stopPrm, ref listInfoNull);
                rtn = GTN.mc.GTN_GetGroupStopParameter(coreTemp, groupTemp, stopType, out stopPrm);

                #endregion

                #region 轨迹前瞻，不用管

                // 4、初始化前瞻
                double time = Convert.ToDouble(0.1), radiusRatio = Convert.ToDouble(1);
                short lookAheadNum = Convert.ToInt16(200);
                short enableBlend = 0;
                short enableLa = 1;
                //GTN.mc.TGroupBlendingParameterPathBlendingPrm blendPrmTemp =
                //    new GTN.mc.TGroupBlendingParameterPathBlendingPrm(); //组轨迹前瞻结构体
                //GTN.mc.TGroupBlendingParameterVelPathBlendingPrm velblendPrmTemp =
                //    new GTN.mc.TGroupBlendingParameterVelPathBlendingPrm(); //速度前瞻结构体
                //blendPrmTemp.pathBlendingPrm.reserve1 = new short[2];
                //blendPrmTemp.pathBlendingPrm.reserve2 = new double[8];

                //blendPrmTemp.reserve = new short[3];
                //velblendPrmTemp.reserve = new short[3];

                // GTN.mc.TGroupBlendingParameter blendPrmTemp = new GTN.mc.TGroupBlendingParameter();
                // memset(&blendPrmTemp,0,sizeof(blendPrmTemp));
                GTN.mc.TGroupLookAheadParameter groupLaPrmTemp = new GTN.mc.TGroupLookAheadParameter();

                // memset(&groupLaPrmTemp,0,sizeof(groupLaPrmTemp));

                ////listInfo初始化
                listInfo.list = listTemp;
                listInfo.modal = 1;
                listInfo.segNum = 0;

                //////blending 使能
                if (1 == enableLa)
                {
                    listInfo.segNum++; //指令流累加
                    rtn = GTN.mc.GTN_GroupLookAheadDisable(coreTemp, groupTemp, ref listInfoNull); //打开 group 前瞻功能。
                    rtn = GTN.mc.GTN_GroupLookAheadEnable(coreTemp, groupTemp, ref listInfo); //打开 group 前瞻功能。

                    groupLaPrmTemp.lookAheadNum = lookAheadNum;
                    groupLaPrmTemp.time = time;
                    groupLaPrmTemp.radiusRatio = radiusRatio;

                    listInfo.segNum++; //指令流累加

                    rtn = GTN.mc.GTN_SetGroupLookAheadParameter(coreTemp, groupTemp, ref groupLaPrmTemp, ref listInfo);
                }

                #endregion

                #region 开始压指令运动

                rtn = GTN.mc.GTN_StopCommandList(coreTemp, listTemp, 1, ref listInfoNull);
                rtn = GTN.mc.GTN_ClearCommandListData(coreTemp, listTemp, ref listInfoNull);

                // 使能Group
                rtn = GTN.mc.GTN_GroupEnable(coreTemp, groupTemp, ref listInfoNull); //使能 group。

                // Group规划坐标系为PCS
                short profileCoord;
                profileCoord = GTN.mc.COORD_SYSTEM_PCS;
                rtn = GTN.mc.GTN_SetGroupProfileCoordinateSystem(
                    coreTemp,
                    groupTemp,
                    profileCoord,
                    ref listInfoNull); //设置 group 的运动规划坐标系。建立坐标系

                // PCS下的坐标点
                short cmdCoord = GTN.mc.COORD_SYSTEM_PCS; //工件坐标系
                short cmdOriMode = GTN.mc.ORI_MODE_NONE; //未知
                short configIndex = 0; //未知

                //
                double posTemp = 0;
                short segMentCountIndex = 0, segMentCount = 5;
                //数组处理,位置与方向  初始化给0
                double[] pos = new double[8];
                for (short k = 0; k < 8; k++)
                {
                    pos[k] = 0;
                }

                short[] dir = new short[8];
                for (short t = 0; t < 8; t++)
                {
                    dir[t] = 0;
                }

                GTN.mc.TGroupMoveParameter groupMovePrm = new GTN.mc.TGroupMoveParameter();

                cmdCoord = GTN.mc.COORD_SYSTEM_PCS;
                rtn = GTN.mc.GTN_SetGroupCommandPosDefine(
                    coreTemp,
                    groupTemp,
                    cmdCoord,
                    cmdOriMode,
                    configIndex,
                    ref listInfoNull); // 设置 group 的插补指令位置描述参数。

                short cmdCoordGet = GTN.mc.COORD_SYSTEM_PCS;
                short cmdOriModeGet = GTN.mc.ORI_MODE_NONE; //当指令位置的描述坐标系为笛卡尔坐标系时，需要指定姿态的描述模式。ORI_MODE_NONE：无姿态描述。
                short configIndexGet = 0;
                rtn = GTN.mc.GTN_GetGroupCommandPosDefine(
                    coreTemp,
                    groupTemp,
                    out cmdCoordGet,
                    out cmdOriModeGet,
                    out configIndexGet); // 读取 group 的插补指令位置描述参数。
                                         // Group运动指令速度参数。
                groupMovePrm.velocity = 10; // 运动速度,mm/s
                groupMovePrm.acceleration = 100; // 运动加速度mm/s^2
                groupMovePrm.deceleration = 100; // 运动加速度mm/s^2

                listInfo.segNum++;
                listInfo.modal = 1;

                double[] reserve2 = new double[8];
                short[] reserve1 = new short[2];

                if (rtn != 0)
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                   "固高插补基本参数错误",
                   "画胶报警",
                   new string[] { "确认" },
                   new DialogResult[] { DialogResult.OK },
                   AlarmLevel.SecondLevel);
                    throw new Exception("固高画胶参数错误");
                }

                #endregion

                #region 真正开始准备指令运动，前面都不用管(中途开关胶需要根据IO自己去设定IO口)

                //--------------------------------------第一段插补运动--------------------------------------/
                GTN.mc.TDigitalOutputBit output = new GTN.mc.TDigitalOutputBit();

                for (int j = 0; j < param.Length - 1; j++)
                {
                    if (param[j].OpenGlueFlag)
                    {
                        // 滞后开
                        listInfo.segNum++;
                        listInfo.modal = 0;
                        GTN.mc.TDigitalOutputProReloadMoveTime tDByTime = new TDigitalOutputProReloadMoveTime();
                        tDByTime.mode = 3;
                        tDByTime.doType = GTN.mc.MC_GPO;
                        tDByTime.doIndex = (short)ioIndex;
                        tDByTime.doCount = 1;
                        tDByTime.moveTime.type = 0; // 0运动后延迟执行开胶  1 为运动结束前提前触发
                        tDByTime.moveTime.motionType = 0;
                        tDByTime.moveTime.delayTime = param[j].LagOpen;
                        short[] pValue = new short[2] { 0, 0 };
                        tDByTime.pValue = Marshal.UnsafeAddrOfPinnedArrayElement(pValue, 0);
                        rtn = GTN.mc.GTN_WriteDigitalOutputPro((short)coreIndex, ref tDByTime, ref listInfo);

                        // 提前关胶，有数值才会关
                        if (param[j].AdvanceClose != 0)
                        {
                            // 提前关
                            listInfo.segNum++;
                            listInfo.modal = 0;
                            GTN.mc.TDigitalOutputProReloadMoveTime tDByTime2 = new TDigitalOutputProReloadMoveTime();
                            tDByTime2.mode = 3;
                            tDByTime2.doType = GTN.mc.MC_GPO;
                            tDByTime2.doIndex = (short)ioIndex;
                            tDByTime2.doCount = 1;
                            tDByTime2.moveTime.type = 1; // 0运动后延迟执行开胶  1 为运动结束前提前触发
                            tDByTime2.moveTime.motionType = 0;
                            tDByTime2.moveTime.delayTime = param[j].AdvanceClose;
                            short[] pValue2 = new short[2] { 1, 0 };
                            tDByTime2.pValue = Marshal.UnsafeAddrOfPinnedArrayElement(pValue2, 0);
                            rtn = GTN.mc.GTN_WriteDigitalOutputPro((short)coreIndex, ref tDByTime2, ref listInfo);
                        }
                    }
                    else
                    {
                        // 关吸气
                        output.mode = 0;
                        output.doType = GTN.mc.MC_GPO;

                        // IO号
                        output.doIndex = (short)ioIndex;
                        output.doValue = 1;
                        listInfo.segNum++;
                        listInfo.modal = 1;
                        rtn = GTN.mc.GTN_WriteDigitalOutputBit((short)coreIndex, ref output, ref listInfo);
                    }

                    listInfo.segNum++; // 指令流加1
                    listInfo.modal = 0;
                    pos[0] = Convert.ToDouble(param[j + 1].X);
                    pos[1] = Convert.ToDouble(param[j + 1].Y);

                    groupMovePrm.velocity = Convert.ToDouble(param[j].Speed); //速度
                    groupMovePrm.acceleration = Convert.ToDouble(param[j].Acc);
                    groupMovePrm.deceleration = Convert.ToDouble(param[j].Dec);
                    groupMovePrm.endVelocityMode = 0;
                    rtn = GTN.mc.GTN_MoveLinearAbsolute(
                        coreTemp,
                        groupTemp,
                        ref pos[0],
                        ref dir[0],
                        ref groupMovePrm,
                        ref listInfo); // 绝对位置模式的直线插补。
                }

                if (rtn != 0)
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                   "固高插补画胶参数错误",
                   "画胶报警",
                   new string[] { "确认" },
                   new DialogResult[] { DialogResult.OK },
                   AlarmLevel.SecondLevel);
                    throw new Exception("固高画胶参数错误");
                }

                #endregion

                #region 发送运动完成之后需要进行的操作，例如关闭IO等

                //// 关闭IO
                //output.mode = 0;
                //output.doType = GTN.mc.MC_GPO;

                //// IO号
                //output.doIndex = 26; //高低电平
                //output.doValue = 1;
                //listInfo.segNum++;
                //listInfo.modal = 1;
                //rtn = GTN.mc.GTN_WriteDigitalOutputBit(2, ref output, ref listInfo);

                #endregion

                #region 发送指令流并等待指令结束

                GTN.mc.TCommandInfoData commandInfoData = new GTN.mc.TCommandInfoData();
                int start = 0;
                int count = 100;

                //listInfo.list = 0;

                LogHelper.Post(Level.Info, $"画胶指令流数据开始压入", LogCategory.Dispense, ViewType.InFileAndUI);

                do
                {
                    rtn = GTN.mc.GTN_CommandListDataEnd(coreTemp, listTemp); //指令流数据全部结束。
                }
                while (rtn != 0);

                LogHelper.Post(Level.Info, $"画胶指令流数据压入完成", LogCategory.Dispense, ViewType.InFileAndUI);

                listInfo.list = 0;
                rtn = GTN.mc.GTN_StartCommandList(coreTemp, listTemp, ref listInfo); //启动指令流。

                LogHelper.Post(Level.Info, $"画胶指令启动指令流完成", LogCategory.Dispense, ViewType.InFileAndUI);

                // 等待指令流结束
                TCommandListStatus pStatus;
                do
                {
                    rtn = GTN.mc.GTN_GetCommandListStatus(coreTemp, listTemp, out pStatus);
                }
                while (/*pStatus.stopInfo != 10*/pStatus.execute != 0);

                LogHelper.Post(Level.Info, $"画胶指令完成", LogCategory.Dispense, ViewType.InFileAndUI);

                //rtn = GTN_GroupStop(coreTemp, groupTemp, ref listInfoNull);
                //rtn = GTN.mc.GTN_UngroupAllAxes(coreTemp, groupTemp, ref listInfoNull);
                //rtn = GTN.mc.GTN_GroupDisable(coreTemp, groupTemp, ref listInfoNull);
                //rtn = GTN.mc.GTN_StopCommandList(coreTemp, listTemp, (short)0, ref listInfo);

                rtn = GTN_GroupStop(coreTemp, groupTemp, ref listInfoNull);
                rtn = GTN.mc.GTN_UngroupAllAxes(coreTemp, groupTemp, ref listInfoNull);
                rtn = GTN.mc.GTN_GroupDisable(coreTemp, groupTemp, ref listInfoNull);
                rtn = GTN.mc.GTN_StopCommandList(coreTemp, listTemp, (short)0, ref listInfoNull);

                LogHelper.Post(Level.Info, $"画胶完成", LogCategory.Dispense, ViewType.InFileAndUI);




                if (rtn == 0)
                {
                    return true;
                }

                return false;




                #endregion
            }
            catch (Exception e)
            {
                throw new Exception("画胶失败，可能原因 1：硬件出现故障 2：画胶参数有问题");
            }
        }

        /// <summary>
        /// 连续插补
        /// </summary>
        /// <param name="interpolationParam">插补参数</param>
        public static void PickContinueInterpolationMove(IAxisDrive[] axisDrives, SegmentConfig[] segmentConfigs, AccuracyMode accuracyMode = AccuracyMode.HighAccuracy)
        {
            Stopwatch sp = Stopwatch.StartNew();

            short rtn = 0;

            // 获取核号
            short coreNo = /*(short)this.CardNum*/ 2;

            short listNo = /*interpolationParam.ListNo;*/ 2; //1;  // 指令流号
            short groupNo = /*(short)interpolationParam.GrpCrd;*/ 2; //1; // 组号

            GTN.mc.TListInfo listInfoNull = new GTN.mc.TListInfo();

            //listInfoNull.reserve1 = new short[2];
            //listInfoNull.reserve2 = new short[3];
            //listInfoNull.reserve3 = new double[4];

            // 指令流结构
            GTN.mc.TListInfo listInfo = new GTN.mc.TListInfo();

            // listInfo初始化
            listInfo.list = listNo;
            listInfo.modal = 0;
            listInfo.segNum = 0;
            //listInfo.reserve1 = new short[2];
            //listInfo.reserve2 = new short[3];
            //listInfo.reserve3 = new double[4];

            try
            {
                // 轴数             
                int axisCount = axisDrives.Length; //2;
                int[] axisNoArr = axisDrives.Select(a => a.AxisNum).ToArray();

                // short externalAxisCount = 0;

                // 声明结构体数组长度 脉冲当量 保存每个轴的脉冲当量
                GTN.mc.TProfileScale[] scale = new GTN.mc.TProfileScale[24];

                // 读取脉冲当量
                // GTN.mc.TProfileScale[] scaleRead = new GTN.mc.TProfileScale[24];
                // double[] gearRatio = new double[24];
                // double velMax, accMax, jerkMax;

                // 1、基本轴初始化
                // velMax = 2000;
                // accMax = 10000;
                // jerkMax = 1000000;

                for (int i = 0; i < axisCount; i++)
                {
                    scale[i].alpha = new double[4];
                    scale[i].beta = new double[4];
                    scale[i].reverse1 = new short[3];

                    // 如果这个个数设置成1，那后面的数组只要设置第一组就可以，设置成2，就设置两组。  有减速比的情况下 可能需要设置2组
                    scale[i].count = 1;   // 固定
                    scale[i].alpha[0] = 1;                 // 脉冲当量，alpha可以认为是mm的单位，beta是脉冲的单位。beta / alpha 默认值设置1 就可以了
                    scale[i].beta[0] = axisDrives[i].PulseEquivalent;    //10000;              // 设置规划器的脉冲当量 10000个脉冲1mm

                    scale[i].alpha[1] = 1;
                    scale[i].beta[1] = 1;

                    // 设置规划器的脉冲当量，毫米 对应的 脉冲数     卡号， 规划器号
                    rtn = GTN.mc.GTN_SetAxisScale(coreNo, (short)(i + 1), ref scale[i], ref listInfoNull);

                    // 轴运动约束参数的结构体
                    //GTN.mc.TAxisMotionConstraint[] axisMotionConstraint = new GTN.mc.TAxisMotionConstraint[24];
                    // rtn = GTN.mc.GTN_GetAxisScale(coreNo, Convert.ToInt16(i + 1), out scaleRead[i]);
                    /*
                     //    memset(&axisMotionConstraint[i],0,sizeof(axisMotionConstraint[i]));
                     //    设置轴运动约束参数，规划器运动的最大参数，GT_运动指令中的运动参数大于这些参数数，约束到该最大值
                     axisMotionConstraint[i].reserve1 = new short[3];
                     axisMotionConstraint[i].reserve2 = new double[8];

                     axisMotionConstraint[i].velMax = velMax;        // 单位：mm/s   或者 度/s
                     axisMotionConstraint[i].accMax = accMax;        // 单位：mm/s^2 或者 度/s^2
                     axisMotionConstraint[i].decMax = accMax;        // 单位：mm/s^2 或者 度/s^2
                     axisMotionConstraint[i].jerkMax = jerkMax;      // 单位：mm/s^3 或者 度/s^3
                     axisMotionConstraint[i].dvMax = 10;             // 单位：mm/s   或者 度/s   轴的最大速度跳变量

                     rtn = GTN.mc.GTN_SetAxisMotionConstraint(coreNo, Convert.ToInt16(i + 1), ref axisMotionConstraint[i], ref listInfoNull);

                     // rtn = GTN.mc.GTN_GetAxisMotionConstraint(coreNo, Convert.ToInt16(i + 1), out axisMotionConstraint[i]);
                    */
                }

                /*  看起来好像没什么用
                // 2、List初始化
                GTN.mc.TCommandListConfig commandListConfig = new GTN.mc.TCommandListConfig();

                // 静态模式只支持指令流空间大小的数据(4096段)，控制器不会自动清除指令流数据，只有调用GTN_ClearCommandListData时才会清空数据，
                // 动态模式下指令流数据执行后可以继续发送新的数据，保证指令流可以动态执行起来。
                commandListConfig.mode = GTN.mc.COMMAND_LIST_MODE_DYNAMIC;
                commandListConfig.elementSize = 128 * sizeof(double);               // 指令流里的每条指令byte个数最大值，
                commandListConfig.forwardSpace = 1000;                              // 指令流允许预存的最大段数
                commandListConfig.reverseSpace = 0;                                 // 指令流允许回退的最大段数

                // rtn = GTN.mc.GTN_SetCommandListConfig(coreNo, listTemp, ref commandListConfig);
                rtn = GTN.mc.GTN_GetCommandListConfig(coreNo, listNo, out commandListConfig);
                */

                // 将Group与List关联。按未关联，最多2个Group关联到同一个List中
                uint linkGroupMask;
                rtn += GTN.mc.GTN_GetCommandListLinkGroup(coreNo, listNo, out linkGroupMask);

                linkGroupMask |= Convert.ToUInt32(1 << (groupNo - 1));

                rtn += GTN.mc.GTN_SetCommandListLinkGroup(coreNo, listNo, linkGroupMask);

                //// 3、Group初始化
                rtn += GTN.mc.GTN_UngroupAllAxes(coreNo, groupNo, ref listInfoNull);
                rtn += GTN.mc.GTN_GroupDisable(coreNo, groupNo, ref listInfoNull);


                // 将轴号添加到Group中去
                //short AXIS_X, AXIS_Y;
                //AXIS_X = Convert.ToInt16(param.AxisX.AxisNum);
                //AXIS_Y = Convert.ToInt16(param.AxisY.AxisNum);

                //把轴添加到组中
                for (int i = 0; i < axisCount; i++)
                {
                    rtn += GTN.mc.GTN_AddAxisToGroup(coreNo, groupNo, (short)axisNoArr[i], (short)(i + 1), ref listInfoNull);
                    // rtn = GTN.mc.GTN_AddAxisToGroup(coreNo, groupNo, (short)axisNoArr[1], 2, ref listInfoNull);
                }

                // 设置运动学参数

                //GTN.mc.TKinematicTransformMultiAxis kinematicTransformMultiAxis = new GTN.mc.TKinematicTransformMultiAxis();
                //kinematicTransformMultiAxis.reserve = new short[3];
                //kinematicTransformMultiAxis.multiAxis.reserve1 = new short[3];
                //kinematicTransformMultiAxis.multiAxis.reserve2 = new Int32[10];
                //kinematicTransformMultiAxis.multiAxis.reserve3 = new double[42];

                //// memset(&kinematicTransform,0,sizeof(kinematicTransform));
                //// 标准正交三轴结构
                //kinematicTransformMultiAxis.type = GTN.mc.KIN_TYPE_ORTHOGONAL;
                //rtn = GTN.mc.GTN_SetGroupKinematicTransform(coreNo, groupTemp, ref kinematicTransformMultiAxis, ref listInfoNull);


                //设置group的笛卡尔坐标系运动约束参数。插补规划器的运动约束
                GTN.mc.TGroupMotionConstraint groupMotionConstraint = new GTN.mc.TGroupMotionConstraint();
                groupMotionConstraint.reserve = new double[10];
                //memset(&groupMotionConstraint,0,sizeof(groupMotionConstraint));
                rtn = GTN.mc.GTN_GetGroupMotionConstraint(coreNo, groupNo, out groupMotionConstraint);
                groupMotionConstraint.velMax = 1500;          // 单位：mm/s   或者 度/s
                groupMotionConstraint.accMax = 15000;          // 单位：mm/s^2 或者 度/s^2
                groupMotionConstraint.decMax = 15000;          // 单位：mm/s^2 或者 度/s^2
                groupMotionConstraint.jerkMax = 150000;        // 单位：mm/s^3 或者 度/s^3
                rtn = GTN.mc.GTN_SetGroupMotionConstraint(coreNo, groupNo, ref groupMotionConstraint, ref listInfoNull);


                // 设置group的速度规划模式。目前只支持平滑模式PROFILE_MODE_SMOOTH
                GTN.mc.TVelProfileSmoothMode velProfileModeSmooth = new GTN.mc.TVelProfileSmoothMode();
                //memset(&velProfileMode, 0, sizeof(velProfileMode));
                velProfileModeSmooth.mode = GTN.mc.VEL_PROFILE_MODE_SMOOTH;
                //velProfileModeSmooth.reserve1 = new short[3];
                //velProfileModeSmooth.smooth.reserve = new double[18];

                velProfileModeSmooth.smooth.accTime = /*interpolationParam.AccAcc;*/ 50; //50;       // 加速度变化时间，类似Trap模式的smoothTime 当 accTime 为 0 时，速度规划曲线为 T 型曲线
                velProfileModeSmooth.smooth.k = 0;              // 形态系数,取值范围：[0,1000], 暂时不清楚是什么意思
                rtn += GTN.mc.GTN_SetGroupVelProfileMode(coreNo, groupNo, ref velProfileModeSmooth, ref listInfoNull);

                //rtn = GTN.mc.GTN_GetGroupVelProfileMode(coreNo, groupTemp, out velProfileModeSmooth);

                // 设置Group的停止参数
                short decelerationSmoothStop = 5; // 组的平滑停止减速度，取值范围：(0,...)，单位：mm/s^2。
                short jerkSmoothStop = 5;         // 停止减减速度，取值范围：(0,...)，单位：mm/s^3。
                short stopType = 0;                       // type：0：平滑停止	1：急停		2：异常停止
                                                          //stopPrm.mode = 0;					// 0：停止；1：停止到段末
                                                          //stopPrm.haltMode = 0;				// 0：暂停过程中不允许再次启动，1：暂停过程中允许再次启动

                GTN.mc.TGroupStopParameter stopPrm = new GTN.mc.TGroupStopParameter();
                //stopPrm.reserve1 = new short[4];
                //stopPrm.reserve2 = new double[5];
                stopPrm.deceleration = decelerationSmoothStop;      // 组的平滑停止减速度，取值范围：(0,...)，单位：mm/s^2。
                stopPrm.jerk = jerkSmoothStop;                      // 停止减减速度，取值范围：(0,...)，单位：mm/s^3。

                rtn += GTN.mc.GTN_SetGroupStopParameter(coreNo, groupNo, stopType, ref stopPrm, ref listInfoNull);
                // rtn = GTN.mc.GTN_GetGroupStopParameter(coreNo, groupTemp, stopType, out stopPrm);

                /*
                short decelerationQuickStop, jerkQuickStop;
                decelerationQuickStop = 5;
                jerkQuickStop = 5;
                GTN.mc.TGroupStopParameter abruptStopPrm = new GTN.mc.TGroupStopParameter();
                abruptStopPrm.reserve1 = new short[4];
                abruptStopPrm.reserve2 = new double[5];
                // memset(&abruptStopPrm, 0, sizeof(abruptStopPrm));
                stopType = 1;                           // type：0：平滑停止	1：急停		 2：异常停止
                                                        //abruptStopPrm.mode = 0;	    // 0：停止；1：停止到段末
                                                        // abruptStopPrm.haltMode = 0;	// 0：暂停过程中不允许再次启动，1：暂停过程中允许再次启动
                abruptStopPrm.deceleration = decelerationQuickStop; // 停止加速度，取值范围：(0,...)，单位：mm/s^2。
                abruptStopPrm.jerk = jerkQuickStop;// 停止加加速度，取值范围：(0,...)，单位：mm/s^3。。
                rtn = GTN.mc.GTN_SetGroupStopParameter(coreNo, groupTemp, stopType, ref abruptStopPrm, ref listInfoNull);
                rtn = GTN.mc.GTN_GetGroupStopParameter(coreNo, groupTemp, stopType, out abruptStopPrm);
                */

                //// 设置group运动最大速度倍率和加速度倍率
                //double maxOverride = Convert.ToDouble(textBox_groupMaxOverride.Text);
                //short overRideType;
                //overRideType = GTN.mc.OVERRIDE_TYPE_VEL;            // OVERRIDE_TYPE_VEL 速度倍率;OVERRIDE_TYPE_ACC 加速度倍率，此类型为保留值，暂不支持
                //rtn = GTN.mc.GTN_SetGroupMaxOverride(coreNo, groupTemp, overRideType, maxOverride, ref listInfoNull);
                //rtn = GTN.mc.GTN_GetGroupMaxOverride(coreNo, groupTemp, overRideType, out maxOverride);

                // 组轨迹前瞻结构体
                //GTN.mc.TGroupBlendingParameterPathBlendingPrm groupBlendPathParam = new GTN.mc.TGroupBlendingParameterPathBlendingPrm();
                //groupBlendPathParam.pathBlendingPrm.reserve1 = new short[2];
                //groupBlendPathParam.pathBlendingPrm.reserve2 = new double[8];
                //groupBlendPathParam.reserve = new short[3];


                // 速度前瞻结构体
                GTN.mc.TGroupBlendingParameterVelPathBlendingPrm velblendPathParam = new GTN.mc.TGroupBlendingParameterVelPathBlendingPrm();
                //velblendPathParam.reserve = new short[3];

                //GTN.mc.TGroupBlendingParameter groupBlendPathParam = new GTN.mc.TGroupBlendingParameter();
                // memset(&groupBlendPathParam,0,sizeof(groupBlendPathParam));

                // 前瞻设置 结构体
                GTN.mc.TGroupLookAheadParameter groupAheadConfigParam = new GTN.mc.TGroupLookAheadParameter();


                short enableLa = 1;

                ////// blending 使能
                if (enableLa == 1)
                {
                    // 指令流累加
                    listInfo.segNum++;

                    // 打开 group 前瞻功能。
                    rtn = GTN.mc.GTN_GroupLookAheadEnable(coreNo, groupNo, ref listInfo);

                    groupAheadConfigParam.lookAheadNum = 200;

                    // 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
                    groupAheadConfigParam.time = /*interpolationParam.AheadParam.Time*/ 10;

                    //曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
                    groupAheadConfigParam.radiusRatio = /*interpolationParam.AheadParam.RadiusRatio*/ 10;

                    // 设置前瞻的参数
                    rtn = GTN.mc.GTN_SetGroupLookAheadParameter(coreNo, groupNo, ref groupAheadConfigParam, ref listInfo);
                }

                // 6、先停一下
                rtn += GTN.mc.GTN_StopCommandList(coreNo, groupNo, 1, ref listInfoNull);

                // 再清空一下指令
                rtn += GTN.mc.GTN_ClearCommandListData(coreNo, listNo, ref listInfoNull);

                // 使能Group
                rtn += GTN.mc.GTN_GroupEnable(coreNo, groupNo, ref listInfoNull);

                // 有什么区别
                // Group规划坐标系为PCS
                short profileCoord = GTN.mc.COORD_SYSTEM_PCS;

                // 设置 group 的运动规划坐标系。建立坐标系
                rtn += GTN.mc.GTN_SetGroupProfileCoordinateSystem(coreNo, groupNo, profileCoord, ref listInfoNull);


                // 数组处理,位置与方向  初始化给0  只有旋转轴会有方向
                double[] pos = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                short[] dir = new short[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

                // 工件坐标系
                short cmdCoord = GTN.mc.COORD_SYSTEM_PCS;
                short cmdOriMode = GTN.mc.ORI_MODE_NONE; // 无姿态描述。
                short configIndex = 0; // 未知

                // 设置 group 的插补指令位置描述参数。
                rtn += GTN.mc.GTN_SetGroupCommandPosDefine(coreNo, groupNo, cmdCoord, cmdOriMode, configIndex, ref listInfoNull);

                //short cmdCoordGet = GTN.mc.COORD_SYSTEM_PCS;
                //// 当指令位置的描述坐标系为笛卡尔坐标系时，需要指定姿态的描述模式。ORI_MODE_NONE：无姿态描述。
                //short cmdOriModeGet = GTN.mc.ORI_MODE_NONE;
                //short configIndexGet = 0;
                //rtn = GTN.mc.GTN_GetGroupCommandPosDefine(coreNo, groupTemp, out cmdCoordGet, out cmdOriModeGet, out configIndexGet);// 读取 group 的插补指令位置描述参数。
                //   

                GTN.mc.TGroupMoveParameter groupMovePrm = new GTN.mc.TGroupMoveParameter();

                // 平滑模式， 加加速时间在上面设置了
                // Group运动指令速度参数。

                listInfo.segNum++;
                listInfo.modal = 1;

                //--------------------------------------第一段插补运动--------------------------------------/
                GTN.mc.TDigitalOutputBit output = new GTN.mc.TDigitalOutputBit();

                foreach (SegmentConfig segment in segmentConfigs)
                {
                    if (Array.IndexOf(segmentConfigs, segment) != segmentConfigs.Length - 1)
                    {
                        GTN.mc.TGroupBlendingParameterPathBlendingPrm blendPrmTemp = new GTN.mc.TGroupBlendingParameterPathBlendingPrm();//组轨迹前瞻结构体
                        GTN.mc.TGroupBlendingParameterVelPathBlendingPrm velblendPrmTemp = new GTN.mc.TGroupBlendingParameterVelPathBlendingPrm();//速度前瞻结构体
                        blendPrmTemp.pathBlendingPrm.reserve1 = new short[2];
                        blendPrmTemp.pathBlendingPrm.reserve2 = new double[8];

                        blendPrmTemp.reserve = new short[3];
                        velblendPrmTemp.reserve = new short[3];

                        ///////////////////////////////////////第一次轨迹BLENDING///////////////////////////////////////////////////////////////////////
                        //blending模式：0：轨迹blending，1：速度blending。轨迹结束，关闭轨迹前瞻
                        // BLEND_MODE_ARC:圆弧blending； BLEND_MODE_BIARC:双圆弧blending	
                        blendPrmTemp.mode = 0;
                        blendPrmTemp.pathBlendingPrm.blendType = Convert.ToInt16(GTN.mc.BLEND_MODE_ARC);       //GTN.mc.BLEND_MODE_ARC;
                                                                                                               // BLEND_PARA_TYPE_ERROR:轮廓误差，只对BLEND_MODE_ARC生效; BLEND_PARA_TYPE_RADIUS:过渡圆弧半径，只对BLEND_MODE_ARC生效；BLEND_PARA_TYPE_DISTANCE:距离
                        blendPrmTemp.pathBlendingPrm.prmType = Convert.ToInt16(GTN.mc.BLEND_PARA_TYPE_RADIUS);// GTN.mc.BLEND_PARA_TYPE_RADIUS;       //过渡圆弧半径，只对BLEND_MODE_ARC生效
                                                                                                              //prmType为BLEND_PARA_TYPE_ERROR时，prm表示最大误差；prmType为BLEND_PARA_TYPE_RADIUS时，prm表示过渡的半径值；prmType为BLEND_PARA_TYPE_DISTANCE时，prm表示距离
                        blendPrmTemp.pathBlendingPrm.prm = Convert.ToDouble(20);                  // 1;           //prm表示过渡的半径值
                        blendPrmTemp.pathBlendingPrm.reserve2[0] = Convert.ToDouble(0.98);        //越小越远离
                        blendPrmTemp.pathBlendingPrm.minAngle = 0;                                                    //进行blending处理的最小角度，当轨迹矢量角度变化小于该角度时不进行处理。
                        blendPrmTemp.pathBlendingPrm.maxAngle = 180;                                                  //进行blending处理的最大角度，当轨迹矢量角度变化大于该角度时不进行处理。

                        listInfo.segNum++;//指令流加1

                        listInfo.modal = 0;

                        rtn = GTN.mc.GTN_SetGroupBlendingParameter(coreNo, groupNo, ref blendPrmTemp, ref listInfo);//设置 轨迹blending 参数。

                        /////////////////////////////////////第一次设置速度blengding////////////////////////////////////////////////////////////////////////////////
                        //listInfo.segNum++;                                            // VEL_BLEND_MODE_NEXT:过渡速度为下一段的速度； VEL_BLEND_MODE_LOW:过渡速度为
                        velblendPrmTemp.mode = 1;
                        velblendPrmTemp.velBlendingPrm.blendType = Convert.ToInt16(GTN.mc.VEL_BLEND_MODE_NEXT);// GTN.mc.VEL_BLEND_MODE_NEXT;
                        velblendPrmTemp.velBlendingPrm.percentMode = Convert.ToInt16(1); // 速度百分比模式，0：过渡速度为由blendType决定的速度，1：过渡速度为由blendType决定的速度*设置的百分比。
                        velblendPrmTemp.velBlendingPrm.percent = Convert.ToDouble(1);//0; // 速度度百分比，在percentMode为1时生效，范围：[0,1]。过渡速度为下一段速度的0.2倍

                        listInfo.segNum++;//指令流加1

                        listInfo.modal = 0;
                        rtn = GTN.mc.GTN_SetGroupBlendingParameter(coreNo, groupNo, ref velblendPrmTemp, ref listInfo);//设置 速度blending 参数
                    }

                    listInfo.segNum++;//指令流加1
                    listInfo.modal = 0;
                    pos[0] = Convert.ToDouble(segment.Point.X);
                    pos[1] = Convert.ToDouble(segment.Point.Y);

                    if (axisCount >= 3)
                    {
                        pos[2] = Convert.ToDouble(segment.Point.Z);
                    }

                    if (axisCount >= 4)
                    {
                        pos[3] = Convert.ToDouble(segment.Point.T);
                    }

                    // 线段速度
                    groupMovePrm.velocity = segment.Velocity;   // 运动速度,mm/s
                    groupMovePrm.acceleration = segment.Acc;    // 运动加速度mm/s^2
                    groupMovePrm.deceleration = segment.Dec;    // 运动加速度mm/s^2

                    rtn = GTN.mc.GTN_MoveLinearAbsolute(coreNo, groupNo, ref pos[0], ref dir[0], ref groupMovePrm, ref listInfo); // 绝对位置模式的直线插补。
                }

                if (rtn != 0)
                {
                    throw new DriveMotionException(MotionControllerType.GT, rtn, "插补指令设置出错");
                }

                do
                {
                    rtn = GTN.mc.GTN_CommandListDataEnd(coreNo, listNo); // 指令流数据全部压入结束。

                } while (0 != rtn);

                sp.Stop();
                AKRS.Galaxy2.Log.LogHelper.Post(
                     Level.Info,
                     $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff")}:  GT插补运动压入指令流 耗时：{sp.ElapsedMilliseconds} ms",
                     LogCategory.MainSoftWare,
                     ViewType.InFileAndUI);
                sp.Restart();
                listInfo.list = 0;
                // 启动指令流。
                rtn = GTN.mc.GTN_StartCommandList(coreNo, listNo, ref listInfo);

                if (rtn != 0)
                {
                    throw new DriveMotionException(MotionControllerType.GT, rtn, "插补指令设置出错");
                }

                #region 等待指令流结束
                //TCommandListStatus pStatus;
                //do
                //{
                //    rtn = GTN.mc.GTN_GetCommandListStatus(coreNo, listNo, out pStatus);
                //} while (pStatus.stopInfo != 10);

                #endregion

                #region 等待指令流结束 唐鹏修改

                TCommandListStatus pStatus;
                TGroupStatus groupStatus;

                do
                {
                    rtn = GTN.mc.GTN_GetCommandListStatus(coreNo, listNo, out pStatus);
                    rtn = GTN.mc.GTN_GetGroupStatus(coreNo, groupNo, out groupStatus);
                }
                while ((1 == groupStatus.run) || (1 == pStatus.execute));

                if (accuracyMode == AccuracyMode.HighAccuracy)
                {
                    foreach (var drive in axisDrives)
                    {
                        drive.WaitInRealPosition();
                    }
                }

                AKRS.Galaxy2.Log.LogHelper.Post(
                    Level.Info,
                    $"{System.DateTime.Now.ToString("MM-dd HH:mm:ss.fff")}:  GT插补运动等指令流执行结束 耗时：{sp.ElapsedMilliseconds} ms",
                    LogCategory.MainSoftWare,
                    ViewType.InFileAndUI);
                sp.Stop();

                #endregion
            }
            finally
            {
                // 移除
                rtn = GTN.mc.GTN_UngroupAllAxes(coreNo, groupNo, ref listInfoNull);
                rtn = GTN.mc.GTN_GroupDisable(coreNo, groupNo, ref listInfoNull);
            }
        }
    }
}
