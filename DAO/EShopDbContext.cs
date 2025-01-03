using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _21880263.DAO
{
    public class EShopDbContext
    {
        private static EShopContext? instance;
        private static string _connectionString = string.Empty;
        public static EShopContext Instance
        {
            get
            {
                if (instance == null || instance.ConnectionString != _connectionString)
                {
                    instance = new EShopContext(_connectionString);
                }
                return instance;
            }
        }

        public static void SetConnectionString(string ConnectionString) { 
            _connectionString = ConnectionString;
        }
    }
}
