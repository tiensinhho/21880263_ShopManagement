using _21880263.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _21880263.DTO
{
    public class Image_DTO
    {
        public int ProductId { get; set; }

        public BitmapImage? ProductImage { get; set; }

        public virtual Product Product { get; set; } = null!;

    }
}
