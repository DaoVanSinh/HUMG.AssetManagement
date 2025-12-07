using HUMG.AssetManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HUMG.AssetManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetHistory> AssetHistories { get; set; }

        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath;

#if ANDROID
    // Đường dẫn cho Android
    dbPath = Path.Combine(FileSystem.AppDataDirectory, "humg_assets.db");
#elif WINDOWS
            // Đường dẫn cho Windows
            dbPath = Path.Combine(FileSystem.AppDataDirectory, "humg_assets.db");
#else
    // Mặc định cho các nền tảng khác
    dbPath = Path.Combine(FileSystem.AppDataDirectory, "humg_assets.db");
#endif

            optionsBuilder.UseSqlite($"Filename={dbPath}");

            // Debug: In ra đường dẫn để kiểm tra
            System.Diagnostics.Debug.WriteLine($"Database path: {dbPath}");
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User mẫu
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "admin123",
                    FullName = "Quản trị viên",
                    Role = "Admin",
                    Email = "admin@humg.edu.vn",
                    PhoneNumber = "",
                    CreatedDate = new DateTime(2024, 1, 1)
                }
            );

            // Assets mẫu
            modelBuilder.Entity<Asset>().HasData(
                new Asset
                {
                    Id = 1,
                    AssetCode = "MT001",
                    AssetName = "Laptop Dell Latitude 5420",
                    Category = "Máy tính",
                    Description = "Laptop dùng cho giảng viên khoa Địa chất",
                    PurchasePrice = 18000000,
                    PurchaseDate = new DateTime(2023, 6, 15),
                    Status = "Đang sử dụng",
                    Location = "Phòng 302 - Nhà A",
                    Department = "Khoa Địa chất",
                    ImagePath = "",
                    CreatedDate = new DateTime(2023, 6, 15),
                    CreatedBy = 1
                },
                new Asset
                {
                    Id = 2,
                    AssetCode = "TB002",
                    AssetName = "Máy chiếu Epson EB-2250U",
                    Category = "Thiết bị văn phòng",
                    Description = "Máy chiếu phòng hội thảo lớn",
                    PurchasePrice = 25000000,
                    PurchaseDate = new DateTime(2023, 8, 20),
                    Status = "Đang sử dụng",
                    Location = "Hội trường B",
                    Department = "Phòng Quản trị",
                    ImagePath = "",
                    CreatedDate = new DateTime(2023, 8, 20),
                    CreatedBy = 1
                },
                new Asset
                {
                    Id = 3,
                    AssetCode = "NT003",
                    AssetName = "Bàn làm việc gỗ Công nghiệp",
                    Category = "Nội thất",
                    Description = "Bàn làm việc cho giảng viên",
                    PurchasePrice = 3500000,
                    PurchaseDate = new DateTime(2023, 3, 10),
                    Status = "Đang sử dụng",
                    Location = "Phòng 205 - Nhà B",
                    Department = "Khoa Mỏ",
                    ImagePath = "",
                    CreatedDate = new DateTime(2023, 3, 10),
                    CreatedBy = 1
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
