using _21880263.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.DTO
{
    public class Category_DTO : ICloneable
    {
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
