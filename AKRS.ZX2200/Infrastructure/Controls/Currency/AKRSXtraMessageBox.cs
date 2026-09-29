
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.Infrastructure.EventBus;
using AKRS.ZX2200.Main.Controls;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Internal;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{

    /// <summary>
    /// 对话框，主线程生成
    /// </summary>
    public static class AKRSXtraMessageBox
    {
        public static DialogResult Show(XtraMessageBoxArgs args)
        {
            DialogResult dialogResult = DialogResult.None;

            if (MainForm.SendUiAction == null)
            {
                dialogResult = XtraMessageBox.Show(args);
                return dialogResult;
            }

            // 如果是工作线程不用竞争锁
            if (Thread.CurrentThread.ManagedThreadId == 1)
            {
                MainForm.SendUiAction(
                    () =>
                    {
                        dialogResult = XtraMessageBox.Show(args);
                    });
            }
            else
            {
                lock (AKRSMessageBoxExt.UiLock)
                {
                    MainForm.SendUiAction(
                          () =>
                          {
                              dialogResult = XtraMessageBox.Show(args);
                          });
                }
            }

            return dialogResult;
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified text.
        //
        // 参数:
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text)
        {
            return Show((IWin32Window)null, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified owner and text.
        //
        // 参数:
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text)
        {
            return Show(owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified text and caption.
        //
        // 参数:
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption)
        {
            return Show((IWin32Window)null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified owner, text and caption.
        //
        // 参数:
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified text, caption and buttons.
        //
        // 参数:
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Show((IWin32Window)null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified owner, text, caption and buttons.
        //
        //
        // 参数:
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified text, caption, buttons and icon.
        //
        //
        // 参数:
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show((IWin32Window)null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified owner, text, caption, buttons
        //     and icon.
        //
        // 参数:
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified text, caption, buttons, icon and
        //     default button.
        //
        // 参数:
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show((IWin32Window)null, text, caption, buttons, icon, defaultButton);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified owner, text, caption, buttons,
        //     icon and default button.
        //
        // 参数:
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(owner, text, caption, XtraMessageHelper.MessageBoxButtonsToDialogResults(buttons), XtraMessageHelper.MessageBoxIconToIcon(icon), XtraMessageHelper.MessageBoxDefaultButtonToInt(defaultButton), icon);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings and text.
        //
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text)
        {
            return Show(lookAndFeel, null, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, owner
        //     and text.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text)
        {
            return Show(lookAndFeel, owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, text and
        //     caption.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption)
        {
            return Show(lookAndFeel, null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, owner,
        //     text and caption.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption)
        {
            return Show(lookAndFeel, owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, text,
        //     caption and buttons.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(lookAndFeel, null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, owner,
        //     text, caption and buttons.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(lookAndFeel, owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, text,
        //     caption, buttons and icon.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(lookAndFeel, null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, owner,
        //     text, caption, buttons and icon.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(lookAndFeel, owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, text,
        //     caption, buttons, icon and default button.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(lookAndFeel, null, text, caption, buttons, icon, defaultButton);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, text,
        //     caption, buttons, icon and default button.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(lookAndFeel, owner, text, caption, XtraMessageHelper.MessageBoxButtonsToDialogResults(buttons), XtraMessageHelper.MessageBoxIconToIcon(icon), XtraMessageHelper.MessageBoxDefaultButtonToInt(defaultButton), icon);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified look and feel settings, owner,
        //     text, caption, buttons, icon, default button and plays the sound that corresponds
        //     to the specified system-alert level.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     An array of values that specify which buttons to display in the message box.
        //
        //
        //   icon:
        //     The System.Drawing.Icon displayed in the message box.
        //
        //   defaultButton:
        //     The zero-based index of the default button.
        //
        //   messageBeepSound:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies a system-alert
        //     level.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, DialogResult[] buttons, Icon icon, int defaultButton, MessageBoxIcon messageBeepSound)
        {
            return Show(lookAndFeel, owner, text, caption, buttons, icon, defaultButton, messageBeepSound, DefaultBoolean.Default);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified owner, text, caption, buttons,
        //     icon, default button and plays the sound that corresponds to the specified system-alert
        //     level.
        //
        // 参数:
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     An array of values that specify which buttons to display in the message box.
        //
        //
        //   icon:
        //     The System.Drawing.Icon displayed in the message box.
        //
        //   defaultButton:
        //     The zero-based index of the default button.
        //
        //   messageBeepSound:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies a system-alert
        //     level.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, DialogResult[] buttons, Icon icon, int defaultButton, MessageBoxIcon messageBeepSound)
        {
            return Show(null, owner, text, caption, buttons, icon, defaultButton, messageBeepSound, DefaultBoolean.Default);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   text:
        //     The text to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, DefaultBoolean allowHtmlText)
        {
            return Show((IWin32Window)null, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, DefaultBoolean allowHtmlText)
        {
            return Show(owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption, DefaultBoolean allowHtmlText)
        {
            return Show((IWin32Window)null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, DefaultBoolean allowHtmlText)
        {
            return Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, DefaultBoolean allowHtmlText)
        {
            return Show((IWin32Window)null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, DefaultBoolean allowHtmlText)
        {
            return Show(owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of the message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, DefaultBoolean allowHtmlText)
        {
            return Show((IWin32Window)null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   owner:
        //     An object that serves as a dialog box’s top-level window and owner.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, DefaultBoolean allowHtmlText)
        {
            return Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, DefaultBoolean allowHtmlText)
        {
            return Show((IWin32Window)null, text, caption, buttons, icon, defaultButton, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, DefaultBoolean allowHtmlText)
        {
            return Show(owner, text, caption, XtraMessageHelper.MessageBoxButtonsToDialogResults(buttons), XtraMessageHelper.MessageBoxIconToIcon(icon), XtraMessageHelper.MessageBoxDefaultButtonToInt(defaultButton), icon, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, null, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, owner, text, "", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     The message box’s caption.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format a message box’
        //     text and caption. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption, MessageBoxButtons buttons, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     A string value that specifies the text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of amessage box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, null, text, caption, buttons, icon, defaultButton, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     A value that specifies which buttons to display in the message box.
        //
        //   icon:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies which icon
        //     to display in the message box.
        //
        //   defaultButton:
        //     One of the System.Windows.Forms.MessageBoxDefaultButton values that specifies
        //     the default button for the message box.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, DefaultBoolean allowHtmlText)
        {
            return Show(lookAndFeel, owner, text, caption, XtraMessageHelper.MessageBoxButtonsToDialogResults(buttons), XtraMessageHelper.MessageBoxIconToIcon(icon), XtraMessageHelper.MessageBoxDefaultButtonToInt(defaultButton), icon, allowHtmlText);
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   lookAndFeel:
        //     A DevExpress.LookAndFeel.UserLookAndFeel object whose properties specify the
        //     look and feel of the message box.
        //
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     An array of values that specify which buttons to display in the message box.
        //
        //
        //   icon:
        //     The System.Drawing.Icon displayed in the message box.
        //
        //   defaultButton:
        //     The zero-based index of the default button.
        //
        //   messageBeepSound:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies a system-alert
        //     level.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(UserLookAndFeel lookAndFeel, IWin32Window owner, string text, string caption, DialogResult[] buttons, Icon icon, int defaultButton, MessageBoxIcon messageBeepSound, DefaultBoolean allowHtmlText)
        {
            // XtraMessageHelper.MessageBeep((uint)messageBeepSound);
            return Show(new XtraMessageBoxArgs(lookAndFeel, owner, text, caption, buttons, icon, defaultButton, allowHtmlText));
        }

        //
        // 摘要:
        //     Displays the XtraMessageBox with the specified settings.
        //
        // 参数:
        //   owner:
        //     An object that serves as the top-level window and owner of a dialog box.
        //
        //   text:
        //     The text to display in the message box.
        //
        //   caption:
        //     A string value that specifies the caption of a message box.
        //
        //   buttons:
        //     An array of values that specify which buttons to display in the message box.
        //
        //
        //   icon:
        //     The System.Drawing.Icon displayed in the message box.
        //
        //   defaultButton:
        //     The zero-based index of the default button.
        //
        //   messageBeepSound:
        //     One of the System.Windows.Forms.MessageBoxIcon values that specifies a system-alert
        //     level.
        //
        //   allowHtmlText:
        //     A value that specifies whether HTML tags can be used to format the text and caption
        //     of a message box. See DevExpress.XtraEditors.XtraMessageBox.AllowHtmlText to
        //     learn more.
        //
        // 返回结果:
        //     One of the System.Windows.Forms.DialogResult values.
        public static DialogResult Show(IWin32Window owner, string text, string caption, DialogResult[] buttons, Icon icon, int defaultButton, MessageBoxIcon messageBeepSound, DefaultBoolean allowHtmlText)
        {
            return Show(null, owner, text, caption, buttons, icon, defaultButton, messageBeepSound, allowHtmlText);
        }
    }
}
