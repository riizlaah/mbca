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
            button2 = new Button();
            button3 = new Button();
            splitContainer1 = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(3, 2);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(82, 30);
            button1.TabIndex = 0;
            button1.Text = "Log Out";
            button1.UseVisualStyleBackColor = true;
            button1.Click += onLogout;
            // 
            // username
            // 
            username.AutoSize = true;
            username.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            username.Location = new Point(91, 5);
            username.Name = "username";
            username.Size = new Size(57, 21);
            username.TabIndex = 1;
            username.Text = "label1";
            // 
            // button2
            // 
            button2.Location = new Point(3, 60);
            button2.Name = "button2";
            button2.Size = new Size(151, 23);
            button2.TabIndex = 2;
            button2.Text = "Events";
            button2.UseVisualStyleBackColor = true;
            button2.Click += onEvents;
            // 
            // button3
            // 
            button3.Location = new Point(3, 89);
            button3.Name = "button3";
            button3.Size = new Size(151, 23);
            button3.TabIndex = 3;
            button3.Text = "Exhibits";
            button3.UseVisualStyleBackColor = true;
            button3.Click += onExhibits;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(button3);
            splitContainer1.Panel1.Controls.Add(button1);
            splitContainer1.Panel1.Controls.Add(button2);
            splitContainer1.Panel1.Controls.Add(username);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.AutoScroll = true;
            splitContainer1.Size = new Size(1046, 507);
            splitContainer1.SplitterDistance = 209;
            splitContainer1.TabIndex = 3;
            // 
            // AdminUserControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(splitContainer1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "AdminUserControl";
            Size = new Size(1046, 507);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Label username;
        private Button button2;
        private Button button3;
        private SplitContainer splitContainer1;
    }
}
