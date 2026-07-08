using MBCA_Desktop.Forms;
using System.Diagnostics;

namespace MBCA_Desktop
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Helper.LockWindow(this);
            tryLoginWithToken();
        }

        private void onLoginClicked(object sender, EventArgs e)
        {
            tryLogin();
        }

        async private void tryLogin()
        {
            login.Enabled = false;
            login.Text = "Loading...";
            var (success, res, msg) = await Helper.jsonReq<LoginRes, LoginReq>("users/login", "post", new LoginReq { usernameOrEmail = usernameOrEmail.Text, password = password.Text });
            login.Text = "LOGIN";
            login.Enabled = true;
            if(!success)
            {
                MessageBox.Show(msg, "Error");
                return;
            }
            if(res.data == null)
            {
                MessageBox.Show(msg, "Error");
                return;
            }
            Helper.token = res.data.token;
            Properties.Settings.Default.token = Helper.token;
            Properties.Settings.Default.Save();
            var (success2, res2, msg2) = await Helper.jsonReq<ProfileRes, object>("users/me");
            if (!success2)
            {
                Properties.Settings.Default.token = "";
                Properties.Settings.Default.Save();
                MessageBox.Show("Login failed", "Error");
                return;
            }
            if (res2.data == null)
            {
                Properties.Settings.Default.token = "";
                Properties.Settings.Default.Save();
                MessageBox.Show("Login failed", "Error");
                return;
            }
            Helper.profile = res2.data;
            usernameOrEmail.Text = "";
            password.Text = "";
            var window = new HomeForm();
            Hide();
            window.Show();
            window.FormClosed += (s, e) => { Show(); };
        }

        async private void tryLoginWithToken()
        {
            Helper.token = Properties.Settings.Default.token;
            var (success, res, msg) = await Helper.jsonReq<ProfileRes, object>("users/me");
            if (!success)
            {
                Properties.Settings.Default.token = "";
                Properties.Settings.Default.Save();
                return;
            }
            if (res.data == null) return;
            Helper.profile = res.data;
            var window = new HomeForm();
            Hide();
            window.Show();
            window.FormClosed += (s, e) => { Show(); };
        }
    }
}
