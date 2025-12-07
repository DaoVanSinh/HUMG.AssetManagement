using HUMG.AssetManagement.Data;
using HUMG.AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace HUMG.AssetManagement.Views
{
    public partial class AssetDetailPage : ContentPage
    {
        private readonly AppDbContext _context;
        private readonly int _assetId;
        private Asset _currentAsset;

        public AssetDetailPage(int assetId)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _assetId = assetId;
            LoadAssetDetail();
        }

        private async void LoadAssetDetail()
        {
            try
            {
                _currentAsset = await _context.Assets.FindAsync(_assetId);

                if (_currentAsset == null)
                {
                    await DisplayAlert("Lỗi", "Không tìm thấy tài sản!", "OK");
                    await Navigation.PopAsync();
                    return;
                }

                // Hiển thị thông tin
                AssetNameLabel.Text = _currentAsset.AssetName;
                AssetCodeLabel.Text = $"Mã: {_currentAsset.AssetCode}";
                StatusLabel.Text = _currentAsset.Status;
                StatusBorder.BackgroundColor = GetStatusColor(_currentAsset.Status);
                PriceLabel.Text = $"{_currentAsset.PurchasePrice:N0}đ";
                CategoryLabel.Text = _currentAsset.Category;
                LocationLabel.Text = _currentAsset.Location;
                DepartmentLabel.Text = _currentAsset.Department;
                PurchaseDateLabel.Text = _currentAsset.PurchaseDate.ToString("dd/MM/yyyy");
                DescriptionLabel.Text = string.IsNullOrWhiteSpace(_currentAsset.Description)
                    ? "Chưa có mô tả"
                    : _currentAsset.Description;

                // Tạo QR Code
                GenerateQRCode(_currentAsset.AssetCode);

                // Load lịch sử
                await LoadHistory();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
            }
        }

        private async Task LoadHistory()
        {
            try
            {
                var histories = await _context.AssetHistories
                    .Where(h => h.AssetId == _assetId)
                    .OrderByDescending(h => h.ActionDate)
                    .Take(10)
                    .ToListAsync();

                HistoryCountLabel.Text = $"({histories.Count})";

                var historyViewModels = new ObservableCollection<AssetHistoryViewModel>();
                foreach (var history in histories)
                {
                    historyViewModels.Add(new AssetHistoryViewModel
                    {
                        Description = history.Description,
                        ActionDate = history.ActionDate,
                        ActionIcon = GetActionIcon(history.Action),
                        ActionColor = GetActionColor(history.Action)
                    });
                }

                HistoryCollectionView.ItemsSource = historyViewModels;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải lịch sử: {ex.Message}", "OK");
            }
        }

        private void GenerateQRCode(string assetCode)
        {
            try
            {
                // Tạo QR Code đơn giản bằng URL
                string qrUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=200x200&data={Uri.EscapeDataString(assetCode)}";
                QRCodeImage.Source = ImageSource.FromUri(new Uri(qrUrl));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"QR Code Error: {ex.Message}");
            }
        }

        private async void OnEditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditAssetPage(_assetId));
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Xác nhận xóa",
                $"Bạn có chắc muốn xóa tài sản '{_currentAsset.AssetName}'?",
                "Xóa", "Hủy");

            if (confirm)
            {
                try
                {
                    // Xóa lịch sử trước
                    var histories = _context.AssetHistories.Where(h => h.AssetId == _assetId);
                    _context.AssetHistories.RemoveRange(histories);

                    // Xóa tài sản
                    _context.Assets.Remove(_currentAsset);
                    await _context.SaveChangesAsync();

                    await DisplayAlert("Thành công", "Đã xóa tài sản!", "OK");
                    await Navigation.PopToRootAsync();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Lỗi", $"Không thể xóa tài sản: {ex.Message}", "OK");
                }
            }
        }

        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "Đang sử dụng" => Color.FromArgb("#4CAF50"),
                "Hỏng" => Color.FromArgb("#F44336"),
                "Thanh lý" => Color.FromArgb("#999999"),
                _ => Color.FromArgb("#0066CC")
            };
        }

        private string GetActionIcon(string action)
        {
            return action switch
            {
                "Thêm mới" => "➕",
                "Sửa" => "✏️",
                "Xóa" => "🗑️",
                "Bàn giao" => "🔄",
                _ => "📝"
            };
        }

        private Color GetActionColor(string action)
        {
            return action switch
            {
                "Thêm mới" => Color.FromArgb("#4CAF50"),
                "Sửa" => Color.FromArgb("#2196F3"),
                "Xóa" => Color.FromArgb("#F44336"),
                "Bàn giao" => Color.FromArgb("#FF9800"),
                _ => Color.FromArgb("#999999")
            };
        }
    }

    // ViewModel cho History
    public class AssetHistoryViewModel
    {
        public string Description { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string ActionIcon { get; set; } = string.Empty;
        public Color ActionColor { get; set; }
    }
}