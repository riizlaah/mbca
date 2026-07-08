namespace MBCA_Desktop.Forms
{
    partial class AddEditExhibitForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            formTitle = new Label();
            label2 = new Label();
            name = new TextBox();
            artist = new TextBox();
            label3 = new Label();
            categories = new ComboBox();
            label4 = new Label();
            timePeriod = new TextBox();
            label5 = new Label();
            tags = new TextBox();
            label6 = new Label();
            label7 = new Label();
            image = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            label8 = new Label();
            chooseImage = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)image).BeginInit();
            SuspendLayout();
            // 
            // formTitle
            // 
            formTitle.AutoSize = true;
            formTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            formTitle.Location = new Point(12, 9);
            formTitle.Name = "formTitle";
            formTitle.Size = new Size(146, 32);
            formTitle.TabIndex = 0;
            formTitle.Text = "Add Exhibit";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 65);
            label2.Name = "label2";
            label2.Size = new Size(39, 15);
            label2.TabIndex = 1;
            label2.Text = "Name";
            // 
            // name
            // 
            name.Location = new Point(12, 83);
            name.Name = "name";
            name.Size = new Size(194, 23);
            name.TabIndex = 2;
            // 
            // artist
            // 
            artist.Location = new Point(12, 157);
            artist.Name = "artist";
            artist.Size = new Size(194, 23);
            artist.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 139);
            label3.Name = "label3";
            label3.Size = new Size(35, 15);
            label3.TabIndex = 3;
            label3.Text = "Artist";
            // 
            // categories
            // 
            categories.DropDownStyle = ComboBoxStyle.DropDownList;
            categories.FormattingEnabled = true;
            categories.Location = new Point(12, 229);
            categories.Name = "categories";
            categories.Size = new Size(194, 23);
            categories.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 211);
            label4.Name = "label4";
            label4.Size = new Size(55, 15);
            label4.TabIndex = 6;
            label4.Text = "Category";
            // 
            // timePeriod
            // 
            timePeriod.Location = new Point(12, 307);
            timePeriod.Name = "timePeriod";
            timePeriod.Size = new Size(194, 23);
            timePeriod.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 289);
            label5.Name = "label5";
            label5.Size = new Size(71, 15);
            label5.TabIndex = 7;
            label5.Text = "Time Period";
            // 
            // tags
            // 
            tags.Location = new Point(257, 83);
            tags.Multiline = true;
            tags.Name = "tags";
            tags.Size = new Size(373, 81);
            tags.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(257, 65);
            label6.Name = "label6";
            label6.Size = new Size(31, 15);
            label6.TabIndex = 9;
            label6.Text = "Tags";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(259, 170);
            label7.Name = "label7";
            label7.Size = new Size(40, 15);
            label7.TabIndex = 11;
            label7.Text = "Image";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // image
            // 
            image.BackColor = SystemColors.ControlLight;
            image.Location = new Point(257, 190);
            image.Name = "image";
            image.Size = new Size(195, 140);
            image.SizeMode = PictureBoxSizeMode.Zoom;
            image.TabIndex = 12;
            image.TabStop = false;
            image.Click += onImageClicked;
            // 
            // button1
            // 
            button1.Location = new Point(547, 307);
            button1.Name = "button1";
            button1.Size = new Size(83, 23);
            button1.TabIndex = 13;
            button1.Text = "Submit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += onSubmit;
            // 
            // button2
            // 
            button2.Location = new Point(458, 307);
            button2.Name = "button2";
            button2.Size = new Size(83, 23);
            button2.TabIndex = 14;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += onCancel;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label8.ForeColor = SystemColors.ControlDark;
            label8.Location = new Point(257, 333);
            label8.Name = "label8";
            label8.Size = new Size(192, 15);
            label8.TabIndex = 15;
            label8.Text = "(Click image to select a new image)";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // chooseImage
            // 
            chooseImage.Filter = "PNG or JPEG|*.png;*.jpeg;*.jpg";
            // 
            // AddEditExhibitForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(647, 365);
            Controls.Add(label8);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(image);
            Controls.Add(label7);
            Controls.Add(tags);
            Controls.Add(label6);
            Controls.Add(timePeriod);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(categories);
            Controls.Add(artist);
            Controls.Add(label3);
            Controls.Add(name);
            Controls.Add(label2);
            Controls.Add(formTitle);
            Name = "AddEditExhibitForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add Exhibit";
            ((System.ComponentModel.ISupportInitialize)image).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label formTitle;
        private Label label2;
        private TextBox name;
        private TextBox artist;
        private Label label3;
        private ComboBox categories;
        private Label label4;
        private TextBox timePeriod;
        private Label label5;
        private TextBox tags;
        private Label label6;
        private Label label7;
        private PictureBox image;
        private Button button1;
        private Button button2;
        private Label label8;
        private OpenFileDialog chooseImage;
    }
}