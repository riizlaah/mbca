namespace MBCA_Desktop.UserControls
{
    partial class CRUDEvents
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
            table = new DataGridView();
            label1 = new Label();
            button1 = new Button();
            table2 = new DataGridView();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)table).BeginInit();
            ((System.ComponentModel.ISupportInitialize)table2).BeginInit();
            SuspendLayout();
            // 
            // table
            // 
            table.AllowUserToAddRows = false;
            table.AllowUserToDeleteRows = false;
            table.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table.Location = new Point(8, 48);
            table.Name = "table";
            table.ReadOnly = true;
            table.Size = new Size(778, 267);
            table.TabIndex = 0;
            table.CellClick += onEventCellClicked;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(8, 8);
            label1.Name = "label1";
            label1.Size = new Size(146, 25);
            label1.TabIndex = 1;
            label1.Text = "Manage Events";
            // 
            // button1
            // 
            button1.Location = new Point(653, 10);
            button1.Name = "button1";
            button1.Size = new Size(133, 23);
            button1.TabIndex = 2;
            button1.Text = "Add New Event";
            button1.UseVisualStyleBackColor = true;
            button1.Click += onAddEvent;
            // 
            // table2
            // 
            table2.AllowUserToAddRows = false;
            table2.AllowUserToDeleteRows = false;
            table2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            table2.Location = new Point(8, 365);
            table2.Name = "table2";
            table2.ReadOnly = true;
            table2.Size = new Size(590, 267);
            table2.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(8, 335);
            label2.Name = "label2";
            label2.Size = new Size(111, 17);
            label2.TabIndex = 4;
            label2.Text = "Exhibits in event";
            // 
            // CRUDEvents
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(label2);
            Controls.Add(table2);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(table);
            Name = "CRUDEvents";
            Size = new Size(799, 661);
            ((System.ComponentModel.ISupportInitialize)table).EndInit();
            ((System.ComponentModel.ISupportInitialize)table2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView table;
        private Label label1;
        private Button button1;
        private DataGridView table2;
        private Label label2;
    }
}
