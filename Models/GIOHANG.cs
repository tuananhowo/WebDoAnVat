namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GIOHANG")]
    public partial class GIOHANG
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int ID_GioHang { get; set; }

        [Column(TypeName = "money")]
        public decimal SoTien { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string DuLieu { get; set; }

        public int TinhTrang { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_SP { get; set; }

        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DanhMuc { get; set; }

        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TK { get; set; }
        public int SoLuong { get; set; }
        public int ID_ThanhToan { get; set; }
        public DateTime? NgayDatHang { get; set; }
        public virtual SANPHAM SANPHAM { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}
