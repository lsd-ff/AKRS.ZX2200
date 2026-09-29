namespace AKRS.ZX2200.SupportFeature.Parameters.ObjParameter
{
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 吸嘴架参数
    /// </summary>
    public class NozzleshelfPara
    {
        /// <summary>
        /// 吸嘴架对象
        /// </summary>
        public NozzleShelf NozzleShelf => BondProgram.GetInstance().NozzleShelfProgram.NozzleShelf;

        /// <summary>
        /// 穴位1吸嘴名
        /// </summary>
        [TreeProgramListArgs("槽位1", false, (TreeGroupChildNodesEnum[])null)]
        public string NozzleName1
        {
            get
            {
                return this.NozzleShelf.NozzleShelfSlots[0].NozzleName;
            }

            set
            {
                this.NozzleShelf.NozzleShelfSlots[0].NozzleName = value;
            }
        }

        /// <summary>
        /// 穴位2吸嘴名
        /// </summary>
        [TreeProgramListArgs("槽位2", false, (TreeGroupChildNodesEnum[])null)]
        public string NozzleName2
        {
            get
            {
                return this.NozzleShelf.NozzleShelfSlots[1].NozzleName;
            }

            set
            {
                this.NozzleShelf.NozzleShelfSlots[1].NozzleName = value;
            }
        }

        /// <summary>
        /// 穴位3吸嘴名
        /// </summary>
        [TreeProgramListArgs("槽位3", false, (TreeGroupChildNodesEnum[])null)]
        public string NozzleName3
        {
            get
            {
                return this.NozzleShelf.NozzleShelfSlots[2].NozzleName;
            }

            set
            {
                this.NozzleShelf.NozzleShelfSlots[2].NozzleName = value;
            }
        }

        /// <summary>
        /// 穴位4吸嘴名
        /// </summary>
        [TreeProgramListArgs("槽位4", false, null)]
        public string NozzleName4
        {
            get
            {
                return this.NozzleShelf.NozzleShelfSlots[3].NozzleName;
            }

            set
            {
                this.NozzleShelf.NozzleShelfSlots[3].NozzleName = value;
            }
        }

        /// <summary>
        /// 穴位5吸嘴名
        /// </summary>
        [TreeProgramListArgs("槽位5", false, null)]
        public string NozzleName5
        {
            get
            {
                return this.NozzleShelf.NozzleShelfSlots[4].NozzleName;
            }

            set
            {
                this.NozzleShelf.NozzleShelfSlots[4].NozzleName = value;
            }
        }

        /// <summary>
        /// 穴位6吸嘴名
        /// </summary>
        [TreeProgramListArgs("槽位6", false, null)]
        public string NozzleName6
        {
            get
            {
                return this.NozzleShelf.NozzleShelfSlots[5].NozzleName;
            }

            set
            {
                this.NozzleShelf.NozzleShelfSlots[5].NozzleName = value;
            }
        }

        /// <summary>
        /// 穴位7吸嘴名
        /// </summary>
        [TreeProgramListArgs("槽位7", false, null)]
        public string NozzleName7
        {
            get
            {
                return this.NozzleShelf.NozzleShelfSlots[6].NozzleName;
            }

            set
            {
                this.NozzleShelf.NozzleShelfSlots[0].NozzleName = value;
            }
        }
    }
}
