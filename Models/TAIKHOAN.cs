namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TAIKHOAN")]
    public partial class TAIKHOAN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TAIKHOAN()
        {
            GIOHANGs = new HashSet<GIOHANG>();
            THANHTOANs = new HashSet<THANHTOAN>();
        }

        [Key]
        public int ID_TK { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDangNhap { get; set; }

        [Required]
        [StringLength(150)]
        public string MatKhau { get; set; }

        [Required]
        [StringLength(120)]
        public string HoTen { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        public DateTime? NgaySinh { get; set; }

        [StringLength(20)]
        public string GioiTinh { get; set; }

        [Required]
        [StringLength(100000)]
        public string DiaChi { get; set; }

        public int TinhTrang { get; set; }

        public int PhanQuyen { get; set; }

        public DateTime? NgayTao { get; set; }

        [StringLength(200)]
        public string HinhAnh { get; set; }
        [StringLength(20)]
        public string SDT { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GIOHANG> GIOHANGs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THANHTOAN> THANHTOANs { get; set; }
    }
}
