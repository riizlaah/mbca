namespace MBCA_Desktop.UserControls
{
    partial class AdminUserControl
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
            panel1.Controls.Add(username);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(0, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(325, 678);
            panel1.TabIndex = 2;
            // 
            // AdminUserControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Name = "AdminUserControl";
            Size = new Size(1195, 676);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label username;
        private Panel panel1;
    }
}
