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
    public partial class VisitorUserControl : UserControl
    {
        public event EventHandler OnLogout;
        private System.Windows.Forms.Timer timer;
        public VisitorUserControl()
        {
            timer = new System.Windows.Forms.Timer
            {
                Interval = 5000
            };
            InitializeComponent();
            refreshToken();
            timer.Tick += (s, e) =>
            {
                refreshToken();
            };
            timer.Start();
            username.Text = "@" + (Helper.profile?.username ?? "Visitor");
        }

        async private void refreshToken()
        {
            otpCode.Text = "------";
            validUntil.Text = "Valid until : ---";
            var (success, res, msg) = await Helper.jsonReq<OTPRes, object>("otp");
            if(success && res.data != null)
            {
                otpCode.Text = res.data.code ?? "------";
                validUntil.Text = "Valid until : " + (res.data.validUntil?.ToString("yyyy-MM-dd HH:mm:ss") ?? "---");
            }
        }

        private void onLogout(object sender, EventArgs e)
        {
            OnLogout?.Invoke(this, null);
        }
    }
}
