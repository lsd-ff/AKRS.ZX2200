using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    /// <summary>
    /// ReferencePoint
    /// </summary>
    [Serializable]
    public class ReferencePoint
    {
        /// <summary>
        /// Row
        /// </summary>
        public int Row { get; set; }

        /// <summary>
        /// Column
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// IntLabel
        /// </summary>
        public int IntLabel { get; set; }
    }
}
