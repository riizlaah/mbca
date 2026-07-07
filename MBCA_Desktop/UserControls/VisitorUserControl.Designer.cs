namespace MBCA_Desktop.UserControls
{
    partial class VisitorUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            username = new Label();
            panel1 = new Panel();
            label1 = new Label();
            label2 = new Label();
            otpCode = new Label();
            validUntil = new Label();
            label4 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(8, 8);
            button1.Name = "button1";
            button1.Size = new Size(94, 40);
            button1.TabIndex = 0;
            button1.Text = "Log Out";
            button1.UseVisualStyleBackColor = true;
            button1.Click += onLogout;
            // 
            // username
            // 
            username.AutoSize = true;
            username.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            username.Location = new Point(115, 12);
            username.Name = "username";
            username.Size = new Size(70, 28);
            username.TabIndex = 1;
            username.Text = "label1";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(username);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(0, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(325, 678);
            panel1.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(29, 84);
            label1.Name = "label1";
            label1.Size = new Size(218, 31);
            label1.TabIndex = 2;
            label1.Text = "One Time Passcode";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(678, 141);
            label2.Name = "label2";
            label2.Size = new Size(195, 28);
            label2.TabIndex = 3;
            label2.Text = "One Time Passcode";
            // 
            // otpCode
            // 
            otpCode.AutoSize = true;
            otpCode.Font = new Font("Consolas", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            otpCode.Location = new Point(623, 196);
            otpCode.Name = "otpCode";
            otpCode.Size = new Size(304, 94);
            otpCode.TabIndex = 4;
            otpCode.Text = "ABC123";
            otpCode.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // validUntil
            // 
            validUntil.AutoSize = true;
            validUntil.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            validUntil.Location = new Point(648, 312);
            validUntil.Name = "validUntil";
            validUntil.Size = new Size(245, 20);
            validUntil.TabIndex = 5;
            validUntil.Text = "Valid until 2026-01-01 (00:00:01)";
            validUntil.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(581, 369);
            label4.Name = "label4";
            label4.Size = new Size(387, 20);
            label4.TabIndex = 6;
            label4.Text = "Enter this code in your phone to activate your account";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // VisitorUserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label4);
            Controls.Add(validUntil);
            Controls.Add(otpCode);
            Controls.Add(label2);
            Controls.Add(panel1);
            Name = "VisitorUserControl";
            Size = new Size(1195, 676);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label username;
        private Panel panel1;
        private Label label1;
        private Label label2;
        private Label otpCode;
        private Label validUntil;
        private Label label4;
    }
}
