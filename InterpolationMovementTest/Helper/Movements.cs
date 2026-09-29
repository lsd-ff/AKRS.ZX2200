using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.LogicHardware.Services;
using ch.etel.edi.dsa.v40;

namespace WindowsFormsApp1.Helper
{
    using System.Threading;
    using System.Windows.Forms;
    using DevExpress.Office;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

    /// <summary>
    /// 存放移动方法
    /// </summary>
    public class Movements
    {
        /// <summary>
        /// BondX轴
        /// </summary>
        public Axis BondAxisX => HardwareRepositoryService.GetHardware<Axis>("BondX");

        /// <summary>
        /// BondY轴
        /// </summary>
        public Axis BondAxisY => HardwareRepositoryService.GetHardware<Axis>("BondY");

        /// <summary>
        /// BondY轴
        /// </summary>
        public Axis BondAxisZ => HardwareRepositoryService.GetHardware<Axis>("BondZ");

        /// <summary>
        /// BondY轴
        /// </summary>
        public Axis BondAxisT => HardwareRepositoryService.GetHardware<Axis>("焊头T");

        /// <summary>
        /// 点集
        /// </summary>
        private MovePositions movePositions => MovePositions.GetInstance();

        /// <summary>
        /// 移动到起始位
        /// </summary>
        public void MoveToStartPos()
        {
            MotionService.MoveAxesToTargetPosition(
                (this.BondAxisX, true, movePositions.StartPos.X/1000, AccuracyMode.HighAccuracy),
                (this.BondAxisY, true, movePositions.StartPos.Y / 1000, AccuracyMode.HighAccuracy));

            BondAxisZ.AbsoluteMove(movePositions.StartPos.Z / 1000);
        }

