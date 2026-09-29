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
    public class DataGridViewDoubleInputColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewDoubleInputColumn()
        {
            CellTemplate = new DataGridViewDoubleInputCell();
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
                if (value != null && !(value is DataGridViewDoubleInputCell))
                {
                    throw new InvalidCastException(nameof(DataGridViewDoubleInputCell));
                }

                base.CellTemplate = value;
            }
        }
    }
}
