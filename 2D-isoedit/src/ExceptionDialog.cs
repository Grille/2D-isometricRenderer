using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program;

internal class ExceptionDialog
{
    public static DialogResult Show(IWin32Window owner, Exception e)
    {
        return MessageBox.Show(owner, e.Message, e.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
