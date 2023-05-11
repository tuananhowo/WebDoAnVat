using System.Data.Entity;

namespace Web.Models
{
    public partial class webdoanvat : DbContext
    {
        public webdoanvat()
            : base("name=webdoanvat")
        {
        }

        public virtual DbSet<DANHMUC> DANHMUCs { get; set; }
        public virtual DbSet<GIOHANG> GIOHANGs { get; set; }
        public virtual DbSet<SANPHAM> SANPHAMs { get; set; }
        public virtual DbSet<TAIKHOAN> TAIKHOANs { get; set; }
        public virtual DbSet<THANHTOAN> THANHTOANs { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DANHMUC>()
                .HasMany(e => e.SANPHAMs)
                .WithRequired(e => e.DANHMUC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GIOHANG>()
                .Property(e => e.SoTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SANPHAM>()
                .Property(e => e.GiaTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<SANPHAM>()
                .HasMany(e => e.GIOHANGs)
                .WithRequired(e => e.SANPHAM)
                .HasForeignKey(e => new { e.ID_SP, e.ID_DanhMuc })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAIKHOAN>()
                .HasMany(e => e.GIOHANGs)
                .WithRequired(e => e.TAIKHOAN)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TAIKHOAN>()
                .HasMany(e => e.THANHTOANs)
                .WithRequired(e => e.TAIKHOAN)
                .WillCascadeOnDelete(false);
        }
    }
}
