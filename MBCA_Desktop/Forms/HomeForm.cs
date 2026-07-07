using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MBCA_Desktop.UserControls;

namespace MBCA_Desktop.Forms
{
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
            if(Helper.profile == null)
            {
                MessageBox.Show("Session invalid");
                Close();
                return;
            }
            if(Helper.profile.role == "Visitor")
            {
                var control = new VisitorUserControl
                {
                    Dock = DockStyle.Fill,
                };
                Controls.Add(control);
                control.OnLogout += (s, e) => { Helper.token = ""; Helper.profile = null; Close(); };
            } else
            {
                var control = new AdminUserControl
                {
                    Dock = DockStyle.Fill,
                };
                Controls.Add(control);
                control.OnLogout += (s, e) => { Helper.token = ""; Helper.profile = null; Close(); };
            }
        }
    }
}
