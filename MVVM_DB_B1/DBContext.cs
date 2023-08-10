using Microsoft.EntityFrameworkCore;
using MVVM_DB_B1.Data.ExelModels;
using MVVM_DB_B1.Data.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_DB_B1
{
    public class DBContext : DbContext
    {
        public DbSet<FileName> FileNames { get; set; } = null!;
        public DbSet<Class> Classes { get; set; }
        public DbSet<BalanceSheetDB> BalanceSheets { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=localSQliteDb.db");
        }
    }
}
