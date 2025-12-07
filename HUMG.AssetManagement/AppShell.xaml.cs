namespace HUMG.AssetManagement
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Đăng ký routes
            Routing.RegisterRoute("assetlist", typeof(Views.AssetListPage));
            Routing.RegisterRoute("assetdetail", typeof(Views.AssetDetailPage));
            Routing.RegisterRoute("addeditasset", typeof(Views.AddEditAssetPage));
            Routing.RegisterRoute("report", typeof(Views.ReportPage));
            Routing.RegisterRoute("history", typeof(Views.HistoryPage)); 


        }
    }
}