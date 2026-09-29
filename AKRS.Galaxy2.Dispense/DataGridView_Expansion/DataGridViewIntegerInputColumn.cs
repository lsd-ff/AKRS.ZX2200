using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    [ToolboxBitmap(typeof(DataGridViewTextBoxColumn), "DataGridViewTextBoxColumn.bmp")]
    public class DataGridViewIntegerInputColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewIntegerInputColumn()
        {
            CellTemplate = new DataGridViewIntegerInputCell();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !(value is DataGridViewIntegerInputCell))
                {
                    throw new InvalidCastException(nameof(DataGridViewIntegerInputCell));
                }

                base.CellTemplate = value;
            }
        }
    }
}
