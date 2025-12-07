using HUMG.AssetManagement.Data;
using HUMG.AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace HUMG.AssetManagement.Views
{
    public partial class AssetListPage : ContentPage
    {
        private readonly AppDbContext _context;
        private ObservableCollection<AssetViewModel> _allAssets;
        private ObservableCollection<AssetViewModel> _filteredAssets;
        private string _currentFilter = "All";

        public ObservableCollection<AssetViewModel> FilteredAssets
        {
            get => _filteredAssets;
            set
            {
                _filteredAssets = value;
                OnPropertyChanged();
            }
        }

        public AssetListPage()
        {
            InitializeComponent();
            _context = new AppDbContext();
            _allAssets = new ObservableCollection<AssetViewModel>();
            _filteredAssets = new ObservableCollection<AssetViewModel>();
            BindingContext = this;
            LoadAssets();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadAssets();
        }

        private async void LoadAssets()
        {
            try
            {
                var assets = await _context.Assets.OrderByDescending(a => a.CreatedDate).ToListAsync();

                _allAssets.Clear();
                foreach (var asset in assets)
                {
                    _allAssets.Add(new AssetViewModel
                    {
                        Id = asset.Id,
                        AssetCode = asset.AssetCode,
                        AssetName = asset.AssetName,
                        Category = asset.Category,
                        CategoryIcon = GetCategoryIcon(asset.Category),
                        Location = asset.Location,
                        Status = asset.Status,
                        StatusColor = GetStatusColor(asset.Status),
                        PurchasePrice = asset.PurchasePrice,
                        Department = asset.Department,
                        Description = asset.Description,
                        PurchaseDate = asset.PurchaseDate
                    });
                }

                ApplyFilter(_currentFilter);
            }
            catch (Exception ex)
            {
                await DisplayAlert("L?i", $"Không th? t?i d? li?u: {ex.Message}", "OK");
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(searchText))
            {
                ApplyFilter(_currentFilter);
            }
            else
            {
                var filtered = _allAssets.Where(a =>
                    a.AssetName.ToLower().Contains(searchText) ||
                    a.AssetCode.ToLower().Contains(searchText) ||
                    a.Location.ToLower().Contains(searchText)
                ).ToList();

                FilteredAssets.Clear();
                foreach (var item in filtered)
                {
                    FilteredAssets.Add(item);
                }
            }
        }

        private void OnFilterClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            string filter = button.Text switch
            {
                "T?t c?" => "All",
                "?ang dùng" => "?ang s? d?ng",
                "H?ng" => "H?ng",
                "Thanh lý" => "Thanh lý",
                _ => "All"
            };

            // Reset màu các nút
            AllFilterBtn.BackgroundColor = Color.FromArgb("#F5F5F5");
            AllFilterBtn.TextColor = Color.FromArgb("#999999");
            InUseFilterBtn.BackgroundColor = Color.FromArgb("#E8F5E9");
            InUseFilterBtn.TextColor = Color.FromArgb("#4CAF50");
            DamagedFilterBtn.BackgroundColor = Color.FromArgb("#FFEBEE");
            DamagedFilterBtn.TextColor = Color.FromArgb("#F44336");
            RetiredFilterBtn.BackgroundColor = Color.FromArgb("#F5F5F5");
            RetiredFilterBtn.TextColor = Color.FromArgb("#999999");

            // Highlight nút ???c ch?n
            if (button == AllFilterBtn)
            {
                button.BackgroundColor = Color.FromArgb("#0066CC");
                button.TextColor = Colors.White;
            }
            else if (button == InUseFilterBtn)
            {
                button.BackgroundColor = Color.FromArgb("#4CAF50");
                button.TextColor = Colors.White;
            }
            else if (button == DamagedFilterBtn)
            {
                button.BackgroundColor = Color.FromArgb("#F44336");
                button.TextColor = Colors.White;
            }
            else if (button == RetiredFilterBtn)
            {
                button.BackgroundColor = Color.FromArgb("#999999");
                button.TextColor = Colors.White;
            }

            _currentFilter = filter;
            ApplyFilter(filter);
        }

        private void ApplyFilter(string filter)
        {
            IEnumerable<AssetViewModel> filtered;

            if (filter == "All")
            {
                filtered = _allAssets;
            }
            else
            {
                filtered = _allAssets.Where(a => a.Status == filter);
            }

            FilteredAssets.Clear();
            foreach (var item in filtered)
            {
                FilteredAssets.Add(item);
            }
        }

        private async void OnAssetSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is AssetViewModel selectedAsset)
            {
                // B? ch?n item
                ((CollectionView)sender).SelectedItem = null;

                // Chuy?n sang trang chi ti?t
                await Navigation.PushAsync(new AssetDetailPage(selectedAsset.Id));
            }
        }

        private async void OnAddAssetClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddEditAssetPage());
        }

        private string GetCategoryIcon(string category)
        {
            return category switch
            {
                "Máy tính" => "??",
                "Thi?t b? v?n phòng" => "???",
                "N?i th?t" => "??",
                "Xe c?" => "??",
                "Thi?t b? y t?" => "??",
                "Thi?t b? thí nghi?m" => "??",
                _ => "??"
            };
        }

        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "?ang s? d?ng" => Color.FromArgb("#4CAF50"),
                "H?ng" => Color.FromArgb("#F44336"),
                "Thanh lý" => Color.FromArgb("#999999"),
                _ => Color.FromArgb("#0066CC")
            };
        }
    }

    // ViewModel cho Asset
    public class AssetViewModel
    {
        public int Id { get; set; }
        public string AssetCode { get; set; } = string.Empty;
        public string AssetName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryIcon { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Color StatusColor { get; set; }
        public decimal PurchasePrice { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
    }
}