        /// <summary>
        /// 四轴连续插补
        /// </summary>
        public void InterpolationMovementWithFourAxis()
        {
            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)BondAxisY.AxisDrive).GetDrive();
            DsaDrive moveDriveZ = ((ETELAxis)BondAxisZ.AxisDrive).GetDrive();
            DsaDrive moveDriveT = ((ETELAxis)BondAxisT.AxisDrive).GetDrive();

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY, moveDriveZ, moveDriveT);

            try
            {
                // 设置控制者
                iGroup.setMaster(dsaMaster);

                // 开始插补
                iGroup.ipolBegin();

                // 设置为绝对坐标系 ，不设置绝对坐标系
                iGroup.ipolSetAbsMode(true, -1);

                double speed = 0.05;
                double acceleration = 0.5;
                double deceleration = 0.5;

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);

                iGroup.ipolTanJerkTime(0.01);

                // 第一段,单位转成m要除以1000
                double[] array1 = new[] { movePositions.Point1.Pos.X / 1000.0, movePositions.Point1.Pos.Y / 1000.0, -movePositions.Point1.Pos.Z / 1000.0, movePositions.Point1.Angle / 360.0 };
                DsaVector vector1 = new DsaVector(array1);
                iGroup.ipolLine(vector1);
                 iGroup.ipolBeginConcatenation();

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);

                // iGroup.ipolTanJerkTime(0.01);

                // 第二段
                double[] array2 = new[] { movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0, -movePositions.Point2.Pos.Z / 1000.0, movePositions.Point2.Angle / 360.0 };
                DsaVector vector2 = new DsaVector(array2);
                iGroup.ipolLine(vector2);
                 iGroup.ipolBeginConcatenation();

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);

                // iGroup.ipolTanJerkTime(0.01);

                // 第三段
                double[] array3 = new[] { movePositions.Point3.Pos.X / 1000.0, movePositions.Point3.Pos.Y / 1000.0, -movePositions.Point3.Pos.Z / 1000.0, movePositions.Point3.Angle / 360.0 };
                DsaVector vector3 = new DsaVector(array3);
                iGroup.ipolLine(vector3);
                 iGroup.ipolBeginConcatenation();

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);

                // iGroup.ipolTanJerkTime(0.01);

                // 第四段
                double[] array4 = new[] { movePositions.Point4.Pos.X / 1000.0, movePositions.Point4.Pos.Y / 1000.0, -movePositions.Point4.Pos.Z / 1000.0, movePositions.Point4.Angle / 360.0 };
                DsaVector vector4 = new DsaVector(array4);
                iGroup.ipolLine(vector4);
                 iGroup.ipolBeginConcatenation();

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);

                // iGroup.ipolTanJerkTime(0.01);

                // 第5段
                double[] array5 = new[] { movePositions.Point5.Pos.X / 1000.0, movePositions.Point5.Pos.Y / 1000.0, -movePositions.Point5.Pos.Z / 1000.0, movePositions.Point5.Angle / 360.0 };
                DsaVector vector5 = new DsaVector(array5);
                iGroup.ipolLine(vector5);

                 iGroup.ipolEndConcatenation();

                // 等待插补结束
                iGroup.ipolWaitMovement(100000);

                // 退出插补模式
                iGroup.ipolEnd();
            }
            catch (DsaException exc)
            {
                exc.diag(iGroup);
                if (moveDriveX.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveX.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveX.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveX.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveX.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveX.powerOff(60000);

                    /// Close the connection.
                    moveDriveX.close();
                }
                if (moveDriveY.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveY.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveY.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveY.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveY.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveY.powerOff(60000);

                    /// Close the connection.
                    moveDriveY.close();
                }
                if (dsaMaster.isOpen())
                {
                    dsaMaster.close();
                }
                return;
            }
        }

        /// <summary>
        /// UML 运动
        /// </summary>
        public void UmlMovement()
        {
            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)BondAxisY.AxisDrive).GetDrive();
            DsaDrive moveDriveZ = ((ETELAxis)BondAxisZ.AxisDrive).GetDrive();
            DsaDrive moveDriveT = ((ETELAxis)BondAxisT.AxisDrive).GetDrive();

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY, moveDriveZ, moveDriveT);

            // 设置控制者
            iGroup.setMaster(dsaMaster);

            // 开始插补
            iGroup.ipolBegin();

            // 设置为绝对坐标系 ，不设置绝对坐标系
            iGroup.ipolSetAbsMode(true, -1);

            // iGroup.ipolSetURelativeMode(false, -1);

            // 设置矢量速度  
            iGroup.ipolUSpeed(0.1);

            // 设置加速时间和加加速时间  加速时间 应该大于等于 2 * 加加速时间 
            iGroup.ipolUTime(0.25, 0.1);

           // 第一段,单位转成m要除以1000
            double[] array1 = new[] { movePositions.Point1.Pos.X / 1000.0, movePositions.Point1.Pos.Y / 1000.0, -movePositions.Point1.Pos.Z / 1000.0, movePositions.Point1.Angle / 360.0 };
            DsaVector vector1 = new DsaVector(array1);
            iGroup.ipolULine(vector1);

            // 第二段
            double[] array2 = new[] { movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0, -movePositions.Point2.Pos.Z / 1000.0, movePositions.Point2.Angle / 360.0 };
            DsaVector vector2 = new DsaVector(array2);
            iGroup.ipolULine(vector2);

            // 第三段
            double[] array3 = new[] { movePositions.Point3.Pos.X / 1000.0, movePositions.Point3.Pos.Y / 1000.0, -movePositions.Point3.Pos.Z / 1000.0, movePositions.Point3.Angle / 360.0 };
            DsaVector vector3 = new DsaVector(array3);
            iGroup.ipolULine(vector3);

            // 第四段
            double[] array4 = new[] { movePositions.Point4.Pos.X / 1000.0, movePositions.Point4.Pos.Y / 1000.0, -movePositions.Point4.Pos.Z / 1000.0, movePositions.Point4.Angle / 360.0 };
            DsaVector vector4 = new DsaVector(array4);
            iGroup.ipolULine(vector4);

            // 第5段
            double[] array5 = new[] { movePositions.Point5.Pos.X / 1000.0, movePositions.Point5.Pos.Y / 1000.0, -movePositions.Point5.Pos.Z / 1000.0, movePositions.Point5.Angle / 360.0 };
            DsaVector vector5 = new DsaVector(array5);
            iGroup.ipolULine(vector5);

            // 等待插补结束
            iGroup.ipolWaitMovement(100000);

            // 退出插补模式
            iGroup.ipolEnd();
        }


        public void Interpolation2()
        {
            Axis axisX = HardwareRepositoryService.GetHardware<Axis>("点胶X");
            Axis axisY = HardwareRepositoryService.GetHardware<Axis>("点胶Y");

            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)axisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)axisY.AxisDrive).GetDrive();

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY);

            // 设置控制者
            iGroup.setMaster(dsaMaster);

            // 开始插补
            iGroup.ipolBegin();

            // 设置为绝对坐标系 ，不设置绝对坐标系
            iGroup.ipolSetAbsMode(true, -1);

            // 开启画胶，打开点胶阀
            // dripElectric.SetOutputValue(true);



            // 连续插补
            //for (int j = 0; j < epoxyApplication.DispensePatternParas[i].Length; j++)
            List<MovePosition> mps = movePositions.GetMovePostions();

            for (int j = 0; j < mps.Count; j++)
            {
                // 由于第一段是到这个位置的参数，所以需要默认值
                //if (j == 0)
                {
                    iGroup.ipolTanVelocity(0.05);
                    iGroup.ipolTanAcceleration(0.05);
                    iGroup.ipolTanDeceleration(0.05);

                    // 设置抖动时间，不知道什么意思
                    iGroup.ipolTanJerkTime(0.01);
                }
                //else
                //{
                //    // 设置速度,Etel的单位为M/s，外接传入为mm/s,需要转换
                //    iGroup.ipolTanVelocity(mps[j].Vel / 1000.0);

                //    // 设置加速度
                //    iGroup.ipolTanAcceleration(epoxyApplication.DispensePatternParas[i][j - 1].Acc / 1000.0);
                //    iGroup.ipolTanDeceleration(epoxyApplication.DispensePatternParas[i][j - 1].Acc / 1000.0);

                //    // 设置抖动时间，不知道什么意思
                //    iGroup.ipolTanJerkTime(0.01);
                //}

                //if (epoxyApplication.DispensePatternParas[i][j].OpenGlueFlag)
                //{
                //    iGroup.ipolMark2Param(5, 5, 0x0000001, 1000);
                //}
                //else
                //{
                //    iGroup.ipolMark2Param(5, 5, 0x0010000, 1000);
                //}

                iGroup.ipolLine(mps[j].Pos.X / 1000, mps[j].Pos.Y / 1000);
                if (j == 0)
                    iGroup.ipolBeginConcatenation();
            }

            iGroup.ipolEndConcatenation();

            // 等待插补结束
            iGroup.ipolWaitMovement(100000);

            //// 点胶完成，关闭点胶阀IO
            //dripElectric.SetOutputValue(false);

            // 退出插补模式
            iGroup.ipolEnd();
        }


        /// <summary>
        /// 2轴连续插补
        /// </summary>
        /// <param name="isThreeParts">是否分三段</param>
        public void InterpolationMovementWithTwoAxis()
        {
            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)BondAxisY.AxisDrive).GetDrive();

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY);

            try
            {
                // 设置控制者
                iGroup.setMaster(dsaMaster);

                // 开始插补
                iGroup.ipolBegin();

                // 设置为绝对坐标系 ，不设置绝对坐标系
                iGroup.ipolSetAbsMode(true, -1);

                double speed = 0.05;
                double acceleration = 0.5;
                double deceleration = 0.5;

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);
   

                //// 第一段,单位转成m要除以1000
                iGroup.ipolLine(movePositions.Point1.Pos.X / 1000.0, movePositions.Point1.Pos.Y / 1000.0);
                iGroup.ipolBeginConcatenation();

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);
    

                // 第二段
                iGroup.ipolLine(movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0);

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);
                // iGroup.ipolTanJerkTime(0.01);


                // 第三段
                iGroup.ipolLine(movePositions.Point3.Pos.X / 1000.0, movePositions.Point3.Pos.Y / 1000.0);


                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);
      

                // 第四段
                iGroup.ipolLine(movePositions.Point4.Pos.X / 1000.0, movePositions.Point4.Pos.Y / 1000.0);

                // 设置速度
                iGroup.ipolTanVelocity(speed);
                iGroup.ipolTanAcceleration(acceleration);
                iGroup.ipolTanDeceleration(deceleration);
               iGroup.ipolTanJerkTime(0.01);

                // 第5段
                iGroup.ipolLine(movePositions.Point5.Pos.X / 1000.0, movePositions.Point5.Pos.Y / 1000.0);
                iGroup.ipolEndConcatenation();

                // 等待插补结束
                iGroup.ipolWaitMovement(100000);

                // 退出插补模式
                iGroup.ipolEnd();
            }
            catch (DsaException exc)
            {
                exc.diag(iGroup);
                if (moveDriveX.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveX.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveX.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveX.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveX.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveX.powerOff(60000);

                    /// Close the connection.
                    moveDriveX.close();
                }
                if (moveDriveY.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveY.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveY.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveY.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveY.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveY.powerOff(60000);

                    /// Close the connection.
                    moveDriveY.close();
                }
                if (dsaMaster.isOpen())
                {
                    dsaMaster.close();
                }
                return;
            }
        }

        /// <summary>
        /// 运动过程中轴不等待
        /// </summary>
        public void MovementWithoutWait()
        {
            //BondAxisZ.SendAbsoluteMoveCommand(MovePositions.EndPos.Z);

            //DateTime startTime = DateTime.Now;
            //DateTime endTime = DateTime.Now;
            //while (true)
            //{
            //    // 到达安全高度就发送运动指令
            //    if (BondAxisZ.GetRealPosition() >= MovePositions.ReadyPos.Z)
            //    {
            //        MotionService.MoveAxesToTargetPosition(
            //            (this.BondAxisX, true, MovePositions.EndPos.X, AccuracyMode.HighAccuracy),
            //            (this.BondAxisY, true, MovePositions.EndPos.Y, AccuracyMode.HighAccuracy),
            //            (this.BondAxisZ, true, MovePositions.EndPos.Z, AccuracyMode.HighAccuracy));

            //        break;
            //    }

            //    Thread.Sleep(0);

            //    // 超时报警
            //    endTime=DateTime.Now;
            //    if ((endTime - startTime).TotalSeconds > 5)
            //    {
            //        MessageBox.Show("Time  out！");
            //        break;
            //    }
            //}
        }



        /// <summary>
        /// 四轴PVT
        /// </summary>
        public void PVTWithFourAxis()
        {
            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)BondAxisY.AxisDrive).GetDrive();
            DsaDrive moveDriveZ = ((ETELAxis)BondAxisZ.AxisDrive).GetDrive();
            DsaDrive moveDriveT = ((ETELAxis)BondAxisT.AxisDrive).GetDrive();

            // 位置
            DsaVector destination = null;

            // 速度
            DsaVector velocity = null;

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY, moveDriveZ, moveDriveT);

            try
            {
                // 设置控制者
                iGroup.setMaster(dsaMaster);

                // 这里应该是运动到起点
                moveDriveX.setTargetPosition(0, movePositions.Point1.Pos.X / 1000.0);
                moveDriveY.setTargetPosition(0, movePositions.Point1.Pos.Y / 1000.0);
                moveDriveZ.setTargetPosition(0, movePositions.Point1.Pos.Z / 1000.0);
                moveDriveT.setTargetPosition(0, movePositions.Point1.Angle / 1000.0);


                // 等待
                iGroup.waitMovement(60000);

                // 开始插补
                iGroup.ipolBegin();

                // 第1点
                destination = new DsaVector(movePositions.Point1.Pos.X / 1000.0, movePositions.Point1.Pos.Y / 1000.0, movePositions.Point1.Pos.Z / 1000.0, movePositions.Point1.Angle / 1000.0);
                velocity = new DsaVector(movePositions.Point1.Vel.X, movePositions.Point1.Vel.Y, movePositions.Point1.Vel.Z, movePositions.Point1.TVel);
                double time = movePositions.Point1.Time;
                iGroup.ipolPvt(destination, velocity, time, 60000);

                // 第2点
                destination.set(movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0, movePositions.Point2.Pos.Z / 1000.0, movePositions.Point2.Angle / 1000.0);
                velocity.set(movePositions.Point2.Vel.X, movePositions.Point2.Vel.Y, movePositions.Point2.Vel.Z, movePositions.Point2.TVel);
                time = movePositions.Point2.Time;
                iGroup.ipolPvt(destination, velocity, time, 60000);

                // 第3点
                destination.set(movePositions.Point3.Pos.X / 1000.0, movePositions.Point3.Pos.Y / 1000.0, movePositions.Point3.Pos.Z / 1000.0, movePositions.Point3.Angle / 1000.0);
                velocity.set(movePositions.Point3.Vel.X, movePositions.Point3.Vel.Y, movePositions.Point3.Vel.Z, movePositions.Point3.TVel);
                time = movePositions.Point3.Time;
                iGroup.ipolPvt(destination, velocity, time, 60000);

                // 第4点
                destination.set(movePositions.Point4.Pos.X / 1000.0, movePositions.Point4.Pos.Y / 1000.0, movePositions.Point4.Pos.Z / 1000.0, movePositions.Point4.Angle / 1000.0);
                velocity.set(movePositions.Point4.Vel.X, movePositions.Point4.Vel.Y, movePositions.Point4.Vel.Z, movePositions.Point4.TVel);
                time = movePositions.Point4.Time;
                iGroup.ipolPvt(destination, velocity, time, 60000);

                // 第5点
                destination.set(movePositions.Point5.Pos.X / 1000.0, movePositions.Point5.Pos.Y / 1000.0, movePositions.Point5.Pos.Z / 1000.0, movePositions.Point5.Angle / 1000.0);
                velocity.set(movePositions.Point5.Vel.X, movePositions.Point5.Vel.Y, movePositions.Point5.Vel.Z, movePositions.Point5.TVel);
                time = movePositions.Point5.Time;
                iGroup.ipolPvt(destination, velocity, time, 60000);

                // 等运动执行完
                iGroup.ipolWaitMovement(60000);

                // 结束
                iGroup.ipolEnd();
            }
            catch (DsaException exc)
            {
                exc.diag(iGroup);
                if (moveDriveX.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveX.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveX.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveX.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveX.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveX.powerOff(60000);

                    /// Close the connection.
                    moveDriveX.close();
                }
                if (moveDriveY.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveY.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveY.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveY.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveY.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveY.powerOff(60000);

                    /// Close the connection.
                    moveDriveY.close();
                }
                if (dsaMaster.isOpen())
                {
                    dsaMaster.close();
                }
                return;
            }
        }

        /// <summary>
        /// 四轴PT
        /// </summary>
        public void PTWithFourAxis()
        {
            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)BondAxisY.AxisDrive).GetDrive();
            DsaDrive moveDriveZ = ((ETELAxis)BondAxisZ.AxisDrive).GetDrive();
            DsaDrive moveDriveT = ((ETELAxis)BondAxisT.AxisDrive).GetDrive();

            // 位置
            DsaVector destination = null;

            // 速度
            DsaVector velocity = null;

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY, moveDriveZ, moveDriveT);

            try
            {
                // 设置控制者
                iGroup.setMaster(dsaMaster);

                // 这里应该是运动到起点
                moveDriveX.setTargetPosition(0, movePositions.Point1.Pos.X / 1000.0);
                moveDriveY.setTargetPosition(0, movePositions.Point1.Pos.Y / 1000.0);
                moveDriveZ.setTargetPosition(0, movePositions.Point1.Pos.Z / 1000.0);
                moveDriveT.setTargetPosition(0, movePositions.Point1.Angle / 1000.0);


                // 等待
                iGroup.waitMovement(60000);

                // 开始插补
                iGroup.ipolBegin();

                // 第1点
                destination = new DsaVector(movePositions.Point1.Pos.X / 1000.0, movePositions.Point1.Pos.Y / 1000.0, movePositions.Point1.Pos.Z / 1000.0, movePositions.Point1.Angle / 1000.0);
                double time = movePositions.Point1.Time;
                iGroup.ipolPt(destination, time, 60000);

                // 第2点
                destination.set(movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0, movePositions.Point2.Pos.Z / 1000.0, movePositions.Point2.Angle / 1000.0);
                time = movePositions.Point2.Time;
                iGroup.ipolPt(destination, 60000);

                // 第3点
                destination.set(movePositions.Point3.Pos.X / 1000.0, movePositions.Point3.Pos.Y / 1000.0, movePositions.Point3.Pos.Z / 1000.0, movePositions.Point3.Angle / 1000.0);
                time = movePositions.Point3.Time;
                iGroup.ipolPt(destination, time, 60000);

                // 第4点
                destination.set(movePositions.Point4.Pos.X / 1000.0, movePositions.Point4.Pos.Y / 1000.0, movePositions.Point4.Pos.Z / 1000.0, movePositions.Point4.Angle / 1000.0);
                time = movePositions.Point4.Time;
                iGroup.ipolPt(destination, time, 60000);

                // 第5点
                destination.set(movePositions.Point5.Pos.X / 1000.0, movePositions.Point5.Pos.Y / 1000.0, movePositions.Point5.Pos.Z / 1000.0, movePositions.Point5.Angle / 1000.0);
                time = movePositions.Point5.Time;
                iGroup.ipolPt(destination, time, 60000);

                // 等运动执行完
                iGroup.ipolWaitMovement(60000);

                // 结束
                iGroup.ipolEnd();
            }
            catch (DsaException exc)
            {
                exc.diag(iGroup);
                if (moveDriveX.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveX.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveX.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveX.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveX.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveX.powerOff(60000);

                    /// Close the connection.
                    moveDriveX.close();
                }
                if (moveDriveY.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveY.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveY.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveY.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveY.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveY.powerOff(60000);

                    /// Close the connection.
                    moveDriveY.close();
                }
                if (dsaMaster.isOpen())
                {
                    dsaMaster.close();
                }
                return;
            }
        }

        /// Define vectors used to specify the destination point and velocity of a
        /// PVT segment
        DsaVector destination = null;
        DsaVector velocity = null;

        /// Define some global variable.
        double time;

        /// <summary>
        /// 四轴PVT
        /// </summary>
        public void PVTWithTwoAxis()
        {
            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)BondAxisY.AxisDrive).GetDrive();

            // 位置
            DsaVector destination = null;

            // 速度
            DsaVector velocity = null;

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup igrp = new DsaIpolGroup(moveDriveX, moveDriveY);
          
            try
            {
                // 设置控制者
                igrp.setMaster(dsaMaster);

                // 开始插补
                igrp.ipolBegin();
                igrp.ipolSetAbsMode(true, -1);

                /// PVT command is defined by its destination point, the velocity at this
                /// point, and the time of the displacement.
                /// The trajectory defined here is quite similar to the one described at
                /// page 52 of the ultimet User's Manual.

                /// Define the first PVT destination point, velocity and time:
                /// x    = 10.0e-3 m
                /// vx   = 50e-3 m/s
                /// y    = 0 m
                /// vy   = 0 m/s
                /// time = 200.0e-3 s
                destination = new DsaVector(10.0e-3, 0, 0, 0);
                velocity = new DsaVector(50.0e-3, 0, 0, 0);
                time = 1;

                /// Add this PVT segment to the trajectory path. After this function call, 
                /// the interpolator will directly start the movement along this segment
                /// and the next ones.
                igrp.ipolPvt(destination, velocity, time, 60000);

                /// Define the 2nd PVT destination point, velocity and time: 
                /// x    = 110.0e-3 m
                /// vx   = 50e-3 m/s
                /// y    = 0 m
                /// vy   = 0 m/s
                /// time = 2.0 s
                destination.set(60.0e-3, 0, 0, 0);    // X 右移动100
                velocity.set(30.0e-3, 0, 0, 0);
                time = 1;
                /// Add this PVT segment to the trajectory path.
                igrp.ipolPvt(destination, velocity, time, 60000);

                // Define and add some other PVT segments:
                destination.set(90.0e-3, 0, 0, 0);
                velocity.set(50.0e-3, 0, 0, 0);
                time = 1.0;
                igrp.ipolPvt(destination, velocity, time, 60000);

                destination.set(110.0e-3, 100.0e-3, 0, 0);   // // Y 前移动100
                velocity.set(150.0e-3, 150.0e-3, 0, 0);
                time = 1;
                igrp.ipolPvt(destination, velocity, time, 60000);

                destination.set(0.0e-3, 0.0e-3, 0, 0);
                velocity.set(100.0e-3, 100.0e-3, 0, 0);
                time = 1.0;
                igrp.ipolPvt(destination, velocity, time, 60000);

                destination.set(-50.0e-3, -50.0e-3, 0, 0);   //  X 左移动100
                velocity.set(0, 0, 0, 0);
                time = 2.0;
                igrp.ipolPvt(destination, velocity, time, 60000);

                //destination.set(10.0e-3, 100.0e-3, 0, 0);
                //velocity.set(0, 50.0e-3, 0, 0);
                //time = 1.0;
                //igrp.ipolPvt(destination, velocity, time, 60000);

                //destination.set(10.0e-3, 0, 0, 0);
                //velocity.set(0, 50.0e-3, 0, 0);
                //time = 2.0;
                //igrp.ipolPvt(destination, velocity, time, 60000);

                //destination.set(10.0e-3, -10e-3, 0, 0);
                //velocity.set(0, 0, 0, 0);
                //time = 1;
                //igrp.ipolPvt(destination, velocity, time, 60000);

                //// 第1点
                //destination = new DsaVector(movePositions.Point1.Pos.X / 1000.0, movePositions.Point1.Pos.Y / 1000.0, 0, 0);
                //velocity = new DsaVector(movePositions.Point1.Vel.X / 1000.0, movePositions.Point1.Vel.Y / 1000.0, 0, 0);
                //double time = movePositions.Point1.Time;
                //iGroup.ipolPvt(destination, velocity, time, 60000);


                //iGroup.ipolBeginConcatenation();

                //// 第2点
                //destination.set(movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0, 0, 0);
                //velocity.set(movePositions.Point2.Vel.X / 1000.0, movePositions.Point2.Vel.Y / 1000.0, 0, 0);
                //time = movePositions.Point2.Time;
                //iGroup.ipolPvt(destination, velocity, time, 60000);

                //iGroup.ipolBeginConcatenation();

                //// 第3点
                //destination.set(movePositions.Point3.Pos.X / 1000.0, movePositions.Point3.Pos.Y / 1000.0, 0, 0);
                //velocity.set(movePositions.Point3.Vel.X / 1000.0, movePositions.Point3.Vel.Y / 1000.0, 0, 0);
                //time = movePositions.Point3.Time;
                //iGroup.ipolPvt(destination, velocity, time, 60000);

                //// 第4点
                //destination.set(movePositions.Point4.Pos.X / 1000.0, movePositions.Point4.Pos.Y / 1000.0, 0, 0);
                //velocity.set(movePositions.Point4.Vel.X / 1000.0, movePositions.Point4.Vel.Y / 1000.0, 0, 0);
                //time = movePositions.Point4.Time;
                //iGroup.ipolPvt(destination, velocity, time, 60000);

                //// 第5点
                //destination.set(movePositions.Point5.Pos.X / 1000.0, movePositions.Point5.Pos.Y / 1000.0, 0, 0);
                //velocity.set(movePositions.Point5.Vel.X / 1000.0, movePositions.Point5.Vel.Y / 1000.0, 0, 0);
                //time = movePositions.Point5.Time;
                //iGroup.ipolPvt(destination, velocity, time, 60000);

                // 等运动执行完
                igrp.ipolWaitMovement(60000);

                // 结束
                igrp.ipolEnd();
            }
            catch (DsaException exc)
            {
                exc.diag(igrp);
                if (moveDriveX.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveX.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveX.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveX.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveX.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveX.powerOff(60000);

                    /// Close the connection.
                    moveDriveX.close();
                }
                if (moveDriveY.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveY.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveY.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveY.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveY.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveY.powerOff(60000);

                    /// Close the connection.
                    moveDriveY.close();
                }
                if (dsaMaster.isOpen())
                {
                    dsaMaster.close();
                }
                return;
            }
        }

        /// <summary>
        /// 四轴PT
        /// </summary>
        public void PTWithTwoAxis()
        {
            HardwareRepositoryService.GetHardware<Axis>("BondX").AbsoluteMove(0);
            HardwareRepositoryService.GetHardware<Axis>("BondY").AbsoluteMove(0);
            // 获取X，Y轴驱动
            DsaDrive moveDriveX = ((ETELAxis)BondAxisX.AxisDrive).GetDrive();
            DsaDrive moveDriveY = ((ETELAxis)BondAxisY.AxisDrive).GetDrive();

            // 位置
            DsaVector destination = null;

            // 速度
            DsaVector velocity = null;

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(moveDriveX, moveDriveY);

            try
            {
                // 设置控制者
                iGroup.setMaster(dsaMaster);

                // 开始插补
                iGroup.ipolBegin();

                // 设置为绝对坐标系 ，不设置绝对坐标系
                iGroup.ipolSetAbsMode(true, -1);

                // 第1点
                destination = new DsaVector(movePositions.Point1.Pos.X / 1000.0, movePositions.Point1.Pos.Y / 1000.0, 0, 0);
                double time = movePositions.Point1.Time;
                iGroup.ipolPt(destination, time, 60000);

                iGroup.ipolBeginConcatenation();
                destination = new DsaVector(movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0, 0, 0);
                // 第2点
                // destination.set(movePositions.Point2.Pos.X / 1000.0, movePositions.Point2.Pos.Y / 1000.0, 0, 0);
                time = movePositions.Point2.Time;
                iGroup.ipolPt(destination, time, 60000);



                //// 第3点
                //destination.set(movePositions.Point3.Pos.X / 1000.0, movePositions.Point3.Pos.Y / 1000.0, 0, 0);
                //time = movePositions.Point3.Time;
                //iGroup.ipolPt(destination, time, 60000);



                //// 第4点
                //destination.set(movePositions.Point4.Pos.X / 1000.0, movePositions.Point4.Pos.Y / 1000.0, 0, 0);
                //time = movePositions.Point4.Time;
                //iGroup.ipolPt(destination, time, 60000);


                //// 第5点
                //destination.set(movePositions.Point5.Pos.X / 1000.0, movePositions.Point5.Pos.Y / 1000.0, 0, 0);
                //time = movePositions.Point5.Time;
                //iGroup.ipolPt(destination, time, 60000);

                // 等运动执行完
                iGroup.ipolWaitMovement(60000);

                // 结束
                iGroup.ipolEnd();
            }
            catch (DsaException exc)
            {
                exc.diag(iGroup);
                if (moveDriveX.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveX.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveX.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveX.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveX.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveX.powerOff(60000);

                    /// Close the connection.
                    moveDriveX.close();
                }
                if (moveDriveY.isOpen())
                {
                    /// We can check if a motor is moving by reading the status of the drive. 
                    if (moveDriveY.getStatus().isMoving())
                    {
                        /// The drive is moving => 
                        /// Stop it immediately (Dsa.QS_BYPASS)
                        /// Stopping its sequence (Dsa.QS_STOP_SEQUENCE)
                        /// With programmed deceleration (Dsa.QS_PROGRAMMED_DEC)
                        moveDriveY.quickStop(Dsa.QS_PROGRAMMED_DEC, Dsa.QS_BYPASS | Dsa.QS_STOP_SEQUENCE);
                        ///Wait that motor is stopped
                        moveDriveY.waitMovement(60000);
                    }

                    /// We can check if motor is powered on by reading the status of the drive. 
                    if (moveDriveY.getStatus().isPowerOn())
                        /// The drive is powered on => 
                        /// Power it off
                        moveDriveY.powerOff(60000);

                    /// Close the connection.
                    moveDriveY.close();
                }
                if (dsaMaster.isOpen())
                {
                    dsaMaster.close();
                }
                return;
            }

        }
    }
}
