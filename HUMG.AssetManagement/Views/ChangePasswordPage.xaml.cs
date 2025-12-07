using HUMG.AssetManagement.Data;
using HUMG.AssetManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace HUMG.AssetManagement.Views
{
    public partial class ChangePasswordPage : ContentPage
    {
        private readonly AppDbContext _context;
        private int _currentUserId;
        private User _currentUser;

        public ChangePasswordPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            LoadCurrentUser();
        }

        private async void LoadCurrentUser()
        {
            try
            {
                _currentUserId = Preferences.Get("current_user_id", 0);

                if (_currentUserId == 0)
                {
                    await DisplayAlert("Lỗi", "Không tìm thấy thông tin người dùng!", "OK");
                    await Navigation.PopAsync();
                    return;
                }

                _currentUser = await _context.Users.FindAsync(_currentUserId);

                if (_currentUser != null)
                {
                    UsernameLabel.Text = _currentUser.Username;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải thông tin: {ex.Message}", "OK");
            }
        }

        private void OnPasswordTextChanged(object sender, TextChangedEventArgs e)
        {
            string password = e.NewTextValue ?? "";

            if (string.IsNullOrWhiteSpace(password))
            {
                PasswordStrengthContainer.IsVisible = false;
                return;
            }

            PasswordStrengthContainer.IsVisible = true;

            // Tính độ mạnh mật khẩu
            int strength = CalculatePasswordStrength(password);

            if (strength < 30)
            {
                PasswordStrengthBar.Progress = 0.3;
                PasswordStrengthBar.ProgressColor = Color.FromArgb("#F44336");
                PasswordStrengthLabel.Text = "Yếu";
                PasswordStrengthLabel.TextColor = Color.FromArgb("#F44336");
            }
            else if (strength < 60)
            {
                PasswordStrengthBar.Progress = 0.6;
                PasswordStrengthBar.ProgressColor = Color.FromArgb("#FF9800");
                PasswordStrengthLabel.Text = "Trung bình";
                PasswordStrengthLabel.TextColor = Color.FromArgb("#FF9800");
            }
            else
            {
                PasswordStrengthBar.Progress = 1.0;
                PasswordStrengthBar.ProgressColor = Color.FromArgb("#4CAF50");
                PasswordStrengthLabel.Text = "Mạnh";
                PasswordStrengthLabel.TextColor = Color.FromArgb("#4CAF50");
            }
        }

        private int CalculatePasswordStrength(string password)
        {
            int strength = 0;

            if (password.Length >= 6) strength += 20;
            if (password.Length >= 8) strength += 10;
            if (password.Length >= 10) strength += 10;

            if (password.Any(char.IsLower)) strength += 15;
            if (password.Any(char.IsUpper)) strength += 15;
            if (password.Any(char.IsDigit)) strength += 15;
            if (password.Any(ch => !char.IsLetterOrDigit(ch))) strength += 15;

            return strength;
        }

        private async void OnChangePasswordClicked(object sender, EventArgs e)
        {
            string currentPassword = CurrentPasswordEntry.Text;
            string newPassword = NewPasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Validation
            if (string.IsNullOrWhiteSpace(currentPassword))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập mật khẩu hiện tại!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập mật khẩu mới!", "OK");
                return;
            }

            if (newPassword.Length < 6)
            {
                await DisplayAlert("Thông báo", "Mật khẩu mới phải có ít nhất 6 ký tự!", "OK");
                return;
            }

            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Thông báo", "Mật khẩu xác nhận không khớp!", "OK");
                return;
            }

            if (currentPassword == newPassword)
            {
                await DisplayAlert("Thông báo", "Mật khẩu mới không được trùng với mật khẩu cũ!", "OK");
                return;
            }

            try
            {
                // Kiểm tra mật khẩu hiện tại
                if (_currentUser.Password != currentPassword)
                {
                    await DisplayAlert("Lỗi", "Mật khẩu hiện tại không đúng!", "OK");
                    return;
                }

                // Cập nhật mật khẩu mới
                _currentUser.Password = newPassword;
                await _context.SaveChangesAsync();

                // Lưu lịch sử (tùy chọn)
                // Có thể thêm bảng lịch sử thay đổi mật khẩu

                await DisplayAlert("Thành công",
                    "Đổi mật khẩu thành công!\nVui lòng đăng nhập lại bằng mật khẩu mới.",
                    "OK");

                // Đăng xuất và quay về trang đăng nhập
                Preferences.Clear();
                await Shell.Current.GoToAsync("//LoginPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Có lỗi xảy ra: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}