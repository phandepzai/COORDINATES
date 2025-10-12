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
        }

        private void ResetApplicationState()
        {
            // Đặt lại đường dẫn tệp đầu vào và các trường liên quan
            inputFilePath = string.Empty;
            txtFilePath.Text = string.Empty;
            txtDefectName.Text = string.Empty;
            txtDefectName.Enabled = chkMode.Checked;
            lblDefectName.Enabled = chkMode.Checked;

            // Đặt lại chế độ về ĐỐM
            //chkMode.Checked = false;
            //chkMode.Text = "ĐỐM";
            //chkMode.BackColor = Color.FromArgb(235, 170, 90); // Màu cam

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
                    return;
                }

                // LƯU Ý: Dòng này đã được xóa để cho phép dán nhiều lần
                // dataGridViewPreview.Rows.Clear(); 
                string[] lines = clipboardText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                bool hasValidData = false;

                if (chkMode.Checked) // Chế độ SPOT
                {
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] parts = line.Split(new[] { '=', '\t', ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length != 3) continue; // CELL_ID, X, Y

                        string cellId = parts[0].Trim();
                        if (string.IsNullOrWhiteSpace(cellId)) continue;

                        string posX = parts[1].Trim();
                        string posY = parts[2].Trim();

                        if (!double.TryParse(posX, out _) || !double.TryParse(posY, out _)) continue;

                        // Thêm dòng mới thay vì xóa dòng cũ
                        dataGridViewPreview.Rows.Add(cellId, $"{posX},{posY}");
                        hasValidData = true;
                    }
                }
                else // Chế độ ĐỐM
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

                        // Thêm dòng mới thay vì xóa dòng cũ
                        dataGridViewPreview.Rows.Add(cellId, sx, sy, ex, ey);
                        hasValidData = true;
                    }
                }

                if (!hasValidData)
                {
                    string mode = chkMode.Checked ? "SPOT (định dạng: CELL_ID=X,Y hoặc CELL_ID<TAB>X,Y hoặc CELL_ID,X,Y)" : "ĐỐM (định dạng: CELL_ID<TAB>Sx<TAB>Sy<TAB>Ex<TAB>Ey)";
                    MessageBox.Show($"Dữ liệu clipboard không hợp lệ cho chế độ {mode}.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                lblOutputPath.Visible = false;
                UpdateCellCountLabel(); // Gọi phương thức cập nhật số lượng
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi paste dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            lblCellCount.Text = $"Q'ty: {rowCount}";
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
                dataGridViewPreview.Columns.Add("CoordinatesXY", "TỌA ĐỘ X,Y");
            }
            else
            {
                dataGridViewPreview.Columns.Add("CellID", "CELL ID");
                dataGridViewPreview.Columns.Add("DefectSx", "TỌA ĐỘ Sx");
                dataGridViewPreview.Columns.Add("DefectSy", "TỌA ĐỘ Sy");
                dataGridViewPreview.Columns.Add("DefectEx", "TỌA ĐỘ Ex");
                dataGridViewPreview.Columns.Add("DefectEy", "TỌA ĐỘ Ey");
            }
            dataGridViewPreview.Refresh(); // Làm mới giao diện DataGridView
        }

        private void UpdateDataGridViewContent()
        {
            dataGridViewPreview.Rows.Clear();
            bool hasValidData = false;

            if (string.IsNullOrEmpty(inputFilePath) || !File.Exists(inputFilePath))
            {
                // Nếu không có tệp đầu vào, kiểm tra dữ liệu trong DataGridView
                if (dataGridViewPreview.Rows.Count > 0)
                {
                    hasValidData = true; // Dữ liệu đã có từ paste hoặc nguồn khác
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
                                string[] parts = line.Split('=');
                                if (parts.Length != 2) continue;
                                string cellId = parts[0].Trim();
                                if (string.IsNullOrWhiteSpace(cellId)) continue;
                                string[] coordinates = parts[1].Split(',');
                                if (coordinates.Length != 2 || string.IsNullOrWhiteSpace(coordinates[0]) || string.IsNullOrWhiteSpace(coordinates[1]))
                                    continue;
                                if (!double.TryParse(coordinates[0].Trim(), out _) || !double.TryParse(coordinates[1].Trim(), out _))
                                    continue;
                                string coordinatesXY = $"{coordinates[0].Trim()},{coordinates[1].Trim()}";
                                dataGridViewPreview.Rows.Add(cellId, coordinatesXY);
                                hasValidData = true;
                            }
                        }
                        else // Chế độ ĐỐM
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
                        string mode = chkMode.Checked ? "SPOT (định dạng: CELL_ID=X,Y)" : "ĐỐM (định dạng: CELL_ID\\tSx\\tSy\\tEx\\tEy)";
                        MessageBox.Show($"File không chứa dữ liệu hợp lệ cho chế độ {mode}.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            dataGridViewPreview.Refresh(); // Làm mới giao diện DataGridView
            UpdateCellCountLabel(); //Cập nhật số lượng cell trong DataGridView
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
            }
        }

        #endregion

        #region NÚT BẤM TẠO FILE VÀ XỬ LÝ SỰ KIỆN

        //Phương thức xử lý sự kiện khi người dùng nhấn nút "Tạo File"
        private void BtnGenerateFiles_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputFilePath) && dataGridViewPreview.Rows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một file hợp lệ hoặc paste dữ liệu vào DataGridView.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkMode.Checked && string.IsNullOrWhiteSpace(txtDefectName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên lỗi cho chế độ SPOT.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (chkMode.Checked && !IsValidDefectName(txtDefectName.Text))
            {
                MessageBox.Show("Tên lỗi không hợp lệ. Vui lòng chỉ sử dụng chữ cái, số, dấu gạch ngang hoặc khoảng trắng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Kiểm tra và tạo thư mục gốc D:\Non_Documents nếu chưa tồn tại
                string baseOutputPath = @"D:\Non_Documents";
                if (!Directory.Exists(baseOutputPath))
                {
                    Directory.CreateDirectory(baseOutputPath);
                }

                // Kiểm tra quyền ghi vào thư mục gốc
                if (!CanWriteToDirectory(baseOutputPath))
                {
                    MessageBox.Show($"Không có quyền ghi vào thư mục {baseOutputPath}. Chuyển sang thư mục Desktop.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Path.GetFileName(outputDirectory));
                }

                // Tạo thư mục đầu ra cụ thể (FAB_YYYYMMDD) nếu chưa tồn tại
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                int fileCount = 0;

                if (dataGridViewPreview.Rows.Count > 0)
                {
                    if (chkMode.Checked) // Chế độ SPOT
                    {
                        Dictionary<string, List<(string X, string Y)>> cellData = new Dictionary<string, List<(string, string)>>();

                        foreach (DataGridViewRow row in dataGridViewPreview.Rows)
                        {
                            if (row.IsNewRow) continue;
                            string cellId = row.Cells["CellID"].Value?.ToString().Trim();
                            string coordinatesXY = row.Cells["CoordinatesXY"].Value?.ToString().Trim();

                            if (string.IsNullOrWhiteSpace(cellId) || string.IsNullOrWhiteSpace(coordinatesXY)) continue;

                            string[] coordinates = coordinatesXY.Split(',');
                            if (coordinates.Length != 2) continue;

                            string posX = coordinates[0].Trim();
                            string posY = coordinates[1].Trim();

                            if (!double.TryParse(posX, out _) || !double.TryParse(posY, out _)) continue;

                            if (!cellData.ContainsKey(cellId))
                            {
                                cellData[cellId] = new List<(string, string)>();
                            }
                            cellData[cellId].Add((posX, posY));
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
                                    writer.WriteLine($"DETAIL_RESULT_NAME={txtDefectName.Text}");
                                    writer.WriteLine($"DETAIL_RESULT_XY_{i + 1}={coordinates[i].X},{coordinates[i].Y}");
                                }
                            }
                            fileCount++;
                        }
                    }
                    else // Chế độ ĐỐM
                    {
                        Dictionary<string, List<(string Sx, string Sy, string Ex, string Ey)>> cellData = new Dictionary<string, List<(string, string, string, string)>>();

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
                }
                else
                {
                    if (chkMode.Checked) // Chế độ SPOT
                    {
                        Dictionary<string, List<(string X, string Y)>> cellData = new Dictionary<string, List<(string, string)>>();

                        using (StreamReader reader = new StreamReader(inputFilePath))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                if (string.IsNullOrWhiteSpace(line)) continue;
                                string[] parts = line.Split('=');
                                if (parts.Length != 2) continue;
                                string cellId = parts[0].Trim();
                                if (string.IsNullOrWhiteSpace(cellId)) continue;
                                string[] coordinates = parts[1].Split(',');
                                if (coordinates.Length != 2) continue;
                                string posX = coordinates[0].Trim();
                                string posY = coordinates[1].Trim();
                                if (!double.TryParse(posX, out _) || !double.TryParse(posY, out _)) continue;
                                if (!cellData.ContainsKey(cellId))
                                {
                                    cellData[cellId] = new List<(string, string)>();
                                }
                                cellData[cellId].Add((posX, posY));
                            }
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
                                    writer.WriteLine($"DETAIL_RESULT_NAME={txtDefectName.Text}");
                                    writer.WriteLine($"DETAIL_RESULT_XY_{i + 1}={coordinates[i].X},{coordinates[i].Y}");
                                }
                            }
                            fileCount++;
                        }
                    }
                    else // Chế độ ĐỐM
                    {
                        Dictionary<string, List<(string Sx, string Sy, string Ex, string Ey)>> cellData = new Dictionary<string, List<(string, string, string, string)>>();

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
                }

                // Sau khi tạo file thành công
                lblOutputPath.Visible = true;
                lblOutputPath.ForeColor = Color.ForestGreen;
                btnOpenDirectory.Text = $"Mở thư mục {Path.GetFileName(outputDirectory)}"; // Cập nhật văn bản nút bấm
                MessageBox.Show($"Tạo thành công {fileCount} tệp!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            lblOutputPath.ForeColor = Color.Peru;
            lblOutputPath.Visible = true;
            btnOpenDirectory.Text = "Mở thư mục chứa file vừa tạo"; // Đặt lại văn bản nút bấm
        }
        #endregion
    }
}
