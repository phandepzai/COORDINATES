namespace Generator_Coordinate
{
    partial class MainForm
    {
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnGenerateFiles;
        private System.Windows.Forms.Button btnOpenDirectory;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.DataGridView dataGridViewPreview;
        private System.Windows.Forms.Label lblOutputPath;
        private System.Windows.Forms.CheckBox chkMode;
        private System.Windows.Forms.TextBox txtDefectName;
        private System.Windows.Forms.Label lblDefectName;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button btnInitialize; // Nút Khởi tạo lại mới
        private System.ComponentModel.IContainer components;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkMode = new System.Windows.Forms.CheckBox();
            this.dataGridViewPreview = new System.Windows.Forms.DataGridView();
            this.txtDefectName = new System.Windows.Forms.TextBox();
            this.lblDefectName = new System.Windows.Forms.Label();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnGenerateFiles = new System.Windows.Forms.Button();
            this.btnOpenDirectory = new System.Windows.Forms.Button();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblOutputPath = new System.Windows.Forms.Label();
            this.btnInitialize = new System.Windows.Forms.Button();
            this.labelAuthor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // chkMode
            // 
            this.chkMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.chkMode.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMode.FlatAppearance.BorderSize = 0;
            this.chkMode.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.chkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMode.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMode.ForeColor = System.Drawing.Color.White;
            this.chkMode.Location = new System.Drawing.Point(9, 84);
            this.chkMode.Name = "chkMode";
            this.chkMode.Size = new System.Drawing.Size(76, 36);
            this.chkMode.TabIndex = 7;
            this.chkMode.Text = "ĐỐM";
            this.chkMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkMode, "BẤM VÀO ĐÂY ĐỂ CHUYỂN ĐỔI:\r\n-Chế độ ĐỐM (Sx | Ex | Sy | Ey) (dành cho các lỗi đốm" +
        ")\r\n-Chế độ SPOT (Position X, Y) (dành cho các lỗi B-spot,White Spot)");
            this.chkMode.UseVisualStyleBackColor = false;
            this.chkMode.CheckedChanged += new System.EventHandler(this.chkMode_CheckedChanged);
            // 
            // dataGridViewPreview
            // 
            this.dataGridViewPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPreview.Location = new System.Drawing.Point(9, 150);
            this.dataGridViewPreview.Name = "dataGridViewPreview";
            this.dataGridViewPreview.ReadOnly = true;
            this.dataGridViewPreview.Size = new System.Drawing.Size(531, 394);
            this.dataGridViewPreview.TabIndex = 5;
            this.toolTip.SetToolTip(this.dataGridViewPreview, "@Nông Văn Phấn\r\nFAB Inspection Part(SDV)");
            // 
            // txtDefectName
            // 
            this.txtDefectName.Enabled = false;
            this.txtDefectName.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDefectName.Location = new System.Drawing.Point(97, 84);
            this.txtDefectName.Multiline = true;
            this.txtDefectName.Name = "txtDefectName";
            this.txtDefectName.Size = new System.Drawing.Size(145, 36);
            this.txtDefectName.TabIndex = 8;
            this.toolTip.SetToolTip(this.txtDefectName, "Nhập tên lỗi đối với các lỗi như:\r\n- B-Spot\r\n- White Spot\r\n\r\n(Chú ý ghi đúng chín" +
        "h tả từng ký tự thì chương trình mới\r\n nhận diện được file.)");
            // 
            // lblDefectName
            // 
            this.lblDefectName.AutoSize = true;
            this.lblDefectName.Location = new System.Drawing.Point(149, 68);
            this.lblDefectName.Name = "lblDefectName";
            this.lblDefectName.Size = new System.Drawing.Size(64, 13);
            this.lblDefectName.TabIndex = 9;
            this.lblDefectName.Text = "Nhập tên lỗi";
            this.toolTip.SetToolTip(this.lblDefectName, "Nhập tên lỗi đối với các lỗi như:\r\n- B-Spot\r\n- White Spot\r\n\r\n(Chú ý ghi đúng chín" +
        "h tả từng ký tự thì chương trình mới\r\n nhận diện được file.)");
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectFile.Location = new System.Drawing.Point(9, 18);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(82, 39);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Chọn tệp";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath.Location = new System.Drawing.Point(97, 18);
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(325, 39);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnGenerateFiles
            // 
            this.btnGenerateFiles.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateFiles.Location = new System.Drawing.Point(289, 78);
            this.btnGenerateFiles.Name = "btnGenerateFiles";
            this.btnGenerateFiles.Size = new System.Drawing.Size(100, 46);
            this.btnGenerateFiles.TabIndex = 2;
            this.btnGenerateFiles.Text = "TẠO FILE";
            this.btnGenerateFiles.UseVisualStyleBackColor = true;
            this.btnGenerateFiles.Click += new System.EventHandler(this.btnGenerateFiles_Click);
            // 
            // btnOpenDirectory
            // 
            this.btnOpenDirectory.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDirectory.Location = new System.Drawing.Point(428, 14);
            this.btnOpenDirectory.Name = "btnOpenDirectory";
            this.btnOpenDirectory.Size = new System.Drawing.Size(116, 46);
            this.btnOpenDirectory.TabIndex = 3;
            this.btnOpenDirectory.Text = "Mở thư mục chứa file vừa tạo";
            this.btnOpenDirectory.UseVisualStyleBackColor = true;
            this.btnOpenDirectory.Click += new System.EventHandler(this.btnOpenDirectory_Click);
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(12, 70);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(0, 13);
            this.lblFilePath.TabIndex = 4;
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.Location = new System.Drawing.Point(12, 127);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(405, 20);
            this.lblOutputPath.TabIndex = 6;
            this.lblOutputPath.Text = "Đường dẫn thư mục đầu ra: ";
            // 
            // btnInitialize
            // 
            this.btnInitialize.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitialize.Location = new System.Drawing.Point(416, 78);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(103, 46);
            this.btnInitialize.TabIndex = 10;
            this.btnInitialize.Text = "Khởi tạo lại";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.BackColor = System.Drawing.Color.Transparent;
            this.labelAuthor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAuthor.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.labelAuthor.Location = new System.Drawing.Point(441, 526);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(92, 13);
            this.labelAuthor.TabIndex = 11;
            this.labelAuthor.Text = "©Nông Văn Phấn";
            this.labelAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAuthor.Click += new System.EventHandler(this.labelAuthor_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 548);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.btnInitialize);
            this.Controls.Add(this.lblDefectName);
            this.Controls.Add(this.txtDefectName);
            this.Controls.Add(this.chkMode);
            this.Controls.Add(this.lblOutputPath);
            this.Controls.Add(this.dataGridViewPreview);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.btnOpenDirectory);
            this.Controls.Add(this.btnGenerateFiles);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnSelectFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generator Coordinate";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label labelAuthor;
    }
}