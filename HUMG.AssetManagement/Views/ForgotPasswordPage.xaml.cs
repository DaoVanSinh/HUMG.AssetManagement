using HUMG.AssetManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace HUMG.AssetManagement.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        private readonly AppDbContext _context;

        public ForgotPasswordPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }

        private async void OnResetPasswordClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string newPassword = NewPasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Validation
            if (string.IsNullOrWhiteSpace(username))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập tên đăng nhập!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập email!", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Thông báo", "Email không hợp lệ!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập mật khẩu mới!", "OK");
                return;
            }

            if (newPassword.Length < 6)
            {
                await DisplayAlert("Thông báo", "Mật khẩu phải có ít nhất 6 ký tự!", "OK");
                return;
            }

            if (newPassword != confirmPassword)
            {
                await DisplayAlert("Thông báo", "Mật khẩu xác nhận không khớp!", "OK");
                return;
            }

            try
            {
                // Tìm user theo username và email
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == username && u.Email == email);

                if (user == null)
                {
                    await DisplayAlert("Lỗi",
                        "Không tìm thấy tài khoản với tên đăng nhập và email này!",
                        "OK");
                    return;
                }

                // Cập nhật mật khẩu mới
                user.Password = newPassword; // Trong thực tế nên mã hóa mật khẩu
                await _context.SaveChangesAsync();

                await DisplayAlert("Thành công",
                    "Đặt lại mật khẩu thành công!\nBạn có thể đăng nhập bằng mật khẩu mới.",
                    "OK");

                // Quay lại trang đăng nhập
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Có lỗi xảy ra: {ex.Message}", "OK");
            }
        }

        private async void OnBackToLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}