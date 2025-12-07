using HUMG.AssetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HUMG.AssetManagement.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly AppDbContext _context;

        public MainPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadDashboardData();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadDashboardData();

            string userName = Preferences.Get("current_user_name", "User");
            WelcomeLabel.Text = $"Xin chào, {userName}";
        }

        private async void LoadDashboardData()
        {
            try
            {
                var assets = await _context.Assets.ToListAsync();

                TotalAssetsLabel.Text = assets.Count.ToString();
                InUseAssetsLabel.Text = assets.Count(a => a.Status == "Đang sử dụng").ToString();
                DamagedAssetsLabel.Text = assets.Count(a => a.Status == "Hỏng").ToString();

                var totalValue = assets.Sum(a => a.PurchasePrice);
                TotalValueLabel.Text = $"{totalValue:N0}đ";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
            }
        }

        private async void OnAssetListTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssetListPage());
        }

        private async void OnAddAssetTapped(object sender, EventArgs e)
        {
            // Chuyển đến trang thêm tài sản
            await Navigation.PushAsync(new AddEditAssetPage());
        }

        private async void OnReportTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportPage());
        }

        private async void OnHistoryTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HistoryPage());
        }

        private async void OnLogoutTapped(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Xác nhận",
                "Bạn có chắc muốn đăng xuất?", "Có", "Không");

            if (confirm)
            {
                Preferences.Clear();
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
        private async void OnChangePasswordTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ChangePasswordPage());
        }
    }
}