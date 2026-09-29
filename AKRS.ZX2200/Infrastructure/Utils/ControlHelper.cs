using DevExpress.XtraSplashScreen;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.Infrastructure.Controls.Common;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Infrastructure.Utils;

public static class ControlHelper
{
    /// <summary>
    /// 弹出等待窗体
    /// </summary>
    public static async Task ShowWaitFormAsync(this IWin32Window owner, string caption, string description,
        System.Action asyncAction)
    {
        SplashScreenManager.ShowDefaultWaitForm((Form)owner, true, true, caption, description);

        try
        {
            await Task.Run(asyncAction).ConfigureAwait(false);
        }
        finally
        {
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }

    /// <summary>
    /// 弹出覆盖当前窗体的等待控件
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="message"></param>
    /// <param name="asyncAction"></param>
    /// <param name="font"></param>
    /// <param name="messageBrush"></param>
    /// <returns></returns>
    public static async Task ShowOverlayAsync(this IWin32Window owner, string message, System.Action asyncAction,
        Font font = null, Brush messageBrush = null)
    {
        OverlayWindowOptions options = new()
        {
            BackColor = Color.Black,
            Opacity = 0.2f,
            DisableInput = true,
            CustomPainter = new TextOverlayPainter
            {
                Text = message,
                Font = font ?? new Font("Tahoma", 12, FontStyle.Bold),
                Brush = messageBrush ?? Brushes.Red
            },
            FadeIn = true,
            FadeOut = true
        };

        IOverlaySplashScreenHandle handle =
            SplashScreenManager.ShowOverlayForm((Form)owner, options);
        try
        {
            await Task.Run(asyncAction).ConfigureAwait(false);
        }
        finally
        {
            SplashScreenManager.CloseOverlayForm(handle);
        }
    }

    /// <summary>
    /// 显示一个窗体，该方法重复调用只会显示一个实例
    /// </summary>
    /// <typeparam name="T">窗体类型</typeparam>
    /// <param name="owner">窗体所属，可为空，如果是IWin32Window的实现类可直接使用扩展方法</param>
    /// <param name="args">构造函数的参数</param>
    /// <returns>当前正在显示的实例</returns>
    public static T ShowForm<T>(this IWin32Window owner, params object[] args)
        where T : Form
    {
        T form = Application
            .OpenForms
            .OfType<T>()
            .FirstOrDefault(f => !f.IsDisposed);

        if (form == null)
        {
            form = (T)Activator.CreateInstance(typeof(T), args);
            form.StartPosition = FormStartPosition.CenterParent;
            if (owner != null)
            {
                form.Show(owner);
            }
            else
            {
                form.Show();
            }
        }
        else
        {
            if (!form.Visible)
            {
                if (owner != null)
                {
                    form.Show(owner);
                }
                else
                {
                    form.Show();
                }
            }

            if (form.WindowState == FormWindowState.Minimized)
            {
                form.WindowState = FormWindowState.Normal;
            }

            form.BringToFront();
            form.Activate();
        }

        return form;
    }

    /// <summary>
    /// 在一个窗体中显示单实例的 UserControl
    /// </summary>
    /// <typeparam name="T">要显示的 UserControl 类型</typeparam>
    /// <param name="owner">窗体所属，可为 null</param>
    /// <param name="title">窗体的标题</param>
    /// <param name="args">传给 T 构造函数的参数</param>
    /// <returns>正在显示的 UserControl 实例</returns>
    public static T ShowUserControl<T>(this IWin32Window owner, string title = "", params object[] args)
        where T : UserControl
    {
        string formName = typeof(T).FullName;

        XtraForm existingForm = Application
            .OpenForms
            .OfType<XtraForm>()
            .FirstOrDefault(f => f.Name == formName && !f.IsDisposed);

        if (existingForm == null)
        {
            T uc = (T)Activator.CreateInstance(typeof(T), args);
            uc.Dock = DockStyle.None;
            XtraForm xf = new()
            {
                Name = formName,
                Text = string.IsNullOrEmpty(title) ? typeof(T).Name : title,
                StartPosition = FormStartPosition.CenterParent
            };

            xf.Controls.Add(uc);

            // 设定合适的窗体大小适配用户控件
            xf.ClientSize = uc.Size;
            uc.Dock = DockStyle.Fill;
            if (owner != null)
            {
                xf.Show(owner);
            }
            else
            {
                xf.Show();
            }

            return uc;
        }
        else
        {
            if (!existingForm.Visible)
            {
                existingForm.Show(owner);
            }

            if (existingForm.WindowState == FormWindowState.Minimized)
            {
                existingForm.WindowState = FormWindowState.Normal;
            }

            existingForm.BringToFront();
            existingForm.Activate();

            T uc = existingForm.Controls.OfType<T>().FirstOrDefault();
            if (uc != null)
            {
                return uc;
            }

            // （一般不会走到这里）如果控件被意外移除，再重新创建一个
            uc = (T)Activator.CreateInstance(typeof(T), args);
            existingForm.Controls.Clear();
            existingForm.Controls.Add(uc);
            uc.Dock = DockStyle.Fill;

            return uc;
        }
    }
}