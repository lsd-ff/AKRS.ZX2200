using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    public class DataGridViewIntegerInputCell : DataGridViewTextBoxCell
    {
        private static Type s_defaultFormattedValueType = typeof(int);

        protected override void OnKeyPress(KeyPressEventArgs e, int rowIndex)
        {
            base.OnKeyPress(e, rowIndex);

            if (!Char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        //public override Type FormattedValueType
        //{
        //    get { return s_defaultFormattedValueType; }
        //}

        public override Type EditType
        {
            get { return typeof(DataGridViewIntegerInputEditingControl); }
        }

        //public override Type ValueType
        //{
        //    get { return typeof(int); }
        //}


        ///// <summary>附加并初始化寄宿的编辑控件。</summary>
        ///// <param name="rowIndex">所编辑的行的索引。</param>
        ///// <param name="initialFormattedValue">要在控件中显示的初始值。</param>
        ///// <param name="dataGridViewCellStyle">用于确定寄宿控件外观的单元格样式。</param>
        ///// <filterpriority>1</filterpriority>
        //public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        //{
        //    base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
        //    TextBox textBox = base.DataGridView.EditingControl as TextBox;
        //    if (textBox != null)
        //    {
        //        textBox.BorderStyle = BorderStyle.None;
        //        textBox.AcceptsReturn = (textBox.Multiline = (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True));
        //        textBox.MaxLength = this.MaxInputLength;
        //        string text = initialFormattedValue as string;
        //        if (text == null)
        //        {
        //            textBox.Text = string.Empty;
        //        }
        //        else
        //        {
        //            textBox.Text = text;
        //        }

        //        this.EditingTextBox = (base.DataGridView.EditingControl as DataGridViewTextBoxEditingControl);
        //    }
        //}

        private static DataGridViewIntegerInputEditingControl s_editingControl = new DataGridViewIntegerInputEditingControl();

        private DataGridViewIntegerInputEditingControl IntegerInputEditingControl
        {
            get
            {
                //return (DataGridViewTextBoxEditingControl)base.Properties.GetObject(DataGridViewTextBoxCell.PropTextBoxCellEditingTextBox);
                return s_editingControl;

            }
            set
            {
                if (value != null)
                {
                    s_editingControl = value;
                }
            }
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            //var ctrl = DataGridView.EditingControl as DataGridViewIntegerInputEditingControl;
            var ctrl = (DataGridViewIntegerInputEditingControl)DataGridView.EditingControl;
            if (this.Value == null)
            {
                //IntegerInputEditingControl.Text = "0";
                ctrl.Text = "0";
            }
            else
            {
                int iVal;
                if (this.Value is string)
                {
                    iVal = Int32.Parse((string)this.Value);                    
                }
                else
                {
                    iVal = (Int32)this.Value;
                }

                ctrl.Text = iVal.ToString();
            }
        }
    }
}
