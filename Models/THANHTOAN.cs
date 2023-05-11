namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("THANHTOAN")]
    public partial class THANHTOAN
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_ThanhToan { get; set; }

        public int TinhTrang { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string PhuongThucTT { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string ThongTinTT { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string BaoMat { get; set; }

        public DateTime NgayTao { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_TK { get; set; }

        public virtual TAIKHOAN TAIKHOAN { get; set; }
    }
}
