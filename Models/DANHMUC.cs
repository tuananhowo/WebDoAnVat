namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DANHMUC")]
    public partial class DANHMUC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DANHMUC()
        {
            SANPHAMs = new HashSet<SANPHAM>();
        }

        [Key]
        public int ID_DanhMuc { get; set; }

        [Required]
        [StringLength(100000)]
        public string TenDanhMuc { get; set; }

        [StringLength(100000)]
        public string GhiChu { get; set; }

        [StringLength(100000)]
        public string AnhBiaDM { get; set; }
        public int KieuDM { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SANPHAM> SANPHAMs { get; set; }
    }
}
