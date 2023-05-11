namespace Web.Models
{
    public class BieuDo
    {
        public int TinhTrangBD { get; set; }
        public decimal? SoTien { get; set; }
        public string Ten
        {
            get
            {
                string k = "";
                if (TinhTrangBD == 0)
                {
                    k = "Chờ đặt hàng";
                }
                if (TinhTrangBD == 1)
                {
                    k = "Đã xác nhận";
                }
                if (TinhTrangBD == 2)
                {
                    k = "Đặt hàng thành công";
                }
                if (TinhTrangBD == 3)
                {
                    k = "Đã thanh toán";
                }
                if (TinhTrangBD == 4)
                {
                    k = "Hủy đơn";
                }
                return k;
            }
            set
            {
                this.Ten = value;
            }
        }
    }
}