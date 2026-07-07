namespace MBCA_Desktop
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            login = new Button();
            label1 = new Label();
            usernameOrEmail = new TextBox();
            password = new TextBox();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // login
            // 
            login.Location = new Point(157, 267);
            login.Name = "login";
            login.Size = new Size(170, 43);
            login.TabIndex = 0;
            login.Text = "LOGIN";
            login.UseVisualStyleBackColor = true;
            login.Click += onLoginClicked;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(107, 93);
            label1.Name = "label1";
            label1.Size = new Size(134, 20);
            label1.TabIndex = 1;
            label1.Text = "Username or Email";
            // 
            // usernameOrEmail
            // 
            usernameOrEmail.Location = new Point(107, 116);
            usernameOrEmail.Name = "usernameOrEmail";
            usernameOrEmail.Size = new Size(276, 27);
            usernameOrEmail.TabIndex = 2;
            // 
            // password
            // 
            password.Location = new Point(107, 176);
            password.Name = "password";
            password.PasswordChar = '*';
            password.Size = new Size(276, 27);
            password.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(107, 153);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 3;
            label2.Text = "Password";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(218, 29);
            label3.Name = "label3";
            label3.Size = new Size(64, 28);
            label3.TabIndex = 5;
            label3.Text = "Login";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(494, 342);
            Controls.Add(label3);
            Controls.Add(password);
            Controls.Add(label2);
            Controls.Add(usernameOrEmail);
            Controls.Add(label1);
            Controls.Add(login);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button login;
        private Label label1;
        private TextBox usernameOrEmail;
        private TextBox password;
        private Label label2;
        private Label label3;
    }
}
