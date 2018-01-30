using System;
using System.Windows.Forms;
using SFRM.Synchronizer.Main.Properties;

namespace SFRM.Synchronizer.Main
{    
    class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var notifyIcon = new NotifyIcon();
            notifyIcon.ContextMenuStrip = GetSincronizadorMenuStrip();
            notifyIcon.Icon = Resources.SyncIcon;
            notifyIcon.Visible = true;

            Application.ApplicationExit += (sender, @event) => notifyIcon.Visible = false;
            Application.Run(new SynchronizerApp());
        }

        private static ContextMenuStrip GetSincronizadorMenuStrip()
        {
            var cms = new ContextMenuStrip();
            cms.Items.Add("Salir", null, (sender, @event) => Application.Exit());
            return cms;
        }
    }
}
