using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    /// <summary>
    /// EjectionBankEntity
    /// </summary>
    public class EjectionBankEntity
    {
        /// <summary>
        /// EjectionBankEntity
        /// </summary>
        /// <param name="i">i</param>
        public EjectionBankEntity(int i = 5)
        {
            this.EjectionBankSlotEntities = new EjectionBankSlotEntity[i];
        }

        /// <summary>
        /// EjectionBankSlotEntities
        /// </summary>
        public EjectionBankSlotEntity[] EjectionBankSlotEntities { get; set; }
    }
}
