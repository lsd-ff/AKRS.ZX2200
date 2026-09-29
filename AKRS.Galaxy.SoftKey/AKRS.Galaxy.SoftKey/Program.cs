using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.Galaxy.SoftKey
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //加密狗检测
            bool success = SoftKey.SoftkeyManager.CheckYtSoftKey();
        }


    }
}
