using HUMG.AssetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HUMG.AssetManagement.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly AppDbContext _context;

        public LoginPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            InitializeDatabase();
        }

        private async void InitializeDatabase()
        {
            try
            {
                await _context.Database.EnsureCreatedAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể khởi tạo database: {ex.Message}", "OK");
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập đầy đủ thông tin!", "OK");
                return;
            }

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    if (RememberCheckBox.IsChecked)
                    {
                        Preferences.Set("saved_username", username);
                        Preferences.Set("is_logged_in", true);
                    }

                    Preferences.Set("current_user_id", user.Id);
                    Preferences.Set("current_user_name", user.FullName);
                    Preferences.Set("current_user_role", user.Role);

                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await DisplayAlert("Đăng nhập thất bại",
                        "Tài khoản hoặc mật khẩu không chính xác!", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Có lỗi xảy ra: {ex.Message}", "OK");
            }
        }

        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPasswordPage());
        }
    }
}