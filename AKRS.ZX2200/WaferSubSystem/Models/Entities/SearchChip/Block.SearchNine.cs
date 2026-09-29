using System;
using System.Diagnostics;
using System.Drawing;
using AKRS.Galaxy2.Machine.Models;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;

    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Structs;

    using DevExpress.XtraEditors;
    using LanguageExt;

    /// <summary>
    /// 芯片搜索类
    /// </summary>
    public partial class Block
    {
        /// <summary>
        /// 搜索9颗芯片
        /// </summary>
        /// <returns>result</returns>
        private bool SearchNine()
        {
            try
            {
                #region 搜9颗的搜索方式
                if (this.isFirstSearch)
                {
                    // 第一颗搜当前位置的晶片
                    this.isFirstSearch = false;
                    this.nextDie.Pos = this.nineDie.CenterDie.Pos;
                    this.nextDie.Exist = false;
                    this.countBlankNum = 0;
                    return this.AnalyseDies();
                }
                else if (this.nineDie.UpDie.Exist)
                {
                    // 上面有芯片先往上
                    // 记下下一行的芯片位置
                    this.MarkNextRowDie();

                    // 此芯片的标记为存在则可以在移到后就可以取不需要定位确认存在
                    this.nextDie = this.nineDie.UpDie;
                    this.countBlankNum = 0;
                    this.turnUpRowNum = this.currentRowNum;
                    this.currentRowNum--;
                    return this.AnalyseDies();
                }

                if (this.currentRowNum == this.turnUpRowNum - 1)
                {
                    if (this.CurrentRowDirection == RowDirection.LeftToRight && !this.nineDie.RightCenterDie.Exist && this.nineDie.DownDie.Exist)
                    {
                        this.countBlankNum = this.RecurLimit - 40;
                    }
                    else if (this.CurrentRowDirection == RowDirection.RightToLeft && !this.nineDie.LeftCenterDie.Exist && this.nineDie.DownDie.Exist)
                    {
                        this.countBlankNum = this.RecurLimit - 40;
                    }
                }

                if (this.countBlankNum >= this.RecurLimit - 40)
                {
                    #region 空晶片已超或是跳起的一行没晶片了
                    if (this.CurrentRowDirection == RowDirection.LeftToRight)
                    {
                        #region 从左向右
                        // 右边有芯片
                        if (this.nineDie.RightCenterDie.Exist)
                        {
                            this.nextDie = this.nineDie.RightCenterDie;

                            // 记下下一行芯片位置
                            this.MarkNextRowDie();
                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.RightUpDie.Exist)
                        {
                            // 右上有芯片
                            this.nextDie = this.nineDie.RightUpDie;
                            this.MarkNextRowDie();
                            this.turnUpRowNum = this.currentRowNum;
                            this.currentRowNum--;
                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.DownDie.Exist)
                        {
                            // 下面有芯片
                            this.nextDie = this.nineDie.DownDie;
                            this.MarkTDie();
                            this.currentRowNum++;
                            if (this.currentRowNum != this.turnUpRowNum)
                            {
                                this.CurrentRowDirection = RowDirection.RightToLeft;
                            }

                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.RightDownDie.Exist)
                        {
                            // 右下有芯片
                            this.nextDie = this.nineDie.RightDownDie;
                            this.MarkTDie();
                            this.currentRowNum++;
                            if (this.currentRowNum != this.turnUpRowNum)
                            {
                                this.CurrentRowDirection = RowDirection.RightToLeft;
                            }

                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.LeftDownDie.Exist)
                        {
                            // 左下有芯片
                            this.nextDie = this.nineDie.LeftDownDie;
                            this.MarkTDie();
                            this.currentRowNum++;
                            if (this.currentRowNum != this.turnUpRowNum)
                            {
                                this.CurrentRowDirection = RowDirection.RightToLeft;
                            }

                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        #endregion
                    }
                    else
                    {
                        #region 从右向左
                        // 左边有芯片
                        if (this.nineDie.LeftCenterDie.Exist)
                        {
                            this.nextDie = this.nineDie.LeftCenterDie;

                            // 记下下一行芯片位置
                            this.MarkNextRowDie();
                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.LeftUpDie.Exist)
                        {
                            // 左上有芯片
                            this.nextDie = this.nineDie.LeftUpDie;
                            this.MarkNextRowDie();
                            this.turnUpRowNum = this.currentRowNum;
                            this.currentRowNum--;
                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.DownDie.Exist)
                        {
                            // 下面有芯片
                            this.nextDie = this.nineDie.DownDie;
                            this.MarkTDie();
                            this.currentRowNum++;
                            if (this.currentRowNum != this.turnUpRowNum)
                            {
                                this.CurrentRowDirection = RowDirection.LeftToRight;
                            }

                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.LeftDownDie.Exist)
                        {
                            // 左下有芯片
                            this.nextDie = this.nineDie.LeftDownDie;
                            this.MarkTDie();
                            this.currentRowNum++;
                            if (this.currentRowNum != this.turnUpRowNum)
                            {
                                this.CurrentRowDirection = RowDirection.LeftToRight;
                            }

                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        else if (this.nineDie.RightDownDie.Exist)
                        {
                            // 右下有芯片
                            this.nextDie = this.nineDie.RightDownDie;
                            this.MarkTDie();
                            this.currentRowNum++;
                            if (this.currentRowNum != this.turnUpRowNum)
                            {
                                this.CurrentRowDirection = RowDirection.LeftToRight;
                            }

                            this.countBlankNum = 0;
                            return this.AnalyseDies();
                        }
                        #endregion
                    }
                    #region 9颗找不到时找记下位置的芯片
                    if (this.NextRowStartDie.DieInfo.Exist)
                    {
                        this.NextRowStartDie.DieInfo.Exist = false;
                        this.nextDie = this.NextRowStartDie.DieInfo;
                        this.currentRowNum = this.NextRowStartDie.RowNum;
                        this.CurrentRowDirection = this.NextRowStartDie.RowDirection;
                        this.countBlankNum = 0;
                        return this.AnalyseDies();
                    }
                    else if (this.tDie.DieInfo.Exist)
                    {
                        this.tDie.DieInfo.Exist = false;
                        this.nextDie = this.tDie.DieInfo;
                        this.currentRowNum = this.tDie.RowNum;
                        this.CurrentRowDirection = this.tDie.RowDirection;
                        this.countBlankNum = 0;
                        return this.AnalyseDies();
                    }
                    else if (this.BiggestRowStartDie.DieInfo.Exist)
                    {
                        this.BiggestRowStartDie.DieInfo.Exist = false;
                        this.nextDie = this.BiggestRowStartDie.DieInfo;
                        this.currentRowNum = this.BiggestRowStartDie.RowNum;
                        this.CurrentRowDirection = this.BiggestRowStartDie.RowDirection;
                        this.countBlankNum = 0;
                        return this.AnalyseDies();
                    }
                    else
                    {
                        this.countBlankNum = 0;
                        this.StartInitNine();
                        return false;
                    }
                    #endregion
                    #endregion
                }
                else
                {
                    #region 空晶片未超
                    if (this.CurrentRowDirection == RowDirection.LeftToRight)
                    {
                        this.nextDie = this.nineDie.RightCenterDie;
                    }
                    else
                    {
                        this.nextDie = this.nineDie.LeftCenterDie;
                    }

                    // 记下分叉晶片
                    this.MarkNagativeDirTDie();

                    // 记下下一行芯片位置
                    this.MarkNextRowDie();

                    return this.AnalyseDies();
                    #endregion
                }
                #endregion
            }
            catch (PickRefEnd e)
            {
                this.isNeedChangeCarrier = true;
                this.RecurCount = 0;
                WaferTableDevicePara.CurrentTablet.SlotState = SlotStatuEnum.Bad;
                WaferSubDevicePara.GetInstance().Save();
                return false;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// AnalyseDies
        /// </summary>
        /// <returns>result</returns>
        private bool AnalyseDies()
        {
            try
            {
                this.StopSearch();

                // 在范围内
                if (this.IsInSearchRange(this.nextDie.Pos))
                {
                    if (this.nextDie.Exist)
                    {
                        // 确认存在的晶片视觉定位确认前就可以取
                        this.RecurCount = 0;
                        this.currentDie = this.nextDie;
                        this.matchResult = this.currentDie.MatchResult;
                        this.MoveDieToPickCenter();
                        return true;
                    }
                    else
                    {
                        this.CalculateDiesPosition();
                        if (this.nineDie.CenterDie.Exist)
                        {
                            this.nextDie = this.nineDie.CenterDie;

                            // 超出晶圆范围
                            if (!this.IsInSearchRange(this.nextDie.Pos))
                            {
                                return this.NextDie();
                            }

                            this.currentDie = this.nextDie;
                            this.RecurCount = 0;
                            this.matchResult = this.currentDie.MatchResult;
                            this.MoveDieToPickCenter();
                            return true;
                        }
                        else
                        {
                            if (this.RecurCount > this.RecurLimit)
                            {
                                //AKRSMessageBoxExt.Show(
                                //    $"Block search nine recurCount is out limit - {this.RecurLimit}!",
                                //    "Error",
                                //    new string[] { "OK" },
                                //    new DialogResult[] { DialogResult.OK });
                                AKRSMessageBoxExt.Show(
                                    $"智能搜索超出递归次数 - {this.RecurLimit}!",
                                    "Error",
                                    new string[] { "OK" },
                                    new DialogResult[] { DialogResult.OK });
                                throw new PickRefEnd();
                            }

                            this.countBlankNum++;
                            this.RecurCount++;
                            
                            return this.NextDie();
                        }
                    }
                }
                else
                {
                    if (this.RecurCount > this.RecurLimit)
                    {
                        //AKRSMessageBoxExt.Show(
                        //    $"Block search nine recurCount is out limit - {this.RecurLimit}!",
                        //    "Error",
                        //    new string[] { "OK" },
                        //    new DialogResult[] { DialogResult.OK });
                        AKRSMessageBoxExt.Show(
                            $"智能搜索超出递归次数 - {this.RecurLimit}!",
                            "Error",
                            new string[] { "OK" },
                            new DialogResult[] { DialogResult.OK });
                        throw new PickRefEnd();
                    }

                    this.countBlankNum++;
                    this.RecurCount++;
                    
                    return this.NextDie();
                }
            }
            catch (PickRefEnd e)
            {
                throw new PickRefEnd();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 分析9颗芯片
        /// </summary>
        private void CalculateDiesPosition()
        {
            this.ResetNineDiesData();
            (bool isSucceed, MatchResult[] matchResults) result = this.MatchResult();
            if (result.isSucceed)
            {
                AKRSPoint3D[] result3D = MatchResultToWorld(result.matchResults);
                for (int i = 0; i < result.matchResults.Length; i++)
                {
                    SingleDieInfo die = new SingleDieInfo()
                    {
                        Exist = true,
                        Pos = this.WaferTableRealPositionToG0 - result3D[i],
                        MatchResult = result.matchResults[i],
                    };

                    // 超出范围的当无芯片处理
                    if (this.IsInSearchRange(die.Pos))
                    {
                        double xPos = die.Pos.X;
                        double yPos = die.Pos.Y;

                        // 计算与理论位置的偏差距离
                        double disOffsetLeftUp = this.Distance(xPos, yPos, this.nineDie.LeftUpDie.Pos);
                        double disOffsetUp = this.Distance(xPos, yPos, this.nineDie.UpDie.Pos);
                        double disOffsetRightUp = this.Distance(xPos, yPos, this.nineDie.RightUpDie.Pos);
                        double disOffsetLeft = this.Distance(xPos, yPos, this.nineDie.LeftCenterDie.Pos);
                        double disOffset = this.Distance(xPos, yPos, this.nineDie.CenterDie.Pos);
                        double disOffsetRight = this.Distance(xPos, yPos, this.nineDie.RightCenterDie.Pos);
                        double disOffsetLeftDown = this.Distance(xPos, yPos, this.nineDie.LeftDownDie.Pos);
                        double disOffsetDown = this.Distance(xPos, yPos, this.nineDie.DownDie.Pos);
                        double disOffsetRightDown = this.Distance(xPos, yPos, this.nineDie.RightDownDie.Pos);
                        double[] value = new double[] { disOffsetLeftUp, disOffsetUp, disOffsetRightUp, disOffsetLeft, disOffset, disOffsetRight, disOffsetLeftDown, disOffsetDown, disOffsetRightDown };
                        double minDisOffset = value.Min();

                        if (minDisOffset == disOffsetLeftUp)
                        {
                            this.nineDie.LeftUpDie = die;
                        }
                        else if (minDisOffset == disOffsetUp)
                        {
                            this.nineDie.UpDie = die;
                        }
                        else if (minDisOffset == disOffsetRightUp)
                        {
                            this.nineDie.RightUpDie = die;
                        }
                        else if (minDisOffset == disOffsetLeft)
                        {
                            this.nineDie.LeftCenterDie = die;
                        }
                        else if (minDisOffset == disOffset)
                        {
                            this.nineDie.CenterDie = die;
                        }
                        else if (minDisOffset == disOffsetRight)
                        {
                            this.nineDie.RightCenterDie = die;
                        }
                        else if (minDisOffset == disOffsetLeftDown)
                        {
                            this.nineDie.LeftDownDie = die;
                        }
                        else if (minDisOffset == disOffsetDown)
                        {
                            this.nineDie.DownDie = die;
                        }
                        else
                        {
                            // disOffsetRightDown
                            this.nineDie.RightDownDie = die;
                        }
                    }
                }

                if (this.currentCarrierConfig.IsInkDotSearch)
                {
                    PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.DieBlobName);
                    if (prEntity != null)
                    {
                        // 设置光源
                        prEntity.SetLight();

                        // 采图
                        Bitmap bmp = prEntity.Photograph(prEntity.CameraName);

                        this.DetectInkDot(this.nineDie.LeftUpDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.UpDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.RightUpDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.LeftCenterDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.CenterDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.RightCenterDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.LeftDownDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.DownDie, bmp, prEntity);
                        this.DetectInkDot(this.nineDie.RightDownDie, bmp, prEntity);

                        bmp.Dispose();
                    }
                    else
                    {
                        //throw new Exception($"The template-{this.DieBlobName} is not exist.");
                        throw new Exception($"视觉模板-{this.DieBlobName} 不存在！");
                    }
                }
            }
        }

        /// <summary>
        /// 检测墨点
        /// </summary>
        /// <param name="singleDieInfo">区域</param>
        /// <param name="bmp">图像</param>
        /// <param name="prEntity">pr实体</param>
        private void DetectInkDot(SingleDieInfo singleDieInfo, Bitmap bmp, PREntity prEntity)
        {
            if (singleDieInfo.Exist)
            {
                // 设置位置修正系数
                prEntity.Alg.SetFix((float)singleDieInfo.MatchResult.CenterX, (float)singleDieInfo.MatchResult.CenterY, (float)singleDieInfo.MatchResult.Angle);

                // 执行墨点检测
                ExcuteResult prResult = prEntity.DoWork(bmp);
                if (prResult == ExcuteResult.Success)
                {
                    List<BaseAlgResult> matchResults = prEntity.AlgResults;
                    if (matchResults[0].IsSuccess)
                    {
                        // 有墨点
                        singleDieInfo.Exist = false;
                    }
                    else
                    {
                        // 无墨点
                    }
                }
                else
                {
                    // 执行不成功当墨点处理
                    singleDieInfo.Exist = false;
                }
            }
        }

        /// <summary>
        /// 重置9颗数据
        /// </summary>
        private void ResetNineDiesData()
        {
            this.nineDie.CenterDie.Exist = false;
            this.nineDie.LeftUpDie.Exist = false;
            this.nineDie.UpDie.Exist = false;
            this.nineDie.RightUpDie.Exist = false;
            this.nineDie.LeftCenterDie.Exist = false;
            this.nineDie.RightCenterDie.Exist = false;
            this.nineDie.LeftDownDie.Exist = false;
            this.nineDie.DownDie.Exist = false;
            this.nineDie.RightDownDie.Exist = false;

            this.nineDie.CenterDie.Pos = this.WaferTableRealPositionToG0;

            AKRSPoint3D leftUpDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X + this.CarrierColSpacing.X + this.CarrierRowSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y + this.CarrierColSpacing.Y + this.CarrierRowSpacing.Y,
                Z = 0
            };
            AKRSPoint3D upDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X + this.CarrierRowSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y + this.CarrierRowSpacing.Y,
                Z = 0
            };
            AKRSPoint3D rightUpDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X - this.CarrierColSpacing.X + this.CarrierRowSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y - this.CarrierColSpacing.Y + this.CarrierRowSpacing.Y,
                Z = 0
            };
            AKRSPoint3D leftCenterDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X + this.CarrierColSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y + this.CarrierColSpacing.Y,
                Z = 0
            };
            AKRSPoint3D rightCenterDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X - this.CarrierColSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y - this.CarrierColSpacing.Y,
                Z = 0
            };
            AKRSPoint3D leftDownDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X + this.CarrierColSpacing.X - this.CarrierRowSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y + this.CarrierColSpacing.Y - this.CarrierRowSpacing.Y,
                Z = 0
            };
            AKRSPoint3D downDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X - this.CarrierRowSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y - this.CarrierRowSpacing.Y,
                Z = 0
            };
            AKRSPoint3D rightDownDiePos = new AKRSPoint3D()
            {
                X = this.WaferTableRealPositionToG0.X - this.CarrierColSpacing.X - this.CarrierRowSpacing.X,
                Y = this.WaferTableRealPositionToG0.Y - this.CarrierColSpacing.Y - this.CarrierRowSpacing.Y,
                Z = 0
            };
            if (this.NineSearchDirection == NineSearchDirection.NineUpToDown)
            {
                this.nineDie.LeftUpDie.Pos = leftUpDiePos;
                this.nineDie.UpDie.Pos = upDiePos;
                this.nineDie.RightUpDie.Pos = rightUpDiePos;
                this.nineDie.LeftCenterDie.Pos = leftCenterDiePos;
                this.nineDie.RightCenterDie.Pos = rightCenterDiePos;
                this.nineDie.LeftDownDie.Pos = leftDownDiePos;
                this.nineDie.DownDie.Pos = downDiePos;
                this.nineDie.RightDownDie.Pos = rightDownDiePos;
            }
            else if (this.NineSearchDirection == NineSearchDirection.NineLeftToRight)
            {
                this.nineDie.LeftUpDie.Pos = leftDownDiePos;
                this.nineDie.UpDie.Pos = leftCenterDiePos;
                this.nineDie.RightUpDie.Pos = leftUpDiePos;
                this.nineDie.LeftCenterDie.Pos = downDiePos;
                this.nineDie.RightCenterDie.Pos = upDiePos;
                this.nineDie.LeftDownDie.Pos = rightDownDiePos;
                this.nineDie.DownDie.Pos = rightCenterDiePos;
                this.nineDie.RightDownDie.Pos = rightUpDiePos;
            }
            else if (this.NineSearchDirection == NineSearchDirection.NineDownToUp)
            {
                this.nineDie.LeftUpDie.Pos = rightDownDiePos;
                this.nineDie.UpDie.Pos = downDiePos;
                this.nineDie.RightUpDie.Pos = leftDownDiePos;
                this.nineDie.LeftCenterDie.Pos = rightCenterDiePos;
                this.nineDie.RightCenterDie.Pos = leftCenterDiePos;
                this.nineDie.LeftDownDie.Pos = rightUpDiePos;
                this.nineDie.DownDie.Pos = upDiePos;
                this.nineDie.RightDownDie.Pos = leftUpDiePos;
            }
            else
            {
                this.nineDie.LeftUpDie.Pos = rightUpDiePos;
                this.nineDie.UpDie.Pos = rightCenterDiePos;
                this.nineDie.RightUpDie.Pos = rightDownDiePos;
                this.nineDie.LeftCenterDie.Pos = upDiePos;
                this.nineDie.RightCenterDie.Pos = downDiePos;
                this.nineDie.LeftDownDie.Pos = leftUpDiePos;
                this.nineDie.DownDie.Pos = leftCenterDiePos;
                this.nineDie.RightDownDie.Pos = leftDownDiePos;
            }
        }

        /// <summary>
        /// 标记下一行芯片
        /// </summary>
        private void MarkNextRowDie()
        {
            if (this.nineDie.DownDie.Exist)
            {
                if ((this.BiggestRowStartDie.DieInfo.Exist && this.currentRowNum >= this.BiggestRowStartDie.RowNum - 1) || !this.BiggestRowStartDie.DieInfo.Exist)
                {
                    // 记下的最下一行已经被超越了,更新最下一行的起始点
                    this.BiggestRowStartDie.RowNum = this.currentRowNum + 1;
                    this.BiggestRowStartDie.DieInfo = this.nineDie.DownDie;
                    this.BiggestRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                }

                this.NextRowStartDie.RowNum = this.currentRowNum + 1;
                this.NextRowStartDie.DieInfo = this.nineDie.DownDie;
                this.NextRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
            }
            else if (this.nineDie.RightDownDie.Exist || this.nineDie.LeftDownDie.Exist)
            {
                #region 记下左下或右下的位置
                if (this.CurrentRowDirection == RowDirection.LeftToRight)
                {
                    if (this.nineDie.RightDownDie.Exist)
                    {
                        if ((this.BiggestRowStartDie.DieInfo.Exist && this.currentRowNum >= this.BiggestRowStartDie.RowNum - 1) || !this.BiggestRowStartDie.DieInfo.Exist)
                        {
                            // 记下的最下一行已经被超越了
                            this.BiggestRowStartDie.RowNum = this.currentRowNum + 1;
                            this.BiggestRowStartDie.DieInfo = this.nineDie.RightDownDie;
                            this.BiggestRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                        }

                        this.NextRowStartDie.RowNum = this.currentRowNum + 1;
                        this.NextRowStartDie.DieInfo = this.nineDie.RightDownDie;
                        this.NextRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                    }
                    else if (this.nineDie.LeftDownDie.Exist)
                    {
                        if ((this.BiggestRowStartDie.DieInfo.Exist && this.currentRowNum >= this.BiggestRowStartDie.RowNum - 1) || !this.BiggestRowStartDie.DieInfo.Exist)
                        {
                            // 记下的最下一行已经被超越了
                            this.BiggestRowStartDie.RowNum = this.currentRowNum + 1;
                            this.BiggestRowStartDie.DieInfo = this.nineDie.LeftDownDie;
                            this.BiggestRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                        }

                        this.NextRowStartDie.RowNum = this.currentRowNum + 1;
                        this.NextRowStartDie.DieInfo = this.nineDie.LeftDownDie;
                        this.NextRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                    }
                }
                else
                {
                    if (this.nineDie.LeftDownDie.Exist)
                    {
                        if ((this.BiggestRowStartDie.DieInfo.Exist && this.currentRowNum >= this.BiggestRowStartDie.RowNum - 1) || !this.BiggestRowStartDie.DieInfo.Exist)
                        {
                            // 记下的最下一行已经被超越了
                            this.BiggestRowStartDie.RowNum = this.currentRowNum + 1;
                            this.BiggestRowStartDie.DieInfo = this.nineDie.LeftDownDie;
                            this.BiggestRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                        }

                        this.NextRowStartDie.RowNum = this.currentRowNum + 1;
                        this.NextRowStartDie.DieInfo = this.nineDie.LeftDownDie;
                        this.NextRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                    }
                    else if (this.nineDie.RightDownDie.Exist)
                    {
                        if ((this.BiggestRowStartDie.DieInfo.Exist && this.currentRowNum >= this.BiggestRowStartDie.RowNum - 1) || !this.BiggestRowStartDie.DieInfo.Exist)
                        {
                            // 记下的最下一行已经被超越了
                            this.BiggestRowStartDie.RowNum = this.currentRowNum + 1;
                            this.BiggestRowStartDie.DieInfo = this.nineDie.RightDownDie;
                            this.BiggestRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                        }

                        this.NextRowStartDie.RowNum = this.currentRowNum + 1;
                        this.NextRowStartDie.DieInfo = this.nineDie.RightDownDie;
                        this.NextRowStartDie.RowDirection = this.CurrentRowDirection == RowDirection.LeftToRight ? RowDirection.RightToLeft : RowDirection.LeftToRight;
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// 标记向下换行时分叉晶片
        /// </summary>
        private void MarkTDie()
        {
            if (this.CurrentRowDirection == RowDirection.LeftToRight)
            {
                if (this.currentRowNum != this.turnUpRowNum - 1)
                {
                    // 记下分叉时的芯片
                    if (this.nineDie.RightDownDie.Exist)
                    {
                        this.tDie.DieInfo = this.nineDie.RightDownDie;
                        this.tDie.RowNum = this.currentRowNum + 1;
                        this.tDie.RowDirection = RowDirection.LeftToRight;
                    }
                }
                else
                {
                    // 若往上一行后下来不用换方向
                    // 记下往下走分叉时的芯片
                    if (this.nineDie.LeftDownDie.Exist)
                    {
                        this.tDie.DieInfo = this.nineDie.LeftDownDie;
                        this.tDie.RowNum = this.currentRowNum + 1;
                        this.tDie.RowDirection = RowDirection.RightToLeft;
                    }
                }
            }
            else
            {
                // RowDirection.RightToLeft
                if (this.currentRowNum != this.turnUpRowNum - 1)
                {
                    // 记下往下走分叉时的芯片
                    if (this.nineDie.LeftDownDie.Exist)
                    {
                        this.tDie.DieInfo = this.nineDie.LeftDownDie;
                        this.tDie.RowNum = this.currentRowNum + 1;
                        this.tDie.RowDirection = RowDirection.RightToLeft;
                    }
                }
                else
                {
                    // 若往上一行后下来不用换方向
                    // 记下分叉时的芯片
                    if (this.nineDie.RightDownDie.Exist)
                    {
                        this.tDie.DieInfo = this.nineDie.RightDownDie;
                        this.tDie.RowNum = this.currentRowNum + 1;
                        this.tDie.RowDirection = RowDirection.LeftToRight;
                    }
                }
            }
        }

        /// <summary>
        /// 标记同一行(不论向上跳或者向下跳到该行或者开始时就在该行)分叉晶片
        /// </summary>
        private void MarkNagativeDirTDie()
        {
            if (this.CurrentRowDirection == RowDirection.LeftToRight)
            {
                if (this.nineDie.RightCenterDie.Exist)
                {
                    this.tDie.DieInfo = this.nineDie.RightCenterDie;
                    this.tDie.RowNum = this.currentRowNum;
                    this.tDie.RowDirection = RowDirection.RightToLeft;
                }
            }
            else
            {
                if (this.nineDie.LeftCenterDie.Exist)
                {
                    this.tDie.DieInfo = this.nineDie.LeftCenterDie;
                    this.tDie.RowNum = this.currentRowNum;
                    this.tDie.RowDirection = RowDirection.LeftToRight;
                }
            }
        }

        /// <summary>
        /// 计算两点之间的距离方法
        /// </summary>
        /// <param name="x">p1点x坐标</param>
        /// <param name="y">p1点y坐标</param>
        /// <param name="pt">p2点坐标对</param>
        /// <returns>两点之间的距离</returns>
        private double Distance(double x, double y, AKRSPoint3D pt)
        {
            return Math.Sqrt(Math.Pow(x - pt.X, 2) + Math.Pow(y - pt.Y, 2));
        }

        /// <summary>
        /// 9颗搜晶方式
        /// </summary>
        private RowDirection CurrentRowDirection;

        /// <summary>
        /// 搜索结果
        /// </summary>
        private NineDie nineDie = new NineDie();

        /// <summary>
        /// 当前行数
        /// </summary>
        private int currentRowNum;

        /// <summary>
        /// 向上转的行号
        /// </summary>
        private int turnUpRowNum;

        /// <summary>
        /// 换到下一行的时候行走方向另一边的芯片位置
        /// </summary>
        private MarkedDie tDie = new MarkedDie();

        /// <summary>
        /// BiggestRowStartDie
        /// </summary>
        private MarkedDie BiggestRowStartDie = new MarkedDie();

        /// <summary>
        /// NextRowStartDie
        /// </summary>
        private MarkedDie NextRowStartDie = new MarkedDie();

        /// <summary>
        /// 当前芯片信息
        /// </summary>
        private SingleDieInfo currentDie;

        /// <summary>
        /// 下一颗芯片信息
        /// </summary>
        private SingleDieInfo nextDie;

        /// <summary>
        /// RecurCount
        /// </summary>
        private int RecurCount;

        /// <summary>
        /// RecurLimit
        /// </summary>
        private int RecurLimit = 50;
    }
}
