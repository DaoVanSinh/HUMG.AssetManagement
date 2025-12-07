using HUMG.AssetManagement.Data;
using HUMG.AssetManagement.Models;

namespace HUMG.AssetManagement.Views
{
    public partial class AddEditAssetPage : ContentPage
    {
        private readonly AppDbContext _context;
        private int? _assetId;
        private string _selectedImagePath;

        public AddEditAssetPage(int? assetId = null)
        {
            InitializeComponent();
            _context = new AppDbContext();
            _assetId = assetId;

            // Set giá trị mặc định
            StatusPicker.SelectedIndex = 0;
            PurchaseDatePicker.Date = DateTime.Now;

            if (_assetId.HasValue)
            {
                Title = "Sửa Tài sản";
                LoadAssetData();
            }
        }

        private async void LoadAssetData()
        {
            try
            {
                var asset = await _context.Assets.FindAsync(_assetId.Value);
                if (asset != null)
                {
                    AssetCodeEntry.Text = asset.AssetCode;
                    AssetNameEntry.Text = asset.AssetName;
                    CategoryPicker.SelectedItem = asset.Category;
                    PriceEntry.Text = asset.PurchasePrice.ToString();
                    PurchaseDatePicker.Date = asset.PurchaseDate;
                    StatusPicker.SelectedItem = asset.Status;
                    LocationEntry.Text = asset.Location;
                    DepartmentEntry.Text = asset.Department;
                    DescriptionEditor.Text = asset.Description;
                    _selectedImagePath = asset.ImagePath;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể tải dữ liệu: {ex.Message}", "OK");
            }
        }

        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Chọn ảnh tài sản"
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    AssetImage.Source = ImageSource.FromStream(() => stream);
                    _selectedImagePath = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể chọn ảnh: {ex.Message}", "OK");
            }
            try
            {
#if ANDROID
        // Yêu cầu quyền cho Android
        var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Thông báo", "Cần cấp quyền truy cập ảnh!", "OK");
                return;
            }
        }
#endif

                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Chọn ảnh tài sản"
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    AssetImage.Source = ImageSource.FromStream(() => stream);
                    _selectedImagePath = result.FullPath;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể chọn ảnh: {ex.Message}", "OK");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrWhiteSpace(AssetCodeEntry.Text))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập mã tài sản!", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AssetNameEntry.Text))
            {
                await DisplayAlert("Thông báo", "Vui lòng nhập tên tài sản!", "OK");
                return;
            }

            if (CategoryPicker.SelectedIndex == -1)
            {
                await DisplayAlert("Thông báo", "Vui lòng chọn loại tài sản!", "OK");
                return;
            }

            if (!decimal.TryParse(PriceEntry.Text, out decimal price) || price <= 0)
            {
                await DisplayAlert("Thông báo", "Giá mua không hợp lệ!", "OK");
                return;
            }

            try
            {
                int currentUserId = Preferences.Get("current_user_id", 1);

                if (_assetId.HasValue)
                {
                    // Sửa tài sản
                    var asset = await _context.Assets.FindAsync(_assetId.Value);
                    if (asset != null)
                    {
                        asset.AssetCode = AssetCodeEntry.Text;
                        asset.AssetName = AssetNameEntry.Text;
                        asset.Category = CategoryPicker.SelectedItem.ToString();
                        asset.PurchasePrice = price;
                        asset.PurchaseDate = PurchaseDatePicker.Date;
                        asset.Status = StatusPicker.SelectedItem?.ToString() ?? "Đang sử dụng";
                        asset.Location = LocationEntry.Text ?? "";
                        asset.Department = DepartmentEntry.Text ?? "";
                        asset.Description = DescriptionEditor.Text ?? "";
                        asset.ImagePath = _selectedImagePath ?? "";

                        // Lưu lịch sử
                        _context.AssetHistories.Add(new AssetHistory
                        {
                            AssetId = asset.Id,
                            Action = "Sửa",
                            Description = $"Cập nhật thông tin tài sản {asset.AssetName}",
                            ActionDate = DateTime.Now,
                            ActionBy = currentUserId
                        });
                    }
                }
                else
                {
                    // Thêm mới
                    var newAsset = new Asset
                    {
                        AssetCode = AssetCodeEntry.Text,
                        AssetName = AssetNameEntry.Text,
                        Category = CategoryPicker.SelectedItem.ToString(),
                        PurchasePrice = price,
                        PurchaseDate = PurchaseDatePicker.Date,
                        Status = StatusPicker.SelectedItem?.ToString() ?? "Đang sử dụng",
                        Location = LocationEntry.Text ?? "",
                        Department = DepartmentEntry.Text ?? "",
                        Description = DescriptionEditor.Text ?? "",
                        ImagePath = _selectedImagePath ?? "",
                        CreatedDate = DateTime.Now,
                        CreatedBy = currentUserId
                    };

                    _context.Assets.Add(newAsset);
                    await _context.SaveChangesAsync();

                    // Lưu lịch sử
                    _context.AssetHistories.Add(new AssetHistory
                    {
                        AssetId = newAsset.Id,
                        Action = "Thêm mới",
                        Description = $"Thêm mới tài sản {newAsset.AssetName}",
                        ActionDate = DateTime.Now,
                        ActionBy = currentUserId
                    });
                }

                await _context.SaveChangesAsync();
                await DisplayAlert("Thành công", "Lưu tài sản thành công!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Lỗi", $"Không thể lưu tài sản: {ex.Message}", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Xác nhận",
                "Bạn có chắc muốn hủy? Dữ liệu chưa lưu sẽ mất.", "Có", "Không");

            if (confirm)
            {
                await Navigation.PopAsync();
            }
        }
    }
}