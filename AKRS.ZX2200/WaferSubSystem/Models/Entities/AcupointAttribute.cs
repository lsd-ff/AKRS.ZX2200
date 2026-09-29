using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    /// <summary>
    /// AcupointAttribute
    /// </summary>
    public class AcupointAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stateDescription">stateDescription</param>
        /// <param name="stateColor">stateColor</param>
        public AcupointAttribute(string stateDescription, string stateColor)
        {
            this.StateDescription = stateDescription;
            this.StateColor = stateColor;
        }

        /// <summary>
        /// 状态描述
        /// </summary>
        public string StateDescription { get; set; }

        /// <summary>
        /// 状态显示颜色
        /// </summary>
        public string StateColor { get; set; }
    }
}
