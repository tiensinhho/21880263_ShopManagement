using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;
using _21880263.DAO;
using System.ComponentModel;
using Image = _21880263.DAO.Image;
using _21880263.DTO;

namespace _21880263.BUS
{
    public class Image_BUS
    {
        public byte[]? ProductImage { get; set; }

        public  BindingList<Image_DTO>? Images { get; set; }

        BindingList<Image>? _images { get; set; }

        public Image_BUS()
        {
            if (_images == null)
            {
                _images = new BindingList<Image>(EShopDbContext.Instance.Images.ToList());
                if (Images == null) { Images = new BindingList<Image_DTO>(); }
                foreach (var item in _images)
                {
                    var imageDTO = new Image_DTO();
                    imageDTO.ProductId = item.ProductId;
                    imageDTO.Product = item.Product;
                    if (item.ProductImage == null) continue;
                    imageDTO.ProductImage = ConvertBytesToBitmapImage(item.ProductImage);
                    Images.Add(imageDTO);
                }
            }
        }
        public BitmapImage ConvertFiletoBitMap(string Path)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(Path);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            return image;
        }

        public byte[] ConvertBitmapImageToBytes(BitmapImage bitmapImage)
        {
            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }
            return data;
        }

        public BitmapImage ConvertBytesToBitmapImage(byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                ms.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = ms;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
