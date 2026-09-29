using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 表示可以承载于 <see cref="T:DataGridViewIntegerInputCell" /> 中的文本框控件。 
    /// </summary>
	/// <filterpriority>2</filterpriority>
    [ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	public class DataGridViewDoubleInputEditingControl : DataGridViewTextBoxEditingControl
    {
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            base.OnKeyPress(e);
        }

		///// <summary>处理键事件。</summary>
		///// <returns>
		///// 如果键事件已由编辑控件处理，则为 true；否则为 false。</returns>
		///// <param name="m">
		/////   <see cref="T:System.Windows.Forms.Message" /> 指示键已被按下。</param>
		//// Token: 0x0600387E RID: 14462 RVA: 0x000CE104 File Offset: 0x000CD104
		//[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		//protected override bool ProcessKeyEventArgs(ref Message m)
		//{
		//	Keys keys = (Keys)((int)m.WParam);
		//	if (keys != Keys.LineFeed)
		//	{
		//		if (keys != Keys.Return)
		//		{
		//			if (keys == Keys.A)
		//			{
		//				if (m.Msg == 256 && Control.ModifierKeys == Keys.Control)
		//				{
		//					base.SelectAll();
		//					return true;
		//				}
		//			}
		//		}
		//		else if (m.Msg == 258 && (Control.ModifierKeys != Keys.Shift || !this.Multiline || !base.AcceptsReturn))
		//		{
		//			return true;
		//		}
		//	}
		//	else if (m.Msg == 258 && Control.ModifierKeys == Keys.Control && this.Multiline && base.AcceptsReturn)
		//	{
		//		return true;
		//	}

  //          if (Keys.D0 <= keys && keys <= Keys.D9 || Keys.NumPad0 <= keys && keys <= Keys.NumPad9)
  //              return base.ProcessKeyEventArgs(ref m);
  //          else
  //              return true;
		//}

        /// <summary>
        /// 确定指定的键是应由编辑控件处理的常规输入键，还是应由 <see cref="T:System.Windows.Forms.DataGridView" /> 处理的特殊键。
        /// </summary>
		/// <param name="keyData">一个 <see cref="T:System.Windows.Forms.Keys" />，表示按下的键。</param>
		/// <param name="dataGridViewWantsInputKey"> 当 <see cref="T:System.Windows.Forms.DataGridView" /> 要处理 <paramref name="keyData" /> 时，为 true；否则为 false。</param>
		/// <returns>
		/// 如果指定的键是应由编辑控件处理的常规输入键，则为 true；否则为 false。
        /// </returns>
		public override bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
		{
            if (base.EditingControlWantsInputKey(keyData, dataGridViewWantsInputKey))
            {
                Keys keys = keyData & Keys.KeyCode;
                if (Keys.NumPad0 <= keys && keys <= Keys.D9 || Keys.NumPad0 <= keys && keys <= Keys.NumPad9)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return !dataGridViewWantsInputKey;
            }
		}
    }
}
