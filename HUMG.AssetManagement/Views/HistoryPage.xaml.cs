using HUMG.AssetManagement.Data;
using HUMG.AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace HUMG.AssetManagement.Views
{
    public partial class HistoryPage : ContentPage
    {
        private readonly AppDbContext _context;
        private ObservableCollection<HistoryViewModel> _allHistories;
        private ObservableCollection<HistoryViewModel> _filteredHistories;
        private string _currentActionFilter = "All";
        private string _currentTimeFilter = "All";

        public ObservableCollection<HistoryViewModel> FilteredHistories
        {
            get => _filteredHistories;
            set
            {
                _filteredHistories = value;
                OnPropertyChanged();
            }
        }

        public HistoryPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _allHistories = new ObservableCollection<HistoryViewModel>();
            _filteredHistories = new ObservableCollection<HistoryViewModel>();
            BindingContext = this;
            LoadHistories();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadHistories();
        }

        private async void LoadHistories()
        {
            try
            {
                var histories = await _context.AssetHistories
                    .OrderByDescending(h => h.ActionDate)
                    .ToListAsync();

                var users = await _context.Users.ToListAsync();
                var assets = await _context.Assets.ToListAsync();

                _allHistories.Clear();
                foreach (var history in histories)
                {
                    var user = users.FirstOrDefault(u => u.Id == history.ActionBy);
                    var asset = assets.FirstOrDefault(a => a.Id == history.AssetId);

                    _allHistories.Add(new HistoryViewModel
                    {
                        Id = history.Id,
                        AssetId = history.AssetId,
                        Action = history.Action,
                        ActionTitle = GetActionTitle(history.Action, asset?.AssetName),
                        Description = history.Description,
                        ActionDate = history.ActionDate,
                        ActionBy = history.ActionBy,
                        ActionByName = user?.FullName ?? "Unknown",
                        ActionIcon = GetActionIcon(history.Action),
                        ActionColor = GetActionColor(history.Action),
                        HasAsset = asset != null
                    });
                }

                // Cập nhật thống kê
                UpdateStatistics();

                // Áp dụng bộ lọc
                ApplyFilters();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
            }
        }

        private void UpdateStatistics()
        {
            TotalHistoryLabel.Text = _allHistories.Count.ToString();

            var today = DateTime.Today;
            var todayCount = _allHistories.Count(h => h.ActionDate.Date == today);
            TodayHistoryLabel.Text = todayCount.ToString();

            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
            var weekCount = _allHistories.Count(h => h.ActionDate.Date >= startOfWeek);
            ThisWeekHistoryLabel.Text = weekCount.ToString();
        }

        private void OnFilterActionClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentActionFilter = button.Text switch
            {
                "Tất cả" => "All",
                "Thêm mới" => "Thêm mới",
                "Sửa" => "Sửa",
                "Xóa" => "Xóa",
                _ => "All"
            };

            // Reset màu các nút
            AllActionBtn.BackgroundColor = Color.FromArgb("#F5F5F5");
            AllActionBtn.TextColor = Color.FromArgb("#666666");
            AddActionBtn.BackgroundColor = Color.FromArgb("#E8F5E9");
            AddActionBtn.TextColor = Color.FromArgb("#4CAF50");
            EditActionBtn.BackgroundColor = Color.FromArgb("#E3F2FD");
            EditActionBtn.TextColor = Color.FromArgb("#2196F3");
            DeleteActionBtn.BackgroundColor = Color.FromArgb("#FFEBEE");
            DeleteActionBtn.TextColor = Color.FromArgb("#F44336");

            // Highlight nút được chọn
            if (button == AllActionBtn)
            {
                button.BackgroundColor = Color.FromArgb("#0066CC");
                button.TextColor = Colors.White;
            }
            else if (button == AddActionBtn)
            {
                button.BackgroundColor = Color.FromArgb("#4CAF50");
                button.TextColor = Colors.White;
            }
            else if (button == EditActionBtn)
            {
                button.BackgroundColor = Color.FromArgb("#2196F3");
                button.TextColor = Colors.White;
            }
            else if (button == DeleteActionBtn)
            {
                button.BackgroundColor = Color.FromArgb("#F44336");
                button.TextColor = Colors.White;
            }

            ApplyFilters();
        }

        private void OnFilterTimeClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            _currentTimeFilter = button.Text;

            // Reset màu các nút
            AllTimeBtn.BackgroundColor = Color.FromArgb("#F5F5F5");
            AllTimeBtn.TextColor = Color.FromArgb("#666666");
            TodayBtn.BackgroundColor = Color.FromArgb("#F5F5F5");
            TodayBtn.TextColor = Color.FromArgb("#666666");
            WeekBtn.BackgroundColor = Color.FromArgb("#F5F5F5");
            WeekBtn.TextColor = Color.FromArgb("#666666");
            MonthBtn.BackgroundColor = Color.FromArgb("#F5F5F5");
            MonthBtn.TextColor = Color.FromArgb("#666666");

            // Highlight nút được chọn
            button.BackgroundColor = Color.FromArgb("#0066CC");
            button.TextColor = Colors.White;

            ApplyFilters();
        }

        private void ApplyFilters()
        {
            IEnumerable<HistoryViewModel> filtered = _allHistories;

            // Lọc theo hành động
            if (_currentActionFilter != "All")
            {
                filtered = filtered.Where(h => h.Action == _currentActionFilter);
            }

            // Lọc theo thời gian
            var now = DateTime.Now;
            filtered = _currentTimeFilter switch
            {
                "Hôm nay" => filtered.Where(h => h.ActionDate.Date == now.Date),
                "Tuần này" => filtered.Where(h => h.ActionDate >= now.AddDays(-(int)now.DayOfWeek)),
                "Tháng này" => filtered.Where(h => h.ActionDate.Year == now.Year && h.ActionDate.Month == now.Month),
                _ => filtered
            };

            FilteredHistories.Clear();
            foreach (var item in filtered)
            {
                FilteredHistories.Add(item);
            }
        }

        private async void OnViewAssetClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button?.CommandParameter is int assetId)
            {
                var asset = await _context.Assets.FindAsync(assetId);
                if (asset != null)
                {
                    await Navigation.PushAsync(new AssetDetailPage(assetId));
                }
                else
                {
                    await DisplayAlert("Thông báo", "Tài sản đã bị xóa", "OK");
                }
            }
        }

        private async void OnRefreshing(object sender, EventArgs e)
        {
            LoadHistories();
            RefreshViewControl.IsRefreshing = false;
        }

        private string GetActionTitle(string action, string assetName)
        {
            return action switch
            {
                "Thêm mới" => $"Thêm mới tài sản: {assetName}",
                "Sửa" => $"Cập nhật tài sản: {assetName}",
                "Xóa" => $"Xóa tài sản: {assetName}",
                "Bàn giao" => $"Bàn giao tài sản: {assetName}",
                _ => action
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
    public class HistoryViewModel
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ActionTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public int ActionBy { get; set; }
        public string ActionByName { get; set; } = string.Empty;
        public string ActionIcon { get; set; } = string.Empty;
        public Color ActionColor { get; set; }
        public bool HasAsset { get; set; }
    }
}