using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Generator_Coordinate
{
    public partial class MainForm : Form
    {
        private string inputFilePath;
        private string outputDirectory;

        #region KHỞI TẠO ỨNG DỤNG VÀ CÀI ĐẶT SỰ KIỆN BÀN PHÍM

        public MainForm()
        {
            InitializeComponent();
            ResetApplicationState(); // Khởi tạo trạng thái ứng dụng
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 300;
            toolTip.ReshowDelay = 100;
            toolTip.ShowAlways = false;
            //Kiểm tra bản cập nhật mới ứng dụng khi chạy
            UpdateManager.CheckForUpdates("Generator Coordinate.exe", new[]
            {
                "http://107.125.221.79:8888/update/Coordinate/",
                //"http://107.126.41.111:8888/update/Coordinate/",              
            });
        }

        private void ResetApplicationState()
        {
            // Đặt lại đường dẫn tệp đầu vào và các trường liên quan
            inputFilePath = string.Empty;
            txtFilePath.Text = string.Empty;
            txtDefectName.Text = string.Empty;
            txtDefectName.Enabled = chkMode.Checked;
            lblDefectName.Enabled = chkMode.Checked;
            btnGenerateFiles.Enabled = false;

            // Đặt lại DataGridView
            dataGridViewPreview.Rows.Clear();
            UpdateDataGridViewColumns(chkMode.Checked);

            // Đặt lại thư mục đầu ra
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string baseOutputPath = @"D:\Non_Documents";
            outputDirectory = $@"{baseOutputPath}\FAB_{currentDate}";

            // Kiểm tra và tạo thư mục gốc D:\Non_Documents nếu chưa tồn tại
            try
            {
                if (!Directory.Exists(baseOutputPath))
                {
                    Directory.CreateDirectory(baseOutputPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tạo thư mục {baseOutputPath}: {ex.Message}. Chuyển sang thư mục Desktop.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"FAB_{currentDate}");
            }

            // Kiểm tra quyền ghi vào thư mục gốc
            if (!CanWriteToDirectory(baseOutputPath))
            {
                MessageBox.Show($"Không có quyền ghi vào thư mục {baseOutputPath}. Chuyển sang thư mục Desktop.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"FAB_{currentDate}");
            }

            // Cập nhật nhãn đường dẫn đầu ra
            lblOutputPath.Text = $"Đường dẫn thư mục đầu ra: {outputDirectory}";
            lblOutputPath.Visible = false;

            // Áp dụng hiệu ứng hình ảnh
            InitializeHoverEffect();

            // Đảm bảo KeyPreview được bật
            this.KeyPreview = true;

            // Gắn lại sự kiện KeyDown cho DataGridView
            dataGridViewPreview.KeyDown -= DataGridViewPreview_KeyDown; // Ngăn đăng ký nhiều lần
            dataGridViewPreview.KeyDown += DataGridViewPreview_KeyDown;
            UpdateCellCountLabel();
        }

        private void DataGridViewPreview_KeyDown(object sender, KeyEventArgs e)
        {
            // Xử lý phím tắt Ctrl+V
            if (e.Control && e.KeyCode == Keys.V)
            {
                HandlePaste();
                e.Handled = true;
            }
        }
        #endregion

        #region XỬ LÝ DỮ LIỆU PASTE VÀ KIỂM TRA QUYỀN GHI
        private void HandlePaste()
        {
            try
            {
                string clipboardText = Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(clipboardText))
                {
                    MessageBox.Show("Clipboard rỗng hoặc không chứa dữ liệu văn bản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnGenerateFiles.Enabled = false; // Vô hiệu hóa nút nếu clipboard rỗng
                    return;
                }

                string[] lines = clipboardText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                bool hasValidData = false;

                if (chkMode.Checked) // Chế độ SPOT
                {
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split('\t');
                        if (parts.Length != 3 && parts.Length != 7) continue; // Chấp nhận 3 cột hoặc 7 cột

                        string cellId = parts[0].Trim();
                        if (string.IsNullOrWhiteSpace(cellId)) continue;

                        // Khởi tạo mảng coordinates với 6 phần tử (cho X1,Y1,X2,Y2,X3,Y3), mặc định rỗng
                        string[] coordinates = new string[6] { "", "", "", "", "", "" };
                        int coordCount = parts.Length == 3 ? 2 : 6; // 3 cột -> chỉ lấy X1,Y1; 7 cột -> lấy tất cả

                        for (int i = 0; i < coordCount && i < parts.Length - 1; i++)
                        {
                            coordinates[i] = parts[i + 1].Trim();
                        }

                        // Thêm dòng vào DataGridView với 7 cột
                        dataGridViewPreview.Rows.Add(cellId, coordinates[0], coordinates[1], coordinates[2], coordinates[3], coordinates[4], coordinates[5]);
                        hasValidData = true;
                    }
                }
                else // Chế độ ĐỐM (giữ nguyên mã gốc)
                {
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split('\t');
                        if (parts.Length != 5) continue;

                        string cellId = parts[0].Trim();
                        string sx = parts[1].Trim();
                        string sy = parts[2].Trim();
                        string ex = parts[3].Trim();
                        string ey = parts[4].Trim();

                        if (!double.TryParse(sx, out _) || !double.TryParse(sy, out _) ||
                            !double.TryParse(ex, out _) || !double.TryParse(ey, out _))
                            continue;

                        dataGridViewPreview.Rows.Add(cellId, sx, sy, ex, ey);
                        hasValidData = true;
                    }
                }

                if (!hasValidData)
                {
                    string mode = chkMode.Checked ? "SPOT (định dạng: CELL_ID<TAB>X1<TAB>Y1 hoặc CELL_ID<TAB>X1<TAB>Y1<TAB>X2<TAB>Y2<TAB>X3<TAB>Y3)" : "ĐỐM (định dạng: CELL_ID<TAB>Sx<TAB>Sy<TAB>Ex<TAB>Ey)";
                    MessageBox.Show($"Dữ liệu clipboard không hợp lệ cho chế độ {mode}.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btnGenerateFiles.Enabled = false; // Vô hiệu hóa nút nếu không có dữ liệu hợp lệ
                }
                else
                {
                    btnGenerateFiles.Enabled = true; // Bật nút nếu có dữ liệu hợp lệ
                }

                lblOutputPath.Visible = false;
                UpdateCellCountLabel();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi paste dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGenerateFiles.Enabled = dataGridViewPreview.Rows.Count > 0; // Cập nhật trạng thái nút dựa trên dữ liệu hiện có
            }
        }

        private void UpdateCellCountLabel()
        {
            // Kiểm tra và trừ đi dòng trống cuối cùng nếu có
            int rowCount = dataGridViewPreview.Rows.Count;
            if (rowCount > 0 && dataGridViewPreview.Rows[rowCount - 1].IsNewRow)
            {
                rowCount--;
            }
            lblCellCount.Text = $"Total: {rowCount}";
            lblCellCount.Visible = true;
        }

        //Test đọc/ghi file trong ổ D:\
        private bool CanWriteToDirectory(string path)
        {
            try
            {
                string testFile = Path.Combine(path, "test.txt");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void InitializeHoverEffect()
        {
            chkMode.MouseEnter += (s, e) =>
            {
                chkMode.BackColor = chkMode.Checked
                    ? Color.FromArgb(60, 179, 113)
                    : Color.FromArgb(138, 80, 10);
            };

            chkMode.MouseLeave += (s, e) =>
            {
                chkMode.BackColor = chkMode.Checked
                    ? Color.FromArgb(46, 139, 87)
                    : Color.FromArgb(235, 170, 90);
            };
        }
        #endregion

        #region GIAO DIỆN DATAGRIDVIEW VÀ CẬP NHẬT NỘI DUNG

        private void UpdateDataGridViewColumns(bool isSpotMode)
        {
            dataGridViewPreview.Columns.Clear();
            if (isSpotMode)
            {
                dataGridViewPreview.Columns.Add("CellID", "CELL ID");
                dataGridViewPreview.Columns.Add("X1", "X1");
                dataGridViewPreview.Columns.Add("Y1", "Y1");
                dataGridViewPreview.Columns.Add("X2", "X2");
                dataGridViewPreview.Columns.Add("Y2", "Y2");
                dataGridViewPreview.Columns.Add("X3", "X3");
                dataGridViewPreview.Columns.Add("Y3", "Y3");
            }
            else
            {
                dataGridViewPreview.Columns.Add("CellID", "CELL ID");
                dataGridViewPreview.Columns.Add("DefectSx", "Pos Sx");
                dataGridViewPreview.Columns.Add("DefectSy", "Pos Sy");
                dataGridViewPreview.Columns.Add("DefectEx", "Pos Ex");
                dataGridViewPreview.Columns.Add("DefectEy", "Pos Ey");
            }
            dataGridViewPreview.Refresh();
        }

        private void UpdateDataGridViewContent()
        {
            dataGridViewPreview.Rows.Clear();
            bool hasValidData = false;

            if (string.IsNullOrEmpty(inputFilePath) || !File.Exists(inputFilePath))
            {
                if (dataGridViewPreview.Rows.Count > 0)
                {
                    hasValidData = true;
                }
            }
            else
            {
                try
                {
                    using (StreamReader reader = new StreamReader(inputFilePath))
                    {
                        if (chkMode.Checked) // Chế độ SPOT
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                string[] parts = line.Split('\t');
                                if (parts.Length != 3 && parts.Length != 7) continue; // Chấp nhận 3 cột hoặc 7 cột

                                string cellId = parts[0].Trim();
                                if (string.IsNullOrWhiteSpace(cellId)) continue;

                                // Khởi tạo mảng coordinates với 6 phần tử (cho X1,Y1,X2,Y2,X3,Y3), mặc định rỗng
                                string[] coordinates = new string[6] { "", "", "", "", "", "" };
                                int coordCount = parts.Length == 3 ? 2 : 6; // 3 cột -> chỉ lấy X1,Y1; 7 cột -> lấy tất cả

                                for (int i = 0; i < coordCount && i < parts.Length - 1; i++)
                                {
                                    coordinates[i] = parts[i + 1].Trim();
                                }

                                // Thêm dòng vào DataGridView
                                dataGridViewPreview.Rows.Add(cellId, coordinates[0], coordinates[1], coordinates[2], coordinates[3], coordinates[4], coordinates[5]);
                                hasValidData = true;
                            }
                        }
                        else // Chế độ ĐỐM (giữ nguyên mã gốc)
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                string[] parts = line.Split('\t');
                                if (parts.Length != 5) continue;
                                if (string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]) ||
                                    string.IsNullOrWhiteSpace(parts[2]) || string.IsNullOrWhiteSpace(parts[3]) ||
                                    string.IsNullOrWhiteSpace(parts[4]))
                                    continue;
                                dataGridViewPreview.Rows.Add(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), parts[3].Trim(), parts[4].Trim());
                                hasValidData = true;
                            }
                        }
                    }

                    if (!hasValidData)
                    {
                        string mode = chkMode.Checked ? "SPOT (định dạng: CELL_ID<TAB>X1<TAB>Y1 hoặc CELL_ID<TAB>X1<TAB>Y1<TAB>X2<TAB>Y2<TAB>X3<TAB>Y3)" : "ĐỐM (định dạng: CELL_ID<TAB>Sx<TAB>Sy<TAB>Ex<TAB>Ey)";
                        MessageBox.Show($"File không chứa dữ liệu hợp lệ cho chế độ {mode}.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnGenerateFiles.Enabled = false; // Vô hiệu hóa nút nếu không có dữ liệu hợp lệ
                    }
                    else
                    {
                        btnGenerateFiles.Enabled = true; // Bật nút nếu có dữ liệu hợp lệ
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnGenerateFiles.Enabled = dataGridViewPreview.Rows.Count > 0; // Cập nhật trạng thái nút
                }
            }
            if (!hasValidData)
            {
                btnGenerateFiles.Enabled = false; // Vô hiệu hóa nút nếu không có dữ liệu
            }

            dataGridViewPreview.Refresh();
            UpdateCellCountLabel();
        }

        private void UpdateDataGridView()
        {
            UpdateDataGridViewColumns(chkMode.Checked);
            UpdateDataGridViewContent();
        }

        private void ChkMode_CheckedChanged(object sender, EventArgs e)
        {
            txtDefectName.Enabled = chkMode.Checked;
            lblDefectName.Enabled = chkMode.Checked;
            chkMode.Text = chkMode.Checked ? "SPOT" : "ĐỐM";
            chkMode.BackColor = chkMode.Checked ? Color.FromArgb(46, 139, 87) : Color.FromArgb(235, 170, 90);
            lblOutputPath.Visible = false;
            UpdateDataGridView(); // Cập nhật cả cột và nội dung
        }

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                inputFilePath = openFileDialog.FileName;
                txtFilePath.Text = inputFilePath;
                lblOutputPath.Visible = false;
                UpdateDataGridView();
                btnGenerateFiles.Enabled = dataGridViewPreview.Rows.Count > 0; // Bật nút nếu có dữ liệu
            }
        }

        #endregion

        #region NÚT BẤM TẠO FILE VÀ XỬ LÝ SỰ KIỆN
        private void BtnGenerateFiles_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputFilePath) && dataGridViewPreview.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một file hợp lệ hoặc paste dữ liệu vào DataGridView.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string baseOutputPath = @"D:\Non_Documents";
                if (!Directory.Exists(baseOutputPath))
                {
                    Directory.CreateDirectory(baseOutputPath);
                }

                if (!CanWriteToDirectory(baseOutputPath))
                {
                    MessageBox.Show($"Không có quyền ghi vào thư mục {baseOutputPath}. Chuyển sang thư mục Desktop.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(outputDirectory));
                }

                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                int fileCount = 0;

                if (chkMode.Checked) // Chế độ SPOT
                {
                    if (string.IsNullOrWhiteSpace(txtDefectName.Text))
                    {
                        MessageBox.Show("Vui lòng nhập tên lỗi cho chế độ SPOT.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!IsValidDefectName(txtDefectName.Text))
                    {
                        MessageBox.Show("Tên lỗi không hợp lệ. Vui lòng chỉ sử dụng chữ cái, số, dấu gạch ngang hoặc khoảng trắng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    Dictionary<string, List<(string X, string Y)>> cellData = new Dictionary<string, List<(string, string)>>();

                    if (dataGridViewPreview.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridViewPreview.Rows)
                        {
                            if (row.IsNewRow) continue;
                            string cellId = row.Cells["CellID"].Value?.ToString().Trim();
                            if (string.IsNullOrWhiteSpace(cellId)) continue;

                            // Xử lý từng cặp tọa độ (X1,Y1), (X2,Y2), (X3,Y3)
                            for (int i = 0; i < 3; i++)
                            {
                                string x = row.Cells[1 + i * 2].Value?.ToString().Trim();
                                string y = row.Cells[2 + i * 2].Value?.ToString().Trim();

                                // Bỏ qua nếu X hoặc Y rỗng hoặc bằng 0
                                if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y)) continue;
                                if (!double.TryParse(x, out double xValue) || !double.TryParse(y, out double yValue)) continue;
                                if (xValue == 0 || yValue == 0) continue;

                                if (!cellData.ContainsKey(cellId))
                                {
                                    cellData[cellId] = new List<(string, string)>();
                                }
                                cellData[cellId].Add((x, y));
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(inputFilePath))
                    {
                        using (StreamReader reader = new StreamReader(inputFilePath))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                string[] parts = line.Split('\t');
                                if (parts.Length != 3 && parts.Length != 7) continue; // Chấp nhận 3 cột hoặc 7 cột

                                string cellId = parts[0].Trim();
                                if (string.IsNullOrWhiteSpace(cellId)) continue;

                                // Xử lý các cặp tọa độ
                                int coordPairs = parts.Length == 3 ? 1 : 3; // 3 cột -> 1 cặp; 7 cột -> 3 cặp
                                for (int i = 0; i < coordPairs; i++)
                                {
                                    string x = parts[1 + i * 2].Trim();
                                    string y = parts[2 + i * 2].Trim();

                                    // Bỏ qua nếu X hoặc Y rỗng hoặc bằng 0
                                    if (string.IsNullOrWhiteSpace(x) || string.IsNullOrWhiteSpace(y)) continue;
                                    if (!double.TryParse(x, out double xValue) || !double.TryParse(y, out double yValue)) continue;
                                    if (xValue == 0 || yValue == 0) continue;

                                    if (!cellData.ContainsKey(cellId))
                                    {
                                        cellData[cellId] = new List<(string, string)>();
                                    }
                                    cellData[cellId].Add((x, y));
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng chọn một file hợp lệ hoặc paste dữ liệu vào DataGridView.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (cellData.Count == 0)
                    {
                        MessageBox.Show("Không có dữ liệu hợp lệ để tạo tệp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        btnGenerateFiles.Enabled = false; // Vô hiệu hóa nút nếu không có dữ liệu hợp lệ
                        return;
                    }

                    foreach (var cell in cellData)
                    {
                        string cellId = cell.Key;
                        var coordinates = cell.Value;

                        string filePath = Path.Combine(outputDirectory, $"{cellId}.txt");
                        using (StreamWriter writer = new StreamWriter(filePath, false, new UTF8Encoding(false)))
                        {
                            writer.WriteLine("[DETAIL_RESULT]");
                            writer.WriteLine($"DETAIL_RESULT_COUNT={coordinates.Count}");
                            for (int i = 0; i < coordinates.Count; i++)
                            {
                                writer.WriteLine($"DETAIL_RESULT_NAME_{i + 1}={txtDefectName.Text}");
                                writer.WriteLine($"DETAIL_RESULT_XY_{i + 1}={coordinates[i].X},{coordinates[i].Y}");
                            }
                        }
                        fileCount++;
                    }
                }
                else // Chế độ ĐỐM (giữ nguyên mã gốc)
                {
                    Dictionary<string, List<(string Sx, string Sy, string Ex, string Ey)>> cellData = new Dictionary<string, List<(string, string, string, string)>>();

                    if (dataGridViewPreview.Rows.Count > 0)
                    {
                        foreach (DataGridViewRow row in dataGridViewPreview.Rows)
                        {
                            if (row.IsNewRow) continue;
                            string cellId = row.Cells["CellID"].Value?.ToString().Trim();
                            string sx = row.Cells["DefectSx"].Value?.ToString().Trim();
                            string sy = row.Cells["DefectSy"].Value?.ToString().Trim();
                            string ex = row.Cells["DefectEx"].Value?.ToString().Trim();
                            string ey = row.Cells["DefectEy"].Value?.ToString().Trim();

                            if (string.IsNullOrWhiteSpace(cellId) || string.IsNullOrWhiteSpace(sx) ||
                                string.IsNullOrWhiteSpace(sy) || string.IsNullOrWhiteSpace(ex) ||
                                string.IsNullOrWhiteSpace(ey))
                                continue;

                            if (!double.TryParse(sx, out _) || !double.TryParse(sy, out _) ||
                                !double.TryParse(ex, out _) || !double.TryParse(ey, out _))
                                continue;

                            if (!cellData.ContainsKey(cellId))
                            {
                                cellData[cellId] = new List<(string, string, string, string)>();
                            }
                            cellData[cellId].Add((sx, sy, ex, ey));
                        }
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(inputFilePath))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                string[] parts = line.Split('\t');
                                if (parts.Length != 5) continue;
                                string cellId = parts[0].Trim();
                                string sx = parts[1].Trim();
                                string sy = parts[2].Trim();
                                string ex = parts[3].Trim();
                                string ey = parts[4].Trim();
                                if (!double.TryParse(sx, out _) || !double.TryParse(sy, out _) ||
                                    !double.TryParse(ex, out _) || !double.TryParse(ey, out _))
                                    continue;
                                if (!cellData.ContainsKey(cellId))
                                {
                                    cellData[cellId] = new List<(string, string, string, string)>();
                                }
                                cellData[cellId].Add((sx, sy, ex, ey));
                            }
                        }
                    }

                    foreach (var cell in cellData)
                    {
                        string cellId = cell.Key;
                        var defects = cell.Value;

                        string filePath = Path.Combine(outputDirectory, $"{cellId}.txt");
                        using (StreamWriter writer = new StreamWriter(filePath, false, new UTF8Encoding(false)))
                        {
                            writer.WriteLine("[MANUAL_POS_DETAIL]");
                            writer.WriteLine($"DETAIL_DEFECT_COUNT={defects.Count}");
                            for (int i = 0; i < defects.Count; i++)
                            {
                                writer.WriteLine($"DEFECT_{i + 1}_X={defects[i].Sx},{defects[i].Ex}");
                                writer.WriteLine($"DEFECT_{i + 1}_Y={defects[i].Sy},{defects[i].Ey}");
                            }
                        }
                        fileCount++;
                    }
                }

                // Hiển thị đường dẫn đầy đủ của thư mục đầu ra
                lblOutputPath.Text = $"Đường dẫn thư mục đầu ra: {outputDirectory}";
                lblOutputPath.Visible = true;
                lblOutputPath.ForeColor = Color.ForestGreen;
                btnOpenDirectory.Text = $"Mở thư mục {Path.GetFileName(outputDirectory)}";
                MessageBox.Show($"Tạo thành công {fileCount} tệp!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGenerateFiles.Enabled = dataGridViewPreview.Rows.Count > 0; // Cập nhật trạng thái nút
            }
        }
        #endregion

        #region KIỂM TRA TÊN LỖI, MỞ THƯ MỤC
        private bool IsValidDefectName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z0-9\s\-]+$");
        }

        private void BtnOpenDirectory_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(outputDirectory))
            {
                System.Diagnostics.Process.Start("explorer.exe", outputDirectory);
            }
            else
            {
                MessageBox.Show("Thư mục đầu ra chưa tồn tại. Vui lòng tạo file trước.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnInitialize_Click(object sender, EventArgs e)
        {
            ResetApplicationState();
            lblOutputPath.Text = "Ứng dụng đã được khởi tạo lại!";
            lblOutputPath.ForeColor = Color.Magenta;
            lblOutputPath.Visible = true;
            btnOpenDirectory.Text = "Mở thư mục chứa file vừa tạo"; // Đặt lại văn bản nút bấm
        }
        #endregion
    }
}
