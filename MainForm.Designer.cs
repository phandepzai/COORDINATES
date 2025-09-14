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
        private System.Windows.Forms.Label labelAuthor;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label lblCellCount;

        #region PHẦN THIẾT GIAO GIAO DIỆN ỨNG DỤNG
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
            this.lblCellCount = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // chkMode
            // 
            this.chkMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(170)))), ((int)(((byte)(90)))));
            this.chkMode.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMode.FlatAppearance.BorderSize = 0;
            this.chkMode.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.chkMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMode.Font = new System.Drawing.Font("Segoe UI Semibold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMode.ForeColor = System.Drawing.Color.White;
            this.chkMode.Location = new System.Drawing.Point(17, 82);
            this.chkMode.Name = "chkMode";
            this.chkMode.Size = new System.Drawing.Size(76, 36);
            this.chkMode.TabIndex = 7;
            this.chkMode.Text = "ĐỐM";
            this.chkMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.chkMode, "BẤM VÀO ĐÂY ĐỂ CHUYỂN ĐỔI:\r\n- Chế độ ĐỐM (Sx | Sy | Ex | Ey) (dành cho các lỗi Đố" +
        "m dường dọc, Đốm Panel, Đốm Spin,v.v..)\r\n- Chế độ SPOT (Position X, Y) (dành cho" +
        " các lỗi B-spot,White Spot)");
            this.chkMode.UseVisualStyleBackColor = false;
            this.chkMode.CheckedChanged += new System.EventHandler(this.chkMode_CheckedChanged);
            // 
            // dataGridViewPreview
            // 
            this.dataGridViewPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewPreview.BackgroundColor = System.Drawing.SystemColors.Info;
            this.dataGridViewPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridViewPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPreview.Location = new System.Drawing.Point(9, 150);
            this.dataGridViewPreview.Name = "dataGridViewPreview";
            this.dataGridViewPreview.ReadOnly = true;
            this.dataGridViewPreview.Size = new System.Drawing.Size(531, 394);
            this.dataGridViewPreview.TabIndex = 5;
            this.toolTip.SetToolTip(this.dataGridViewPreview, "Bấm chuột trái vào đây rồi dán list cell từ clipboard để thêm cell cần tạo file t" +
        "ọa độ");
            // 
            // txtDefectName
            // 
            this.txtDefectName.Enabled = false;
            this.txtDefectName.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDefectName.Location = new System.Drawing.Point(104, 82);
            this.txtDefectName.Multiline = true;
            this.txtDefectName.Name = "txtDefectName";
            this.txtDefectName.Size = new System.Drawing.Size(145, 36);
            this.txtDefectName.TabIndex = 8;
            this.toolTip.SetToolTip(this.txtDefectName, "Nhập tên lỗi đối với các lỗi như:\r\n- B-Spot\r\n- White Spot\r\n\r\n(Lưu ý: Ghi đúng chí" +
        "nh tả từng ký tự thì chương trình mới\r\n nhận diện được file.)");
            // 
            // lblDefectName
            // 
            this.lblDefectName.AutoSize = true;
            this.lblDefectName.Location = new System.Drawing.Point(115, 64);
            this.lblDefectName.Name = "lblDefectName";
            this.lblDefectName.Size = new System.Drawing.Size(99, 13);
            this.lblDefectName.TabIndex = 9;
            this.lblDefectName.Text = "Nhập tên lỗi rework";
            this.toolTip.SetToolTip(this.lblDefectName, "Nhập tên lỗi đối với các lỗi như:\r\n- B-Spot\r\n- White Spot\r\n\r\n(Lưu ý: Ghi đúng chí" +
        "nh tả từng ký tự thì chương trình mới\r\n nhận diện được file.)\r\n");
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectFile.Location = new System.Drawing.Point(16, 15);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(82, 39);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Chọn tệp";
            this.toolTip.SetToolTip(this.btnSelectFile, "Bấm vào đây để nhập dữ liệu từ file .txt lưu trong máy.");
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilePath.Location = new System.Drawing.Point(104, 15);
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(311, 39);
            this.txtFilePath.TabIndex = 1;
            // 
            // btnGenerateFiles
            // 
            this.btnGenerateFiles.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateFiles.Location = new System.Drawing.Point(315, 82);
            this.btnGenerateFiles.Name = "btnGenerateFiles";
            this.btnGenerateFiles.Size = new System.Drawing.Size(100, 36);
            this.btnGenerateFiles.TabIndex = 2;
            this.btnGenerateFiles.Text = "TẠO FILE";
            this.btnGenerateFiles.UseVisualStyleBackColor = true;
            this.btnGenerateFiles.Click += new System.EventHandler(this.btnGenerateFiles_Click);
            // 
            // btnOpenDirectory
            // 
            this.btnOpenDirectory.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDirectory.Location = new System.Drawing.Point(432, 15);
            this.btnOpenDirectory.Name = "btnOpenDirectory";
            this.btnOpenDirectory.Size = new System.Drawing.Size(103, 39);
            this.btnOpenDirectory.TabIndex = 3;
            this.btnOpenDirectory.Text = "Mở thư mục chứa file vừa tạo";
            this.toolTip.SetToolTip(this.btnOpenDirectory, "Bấm vào đây để mở thư mục chứa file tọa độ vừa tạo.");
            this.btnOpenDirectory.UseVisualStyleBackColor = true;
            this.btnOpenDirectory.Click += new System.EventHandler(this.btnOpenDirectory_Click);
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(12, 68);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(0, 13);
            this.lblFilePath.TabIndex = 4;
            // 
            // lblOutputPath
            // 
            this.lblOutputPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOutputPath.Location = new System.Drawing.Point(12, 127);
            this.lblOutputPath.Name = "lblOutputPath";
            this.lblOutputPath.Size = new System.Drawing.Size(451, 20);
            this.lblOutputPath.TabIndex = 6;
            this.lblOutputPath.Text = "Thư mục file tọa độ:";
            this.lblOutputPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnInitialize
            // 
            this.btnInitialize.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInitialize.Location = new System.Drawing.Point(432, 82);
            this.btnInitialize.Name = "btnInitialize";
            this.btnInitialize.Size = new System.Drawing.Size(103, 36);
            this.btnInitialize.TabIndex = 10;
            this.btnInitialize.Text = "Khởi tạo lại";
            this.btnInitialize.UseVisualStyleBackColor = true;
            this.btnInitialize.Click += new System.EventHandler(this.btnInitialize_Click);
            // 
            // labelAuthor
            // 
            this.labelAuthor.AutoSize = true;
            this.labelAuthor.BackColor = System.Drawing.Color.Transparent;
            this.labelAuthor.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAuthor.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.labelAuthor.Location = new System.Drawing.Point(457, 529);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(79, 11);
            this.labelAuthor.TabIndex = 11;
            this.labelAuthor.Text = "©Nông Văn Phấn";
            this.labelAuthor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCellCount
            // 
            this.lblCellCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCellCount.AutoSize = true;
            this.lblCellCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCellCount.ForeColor = System.Drawing.Color.Red;
            this.lblCellCount.Location = new System.Drawing.Point(482, 129);
            this.lblCellCount.Name = "lblCellCount";
            this.lblCellCount.Size = new System.Drawing.Size(44, 15);
            this.lblCellCount.TabIndex = 12;
            this.lblCellCount.Text = "Q\'ty: 0";
            this.lblCellCount.Visible = false;
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
            this.Controls.Add(this.lblCellCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generator Coordinate";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
    }
}