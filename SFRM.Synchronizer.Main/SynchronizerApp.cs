using System.Windows.Forms;
using SFRM.Synchronizer.Main.Farmatic;
using SFRM.Synchronizer.Main.Fisiotes;

namespace SFRM.Synchronizer.Main
{
    public class SynchronizerApp : ApplicationContext
    {
        public SynchronizerApp()
        {
            DbFisiotes.SetCeroClientes();
        }
    }
}