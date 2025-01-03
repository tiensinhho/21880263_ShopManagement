using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using _21880263.DTO;
using _21880263.DAO;
using AutoMapper;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;
using Image = _21880263.DAO.Image;
using LiveCharts.Configurations;


namespace _21880263.BUS
{
    public class Product_BUS
    {
        public BindingList<Product_DTO>? Products { get; set; }
        private BindingList<Product>? _products {  get; set; } 
        public BindingList<Category_DTO>? Categories { get; set; }
        private BindingList<Category>? _categories { get; set; }
        public BindingList<Brand_DTO>? Brands { get; set; }
        private BindingList<Brand>? _brands { get; set; }
        public Product_BUS(int pageNumber, int pageSize)
        {
            // Tạo cấu hình ánh xạ
            _loadData(pageNumber, pageSize);
        }
        public Product_BUS()
        {
            // Tạo cấu hình ánh xạ
            _loadData(0, 0);
        }
        private void _loadData(int pageNumber, int pageSize)
        {
            IMapper mapper = _configMapper();
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;

            //Load dữ liệu
            if (pageSize <= 0 && pageNumber <= 0)
            {
                if (_products == null) _products = new BindingList<Product>(EShopDbContext.Instance.Products.Include(c => c.Category).Include(b => b.Brand).Include(x => x.Image).Where(s => s.ProductIsdeleted == false).ToList());
                
            }
            else
            {
                if (_products == null) _products = new BindingList<Product>(EShopDbContext.Instance.Products.Include(c => c.Category).Include(b => b.Brand).Include(x => x.Image).Where(s => s.ProductIsdeleted == false).OrderBy(p => p.ProductId).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList());
            }
            if (_categories == null) _categories = new BindingList<Category>(EShopDbContext.Instance.Categories.Where(x => x.CategoryIsdeleted == false).ToList());
            if (_brands == null) _brands = new BindingList<Brand>(EShopDbContext.Instance.Brands.Where(x => x.BrandIsdeleted == false).ToList());

            // Sử dụng mapper để ánh xạ từ Entity sang DTO
            Products = mapper.Map<BindingList<Product>, BindingList<Product_DTO>>(_products);
            Categories = mapper.Map<BindingList<Category>, BindingList<Category_DTO>>(_categories);
            Brands = mapper.Map<BindingList<Brand>, BindingList<Brand_DTO>>(_brands);


            //// Sử dụng mapper để ánh xạ từ DTO sang Entity
            _products = mapper.Map<BindingList<Product_DTO>, BindingList<Product>>(Products);
            _categories = mapper.Map<BindingList<Category_DTO>, BindingList<Category>>(Categories);
            _brands = mapper.Map<BindingList<Brand_DTO>, BindingList<Brand>>(Brands);
        }

