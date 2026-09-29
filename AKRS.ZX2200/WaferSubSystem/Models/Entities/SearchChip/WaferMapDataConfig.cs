using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    using System.Drawing;

    using AKRS.WM;

    /// <summary>
    /// WaferMapDataConfig
    /// </summary>
    [Serializable]
    public class WaferMapDataConfig
    {
        /// <summary>
        /// JumpStep
        /// </summary>
        public int JumpStep { get; set; } = 0;

        /// <summary>
        /// Reference
        /// </summary>
        public ReferencePoint Reference { get; set; } = new ReferencePoint();

        /// <summary>
        /// StepPosition
        /// </summary>
        public Point StepPosition { get; set; } = new Point();

        /// <summary>
        /// LastWaferMapFilePath
        /// </summary>
        public string LastWaferMapFilePath { get; set; } = string.Empty;

        /// <summary>
        /// ReferencePoints
        /// </summary>
        public List<ReferencePoint> ReferencePoints { get; set; } = new List<ReferencePoint>();

        /// <summary>
        /// UserDirection
        /// </summary>
        public UserDirection UserDirection { get; set; } = UserDirection.LeftToRight_UpToDown;

        /// <summary>
        /// StartPoint
        /// </summary>
        public Point StartPoint { get; set; } = new Point();

        /// <summary>
        /// SelectedChipTypes
        /// </summary>
        public List<string> SelectedChipTypes { get; set; } = new List<string>();
    }
}
