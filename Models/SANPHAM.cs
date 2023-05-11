namespace Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;

    [Table("SANPHAM")]
    public partial class SANPHAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SANPHAM()
        {
            GIOHANGs = new HashSet<GIOHANG>();
        }
        public static string ConvertToThousand64(object number, string code = "vi-VN")
        {
            string str;
            if (number != null)
                str = string.Format(CultureInfo.GetCultureInfo(code), "{0:N0}", Convert.ToInt64(number));
            else
                str = "0";
            return str;
        }
        public static string ConvertToThousand64_From_Float(object number, string code = "vi-VN")
        {
            var numStr = number.ToString();
            if (number != null && numStr.Contains(","))
            {
                var decNum = numStr.Split(',')[1];
                var str = ConvertToThousand64(numStr.Split(',')[0], code);
                return str + "," + decNum;
            }
            return ConvertToThousand64(number, code);
        }
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int ID_SP { get; set; }

        [Required]
        [StringLength(100000)]
        public string TenSanPham { get; set; }

        [Required]
        [StringLength(100000)]
        public string AnhBia { get; set; }

        [StringLength(100000)]
        public string Anh1 { get; set; }

        [StringLength(100000)]
        public string Anh2 { get; set; }

        [Column(TypeName = "money")]
        public decimal GiaTien { get; set; }

        public double? GiamGia { get; set; }

        [Required]
        [StringLength(100000)]
        public string TinhTrang { get; set; }

        public int SoLuong { get; set; }

        [StringLength(100000)]
        public string MoTa { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID_DanhMuc { get; set; }

        public virtual DANHMUC DANHMUC { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GIOHANG> GIOHANGs { get; set; }
    }
}
