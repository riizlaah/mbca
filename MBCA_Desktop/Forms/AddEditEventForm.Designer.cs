namespace MBCA_Desktop.Forms
{
    partial class AddEditEventForm
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
            title = new TextBox();
            description = new TextBox();
            label3 = new Label();
            categories = new ComboBox();
            label4 = new Label();
            location = new TextBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            image = new PictureBox();
            button1 = new Button();
            button2 = new Button();
            label8 = new Label();
            chooseImage = new OpenFileDialog();
            label1 = new Label();
            date = new DateTimePicker();
            startTime = new DateTimePicker();
            label9 = new Label();
            endTime = new DateTimePicker();
            label10 = new Label();
            initiator = new TextBox();
            label11 = new Label();
            price = new TextBox();
            label12 = new Label();
            prevBtn = new Button();
            nextBtn = new Button();
            exhibitName = new TextBox();
            label13 = new Label();
            exhibitsAutocomplete = new ListBox();
            button5 = new Button();
            exhibitTable = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)image).BeginInit();
            ((System.ComponentModel.ISupportInitialize)exhibitTable).BeginInit();
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
            // title
            // 
            title.Location = new Point(12, 83);
            title.Name = "title";
            title.Size = new Size(214, 23);
            title.TabIndex = 2;
            // 
            // description
            // 
            description.Location = new Point(12, 136);
            description.Multiline = true;
            description.Name = "description";
            description.Size = new Size(214, 56);
            description.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 118);
            label3.Name = "label3";
            label3.Size = new Size(67, 15);
            label3.TabIndex = 3;
            label3.Text = "Description";
            // 
            // categories
            // 
            categories.DropDownStyle = ComboBoxStyle.DropDownList;
            categories.FormattingEnabled = true;
            categories.Location = new Point(238, 218);
            categories.Name = "categories";
            categories.Size = new Size(214, 23);
            categories.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(238, 200);
            label4.Name = "label4";
            label4.Size = new Size(55, 15);
            label4.TabIndex = 6;
            label4.Text = "Category";
            // 
            // location
            // 
            location.Location = new Point(12, 335);
            location.Name = "location";
            location.Size = new Size(214, 23);
            location.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 317);
            label5.Name = "label5";
            label5.Size = new Size(53, 15);
            label5.TabIndex = 7;
            label5.Text = "Location";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(516, 65);
            label6.Name = "label6";
            label6.Size = new Size(98, 15);
            label6.TabIndex = 9;
            label6.Text = "Exhibits in Event";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(242, 256);
            label7.Name = "label7";
            label7.Size = new Size(44, 15);
            label7.TabIndex = 11;
            label7.Text = "Banner";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // image
            // 
            image.BackColor = SystemColors.ControlLight;
            image.Location = new Point(242, 274);
            image.Name = "image";
            image.Size = new Size(210, 102);
            image.SizeMode = PictureBoxSizeMode.Zoom;
            image.TabIndex = 12;
            image.TabStop = false;
            image.Click += onImageClicked;
            // 
            // button1
            // 
            button1.Location = new Point(860, 366);
            button1.Name = "button1";
            button1.Size = new Size(83, 23);
            button1.TabIndex = 13;
            button1.Text = "Submit";
            button1.UseVisualStyleBackColor = true;
            button1.Click += onSubmit;
            // 
            // button2
            // 
            button2.Location = new Point(771, 366);
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
            label8.Location = new Point(238, 379);
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
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 200);
            label1.Name = "label1";
            label1.Size = new Size(31, 15);
            label1.TabIndex = 16;
            label1.Text = "Date";
            // 
            // date
            // 
            date.Format = DateTimePickerFormat.Short;
            date.Location = new Point(12, 218);
            date.Name = "date";
            date.Size = new Size(214, 23);
            date.TabIndex = 17;
            // 
            // startTime
            // 
            startTime.Format = DateTimePickerFormat.Time;
            startTime.Location = new Point(12, 274);
            startTime.Name = "startTime";
            startTime.Size = new Size(96, 23);
            startTime.TabIndex = 19;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 256);
            label9.Name = "label9";
            label9.Size = new Size(61, 15);
            label9.TabIndex = 18;
            label9.Text = "Start Time";
            // 
            // endTime
            // 
            endTime.Format = DateTimePickerFormat.Time;
            endTime.Location = new Point(130, 274);
            endTime.Name = "endTime";
            endTime.Size = new Size(96, 23);
            endTime.TabIndex = 21;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(130, 256);
            label10.Name = "label10";
            label10.Size = new Size(57, 15);
            label10.TabIndex = 20;
            label10.Text = "End Time";
            // 
            // initiator
            // 
            initiator.Location = new Point(238, 85);
            initiator.Name = "initiator";
            initiator.Size = new Size(214, 23);
            initiator.TabIndex = 23;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(238, 67);
            label11.Name = "label11";
            label11.Size = new Size(48, 15);
            label11.TabIndex = 22;
            label11.Text = "Initiator";
            // 
            // price
            // 
            price.Location = new Point(238, 136);
            price.Name = "price";
            price.Size = new Size(214, 23);
            price.TabIndex = 25;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(238, 118);
            label12.Name = "label12";
            label12.Size = new Size(33, 15);
            label12.TabIndex = 24;
            label12.Text = "Price";
            // 
            // prevBtn
            // 
            prevBtn.Location = new Point(242, 301);
            prevBtn.Name = "prevBtn";
            prevBtn.Size = new Size(21, 47);
            prevBtn.TabIndex = 26;
            prevBtn.Text = "<";
            prevBtn.UseVisualStyleBackColor = true;
            prevBtn.Click += onPrevBanner;
            // 
            // nextBtn
            // 
            nextBtn.Location = new Point(431, 301);
            nextBtn.Name = "nextBtn";
            nextBtn.Size = new Size(21, 47);
            nextBtn.TabIndex = 27;
            nextBtn.Text = ">";
            nextBtn.UseVisualStyleBackColor = true;
            nextBtn.Click += onNextBanner;
            // 
            // exhibitName
            // 
            exhibitName.Location = new Point(516, 110);
            exhibitName.Name = "exhibitName";
            exhibitName.Size = new Size(214, 23);
            exhibitName.TabIndex = 29;
            exhibitName.TextChanged += onExhibitNameChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(516, 92);
            label13.Name = "label13";
            label13.Size = new Size(39, 15);
            label13.TabIndex = 28;
            label13.Text = "Name";
            // 
            // exhibitsAutocomplete
            // 
            exhibitsAutocomplete.FormattingEnabled = true;
            exhibitsAutocomplete.ItemHeight = 15;
            exhibitsAutocomplete.Location = new Point(516, 132);
            exhibitsAutocomplete.Name = "exhibitsAutocomplete";
            exhibitsAutocomplete.Size = new Size(214, 94);
            exhibitsAutocomplete.TabIndex = 30;
            exhibitsAutocomplete.KeyDown += onAutocompleteKeyDown;
            // 
            // button5
            // 
            button5.Location = new Point(736, 110);
            button5.Name = "button5";
            button5.Size = new Size(55, 23);
            button5.TabIndex = 31;
            button5.Text = "Add";
            button5.UseVisualStyleBackColor = true;
            button5.Click += onAddExhibit;
            // 
            // exhibitTable
            // 
            exhibitTable.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            exhibitTable.Location = new Point(516, 157);
            exhibitTable.Name = "exhibitTable";
            exhibitTable.Size = new Size(427, 150);
            exhibitTable.TabIndex = 32;
            exhibitTable.CellClick += onExhibitTableCellClicked;
            // 
            // AddEditEventForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(955, 401);
            Controls.Add(exhibitsAutocomplete);
            Controls.Add(exhibitTable);
            Controls.Add(button5);
            Controls.Add(exhibitName);
            Controls.Add(label13);
            Controls.Add(nextBtn);
            Controls.Add(prevBtn);
            Controls.Add(price);
            Controls.Add(label12);
            Controls.Add(initiator);
            Controls.Add(label11);
            Controls.Add(endTime);
            Controls.Add(label10);
            Controls.Add(startTime);
            Controls.Add(label9);
            Controls.Add(date);
            Controls.Add(label1);
            Controls.Add(label8);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(image);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(location);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(categories);
            Controls.Add(description);
            Controls.Add(label3);
            Controls.Add(title);
            Controls.Add(label2);
            Controls.Add(formTitle);
            Name = "AddEditEventForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Add Exhibit";
            ((System.ComponentModel.ISupportInitialize)image).EndInit();
            ((System.ComponentModel.ISupportInitialize)exhibitTable).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label formTitle;
        private Label label2;
        private TextBox title;
        private TextBox description;
        private Label label3;
        private ComboBox categories;
        private Label label4;
        private TextBox location;
        private Label label5;
        private Label label6;
        private Label label7;
        private PictureBox image;
        private Button button1;
        private Button button2;
        private Label label8;
        private OpenFileDialog chooseImage;
        private Label label1;
        private DateTimePicker date;
        private DateTimePicker startTime;
        private Label label9;
        private DateTimePicker endTime;
        private Label label10;
        private TextBox initiator;
        private Label label11;
        private TextBox price;
        private Label label12;
        private Button prevBtn;
        private Button nextBtn;
        private TextBox exhibitName;
        private Label label13;
        private ListBox exhibitsAutocomplete;
        private Button button5;
        private DataGridView exhibitTable;
    }
}