        /// <summary>
        /// Cấu hình mapper
        /// </summary>
        /// <returns></returns>
        private static IMapper _configMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product_DTO, Product>();
                cfg.CreateMap<Product, Product_DTO>().ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => src.Image == null ? null : ConvertBytesToBitmapImage(src.Image.ProductImage)));
                cfg.CreateMap<Category, Category_DTO>();
                cfg.CreateMap<Category_DTO, Category>();
                cfg.CreateMap<Brand, Brand_DTO>();
                cfg.CreateMap<Brand_DTO, Brand>();
                cfg.CreateMap<Image, Image_DTO>().ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => ConvertBytesToBitmapImage(src.ProductImage)));
                cfg.CreateMap<Image_DTO, Image>().ForMember(dest => dest.ProductImage, opt => opt.MapFrom(src => ConvertBitmapImageToBytes(src.ProductImage)));
            });
            // Tạo mapper từ cấu hình
            IMapper mapper = config.CreateMapper();
            return mapper;
        }

        /// <summary>
        /// Thêm sản phẩm
        /// </summary>
        /// <param name="product"></param>
        public void Add(Product_DTO product)
        {
            IMapper mapper = _configMapper();
            var newproduct = mapper.Map<Product_DTO, Product>(product);
            EShopDbContext.Instance.Products.Add(newproduct);
            if (product.ProductImage != null)
            {
                var _productImage = new Image();
                _productImage.ProductId = product.ProductId;
                _productImage.ProductImage = ConvertBitmapImageToBytes(product.ProductImage);
                EShopDbContext.Instance.Images.Add(_productImage);
            }
            EShopDbContext.Instance.SaveChanges();
            if(_categories != null) product.Category = _categories.FirstOrDefault(x => x.CategoryId == product.CategoryId);
            if (_brands != null) product.Brand = _brands.FirstOrDefault(x => x.BrandId == product.BrandId);
            if (Products != null) Products.Add(product);
        }

        /// <summary>
        /// Sửa sản phẩm
        /// </summary>
        /// <param name="product"></param>
        public void Edit(Product_DTO product)
        {
            var oldproduct = EShopDbContext.Instance.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (oldproduct == null) return;
            if (string.IsNullOrEmpty(product.ProductName)) return;
            oldproduct.ProductName = product.ProductName;
            oldproduct.ProductImportprice = product.ProductImportprice;
            oldproduct.ProductSellprice = product.ProductSellprice;
            oldproduct.BrandId = product.BrandId;
            oldproduct.CategoryId = product.CategoryId;
            var _productImage = EShopDbContext.Instance.Images.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (product.ProductImage != null)
            {
                if (_productImage != null) { _productImage.ProductImage = ConvertBitmapImageToBytes(product.ProductImage); }
                else 
                { 
                    _productImage = new Image();
                    _productImage.ProductId = product.ProductId;
                    _productImage.ProductImage = ConvertBitmapImageToBytes(product.ProductImage);
                    EShopDbContext.Instance.Images.Add(_productImage);
                }
            }
            
            EShopDbContext.Instance.SaveChanges();
            if (_categories != null) product.Category = _categories.FirstOrDefault(x => x.CategoryId == product.CategoryId);
            if (_brands != null) product.Brand = _brands.FirstOrDefault(x => x.BrandId == product.BrandId);
            if (Products != null)
            {
                var _product = Products.FirstOrDefault(x => x.ProductId == product.ProductId);
                if (_product != null) Products[Products.IndexOf(_product)] = product;
            }
        }
        /// <summary>
        /// Xoá sản phẩm
        /// </summary>
        /// <param name="product"></param>
        public void Remove(Product_DTO product)
        {
            var oldproduct = EShopDbContext.Instance.Products.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (oldproduct == null) return;
            oldproduct.ProductIsdeleted = true;
            var __image = EShopDbContext.Instance.Images.FirstOrDefault(x => x.ProductId == product.ProductId);
            if (__image != null) EShopDbContext.Instance.Images.Remove(__image);
            EShopDbContext.Instance.SaveChanges();
            if (Products != null) Products.Remove(product);
        }
        public void AddCategory(Category_DTO categoryDTO)
        {
            IMapper mapper = _configMapper();
            var newcategory = mapper.Map<Category_DTO, Category>(categoryDTO);
            EShopDbContext.Instance.Categories.Add(newcategory);
            EShopDbContext.Instance.SaveChanges();
            if (Categories != null) Categories.Add(categoryDTO);
        }

        public void RemoveCategory(Category_DTO categoryDTO)
        {
            var oldcategory = EShopDbContext.Instance.Categories.FirstOrDefault(x => x.CategoryId == categoryDTO.CategoryId);
            if (oldcategory == null) return;
            oldcategory.CategoryIsdeleted = true;
            EShopDbContext.Instance.SaveChanges();
            if (Categories != null)
            {
                var _category = Categories.FirstOrDefault(x => x.CategoryId == categoryDTO.CategoryId);
                if (_category != null) Categories.Remove(_category);
            }
        }

        public void EditCategory(Category_DTO categoryDTO)
        {
            IMapper mapper = _configMapper();
            Category newcategory = mapper.Map<Category_DTO, Category>(categoryDTO);
            var oldcategory = EShopDbContext.Instance.Categories.FirstOrDefault(x => x.CategoryId == categoryDTO.CategoryId);
            if (oldcategory == null) return;
            oldcategory.CategoryName = newcategory.CategoryName;
            oldcategory.CategoryDescription = newcategory.CategoryDescription;
            EShopDbContext.Instance.SaveChanges();
            if (Categories != null)
            {
                var _category = Categories.FirstOrDefault(x => x.CategoryId == categoryDTO.CategoryId);
                if (_category != null) Categories[Categories.IndexOf(_category)] = categoryDTO;
            }
        }

        public void AddBrand(Brand_DTO brandDTO)
        {
            IMapper mapper = _configMapper();
            var newbrand = mapper.Map<Brand_DTO, Brand>(brandDTO);
            EShopDbContext.Instance.Brands.Add(newbrand);
            EShopDbContext.Instance.SaveChanges();
            if (Brands != null)  Brands.Add(brandDTO);

        }

        public void RemoveBrand(Brand_DTO brandDTO)
        {
            IMapper mapper = _configMapper();
            var newbrand = mapper.Map<Brand_DTO, Brand>(brandDTO);
            var oldbrand = EShopDbContext.Instance.Brands.FirstOrDefault(x => x.BrandId == brandDTO.BrandId);
            if (oldbrand == null) return;
            oldbrand.BrandIsdeleted = true;
            EShopDbContext.Instance.SaveChanges();
            if (Brands != null) Brands.Remove(brandDTO);
        }

        public void EditBrand(Brand_DTO brandDTO)
        {
            IMapper mapper = _configMapper();
            var newbrand = mapper.Map<Brand_DTO, Brand>(brandDTO);
            var oldbrand = EShopDbContext.Instance.Brands.FirstOrDefault(x => x.BrandId == brandDTO.BrandId);
            oldbrand = newbrand;
            EShopDbContext.Instance.SaveChanges();
            if (Brands != null)
            {
                var _brand = Brands.FirstOrDefault(x => x.BrandId == brandDTO.BrandId);
                if (_brand != null) { Brands[Brands.IndexOf(_brand)] = brandDTO; }
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

        static byte[]? ConvertBitmapImageToBytes(BitmapImage? bitmapImage)
        {
            if (bitmapImage == null) return null;
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

        static BitmapImage? ConvertBytesToBitmapImage(byte[]? bytes)
        {
            if (bytes == null) return null;
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

        public int TotalPage(int PageSize)
        {
            int total = 1;
            if (PageSize != 0) total = EShopDbContext.Instance.Products.Count()%PageSize == 0 ? (int)EShopDbContext.Instance.Products.Count()/PageSize : (int)EShopDbContext.Instance.Products.Count()/PageSize+1;
            return total;
        }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }
    }
}
