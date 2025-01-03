using _21880263.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.DTO
{
    public class Brand_DTO: ICloneable
    {
        public int BrandId { get; set; }

        public string? BrandName { get; set; }

        public string? BrandDescription { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
