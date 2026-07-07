using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MBCA_Desktop.UserControls
{
    public partial class AdminUserControl : UserControl
    {
        public event EventHandler OnLogout;
        public AdminUserControl()
        {
            InitializeComponent();
        }

        private void onLogout(object sender, EventArgs e)
        {
            OnLogout?.Invoke(this, null);
        }
    }
}
