using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models
{
    [Table("Banner")]
    public partial class Banner
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Required]
        [StringLength(100000)]
        public string LinkBanner { get; set; }
        public int ThuTu { get; set; }
    }
}