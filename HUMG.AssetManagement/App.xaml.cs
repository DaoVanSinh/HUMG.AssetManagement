namespace HUMG.AssetManagement
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Chỉ cần set MainPage, AppShell sẽ tự động hiển thị LoginPage (route đầu tiên)
            MainPage = new AppShell();
        }
    }
}