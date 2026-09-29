namespace AKRS.ZX2200.SupportFeature.Parameters.ObjParameter
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;

    /// <summary>
    /// 华夫盘参数类
    /// </summary>
    public class AdapterPara
    {
        /// <summary>
        /// 华夫盘配置
        /// </summary>
        private AdapterConfig currentAdapterConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="adapterConfig">华夫盘配置</param>
        public AdapterPara(AdapterConfig adapterConfig)
        {
            this.currentAdapterConfig = adapterConfig;
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽1首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot1
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[0].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[0].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽2首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot2
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[1].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[1].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽3首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot3
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[2].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[2].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽4首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot4
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[3].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[3].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽5首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot5
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[4].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[4].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽6首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot6
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[5].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[5].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽7首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot7
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[6].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[6].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽8首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot8
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[7].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[7].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽9首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot9
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[8].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[8].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽10首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot10
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[9].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[9].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽11首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot11
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[10].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[10].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽12首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot12
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[11].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[11].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽13首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot13
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[12].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[12].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽14首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot14
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[13].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[13].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽15首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot15
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[14].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[14].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽16首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot16
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[15].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[15].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽17首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot17
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[16].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[16].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽18首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot18
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[17].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[17].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽19首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot19
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[18].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[18].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽20首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot20
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[19].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[19].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽21首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot21
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[20].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[20].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽22首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot22
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[21].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[21].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽23首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot23
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[22].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[22].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽24首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot24
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[23].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[23].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽25首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot25
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[24].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[24].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽26首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot26
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[25].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[25].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽27首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot27
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[26].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[26].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽28首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot28
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[27].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[27].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽29首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot29
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[28].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[28].FirstPosition = value;
            }
        }

        /// <summary>
        /// 首位置
        /// </summary>
        [TreeProgramListArgs("槽30首位置", true, null, UnitHelper.mm)]
        public AKRSPoint3D Slot30
        {
            get
            {
                return this.currentAdapterConfig.WaffleArray[29].FirstPosition;
            }

            set
            {
                this.currentAdapterConfig.WaffleArray[29].FirstPosition = value;
            }
        }
    }
}
