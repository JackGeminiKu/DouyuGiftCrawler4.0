using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace DouyuGiftCrawler
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);    // 非UI异常
            Application.ThreadException += new ThreadExceptionEventHandler(UI_ThreadException);                                     // UI异常

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }

        static void UI_ThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try {
                MessageBox.Show("Windows窗体线程异常: " + Environment.NewLine + t.Exception.Message + Environment.NewLine + t.Exception.StackTrace);
            } catch {
                MessageBox.Show("不可恢复的Windows窗体异常，应用程序 即将退出！");
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try {
                Exception ex = (Exception)e.ExceptionObject;
                MessageBox.Show("非窗体线程异常: " + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
            } catch {
                MessageBox.Show("不可恢复的非Windows窗体线程异常，应用程序 即将退出！");
            }
        }
    }
}
