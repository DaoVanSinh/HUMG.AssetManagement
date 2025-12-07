using HUMG.AssetManagement.Data;
using Microsoft.EntityFrameworkCore;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace HUMG.AssetManagement.Views
{
    public partial class ReportPage : ContentPage
    {
        private readonly AppDbContext _context;

        public ReportPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadReportData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadReportData();
        }

        private async void LoadReportData()
        {
            try
            {
                var assets = await _context.Assets.ToListAsync();

                // Cập nhật ngày
                ReportDateLabel.Text = $"Cập nhật: {DateTime.Now:dd/MM/yyyy HH:mm}";

                // Tổng quan
                TotalAssetsLabel.Text = assets.Count.ToString();
                var totalValue = assets.Sum(a => a.PurchasePrice);
                TotalValueLabel.Text = $"{totalValue:N0}đ";

                // Thống kê theo trạng thái
                var inUse = assets.Count(a => a.Status == "Đang sử dụng");
                var damaged = assets.Count(a => a.Status == "Hỏng");
                var retired = assets.Count(a => a.Status == "Thanh lý");

                InUseCountLabel.Text = $"{inUse} tài sản";
                DamagedCountLabel.Text = $"{damaged} tài sản";
                RetiredCountLabel.Text = $"{retired} tài sản";

                // Tạo biểu đồ tròn cho trạng thái
                var statusEntries = new List<ChartEntry>
                {
                    new ChartEntry(inUse)
                    {
                        Label = "Đang sử dụng",
                        ValueLabel = inUse.ToString(),
                        Color = SKColor.Parse("#4CAF50")
                    },
                    new ChartEntry(damaged)
                    {
                        Label = "Hỏng",
                        ValueLabel = damaged.ToString(),
                        Color = SKColor.Parse("#F44336")
                    },
                    new ChartEntry(retired)
                    {
                        Label = "Thanh lý",
                        ValueLabel = retired.ToString(),
                        Color = SKColor.Parse("#999999")
                    }
                };

                StatusChart.Chart = new DonutChart
                {
                    Entries = statusEntries,
                    LabelTextSize = 40,
                    BackgroundColor = SKColors.Transparent,
                    HoleRadius = 0.5f
                };

                // Thống kê theo loại tài sản
                var categoryGroups = assets.GroupBy(a => a.Category)
                    .Select(g => new
                    {
                        Category = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                var colors = new[] { "#2196F3", "#FF9800", "#9C27B0", "#00BCD4", "#FFC107", "#E91E63" };
                var categoryEntries = new List<ChartEntry>();

                for (int i = 0; i < categoryGroups.Count; i++)
                {
                    categoryEntries.Add(new ChartEntry(categoryGroups[i].Count)
                    {
                        Label = categoryGroups[i].Category,
                        ValueLabel = categoryGroups[i].Count.ToString(),
                        Color = SKColor.Parse(colors[i % colors.Length])
                    });
                }

                CategoryChart.Chart = new BarChart
                {
                    Entries = categoryEntries,
                    LabelTextSize = 40,
                    ValueLabelTextSize = 40,
                    BackgroundColor = SKColors.Transparent,
                    LabelOrientation = Orientation.Horizontal
                };

                // Thống kê theo phòng ban
                var departmentStats = assets
                    .GroupBy(a => a.Department)
                    .Where(g => !string.IsNullOrWhiteSpace(g.Key))
                    .Select(g => new DepartmentStatViewModel
                    {
                        DepartmentName = g.Key,
                        Count = g.Count(),
                        TotalValue = g.Sum(a => a.PurchasePrice)
                    })
                    .OrderByDescending(d => d.Count)
                    .ToList();

                DepartmentCollectionView.ItemsSource = departmentStats;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
            }
        }

        private async void OnExportPdfClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Thông báo",
                "Chức năng xuất PDF đang được phát triển!\n\nSẽ cập nhật trong phiên bản tiếp theo.",
                "OK");
        }

        private async void OnExportExcelClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Thông báo",
                "Chức năng xuất Excel đang được phát triển!\n\nSẽ cập nhật trong phiên bản tiếp theo.",
                "OK");
        }
    }

    // ViewModel cho thống kê phòng ban
    public class DepartmentStatViewModel
    {
        public string DepartmentName { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalValue { get; set; }
    }
}